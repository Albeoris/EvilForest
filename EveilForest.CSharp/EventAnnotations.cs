using System.Text.RegularExpressions;
using EvilForest.Resources;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class EventAnnotations(string eventName)
{
    private readonly DBEvent.FieldDescriptor? _field = DB.Events.Values.FirstOrDefault(evt => evt.FileName.Equals(eventName, StringComparison.OrdinalIgnoreCase))?.Field;

    private static readonly HashSet<string> Substitutions = new(StringComparer.Ordinal)
    {
        "Variable", "Item", "Text", "Icon", "Zidane", "Dagger", "Garnet", "Vivi", "Steiner", "Freya", "Fraya",
        "Quina", "Eiko", "Amarant", "Start", "Select", "Circle", "Cross", "Triangle", "Square", "L1", "L2", "R1", "R2",
        "Up", "Down", "Left", "Right"
    };

    public IEnumerable<string> For(EVInstruction instruction)
    {
        if (instruction.Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESN or Jsm.Opcode.MESA or Jsm.Opcode.MESAN or Jsm.Opcode.MESB)
        {
            EVOperand message = instruction.Operands[^1];
            if (message.Expression == null && _field != null &&
                DB.FieldMessages.TryGetValue(_field.MessageFileId, out var bank) &&
                bank.TryGetValue((DBFieldMessage.Id)message.Value, out DBFieldMessage? entry))
                return DialogueLines(entry.Text);
        }
        else if (ResolveAudio(instruction) is { } audio && audio.Command is "SongPlay" or "SongLoad" or "StreamPlay" or "SfxPlay" or "SfxResidentPlay")
        {
            return audio.Title.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim());
        }
        return Array.Empty<string>();
    }

    public static (string Command, string Name, string Title)? ResolveAudio(EVInstruction instruction)
    {
        if (instruction.Opcode is not (Jsm.Opcode.FLDSND0 or Jsm.Opcode.FLDSND1 or Jsm.Opcode.FLDSND2 or Jsm.Opcode.FLDSND3) ||
            instruction.Operands[0].Expression != null || instruction.Operands[1].Expression != null ||
            !SourceInstruction.SoundCommands.TryGetValue(instruction.Operands[0].Value & ~0x40, out string? command))
            return null;

        int id = instruction.Operands[1].Value;
        if (command.StartsWith("Song", StringComparison.Ordinal) && DB.Music.TryGetValue((DBMusic.Id)id, out DBMusic? music))
            return (command, music.FileName, music.DisplayName);
        if (command.StartsWith("Stream", StringComparison.Ordinal) && DB.Songs.TryGetValue((DBSong.Id)id, out DBSong? stream))
            return (command, stream.FileName, stream.DisplayName);
        if (command.StartsWith("Sfx", StringComparison.Ordinal) && DB.SoundEffects.TryGetValue((DBSfx.Id)id, out DBSfx? effect))
            return (command, effect.FileName, effect.DisplayName);
        return null;
    }

    private static IEnumerable<string> DialogueLines(string message)
    {
        foreach (string line in FF8.JSM.Format.FormatHelper.SplitMonologue(message))
        {
            bool choice = line.Contains("{Choice}", StringComparison.Ordinal) || line.TrimStart().StartsWith("{Tab}", StringComparison.Ordinal);
            string text = Regex.Replace(line, @"\{([^{}\r\n]+)\}", match =>
            {
                string token = match.Groups[1].Value;
                int separator = token.IndexOf(' ');
                string command = separator < 0 ? token : token.Substring(0, separator);
                if (Substitutions.Contains(command.TrimEnd('+')))
                    return $"[{token}]";
                return "";
            }).Trim();
            if (text.Length > 0)
                yield return choice ? $"- {text}" : text;
        }
    }
}
