using System.Text.Json;

namespace EveilForest.CSharp;

public sealed record CutsceneCatalogEntry(string Event, int Slot, ushort Tag, int? Phase = null, string? Name = null, string? Group = null, string? Fragment = null, string? Parallel = null,
    IReadOnlyDictionary<string, string>? Roles = null);

public sealed class CutsceneCatalog(IReadOnlyList<CutsceneCatalogEntry> entries)
{
    internal IReadOnlyList<CutsceneCatalogEntry> Entries { get; } = entries;

    private sealed record ParticipantLabel(string Event, int Slot, string Name, Dictionary<ushort, string>? Handlers);

    private static readonly Dictionary<(string Event, int Slot), ParticipantLabel> Participants = ReadBundled<ParticipantLabel>("CutsceneParticipantNames.json").ToDictionary(p => (p.Event, p.Slot));

    internal static CutsceneCatalog Default { get; } = new(ReadBundled<CutsceneCatalogEntry>("CutsceneNames.json"));

    internal static string? ParticipantName(string eventName, int slot) => Participants.GetValueOrDefault((eventName, slot))?.Name;

    internal static string? ParticipantHandlerName(string eventName, int slot, ushort tag) => Participants.GetValueOrDefault((eventName, slot))?.Handlers?.GetValueOrDefault(tag);

    private static T[] ReadBundled<T>(string name)
    {
        using Stream input = typeof(CutsceneCatalog).Assembly.GetManifestResourceStream(name) ?? throw new InvalidDataException($"The bundled catalog {name} is missing.");
        return JsonSerializer.Deserialize<T[]>(input) ?? throw new InvalidDataException($"The bundled catalog {name} is empty.");
    }

    internal IReadOnlyDictionary<string, string>? RoleNames(string eventName, CutsceneExport.Entry entry, int phase, string fragment)
    {
        CutsceneCatalogEntry[] matches = Entries.Where(row => row.Slot == entry.Slot && row.Tag == entry.Tag && row.Phase == phase &&
            string.Equals(row.Event, eventName, StringComparison.OrdinalIgnoreCase)).ToArray();
        return matches is [var match] && string.Equals(match.Fragment, fragment, StringComparison.OrdinalIgnoreCase) ? match.Roles : null;
    }

    public static CutsceneCatalog Read(string path)
    {
        try
        {
            var entries = JsonSerializer.Deserialize<CutsceneCatalogEntry[]>(File.ReadAllText(path),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new CutsceneCatalog(entries ?? []);
        }
        catch (JsonException error)
        {
            throw new InvalidDataException($"Invalid cutscene catalog '{path}': {error.Message}", error);
        }
    }
}
