using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

public static class EngineStorageReport
{
    public static int Write(string sourceDirectory, string reportPath)
    {
        using var report = new StreamWriter(reportPath);
        report.WriteLine("scope\ttype\taddress\tunit\twidthBits\tsigned\taccess\tfile\tline\treference\tsource");
        int count = 0;
        foreach (string file in Directory.EnumerateFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
        {
            string source = File.ReadAllText(file);
            if (!source.Contains("gEventGlobal", StringComparison.Ordinal) && !source.Contains("VarManually", StringComparison.Ordinal)
                && !source.Contains("ScenarioCounter", StringComparison.Ordinal) && !source.Contains("FieldEntrance", StringComparison.Ordinal))
                continue;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            string[] lines = source.ReplaceLineEndings("\n").Split('\n');
            foreach (SyntaxNode node in tree.GetRoot().DescendantNodes())
            {
                switch (node)
                {
                    case ElementAccessExpressionSyntax element when IsGlobalBuffer(element.Expression) && element.ArgumentList.Arguments.Count == 1:
                        ExpressionSyntax index = element.ArgumentList.Arguments[0].Expression;
                        Add(element, Integer(index) is int offset ? new EVVariable(EVStorageScope.Global, EVStorageType.Byte, offset) : null, AccessOf(element), EVStorageScope.Global);
                        break;
                    case MemberAccessExpressionSyntax member when LastName(member.Expression) == "EventState":
                        if (member.Name.Identifier.ValueText == "ScenarioCounter")
                            Add(member, new EVVariable(EVStorageScope.Global, EVStorageType.UInt16, 0), AccessOf(member), EVStorageScope.Global);
                        if (member.Name.Identifier.ValueText == "FieldEntrance")
                            Add(member, new EVVariable(EVStorageScope.Global, EVStorageType.UInt16, 2), AccessOf(member), EVStorageScope.Global);
                        break;
                    case InvocationExpressionSyntax call when LastName(call.Expression) is "getVarManually" or "setVarManually":
                        if (call.ArgumentList.Arguments.Count > 0)
                            Add(call, ResolvePacked(call.ArgumentList.Arguments[0].Expression), LastName(call.Expression) == "getVarManually" ? "Read" : "Write");
                        break;
                    case InvocationExpressionSyntax call when LastName(call.Expression) is "GetVariableValueInternal" or "SetVariableValueInternal":
                        var arguments = call.ArgumentList.Arguments;
                        if (arguments.Count >= 3 && IsGlobalBuffer(arguments[0].Expression))
                        {
                            EVVariable? view = Integer(arguments[1].Expression) is int address &&
                                Enum.TryParse(LastName(arguments[2].Expression), out EVStorageType type)
                                ? new EVVariable(EVStorageScope.Global, type, address) : null;
                            Add(call, view, LastName(call.Expression) == "GetVariableValueInternal" ? "Read" : "Write", EVStorageScope.Global);
                        }
                        break;
                }
            }

            void Add(SyntaxNode node, EVVariable? view, string access, EVStorageScope? knownScope = null)
            {
                int line = node.GetLocation().GetLineSpan().StartLinePosition.Line;
                string physical = view is EVVariable value
                    ? $"{value.Scope}\t{value.Type}\t{value.Address}\t{(value.UsesBitAddress ? "bit" : "byte")}\t{value.Width}\t{value.IsSigned}"
                    : $"{knownScope?.ToString() ?? "Unresolved"}\t\t\t\t\t";
                report.WriteLine($"{physical}\t{access}\t{Path.GetRelativePath(sourceDirectory, file)}\t{line + 1}\t{ForCell(node.ToString())}\t{ForCell(lines[line])}");
                count++;
            }
        }
        return count;
    }

    private static EVVariable? ResolvePacked(ExpressionSyntax expression)
    {
        if (expression is InvocationExpressionSyntax call && LastName(call.Expression) == "getVarOperation" && call.ArgumentList.Arguments.Count == 3)
        {
            var arguments = call.ArgumentList.Arguments;
            if (Enum.TryParse(LastName(arguments[0].Expression), out EVStorageScope scope) &&
                Enum.TryParse(LastName(arguments[1].Expression), out EVStorageType type) && Integer(arguments[2].Expression) is int offset)
                return new EVVariable(scope, type, offset);
            return null;
        }

        int? command = LastName(expression) switch
        {
            "SC_COUNTER_SVR" => 0xDC,
            "MAP_INDEX_SVR" => 0x2D8,
            _ => Integer(expression)
        };
        if (command is not int packed)
            return null;
        int address = (packed >> 8) & ((packed & 0x20) == 0 ? 0xFF : 0xFFFF);
        return new EVVariable((EVStorageScope)(packed & 3), (EVStorageType)((packed >> 2) & 7), address);
    }

    private static string AccessOf(ExpressionSyntax access) => access.Parent switch
    {
        AssignmentExpressionSyntax assignment when assignment.Left == access => assignment.IsKind(SyntaxKind.SimpleAssignmentExpression) ? "Write" : "ReadWrite",
        PrefixUnaryExpressionSyntax prefix when prefix.IsKind(SyntaxKind.PreIncrementExpression) || prefix.IsKind(SyntaxKind.PreDecrementExpression) => "ReadWrite",
        PostfixUnaryExpressionSyntax postfix when postfix.IsKind(SyntaxKind.PostIncrementExpression) || postfix.IsKind(SyntaxKind.PostDecrementExpression) => "ReadWrite",
        _ => "Read"
    };

    private static bool IsGlobalBuffer(ExpressionSyntax expression) => LastName(expression) == "gEventGlobal";

    private static string? LastName(ExpressionSyntax expression) => expression switch
    {
        IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
        MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
        _ => null
    };

    private static int? Integer(ExpressionSyntax expression) =>
        expression is LiteralExpressionSyntax { Token.Value: int value } && value >= 0 ? value : null;

    private static string ForCell(string source) => source.Trim().ReplaceLineEndings(" ").Replace('\t', ' ');
}
