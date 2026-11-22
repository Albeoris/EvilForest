using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EvilForest.EventSource;

internal static partial class ManagedSceneBindings
{
    internal static (Func<int> Read, StorageType Type) BindChoiceSelector(Expression<Func<int>> selector, EventCutscene owner, EventSceneContext context)
    {
        var cell = BindIntegerMapCell(selector, owner, context, "Choose requires a writable integer map storage property on the current context.");
        return (() => ScriptMemory.Read(StorageScope.Map, cell.Type, cell.Address), cell.Type);
    }

    internal static (StorageType Type, int Address) BindIntegerMapCell(Expression<Func<int>> selector, EventCutscene owner, EventSceneContext context, string diagnostic)
    {
        if (selector?.Body is not MemberExpression { Expression: { } receiver, Member: PropertyInfo
            { GetMethod: { IsVirtual: false }, SetMethod: { IsVirtual: false }, PropertyType: { } type } property } || type != typeof(int) ||
            receiver is not MemberExpression { Expression: { } target, Member: PropertyInfo { GetMethod: { } holder, SetMethod: null } } ||
            !IsMapHolder(holder, ReadContextOrCutscene(target), new HashSet<MethodInfo>()) ||
            ReadIntegerMapCell(property) is not { } cell)
            throw new InvalidOperationException(diagnostic);

        return cell;

        object? ReadContextOrCutscene(Expression expression)
        {
            if (expression is ConstantExpression constant && (ReferenceEquals(constant.Value, owner) || ReferenceEquals(constant.Value, context)))
                return constant.Value;
            if (expression is MemberExpression { Expression: ConstantExpression root, Member: FieldInfo field } &&
                ReferenceEquals(root.Value, owner) && IsContextField(field, owner))
                return context;
            return null;
        }

        bool IsMapHolder(MethodInfo getter, object? target, HashSet<MethodInfo> visited)
        {
            if (getter.IsVirtual || getter.IsStatic || getter.DeclaringType != target?.GetType() || !visited.Add(getter))
                return false;
            var body = AccessorInstructions(getter);
            if (ReferenceEquals(target, context))
            {
                return getter.IsDefined(typeof(CompilerGeneratedAttribute)) &&
                    body is [{ Opcode: var self }, var load, { Opcode: var ret }] && self == OpCodes.Ldarg_0 && ret == OpCodes.Ret &&
                    ReadField(getter, load, OpCodes.Ldfld) is { IsInitOnly: true, IsStatic: false } backing &&
                    backing.FieldType == getter.ReturnType && backing.GetValue(context) != null;
            }

            if (!ReferenceEquals(target, owner) || body.Length < 3 || body[0].Opcode != OpCodes.Ldarg_0 || body[^1].Opcode != OpCodes.Ret ||
                !IsCall(body[^2].Opcode) || Resolve(getter, body[^2].Operand) is not MethodInfo next ||
                !next.IsSpecialName || !next.Name.StartsWith("get_", StringComparison.Ordinal) || next.ReturnType != getter.ReturnType)
                return false;
            if (body.Length == 3)
                return IsMapHolder(next, owner, visited);
            return body.Length == 4 && ReadField(getter, body[1], OpCodes.Ldfld) is { } field && IsContextField(field, owner) &&
                IsMapHolder(next, context, visited);
        }

        bool IsContextField(FieldInfo field, EventCutscene target) => !field.IsStatic && field.DeclaringType == target.GetType() &&
            field.FieldType == context.GetType() && ReferenceEquals(field.GetValue(target), context);
    }

    private static (StorageType Type, int Address)? ReadIntegerMapCell(PropertyInfo property)
    {
        MethodInfo getter = property.GetMethod!;
        MethodInfo setter = property.SetMethod!;
        if (getter.IsStatic || setter.IsStatic || property.GetIndexParameters().Length != 0)
            return null;

        var read = AccessorInstructions(getter);
        var write = AccessorInstructions(setter);
        StorageType type;
        int address;
        if (property.GetCustomAttribute<StorageViewAttribute>() is { } view)
        {
            if (view.Scope != StorageScope.Map || !UsesDeclaredView(getter, read, setter, write, property, view))
                return null;
            type = view.Type;
            address = view.Address;
        }
        else
        {
            if (read.Length != 5 || write.Length != 6 || read[0].Opcode != OpCodes.Ldarg_0 || write[0].Opcode != OpCodes.Ldarg_0 ||
                read[^1].Opcode != OpCodes.Ret || write[^1].Opcode != OpCodes.Ret || write[3].Opcode != OpCodes.Ldarg_1 ||
                !IsCall(read[1].Opcode) || Resolve(getter, read[1].Operand) is not MethodInfo memory ||
                memory.DeclaringType != property.DeclaringType || !memory.IsSpecialName || memory.IsStatic || memory.IsVirtual ||
                !memory.ReturnType.IsByRef || memory.ReturnType.GetElementType() is not { IsExplicitLayout: true } layout ||
                !Calls(setter, write[1], memory) || ReadField(getter, read[2], OpCodes.Ldflda) is not { } field ||
                field.DeclaringType != layout || ReadField(setter, write[2], OpCodes.Ldflda) != field ||
                field.GetCustomAttribute<FieldOffsetAttribute>() is not { } offset ||
                !ReadsMapLayout(memory, layout) || !ReadIntegerCell(field, out type))
                return null;

            PropertyInfo value = field.FieldType.GetProperty("Value")!;
            if (!Calls(getter, read[3], value.GetMethod!) || !Calls(setter, write[4], value.SetMethod!))
                return null;
            address = offset.Value;
        }

        int width = type switch
        {
            StorageType.Byte or StorageType.SByte => 1,
            StorageType.Int16 or StorageType.UInt16 => 2,
            StorageType.Int24 or StorageType.UInt24 => 3,
            _ => 0
        };
        return width != 0 && address >= 0 && address <= 80 - width ? (type, address) : null;
    }

