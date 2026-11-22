using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal static partial class CutscenePhaseGraph
{
    internal readonly record struct Cell(EVStorageScope Scope, int Owner, int FirstBit, int Width)
    {
        public static Cell Of(EVVariable view, int slot) => new(view.Scope, view.Scope == EVStorageScope.Instance ? slot : -1,
            view.UsesBitAddress ? view.Address : view.Address * 8, view.Width);

        public bool Overlaps(Cell other) => Scope == other.Scope && Owner == other.Owner &&
            FirstBit < other.FirstBit + other.Width && other.FirstBit < FirstBit + Width;
    }

    internal sealed record Phase(CutsceneExport.Entry Entry, byte OwnerKind, EVExpressionToken Selector, Cell Register, int Value, int Start, EVInstruction[] Instructions)
    {
        public int? GuardConditionLabel { get; init; }

        public string Identity()
        {
            var relative = Instructions.Select((instruction, index) => (instruction.Label, index)).ToDictionary(item => item.Label, item => item.index);
            using var buffer = new MemoryStream();
            using var writer = new BinaryWriter(buffer);
            foreach (EVInstruction instruction in Instructions)
            {
                writer.Write((int)instruction.Opcode);
                writer.Write(instruction.Targets.Length);
                foreach (int target in instruction.Targets)
                    writer.Write(relative.GetValueOrDefault(target, -1));
            }
            return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(buffer.ToArray()).AsSpan(0, 12));
        }

        public bool Has(params Jsm.Opcode[] opcodes) => opcodes.Any(opcode => Instructions.Any(instruction => instruction.Opcode == opcode));
        public bool Speaks => Has(Jsm.Opcode.MES, Jsm.Opcode.MESA, Jsm.Opcode.MESN, Jsm.Opcode.MESAN);
        public bool Chooses => Instructions.Any(instruction => instruction.Expression?.Any(token => token.Opcode == (byte)Jsm.Excode.B_SYSVAR &&
            token.Value == (int)EvilForest.EventSource.SystemVariable.DialogChoice) == true);

        public IEnumerable<int> Successors => Writes().Where(write => write.Cell == Register).Select(write => write.Value).Distinct();

        public IEnumerable<(Cell Cell, int Value)> Writes()
        {
            foreach (EVInstruction instruction in Instructions)
            {
                if (instruction.Expression is [var target, var value, { Opcode: (byte)Jsm.Excode.Let }] &&
                    StorageDeclarations.IsStorage(target) && value.Opcode is (byte)Jsm.Excode.Const16 or (byte)Jsm.Excode.Const26)
                    yield return (Cell.Of(EVVariable.Decode(target), Entry.Slot), value.Opcode == (byte)Jsm.Excode.Const16 ? value.Value : value.Value << 6 >> 6);
            }
        }

        public IEnumerable<Cell> Reads() => Instructions.SelectMany(CutsceneMemoryIndex.Accesses).Where(access => access.Read)
            .Select(access => Cell.Of(access.Variable, Entry.Slot)).Distinct();
    }

    public static IEnumerable<Phase[]> ActorExchanges(Phase[] arms)
    {
        var byValue = arms.ToLookup(phase => phase.Value);
        var adjacent = arms.ToDictionary(phase => phase, _ => new HashSet<Phase>());
        var speakers = new HashSet<Phase>();
        foreach (Phase phase in arms)
        {
            foreach (Phase next in phase.Successors.Where(value => value != phase.Value).SelectMany(value => byValue[value]))
            {
                adjacent[phase].Add(next);
                adjacent[next].Add(phase);
                if (phase.Entry != next.Entry && phase.Speaks && next.Speaks)
                    speakers.Add(phase);
            }
        }

        var visited = new HashSet<Phase>();
        foreach (Phase sender in arms.Where(speakers.Contains))
        {
            if (!visited.Add(sender))
                continue;

            var component = new HashSet<Phase> { sender };
            var pending = new Queue<Phase>();
            pending.Enqueue(sender);
            while (pending.TryDequeue(out Phase? phase))
            {
                foreach (Phase neighbor in adjacent[phase])
                {
                    if (visited.Add(neighbor))
                    {
                        component.Add(neighbor);
                        pending.Enqueue(neighbor);
                    }
                }
            }

            // Require dialogue that advances across actors more than once, including another actor's write.
            if (component.Where(speakers.Contains).Select(phase => phase.Entry).Distinct().Count() < 2)
                continue;
            yield return arms.Where(component.Contains).ToArray();
        }
    }

    public static IEnumerable<Phase> Read(CutsceneExport.Entry entry, byte ownerKind, EVInstructionBody body, bool includeSharedArms = false)
    {
        IList<EVInstruction> instructions = body.Instructions;
        var dispatches = new List<(EVInstruction Selector, EVInstruction Table, int[] Values)>();
        for (int index = 1; index < instructions.Count; index++)
        {
            EVInstruction table = instructions[index];
            EVInstruction selector = instructions[index - 1];
            if (selector.Opcode != Jsm.Opcode.EXPR || selector.Expression is not [var token] || !StorageDeclarations.IsStorage(token) ||
                EVVariable.Decode(token) is not { Scope: EVStorageScope.Map or EVStorageScope.Global, UsesBitAddress: false } ||
                table.Opcode is not (Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d) || table.CaseValues.Length == 0)
                continue;
            int[] values = table.Opcode == Jsm.Opcode.JMP_SWITCHEX ? table.CaseValues.Select(value => (int)value).ToArray() :
                Enumerable.Range(unchecked((short)table.CaseValues[0]), table.Targets.Length - 1).ToArray();
            dispatches.Add((selector, table, values));
        }
        if (dispatches.Count == 0)
            yield break;

        IReadOnlyDictionary<int, int[]> graph = body.GetSuccessors();
        foreach (var (selector, table, values) in dispatches)
        {
            int[] targets = table.Targets.Distinct().ToArray();
            var boundaries = targets.Append(selector.Label).Append(table.Label).ToHashSet();
            var reachable = targets.ToDictionary(target => target, Follow);
            var incoming = reachable.Values.SelectMany(nodes => nodes).GroupBy(label => label).ToDictionary(group => group.Key, group => group.Count());
            EVExpressionToken token = selector.Expression![0];
            Cell register = Cell.Of(EVVariable.Decode(token), entry.Slot);
            for (int arm = 0; arm < values.Length; arm++)
            {
                int target = table.Targets[arm + 1];
                if (target == table.Targets[0] || !includeSharedArms && table.Targets.Count(candidate => candidate == target) != 1)
                    continue;
                EVInstruction[] owned = instructions.Where(instruction => reachable[target].Contains(instruction.Label) && incoming[instruction.Label] == 1).ToArray();
                if (owned.Length != 0)
                    yield return new Phase(entry, ownerKind, token, register, values[arm], target, owned);
            }

            HashSet<int> Follow(int start)
            {
                var visited = new HashSet<int>();
                var pending = new Stack<int>();
                pending.Push(start);
                while (pending.TryPop(out int label))
                {
                    if (!visited.Add(label) || !graph.TryGetValue(label, out int[]? next))
                        continue;
                    foreach (int successor in next)
                    {
                        if (!boundaries.Contains(successor))
                            pending.Push(successor);
                    }
                }
                return visited;
            }
        }
    }
}
