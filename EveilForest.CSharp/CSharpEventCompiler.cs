using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Albeoris.Framework.Collections;
using EvilForest.Resources.Enums;
using FF8.Core;
using Memoria.EventEngine.EV;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FF8.JSM;
using FF8.JSM.Instructions;
using Memoria.EventEngine.Execution;
using Microsoft.CodeAnalysis;

namespace EveilForest.CSharp;

/// <summary>
/// Compiles C# pseudo-code files back to JSM bytecode wrapped in EVObject[].
/// </summary>
public sealed class CSharpEventCompiler : IEventCompiler
{
    public EVObject[] CompileDirectory(String directoryPath)
    {
        var objectFiles = Directory
            .GetFiles(directoryPath, "*.cs")
            .Where(f => IsObjectFile(f))
            .ToArray();

        var results = new List<EVObject>(objectFiles.Length);
        foreach (var file in objectFiles)
        {
            try
            {
                results.Add(ParseObject(file));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Failed to compile event object '{file}'.", ex);
            }
        }

        results.Sort((a, b) => a.Id.CompareTo(b.Id));
        return results.ToArray();
    }

    private static bool IsObjectFile(string path)
    {
        var stem = Path.GetFileNameWithoutExtension(path);
        var us = stem.IndexOf('_');
        return us > 0 && int.TryParse(stem.Substring(0, us), out _);
    }

    private static int ParseObjectId(string path)
    {
        var stem = Path.GetFileNameWithoutExtension(path);
        return int.Parse(stem.Substring(0, stem.IndexOf('_')));
    }

    private EVObject ParseObject(string filePath)
    {
        int id      = ParseObjectId(filePath);
        string code = File.ReadAllText(filePath);
        var tree    = CSharpSyntaxTree.ParseText(code);
        var root    = (CompilationUnitSyntax)tree.GetRoot();
        var cls     = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
        if (cls == null) return new EVObject(id, 0, 0, Array.Empty<EVScript>());

        byte varCount = ComputeVarCount(cls);
        byte flags = ReadByteConstant(cls, "__Flags");
        var scripts   = CompileScripts(cls).ToArray();
        return new EVObject(id, varCount, flags, scripts);
    }

    // ─── variable-count scanner ───────────────────────────────────────────────

    private static byte ComputeVarCount(ClassDeclarationSyntax cls)
    {
        byte declared = ReadByteConstant(cls, "__VariableCount");
        if (declared != 0)
            return declared;
        int maxEnd = 0;
        foreach (PropertyDeclarationSyntax property in cls.Members.OfType<PropertyDeclarationSyntax>())
        {
            string name = property.Identifier.ValueText;
            int separator = name.LastIndexOf('_');
            if (separator <= 0 ||
                !int.TryParse(name.Substring(separator + 1), NumberStyles.None,
                    CultureInfo.InvariantCulture, out int n))
                continue;

            string tn = name.Substring(0, separator);
            int byteEnd = tn switch
            {
                "Int16" or "UInt16" => n     + 2,
                "Int24" or "UInt24" => n     + 3,
                "Int32" or "UInt32" => n     + 4,
                "Bit" or "Boolean" => n / 8 + 1,
                "Byte" or "SByte" or "BitByte" or "SBitByte" => n + 1,
                _ => 0
            };
            if (byteEnd > maxEnd) maxEnd = byteEnd;
        }
        return (byte)Math.Min(maxEnd, 255);
    }

    private static byte ReadByteConstant(ClassDeclarationSyntax cls, string name)
    {
        VariableDeclaratorSyntax? variable = cls.Members.OfType<FieldDeclarationSyntax>()
            .SelectMany(field => field.Declaration.Variables)
            .FirstOrDefault(candidate => candidate.Identifier.ValueText == name);
        return variable?.Initializer?.Value is ExpressionSyntax expression &&
               TryParseConst(expression, out long value)
            ? (byte)value
            : (byte)0;
    }

    // ─── script compiler ─────────────────────────────────────────────────────

