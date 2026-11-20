using System.Text;
using EvilForest.Resources;

namespace EveilForest.CSharp;

internal static class ResourceCatalog
{
    private static readonly IReadOnlyDictionary<(string Name, int Id), string> Members = Build();

    public static string Qualify(string name, int id) => $"Resources.{Members[(name, id)]}";

    public static void Append(StringBuilder source)
    {
        source.AppendLine("public static class Resources");
        source.AppendLine("{");
        foreach (var entry in Members.OrderBy(entry => entry.Value, StringComparer.Ordinal))
            source.AppendLine($"    public const int {entry.Value} = {entry.Key.Id};");
        source.AppendLine("}");
    }

    private static IReadOnlyDictionary<(string Name, int Id), string> Build()
    {
        var result = new Dictionary<(string Name, int Id), string>();
        var names = new HashSet<string>(StringComparer.Ordinal);
        var entries = DB.Models.Values.Select(model => (model.FileName, (int)model.EntryId))
            .Concat(DB.Animations.Values.Select(animation => (animation.FileName, (int)animation.EntryId)))
            .Concat(DB.Events.Values.Select(evt => (evt.FileName, (int)evt.EntryId)))
            .Concat(DB.Music.Values.Select(music => (music.FileName, (int)music.EntryId)))
            .Concat(DB.Songs.Values.Select(song => (song.FileName, (int)song.EntryId)))
            .Concat(DB.SoundEffects.Values.Select(effect => (effect.FileName, (int)effect.EntryId)));
        foreach (var entry in entries.Distinct().OrderBy(entry => entry.FileName, StringComparer.Ordinal).ThenBy(entry => entry.Item2))
        {
            string baseName = EventProjectExporter.Identifier(entry.FileName);
            string name = baseName;
            for (int suffix = entry.Item2; !names.Add(name); suffix++)
                name = $"{baseName}_{suffix}";
            result.Add(entry, name);
        }
        return result;
    }
}
