using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private bool HasDirectedDialogue(CutscenePhaseGraph.Phase phase, EVInstructionBody body)
    {
        var instructions = phase.Instructions.ToDictionary(instruction => instruction.Label);
        IReadOnlyDictionary<int, int[]> successors = body.GetSuccessors();
        var visited = new HashSet<int>();
        var requested = new HashSet<int>();
        var completed = new HashSet<int>();
        var speakers = new HashSet<int>();
        var staged = new HashSet<int>();
        var stagedSpeakers = new HashSet<int>();
        bool reachedDialogue = false;
        bool movesCamera = false;
        bool pausesAfterStaging = false;
        int label = phase.Start;
        while (visited.Add(label) && instructions.TryGetValue(label, out EVInstruction? instruction))
        {
            if (instruction.Opcode is Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2)
                return false;
            if (IsRequest(instruction) && instruction.Operands is [_, { Expression: null } receiver, { Expression: null } tag] &&
                _possibleDefinitions.TryGetValue(receiver.Value, out var definitions) && definitions.Count == 1 &&
                _symbols.Participants.TryGetValue(definitions.Single(), out var participant) && participant.Kind == 2 &&
                participant.Scripts.Any(script => script.Id == tag.Value))
            {
                requested.Add(receiver.Value);
                if (instruction.Opcode == Jsm.Opcode.REQEW)
                    completed.Add(receiver.Value);
                if (!reachedDialogue && instruction.Opcode == Jsm.Opcode.REQSW)
                    staged.Add(receiver.Value);
            }
            if (!reachedDialogue)
            {
                if (instruction.Opcode == Jsm.Opcode.BGSSCROLL && instruction.Operands is [_, _, { Expression: null, Value: > 0 }, _])
                    movesCamera = true;
                if (staged.Count >= 3 && instruction.Opcode == Jsm.Opcode.WAIT && instruction.Operands is [{ Expression: null, Value: > 0 }])
                    pausesAfterStaging = true;
            }
            if (instruction.Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESN or Jsm.Opcode.MESA or Jsm.Opcode.MESAN)
            {
                reachedDialogue = true;
                if (instruction.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN && instruction.Operands[0] is { Expression: null } speaker)
                {
                    speakers.Add(speaker.Value);
                    if (instruction.Opcode == Jsm.Opcode.MESA && staged.Contains(speaker.Value))
                        stagedSpeakers.Add(speaker.Value);
                }
            }

            if (requested.Intersect(speakers).Take(2).Count() == 2 && completed.Overlaps(speakers) ||
                movesCamera && pausesAfterStaging && stagedSpeakers.Count >= 2)
                return true;
            if (successors[label] is not [int next])
                break;
            label = next;
        }
        return false;
    }
}