    private IEnumerable<EVScript> CompileScripts(ClassDeclarationSyntax cls)
    {
        foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
        {
            int sid = GetScriptId(method);
            if (sid < 0) continue;

            var ctx = new CompileCtx();
            CollectServiceAliases(method.Body, ctx);
            CompileBlock(method.Body, ctx);
            if (ctx.IsJumpTarget(ctx.Position))
            {
                if (ctx.LastInstructionOpcode == Jsm.Opcode.NOP)
                    ctx.Retarget(ctx.Position, ctx.LastInstructionStart);
                else
                {
                    int label = ctx.Position;
                    ctx.EmitOpcode(Jsm.Opcode.NOP);
                    ctx.RecordLabel(label);
                    ctx.RecordInstructionOpcode(Jsm.Opcode.NOP);
                }
            }

            byte[] bytecode = ctx.GetBytes();
            Jsm.ExecutableSegment seg;
            try
            {
                seg = EVFileReader.ParseScript(bytecode);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Script '{method.Identifier.ValueText}' produced invalid bytecode: " +
                    Convert.ToHexString(bytecode), ex);
            }
            CompiledBytecodeTag.Set(seg, bytecode);
            yield return new EVScript((ushort)sid, seg);
        }
    }

    private static int GetScriptId(MethodDeclarationSyntax m)
    {
        string name = m.Identifier.Text;
        if (name == "Init") return 0;
        if (name.StartsWith("Script_") && int.TryParse(name.Substring(7), out int sid)) return sid;
        return name switch { "OnLoop" => 1, "OnEnter" => 2, "OnExit" => 3, _ => -1 };
    }

    private static void CollectServiceAliases(BlockSyntax? body, CompileCtx ctx)
    {
        if (body == null) return;
        foreach (var stmt in body.Statements.OfType<LocalDeclarationStatementSyntax>())
        {
            foreach (var decl in stmt.Declaration.Variables)
            {
                var short_name = decl.Identifier.Text;
                if (short_name.StartsWith("@") &&
                    decl.Initializer?.Value is ElementAccessExpressionSyntax ea &&
                    ea.Expression is MemberAccessExpressionSyntax ma)
                {
                    ctx.AddService(short_name, ma.Name.Identifier.Text);
                }
            }
        }
    }

    // ─── statement dispatcher ────────────────────────────────────────────────

    private void CompileBlock(BlockSyntax? body, CompileCtx ctx)
    {
        if (body == null) return;
        StatementSyntax? previousNext = ctx.NextStatement;
        for (int i = 0; i < body.Statements.Count; i++)
        {
            ctx.NextStatement = i + 1 < body.Statements.Count
                ? body.Statements[i + 1]
                : previousNext;
            CompileStmt(body.Statements[i], ctx);
            ctx.ResolveDeferredJumps(body.Statements[i]);
        }
        ctx.NextStatement = previousNext;
    }

    private void CompileInfiniteWhile(WhileStatementSyntax loop, CompileCtx ctx)
    {
        int loopStart = ctx.Position;
        CompileBlock(loop.Statement as BlockSyntax ?? WrapInBlock(loop.Statement), ctx);

        ctx.EmitOpcode(Jsm.Opcode.JMP);
        int slot = ctx.Position;
        ctx.WriteInt16(0);
        ctx.RecordJumpTarget(loopStart);
        ctx.PatchInt16(slot, (short)(loopStart - (slot + 2)));
    }

    private static bool IsTrue(ExpressionSyntax expression)
    {
        while (expression is ParenthesizedExpressionSyntax parenthesized)
            expression = parenthesized.Expression;
        return expression.IsKind(SyntaxKind.TrueLiteralExpression);
    }

    private void CompileStmt(StatementSyntax stmt, CompileCtx ctx)
    {
        switch (stmt)
        {
            case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax asgn }:
                CompileAssign(asgn, ctx);
                break;
            case ExpressionStatementSyntax { Expression: InvocationExpressionSyntax inv }:
                CompileInvocation(inv, ctx);
                break;
            case ExpressionStatementSyntax { Expression: PostfixUnaryExpressionSyntax unary }:
                CompileStandaloneUnary(unary.Operand,
                    unary.IsKind(SyntaxKind.PostIncrementExpression)
                        ? Jsm.Excode.IncrementPost
                        : Jsm.Excode.DecrementPost, ctx);
                break;
            case ExpressionStatementSyntax { Expression: PrefixUnaryExpressionSyntax unary }
                when unary.IsKind(SyntaxKind.PreIncrementExpression) ||
                     unary.IsKind(SyntaxKind.PreDecrementExpression):
                CompileStandaloneUnary(unary.Operand,
                    unary.IsKind(SyntaxKind.PreIncrementExpression)
                        ? Jsm.Excode.B_PRE_PLUS
                        : Jsm.Excode.B_PRE_MINUS, ctx);
                break;
            case YieldStatementSyntax ys:
                if (ys.IsKind(SyntaxKind.YieldBreakStatement))
                    ctx.EmitOpcode(Jsm.Opcode.Return);
                else if (ys.Expression is InvocationExpressionSyntax yi)
                    CompileYieldReturn(yi, ctx);
                break;
            case IfStatementSyntax ifS:
                CompileIf(ifS, ctx);
                break;
            case WhileStatementSyntax ws:
                if (IsTrue(ws.Condition))
                    CompileInfiniteWhile(ws, ctx);
                else
                    CompileWhile(ws, ctx);
                break;
            case SwitchStatementSyntax ss:
                CompileSwitch(ss, ctx);
                break;
            case BlockSyntax blk:
                CompileBlock(blk, ctx);
                break;
            case LocalDeclarationStatementSyntax:
            case ReturnStatementSyntax:
            case BreakStatementSyntax:
            case EmptyStatementSyntax:
                break;
        }
    }

    // ─── assignment: @prefix.TypeName_N[k] = value ───────────────────────────

    private static void CompileAssign(AssignmentExpressionSyntax asgn, CompileCtx ctx)
    {
        // Skip Init-method field assignments: @ctx = …, @evt = …
        if (asgn.Left is IdentifierNameSyntax lid &&
            (lid.Identifier.Text == "@ctx" || lid.Identifier.Text == "@evt"))
            return;

        if (!TryParseVarRef(asgn.Left, out var v26)) return;

        var enc = new ExprEnc();
        enc.PushVar(v26);
        if (!asgn.IsKind(SyntaxKind.SimpleAssignmentExpression))
            enc.PushVar(v26);
        EncodeExpr(asgn.Right, enc);
        enc.Op(asgn.Kind() switch
        {
            SyntaxKind.SimpleAssignmentExpression      => (byte)Jsm.Excode.Let,
            SyntaxKind.AddAssignmentExpression         => (byte)Jsm.Excode.Add,
            SyntaxKind.SubtractAssignmentExpression    => (byte)Jsm.Excode.Sub,
            SyntaxKind.MultiplyAssignmentExpression    => (byte)Jsm.Excode.Mul,
            SyntaxKind.DivideAssignmentExpression      => (byte)Jsm.Excode.Div,
            SyntaxKind.ModuloAssignmentExpression      => (byte)Jsm.Excode.Mod,
            SyntaxKind.AndAssignmentExpression         => (byte)Jsm.Excode.BitAnd,
            SyntaxKind.OrAssignmentExpression          => (byte)Jsm.Excode.BitOr,
            SyntaxKind.ExclusiveOrAssignmentExpression => (byte)Jsm.Excode.BitXor,
            SyntaxKind.LeftShiftAssignmentExpression   => (byte)Jsm.Excode.BitLeft,
            SyntaxKind.RightShiftAssignmentExpression  => (byte)Jsm.Excode.BitRight,
            _                                           => (byte)Jsm.Excode.Let
        });
        if (!asgn.IsKind(SyntaxKind.SimpleAssignmentExpression))
            enc.Op((byte)Jsm.Excode.Let);
        enc.Op((byte)Jsm.Excode.End);

        int label = ctx.Position;
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(enc.ToArray());
        ctx.RecordLabel(label);
    }

    private static void CompileStandaloneUnary(ExpressionSyntax operand, Jsm.Excode opcode,
        CompileCtx ctx)
    {
        var encoded = new ExprEnc();
        EncodeExpr(operand, encoded);
        encoded.Op((byte)opcode);
        encoded.Op((byte)Jsm.Excode.End);
        int label = ctx.Position;
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(encoded.ToArray());
        ctx.RecordLabel(label);
    }

    // ─── yield return @svc.Method(args); // OPCODE ────────────────────────────

    private void CompileYieldReturn(InvocationExpressionSyntax inv, CompileCtx ctx)
    {
        EmitInvocation(inv, ctx, "yield-return");
    }

    // ─── standalone call: REQ(…); AICON(…); etc. ─────────────────────────────

    private void CompileInvocation(InvocationExpressionSyntax inv, CompileCtx ctx)
    {
        EmitInvocation(inv, ctx, "instruction");
    }

    private static void EmitInvocation(InvocationExpressionSyntax invocation, CompileCtx ctx, string kind)
    {
        List<ExpressionSyntax> encodedArguments;
        Jsm.Opcode opcode;
        if (!TryParseParameterizedComment(invocation, out opcode, out encodedArguments))
        {
            if (!TryResolveOpcode(invocation, ctx, out opcode))
            {
                if (TryEmitExpressionInstruction(invocation, ctx))
                    return;
                throw new InvalidDataException(
                    $"Unsupported {kind} '{invocation}'. A compiler must never silently alter it.");
            }
            encodedArguments = invocation.ArgumentList.Arguments
                .Select(argument => argument.Expression).ToList();
        }

        InstructionSchema schema = InstructionSchema.For(opcode);
        AdaptSemanticArguments(opcode, schema, encodedArguments, invocation);
        if (schema.Widths.Length != 0 && encodedArguments.Count != schema.Widths.Length)
            throw new InvalidDataException(
                $"Instruction {opcode} expects {schema.Widths.Length} encoded arguments, " +
                $"but '{invocation}' supplies {encodedArguments.Count}.");

        int instrLabel = ctx.Position;
        ctx.EmitOpcode(opcode);
        int maskSlot = -1;
        byte argumentMask = 0;
        if (schema.HasArgumentMask)
        {
            maskSlot = ctx.Position;
            ctx.WriteByte(0);
        }

        for (int i = 0; i < schema.Widths.Length; i++)
        {
            ExpressionSyntax expression = encodedArguments[i];
            int width = schema.Widths[i];
            if (schema.HasArgumentMask && !TryParseConst(expression, out long v))
            {
                argumentMask |= (byte)(1 << i);
                var encoded = new ExprEnc();
                EncodeExpr(expression, encoded);
                encoded.Op((byte)Jsm.Excode.End);
                ctx.WriteBytes(encoded.ToArray());
                continue;
            }

            TryParseConst(expression, out v);
            switch (width) {
                case 1: ctx.WriteByte((byte)v);   break;
                case 2: ctx.WriteInt16((short)v); break;
                case 3:
                    ctx.WriteByte((byte)v);
                    ctx.WriteByte((byte)(v >> 8));
                    ctx.WriteByte((byte)(v >> 16));
                    break;
                case 4: ctx.WriteInt32((int)v);   break;
            }
        }
        if (maskSlot >= 0)
            ctx.PatchByte(maskSlot, argumentMask);
        ctx.RecordLabel(instrLabel);
        ctx.RecordInstructionOpcode(opcode);
    }

    private static void AdaptSemanticArguments(Jsm.Opcode opcode, InstructionSchema schema,
        List<ExpressionSyntax> arguments, InvocationExpressionSyntax invocation)
    {
        if (opcode is Jsm.Opcode.AIDLE or Jsm.Opcode.AWALK or Jsm.Opcode.ARUN or
            Jsm.Opcode.ATURNL or Jsm.Opcode.ATURNR or Jsm.Opcode.ASLEEP && arguments.Count == 2)
        {
            arguments.RemoveAt(0); // AnimationKind selects the opcode; ID is encoded.
        }
        else if (opcode == Jsm.Opcode.WIPERGB && arguments.Count == 5 && schema.Widths.Length == 6)
        {
            Match unknown = Regex.Match(GetTrailingComment(invocation),
                @"WIPERGB\(unknown:\s*(.+)\)");
            arguments.Insert(2, unknown.Success
                ? SyntaxFactory.ParseExpression(unknown.Groups[1].Value)
                : SyntaxFactory.LiteralExpression(
                    SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0)));
        }
    }

    private static bool TryParseParameterizedComment(InvocationExpressionSyntax invocation,
        out Jsm.Opcode opcode, out List<ExpressionSyntax> arguments)
    {
        string comment = GetTrailingComment(invocation);
        Match sound = Regex.Match(comment,
            @"FLDSND\(op:\s*([^,\)]+),\s*sound:\s*([^,\)]+)(?:,\s*a1:\s*([^,\)]+))?(?:,\s*a2:\s*([^,\)]+))?(?:,\s*a3:\s*([^,\)]+))?\)");
        if (sound.Success)
        {
            int optionalCount = Enumerable.Range(3, 3).Count(index => sound.Groups[index].Success);
            opcode = optionalCount switch
            {
                0 => Jsm.Opcode.FLDSND0,
                1 => Jsm.Opcode.FLDSND1,
                2 => Jsm.Opcode.FLDSND2,
                _ => Jsm.Opcode.FLDSND3
            };
            arguments = new List<ExpressionSyntax>
            {
                SyntaxFactory.ParseExpression(sound.Groups[1].Value),
                SyntaxFactory.ParseExpression(sound.Groups[2].Value)
            };
            for (int index = 3; index <= 5; index++)
                if (sound.Groups[index].Success)
                    arguments.Add(SyntaxFactory.ParseExpression(sound.Groups[index].Value));
            return true;
        }

        Match sps = Regex.Match(comment,
            @"SPS\(\s*([^,]+),\s*([A-Za-z0-9_]+),\s*([^,]+),\s*([^,]+),\s*([^\)]+)\)");
        if (sps.Success && SpsOperationCodes.TryGetValue(sps.Groups[2].Value, out byte operation))
        {
            opcode = sps.Groups[2].Value == "SetCharacter" ? Jsm.Opcode.SPS2 : Jsm.Opcode.SPS;
            arguments = new List<ExpressionSyntax>
            {
                SyntaxFactory.ParseExpression(sps.Groups[1].Value),
                SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                    SyntaxFactory.Literal(operation)),
                SyntaxFactory.ParseExpression(sps.Groups[3].Value),
                SyntaxFactory.ParseExpression(sps.Groups[4].Value),
                SyntaxFactory.ParseExpression(sps.Groups[5].Value)
            };
            return true;
        }

        opcode = default;
        arguments = new List<ExpressionSyntax>();
        return false;
    }

    private static readonly Dictionary<string, byte> SpsOperationCodes = new()
    {
        ["SetReference"] = 130, ["SetAttribute"] = 131, ["SetPosition"] = 135,
        ["SetRotation"] = 140, ["SetScale"] = 145, ["SetCharacter"] = 150,
        ["SetFade"] = 155, ["SetAnimationRate"] = 156, ["SetFrameRate"] = 160,
        ["SetCurrentFrame"] = 161, ["SetPositionOffset"] = 165,
        ["SetDepthOffset"] = 170,
    };

    private static string GetTrailingComment(InvocationExpressionSyntax invocation)
    {
        StatementSyntax? statement = invocation.AncestorsAndSelf().OfType<StatementSyntax>().FirstOrDefault();
        return statement?.GetTrailingTrivia().ToFullString() ?? String.Empty;
    }

    private static bool TryEmitExpressionInstruction(InvocationExpressionSyntax invocation, CompileCtx ctx)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax member ||
            member.Name.Identifier.ValueText != "CalcAngleByTangent")
            return false;

        var encoded = new ExprEnc();
        foreach (ArgumentSyntax argument in invocation.ArgumentList.Arguments.Reverse())
            EncodeExpr(argument.Expression, encoded);
        encoded.Op((byte)Jsm.Excode.GetAngle);
        encoded.Op((byte)Jsm.Excode.End);

        int label = ctx.Position;
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(encoded.ToArray());
        ctx.RecordLabel(label);
        return true;
    }

    private static bool TryResolveOpcode(InvocationExpressionSyntax invocation, CompileCtx ctx,
        out Jsm.Opcode opcode)
    {
        if (invocation.Expression is IdentifierNameSyntax identifier &&
            Enum.TryParse(identifier.Identifier.ValueText, out opcode))
            return true;

        string trivia = GetTrailingComment(invocation);
        Match commentOpcode = Regex.Match(trivia, @"//\s*([A-Z][A-Z0-9_]*)");
        if (commentOpcode.Success &&
            Enum.TryParse(commentOpcode.Groups[1].Value, out opcode))
            return true;

        if (invocation.Expression is MemberAccessExpressionSyntax member &&
            member.Expression is IdentifierNameSyntax serviceIdentifier)
        {
            string service = ctx.ResolveService(serviceIdentifier.Identifier.Text);
            string method = member.Name.Identifier.ValueText;
            if (ServiceOpcode.TryGetValue((service, method), out opcode))
                return true;
        }

        opcode = default;
        return false;
    }

    private static readonly Dictionary<(string Service, string Method), Jsm.Opcode> ServiceOpcode = new()
    {
        { ("System", "Wait"), Jsm.Opcode.WAIT },
        { ("Messages", "ShowAndWait"), Jsm.Opcode.MES },
        { ("Messages", "Show"), Jsm.Opcode.MESN },
        { ("Messages", "Wait"), Jsm.Opcode.WAITMES },
    };

    // ─── if statement ─────────────────────────────────────────────────────────

    private void CompileIf(IfStatementSyntax ifS, CompileCtx ctx)
    {
        var (cond, negated) = UnwrapNot(ifS.Condition);
        // if(cond)  → JMP_IFN (isTrue=true)   : skip when cond TRUE
        // if(!(c))  → JMP_IF  (isTrue=false)  : skip when cond FALSE
        var jmpOp = negated ? Jsm.Opcode.JMP_IF : Jsm.Opcode.JMP_IFN;

        EmitCondExpr(cond, ctx);
        ctx.EmitOpcode(jmpOp);
        int slot    = ctx.Position;
        ctx.WriteInt16(0);         // forward placeholder

        BlockSyntax body = ifS.Statement as BlockSyntax ?? WrapInBlock(ifS.Statement);
        int bodyStart = ctx.Position;
        if (ifS.Else != null)
        {
            bool hasTailAfterJump = body.Statements.Count > 1;
            int bodyCountBeforeJump = hasTailAfterJump
                ? body.Statements.Count - 1
                : body.Statements.Count;
            for (int i = 0; i < bodyCountBeforeJump; i++)
                CompileStmt(body.Statements[i], ctx);

            ctx.EmitOpcode(Jsm.Opcode.JMP);
            int endSlot = ctx.Position;
            ctx.WriteInt16(0);
            ctx.RecordJumpTarget(ctx.Position);
            ctx.PatchInt16(slot, (short)(ctx.Position - (slot + 2)));
            if (hasTailAfterJump)
                CompileStmt(body.Statements[^1], ctx);
            CompileStmt(ifS.Else.Statement, ctx);
            int endTarget = ctx.LastInstructionStart >= bodyStart
                ? ctx.LastInstructionStart
                : ctx.Position;
            ctx.RecordJumpTarget(endTarget);
            ctx.PatchInt16(endSlot, (short)(endTarget - (endSlot + 2)));
        }
        else
        {
            CompileBlock(body, ctx);
            bool nextStartsControl = ctx.NextStatement is IfStatementSyntax or
                WhileStatementSyntax or SwitchStatementSyntax;
            bool trailingAssignments = body.Statements.Count >= 2 &&
                IsAssignment(body.Statements[^1]) && IsAssignment(body.Statements[^2]);
            bool endsWithYieldBreak = body.Statements.LastOrDefault() is YieldStatementSyntax;
            bool endsWithControl = body.Statements.LastOrDefault() is IfStatementSyntax or
                WhileStatementSyntax or SwitchStatementSyntax;
            int target = !endsWithYieldBreak && !endsWithControl &&
                         (!nextStartsControl || trailingAssignments) &&
                         ctx.LastInstructionStart >= bodyStart
                ? ctx.LastInstructionStart
                : ctx.Position;
            if (ctx.NextStatement is YieldStatementSyntax)
            {
                ctx.DeferRelativeJump(slot, ctx.NextStatement);
            }
            else
            {
                ctx.RecordJumpTarget(target);
                ctx.PatchInt16(slot, (short)(target - (slot + 2)));
            }
        }
    }

    // ─── while statement ──────────────────────────────────────────────────────

    private void CompileWhile(WhileStatementSyntax ws, CompileCtx ctx)
    {
        var (cond, negated) = UnwrapNot(ws.Condition);
        var jmpOp = negated ? Jsm.Opcode.JMP_IF : Jsm.Opcode.JMP_IFN;

        // Record position where the condition EXPR begins
        int condStart = ctx.Position;
        EmitCondExpr(cond, ctx);

        ctx.EmitOpcode(jmpOp);
        int forwardSlot = ctx.Position;
        ctx.WriteInt16(0);         // forward placeholder (exit loop)

        ctx.StartBody();
        CompileBlock(ws.Statement as BlockSyntax ?? WrapInBlock(ws.Statement), ctx);
        ctx.EndBody();

        // Backward JMP → condStart
        ctx.EmitOpcode(Jsm.Opcode.JMP);
        int backSlot = ctx.Position;
        ctx.WriteInt16(0);
        int afterLoop = ctx.Position;

        ctx.RecordJumpTarget(condStart);
        ctx.PatchInt16(backSlot,    (short)(condStart - afterLoop));
        ctx.RecordJumpTarget(afterLoop);
        ctx.PatchInt16(forwardSlot, (short)(afterLoop - (forwardSlot + 2)));
    }

    private static bool IsAssignment(StatementSyntax statement) =>
        statement is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax };

    private void CompileSwitch(SwitchStatementSyntax statement, CompileCtx ctx)
    {
        var cases = new List<(int Value, SwitchSectionSyntax Section)>();
        SwitchSectionSyntax? defaultSection = null;
        foreach (SwitchSectionSyntax section in statement.Sections)
        {
            foreach (SwitchLabelSyntax label in section.Labels)
            {
                if (label is DefaultSwitchLabelSyntax)
                    defaultSection = section;
                else if (label is CaseSwitchLabelSyntax caseLabel &&
                         TryParseConst(caseLabel.Value, out long value))
                    cases.Add(((int)value, section));
            }
        }

        if (cases.Count == 0)
            throw new InvalidDataException("A switch must contain at least one constant case.");
        bool isExtended = false;
        for (int i = 1; i < cases.Count; i++)
            if (cases[i].Value != cases[0].Value + i)
                isExtended = true;

        var condition = new ExprEnc();
        EncodeExpr(statement.Expression, condition);
        condition.Op((byte)Jsm.Excode.End);
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(condition.ToArray());

        ctx.EmitOpcode(isExtended ? Jsm.Opcode.JMP_SWITCHEX : Jsm.Opcode.JMP_SWITCH);
        int switchBase = ctx.Position + (isExtended ? 3 : 0);
        ctx.WriteByte((byte)cases.Count);
        if (!isExtended)
            ctx.WriteInt16((short)cases[0].Value);
        int defaultSlot = ctx.Position;
        ctx.WriteInt16(0);
        int[] caseSlots = new int[cases.Count];
        for (int i = 0; i < caseSlots.Length; i++)
        {
            if (isExtended)
                ctx.WriteInt16((short)cases[i].Value);
            caseSlots[i] = ctx.Position;
            ctx.WriteInt16(0);
        }

        var starts = new int[cases.Count];
        var breakSlots = new List<int>();
        for (int i = 0; i < cases.Count; i++)
        {
            starts[i] = ctx.Position;
            CompileSwitchSection(cases[i].Section, ctx, breakSlots);
        }

        int defaultStart = ctx.Position;
        if (defaultSection != null)
            CompileSwitchSection(defaultSection, ctx, breakSlots);
        int end = ctx.Position;

        ctx.RecordJumpTarget(defaultStart);
        ctx.PatchInt16(defaultSlot, (short)(defaultStart - switchBase));
        for (int i = 0; i < starts.Length; i++)
        {
            ctx.RecordJumpTarget(starts[i]);
            ctx.PatchInt16(caseSlots[i], (short)(starts[i] - switchBase));
        }
        foreach (int slot in breakSlots)
        {
            ctx.RecordJumpTarget(end);
            ctx.PatchInt16(slot, (short)(end - (slot + 2)));
        }
    }

    private void CompileSwitchSection(SwitchSectionSyntax section, CompileCtx ctx, List<int> breakSlots)
    {
        foreach (StatementSyntax child in section.Statements)
            CompileSwitchChild(child, ctx, breakSlots);
    }

    private void CompileSwitchChild(StatementSyntax child, CompileCtx ctx, List<int> breakSlots)
    {
        if (child is BreakStatementSyntax)
        {
            ctx.EmitOpcode(Jsm.Opcode.JMP);
            breakSlots.Add(ctx.Position);
            ctx.WriteInt16(0);
        }
        else if (child is BlockSyntax block)
        {
            foreach (StatementSyntax nested in block.Statements)
                CompileSwitchChild(nested, ctx, breakSlots);
        }
        else
        {
            CompileStmt(child, ctx);
        }
    }

    // ─── condition expression emitter ────────────────────────────────────────

    private static void EmitCondExpr(ExpressionSyntax cond, CompileCtx ctx)
    {
        var enc = new ExprEnc();
        EncodeExpr(cond, enc);
        enc.Op((byte)Jsm.Excode.End);
        // Condition EXPR is NOT an instruction (no label recorded), just raw bytes
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(enc.ToArray());
    }

    private static void EncodeExpr(ExpressionSyntax e, ExprEnc enc)
    {
        while (e is ParenthesizedExpressionSyntax p) e = p.Expression;

        if (e is CastExpressionSyntax cast)
        {
            EncodeExpr(cast.Expression, enc);
            return;
        }
        if (e is PrefixUnaryExpressionSyntax constantUnary &&
            TryParseConst(constantUnary, out long constantValue))
        {
            enc.PushConst16((short)constantValue);
            return;
        }
        if (e is PrefixUnaryExpressionSyntax unary)
        {
            EncodeExpr(unary.Operand, enc);
            enc.Op(unary.Kind() switch
            {
                SyntaxKind.UnaryMinusExpression       => (byte)Jsm.Excode.B_SINGLE_MINUS,
                SyntaxKind.UnaryPlusExpression        => (byte)Jsm.Excode.B_SINGLE_PLUS,
                SyntaxKind.LogicalNotExpression       => (byte)Jsm.Excode.B_NOT,
                SyntaxKind.BitwiseNotExpression       => (byte)Jsm.Excode.B_COMP,
                SyntaxKind.PreIncrementExpression     => (byte)Jsm.Excode.B_PRE_PLUS,
                SyntaxKind.PreDecrementExpression     => (byte)Jsm.Excode.B_PRE_MINUS,
                _                                     => (byte)Jsm.Excode.B_SINGLE_PLUS
            });
            return;
        }

        if (e is BinaryExpressionSyntax bin)
        {
            EncodeExpr(bin.Left,  enc);
            EncodeExpr(bin.Right, enc);
            enc.Op(bin.Kind() switch {
                SyntaxKind.EqualsExpression             => (byte)Jsm.Excode.Equivalence,
                SyntaxKind.NotEqualsExpression          => (byte)Jsm.Excode.NotEquivalence,
                SyntaxKind.LessThanExpression           => (byte)Jsm.Excode.LessThan,
                SyntaxKind.LessThanOrEqualExpression    => (byte)Jsm.Excode.LessOrEquals,
                SyntaxKind.GreaterThanExpression        => (byte)Jsm.Excode.GreatThan,
                SyntaxKind.GreaterThanOrEqualExpression => (byte)Jsm.Excode.GreatOrEquals,
                SyntaxKind.LogicalAndExpression         => (byte)Jsm.Excode.BooleanAnd,
                SyntaxKind.LogicalOrExpression          => (byte)Jsm.Excode.BooleanOr,
                SyntaxKind.AddExpression                => (byte)Jsm.Excode.Add,
                SyntaxKind.SubtractExpression           => (byte)Jsm.Excode.Sub,
                SyntaxKind.MultiplyExpression           => (byte)Jsm.Excode.Mul,
                SyntaxKind.DivideExpression             => (byte)Jsm.Excode.Div,
                SyntaxKind.ModuloExpression             => (byte)Jsm.Excode.Mod,
                SyntaxKind.BitwiseAndExpression         => (byte)Jsm.Excode.BitAnd,
                SyntaxKind.BitwiseOrExpression          => (byte)Jsm.Excode.BitOr,
                SyntaxKind.ExclusiveOrExpression        => (byte)Jsm.Excode.BitXor,
                SyntaxKind.LeftShiftExpression          => (byte)Jsm.Excode.BitLeft,
                SyntaxKind.RightShiftExpression         => (byte)Jsm.Excode.BitRight,
                _                                       => (byte)Jsm.Excode.Equivalence
            });
            return;
        }
        if (e is LiteralExpressionSyntax lit)
        {
            short v = lit.IsKind(SyntaxKind.TrueLiteralExpression)  ?  (short)1 :
                      lit.IsKind(SyntaxKind.FalseLiteralExpression) ?  (short)0 :
                      (short)Convert.ToInt32(lit.Token.Value);
            enc.PushConst16(v);
            return;
        }
        if (e is MemberAccessExpressionSyntax systemMember &&
            systemMember.Expression is IdentifierNameSyntax systemIdentifier &&
            systemIdentifier.Identifier.Text == "@sys" &&
            Enum.TryParse(systemMember.Name.Identifier.ValueText, out SystemData systemData))
        {
            enc.Op((byte)Jsm.Excode.B_SYSVAR);
            enc.Op((byte)systemData);
            return;
        }
        if (TryParseVarRef(e, out var v26)) { enc.PushVar(v26);  return; }
        enc.PushConst16(0); // fallback
    }

    // ─── variable-reference parser ────────────────────────────────────────────

    private static bool TryParseVarRef(ExpressionSyntax e, out Int26 result)
    {
        result = default;
        while (e is ParenthesizedExpressionSyntax pe) e = pe.Expression;

        // @prefix.TypeName_N[k]  — Bit
        if (e is ElementAccessExpressionSyntax ea)
        {
            if (!TryMemberAccess(ea.Expression, out var src, out var tn, out int n)) return false;
            if (ea.ArgumentList.Arguments.Count != 1) return false;
            if (!TryParseConst(ea.ArgumentList.Arguments[0].Expression, out long kl)) return false;
            int k = (int)kl;
            int bitOfs = n * GetTypeBytes(tn) * 8 + k;
            result = new Int26(bitOfs, src, Jsm.Expression.VariableType.Bit);
            return true;
        }
        // @prefix.TypeName_N
        if (TryMemberAccess(e, out var src2, out var tn2, out int idx2))
        {
            result = new Int26(idx2, src2, GetVarType(tn2));
            return true;
        }
        return false;
    }

    private static bool TryMemberAccess(ExpressionSyntax e,
        out Jsm.Expression.VariableSource src, out string typeName, out int index)
    {
        src = default; typeName = ""; index = 0;
        string pfx;
        string mn;
        if (e is MemberAccessExpressionSyntax ma)
        {
            pfx = (ma.Expression as IdentifierNameSyntax)?.Identifier.Text ?? "";
            mn = ma.Name.Identifier.Text;
        }
        else if (e is IdentifierNameSyntax identifier)
        {
            pfx = "@loc";
            mn = identifier.Identifier.Text;
        }
        else
        {
            return false;
        }
        src = pfx switch {
            "@var"            => Jsm.Expression.VariableSource.Global,
            "@loc"            => Jsm.Expression.VariableSource.Instance,
            "@evt" or "@map"  => Jsm.Expression.VariableSource.Map,
            "@sys"            => Jsm.Expression.VariableSource.System,
            _                 => (Jsm.Expression.VariableSource)99
        };
        if ((int)src == 99) return false;
        int us = mn.IndexOf('_');
        if (us <= 0) return false;
        typeName = mn.Substring(0, us);
        return int.TryParse(mn.Substring(us + 1), out index);
    }

    private static Jsm.Expression.VariableType GetVarType(string n) => n switch {
        "Byte"   => Jsm.Expression.VariableType.Byte,
        "SByte"  => Jsm.Expression.VariableType.SByte,
        "Int16"  => Jsm.Expression.VariableType.Int16,
        "UInt16" => Jsm.Expression.VariableType.UInt16,
        "Int24"  => Jsm.Expression.VariableType.Int24,
        "UInt24" => Jsm.Expression.VariableType.UInt24,
        "Bit"    => Jsm.Expression.VariableType.Bit,
        _        => Jsm.Expression.VariableType.Byte
    };

    private static int GetTypeBytes(string n) => n switch {
        "Int16" or "UInt16" => 2,
        "Int24" or "UInt24" => 3,
        _                   => 1
    };

    private static bool TryParseConst(ExpressionSyntax e, out long v)
    {
        v = 0;
        while (e is ParenthesizedExpressionSyntax pe) e = pe.Expression;
        if (e is LiteralExpressionSyntax lit) {
            if (lit.IsKind(SyntaxKind.TrueLiteralExpression))  { v = 1; return true; }
            if (lit.IsKind(SyntaxKind.FalseLiteralExpression)) { v = 0; return true; }
            if (lit.Token.Value is string text)
            {
                Match numericSuffix = Regex.Match(text, @"(-?\d+)$");
                if (numericSuffix.Success)
                    return long.TryParse(numericSuffix.Groups[1].Value, NumberStyles.Integer,
                        CultureInfo.InvariantCulture, out v);
                return false;
            }
            if (lit.Token.Value != null)
            {
                try { v = Convert.ToInt64(lit.Token.Value, CultureInfo.InvariantCulture); return true; }
                catch (FormatException) { return false; }
            }
        }
        if (e is MemberAccessExpressionSyntax member &&
            long.TryParse(member.Name.Identifier.ValueText, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out v))
            return true;
        Match enumNumber = Regex.Match(e.ToString(), @"\.(-?\d+)$");
        if (enumNumber.Success && long.TryParse(enumNumber.Groups[1].Value,
                NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
            return true;
        if (e is PrefixUnaryExpressionSyntax neg &&
            neg.IsKind(SyntaxKind.UnaryMinusExpression) &&
            neg.Operand is LiteralExpressionSyntax nl)
        { v = -Convert.ToInt64(nl.Token.Value); return true; }
        return false;
    }

    // ─── helpers ─────────────────────────────────────────────────────────────

    private static (ExpressionSyntax cond, bool negated) UnwrapNot(ExpressionSyntax e)
    {
        while (e is ParenthesizedExpressionSyntax pe) e = pe.Expression;
        if (e is PrefixUnaryExpressionSyntax neg &&
            neg.IsKind(SyntaxKind.LogicalNotExpression))
        {
            var inner = neg.Operand;
            while (inner is ParenthesizedExpressionSyntax pi) inner = pi.Expression;
            return (inner, true);
        }
        return (e, false);
    }

    private static BlockSyntax WrapInBlock(StatementSyntax s)
        => SyntaxFactory.Block(s);
}

// ─── Int26 encoder ───────────────────────────────────────────────────────────

internal sealed class ExprEnc
{
    private readonly List<byte> _b = new();

    public void Op(byte code) => _b.Add(code);

    public void PushVar(Int26 v)
    {
        // Compact format: negative byte header + 1-2 value bytes
        int src  = (int)v.Source & 3;
        int type = (int)v.Type;
        int val  = v.Value;
        bool ext = val > 0xFF;
        byte h = (byte)(0x80 | (ext ? 0x20 : 0) | ((type & 7) << 2) | (src & 3));
        _b.Add(h);
        _b.Add((byte)(val & 0xFF));
        if (ext) _b.Add((byte)((val >> 8) & 0xFF));
    }

    public void PushConst16(short v)
    {
        _b.Add((byte)Jsm.Excode.Const16);
        _b.Add((byte)(v & 0xFF));
        _b.Add((byte)((v >> 8) & 0xFF));
    }

    public byte[] ToArray() => _b.ToArray();
}

// ─── bytecode emit context ───────────────────────────────────────────────────

internal sealed class CompileCtx
{
    private readonly List<byte>   _b    = new();
    private readonly Dictionary<string, string> _svc = new(StringComparer.Ordinal);
    private readonly HashSet<int> _jumpTargets = new();
    private readonly List<(int Slot, int Target, int Base)> _jumpPatches = new();
    private readonly List<(int Slot, StatementSyntax Target)> _deferredJumps = new();
    private int? _pendingJumpTarget;
    public int LastInstructionStart { get; private set; } = -1;
    public Jsm.Opcode? LastInstructionOpcode { get; private set; }
    public StatementSyntax? NextStatement { get; set; }

    // Per-instruction label tracking (byte position of each instruction's start)
    private readonly List<int> _bodyLabels = new();
    private bool _inBody;

    public int Position => _b.Count;

    public void AddService(string sh, string full) => _svc[sh] = full;
    public string ResolveService(string sh)
    {
        if (_svc.TryGetValue(sh, out var f)) return f;
        return sh.TrimStart('@');
    }

    public void StartBody() { _inBody = true;  _bodyLabels.Clear(); }
    public int  EndBody()   { _inBody = false;
        return _bodyLabels.Count > 0 ? _bodyLabels[_bodyLabels.Count - 1] : Position; }

    /// <summary>Records that an instruction started at byte position 'pos'.</summary>
    public void RecordLabel(int pos)
    {
        LastInstructionStart = pos;
        LastInstructionOpcode = null;
        if (_inBody) _bodyLabels.Add(pos);
    }
    public void RecordInstructionOpcode(Jsm.Opcode opcode) => LastInstructionOpcode = opcode;

    public void EmitOpcode(Jsm.Opcode op) => _b.Add((byte)op);
    public void WriteByte(byte x)  => _b.Add(x);
    public void WriteInt16(short v)  { _b.Add((byte)(v & 0xFF)); _b.Add((byte)((v >> 8) & 0xFF)); }
    public void WriteInt32(int v)    { _b.Add((byte)v); _b.Add((byte)(v>>8)); _b.Add((byte)(v>>16)); _b.Add((byte)(v>>24)); }
    public void WriteBytes(byte[] d) => _b.AddRange(d);

    public void PatchInt16(int slot, short v)
    {
        _b[slot] = (byte)(v & 0xFF);
        _b[slot+1] = (byte)((v>>8) & 0xFF);
        if (_pendingJumpTarget is int target)
        {
            _jumpPatches.Add((slot, target, target - v));
            _pendingJumpTarget = null;
        }
    }
    public void RecordJumpTarget(int position)
    {
        _jumpTargets.Add(position);
        _pendingJumpTarget = position;
    }
    public bool IsJumpTarget(int position) => _jumpTargets.Contains(position);
    public void DeferRelativeJump(int slot, StatementSyntax target) =>
        _deferredJumps.Add((slot, target));
    public void ResolveDeferredJumps(StatementSyntax statement)
    {
        for (int i = _deferredJumps.Count - 1; i >= 0; i--)
        {
            var deferred = _deferredJumps[i];
            if (!ReferenceEquals(deferred.Target, statement))
                continue;
            RecordJumpTarget(Position);
            PatchInt16(deferred.Slot, checked((short)(Position - (deferred.Slot + 2))));
            _deferredJumps.RemoveAt(i);
        }
    }
    public void Retarget(int oldTarget, int newTarget)
    {
        for (int i = 0; i < _jumpPatches.Count; i++)
        {
            var patch = _jumpPatches[i];
            if (patch.Target != oldTarget) continue;
            short value = checked((short)(newTarget - patch.Base));
            _b[patch.Slot] = (byte)(value & 0xFF);
            _b[patch.Slot + 1] = (byte)((value >> 8) & 0xFF);
            _jumpPatches[i] = (patch.Slot, newTarget, patch.Base);
        }
        _jumpTargets.Remove(oldTarget);
        _jumpTargets.Add(newTarget);
    }
    public void PatchByte(int slot, byte value) => _b[slot] = value;

    public byte[] GetBytes() => _b.ToArray();
}

internal sealed class InstructionSchema
{
    private static readonly Dictionary<Jsm.Opcode, InstructionSchema> Cache = new();

    public bool HasArgumentMask { get; }
    public int[] Widths { get; }

    private InstructionSchema(bool hasArgumentMask, int[] widths)
    {
        HasArgumentMask = hasArgumentMask;
        Widths = widths;
    }

    public static InstructionSchema For(Jsm.Opcode opcode)
    {
        lock (Cache)
        {
            if (!Cache.TryGetValue(opcode, out InstructionSchema? schema))
                Cache.Add(opcode, schema = Build(opcode));
            return schema;
        }
    }

    private static InstructionSchema Build(Jsm.Opcode opcode)
    {
        Assembly assembly = typeof(JsmInstruction).Assembly;
        Type? type = assembly.GetType($"FF8.JSM.Instructions.{opcode}");
        MethodInfo? factory = type?.GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
        byte[]? il = factory?.GetMethodBody()?.GetILAsByteArray();
        if (factory == null || il == null)
            throw new NotSupportedException($"Opcode {opcode} has no readable instruction factory.");

        var calls = new List<string>();
        for (int i = 0; i + 4 < il.Length; i++)
        {
            if (il[i] != 0x28 && il[i] != 0x6F) // call / callvirt
                continue;
            int token = BitConverter.ToInt32(il, i + 1);
            try
            {
                if (factory.Module.ResolveMethod(token) is MethodBase called &&
                    called.DeclaringType == typeof(JsmInstructionReader))
                    calls.Add(called.Name);
            }
            catch (ArgumentException)
            {
                // The byte happened to look like a call opcode inside another operand.
            }
        }

        bool raw = calls.Contains(nameof(JsmInstructionReader.Arguments));
        int[] widths = calls.Select(name => name switch
        {
            nameof(JsmInstructionReader.ArgumentByte) or nameof(JsmInstructionReader.Arguments) or
                nameof(JsmInstructionReader.ReadByte) => 1,
            nameof(JsmInstructionReader.ArgumentInt16) or nameof(JsmInstructionReader.ReadInt16) or
                nameof(JsmInstructionReader.ReadUInt16) => 2,
            nameof(JsmInstructionReader.ArgumentInt24) or nameof(JsmInstructionReader.ReadInt24) => 3,
            _ => 0
        }).Where(width => width != 0).ToArray();

        return new InstructionSchema(!raw && widths.Length > 0, widths);
    }
}

// ─── instruction info table ──────────────────────────────────────────────────

internal readonly struct ArgDef { public readonly string Name; public readonly int Width;
    public ArgDef(string n, int w) { Name=n; Width=w; } }
internal readonly struct InstrInfo { public readonly Jsm.Opcode Opcode; public readonly ArgDef[] Args;
    public InstrInfo(Jsm.Opcode op, params ArgDef[] args) { Opcode=op; Args=args; } }

internal static class InstrTable
{
    // (ServiceFullName, MethodName) → InstrInfo
    private static readonly Dictionary<(string,string), InstrInfo> BySvcMethod
        = new()
    {
        // Messages service
        {("Messages","ShowAndWait"), new InstrInfo(Jsm.Opcode.MES,
            new ArgDef("windowId",1), new ArgDef("ui",1), new ArgDef("text",2))},
        {("Messages","Show"), new InstrInfo(Jsm.Opcode.MESN,
            new ArgDef("windowId",1), new ArgDef("ui",1), new ArgDef("text",2))},
        {("Messages","Wait"), new InstrInfo(Jsm.Opcode.WAITMES,
            new ArgDef("_windowId",1))},
    };

    // Instruction class name → InstrInfo (for default-formatted instructions)
    private static readonly Dictionary<string, InstrInfo> ByClassName = new()
    {
        {"REQ",    new InstrInfo(Jsm.Opcode.REQ,
            new ArgDef("_scriptLevel",1), new ArgDef("_entry",1), new ArgDef("_function",1))},
        {"REQSW",  new InstrInfo(Jsm.Opcode.REQSW,
            new ArgDef("_scriptLevel",1), new ArgDef("_entry",1), new ArgDef("_function",1))},
        {"REQEW",  new InstrInfo(Jsm.Opcode.REQEW,
            new ArgDef("_scriptLevel",1), new ArgDef("_entry",1), new ArgDef("_function",1))},
        {"AICON",  new InstrInfo(Jsm.Opcode.AICON,  new ArgDef("_modeFlags",1))},
        {"NOP",    new InstrInfo(Jsm.Opcode.NOP)},
        {"DELETE", new InstrInfo(Jsm.Opcode.DELETE, new ArgDef("_entry",1))},
        {"NEW",    new InstrInfo(Jsm.Opcode.NEW,    new ArgDef("_entry",1), new ArgDef("_uid",1))},
        {"NEW2",   new InstrInfo(Jsm.Opcode.NEW2,   new ArgDef("_entry",1), new ArgDef("_uid",1))},
        {"NEW3",   new InstrInfo(Jsm.Opcode.NEW3,   new ArgDef("_entry",1), new ArgDef("_uid",1))},
        {"CLOSE",  new InstrInfo(Jsm.Opcode.CLOSE,  new ArgDef("_windowId",1))},
        {"WAIT",   new InstrInfo(Jsm.Opcode.WAIT,   new ArgDef("_frameDuration",1))},
        {"MOVE",   new InstrInfo(Jsm.Opcode.MOVE,
            new ArgDef("_x",2), new ArgDef("_y",2), new ArgDef("_z",2))},
        {"MOVA",   new InstrInfo(Jsm.Opcode.MOVA,   new ArgDef("_entry",1))},
        {"MODEL",  new InstrInfo(Jsm.Opcode.MODEL,  new ArgDef("_model",2), new ArgDef("_height",1))},
        {"POS",    new InstrInfo(Jsm.Opcode.POS,    new ArgDef("_x",2), new ArgDef("_y",2))},
        {"UCON",   new InstrInfo(Jsm.Opcode.UCON)},
        {"UCOFF",  new InstrInfo(Jsm.Opcode.UCOFF)},
    };

    public static bool TryGet(string svc, string method, out InstrInfo info)
        => BySvcMethod.TryGetValue((svc, method), out info);

    public static bool TryGetByName(string className, out InstrInfo info)
        => ByClassName.TryGetValue(className, out info);
}
