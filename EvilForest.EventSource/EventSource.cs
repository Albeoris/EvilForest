using System;

namespace EvilForest.EventSource;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class EventFormatAttribute(byte version) : Attribute
{
    public byte Version { get; } = version;
}

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class EventLayoutAttribute(string header, string trailer, int objectCount) : Attribute
{
    public EventLayoutAttribute(string header, string trailer) : this(header, trailer, -1)
    {
    }

    public string Header { get; } = header;
    public string Trailer { get; } = trailer;
    public int ObjectCount { get; } = objectCount;
}

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class MapStorageReservationAttribute(int firstBit, int bitCount) : Attribute
{
    public int FirstBit { get; } = firstBit;
    public int BitCount { get; } = bitCount;
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ObjectSlotAttribute(int id, byte variableCount, byte flags, ushort reserved, int bodyOrder, ushort emptyOffset, byte type, string gap, string prefix) : Attribute
{
    public int Id { get; } = id;
    public byte VariableCount { get; } = variableCount;
    public byte Flags { get; } = flags;
    public ushort Reserved { get; } = reserved;
    public int BodyOrder { get; } = bodyOrder;
    public ushort EmptyOffset { get; } = emptyOffset;
    public byte Type { get; } = type;
    public string Gap { get; } = gap;
    public string Prefix { get; } = prefix;
}

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ControlledActorContractAttribute(int slot) : Attribute
{
    public int Slot { get; } = slot;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class OriginalDuplicateTagAttribute(ushort tag, int directoryIndex) : Attribute
{
    public ushort Tag { get; } = tag;
    public int DirectoryIndex { get; } = directoryIndex;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class ScriptAttribute(ushort id, int directoryIndex) : Attribute
{
    public ushort Id { get; } = id;
    public int DirectoryIndex { get; } = directoryIndex;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class SwitchLayoutAttribute(string key, SwitchTable table, string order, bool fallsThrough) : Attribute
{
    public string Key { get; } = key;
    public SwitchTable Table { get; } = table;
    public string Order { get; } = order;
    public bool FallsThrough { get; } = fallsThrough;
    public string TransferExitSections { get; set; } = "";
    public string ThreadedExitSections { get; set; } = "";
    public string FallThroughSections { get; set; } = "";
    public bool HasUnreachablePrefix { get; set; }
    public int MergedCaseSection { get; set; } = -1;
    public bool ThreadDefaultExit { get; set; }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class OmitUnreachablePollingExitAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class PreserveEmptyJumpsAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class ShareSwitchCaseActionsAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class InstructionAttribute(Opcode opcode, int fixedIndex = -1, int fixedValue = 0) : Attribute
{
    public Opcode Opcode { get; } = opcode;
    public int FixedIndex { get; } = fixedIndex;
    public int FixedValue { get; } = fixedValue;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class ExpressionInstructionAttribute(Excode opcode) : Attribute
{
    public Excode Opcode { get; } = opcode;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class StorageFieldAttribute(StorageScope scope, StorageType type) : Attribute
{
    public StorageScope Scope { get; } = scope;
    public StorageType Type { get; } = type;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class StorageViewAttribute(StorageScope scope, StorageType type, int address, bool wide = false, bool alternatePrefix = false) : Attribute
{
    public StorageScope Scope { get; } = scope;
    public StorageType Type { get; } = type;
    public int Address { get; } = address;
    public bool Wide { get; } = wide;
    public bool AlternatePrefix { get; } = alternatePrefix;
}

public sealed class ScriptMemory(byte[] globals, byte[] location, byte[] instance)
{
    private static readonly System.Threading.AsyncLocal<ScriptMemory?> Current = new();

    public IDisposable Enter()
    {
        var previous = Current.Value;
        Current.Value = this;
        return new Restore(previous);
    }

    public static ref T View<T>(StorageScope scope) where T : unmanaged =>
        ref System.Runtime.InteropServices.MemoryMarshal.AsRef<T>(Buffer(scope).AsSpan());

    private static readonly System.Collections.Concurrent.ConcurrentDictionary<(Type Owner, string Name), StorageViewAttribute> Views = new();

    public static int ReadView(Type owner, [System.Runtime.CompilerServices.CallerMemberName] string name = "")
    {
        StorageViewAttribute view = GetView(owner, name);
        return Read(view.Scope, view.Type, view.Address);
    }

    public static void WriteView(Type owner, int value, [System.Runtime.CompilerServices.CallerMemberName] string name = "")
    {
        StorageViewAttribute view = GetView(owner, name);
        Write(view.Scope, view.Type, view.Address, value);
    }

    private static StorageViewAttribute GetView(Type owner, string name) => Views.GetOrAdd((owner, name), static key =>
        (StorageViewAttribute?)Attribute.GetCustomAttribute(key.Owner.GetProperty(key.Name,
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static)
            ?? throw new MissingMemberException(key.Owner.FullName, key.Name), typeof(StorageViewAttribute))
        ?? throw new InvalidOperationException($"Missing storage declaration for {key.Owner.FullName}.{key.Name}."));

    public static int Read(StorageScope scope, StorageType type, int address)
    {
        byte[] bytes = Buffer(scope);
        if (type is StorageType.SBit or StorageType.Bit)
            return (bytes[address >> 3] >> (address & 7)) & 1;
        int value = bytes[address];
        return type switch
        {
            StorageType.SByte => (sbyte)value,
            StorageType.Int16 => (short)(value | bytes[address + 1] << 8),
            StorageType.UInt16 => value | bytes[address + 1] << 8,
            StorageType.Int24 or StorageType.UInt24 => value | bytes[address + 1] << 8 | (sbyte)bytes[address + 2] << 16,
            _ => value
        };
    }

    public static void Write(StorageScope scope, StorageType type, int address, int value)
    {
        byte[] bytes = Buffer(scope);
        if (type is StorageType.SBit or StorageType.Bit)
        {
            int mask = 1 << (address & 7);
            ref byte target = ref bytes[address >> 3];
            target = (byte)(value == 0 ? target & ~mask : target | mask);
            return;
        }
        bytes[address] = (byte)value;
        if (type is StorageType.Int16 or StorageType.UInt16 or StorageType.Int24 or StorageType.UInt24)
            bytes[address + 1] = (byte)(value >> 8);
        if (type is StorageType.Int24 or StorageType.UInt24)
            bytes[address + 2] = (byte)(value >> 16);
    }

    private static byte[] Buffer(StorageScope scope)
    {
        ScriptMemory current = Current.Value ?? throw new InvalidOperationException("A script memory context is required.");
        return current.GetBuffer(scope);
    }

    private byte[] GetBuffer(StorageScope scope) => scope switch
    {
        StorageScope.Global => globals,
        StorageScope.Map => location,
        StorageScope.Instance => instance,
        _ => throw new NotSupportedException("Special storage requires an event engine.")
    };

    private sealed class Restore(ScriptMemory? previous) : IDisposable
    {
        public void Dispose() => Current.Value = previous;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class TrailingPaddingAttribute(int count) : Attribute
{
    public int Count { get; } = count;
}

[System.Runtime.CompilerServices.AsyncMethodBuilder(typeof(EventRoutineBuilder))]
public readonly struct EventRoutine(System.Threading.Tasks.Task completion)
{
    public System.Runtime.CompilerServices.TaskAwaiter GetAwaiter() => Completion.GetAwaiter();
    public System.Threading.Tasks.Task Completion { get; } = completion;

    public static EventRoutine Run(Action handler)
    {
        handler();
        return new EventRoutine(System.Threading.Tasks.Task.CompletedTask);
    }
}

public struct EventRoutineBuilder
{
    private System.Runtime.CompilerServices.AsyncTaskMethodBuilder _builder;

    public static EventRoutineBuilder Create() => new() { _builder = System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Create() };
    public EventRoutine Task => new(_builder.Task);
    public void SetResult() => _builder.SetResult();
    public void SetException(Exception error) => _builder.SetException(error);
    public void SetStateMachine(System.Runtime.CompilerServices.IAsyncStateMachine state) => _builder.SetStateMachine(state);

    public void Start<TState>(ref TState state) where TState : System.Runtime.CompilerServices.IAsyncStateMachine =>
        _builder.Start(ref state);

    public void AwaitOnCompleted<TAwaiter, TState>(ref TAwaiter awaiter, ref TState state)
        where TAwaiter : System.Runtime.CompilerServices.INotifyCompletion where TState : System.Runtime.CompilerServices.IAsyncStateMachine =>
        _builder.AwaitOnCompleted(ref awaiter, ref state);

    public void AwaitUnsafeOnCompleted<TAwaiter, TState>(ref TAwaiter awaiter, ref TState state)
        where TAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion where TState : System.Runtime.CompilerServices.IAsyncStateMachine =>
        _builder.AwaitUnsafeOnCompleted(ref awaiter, ref state);
}

public readonly struct EventAwaitable(bool completed, Action<Action> register, Action? onCompletion = null)
{
    public Awaiter GetAwaiter() => new(completed, register, onCompletion);
    internal EventAwaitable OnCompletion(Action continuation) => new(completed, register, continuation);

    public readonly struct Awaiter(bool completed, Action<Action> register, Action? onCompletion) : System.Runtime.CompilerServices.INotifyCompletion
    {
        public bool IsCompleted => completed;
        public void GetResult() => onCompletion?.Invoke();
        public void OnCompleted(Action continuation) => register(continuation);
    }
}

public readonly struct Operand(int value)
{
    public static explicit operator int(Operand operand) => operand.Value;
    public static implicit operator Operand(bool value) => new(value ? 1 : 0);
    public static implicit operator Operand(int value) => new(value);

    private int Value { get; } = value;
}

public static partial class Values
{
    // EBin.SetVariableValue ignores a computed destination tagged as Int26.
    [ExpressionInstruction(Excode.Let)]
    public static int DiscardAssignment(Operand destinationValue, Operand value) => (int)value;
}

public enum SwitchTable { Dense, Explicit, Wide }

public readonly record struct Vertex(Operand X, Operand Z);
public readonly struct Token;

public readonly struct Storage
{
    public VariableRef this[StorageType type, int address] => throw new NotSupportedException();
}

public readonly struct VariableRef
{
    public VariableRef this[int bit] => throw new NotSupportedException();
}

// Intrinsics are read by the event compiler from the C# compilation.
public static class Vm
{
    public static Storage Global => throw new NotSupportedException();
    public static Storage Map => throw new NotSupportedException();
    public static Storage Instance => throw new NotSupportedException();
    public static Storage Special => throw new NotSupportedException();
    public static void Emit(Opcode opcode, byte mask, params Operand[] operands) => throw new NotSupportedException();
    public static void Evaluate(Operand value) { }
    public static void Evaluate(params Token[] tokens) => throw new NotSupportedException();
    public static void WatchAddress(int objectIp) { }
    public static void Label(int label) => throw new NotSupportedException();
    public static void Branch(Opcode opcode, int target) => throw new NotSupportedException();
    public static int Select(SwitchTable table, Operand value, bool lastCaseFallsThrough = false, int[]? caseOrder = null) => (int)value;
    public static void Switch(Opcode opcode, ushort[] values, int defaultTarget, params int[] targets) => throw new NotSupportedException();
    public static void Region(byte mask, Operand vertexCount, params Vertex[] vertices) => throw new NotSupportedException();
    public static Vertex Point(Operand x, Operand z) => new(x, z);
    public static EventAwaitable WaitThenReturn(Operand frames, byte mask) => EventScheduler.WaitThenReturn((int)frames);
    public static void Wait(Operand frames, byte mask) => throw new NotSupportedException();
    public static void SetScale(Operand effect, Operand scale, Operand parameter2, Operand parameter3, byte mask) => throw new NotSupportedException();
    public static void Model(Operand model, Operand height, byte mask) => throw new NotSupportedException();
    public static void Animation(Opcode opcode, Operand animation, byte mask) => throw new NotSupportedException();
    public static void Sps(Opcode opcode, byte mask, Operand effect, Operand command, Operand parameter1, Operand parameter2, Operand parameter3) => throw new NotSupportedException();
    public static void Sound(Opcode opcode, byte mask, Operand command, Operand sound, params Operand[] parameters) => throw new NotSupportedException();
    public static void Wipe(byte mask, Operand mode, Operand time, Operand unknown, Operand cyan, Operand magenta, Operand yellow) => throw new NotSupportedException();
    public static Operand Read(VariableRef reference, bool wide = false, bool alternatePrefix = false) => throw new NotSupportedException();
    public static Operand Value(int value) => value;
    public static int Wide(int value, byte highBits = 0) => value;
    public static Operand Expression(params Token[] tokens) => throw new NotSupportedException();
    public static Token Op(Excode opcode, int data = 0) => throw new NotSupportedException();
    public static Token Literal(int value, bool wide = false, byte highBits = 0) => throw new NotSupportedException();
    public static Token Variable(VariableRef reference, bool wide = false, bool alternatePrefix = false) => throw new NotSupportedException();
    public static Token IsKeyPressed() => throw new NotSupportedException();
}
