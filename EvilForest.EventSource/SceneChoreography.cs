using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EvilForest.EventSource;

public abstract partial class EventCutscene
{
    private EventSceneContext? _choreographyContext;
    private MethodInfo? _declaringChoreography;
    private SceneChoreography? _choreography;

    protected SceneChoreography Choreography(Expression<Func<int>> phase, Expression<Func<bool>> cue, Expression<Func<bool>> pulse)
    {
        ArgumentNullException.ThrowIfNull(pulse);
        return BindChoreography(phase, cue, pulse);
    }

    protected SceneChoreography Choreography(Expression<Func<int>> phase, Expression<Func<bool>> cue) => BindChoreography(phase, cue, null);

    private SceneChoreography BindChoreography(Expression<Func<int>> phase, Expression<Func<bool>> cue, Expression<Func<bool>>? pulse)
    {
        if (_declaringChoreography == null || _choreographyContext == null || _choreography != null)
            throw new InvalidOperationException("Declare one choreography in the cutscene director.");
        _choreography = new SceneChoreography(this, _choreographyContext, _declaringChoreography, phase, cue, pulse);
        return _choreography;
    }

    protected ChoreographyRole Role<T>(T receiver, Func<T, EventRoutine> action) where T : EventParticipant
    {
        ValidateRole(receiver, action);
        return new ChoreographyRole(receiver.Slot, participant => action((T)participant));
    }

    protected ChoreographyRole Role<T>(T receiver, Action<T> action) where T : EventParticipant
    {
        ValidateRole(receiver, action);
        return new ChoreographyRole(receiver.Slot, participant => EventRoutine.Run(() => action((T)participant)));
    }

    protected ChoreographyRole Role<T>(T receiver, Func<T, EventRoutine> action, int thenPhase) where T : EventParticipant
    {
        ValidateRole(receiver, action);
        return new ChoreographyRole(receiver.Slot, participant => action((T)participant), thenPhase);
    }

    protected ChoreographyRole Role<T>(T receiver, Action<T> action, int thenPhase) where T : EventParticipant
    {
        ValidateRole(receiver, action);
        return new ChoreographyRole(receiver.Slot, participant => EventRoutine.Run(() => action((T)participant)), thenPhase);
    }

    protected PhaseBarrier Barrier(Expression<Func<int>> count)
    {
        if (_declaringChoreography == null)
            throw new InvalidOperationException("Declare phase barriers in the cutscene director.");
        return new PhaseBarrier(count);
    }

    public EventRoutine Perform(EventParticipant participant)
    {
        return ForParticipant(participant).Perform(participant);
    }

    public EventRoutine PerformPhase(EventParticipant participant)
    {
        return ForParticipant(participant).PerformPhase(participant);
    }

    public EventRoutine PerformPhase(EventParticipant participant, Action fallback) =>
        ForParticipant(participant).PerformPhase(participant, () => EventRoutine.Run(fallback));

    public EventRoutine PerformPhase(EventParticipant participant, Func<EventRoutine> fallback) =>
        ForParticipant(participant).PerformPhase(participant, fallback);

    private SceneChoreography ForParticipant(EventParticipant participant)
    {
        EventScheduler.RequirePerformingParticipant(participant);
        if (_choreography == null || _choreographyContext == null || !participant.BelongsTo(_choreographyContext))
            throw new InvalidOperationException("Perform choreography on the registered cutscene in the participant's context.");
        return _choreography;
    }

    internal SceneDirector BindChoreography(SceneDirector director, EventSceneContext context) => BindChoreography(director.Method, director.Invoke, context);

    internal SceneDirector BindChoreography(MethodInfo director, Func<EventRoutine> declare, EventSceneContext context)
    {
        if (_choreography != null)
            throw new InvalidOperationException("A cutscene may declare only one choreography.");
        _choreographyContext = context;
        _declaringChoreography = director;
        try
        {
            // Phase declarations complete immediately, so even a participant
            // visited before the director can find its role.
            var declaration = declare().Completion;
            if (!declaration.IsCompleted)
                throw new InvalidOperationException("Choreography registration must complete synchronously.");
            declaration.GetAwaiter().GetResult();
            if (_choreography == null || !_choreography.IsComplete)
                throw new InvalidOperationException("Choreography requires a terminal Finish or Idle phase.");
            _choreography.ValidateFallThrough();
            return _choreography.Direct;
        }
        finally
        {
            _declaringChoreography = null;
        }
    }

