using System.Text;
using EvilForest.Resources;
using EvilForest.Resources.Enums;

namespace EveilForest.CSharp;

internal static class ResourceCatalog
{
    private static readonly IReadOnlyDictionary<(string Name, int Id), string> Members = Build();

    public static string Qualify(string name, int id) => $"Resources.{Members[(name, id)]}";

    public static string MemberName(string name, int id) => Members[(name, id)];

    public static void AppendCharacterResourceIds(StringBuilder source, IEnumerable<int> animationIds, IEnumerable<int> soundIds)
    {
        AppendDomain("AnimationIds", "AnimationId", animationIds.Where(id => DB.Animations.ContainsKey((DBAnimation.Id)id))
            .Select(id => (MemberName(DB.Animations[(DBAnimation.Id)id].FileName, id), id)));
        AppendDomain("Sounds", "SoundEffectId", soundIds.Where(id => DB.SoundEffects.ContainsKey((DBSfx.Id)id))
            .Select(id => (SoundMember(DB.SoundEffects[(DBSfx.Id)id].FileName, id), id)));

        void AppendDomain(string name, string type, IEnumerable<(string Member, int Id)> entries)
        {
            source.AppendLine($"public static class {name}");
            source.AppendLine("{");
            foreach (var (member, id) in entries.OrderBy(entry => entry.Member, StringComparer.Ordinal))
            {
                source.AppendLine($"    [ResourceValue({id})]");
                source.AppendLine($"    public static {type} {member} => new({id});");
            }
            source.AppendLine("}");
            source.AppendLine();
        }
    }

    public static string SoundMember(string name, int id)
    {
        string member = MemberName(name, id);
        return char.ToUpperInvariant(member[0]) + member[1..];
    }

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
