using System.Xml.Linq;

namespace EveilForest.CSharp;

internal static class EventSourceDependency
{
    private const string AssemblyName = "EvilForest.EventSource";
    private const string AssemblyFile = AssemblyName + ".dll";

    public static string CopyTo(string directory)
    {
        Directory.CreateDirectory(directory);
        string destination = Path.Combine(directory, AssemblyFile);
        File.Copy(typeof(global::EvilForest.EventSource.EventLayoutAttribute).Assembly.Location, destination, overwrite: true);
        return destination;
    }

    public static string? FindForProject(string directory)
    {
        string project = Path.Combine(directory, "Event.csproj");
        if (!File.Exists(project))
            return null;

        XDocument document = XDocument.Load(project);
        XElement? reference = document.Descendants().SingleOrDefault(element => element.Name.LocalName == "Reference" &&
            (string?)element.Attribute("Include") == AssemblyName);
        if (reference == null)
            return null;

        string? hint = reference.Elements().SingleOrDefault(element => element.Name.LocalName == "HintPath")?.Value;
        if (string.IsNullOrWhiteSpace(hint))
            throw new InvalidDataException("Event.csproj must specify a HintPath for the EvilForest.EventSource reference.");
        string path = Path.GetFullPath(hint, Path.GetFullPath(directory));
        if (!File.Exists(path))
            throw new InvalidDataException($"EventSource dependency is missing at {path}.");
        return path;
    }
}