    private void ValidateRole<T>(T receiver, Delegate action) where T : EventParticipant
    {
        if (_declaringChoreography == null || _choreographyContext == null || !receiver.BelongsTo(_choreographyContext) ||
            receiver.LiveUid != null || receiver.IsControlledBinding || receiver.GetType() != typeof(T))
            throw new InvalidOperationException("Declare roles on fixed participants in the current cutscene.");
        SceneChoreography.ValidateAction(this, _declaringChoreography, action, typeof(T));
    }
}

public sealed class ChoreographyRole
{
    internal ChoreographyRole(byte slot, Func<EventParticipant, EventRoutine> run, int? thenPhase = null)
    {
        Slot = slot;
        Run = run;
        ThenPhase = thenPhase;
    }

    internal byte Slot { get; }
    internal Func<EventParticipant, EventRoutine> Run { get; }
    internal int? ThenPhase { get; }
}

public sealed class PhaseBarrier
{
    internal PhaseBarrier(Expression<Func<int>> count)
    {
        ReadCount = count.Compile();
        WriteCount = SceneChoreography.Setter(count);
    }

    internal Func<int> ReadCount { get; }
    internal Action<int> WriteCount { get; }
}

public sealed class ChoreographyCase
{
    internal ChoreographyCase(int value, Func<SceneChoreography, EventRoutine> continuation)
    {
        Value = value;
        Continuation = continuation;
    }

    internal int Value { get; }
    internal Func<SceneChoreography, EventRoutine> Continuation { get; }
}

public sealed class SceneChoreography
{
    private readonly EventCutscene _owner;
    private readonly EventSceneContext _context;
    private readonly MethodInfo _director;
    private readonly StorageType _selectorType;
    private readonly Func<int> _readPhase;
    private readonly Action<int> _writePhase;
    private readonly Func<bool> _readCue;
    private readonly Action<bool> _writeCue;
    private readonly (Func<bool> Read, Action<bool> Write)? _pulse;
    private readonly HashSet<int> _phases = new();
    private readonly Dictionary<int, int> _transitions = new();
    private readonly Dictionary<int, ChoiceTransition> _choices = new();
    private readonly Dictionary<(int Phase, byte Slot), Func<EventParticipant, EventRoutine>> _roles = new();
    private readonly Dictionary<(int Phase, byte Slot), int> _roleContinuations = new();
    private readonly Dictionary<int, Func<EventRoutine>> _finishes = new();
    private readonly Dictionary<int, int> _fallThrough = new();
    private readonly Dictionary<MethodInfo, int> _routeEntries = new();
    private readonly HashSet<MethodInfo> _registeringRoutes = new();
    private readonly Queue<Func<SceneChoreography, EventRoutine>> _choiceRoutes = new();
    private readonly Queue<(int Phase, Func<SceneChoreography, EventRoutine> Route)> _repeatRoutes = new();
    private HashSet<MethodInfo>? _validatedRoutes;
    private RouteDeclaration _route = new();
    private Action? _beforePoll;

    internal SceneChoreography(EventCutscene owner, EventSceneContext context, MethodInfo director, Expression<Func<int>> phase, Expression<Func<bool>> cue, Expression<Func<bool>>? pulse)
    {
        _owner = owner;
        _context = context;
        _director = director;
        var selector = ManagedSceneBindings.BindIntegerMapCell(phase, owner, context, "Choreography requires a writable integer map selector on the current context.");
        _selectorType = selector.Type;
        if (pulse != null)
        {
            if (WritableProperty(cue).Member == WritableProperty(pulse).Member)
                throw new InvalidOperationException("Choreography requires separate cue and pulse storage.");
            _pulse = (pulse.Compile(), Setter(pulse));
        }
        _readPhase = () => ScriptMemory.Read(StorageScope.Map, selector.Type, selector.Address);
        _writePhase = value => ScriptMemory.Write(StorageScope.Map, selector.Type, selector.Address, value);
        _readCue = cue.Compile();
        _writeCue = Setter(cue);
    }

