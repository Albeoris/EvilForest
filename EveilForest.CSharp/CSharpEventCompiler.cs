using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EvilForest.Resources.Enums;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        int id = ParseObjectId(filePath);
        string code = File.ReadAllText(filePath);
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code, path: filePath);
        Diagnostic? error = tree.GetDiagnostics().FirstOrDefault(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        if (error != null)
            throw new InvalidDataException(error.ToString());

        var root = (CompilationUnitSyntax)tree.GetRoot();
        ClassDeclarationSyntax? cls = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
        if (cls == null)
            throw Unsupported(root, "object declaration");

        byte varCount = ComputeVarCount(cls);
        byte flags = ReadByteConstant(cls, "__Flags") ?? 0;
        EVScript[] scripts = CompileScripts(cls).ToArray();
        return new EVObject(id, varCount, flags, scripts);
    }

    // ─── variable-count scanner ───────────────────────────────────────────────

    private static byte ComputeVarCount(ClassDeclarationSyntax cls)
    {
        if (ReadByteConstant(cls, "__VariableCount") is byte declared)
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
                "Int16" or "UInt16" => n + 2,
                "Int24" or "UInt24" => n + 3,
                "Int32" or "UInt32" => n + 4,
                "Bit" or "Boolean" => n / 8 + 1,
                "Byte" or "SByte" or "BitByte" or "SBitByte" => n + 1,
                _ => 0
            };
            if (byteEnd > maxEnd)
                maxEnd = byteEnd;
        }
        return checked((byte)maxEnd);
    }

    private static byte? ReadByteConstant(ClassDeclarationSyntax cls, string name)
    {
        VariableDeclaratorSyntax? variable = cls.Members.OfType<FieldDeclarationSyntax>()
            .SelectMany(field => field.Declaration.Variables)
            .FirstOrDefault(candidate => candidate.Identifier.ValueText == name);
        if (variable == null)
            return null;
        if (variable.Initializer?.Value is not ExpressionSyntax expression || !TryParseConst(expression, out long value) ||
            value < byte.MinValue || value > byte.MaxValue)
            throw Unsupported(variable, "byte metadata declaration");
        return (byte)value;
    }

    // ─── script compiler ─────────────────────────────────────────────────────

    private IEnumerable<EVScript> CompileScripts(ClassDeclarationSyntax cls)
    {
        foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
        {
            int sid = GetScriptId(method);
            if (sid < 0 || sid > ushort.MaxValue || method.Body == null)
                throw Unsupported(method, "script declaration");

            var ctx = new CompileCtx();
            CollectServiceAliases(method.Body, ctx);
            CompileBlock(method.Body, ctx);
            byte[] bytecode = ctx.GetBytes();
            yield return new EVScript((ushort)sid, EVInstructionBody.Decode(bytecode));
        }
    }

    private static int GetScriptId(MethodDeclarationSyntax m)
    {
        string name = m.Identifier.Text;
        if (name == "Init")
            return 0;
        if (name.StartsWith("Script_", StringComparison.Ordinal) && int.TryParse(name.Substring(7), out int sid))
            return sid;
        return name switch { "OnLoop" => 1, "OnEnter" => 2, "OnExit" => 3, _ => -1 };
    }

    private static void CollectServiceAliases(BlockSyntax? body, CompileCtx ctx)
    {
        if (body == null)
            return;
        foreach (var stmt in body.Statements.OfType<LocalDeclarationStatementSyntax>())
        {
            foreach (var decl in stmt.Declaration.Variables)
            {
                string alias = decl.Identifier.Text;
                if (IsServiceAlias(decl) &&
                    decl.Initializer?.Value is ElementAccessExpressionSyntax { Expression: MemberAccessExpressionSyntax member })
                    ctx.AddService(alias, member.Name.Identifier.Text);
            }
        }
    }

    private static bool IsServiceAlias(VariableDeclaratorSyntax declaration) =>
        declaration.Initializer?.Value is ElementAccessExpressionSyntax
        {
            Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax service },
            ArgumentList.Arguments.Count: 1
        } access &&
        service.Identifier.ValueText == "ServiceId" &&
        access.ArgumentList.Arguments[0].Expression is IdentifierNameSyntax context &&
        context.Identifier.ValueText == "ctx";

    // ─── statement dispatcher ────────────────────────────────────────────────

    private void CompileBlock(BlockSyntax? body, CompileCtx ctx)
    {
        if (body == null)
            return;
        foreach (StatementSyntax statement in body.Statements)
            CompileStmt(statement, ctx);
    }

    private void CompileLoop(WhileStatementSyntax loop, CompileCtx ctx)
    {
        var start = new CompileCtx.Label();
        var end = new CompileCtx.Label();
        ctx.Mark(start);
        if (!IsTrue(loop.Condition))
        {
            EmitCondExpr(loop.Condition, ctx);
            ctx.Jump(Jsm.Opcode.JMP_IFN, end);
        }
        ctx.Breaks.Push(end);
        ctx.Continues.Push(start);
        CompileStmt(loop.Statement, ctx);
        ctx.Continues.Pop();
        ctx.Breaks.Pop();
        ctx.Jump(Jsm.Opcode.JMP, start);
        ctx.Mark(end);
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
                else
                    throw Unsupported(ys, "yield statement");
                break;
            case IfStatementSyntax ifS:
                CompileIf(ifS, ctx);
                break;
            case WhileStatementSyntax ws:
                CompileLoop(ws, ctx);
                break;
            case SwitchStatementSyntax ss:
                CompileSwitch(ss, ctx);
                break;
            case BlockSyntax blk:
                CompileBlock(blk, ctx);
                break;
            case LocalDeclarationStatementSyntax local when local.Declaration.Variables.All(IsServiceAlias):
            case EmptyStatementSyntax:
                break;
            case ReturnStatementSyntax { Expression: null }:
                ctx.EmitOpcode(Jsm.Opcode.Return);
                break;
            case BreakStatementSyntax when ctx.Breaks.TryPeek(out CompileCtx.Label? exit):
                ctx.Jump(Jsm.Opcode.JMP, exit);
                break;
            case ContinueStatementSyntax when ctx.Continues.TryPeek(out CompileCtx.Label? continuation):
                ctx.Jump(Jsm.Opcode.JMP, continuation);
                break;
            default:
                throw Unsupported(stmt, "statement");
        }
    }

    // ─── assignment: @prefix.TypeName_N[k] = value ───────────────────────────

    private static void CompileAssign(AssignmentExpressionSyntax asgn, CompileCtx ctx)
    {
        // Skip Init-method field assignments: @ctx = …, @evt = …
        if (asgn.Left is IdentifierNameSyntax lid &&
            asgn.IsKind(SyntaxKind.SimpleAssignmentExpression) &&
            asgn.Right is IdentifierNameSyntax binding &&
            ((lid.Identifier.Text == "@ctx" && binding.Identifier.ValueText == "executionContext") ||
             (lid.Identifier.Text == "@evt" && binding.Identifier.ValueText == "evt")))
            return;

        if (!TryParseVarRef(asgn.Left, out var v26))
            throw Unsupported(asgn.Left, "assignment target");

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
            _                                           => throw Unsupported(asgn, "assignment")
        });
        if (!asgn.IsKind(SyntaxKind.SimpleAssignmentExpression))
            enc.Op((byte)Jsm.Excode.Let);
        enc.Op((byte)Jsm.Excode.End);

        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(enc.ToArray());
    }

    private static void CompileStandaloneUnary(ExpressionSyntax operand, Jsm.Excode opcode,
        CompileCtx ctx)
    {
        if (!TryParseVarRef(operand, out Int26 variable))
            throw Unsupported(operand, "increment or decrement target");
        var encoded = new ExprEnc();
        encoded.PushVar(variable);
        encoded.Op((byte)opcode);
        encoded.Op((byte)Jsm.Excode.End);
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(encoded.ToArray());
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
        if (!TryResolveOpcode(invocation, ctx, out Jsm.Opcode opcode))
        {
            throw Unsupported(invocation, kind);
        }

        List<ExpressionSyntax> encodedArguments = invocation.ArgumentList.Arguments
            .Select(argument => argument.Expression).ToList();
        string layout = EVOperands.For(opcode);
        bool hasMask = layout.StartsWith("m", StringComparison.Ordinal);
        string operands = hasMask ? layout.Substring(1) : layout;
        if (encodedArguments.Count != operands.Length)
            throw new InvalidDataException(
                $"Instruction {opcode} expects {operands.Length} encoded arguments, " +
                $"but '{invocation}' supplies {encodedArguments.Count}.");

        ctx.EmitOpcode(opcode);
        int maskSlot = -1;
        byte argumentMask = 0;
        if (hasMask)
        {
            maskSlot = ctx.Position;
            ctx.WriteByte(0);
        }

        for (int i = 0; i < operands.Length; i++)
        {
            ExpressionSyntax expression = encodedArguments[i];
            int width = EVOperands.Width(operands[i]);
            if (!TryParseConst(expression, out long v))
            {
                if (!hasMask || !EVOperands.UsesExpression(opcode, byte.MaxValue, i))
                    throw Unsupported(expression, $"operand {i + 1} of {opcode}");
                argumentMask |= (byte)(1 << i);
                var encoded = new ExprEnc();
                EncodeExpr(expression, encoded);
                encoded.Op((byte)Jsm.Excode.End);
                ctx.WriteBytes(encoded.ToArray());
                continue;
            }

            bool signed = EVOperands.IsSigned(operands[i]);
            long limit = 1L << (8 * width - (signed ? 1 : 0));
            if (v < (signed ? -limit : 0) || v >= limit)
                throw Unsupported(expression, $"operand {i + 1} of {opcode}");
            for (int shift = 0; shift < width * 8; shift += 8)
                ctx.WriteByte((byte)(v >> shift));
        }
        if (maskSlot >= 0)
            ctx.PatchByte(maskSlot, argumentMask);
    }

    private static InvalidDataException Unsupported(SyntaxNode node, string kind)
    {
        FileLinePositionSpan span = node.GetLocation().GetLineSpan();
        return new InvalidDataException(
            $"Unsupported {kind} at {span.Path}:{span.StartLinePosition.Line + 1}:{span.StartLinePosition.Character + 1}: '{node}'. A compiler must never silently alter it.");
    }

    private static bool TryResolveOpcode(InvocationExpressionSyntax invocation, CompileCtx ctx,
        out Jsm.Opcode opcode)
    {
        if (invocation.Expression is IdentifierNameSyntax identifier &&
            Enum.TryParse(identifier.Identifier.ValueText, out opcode))
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

    private void CompileIf(IfStatementSyntax statement, CompileCtx ctx)
    {
        var alternative = new CompileCtx.Label();
        EmitCondExpr(statement.Condition, ctx);
        ctx.Jump(Jsm.Opcode.JMP_IFN, alternative);
        CompileStmt(statement.Statement, ctx);
        if (statement.Else == null)
        {
            ctx.Mark(alternative);
            return;
        }

        var end = new CompileCtx.Label();
        ctx.Jump(Jsm.Opcode.JMP, end);
        ctx.Mark(alternative);
        CompileStmt(statement.Else.Statement, ctx);
        ctx.Mark(end);
    }

    private void CompileSwitch(SwitchStatementSyntax statement, CompileCtx ctx)
    {
        var cases = new List<(long Value, CompileCtx.Label Target)>();
        var sections = new Dictionary<SwitchSectionSyntax, CompileCtx.Label>();
        var end = new CompileCtx.Label();
        CompileCtx.Label fallback = end;
        foreach (SwitchSectionSyntax section in statement.Sections)
        {
            var target = new CompileCtx.Label();
            sections.Add(section, target);
            foreach (SwitchLabelSyntax label in section.Labels)
            {
                if (label is DefaultSwitchLabelSyntax)
                    fallback = target;
                else if (label is CaseSwitchLabelSyntax caseLabel && TryParseConst(caseLabel.Value, out long value))
                    cases.Add((value, target));
                else
                    throw Unsupported(label, "switch label");
            }
        }

        if (cases.Count == 0 || cases.Count > byte.MaxValue || cases.Select(item => item.Value).Distinct().Count() != cases.Count)
            throw Unsupported(statement, "switch case table");

        bool extended = cases[0].Value < short.MinValue || cases[0].Value > short.MaxValue;
        for (int i = 1; i < cases.Count; i++)
            extended |= cases[i].Value != cases[0].Value + i;
        if (extended && cases.Any(item => item.Value < 0 || item.Value > ushort.MaxValue))
            throw Unsupported(statement, "extended switch case value");

        EmitCondExpr(statement.Expression, ctx);
        ctx.EmitOpcode(extended ? Jsm.Opcode.JMP_SWITCHEX : Jsm.Opcode.JMP_SWITCH);
        int offsetBase = ctx.Position + (extended ? 3 : 0);
        ctx.WriteByte((byte)cases.Count);
        if (!extended)
            ctx.WriteInt16((short)cases[0].Value);
        ctx.WriteBranch(fallback, offsetBase, signed: false);
        foreach (var (value, target) in cases)
        {
            if (extended)
                ctx.WriteInt16(unchecked((short)value));
            ctx.WriteBranch(target, offsetBase, signed: false);
        }

        ctx.Breaks.Push(end);
        foreach (var (section, start) in sections)
        {
            ctx.Mark(start);
            foreach (StatementSyntax child in section.Statements)
                CompileStmt(child, ctx);
        }
        ctx.Breaks.Pop();
        ctx.Mark(end);
    }

    private static void EmitCondExpr(ExpressionSyntax cond, CompileCtx ctx)
    {
        var enc = new ExprEnc();
        EncodeExpr(cond, enc);
        enc.Op((byte)Jsm.Excode.End);
        ctx.EmitOpcode(Jsm.Opcode.EXPR);
        ctx.WriteBytes(enc.ToArray());
    }

    private static void EncodeExpr(ExpressionSyntax e, ExprEnc enc)
    {
        while (e is ParenthesizedExpressionSyntax p)
            e = p.Expression;

        if (e is CastExpressionSyntax { Type: PredefinedTypeSyntax target } cast && target.Keyword.IsKind(SyntaxKind.IntKeyword))
        {
            EncodeExpr(cast.Expression, enc);
            return;
        }
        if (e is PrefixUnaryExpressionSyntax constantUnary &&
            TryParseConst(constantUnary, out long constantValue))
        {
            enc.PushConstant(constantValue);
            return;
        }
        if (e is PrefixUnaryExpressionSyntax unary)
        {
            EncodeExpr(unary.Operand, enc);
            enc.Op(unary.Kind() switch
            {
                SyntaxKind.UnaryMinusExpression       => (byte)Jsm.Excode.B_SINGLE_MINUS,
                SyntaxKind.LogicalNotExpression       => (byte)Jsm.Excode.B_NOT,
                SyntaxKind.BitwiseNotExpression       => (byte)Jsm.Excode.B_COMP,
                SyntaxKind.PreIncrementExpression     => (byte)Jsm.Excode.B_PRE_PLUS,
                SyntaxKind.PreDecrementExpression     => (byte)Jsm.Excode.B_PRE_MINUS,
                _                                     => throw Unsupported(unary, "unary expression")
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
                _                                       => throw Unsupported(bin, "binary expression")
            });
            return;
        }
        if (e is LiteralExpressionSyntax && TryParseConst(e, out long literal))
        {
            enc.PushConstant(literal);
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
        if (TryParseVarRef(e, out var v26))
        {
            enc.PushVar(v26);
            return;
        }
        throw Unsupported(e, "expression");
    }

    // ─── variable-reference parser ────────────────────────────────────────────

    private static bool TryParseVarRef(ExpressionSyntax e, out Int26 result)
    {
        result = default;
        while (e is ParenthesizedExpressionSyntax pe)
            e = pe.Expression;

        // @prefix.TypeName_N[k]  — Bit
        if (e is ElementAccessExpressionSyntax ea)
        {
            if (!TryMemberAccess(ea.Expression, out var src, out var tn, out int n) ||
                ea.ArgumentList.Arguments.Count != 1 ||
                !TryParseConst(ea.ArgumentList.Arguments[0].Expression, out long bit) ||
                bit < 0 || bit >= GetTypeBytes(tn) * 8 || tn == "Bit")
                return false;
            int bitOfs = checked(n * 8 + (int)bit);
            if (bitOfs > ushort.MaxValue)
                return false;
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
        src = default;
        typeName = "";
        index = 0;
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
        if ((int)src == 99)
            return false;
        int us = mn.IndexOf('_');
        if (us <= 0)
            return false;
        typeName = mn.Substring(0, us);
        if (typeName is not ("Byte" or "SByte" or "Int16" or "UInt16" or "Int24" or "UInt24" or "Bit"))
            return false;
        return int.TryParse(mn.Substring(us + 1), out index) && index >= 0 && index <= ushort.MaxValue;
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
        while (e is ParenthesizedExpressionSyntax pe)
            e = pe.Expression;
        if (e is LiteralExpressionSyntax lit)
        {
            if (lit.IsKind(SyntaxKind.TrueLiteralExpression))
            {
                v = 1;
                return true;
            }
            if (lit.IsKind(SyntaxKind.FalseLiteralExpression))
            {
                v = 0;
                return true;
            }
            if (lit.Token.Value is int or uint or long or ulong)
            {
                v = Convert.ToInt64(lit.Token.Value, CultureInfo.InvariantCulture);
                return true;
            }
        }
        if (e is PrefixUnaryExpressionSyntax neg &&
            neg.IsKind(SyntaxKind.UnaryMinusExpression) &&
            TryParseConst(neg.Operand, out long magnitude))
        {
            v = checked(-magnitude);
            return true;
        }
        return false;
    }

    // ─── helpers ─────────────────────────────────────────────────────────────

}

internal sealed class ExprEnc
{
    private readonly List<byte> _b = new();

    public void Op(byte code) => _b.Add(code);

    public void PushVar(Int26 v)
    {
        EVExpressionToken token = new EVVariable((EVStorageScope)v.Source, (EVStorageType)v.Type, v.Value).Encode();
        _b.Add(token.Opcode);
        _b.Add((byte)token.Value);
        if ((token.Opcode & 0x20) != 0)
            _b.Add((byte)(token.Value >> 8));
    }

    public void PushConstant(long value)
    {
        if (value >= short.MinValue && value <= short.MaxValue)
        {
            _b.Add((byte)Jsm.Excode.Const16);
            _b.Add((byte)value);
            _b.Add((byte)(value >> 8));
            return;
        }

        if (value < -(1 << 25) || value >= (1 << 25))
            throw new InvalidDataException($"Value {value} does not fit a signed VM literal.");
        int bits = (int)value & 0x03FFFFFF;
        _b.Add((byte)Jsm.Excode.Const26);
        for (int shift = 0; shift < 32; shift += 8)
            _b.Add((byte)(bits >> shift));
    }

    public byte[] ToArray() => _b.ToArray();
}

// ─── bytecode emit context ───────────────────────────────────────────────────

internal sealed class CompileCtx
{
    private readonly List<byte> _bytes = new();
    private readonly Dictionary<string, string> _services = new(StringComparer.Ordinal);
    private readonly List<(int Slot, int Base, Label Target, bool Signed)> _branches = new();

    public sealed class Label
    {
        public int? Position { get; set; }
    }

    public Stack<Label> Breaks { get; } = new();
    public Stack<Label> Continues { get; } = new();
    public int Position => _bytes.Count;

    public void AddService(string alias, string name) => _services.Add(alias, name);

    public string ResolveService(string alias) => _services.GetValueOrDefault(alias) ?? alias.TrimStart('@');

    public void Mark(Label label) => label.Position = Position;

    public void Jump(Jsm.Opcode opcode, Label target)
    {
        EmitOpcode(opcode);
        WriteBranch(target, Position + 2, opcode != Jsm.Opcode.JMP_IFN);
    }

    public void WriteBranch(Label target, int offsetBase, bool signed)
    {
        _branches.Add((Position, offsetBase, target, signed));
        WriteInt16(0);
    }

    public void EmitOpcode(Jsm.Opcode opcode) => _bytes.Add((byte)opcode);

    public void WriteByte(byte value) => _bytes.Add(value);

    public void WriteInt16(short value)
    {
        _bytes.Add((byte)value);
        _bytes.Add((byte)(value >> 8));
    }

    public void WriteBytes(byte[] data) => _bytes.AddRange(data);

    public void PatchByte(int slot, byte value) => _bytes[slot] = value;

    public byte[] GetBytes()
    {
        foreach (var (slot, offsetBase, target, signed) in _branches)
        {
            int displacement = (target.Position ?? throw new InvalidDataException("Missing branch target.")) - offsetBase;
            if (signed ? displacement < short.MinValue || displacement > short.MaxValue : displacement < 0 || displacement > ushort.MaxValue)
                throw new InvalidDataException($"Branch displacement {displacement} does not fit its 16-bit operand.");
            _bytes[slot] = (byte)displacement;
            _bytes[slot + 1] = (byte)(displacement >> 8);
        }
        return _bytes.ToArray();
    }
}
