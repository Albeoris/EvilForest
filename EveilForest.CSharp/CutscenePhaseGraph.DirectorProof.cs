using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal static partial class CutscenePhaseGraph
{
    internal static bool IsImmediateSelfDelete(EVInstruction step) => step is
        { Opcode: Jsm.Opcode.DELETE, ArgumentMask: 0, Operands: [{ Expression: null, Value: 255 }], Targets: [] };

    internal static bool TakesLiteralBranch(EVInstruction condition, EVInstruction branch) =>
        condition is { Opcode: Jsm.Opcode.EXPR, Expression: [var literal] } && TryReadConstant(literal, out int value) &&
        (branch.Opcode == Jsm.Opcode.JMP_IF && value != 0 || branch.Opcode == Jsm.Opcode.JMP_IFN && value == 0);

    public static IEnumerable<Phase> ReadPhaseGuards(EVInstructionBody body, Cell register, CutsceneExport.Entry entry, byte kind, IReadOnlySet<int> phases,
        IEnumerable<Phase> knownArms, bool knownRole, IReadOnlySet<Cell> cues)
    {
        var owned = knownArms.SelectMany(phase => phase.Instructions).Select(step => step.Label).ToHashSet();
        IList<EVInstruction> instructions = body.Instructions;
        for (int index = 1; index < instructions.Count; index++)
        {
            EVInstruction branch = instructions[index];
            EVInstruction condition = instructions[index - 1];
            if (owned.Contains(condition.Label) || branch.Opcode is not (Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN) ||
                condition.Opcode != Jsm.Opcode.EXPR || condition.Expression is not [var left, var right, var comparison] ||
                comparison.Opcode is not ((byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence))
                continue;

            if (!StorageDeclarations.IsStorage(left))
                (left, right) = (right, left);
            if (!StorageDeclarations.IsStorage(left) || Cell.Of(EVVariable.Decode(left), entry.Slot) != register ||
                !TryReadConstant(right, out int phase) || !phases.Contains(phase))
                continue;

            IReadOnlyDictionary<int, int[]> graph = body.GetSuccessors();
            int[] successors = graph[branch.Label];
            bool takenWhenEqual = (branch.Opcode == Jsm.Opcode.JMP_IF) == (comparison.Opcode == (byte)Jsm.Excode.Equivalence);
            int start = takenWhenEqual ? successors[0] : successors[1];
            HashSet<int> matching = Walk(start);
            matching.ExceptWith(Walk(takenWhenEqual ? successors[1] : successors[0]));
            EVInstruction[] guarded = instructions.Where(step => matching.Contains(step.Label) && !owned.Contains(step.Label)).ToArray();
            if (guarded.Any(IsRoleWork))
                yield return new Phase(entry, kind, left, register, phase, start, guarded) { GuardConditionLabel = condition.Label };

            HashSet<int> Walk(int start)
            {
                var reached = new HashSet<int>();
                var pending = new Stack<int>();
                pending.Push(start);
                while (pending.TryPop(out int label))
                {
                    if (label == condition.Label || !reached.Add(label) || !graph.TryGetValue(label, out int[]? next))
                        continue;
                    foreach (int successor in next)
                        pending.Push(successor);
                }
                return reached;
            }
        }
        bool IsRoleWork(EVInstruction step)
        {
            if (knownRole)
                return step.Opcode is not (Jsm.Opcode.JMP or Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN or Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d);
            if (SourceInstruction.TryGet(step, out var operation) && (operation.Service == "Camera" || kind == 2 && (operation.Service is "Actors" or "Animations")))
                return true;
            if (kind == 2 && step.Opcode == Jsm.Opcode.DELETE && step.Operands is [{ Expression: null, Value: 255 }])
                return true;
            return CutsceneMemoryIndex.Accesses(step).Any(access => access.Write && (cues.Contains(Cell.Of(access.Variable, entry.Slot)) || Cell.Of(access.Variable, entry.Slot) == register));
        }
    }

    public static EVInstruction? FindUnmappedTransition(Phase phase) => phase.Instructions.FirstOrDefault(step =>
        CutsceneMemoryIndex.Accesses(step).Any(access => access.Write && phase.Register.Overlaps(Cell.Of(access.Variable, phase.Entry.Slot))) &&
        (step.Expression is not [var target, var value, { Opcode: (byte)Jsm.Excode.Let }] ||
            !StorageDeclarations.IsStorage(target) || Cell.Of(EVVariable.Decode(target), phase.Entry.Slot) != phase.Register || !TryReadConstant(value, out _)));

    internal static bool TryReadConstant(EVExpressionToken token, out int value)
    {
        value = token.Opcode == (byte)Jsm.Excode.Const26 ? token.Value << 6 >> 6 : token.Value;
        return token.Opcode is (byte)Jsm.Excode.Const16 or (byte)Jsm.Excode.Const26;
    }
}