    internal bool IsComplete => _route.IsComplete;

    public void BeforePoll(Action work)
    {
        if (_beforePoll != null || _route.Method != null || _route.FirstPhase != null || _route.IsComplete)
            throw new InvalidOperationException("Declare BeforePoll once before the choreography phases.");
        ValidateAction(_owner, _director, work, null);
        _beforePoll = work;
    }

    public EventRoutine Routes(params Func<SceneChoreography, EventRoutine>[] entries)
    {
        if (_route.Method != null || _route.FirstPhase != null || _route.IsComplete)
            throw new InvalidOperationException("Declare Routes immediately after binding the choreography.");
        if (entries == null || entries.Length == 0 || entries.Any(entry => entry == null) || entries.Select(entry => entry.Method).Distinct().Count() != entries.Length)
            throw new InvalidOperationException("Routes requires distinct choreography entry methods.");
        foreach (var entry in entries)
            ValidateAction(_owner, _director, entry, typeof(SceneChoreography));
        _validatedRoutes = ManagedSceneBindings.ValidateChoreographyRoutes(_owner.GetType(), _director, entries.Select(entry => entry.Method));
        foreach (var entry in entries)
            RegisterRoute(entry);
        // Choice continuations can lead back to a phase already declared in this play.
        bool repeats = false;
        while (_choiceRoutes.Count != 0 || _repeatRoutes.Count != 0)
        {
            while (_choiceRoutes.TryDequeue(out var continuation))
                RegisterRoute(continuation);
            if (_repeatRoutes.TryDequeue(out var repeat))
            {
                repeats = true;
                _transitions.Add(repeat.Phase, RegisterRoute(repeat.Route));
            }
        }
        if (entries.Length == 1 && _choices.Count == 0 && !repeats && _fallThrough.Count == 0)
            throw new InvalidOperationException("Routes requires at least two distinct choreography entry methods or a reachable choice or repeat.");
        _route.IsComplete = true;
        return Completed();
    }

    public EventRoutine ContinueWith(Func<SceneChoreography, EventRoutine> continuation)
    {
        if (_route.Method == null || _route.LastPhase is not int phase || _route.IsComplete)
            throw new InvalidOperationException("ContinueWith must end a route with at least one phase.");
        _transitions.Add(phase, RegisterRoute(continuation));
        _route.IsComplete = true;
        return Completed();
    }

    public EventRoutine RepeatFrom(Func<SceneChoreography, EventRoutine> route)
    {
        if (_route.Method == null || _route.LastPhase is not int phase || _route.IsComplete)
            throw new InvalidOperationException("RepeatFrom must end a route with at least one phase.");
        ValidateRoute(route);
        _repeatRoutes.Enqueue((phase, route));
        _route.IsComplete = true;
        return Completed();
    }

    public ChoreographyCase Case(int value, Func<SceneChoreography, EventRoutine> continuation)
    {
        ValidateRoute(continuation);
        return new ChoreographyCase(value, continuation);
    }

    public EventRoutine Choose(Expression<Func<int>> selector, params ChoreographyCase[] cases)
    {
        if (_route.Method == null || _route.LastPhase is not int phase || _route.IsComplete)
            throw new InvalidOperationException("Choose must end a route with at least one phase.");
        var (read, type) = ManagedSceneBindings.BindChoiceSelector(selector, _owner, _context);
        var targets = new Dictionary<int, MethodInfo>();
        if (cases == null || cases.Length == 0)
            throw new InvalidOperationException("Choose requires at least one case.");
        foreach (ChoreographyCase choice in cases)
        {
            if (choice == null || !targets.TryAdd(choice.Value, choice.Continuation.Method))
                throw new InvalidOperationException("Choose requires a unique value for each case.");
        }
        if (ChoreographyChoiceTable.Infer(targets.Keys, type) == null)
            throw new InvalidOperationException(ChoreographyChoiceTable.Diagnostic);
        foreach (ChoreographyCase choice in cases)
            _choiceRoutes.Enqueue(choice.Continuation);
        _choices.Add(phase, new ChoiceTransition(read, targets));
        _route.IsComplete = true;
        return Completed();
    }

