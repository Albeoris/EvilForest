namespace EvilForest.EventSource;

public enum DialogueOption
{
    First,
    Second,
    Third,
    Fourth
}

public readonly struct DialogueLine(Operand windowId, Operand flags, Operand messageId)
{
    internal Operand WindowId { get; } = windowId;
    internal Operand Flags { get; } = flags;
    internal Operand MessageId { get; } = messageId;
}

public static class Dialogue
{
    [SceneInstruction(Opcode.MES)]
    public static EventAwaitable Say(DialogueLine line, int mask = -1) => Messages.ShowAndWait(line.WindowId, line.Flags, line.MessageId, mask);
}
