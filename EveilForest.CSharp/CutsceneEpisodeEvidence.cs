using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private sealed record LiveParticipants(Dictionary<int, HashSet<int>> Definitions, HashSet<int> SpawnedSlots, HashSet<int> SequenceUids);
    private readonly LiveParticipants _liveParticipants;

    private int? EpisodeActorSlot(int uid)
    {
        if ((uint)uid >= 250 || _liveParticipants.SequenceUids.Contains(uid) ||
            !_liveParticipants.Definitions.TryGetValue(uid, out var candidates) || candidates.Count != 1)
            return null;

        int slot = candidates.Single();
        return _symbols.Participants.TryGetValue(slot, out var participant) && participant.Kind == 2 ? slot : null;
    }

    private LiveParticipants IndexLiveParticipants(Handler[] handlers)
    {
        var instructions = handlers.SelectMany(handler => handler.Body.Instructions.Select(step => (Handler: handler, Step: step))).ToArray();
        var sequences = instructions.Where(item => item.Step.Opcode == Jsm.Opcode.STARTSEQ).ToArray();
        var sequenceUids = new HashSet<int>();
        var normalSlots = _possibleDefinitions.Values.SelectMany(definitions => definitions).ToHashSet();
        var possibleDefinitions = _possibleDefinitions.ToDictionary(pair => pair.Key, pair => pair.Value.ToHashSet());

        foreach (var (_, step) in instructions.Where(item => item.Step.Opcode == Jsm.Opcode.NEW3 &&
                     item.Step.Operands[0].Value is >= 251 and <= 254))
        {
            foreach (int actor in _symbols.Participants.Where(pair => pair.Value.Kind == 2).Select(pair => pair.Key))
            {
                int uid = step.Operands[1].Value == 0 ? actor : step.Operands[1].Value;
                AddInstance(uid, actor);
                normalSlots.Add(actor);
            }
        }

        // Sequences create relative UIDs, which can collide with normal instance UIDs.
        // Propagate all possible sequence instances, including nested sequences.
        bool changed;
        do
        {
            changed = false;
            var instances = possibleDefinitions.SelectMany(pair => pair.Value.Select(slot => (Uid: pair.Key, Slot: slot))).ToLookup(pair => pair.Slot);
            foreach (var (handler, step) in sequences)
            {
                IEnumerable<int> targets = step.Operands[0].Expression == null ? [step.Operands[0].Value] : _symbols.Participants.Keys;
                foreach (int uid in instances[handler.Entry.Slot].Select(instance => (int)unchecked((byte)(instance.Uid + SequenceUidOffset))))
                {
                    sequenceUids.Add(uid);
                    foreach (int target in targets)
                        changed |= AddInstance(uid, target);
                }
            }
        }
        while (changed);

        return new LiveParticipants(possibleDefinitions, normalSlots, sequenceUids);

        bool AddInstance(int uid, int slot)
        {
            if (!possibleDefinitions.TryGetValue(uid, out var definitions))
                possibleDefinitions.Add(uid, definitions = new());
            return definitions.Add(slot);
        }
    }

    private static bool RequiresOnCompletion(EVInstructionBody body, Func<EVInstruction, bool> witness)
    {
        if (body.Instructions.Count == 0)
            return false;

        var successors = body.GetSuccessors();
        var instructions = body.Instructions.ToDictionary(step => step.Label);
        var pending = new Stack<(int Label, bool Seen)>();
        var visited = new HashSet<(int Label, bool Seen)>();
        pending.Push((body.Instructions[0].Label, false));
        bool returns = false;
        while (pending.TryPop(out var state))
        {
            if (!visited.Add(state))
                continue;
            if (state.Label == body.EndLabel)
                return false;
            EVInstruction step = instructions[state.Label];
            bool seen = state.Seen || witness(step);
            if (!seen && step.Opcode is (Jsm.Opcode.DRET or Jsm.Opcode.DELETE or Jsm.Opcode.STOP or Jsm.Opcode.MAPJUMP))
                return false;
            if (step.Opcode == Jsm.Opcode.Return)
            {
                if (!seen)
                    return false;
                returns = true;
                continue;
            }
            foreach (int next in successors[state.Label])
                pending.Push((next, seen));
        }
        return returns;
    }

    private static bool ReadsEpisodeInput(EVInstruction step) =>
        (step.Expression ?? []).Concat(step.Operands.SelectMany(operand => operand.Expression ?? [])).Any(token =>
            IsChoiceRegister(token) || (Jsm.Excode)token.Opcode is Jsm.Excode.IsKeyPressed or Jsm.Excode.B_KEY or Jsm.Excode.B_KEYOFF or
                Jsm.Excode.B_KEY2 or Jsm.Excode.B_KEYON2 or Jsm.Excode.B_KEYOFF2);

    private sealed class EpisodeFlow
    {
        private readonly Dictionary<int, int> _positions;
        public IList<EVInstruction> Code { get; }
        public int[][] Successors { get; }
        public ILookup<int, int> Incoming { get; }

        public EpisodeFlow(EVInstructionBody body)
        {
            Code = body.Instructions;
            _positions = Code.Select((step, index) => (step.Label, index)).ToDictionary(item => item.Label, item => item.index);
            _positions.Add(body.EndLabel, Code.Count);
            var graph = body.GetSuccessors();
            Successors = Code.Select(step => graph[step.Label].Select(Position).ToArray()).ToArray();
            Incoming = Successors.SelectMany((targets, source) => targets.Select(target => (target, source))).ToLookup(edge => edge.target, edge => edge.source);
        }

        public int Position(int label) => _positions[label];

        public int? JumpTarget(int index) => Code[index] is { Opcode: Jsm.Opcode.JMP, Targets: [int label] } ? Position(label) : null;

        public int ForwardBranch(int expression) => expression + 2 < Code.Count && Code[expression + 1] is { Opcode: Jsm.Opcode.JMP_IFN or Jsm.Opcode.JMP_IF, Targets: [int label] } &&
            Position(label) > expression + 2 ? Position(label) : -1;

        public bool Closed(int start, int end, int entry)
        {
            if (Reachable(start, start, end).Count != end - start)
                return false;
            return Enumerable.Range(start, end - start).All(index =>
                Code[index].Opcode is not (Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2 or Jsm.Opcode.MINIGAME) &&
                Incoming[index].All(source => source >= start && source < end || index == start && source == entry) &&
                Successors[index].Length > 0 && Successors[index].All(target => target >= start && target <= end));
        }

        public bool Requires(int start, int end, int required) => !Reachable(start, start, end + 1, required).Contains(end);

        public HashSet<int> Reachable(int start, int first, int end, int? excluded = null)
        {
            var visited = new HashSet<int>();
            var pending = new Stack<int>();
            pending.Push(start);
            while (pending.TryPop(out int index))
            {
                if (index < first || index >= end || index == excluded || !visited.Add(index))
                    continue;
                if (index < Successors.Length)
                {
                    foreach (int next in Successors[index])
                        pending.Push(next);
                }
            }
            return visited;
        }
    }
}