    public EventRoutine Together(int phase, params ChoreographyRole[] roles)
    {
        if (roles is not [var role])
            throw new InvalidOperationException("Together requires a barrier when more than one role must complete.");
        return Until(phase, role);
    }

    public EventRoutine Together(int phase, PhaseBarrier barrier, params ChoreographyRole[] roles)
    {
        if (_pulse is not { } pulse)
            throw new InvalidOperationException("A phase barrier requires choreography pulse storage.");
        if (roles.Length == 0)
            throw new InvalidOperationException("A phase barrier requires at least one role.");
        AddPhase(phase);
        for (int index = 0; index < roles.Length; index++)
        {
            ChoreographyRole role = roles[index];
            bool initialize = index == 0;
            AddRole(phase, role, participant => RunThroughBarrier(role, participant, barrier, initialize, roles.Length, pulse));
        }
        return Completed();
    }

    public EventRoutine Until(int phase, ChoreographyRole firstCompletingRole, params ChoreographyRole[] backgroundRoles)
    {
        AddPhase(phase);
        AddRole(phase, firstCompletingRole, participant => RunUntilComplete(firstCompletingRole, participant));
        foreach (ChoreographyRole role in backgroundRoles)
            AddRole(phase, role, role.Run);
        return Completed();
    }

    public EventRoutine RunUntilCue(int phase, params ChoreographyRole[] roles)
    {
        AddPhase(phase);
        foreach (ChoreographyRole role in roles)
            AddRole(phase, role, role.Run);
        return Completed();
    }

    public EventRoutine Finish(int phase, Action finish) => Finish(phase, finish, () => EventRoutine.Run(finish), []);

    public EventRoutine Finish(int phase, Func<EventRoutine> finish) => Finish(phase, finish, finish, []);

    public EventRoutine Finish(int phase, Action finish, params ChoreographyRole[] roles) => Finish(phase, finish, () => EventRoutine.Run(finish), roles);

    public EventRoutine Finish(int phase, Func<EventRoutine> finish, params ChoreographyRole[] roles) => Finish(phase, finish, finish, roles);

    public EventRoutine FallThrough(int phase, Action work, int next, params ChoreographyRole[] roles) => FallThrough(phase, work, () => EventRoutine.Run(work), next, roles);

    public EventRoutine FallThrough(int phase, Func<EventRoutine> work, int next, params ChoreographyRole[] roles) => FallThrough(phase, work, work, next, roles);

    private EventRoutine FallThrough(int phase, Delegate source, Func<EventRoutine> work, int next, ChoreographyRole[] roles)
    {
        EventRoutine declaration = Finish(phase, source, work, roles);
        _fallThrough.Add(phase, next);
        return declaration;
    }

    internal void ValidateFallThrough()
    {
        if (!HasValidFallThrough(_fallThrough, _phases))
            throw new InvalidOperationException("FallThrough must reach a declared phase without a same-tick cycle.");
        foreach (var roles in _roleContinuations.GroupBy(edge => edge.Key.Slot))
        {
            var edges = roles.ToDictionary(edge => edge.Key.Phase, edge => edge.Value);
            var phases = _roles.Keys.Where(role => role.Slot == roles.Key).Select(role => role.Phase).ToHashSet();
            if (!HasValidFallThrough(edges, phases))
                throw new InvalidOperationException("A role continuation must reach a declared phase for the same participant without a same-tick cycle.");
        }
    }

    internal static bool HasValidFallThrough(IReadOnlyDictionary<int, int> edges, IReadOnlySet<int> phases)
    {
        foreach (int phase in edges.Keys)
        {
            var visited = new HashSet<int> { phase };
            int current = phase;
            while (edges.TryGetValue(current, out int next))
            {
                if (!phases.Contains(next) || !visited.Add(next))
                    return false;
                current = next;
            }
        }
        return true;
    }

