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
          // For now, return 0 variables. In a full implementation, this would
          // scan the C# code for variable declarations and count them.
          // Variables are typically defined in class-level fields or properties.
          return 0;
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
                  
              default:
                  throw new NotSupportedException($"Statement type {statement.GetType().Name} is not supported");
          }
     }

     private static void ProcessExpression(ExpressionSyntax expression, EVScriptWriter writer)
     {
          switch (expression)
          {
              case InvocationExpressionSyntax invocation:
                  ProcessInvocation(invocation, writer);
                  break;
                  
              default:
                  throw new NotSupportedException($"Expression type {expression.GetType().Name} is not supported");
          }
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
                          throw new InvalidOperationException($"Missing required argument '{expectedArg.Name}' for {opcode}");
                      }
                  }

                  // Write the opcode
                  writer.WriteOpcode(opcode);
              }
              else
              {
                  throw new NotSupportedException($"Method {serviceName}.{methodName} is not supported for compilation");
              }
          }
          else
          {
              throw new NotSupportedException("Only member access method calls are supported (e.g., @service.Method)");
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
                  "variables" => "Variables",
                  "actor" => "Actor",
                  "player" => "Actor", // player is often an alias for actor
                  "character" => "Actor",
                  "sound" => "Sound",
                  "music" => "Music",
                  "bg" => "Background",
                  "background" => "Background",
                  "camera" => "Camera",
                  "field" => "Field",
                  _ => char.ToUpper(name[0]) + name.Substring(1) // Capitalize first letter
              };
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
                  throw new NotSupportedException("Only named arguments are supported (e.g., windowId: 2)");
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
                  
              default:
                  throw new NotSupportedException($"Only literal values are supported, got {expression.GetType().Name}");
          }
     }

     private static void WriteArgument(EVScriptWriter writer, object value, ArgumentType type)
     {
          switch (type)
          {
              case ArgumentType.Byte:
                  writer.WriteByte(Convert.ToByte(value));
                  break;
              case ArgumentType.SByte:
                  writer.WriteSByte(Convert.ToSByte(value));
                  break;
              case ArgumentType.Int16:
                  writer.WriteInt16(Convert.ToInt16(value));
                  break;
              case ArgumentType.UInt16:
                  writer.WriteUInt16(Convert.ToUInt16(value));
                  break;
              case ArgumentType.Int24:
                  writer.WriteInt24(Convert.ToInt32(value));
                  break;
              case ArgumentType.Int32:
                  writer.WriteInt32(Convert.ToInt32(value));
                  break;
              default:
                  throw new NotSupportedException($"Argument type {type} is not supported");
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