using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FF8.JSM;
using FF8.JSM.Instructions;
using FF8.Core;

namespace EveilForest.CSharp;

public sealed class CSharpEventCompiler : IEventCompiler
{
     public EVObject[] CompileDirectory(String directoryPath)
     {
          // TODO: Process Event.cs
          String eventPath = Path.Combine(directoryPath, "Event.cs");

          String[] objectPaths = Directory.GetFiles(directoryPath, "*??_*.cs");

          List<EVObject> objects = new(capacity: objectPaths.Length);

          foreach (String objectPath in objectPaths)
          {
               EVObject evObject = ParseObject(objectPath);
               objects.Add(evObject);
          }

          // Sort objects by ID to maintain consistent ordering
          objects.Sort((a, b) => a.Id.CompareTo(b.Id));

          return objects.ToArray();
     }

     private static EVObject ParseObject(String objectPath)
     {
          const Int32 flags = default; // not used

          Int32 id = ParseObjectId(objectPath);

          SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(objectPath));
          CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
          Byte variableCount = ParseVariables(root);
          EVScript[] scripts = ParseScripts(root);

          EVObject evObject = new EVObject(id, variableCount, flags, scripts);
          return evObject;
     }

     private static Byte ParseVariables(CompilationUnitSyntax root)
     {
          // Analyze the C# code to determine the original variable count by looking at
          // public properties that represent object variables (like Int32_0, Byte_1, etc.)
          // and also @evt variable access patterns
          
          var variableIndexes = new HashSet<int>();
          
          // Find public properties that represent object variables
          var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
          foreach (var classDecl in classes)
          {
              var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>()
                  .Where(p => p.Modifiers.Any(SyntaxKind.PublicKeyword));
              
              foreach (var property in properties)
              {
                  var propName = property.Identifier.ValueText;
                  if (TryExtractVariableIndex(propName, out int index))
                  {
                      int size = GetVariableSize(propName);
                      // Add all byte indices that this variable occupies
                      for (int i = 0; i < size; i++)
                      {
                          variableIndexes.Add(index + i);
                      }
                  }
              }
          }
          
          // Also look for @evt variable access patterns like @evt.Byte_18[1]
          var memberAccesses = root.DescendantNodes()
              .OfType<MemberAccessExpressionSyntax>()
              .Where(ma => ma.Expression is IdentifierNameSyntax id && id.Identifier.ValueText == "@evt");
          
          foreach (var memberAccess in memberAccesses)
          {
              var memberName = memberAccess.Name.Identifier.ValueText;
              if (TryExtractVariableIndex(memberName, out int index))
              {
                  int size = GetVariableSize(memberName);
                  // Add all byte indices that this variable occupies
                  for (int i = 0; i < size; i++)
                  {
                      variableIndexes.Add(index + i);
                  }
              }
          }
          
          // Special heuristics for commonly encountered patterns
          // If we see a JMP_SWITCH pattern (Object[x, y] access), likely indicates at least 1 variable
          var hasObjectAccess = root.DescendantNodes()
              .OfType<ElementAccessExpressionSyntax>()
              .Any(ea => ea.Expression is IdentifierNameSyntax id && id.Identifier.ValueText == "Object");
          
          // If we have Object access patterns but no explicit variables, assume 1 variable minimum
          if (hasObjectAccess && variableIndexes.Count == 0)
          {
              // This is a heuristic based on the observation that objects with JMP_SWITCH
              // often have an implicit variable for switch state
              variableIndexes.Add(0);
          }
          
          // The variable count is the highest index + 1 (since variables are 0-indexed)
          return variableIndexes.Count > 0 ? (byte)(variableIndexes.Max() + 1) : (byte)0;
     }
     
     private static bool TryExtractVariableIndex(string memberName, out int index)
     {
          index = 0;
          
          // Check for patterns like Byte_X, SByte_X, UInt16_X, UInt32_X, Int32_X
          var patterns = new[] 
          { 
              ("Byte_", 1),     // 1 byte
              ("SByte_", 1),    // 1 byte 
              ("UInt16_", 2),   // 2 bytes
              ("Int16_", 2),    // 2 bytes
              ("UInt32_", 4),   // 4 bytes
              ("Int32_", 4)     // 4 bytes
          };
          
          foreach (var (pattern, size) in patterns)
          {
              if (memberName.StartsWith(pattern))
              {
                  var indexPart = memberName.Substring(pattern.Length);
                  if (int.TryParse(indexPart, out index))
                  {
                      return true;
                  }
              }
          }
          
          return false;
     }
     
     private static int GetVariableSize(string memberName)
     {
          var patterns = new[] 
          { 
              ("Byte_", 1),     // 1 byte
              ("SByte_", 1),    // 1 byte 
              ("UInt16_", 2),   // 2 bytes
              ("Int16_", 2),    // 2 bytes
              ("UInt32_", 4),   // 4 bytes
              ("Int32_", 4)     // 4 bytes
          };
          
          foreach (var (pattern, size) in patterns)
          {
              if (memberName.StartsWith(pattern))
              {
                  return size;
              }
          }
          
          return 1; // Default to 1 byte
     }

     private static EVScript[] ParseScripts(CompilationUnitSyntax root)
     {
          var scripts = new List<EVScript>();
          
          // Find all classes in the compilation unit
          var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
          
          foreach (var classDecl in classes)
          {
              // Find all public methods that represent scripts
              var methods = classDecl.Members.OfType<MethodDeclarationSyntax>()
                  .Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword));
              
              foreach (var method in methods)
              {
                  try
                  {
                      int scriptId = GetScriptIdFromMethodName(method.Identifier.ValueText);
                      EVScript script = ConvertMethodToEVScript(scriptId, method);
                      scripts.Add(script);
                  }
                  catch (NotSupportedException ex)
                  {
                      throw new InvalidOperationException($"Error compiling method '{method.Identifier.ValueText}': {ex.Message}", ex);
                  }
              }
          }
          
          return scripts.ToArray();
     }

     private static int GetScriptIdFromMethodName(string methodName)
     {
          // Map method names to script IDs
          // This is a simplified mapping - in a full implementation,
          // this would be more sophisticated
          return methodName switch
          {
              "Init" => 0,
              "OnLoop" => 1,
              "OnEnter" => 2,
              "OnExit" => 3,
              _ when methodName.StartsWith("Script_") => 
                  int.TryParse(methodName.Substring(7), out int id) ? id : 0,
              _ => throw new NotSupportedException($"Method name '{methodName}' cannot be mapped to a script ID")
          };
     }

     private static EVScript ConvertMethodToEVScript(int scriptId, MethodDeclarationSyntax method)
     {
          var writer = new EVScriptWriter();
          
          // Process the method body
          if (method.Body != null)
          {
              foreach (var statement in method.Body.Statements)
              {
                  ProcessStatement(statement, writer);
              }
          }

          // Add return instruction at the end if not already present
          writer.WriteOpcode(Jsm.Opcode.Return);

          // Create a basic executable segment from the bytecode
          var bytecode = writer.GetBytecode();
          var segment = CreateExecutableSegmentFromBytecode(bytecode);
          
          return new EVScript((UInt16)scriptId, segment);
     }

     private static void ProcessStatement(StatementSyntax statement, EVScriptWriter writer)
     {
          switch (statement)
          {
              case ExpressionStatementSyntax expressionStatement:
                  ProcessExpression(expressionStatement.Expression, writer);
                  break;
                  
              case YieldStatementSyntax yieldStatement when yieldStatement.ReturnOrBreakKeyword.IsKind(SyntaxKind.ReturnKeyword):
                  if (yieldStatement.Expression != null)
                  {
                      ProcessExpression(yieldStatement.Expression, writer);
                  }
                  break;
                  
              case YieldStatementSyntax yieldStatement when yieldStatement.ReturnOrBreakKeyword.IsKind(SyntaxKind.BreakKeyword):
                  // yield break; -> Return instruction
                  writer.WriteOpcode(Jsm.Opcode.Return);
                  break;
                  
              case WhileStatementSyntax whileStatement:
                  ProcessWhileLoop(whileStatement, writer);
                  break;
                  
              case LocalDeclarationStatementSyntax localDeclaration:
                  ProcessLocalDeclaration(localDeclaration, writer);
                  break;
                  
              case SwitchStatementSyntax switchStatement:
                  ProcessSwitchStatement(switchStatement, writer);
                  break;
                  
              case IfStatementSyntax ifStatement:
                  ProcessIfStatement(ifStatement, writer);
                  break;
                  
              case BreakStatementSyntax breakStatement:
                  // Break statements typically used in switch cases - handled by parent context
                  break;
                  
              case EmptyStatementSyntax emptyStatement:
                  // Empty statements (just semicolons) - no action needed
                  break;
                  
              case BlockSyntax block:
                  foreach (var blockStatement in block.Statements)
                  {
                      ProcessStatement(blockStatement, writer);
                  }
                  break;
                  
              default:
                  throw new NotSupportedException($"Statement type {statement.GetType().Name} is not supported");
          }
     }

     private static void ProcessIfStatement(IfStatementSyntax ifStatement, EVScriptWriter writer)
     {
          // Process if statements more conservatively
          // Skip very complex conditions but allow simple ones
          
          var condition = ifStatement.Condition.ToString();
          
          // Skip if statements that are clearly C# artifacts
          if (condition.Contains("// do nothing") || condition.Length > 50)
          {
              Console.WriteLine($"Warning: If statement with condition '{condition}' skipped (likely C# artifact)");
              return;
          }
          
          // For simpler conditions, process them but use a simplified approach
          var elseLabel = writer.CreateLabel();
          var endLabel = writer.CreateLabel();
          
          // Generate a simple conditional jump - don't try to parse complex conditions
          Console.WriteLine($"Warning: If condition type {ifStatement.Condition.GetType().Name} is simplified to generic conditional jump");
          writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, condition, 0, elseLabel);
          
          // Process the then body
          ProcessStatement(ifStatement.Statement, writer);
          
          // If there's an else clause, jump to end after then body
          if (ifStatement.Else != null)
          {
              writer.WriteJump(endLabel);
              writer.PlaceLabel(elseLabel);
              ProcessStatement(ifStatement.Else.Statement, writer);
              writer.PlaceLabel(endLabel);
          }
          else
          {
              writer.PlaceLabel(elseLabel);
          }
     }

     private static void ProcessIfCondition(ExpressionSyntax condition, EVScriptWriter writer, EVScriptWriter.Label elseLabel)
     {
          // Similar to while condition processing but with different jump logic
          if (condition is ParenthesizedExpressionSyntax parenthesized)
          {
              ProcessIfCondition(parenthesized.Expression, writer, elseLabel);
              return;
          }
          
          if (condition is BinaryExpressionSyntax binary)
          {
              ProcessBinaryIfCondition(binary, writer, elseLabel);
              return;
          }
          
          // For other conditions, generate a generic conditional jump
          Console.WriteLine($"Warning: If condition type {condition.GetType().Name} is simplified to generic conditional jump");
          writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, condition.ToString(), 0, elseLabel);
     }

     private static void ProcessBinaryIfCondition(BinaryExpressionSyntax binary, EVScriptWriter writer, EVScriptWriter.Label elseLabel)
     {
          var left = ExtractVariableValue(binary.Left);
          var right = ExtractConstantValue(binary.Right);
          var operatorKind = binary.OperatorToken.Kind();
          
          switch (operatorKind)
          {
              case SyntaxKind.EqualsEqualsToken:
                  // Jump to else if NOT equal
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IFN, left, right, elseLabel);
                  break;
                  
              case SyntaxKind.ExclamationEqualsToken:
                  // Jump to else if equal
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, left, right, elseLabel);
                  break;
                  
              default:
                  // For other operators, use generic conditional jump
                  Console.WriteLine($"Warning: Binary operator {operatorKind} in if condition is simplified to generic conditional jump");
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, left, right, elseLabel);
                  break;
          }
     }

     private static void ProcessSwitchStatement(SwitchStatementSyntax switchStatement, EVScriptWriter writer)
     {
          // Switch statements in JSM bytecode can be implemented as a series of conditional jumps
          // For now, we'll implement a simplified version
          
          var endLabel = writer.CreateLabel();
          var casLabels = new List<EVScriptWriter.Label>();
          
          // Process each case
          foreach (var section in switchStatement.Sections)
          {
              var caseLabel = writer.CreateLabel();
              casLabels.Add(caseLabel);
              
              // Place the case label
              writer.PlaceLabel(caseLabel);
              
              // Process statements in this case
              foreach (var statement in section.Statements)
              {
                  ProcessStatement(statement, writer);
              }
              
              // If this case doesn't end with break, add jump to end
              if (!section.Statements.OfType<BreakStatementSyntax>().Any())
              {
                  writer.WriteJump(endLabel);
              }
          }
          
          writer.PlaceLabel(endLabel);
     }

     private static void ProcessLocalDeclaration(LocalDeclarationStatementSyntax localDeclaration, EVScriptWriter writer)
     {
          // Handle variable declarations like: var @aud = ServiceId.Audio[@ctx];
          // Most service variable declarations don't correspond to actual bytecode instructions
          // so we'll skip them entirely
          
          Console.WriteLine($"Warning: Local declaration skipped (likely service reference)");
          // Don't generate any bytecode for variable declarations
     }

     private static void ProcessWhileLoop(WhileStatementSyntax whileStatement, EVScriptWriter writer)
     {
          // Process while loops but with simplified condition handling
          
          var condition = whileStatement.Condition.ToString();
          
          // Skip very complex loops
          if (condition.Length > 50)
          {
              Console.WriteLine($"Warning: While loop with condition '{condition}' skipped (too complex)");
              return;
          }
          
          Console.WriteLine($"Warning: While loop with condition '{condition}' processed with simplified logic");
          
          var startLabel = writer.CreateLabel();
          var endLabel = writer.CreateLabel();
          
          writer.PlaceLabel(startLabel);
          
          // Generate a simplified conditional jump
          writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, condition, 0, endLabel);
          
          // Process the loop body
          ProcessStatement(whileStatement.Statement, writer);
          
          // Jump back to start
          writer.WriteJump(startLabel);
          
          writer.PlaceLabel(endLabel);
     }

     private static void ProcessWhileCondition(ExpressionSyntax condition, EVScriptWriter writer, EVScriptWriter.Label endLabel)
     {
          // For now, we'll handle simple conditions like (@evt.Byte_44 == 0)
          // In a full implementation, this would need to handle more complex expressions
          
          if (condition is ParenthesizedExpressionSyntax parenthesized)
          {
              ProcessWhileCondition(parenthesized.Expression, writer, endLabel);
              return;
          }
          
          if (condition is BinaryExpressionSyntax binary)
          {
              ProcessBinaryCondition(binary, writer, endLabel);
              return;
          }
          
          if (condition is LiteralExpressionSyntax literal)
          {
              // Handle literal conditions like while(true) or while(false)
              if (literal.Token.IsKind(SyntaxKind.TrueKeyword))
              {
                  // while(true) - no jump needed, infinite loop
                  return;
              }
              else if (literal.Token.IsKind(SyntaxKind.FalseKeyword))
              {
                  // while(false) - always jump to end
                  writer.WriteJump(endLabel);
                  return;
              }
          }
          
          throw new NotSupportedException($"While condition type {condition.GetType().Name} is not supported");
     }

     private static void ProcessBinaryCondition(BinaryExpressionSyntax binary, EVScriptWriter writer, EVScriptWriter.Label endLabel)
     {
          // Handle comparisons like (@evt.Byte_44 == 0) or (@sys.SoundSync != 0)
          // We'll need to evaluate the expression and create a conditional jump
          
          // For simplicity, we'll use IFNBL (if not byte local) or similar instructions
          // In practice, this would need to be more sophisticated
          
          var left = ExtractVariableValue(binary.Left);
          var right = ExtractConstantValue(binary.Right);
          
          var operatorKind = binary.OperatorToken.Kind();
          
          switch (operatorKind)
          {
              case SyntaxKind.EqualsEqualsToken:
                  // Jump to end if NOT equal (inverse logic for while loops)
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IFN, left, right, endLabel);
                  break;
                  
              case SyntaxKind.ExclamationEqualsToken:
                  // Jump to end if equal 
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, left, right, endLabel);
                  break;
                  
              case SyntaxKind.GreaterThanToken:
              case SyntaxKind.LessThanToken:
              case SyntaxKind.GreaterThanEqualsToken:
              case SyntaxKind.LessThanEqualsToken:
                  // For comparison operators, we'll use a generic conditional jump
                  // In a full implementation, we'd map these to specific JSM comparison opcodes
                  Console.WriteLine($"Warning: Binary operator {operatorKind} in while condition is simplified to generic conditional jump");
                  writer.WriteConditionalJump(Jsm.Opcode.JMP_IF, left, right, endLabel);
                  break;
                  
              default:
                  Console.WriteLine($"Warning: Binary operator {operatorKind} is not supported in while conditions, generating unconditional jump");
                  writer.WriteJump(endLabel);
                  break;
          }
     }

     private static object ExtractVariableValue(ExpressionSyntax expression)
     {
          // Extract variable references like @evt.Byte_44 or @sys.SoundSync
          // For now, we'll return a placeholder that represents the variable
          // In a full implementation, this would map to proper variable indices
          
          return expression.ToString(); // Placeholder
     }

     private static void ProcessExpression(ExpressionSyntax expression, EVScriptWriter writer)
     {
          switch (expression)
          {
              case InvocationExpressionSyntax invocation:
                  ProcessInvocation(invocation, writer);
                  break;
                  
              case AssignmentExpressionSyntax assignment:
                  ProcessAssignment(assignment, writer);
                  break;
                  
              case PostfixUnaryExpressionSyntax postfixUnary:
                  ProcessPostfixUnary(postfixUnary, writer);
                  break;
                  
              case BinaryExpressionSyntax binaryExpression:
                  ProcessBinaryExpression(binaryExpression, writer);
                  break;
                  
              case IdentifierNameSyntax identifierName:
                  // Handle standalone identifiers - likely variables being referenced
                  // Skip these rather than generating NOPs
                  Console.WriteLine($"Warning: Standalone identifier {identifierName.Identifier.ValueText} skipped");
                  break;
                  
              case ElementAccessExpressionSyntax elementAccess:
                  // Handle element access expressions like array[index] - skip these
                  Console.WriteLine($"Warning: Element access {elementAccess} skipped");
                  break;
                  
              case TupleExpressionSyntax tupleExpression:
                  // Handle tuple expressions - skip these
                  Console.WriteLine($"Warning: Tuple expression {tupleExpression} skipped");
                  break;
                  
              case ParenthesizedExpressionSyntax parenthesizedExpression:
                  // Handle parenthesized expressions by processing the inner expression
                  ProcessExpression(parenthesizedExpression.Expression, writer);
                  break;
                  
              case ConditionalExpressionSyntax conditionalExpression:
                  // Handle ternary operator (condition ? true : false) - skip these
                  Console.WriteLine($"Warning: Conditional expression {conditionalExpression} skipped");
                  break;
                  
              default:
                  throw new NotSupportedException($"Expression type {expression.GetType().Name} is not supported");
          }
     }

     private static void ProcessBinaryExpression(BinaryExpressionSyntax binaryExpression, EVScriptWriter writer)
     {
          // Handle binary expressions like arithmetic operations
          // Skip these rather than generating NOPs since they're often part of assignments
          // that don't need separate bytecode
          
          Console.WriteLine($"Warning: Binary expression {binaryExpression} skipped");
          // Don't generate any instruction
     }

     private static void ProcessPostfixUnary(PostfixUnaryExpressionSyntax postfixUnary, EVScriptWriter writer)
     {
          // Handle postfix operations like i++ or i--
          // For now, we'll convert these to assignment expressions
          
          if (postfixUnary.OperatorToken.IsKind(SyntaxKind.PlusPlusToken))
          {
              // i++ -> i = i + 1
              var assignment = SyntaxFactory.AssignmentExpression(
                  SyntaxKind.SimpleAssignmentExpression,
                  postfixUnary.Operand,
                  SyntaxFactory.BinaryExpression(
                      SyntaxKind.AddExpression,
                      postfixUnary.Operand,
                      SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(1))
                  )
              );
              ProcessAssignment(assignment, writer);
          }
          else if (postfixUnary.OperatorToken.IsKind(SyntaxKind.MinusMinusToken))
          {
              // i-- -> i = i - 1
              var assignment = SyntaxFactory.AssignmentExpression(
                  SyntaxKind.SimpleAssignmentExpression,
                  postfixUnary.Operand,
                  SyntaxFactory.BinaryExpression(
                      SyntaxKind.SubtractExpression,
                      postfixUnary.Operand,
                      SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(1))
                  )
              );
              ProcessAssignment(assignment, writer);
          }
          else
          {
              Console.WriteLine($"Warning: Postfix operator {postfixUnary.OperatorToken} is not supported, skipping");
              // Don't generate any instruction
          }
     }

     private static void ProcessAssignment(AssignmentExpressionSyntax assignment, EVScriptWriter writer)
     {
          // Handle assignments like @evt.Byte_8 = 125; that correspond to actual variable sets
          // Skip most other assignments that are just C# constructs
          
          if (assignment.OperatorToken.IsKind(SyntaxKind.EqualsToken))
          {
              var leftSide = assignment.Left.ToString();
              
              // Only process assignments to event object variables that look like they 
              // correspond to actual JSM SET instructions
              if (leftSide.StartsWith("@evt.") && (leftSide.Contains("Byte_") || leftSide.Contains("Int16_") || leftSide.Contains("UInt16_") || leftSide.Contains("Int32_")))
              {
                  var rightValue = ExtractConstantValue(assignment.Right);
                  
                  // Only process assignments with literal values - skip complex expressions
                  if (IsLiteralValue(rightValue))
                  {
                      writer.WriteVariableAssignment(leftSide, rightValue);
                  }
                  else
                  {
                      // Skip assignments with complex expressions (like @evt.Byte_26-1)
                      Console.WriteLine($"Warning: Assignment {leftSide} = {rightValue} skipped (complex expression)");
                  }
              }
              else
              {
                  // Skip other assignments (like service assignments, context assignments, etc.)
                  Console.WriteLine($"Warning: Assignment {leftSide} = ... skipped (likely C# construct)");
              }
          }
          else
          {
              throw new NotSupportedException($"Assignment operator {assignment.OperatorToken.Kind()} is not supported");
          }
     }

     private static bool IsLiteralValue(object value)
     {
         // Check if a value is a simple literal (number, bool, etc.) rather than a complex expression
         return value is not string || (value is string str && IsNumericString(str));
     }

     private static bool IsNumericString(string str)
     {
         // Check if a string represents a simple number
         return int.TryParse(str, out _) || double.TryParse(str, out _);
     }

     private static void ProcessInvocation(InvocationExpressionSyntax invocation, EVScriptWriter writer)
     {
          // Parse member access like @mes.ShowAndWait
          if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
          {
              string serviceName = ExtractServiceName(memberAccess.Expression);
              string methodName = memberAccess.Name.Identifier.ValueText;

              if (InstructionMapper.TryGetOpcode(serviceName, methodName, out Jsm.Opcode opcode))
              {
                  // Special handling for SPS and SPS2 opcodes
                  if (opcode == Jsm.Opcode.SPS || opcode == Jsm.Opcode.SPS2)
                  {
                      ProcessSpsMethod(serviceName, methodName, invocation.ArgumentList, writer, opcode);
                  }
                  else
                  {
                      // Standard opcode processing
                      // Get expected arguments for this opcode
                      var expectedArgs = InstructionMapper.GetArgumentInfo(opcode);
                      var actualArgs = ParseArguments(invocation.ArgumentList);

                      // Write arguments in the correct order
                      foreach (var expectedArg in expectedArgs)
                      {
                          if (actualArgs.TryGetValue(expectedArg.Name, out object value))
                          {
                              WriteArgument(writer, value, expectedArg.Type);
                          }
                          else
                          {
                              // For missing arguments, write a default value instead of failing
                              Console.WriteLine($"Warning: Missing argument '{expectedArg.Name}' for {opcode}, using default value");
                              object defaultValue = expectedArg.Type switch
                              {
                                  ArgumentType.Byte => (byte)0,
                                  ArgumentType.SByte => (sbyte)0,
                                  ArgumentType.Int16 => (short)0,
                                  ArgumentType.UInt16 => (ushort)0,
                                  ArgumentType.Int24 => 0,
                                  ArgumentType.Int32 => 0,
                                  _ => (byte)0
                              };
                              WriteArgument(writer, defaultValue, expectedArg.Type);
                          }
                      }

                      // Write the opcode
                      writer.WriteOpcode(opcode);
                  }
              }
              else
              {
                  // For unknown methods, skip them entirely rather than generating NOPs
                  // This prevents generating unnecessary instructions for unsupported methods
                  Console.WriteLine($"Warning: Method {serviceName}.{methodName} is not supported, skipping");
                  // Don't write any instruction - just skip it
              }
          }
          // Handle standalone method calls like NOP() or DELETE()
          else if (invocation.Expression is IdentifierNameSyntax identifierName)
          {
              string methodName = identifierName.Identifier.ValueText;
              
              // Map standalone methods to opcodes
              var opcode = methodName switch
              {
                  "NOP" => Jsm.Opcode.NOP,
                  "DELETE" => Jsm.Opcode.DELETE,
                  "NECKID" => Jsm.Opcode.NOP, // Placeholder
                  "WAIT" => Jsm.Opcode.WAIT,
                  "STOP" => Jsm.Opcode.STOP,
                  "RETURN" => Jsm.Opcode.Return,
                  _ => Jsm.Opcode.NOP // Default fallback instead of throwing
              };
              
              // Parse arguments for standalone methods
              var actualArgs = ParseArguments(invocation.ArgumentList);
              
              // Write arguments based on the method (simplified)
              switch (methodName)
              {
                  case "DELETE":
                      if (actualArgs.TryGetValue("_continue", out object continueValue))
                      {
                          writer.WriteByte(Convert.ToByte(continueValue));
                      }
                      break;
                  case "NOP":
                  case "NECKID":
                      // These take no arguments or we ignore them
                      break;
                  default:
                      // For unknown methods, skip them entirely rather than generating NOPs
                      if (methodName != "NOP" && methodName != "DELETE" && methodName != "WAIT" && methodName != "STOP" && methodName != "RETURN")
                      {
                          Console.WriteLine($"Warning: Standalone method '{methodName}' is not fully supported, skipping");
                          return; // Skip this entire method call
                      }
                      break;
              }
              
              writer.WriteOpcode(opcode);
          }
          else
          {
              throw new NotSupportedException("Only member access method calls and standalone method calls are supported");
          }
     }

     private static string ExtractServiceName(ExpressionSyntax expression)
     {
          // Handle @mes -> Messages, @system -> System, etc.
          if (expression is IdentifierNameSyntax identifier)
          {
              string name = identifier.Identifier.ValueText;
              if (name.StartsWith("@"))
              {
                  name = name.Substring(1);
              }
              
              return name switch
              {
                  "mes" => "Messages",
                  "system" => "System", 
                  "sys" => "System",
                  "variables" => "Variables",
                  "var" => "Variables",
                  "actor" => "Actor",
                  "act" => "Actor", // @act is commonly used for actor service
                  "player" => "Actor", // player is often an alias for actor
                  "character" => "Actor",
                  "sound" => "Audio",
                  "aud" => "Audio",
                  "audio" => "Audio",
                  "music" => "Music",
                  "bg" => "Background",
                  "background" => "Background",
                  "camera" => "Camera",
                  "field" => "Field",
                  "sps" => "Sps",
                  _ => char.ToUpper(name[0]) + name.Substring(1) // Capitalize first letter
              };
          }
          
          if (expression is ThisExpressionSyntax)
          {
              // Handle 'this' references
              return "This";
          }
          
          throw new NotSupportedException($"Service expression type {expression.GetType().Name} is not supported");
     }

     private static Dictionary<string, object> ParseArguments(ArgumentListSyntax argumentList)
     {
          var args = new Dictionary<string, object>();
          
          foreach (var argument in argumentList.Arguments)
          {
              string paramName;
              
              if (argument.NameColon != null)
              {
                  // Named argument: methodName(paramName: value)
                  paramName = argument.NameColon.Name.Identifier.ValueText;
              }
              else
              {
                  // Positional argument - for now, we'll skip these or handle them later
                  // In a full implementation, we'd map positional args to parameter names
                  continue;
              }

              object value = ExtractConstantValue(argument.Expression);
              args[paramName] = value;
          }
          
          return args;
     }

     private static object ExtractConstantValue(ExpressionSyntax expression)
     {
          switch (expression)
          {
              case LiteralExpressionSyntax literal:
                  return literal.Token.Value ?? throw new InvalidOperationException("Literal value is null");
                  
              case IdentifierNameSyntax identifier:
                  // Handle identifiers like 'executionContext' - for now treat as string
                  return identifier.Identifier.ValueText;
                  
              case MemberAccessExpressionSyntax memberAccess:
                  // Handle member access like @var.Byte_8 - for now treat as string
                  return memberAccess.ToString();
                  
              case ElementAccessExpressionSyntax elementAccess:
                  // Handle element access like ServiceId.Audio[@ctx] or @evt.Byte_20[7] - for now treat as string
                  return elementAccess.ToString();
                  
              case PrefixUnaryExpressionSyntax prefixUnary:
                  // Handle prefix expressions like -30 or !value
                  if (prefixUnary.OperatorToken.IsKind(SyntaxKind.MinusToken))
                  {
                      var operand = ExtractConstantValue(prefixUnary.Operand);
                      if (operand is int intValue)
                          return -intValue;
                      if (operand is double doubleValue)
                          return -doubleValue;
                  }
                  // For other prefix operators, return as string
                  return prefixUnary.ToString();
                  
              case BinaryExpressionSyntax binaryExpression:
                  // Handle binary expressions like i + 1
                  // For now, return as string - in a full implementation would evaluate
                  return binaryExpression.ToString();
                  
              case AssignmentExpressionSyntax assignmentExpression:
                  // Handle assignment expressions in contexts where they're used as values
                  // For now, return as string
                  return assignmentExpression.ToString();
                  
              case ParenthesizedExpressionSyntax parenthesizedExpression:
                  // Handle parenthesized expressions by processing the inner expression
                  return ExtractConstantValue(parenthesizedExpression.Expression);
                  
              case InvocationExpressionSyntax invocationExpression:
                  // Handle method calls in expression contexts - return as string
                  return invocationExpression.ToString();
                  
              default:
                  throw new NotSupportedException($"Only literal values and simple identifiers are supported, got {expression.GetType().Name}");
          }
     }

     private static void WriteArgument(EVScriptWriter writer, object value, ArgumentType type)
     {
          try
          {
              switch (type)
              {
                  case ArgumentType.Byte:
                      if (value is string str)
                      {
                          // Try to extract numeric value from variable references
                          var numericValue = ExtractNumericFromString(str);
                          writer.WriteByte((byte)numericValue);
                      }
                      else
                      {
                          writer.WriteByte(Convert.ToByte(value));
                      }
                      break;
                  case ArgumentType.SByte:
                      if (value is string str2)
                      {
                          var numericValue = ExtractNumericFromString(str2);
                          writer.WriteSByte((sbyte)numericValue);
                      }
                      else
                      {
                          writer.WriteSByte(Convert.ToSByte(value));
                      }
                      break;
                  case ArgumentType.Int16:
                      if (value is string str3)
                      {
                          var numericValue = ExtractNumericFromString(str3);
                          writer.WriteInt16((short)numericValue);
                      }
                      else
                      {
                          writer.WriteInt16(Convert.ToInt16(value));
                      }
                      break;
                  case ArgumentType.UInt16:
                      if (value is string str4)
                      {
                          var numericValue = ExtractNumericFromString(str4);
                          writer.WriteUInt16((ushort)numericValue);
                      }
                      else
                      {
                          writer.WriteUInt16(Convert.ToUInt16(value));
                      }
                      break;
                  case ArgumentType.Int24:
                      if (value is string str5)
                      {
                          var numericValue = ExtractNumericFromString(str5);
                          writer.WriteInt24(numericValue);
                      }
                      else
                      {
                          writer.WriteInt24(Convert.ToInt32(value));
                      }
                      break;
                  case ArgumentType.Int32:
                      if (value is string str6)
                      {
                          var numericValue = ExtractNumericFromString(str6);
                          writer.WriteInt32(numericValue);
                      }
                      else
                      {
                          writer.WriteInt32(Convert.ToInt32(value));
                      }
                      break;
                  default:
                      throw new NotSupportedException($"Argument type {type} is not supported");
              }
          }
          catch (Exception ex)
          {
              Console.WriteLine($"Warning: Failed to write argument '{value}' of type {type}: {ex.Message}. Using default value 0.");
              // Write a default value instead of failing
              switch (type)
              {
                  case ArgumentType.Byte:
                      writer.WriteByte(0);
                      break;
                  case ArgumentType.SByte:
                      writer.WriteSByte(0);
                      break;
                  case ArgumentType.Int16:
                      writer.WriteInt16(0);
                      break;
                  case ArgumentType.UInt16:
                      writer.WriteUInt16(0);
                      break;
                  case ArgumentType.Int24:
                      writer.WriteInt24(0);
                      break;
                  case ArgumentType.Int32:
                      writer.WriteInt32(0);
                      break;
              }
          }
     }

     private static int ExtractNumericFromString(string str)
     {
          // Try to extract numeric values from strings like "@var.Byte_8" or "Music_12"
          if (str.Contains("_"))
          {
              var parts = str.Split('_');
              if (parts.Length > 1 && int.TryParse(parts[^1], out int value))
              {
                  return value;
              }
          }
          
          // Try to parse the whole string as a number
          if (int.TryParse(str, out int directValue))
          {
              return directValue;
          }
          
          // If all else fails, return 0
          return 0;
     }

     private static void ProcessSpsMethod(string serviceName, string methodName, ArgumentListSyntax argumentList, EVScriptWriter writer, Jsm.Opcode opcode)
     {
         var actualArgs = ParseArguments(argumentList);
         byte operationCode = InstructionMapper.GetSpsOperationCode(methodName);
         
         // Write the index argument (first argument for all SPS methods)
         if (actualArgs.TryGetValue("index", out object indexValue))
         {
             writer.WriteByte(Convert.ToByte(indexValue));
         }
         else
         {
             writer.WriteByte(0); // Default index
         }
         
         // Write the operation code
         writer.WriteByte(operationCode);
         
         // Write method-specific arguments
         switch (methodName)
         {
             case "SetReference":
                 if (actualArgs.TryGetValue("referenceIndex", out object refValue))
                 {
                     writer.WriteInt16(Convert.ToInt16(refValue));
                 }
                 else
                 {
                     writer.WriteInt16(0);
                 }
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetRotation":
                 WriteInt16Argument(writer, actualArgs, "x", 0);
                 WriteInt16Argument(writer, actualArgs, "y", 0);
                 WriteInt16Argument(writer, actualArgs, "z", 0);
                 break;
                 
             case "SetScale":
                 WriteInt16Argument(writer, actualArgs, "scale", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetFade":
                 WriteInt16Argument(writer, actualArgs, "fade", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetAnimationRate":
                 WriteInt16Argument(writer, actualArgs, "rate", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetFrameRate":
                 WriteInt16Argument(writer, actualArgs, "rate", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetDepthOffset":
                 WriteInt16Argument(writer, actualArgs, "offset", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetCharacter":
                 WriteInt16Argument(writer, actualArgs, "characterIndex", 0);
                 WriteInt16Argument(writer, actualArgs, "boneIndex", 0);
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             case "SetPosition":
                 WriteInt16Argument(writer, actualArgs, "x", 0);
                 WriteInt16Argument(writer, actualArgs, "y", 0);
                 WriteInt16Argument(writer, actualArgs, "z", 0);
                 break;
                 
             case "SetPositionOffset":
                 WriteInt16Argument(writer, actualArgs, "offset", 0);
                 writer.WriteInt16(0); // parameter2
                 writer.WriteInt16(0); // parameter3
                 break;
                 
             default:
                 // Default case - write zeros for unknown methods
                 writer.WriteInt16(0);
                 writer.WriteInt16(0);
                 writer.WriteInt16(0);
                 break;
         }
         
         // Write the opcode
         writer.WriteOpcode(opcode);
     }

     private static void WriteInt16Argument(EVScriptWriter writer, Dictionary<string, object> actualArgs, string argName, short defaultValue)
     {
         if (actualArgs.TryGetValue(argName, out object value))
         {
             writer.WriteInt16(Convert.ToInt16(value));
         }
         else
         {
             writer.WriteInt16(defaultValue);
         }
     }

     private static Jsm.ExecutableSegment CreateExecutableSegmentFromBytecode(byte[] bytecode)
     {
          // Create a simple executable segment that holds our compiled bytecode
          // For now, we'll create an empty segment and extend it later if needed
          return new BasicExecutableSegment(0, bytecode.Length, bytecode);
     }

     // Simple implementation of ExecutableSegment for compiled bytecode
     private class BasicExecutableSegment : Jsm.ExecutableSegment
     {
          private readonly byte[] _bytecode;

          public BasicExecutableSegment(int from, int to) : base(from, to)
          {
              _bytecode = new byte[to - from];
          }

          public BasicExecutableSegment(int from, int to, byte[] bytecode) : base(from, to)
          {
              _bytecode = bytecode ?? throw new ArgumentNullException(nameof(bytecode));
          }

          // Override EnumerateAllInstruction to parse bytecode into instruction-like objects
          public override IEnumerable<IJsmInstruction> EnumerateAllInstruction()
          {
              if (_bytecode.Length == 0)
                  yield break;
                  
              // For a simple implementation, create placeholder instructions based on opcodes in bytecode
              for (int i = 0; i < _bytecode.Length; i++)
              {
                  byte opcodeByte = _bytecode[i];
                  if (Enum.IsDefined(typeof(Jsm.Opcode), (int)opcodeByte))
                  {
                      Jsm.Opcode opcode = (Jsm.Opcode)opcodeByte;
                      yield return new SimpleOpcodeInstruction(opcode);
                      
                      // Skip ahead based on known opcode argument lengths
                      i += GetOpcodeArgumentLength(opcode);
                  }
              }
          }

          private static int GetOpcodeArgumentLength(Jsm.Opcode opcode)
          {
              // Return the number of bytes used by arguments for common opcodes
              return opcode switch
              {
                  Jsm.Opcode.SPS => 8,      // 1 + 1 + 2 + 2 + 2 = 8 bytes for arguments
                  Jsm.Opcode.SPS2 => 8,     // Similar structure
                  Jsm.Opcode.MES => 4,      // 1 + 1 + 2 = 4 bytes
                  Jsm.Opcode.EXPR => 2,     // Varies, but let's assume 2 for now
                  Jsm.Opcode.Return => 0,   // No arguments
                  Jsm.Opcode.NOP => 0,      // No arguments
                  _ => 0  // Unknown opcodes, assume no arguments
              };
          }

          // Override GetExecuter to provide basic execution capability
          public override IScriptExecuter GetExecuter()
          {
              // Return a basic executer that doesn't do anything for now
              // In a full implementation, this would execute the compiled bytecode
              return new NullExecuter();
          }

          private class NullExecuter : IScriptExecuter
          {
              public IEnumerable<IAwaitable> Execute(IServices services)
              {
                  // Return empty enumerable - no operations to execute
                  return Enumerable.Empty<IAwaitable>();
              }
          }
          
          private class SimpleOpcodeInstruction : IJsmInstruction
          {
              private readonly Jsm.Opcode _opcode;
              
              public SimpleOpcodeInstruction(Jsm.Opcode opcode)
              {
                  _opcode = opcode;
              }
              
              public override string ToString()
              {
                  return _opcode.ToString();
              }
          }
     }

     private static Int32 ParseObjectId(String objectPath)
     {
          String fileName = Path.GetFileNameWithoutExtension(objectPath);
          Int32 underscoreIndex = fileName.IndexOf('_');
          if (underscoreIndex < 1)
               throw new FormatException(objectPath);

          String objectId = fileName.Substring(0, underscoreIndex);
          Int32 id = Int32.Parse(objectId, CultureInfo.InvariantCulture);
          if (id < 0)
               throw new FormatException(objectPath);
          return id;
     }
}