    public EventRoutine Idle(int phase, params ChoreographyRole[] roles)
    {
        End(phase);
        foreach (ChoreographyRole role in roles)
            AddRole(phase, role, role.Run);
        return Completed();
    }

    private EventRoutine Finish(int phase, Delegate source, Func<EventRoutine> finish, ChoreographyRole[] roles)
    {
        ValidateAction(_owner, _director, source, null);
        End(phase);
        _finishes.Add(phase, finish);
        foreach (ChoreographyRole role in roles)
            AddRole(phase, role, role.Run);
        return Completed();
    }

    private void End(int phase)
    {
        if (_route.Method == null && _route.FirstPhase == null)
            throw new InvalidOperationException("Declare at least one choreography phase before Finish or Idle.");
        AddPhase(phase);
        _route.IsComplete = true;
    }

    internal async EventRoutine Direct()
    {
        while (true)
        {
            _beforePoll?.Invoke();
            _pulse?.Write(false);
            int phase = _readPhase();
            while (_fallThrough.TryGetValue(phase, out int continuation))
            {
                await _finishes[phase]();
                phase = continuation;
            }
            if (_finishes.TryGetValue(phase, out var finish))
            {
                await finish();
            }
            else if (_transitions.TryGetValue(phase, out int next) && _readCue())
            {
                _writePhase(next);
                _writeCue(false);
            }
            else if (_choices.TryGetValue(phase, out var choice) && _readCue())
            {
                if (choice.Targets.TryGetValue(choice.Read(), out MethodInfo? target))
                    _writePhase(_routeEntries[target]);
                _writeCue(false);
            }
            await EventScheduler.Wait(1);
        }
    }

    internal async EventRoutine Perform(EventParticipant participant)
    {
        while (true)
        {
            await PerformPhase(participant);
            await EventScheduler.Wait(1);
        }
    }

    internal EventRoutine PerformPhase(EventParticipant participant, Func<EventRoutine>? fallback = null)
    {
        int phase = _readPhase();
        if (!_roles.TryGetValue((phase, participant.Slot), out var role))
            return fallback?.Invoke() ?? Completed();
        return _roleContinuations.ContainsKey((phase, participant.Slot)) ? RunLinkedRoles(participant, role, phase) : role(participant);
    }

    private async EventRoutine RunLinkedRoles(EventParticipant participant, Func<EventParticipant, EventRoutine> role, int phase)
    {
        while (true)
        {
            await role(participant);
            if (!_roleContinuations.TryGetValue((phase, participant.Slot), out phase))
                return;
            role = _roles[(phase, participant.Slot)];
        }
    }

    private async EventRoutine RunUntilComplete(ChoreographyRole role, EventParticipant participant)
    {
        await role.Run(participant);
        _writeCue(true);
    }

    private async EventRoutine RunThroughBarrier(ChoreographyRole role, EventParticipant participant, PhaseBarrier barrier, bool initialize, int roleCount, (Func<bool> Read, Action<bool> Write) pulse)
    {
        if (initialize)
        {
            barrier.WriteCount(roleCount);
            await EventScheduler.Wait(1);
        }
        await role.Run(participant);
        barrier.WriteCount(barrier.ReadCount() - 1);
        while (barrier.ReadCount() > 0)
        {
            pulse.Write(true);
            await EventScheduler.Wait(1);
        }
        while (pulse.Read())
            await EventScheduler.Wait(1);
        _writeCue(true);
    }

    private void AddRole(int phase, ChoreographyRole role, Func<EventParticipant, EventRoutine> run)
    {
        if (!_roles.TryAdd((phase, role.Slot), run))
            throw new InvalidOperationException($"A participant may have only one role in phase {phase}.");
        if (role.ThenPhase is int next)
        {
            if (run != role.Run)
                throw new InvalidOperationException("A linked role must declare any completion cue in its action.");
            _roleContinuations.Add((phase, role.Slot), next);
        }
    }

