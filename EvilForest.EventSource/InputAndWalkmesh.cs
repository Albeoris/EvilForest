namespace EvilForest.EventSource;

public enum BubbleSymbol
{
    Question,
    Exclamation,
    ExclamationAndDuel,
    Beach,
    ExclamationAndBeach
}

[Flags]
public enum PlayerInput
{
    None = 0,
    Select = 0x1,
    Start = 0x8,
    Up = 0x10,
    Right = 0x20,
    Down = 0x40,
    Left = 0x80,
    L2 = 0x100,
    R2 = 0x200,
    L1 = 0x400,
    R1 = 0x800,
    Triangle = 0x1000,
    Circle = 0x2000,
    Cross = 0x4000,
    Square = 0x8000,
    Cancel = 0x10000,
    Confirm = 0x20000,
    MenuControl = 0x40000,
    Special = 0x80000,
    LeftBumper = 0x100000,
    RightBumper = 0x200000,
    LeftTrigger = 0x400000,
    RightTrigger = 0x800000,
    Menu = 0x1000000,
    NaviControl = 0x2000000
}

[Flags]
public enum TriangleAttributeMask
{
    None = 0,
    RestrictOtherActors = 0x40,
    RestrictPlayer = 0x80,
    All = 0xFF
}

public static partial class Input
{
    [Instruction(Opcode.FICON)]
    public static void Bubble(BubbleSymbol symbol, int mask = -1)
    {
        if (!Enum.IsDefined(symbol))
            throw new ArgumentOutOfRangeException(nameof(symbol));
        EventScheduler.Execute(Opcode.FICON, (int)symbol);
    }

    [ExpressionInstruction(Excode.IsKeyPressed)]
    public static bool IsKeyPressed(PlayerInput buttons) => EventScheduler.Read(Excode.IsKeyPressed, (int)buttons) != 0;

    [ExpressionInstruction(Excode.B_KEYOFF)]
    public static bool IsKeyReleased(PlayerInput buttons) => EventScheduler.Read(Excode.B_KEYOFF, (int)buttons) != 0;

    [ExpressionInstruction(Excode.B_KEY)]
    public static bool IsKeyHeld(PlayerInput buttons) => EventScheduler.Read(Excode.B_KEY, (int)buttons) != 0;
}

public static partial class Regions
{
    [Instruction(Opcode.BGIMASK)]
    public static void SetTriangleFlagMask(TriangleAttributeMask flags, int mask = -1) => EventScheduler.Execute(Opcode.BGIMASK, (int)flags);
}
