using System.Buffers.Binary;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EveilForest.CSharp")]

namespace EvilForest.EventSource;

public delegate EventRoutine SceneDirector();

[AttributeUsage(AttributeTargets.Method)]
public sealed class OriginalDirectorSlotAttribute(int slot) : Attribute
{
    public int Slot { get; } = slot;
}

public sealed partial class ScriptService
{
    public void StartDirector(SceneDirector director) => EventScheduler.StartDirector(this, director);
}

internal static class SceneDirectorSlots
{
    internal static Dictionary<string, int> Assign(IEnumerable<int> occupiedSlots, IEnumerable<(string Name, int? OriginalSlot)> directors)
    {
        var occupied = occupiedSlots.ToHashSet();
        var slots = new Dictionary<string, int>(StringComparer.Ordinal);
        var ordered = directors.OrderBy(director => director.Name, StringComparer.Ordinal).ToArray();
        foreach (var (name, original) in ordered)
        {
            if (original is not int slot)
                continue;
            if (slot is < 1 or >= 250 || !occupied.Add(slot))
                throw new InvalidOperationException($"Invalid or duplicate director slot {slot} for {name}.");
            slots.Add(name, slot);
        }

        int next = 1;
        foreach (var (name, original) in ordered)
        {
            if (original.HasValue)
                continue;
            while (occupied.Contains(next))
                next++;
            if (next >= 250)
                throw new InvalidOperationException($"No free object slot for director {name}.");
            slots.Add(name, next++);
        }

        return slots;
    }
}

