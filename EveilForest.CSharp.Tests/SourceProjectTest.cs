using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace EveilForest.CSharp.Tests;

internal static class SourceProjectTest
{
    internal static string ProjectFile(string directory) => Path.Combine(directory, "Event.csproj");

    internal static MetadataReference[] EventReferences(string directory) => new[]
    {
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll")),
        MetadataReference.CreateFromFile(Path.Combine(directory, "lib", "EvilForest.EventSource.dll"))
    };

    internal static void WriteProject(string directory)
    {
        File.WriteAllText(ProjectFile(directory), """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>
            </Project>
            """);
    }

    internal static string CopyWithoutComments(string source)
    {
        string destination = source + "-stripped";
        Directory.CreateDirectory(destination);
        foreach (string file in SourceFiles(source))
        {
            string target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            if (Path.GetExtension(file) is ".csproj" or ".dll")
                File.Copy(file, target);
            if (Path.GetExtension(file) != ".cs")
                continue;

            SyntaxNode root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
            SyntaxTrivia[] comments = root.DescendantTrivia(descendIntoTrivia: true).Where(IsComment).ToArray();
            SyntaxNode stripped = root.ReplaceTrivia(comments, (trivia, _) => SyntaxFactory.Whitespace(
                new string(trivia.ToFullString().Select(character => character is '\r' or '\n' ? character : ' ').ToArray())));
            Assert.DoesNotContain(CSharpSyntaxTree.ParseText(stripped.ToFullString()).GetRoot().DescendantTrivia(descendIntoTrivia: true), IsComment);
            File.WriteAllText(target, stripped.ToFullString());
        }
        return destination;
    }

    private static bool IsComment(SyntaxTrivia trivia) => trivia.Kind() is SyntaxKind.SingleLineCommentTrivia or
        SyntaxKind.MultiLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia or SyntaxKind.MultiLineDocumentationCommentTrivia;

    private static IEnumerable<string> SourceFiles(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory))
        {
            if (Path.GetExtension(file) is ".cs" or ".csproj" or ".dll")
                yield return file;
        }

        foreach (string child in Directory.EnumerateDirectories(directory))
        {
            if (Path.GetFileName(child) is "bin" or "obj" || Path.GetFileName(child).StartsWith('.'))
                continue;
            foreach (string file in SourceFiles(child))
                yield return file;
        }
    }

    internal static string RunDotnet(string[] arguments, int expectedExit = 0)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.Environment["DOTNET_NOLOGO"] = "1";
        start.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        start.Environment["DOTNET_PROCESSOR_COUNT"] = "2";
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> stdout = process.StandardOutput.ReadToEndAsync();
        Task<string> stderr = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(120_000))
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException($"dotnet {string.Join(" ", arguments)} timed out.");
        }
        string output = stdout.GetAwaiter().GetResult() + stderr.GetAwaiter().GetResult();
        if (process.ExitCode != expectedExit)
            throw new InvalidDataException($"dotnet {string.Join(" ", arguments)} exited with {process.ExitCode}: {output}");
        return output;
    }

}
