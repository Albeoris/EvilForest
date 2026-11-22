using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private void AddDirectorEpisodes(Handler[] handlers, List<Scene> scenes)
    {
        var entries = handlers.ToLookup(handler => handler.Entry);
        var actorStaging = new Dictionary<Entry, bool>();
        foreach (Handler director in handlers)
        {
            if (director.Owner.Type != 0 || entries[director.Entry].Count() != 1 || scenes.Any(scene => scene.For(director.Entry) != null))
                continue;

            EVInstruction[] required = RequiredDirectorChoreography(director.Body);
            if (!IsDirectorEpisode(required, IsStagedRequest))
                continue;

            scenes.Add(new Scene(Fallback(director.Entry), director.Entry, true, new Dictionary<int, string> { [0] = "Play" })
            {
                Rule = "restored controller dialogue"
            });
        }

        bool IsStagedRequest(int uid, int tag)
        {
            if (tag <= LastEngineCallback || EpisodeActorSlot(uid) is not int slot)
                return false;

            var entry = new Entry(slot, (ushort)tag);
            if (!actorStaging.TryGetValue(entry, out bool staged))
            {
                staged = entries[entry].ToArray() is [var action] && HasActorChoreography(action.Body);
                actorStaging.Add(entry, staged);
            }
            return staged;
        }
    }

    private static EVInstruction[] RequiredDirectorChoreography(EVInstructionBody body)
    {
        IList<EVInstruction> code = body.Instructions;
        if (!code.Any(step => step.Opcode == Jsm.Opcode.UCON) || !code.Any(step => step.Opcode == Jsm.Opcode.MESA))
            return [];

        IReadOnlyDictionary<int, int[]> successors = body.GetSuccessors();
        int start = code[0].Label;
        var reachable = DirectorReachable(start, successors);
        EVInstruction[] live = code.Where(step => reachable.ContainsKey(step.Label)).ToArray();
        if (live.Any(PrecludesWholeDirectorEpisode))
            return [];

        int[] restores = live.Where(step => step.Opcode == Jsm.Opcode.UCON).Select(step => step.Label).ToArray();
        if (restores.Length == 0 || live.All(step => step.Opcode != Jsm.Opcode.Return) ||
            restores.Any(restore =>
            {
                var tail = DirectorReachable(restore, successors);
                return live.Any(step => tail.ContainsKey(step.Label) &&
                    (OpensMessage(step) || IsRequest(step) || step.Opcode is Jsm.Opcode.STARTSEQ or Jsm.Opcode.DELETE or Jsm.Opcode.DTURN or Jsm.Opcode.DANIM));
            }))
            return [];

        var checkpoints = live.Where(step => step.Opcode == Jsm.Opcode.Return).Select(step => step.Label).Concat(restores).Append(body.EndLabel).ToHashSet();
        var path = new Stack<int>();
        for (int label = restores[0]; label != start; label = reachable[label])
            path.Push(reachable[label]);

        var instructions = live.ToDictionary(step => step.Label);
        return path.Select(label => instructions[label])
            .Where(step => step.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.DTURN or Jsm.Opcode.DANIM or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW or Jsm.Opcode.DELETE)
            .Where(step => !DirectorReachable(start, successors, step.Label).Keys.Any(checkpoints.Contains)).ToArray();
    }

    private bool IsDirectorEpisode(EVInstruction[] choreography, Func<int, int, bool> isStagedRequest)
    {
        var requested = new HashSet<int>();
        var waitingOn = new HashSet<int>();
        var gestures = new HashSet<int>();
        var speaking = new HashSet<int>();
        var repeatedSpeakers = new HashSet<int>();
        var completingDeparture = new HashSet<int>();
        var departed = new HashSet<int>();
        foreach (EVInstruction step in choreography)
        {
            if (step.Opcode is Jsm.Opcode.REQSW or Jsm.Opcode.REQEW &&
                step.Operands is [_, { Expression: null } receiver, { Expression: null } tag] && isStagedRequest(receiver.Value, tag.Value))
            {
                requested.Add(receiver.Value);
                if (step.Opcode == Jsm.Opcode.REQEW)
                {
                    waitingOn.Add(receiver.Value);
                    if (repeatedSpeakers.Contains(receiver.Value))
                        completingDeparture.Add(receiver.Value);
                }
            }
            else if (step.Operands is [{ Expression: null } target, ..] && EpisodeActorSlot(target.Value).HasValue)
            {
                int uid = target.Value;
                if (step.Opcode == Jsm.Opcode.MESA && requested.Contains(uid) && !speaking.Add(uid))
                    repeatedSpeakers.Add(uid);
                if (step.Opcode is Jsm.Opcode.DTURN or Jsm.Opcode.DANIM)
                    gestures.Add(uid);
                if (step.Opcode == Jsm.Opcode.DELETE && completingDeparture.Contains(uid))
                    departed.Add(uid);
            }
        }

        return speaking.Count >= 2 && waitingOn.Overlaps(speaking) && gestures.Overlaps(speaking) ||
            departed.Any(actor => gestures.Any(listener => listener != actor));
    }

    private static bool PrecludesWholeDirectorEpisode(EVInstruction step) =>
        step.Opcode is Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2 or Jsm.Opcode.MINIGAME or Jsm.Opcode.MENU or Jsm.Opcode.PARTYMENU or
            Jsm.Opcode.ITEMDELETE or Jsm.Opcode.GILDELETE or Jsm.Opcode.STOP or Jsm.Opcode.DRET or Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 ||
        ReadsEpisodeInput(step);

    private static bool HasActorChoreography(EVInstructionBody body) => RequiresOnCompletion(body, step =>
        step.Opcode is Jsm.Opcode.TURN or Jsm.Opcode.TURNA or Jsm.Opcode.TURNDS or Jsm.Opcode.TURNTO or Jsm.Opcode.ANIM or
            Jsm.Opcode.MOVE or Jsm.Opcode.MOVE3 or Jsm.Opcode.MOVE3H or Jsm.Opcode.MOVA or Jsm.Opcode.MOVH);

    private static Dictionary<int, int> DirectorReachable(int start, IReadOnlyDictionary<int, int[]> successors, int? omitted = null)
    {
        var reached = new Dictionary<int, int>();
        var pending = new Stack<(int Label, int Previous)>();
        pending.Push((start, start));
        while (pending.TryPop(out var item))
        {
            if (item.Label == omitted || !reached.TryAdd(item.Label, item.Previous) || !successors.TryGetValue(item.Label, out int[]? targets))
                continue;

            foreach (int target in targets)
                pending.Push((target, item.Label));
        }
        return reached;
    }
}
