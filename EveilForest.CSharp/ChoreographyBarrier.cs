using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal static class ChoreographyBarrier
{
    internal static StatementSyntax[] Begin(string counter, int count, string engine = "Engine") =>
    [
        SyntaxFactory.ParseStatement($"{counter} = {count};"),
        SyntaxFactory.ParseStatement($"await {engine}.Wait(frames: 1);")
    ];

    internal static StatementSyntax[] Complete(string counter, string pulse, string cue, string engine = "Engine") =>
    [
        SyntaxFactory.ParseStatement($"{counter}--;"),
        SyntaxFactory.ParseStatement($"for (; {counter} > 0;) {{ {pulse} = true; await {engine}.Wait(frames: 1); }}"),
        SyntaxFactory.ParseStatement($"for (; {pulse} == true;) {{ await {engine}.Wait(frames: 1); }}"),
        Signal(cue)
    ];

    internal static StatementSyntax Signal(string cue) => SyntaxFactory.ParseStatement($"{cue} = true;");
}