internal static partial class ManagedSceneBindings
{
    private static readonly Dictionary<short, OpCode> OpCodesByValue = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(OpCode))
        .Select(field => (OpCode)field.GetValue(null)!)
        .ToDictionary(opcode => opcode.Value);
    private static readonly MethodInfo StartMethod = typeof(ScriptService).GetMethod(nameof(ScriptService.StartDirector))!;
    private static bool IsChoreographyMethod(MethodBase? method) => method?.DeclaringType == typeof(EventCutscene) && method.Name == "Choreography";

    internal static bool DeclaresChoreography(MethodInfo director)
    {
        Type? state = director.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType;
        MethodInfo body = state?.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? director;
        var instructions = Instructions(body).ToArray();
        if (!instructions.Any(instruction => IsCall(instruction.Opcode) && IsChoreographyMethod(Resolve(body, instruction.Operand))))
            return false;

        if (state == null || instructions.Any(instruction => !IsDeclarationInstruction(body, state, instruction.Opcode, instruction.Operand, route: false)))
            throw new InvalidOperationException($"Choreography director {director.Name} must contain only scene bindings and phase declarations.");
        return true;
    }

    internal static HashSet<MethodInfo> ValidateChoreographyRoutes(Type owner, MethodInfo director, IEnumerable<MethodInfo> entries)
    {
        var validated = new HashSet<MethodInfo>();
        foreach (MethodInfo entry in entries)
            Visit(entry);
        return validated;

        void Visit(MethodInfo method)
        {
            if (!validated.Add(method))
                return;

            Type? state = method.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType;
            MethodInfo? body = state?.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (body == null || method.DeclaringType != owner || method == director || method.IsStatic || method.IsVirtual ||
                method.IsSpecialName || method.IsGenericMethod || method.ReturnType != typeof(EventRoutine) ||
                method.IsDefined(typeof(ScriptAttribute)) || method.IsDefined(typeof(CompilerGeneratedAttribute)) ||
                method.GetParameters() is not [var parameter] || parameter.ParameterType != typeof(SceneChoreography))
                throw new InvalidOperationException($"Invalid choreography route method {method.Name}.");

            // Validate every reachable declaration before invoking the first one.
            // Normal SDK builds have IL but no source tree to inspect.
            var instructions = Instructions(body).ToArray();
            if (instructions.Any(instruction => !IsDeclarationInstruction(body, state!, instruction.Opcode, instruction.Operand, route: true)))
                throw new InvalidOperationException($"Choreography route {method.Name} must contain only scene bindings and phase declarations.");
            for (int index = 1; index < instructions.Length; index++)
            {
                var (opcode, operand) = instructions[index];
                var (previous, target) = instructions[index - 1];
                if (opcode == OpCodes.Newobj && (previous == OpCodes.Ldftn || previous == OpCodes.Ldvirtftn) &&
                    Resolve(body, operand)?.DeclaringType == typeof(Func<SceneChoreography, EventRoutine>) && Resolve(body, target) is MethodInfo continuation)
                    Visit(continuation);
            }
        }
    }

    private static bool IsDeclarationInstruction(MethodInfo body, Type state, OpCode opcode, int operand, bool route)
    {
        if (opcode.OperandType is OperandType.InlineBrTarget or OperandType.ShortInlineBrTarget && operand < 0)
            return false;
        if (IsCall(opcode))
            return IsDeclarationCall(Resolve(body, operand), route);
        if (opcode == OpCodes.Newobj)
        {
            Type? type = Resolve(body, operand)?.DeclaringType;
            return type == typeof(Action) || type?.IsConstructedGenericType == true &&
                (type.GetGenericTypeDefinition() == typeof(Action<>) || type.GetGenericTypeDefinition() == typeof(Func<>) ||
                 type.GetGenericTypeDefinition() == typeof(Func<,>));
        }
        if (opcode == OpCodes.Stfld)
            return body.Module.ResolveField(operand)?.DeclaringType == state;
        if (opcode == OpCodes.Initobj)
            return body.Module.ResolveType(operand) == typeof(TaskAwaiter);
        return opcode != OpCodes.Calli && opcode != OpCodes.Stobj && opcode != OpCodes.Cpobj && opcode != OpCodes.Cpblk &&
            opcode != OpCodes.Initblk && opcode != OpCodes.Ldsfld && opcode != OpCodes.Ldsflda && opcode != OpCodes.Stsfld &&
            opcode.Name?.StartsWith("stind.", StringComparison.Ordinal) != true;
    }

    private static bool IsCall(OpCode opcode) => opcode == OpCodes.Call || opcode == OpCodes.Callvirt;

    private static bool IsDeclarationCall(MethodBase? operation, bool route)
    {
        if (operation is not MethodInfo method || method.DeclaringType is not { } type)
            return false;
        if (type == typeof(EventCutscene))
            return !route && IsChoreographyMethod(method) || method.Name is "Role" or "Barrier";
        if (type == typeof(SceneChoreography))
            return method.Name is "Together" or "Until" or "RunUntilCue" or "Finish" or "FallThrough" or "Idle" ||
                (route ? method.Name is "ContinueWith" or "RepeatFrom" or "Choose" or "Case" : method.Name is "Routes" or "BeforePoll");
        if (type == typeof(EventRoutineBuilder))
            return method.Name is nameof(EventRoutineBuilder.AwaitUnsafeOnCompleted) or nameof(EventRoutineBuilder.SetResult) or nameof(EventRoutineBuilder.SetException);
        if (type == typeof(TaskAwaiter))
            return method.Name is "get_IsCompleted" or nameof(TaskAwaiter.GetResult);
        if (type == typeof(Expression))
            return method.Name is nameof(Expression.Constant) or nameof(Expression.Field) or nameof(Expression.Property) or nameof(Expression.Lambda);
        if (type == typeof(EventRoutine) && method.Name == nameof(EventRoutine.GetAwaiter) ||
            type == typeof(Array) && method.Name == nameof(Array.Empty) ||
            type == typeof(Type) && method.Name == nameof(Type.GetTypeFromHandle) ||
            type == typeof(MethodBase) && method.Name == nameof(MethodBase.GetMethodFromHandle) ||
            type == typeof(FieldInfo) && method.Name == nameof(FieldInfo.GetFieldFromHandle))
            return true;

        return method.IsSpecialName && method.Name.StartsWith("get_", StringComparison.Ordinal) && !method.IsStatic && !method.IsVirtual &&
            method.IsDefined(typeof(CompilerGeneratedAttribute)) &&
            type.GetProperty(method.Name[4..], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is { SetMethod: null } &&
            (type.IsSubclassOf(typeof(EventSceneContext)) && method.ReturnType.IsDefined(typeof(SceneRootAttribute)) ||
             type.IsDefined(typeof(SceneRootAttribute)) && method.ReturnType.IsSubclassOf(typeof(EventParticipant)));
    }

    internal static PropertyInfo[] ReadCutscenes(Type context)
    {
        const BindingFlags instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        const BindingFlags privateFields = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        ConstructorInfo? constructor = context.GetConstructor(instance, binder: null, Type.EmptyTypes, modifiers: null);
        if (constructor == null || context.IsAbstract || !context.IsSubclassOf(typeof(EventSceneContext)))
            throw new InvalidOperationException("The event scene must supply a constructible EventSceneContext.");

        var types = new HashSet<Type>();
        var bindings = new List<PropertyInfo>();
        var instructions = Instructions(constructor).ToArray();
        foreach (PropertyInfo property in context.GetProperties(instance | BindingFlags.Static))
        {
            Type cutscene = property.PropertyType;
            if (!cutscene.IsSubclassOf(typeof(EventCutscene)) && !property.IsDefined(typeof(SceneCutsceneAttribute)))
                continue;
            if (!cutscene.IsSubclassOf(typeof(EventCutscene)) || cutscene.IsAbstract ||
                property.GetMethod is not { IsStatic: false, IsVirtual: false } getter || !getter.IsDefined(typeof(CompilerGeneratedAttribute)) ||
                property.SetMethod != null || context.GetField($"<{property.Name}>k__BackingField", privateFields) is not { IsInitOnly: true } backing ||
                !backing.IsDefined(typeof(CompilerGeneratedAttribute)) || !InitializedOnce(backing, cutscene) || !types.Add(cutscene))
                throw new InvalidOperationException($"Expected a unique get-only cutscene property initialized once with new CutsceneType(this): {property.Name}.");
            bindings.Add(property);
        }
        return bindings.ToArray();

        bool InitializedOnce(FieldInfo backing, Type cutscene)
        {
            // A readonly auto-property assignment emits one backing-field store.
            // The field receiver and constructor argument must both be `this`.
            var stores = Enumerable.Range(0, instructions.Length).Where(index =>
                instructions[index].Opcode == OpCodes.Stfld && constructor.Module.ResolveField(instructions[index].Operand) == backing).ToArray();
            return stores is [var store] && store >= 3 && instructions[store - 3].Opcode == OpCodes.Ldarg_0 &&
                instructions[store - 2].Opcode == OpCodes.Ldarg_0 && instructions[store - 1].Opcode == OpCodes.Newobj &&
                Resolve(constructor, instructions[store - 1].Operand) is ConstructorInfo creation && creation.DeclaringType == cutscene &&
                creation.GetParameters().Length == 1;
        }
    }

    internal static HashSet<MethodInfo> ReadDirectors(IEnumerable<Type> projectTypes)
    {
        // SDK-built projects have no separate entry manifest. A direct method
        // group passed to StartDirector constructs the dedicated delegate in IL.
        const BindingFlags members = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        var directors = new HashSet<MethodInfo>();
        foreach (Type type in projectTypes)
        {
            foreach (MethodInfo method in type.GetMethods(members))
            {
                ReadMethodGroups(method, directors);
            }
        }
        return directors;
    }

    private static void ReadMethodGroups(MethodInfo method, HashSet<MethodInfo> directors)
    {
        int target = 0;
        int bound = 0;
        foreach (var (opcode, operand) in Instructions(method))
        {
            if (bound != 0 && (opcode == OpCodes.Call || opcode == OpCodes.Callvirt) &&
                Resolve(method, operand) == StartMethod && Resolve(method, bound) is MethodInfo director)
                directors.Add(director);

            bound = 0;
            if (opcode == OpCodes.Newobj && target != 0)
            {
                MethodBase? constructor = Resolve(method, operand);
                if (constructor?.DeclaringType == typeof(SceneDirector))
                    bound = target;
            }
            target = opcode == OpCodes.Ldftn || opcode == OpCodes.Ldvirtftn ? operand : 0;
        }
    }

    private static IEnumerable<(OpCode Opcode, int Operand)> Instructions(MethodBase method)
    {
        if (method.GetMethodBody()?.GetILAsByteArray() is not { } bytes)
            yield break;
        int offset = 0;
        while (offset < bytes.Length)
        {
            short code = bytes[offset++];
            if (code == 0xFE)
                code = unchecked((short)(0xFE00 | bytes[offset++]));
            OpCode opcode = OpCodesByValue[code];
            int size = opcode.OperandType switch
            {
                OperandType.InlineNone => 0,
                OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
                OperandType.InlineVar => 2,
                OperandType.InlineI8 or OperandType.InlineR => 8,
                OperandType.InlineSwitch => 4 + sizeof(int) * BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(offset)),
                _ => 4
            };
            int operand = opcode.OperandType switch
            {
                OperandType.ShortInlineBrTarget or OperandType.ShortInlineI => unchecked((sbyte)bytes[offset]),
                OperandType.InlineSwitch => 0,
                _ => size == 4 ? BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(offset)) : 0
            };
            offset += size;
            yield return (opcode, operand);
        }
    }

    private static MethodBase? Resolve(MethodBase method, int token) =>
        method.Module.ResolveMethod(token, method.DeclaringType?.GetGenericArguments(), (method as MethodInfo)?.GetGenericArguments());
}