    private void AddPhase(int phase)
    {
        if (_route.IsComplete)
            throw new InvalidOperationException("Choreography phases must precede Finish, Idle, ContinueWith or Choose.");
        if (!ChoreographyChoiceTable.CanStore(_selectorType, phase))
            throw new InvalidOperationException($"Declare a phase value that fits the map selector. Phase {phase} is outside {_selectorType} storage.");
        if (!_phases.Add(phase))
            throw new InvalidOperationException($"Choreography phase {phase} is declared more than once.");
        if (_route.LastPhase is int previous)
            _transitions.Add(previous, phase);
        _route.FirstPhase ??= phase;
        _route.LastPhase = phase;
    }

    private int RegisterRoute(Func<SceneChoreography, EventRoutine> declaration)
    {
        ValidateRoute(declaration);
        MethodInfo method = declaration.Method;
        if (_routeEntries.TryGetValue(method, out int entry))
            return entry;
        if (!_registeringRoutes.Add(method))
            throw new InvalidOperationException($"Choreography route {method.Name} forms a cycle.");

        RouteDeclaration parent = _route;
        _route = new RouteDeclaration(method);
        try
        {
            var registration = declaration(this).Completion;
            if (!registration.IsCompleted)
                throw new InvalidOperationException($"Choreography route {method.Name} must register synchronously.");
            registration.GetAwaiter().GetResult();
            if (!_route.IsComplete || _route.FirstPhase is not int first)
                throw new InvalidOperationException($"Choreography route {method.Name} requires a phase and a terminal Finish, Idle, ContinueWith, RepeatFrom or Choose.");
            _routeEntries.Add(method, first);
            return first;
        }
        finally
        {
            _route = parent;
            _registeringRoutes.Remove(method);
        }
    }

    private void ValidateRoute(Func<SceneChoreography, EventRoutine> declaration)
    {
        if (declaration == null || _validatedRoutes?.Contains(declaration.Method) != true)
            throw new InvalidOperationException("Declare each continuation with a direct choreography route method group.");
        ValidateAction(_owner, _director, declaration, typeof(SceneChoreography));
    }

    private static EventRoutine Completed() => new(Task.CompletedTask);

    internal static void ValidateAction(EventCutscene owner, MethodInfo director, Delegate action, Type? participant)
    {
        MethodInfo method = action.Method;
        ParameterInfo[] parameters = method.GetParameters();
        if (!ReferenceEquals(action.Target, owner) || action.GetInvocationList().Length != 1 || method.DeclaringType != owner.GetType() ||
            method == director || method.IsSpecialName || method.IsGenericMethod || method.IsStatic || method.IsVirtual ||
            method.IsDefined(typeof(ScriptAttribute)) || method.IsDefined(typeof(CompilerGeneratedAttribute)) ||
            method.ReturnType == typeof(void) && method.IsDefined(typeof(AsyncStateMachineAttribute)) ||
            (participant == null ? parameters.Length != 0 : parameters is not [var parameter] || parameter.ParameterType != participant))
            throw new InvalidOperationException(participant == typeof(SceneChoreography)
                ? "Use a cutscene method group taking one SceneChoreography parameter."
                : "Use a cutscene method group with the exact role participant, or a parameterless finish method.");
    }

    internal static Action<T> Setter<T>(Expression<Func<T>> field)
    {
        MemberExpression member = WritableProperty(field);
        MethodInfo setter = ((PropertyInfo)member.Member).SetMethod!;
        var value = Expression.Parameter(typeof(T));
        return Expression.Lambda<Action<T>>(Expression.Call(member.Expression, setter, value), value).Compile();
    }

    private static MemberExpression WritableProperty<T>(Expression<Func<T>> field)
    {
        if (field.Body is not MemberExpression { Expression: not null, Member: PropertyInfo
            { GetMethod: { IsVirtual: false }, SetMethod: { IsVirtual: false } } } member)
            throw new InvalidOperationException("Choreography storage requires a writable instance property.");
        return member;
    }

    private sealed class RouteDeclaration(MethodInfo? method = null)
    {
        internal MethodInfo? Method { get; } = method;
        internal int? FirstPhase { get; set; }
        internal int? LastPhase { get; set; }
        internal bool IsComplete { get; set; }
    }

    private sealed record ChoiceTransition(Func<int> Read, Dictionary<int, MethodInfo> Targets);
}
