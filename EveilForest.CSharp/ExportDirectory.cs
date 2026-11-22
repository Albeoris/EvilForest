using System.Security.Cryptography;

namespace EveilForest.CSharp;

public static class ExportDirectory
{
    public static void Publish(string directory, Action<string> generate)
    {
        string destination = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
        if (File.Exists(destination) || Directory.Exists(destination) && Directory.EnumerateFileSystemEntries(destination).Any())
            throw new InvalidDataException($"Export target '{destination}' is populated. Choose a fresh directory to preserve the project.");

        string parent = Path.GetDirectoryName(destination) ?? throw new ArgumentException("An export cannot target a filesystem root.");
        Directory.CreateDirectory(parent);
        string stage = Path.Combine(parent, $".evilforest-export-{Guid.NewGuid():N}");
        Directory.CreateDirectory(stage);
        try
        {
            generate(stage);
            const string manifest = ".evilforest-export.tsv";
            string[] files = Directory.EnumerateFiles(stage, "*", SearchOption.AllDirectories)
                .OrderBy(file => file, StringComparer.Ordinal).ToArray();
            File.WriteAllLines(Path.Combine(stage, manifest), files.Select(file =>
            {
                using var input = File.OpenRead(file);
                return $"{Convert.ToHexString(SHA256.HashData(input))}\t{Path.GetRelativePath(stage, file).Replace('\\', '/')}";
            }));
            if (Directory.Exists(destination))
                Directory.Delete(destination);
            for (int retry = 0; ; retry++)
            {
                try
                {
                    Directory.Move(stage, destination);
                    break;
                }
                catch (Exception error) when (OperatingSystem.IsWindows() && retry < 4 &&
                    (error is IOException or UnauthorizedAccessException) && !File.Exists(destination) && !Directory.Exists(destination))
                {
                    Thread.Sleep(50 * (retry + 1));
                }
            }
        }
        finally
        {
            if (Directory.Exists(stage))
                Directory.Delete(stage, recursive: true);
        }
    }
}
