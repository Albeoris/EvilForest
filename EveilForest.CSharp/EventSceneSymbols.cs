using EvilForest.Resources;
using EvilForest.Resources.Enums;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class EventSceneSymbols
{
    private static readonly IReadOnlyDictionary<string, SceneNames> Catalog =
        new Dictionary<string, SceneNames>(StringComparer.OrdinalIgnoreCase)
        {
            ["EVT_ALEX1_TS_CARGO_0"] = new(
                new Dictionary<int, string> { [1] = "AttachedSpriteController", [16] = "StrategyMusicController", [17] = "Zidane" },
                17,
                new Dictionary<ushort, string>
                {
                    [11] = "ConfigureActorEffects",
                    [12] = "HandleTreasureReward",
                    [13] = "ShowPrimaVistaInformation"
                },
                new HashSet<(int, ushort)> { (1, 0), (1, 1), (8, 1), (17, 12) })
        };

    public SceneNames? Names { get; }
    public IReadOnlyDictionary<int, Participant> Participants { get; }
    private readonly Dictionary<int, int> _spawnedInstances;
    private readonly Dictionary<int, string> _directorRoles = new();

    public string DirectorRoleName(int slot) => _directorRoles.GetValueOrDefault(slot) ?? Participants[slot].Name;

    private static string? ModelRole(IEnumerable<EVInstructionBody> bodies)
    {
        string? name = null;
        foreach (EVInstruction step in bodies.SelectMany(body => body.Instructions).Where(step => step.Opcode == FF8.JSM.Jsm.Opcode.MODEL))
        {
            if (step.Operands[0] is not { Expression: null } id || !DB.Models.TryGetValue((DBModel.Id)id.Value, out var model) ||
                string.IsNullOrWhiteSpace(model.DisplayName) || name != null && name != model.DisplayName)
                return null;
            name = model.DisplayName;
        }
        return name == null ? null : EventProjectExporter.Identifier(string.Concat(name.Split(' ', StringSplitOptions.RemoveEmptyEntries)));
    }

    public EventSceneSymbols(string eventName, IEnumerable<(EVContainerObject Definition, string Name, EVInstructionBody[] Bodies)> definitions, bool useCutsceneNames = false)
    {
        Names = FindNames(eventName);
        if (useCutsceneNames)
        {
            var names = definitions.Where(entry => entry.Definition.Scripts.Length != 0).ToDictionary(entry => entry.Definition.Id, entry =>
                Names?.Participants.GetValueOrDefault(entry.Definition.Id) ?? CutsceneCatalog.ParticipantName(eventName, entry.Definition.Id) ??
                ModelRole(entry.Bodies) ?? entry.Name);
            var reserved = names.Values.ToHashSet(StringComparer.Ordinal);
            foreach (var group in names.GroupBy(pair => pair.Value, StringComparer.Ordinal))
            {
                foreach (var (slot, name) in group)
                {
                    string role = name;
                    if (group.Count() > 1)
                    {
                        role = $"{name}_{slot:D2}";
                        while (!reserved.Add(role))
                            role += "_";
                    }
                    _directorRoles.Add(slot, role);
                }
            }
        }
        Participants = definitions.Where(entry => entry.Definition.Scripts.Length > 0).ToDictionary(
            entry => entry.Definition.Id,
            entry => new Participant(entry.Name, entry.Definition.Type, HandlerNames(entry.Definition, Names, useCutsceneNames ? eventName : null), entry.Definition.Scripts));
        _spawnedInstances = definitions.SelectMany(item => item.Bodies).SelectMany(body => body.Instructions)
            .Where(instruction => instruction.Opcode is FF8.JSM.Jsm.Opcode.NEW or FF8.JSM.Jsm.Opcode.NEW2 or FF8.JSM.Jsm.Opcode.NEW3 &&
                instruction.Operands is [{ Expression: null }, { Expression: null }])
            .Select(instruction => (Slot: instruction.Operands[0].Value, Uid: instruction.Operands[1].Value))
            .GroupBy(spawn => spawn.Uid == 0 ? spawn.Slot : spawn.Uid)
            .Where(group => group.Key < 250 && group.Select(spawn => spawn.Slot).Distinct().Count() == 1)
            .ToDictionary(group => group.Key, group => group.First().Slot);
    }

    public static SceneNames? FindNames(string eventName) => Catalog.GetValueOrDefault(eventName);

    public static IReadOnlyList<string> HandlerNames(EVContainerObject definition, SceneNames? scene, string? cutsceneEvent = null)
    {
        var usedNames = new HashSet<string>(StringComparer.Ordinal);
        var directory = new List<string>(definition.Scripts.Length);
        for (int index = 0; index < definition.Scripts.Length; index++)
        {
            ushort tag = definition.Scripts[index].Id;
            string name = tag == 0 ? "Init" : $"Script_{tag:D2}";
            if (DB.ScriptNames.TryGetValue((DBScriptName.Id)tag, out DBScriptName? script) && !string.IsNullOrEmpty(script.Name))
                name = EventProjectExporter.Identifier(char.ToUpperInvariant(script.Name[0]) + script.Name[1..]);
            if (scene != null && definition.Id == scene.ControlledDefinition && scene.ControlledHandlers.TryGetValue(tag, out var entry))
                name = entry;
            if (cutsceneEvent != null && CutsceneCatalog.ParticipantHandlerName(cutsceneEvent, definition.Id, tag) is { } catalogName)
                name = catalogName;
            string baseName = name;
            for (int suffix = index; !usedNames.Add(name); suffix++)
                name = $"{baseName}_{suffix}";
            directory.Add(name);
        }
        return directory;
    }

    public string? HandlerReceiver(int uid, int tag)
    {
        bool controlled = uid == 250 && Names != null;
        int slot = controlled ? Names!.ControlledDefinition : uid;
        if (slot >= 250 || !Participants.TryGetValue(slot, out Participant? participant))
            return null;
        int index = Array.FindIndex(participant.Scripts, script => script.Id == tag);
        string receiver = controlled ? "ControlledActor" : participant.Name;
        return index < 0 ? null : $"evt.{receiver}.{participant.Handlers[index]}";
    }

    public (string Method, int Slot)? InstanceHandler(int uid, int tag)
    {
        if (uid == 0 || !_spawnedInstances.TryGetValue(uid, out int slot) || uid == slot || !Participants.TryGetValue(slot, out Participant? participant))
            return null;
        int index = Array.FindIndex(participant.Scripts, script => script.Id == tag);
        return index < 0 ? null : ($"evt.{participant.Name}.{participant.Handlers[index]}", slot);
    }

    internal sealed record Participant(string Name, byte Kind, IReadOnlyList<string> Handlers, EVContainerScript[] Scripts);
    internal sealed record SceneNames(
        IReadOnlyDictionary<int, string> Participants,
        int ControlledDefinition,
        IReadOnlyDictionary<ushort, string> ControlledHandlers,
        IReadOnlySet<(int Slot, ushort Tag)> ComposedHandlers);
}