    private static bool UsesDeclaredView(MethodInfo getter, (OpCode Opcode, int Operand)[] read, MethodInfo setter,
        (OpCode Opcode, int Operand)[] write, PropertyInfo property, StorageViewAttribute view)
    {
        if (read.Length != 5 || write.Length != 6 || read[^1].Opcode != OpCodes.Ret || write[^1].Opcode != OpCodes.Ret)
            return false;
        if (LoadsNumber(read[0], (int)view.Scope) && LoadsNumber(read[1], (int)view.Type) && LoadsNumber(read[2], view.Address) &&
            LoadsNumber(write[0], (int)view.Scope) && LoadsNumber(write[1], (int)view.Type) && LoadsNumber(write[2], view.Address) && write[3].Opcode == OpCodes.Ldarg_1)
            return Calls(getter, read[3], typeof(ScriptMemory).GetMethod(nameof(ScriptMemory.Read))!) &&
                Calls(setter, write[4], typeof(ScriptMemory).GetMethod(nameof(ScriptMemory.Write))!);

        MethodInfo getType = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!;
        return read[0].Opcode == OpCodes.Ldtoken && getter.Module.ResolveType(read[0].Operand) == property.DeclaringType &&
            write[0].Opcode == OpCodes.Ldtoken && setter.Module.ResolveType(write[0].Operand) == property.DeclaringType &&
            Calls(getter, read[1], getType) && Calls(setter, write[1], getType) && write[2].Opcode == OpCodes.Ldarg_1 &&
            read[2].Opcode == OpCodes.Ldstr && getter.Module.ResolveString(read[2].Operand) == property.Name &&
            write[3].Opcode == OpCodes.Ldstr && setter.Module.ResolveString(write[3].Operand) == property.Name &&
            Calls(getter, read[3], typeof(ScriptMemory).GetMethod(nameof(ScriptMemory.ReadView))!) &&
            Calls(setter, write[4], typeof(ScriptMemory).GetMethod(nameof(ScriptMemory.WriteView))!);
    }

    private static bool ReadsMapLayout(MethodInfo memory, Type layout)
    {
        var body = AccessorInstructions(memory);
        return body.Length == 3 && LoadsNumber(body[0], (int)StorageScope.Map) && body[2].Opcode == OpCodes.Ret &&
            Calls(memory, body[1], typeof(ScriptMemory).GetMethod(nameof(ScriptMemory.View))!.MakeGenericMethod(layout));
    }

    private static bool ReadIntegerCell(FieldInfo field, out StorageType type)
    {
        Type cell = field.FieldType;
        if (cell == typeof(ByteCell))
            type = StorageType.Byte;
        else if (cell == typeof(SByteCell))
            type = StorageType.SByte;
        else if (cell == typeof(Int16Cell))
            type = StorageType.Int16;
        else if (cell == typeof(UInt16Cell))
            type = StorageType.UInt16;
        else if (cell == typeof(Int24Cell))
            type = field.GetCustomAttribute<StorageEncodingAttribute>()?.Unsigned24 == true ? StorageType.UInt24 : StorageType.Int24;
        else
        {
            type = default;
            return false;
        }
        return true;
    }

    private static (OpCode Opcode, int Operand)[] AccessorInstructions(MethodInfo method) =>
        Instructions(method).Where(instruction => instruction.Opcode != OpCodes.Nop).ToArray();

    private static bool Calls(MethodInfo method, (OpCode Opcode, int Operand) instruction, MethodInfo target) =>
        IsCall(instruction.Opcode) && Resolve(method, instruction.Operand) == target;

    private static FieldInfo? ReadField(MethodInfo method, (OpCode Opcode, int Operand) instruction, OpCode opcode) =>
        instruction.Opcode == opcode ? method.Module.ResolveField(instruction.Operand) : null;

    private static bool LoadsNumber((OpCode Opcode, int Operand) instruction, int value) =>
        instruction.Opcode == OpCodes.Ldc_I4 || instruction.Opcode == OpCodes.Ldc_I4_S ? instruction.Operand == value :
        instruction.Opcode.Value >= OpCodes.Ldc_I4_M1.Value && instruction.Opcode.Value <= OpCodes.Ldc_I4_8.Value &&
        instruction.Opcode.Value - OpCodes.Ldc_I4_0.Value == value;
}
