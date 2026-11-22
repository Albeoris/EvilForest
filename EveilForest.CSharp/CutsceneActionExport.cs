using FF8.JSM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    // The engine invokes the standard entry points independently of bytecode requests.
    private const int LastEngineCallback = 10;
    private const int SequenceUidOffset = 64;
    private const string CallAnnotation = "DecodedSceneCall";

    private readonly Dictionary<Entry, HashSet<SyntaxAnnotation>> _actionCandidates = new();
    private readonly Dictionary<SyntaxAnnotation, ActionCall> _actionCalls = new();
    private readonly Dictionary<(int Slot, int Ordinal, int Position), SyntaxAnnotation> _callSites = new();
    private readonly Dictionary<Entry, MethodDeclarationSyntax> _actionSources = new();
    private readonly HashSet<SyntaxAnnotation> _retainedCalls = new();
    private readonly IReadOnlyList<CutsceneCatalogEntry> _actionNames;
    private readonly Dictionary<(string Group, string Method), string> _renamedGroups = new();

    private sealed record ActionCall(Entry Target, Jsm.Opcode Opcode, int Uid);

    private void IndexActionCandidates(Handler[] handlers, Dictionary<Entry, int> counts, Request[] requests)
    {
        if (Scenes.Count == 0)
            return;

        var instructions = handlers.SelectMany(handler => handler.Body.Instructions.Select(step => (Handler: handler, Step: step))).ToArray();
        var sequences = instructions.Where(item => item.Step.Opcode == Jsm.Opcode.STARTSEQ).ToArray();
        bool indirectSequence = sequences.Any(item => item.Step.Operands[0].Expression != null);

        var uncertainTags = requests.Where(request => request.Receiver == null).Select(request => request.Tag).ToHashSet();
        uncertainTags.UnionWith(instructions.Where(item => item.Step.Opcode is Jsm.Opcode.REPLY or Jsm.Opcode.REPLYSW or Jsm.Opcode.REPLYEW)
            .Select(item => item.Step.Operands[1].Value));
        var blocked = new HashSet<Entry>();
        foreach (Request request in requests.Where(request => request.Receiver.HasValue))
        {
            int uid = request.Receiver!.Value;
            if (!_liveParticipants.Definitions.TryGetValue(uid, out var definitions))
                continue;
            var targets = definitions.Select(slot => new Entry(slot, (ushort)request.Tag)).Where(counts.ContainsKey).ToArray();
            if (definitions.Count != 1 || targets is not [var target] || !CanMove(target) ||
                target.Tag <= LastEngineCallback || uncertainTags.Contains(target.Tag))
            {
                blocked.UnionWith(targets);
                continue;
            }
            AddCall(request.Caller.Slot, request.Ordinal, request.Position, new ActionCall(target, request.Mode, uid));
        }

        foreach (var (handler, step) in sequences)
        {
            var selector = step.Operands[0];
            if (selector.Expression != null)
                continue;
            var target = new Entry(selector.Value, 0);
            if (CanMove(target) && !_liveParticipants.SpawnedSlots.Contains(target.Slot) && !indirectSequence && !uncertainTags.Contains(target.Tag))
                AddCall(handler.Entry.Slot, handler.Ordinal, step.Label, new ActionCall(target, step.Opcode, target.Slot));
        }
        foreach (Entry entry in blocked)
            _actionCandidates.Remove(entry);

        bool CanMove(Entry target) => counts.GetValueOrDefault(target) == 1 && !Scenes.Any(scene => scene.For(target) != null);


        void AddCall(int slot, int ordinal, int position, ActionCall call)
        {
            if (!_actionCandidates.TryGetValue(call.Target, out var incoming))
                _actionCandidates.Add(call.Target, incoming = new());
            var site = new SyntaxAnnotation(CallAnnotation);
            incoming.Add(site);
            _callSites.Add((slot, ordinal, position), site);
            _actionCalls.Add(site, call);
        }
    }

    private MethodDeclarationSyntax AnnotateCalls(MethodDeclarationSyntax method, int slot, int ordinal, int statementOffset, IReadOnlyDictionary<int, int> callOffsets)
    {
        return method.ReplaceNodes(method.DescendantNodes().OfType<ExpressionStatementSyntax>(), (original, rewritten) =>
        {
            if (!callOffsets.TryGetValue(original.SpanStart - statementOffset, out int position) ||
                !_callSites.TryGetValue((slot, ordinal, position), out var site))
                return rewritten;
            ExpressionSyntax expression = rewritten.Expression is AwaitExpressionSyntax awaiter ? awaiter.Expression : rewritten.Expression;
            return expression is InvocationExpressionSyntax call ? rewritten.ReplaceNode(call, call.WithAdditionalAnnotations(site)) : rewritten;
        });
    }

    private static IEnumerable<(InvocationExpressionSyntax Call, SyntaxAnnotation Site)> AnnotatedCalls(MethodDeclarationSyntax method) =>
        method.GetAnnotatedNodes(CallAnnotation).OfType<InvocationExpressionSyntax>()
            .SelectMany(call => call.GetAnnotations(CallAnnotation).Select(site => (Call: call, Site: site)));

    public void FinalizeOwnedActions()
    {
        var callsByScene = Scenes.ToDictionary(scene => scene, scene => scene.Methods.SelectMany(AnnotatedCalls).ToArray(),
            (IEqualityComparer<Scene>)ReferenceEqualityComparer.Instance);
        var rootCounts = callsByScene.Values.SelectMany(calls => calls).GroupBy(pair => pair.Site).ToDictionary(group => group.Key, group => group.Count());
        var callsByAction = _actionSources.ToDictionary(pair => pair.Key, pair => ReadCalls(AnnotatedCalls(pair.Value)).ToHashSet());
        var owners = new Dictionary<Entry, Scene>();
        foreach (Scene scene in Scenes)
        {
            var roots = ReadCalls(callsByScene[scene].Where(pair => rootCounts[pair.Site] == 1 && !_retainedCalls.Contains(pair.Site))).ToHashSet();
            var covered = roots.ToHashSet();
            var candidates = new HashSet<Entry>();
            var pending = new Queue<SyntaxAnnotation>(roots);
            while (pending.TryDequeue(out var site))
            {
                Entry target = _actionCalls[site].Target;
                if (!callsByAction.TryGetValue(target, out var children) || !candidates.Add(target))
                    continue;
                foreach (SyntaxAnnotation child in children)
                {
                    if (covered.Add(child))
                        pending.Enqueue(child);
                }
            }

            // Every incoming edge must be emitted in this scene, or belong to another
            // action that can move with it. Pruning also handles mutually recursive helpers.
            while (candidates.RemoveWhere(entry => !_actionCandidates[entry].IsSubsetOf(covered)) != 0)
            {
                covered = roots.ToHashSet();
                covered.UnionWith(candidates.SelectMany(entry => callsByAction[entry]));
            }
            foreach (Entry entry in candidates)
                owners.Add(entry, scene);
        }

        ApplyActionGroups(owners);
        foreach (Scene scene in Scenes)
        {
            var used = Scenes.Where(other => other.Name == scene.Name).SelectMany(other => other.Methods.Select(method => method.Identifier.ValueText)).ToHashSet(StringComparer.Ordinal);
            used.UnionWith([scene.Name, "On", "Track", "Together", "Choreography", "Role", "Perform", "PerformPhase", "ctx", "Location", "Globals", "Jobs"]);
            foreach (Entry entry in owners.Where(pair => ReferenceEquals(pair.Value, scene)).Select(pair => pair.Key)
                .OrderBy(entry => entry.Slot).ThenBy(entry => Array.FindIndex(_symbols.Participants[entry.Slot].Scripts, script => script.Id == entry.Tag)))
            {
                CutsceneCatalogEntry? row = _actionNames.SingleOrDefault(row => row.Slot == entry.Slot && row.Tag == entry.Tag);
                string fallback = $"Action_{entry.Slot:D2}_{entry.Tag:D2}";
                string name = row?.Name ?? fallback;
                if (!SyntaxFacts.IsValidIdentifier(name) || used.Contains(name))
                {
                    if (row != null)
                        _outline.Add($"{entry.Slot}\t{entry.Tag}\tCatalog entry\t\tInvalid or conflicting action name");
                    name = fallback;
                }
                name = ReserveName(name, used);
                scene.Actions.Add(entry, name);
                MethodDeclarationSyntax method = _actionSources[entry];
                string participant = _symbols.Participants[entry.Slot].Name;
                scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name))
                    .WithParameterList(SyntaxFactory.ParseParameterList($"({participant} participant)"))
                    .WithModifiers(Modifiers("private", method)).WithBody((BlockSyntax)new RewriteBody().Visit(method.Body!)!));
            }
            for (int index = 0; index < scene.Methods.Count; index++)
            {
                MethodDeclarationSyntax method = scene.Methods[index];
                var edits = AnnotatedCalls(method).Where(pair => scene.Actions.ContainsKey(_actionCalls[pair.Site].Target))
                    .ToDictionary(pair => pair.Call, pair => _actionCalls[pair.Site]);
                scene.Methods[index] = method.ReplaceNodes(edits.Keys, (original, rewritten) =>
                    BindAction(rewritten, edits[original], scene.Actions[edits[original].Target]));
            }
        }

        foreach (CutsceneCatalogEntry row in _actionNames.Where(row => !owners.ContainsKey(new Entry(row.Slot, row.Tag))))
            _outline.Add($"{row.Slot}\t{row.Tag}\tCatalog entry\t\tStale or ambiguous catalog entry");

        if (_renamedGroups.Count != 0)
        {
            foreach (Scene scene in Scenes)
            {
                for (int index = 0; index < scene.Methods.Count; index++)
                    scene.Methods[index] = RenameSceneReferences(scene.Methods[index]);
            }
        }

        IEnumerable<SyntaxAnnotation> ReadCalls(IEnumerable<(InvocationExpressionSyntax Call, SyntaxAnnotation Site)> calls) =>
            calls.GroupBy(pair => pair.Site).Where(group => group.Count() == 1 && CanBind(group.Single().Call, _actionCalls[group.Key]))
                .Select(group => group.Key).Where(site => _actionCandidates.ContainsKey(_actionCalls[site].Target));
    }

    private void ApplyActionGroups(IReadOnlyDictionary<Entry, Scene> owners)
    {
        var labels = _actionNames.Where(row => row.Group != null && owners.ContainsKey(new Entry(row.Slot, row.Tag)))
            .GroupBy(row => owners[new Entry(row.Slot, row.Tag)], (IEqualityComparer<Scene>)ReferenceEqualityComparer.Instance);
        foreach (var labeled in labels)
        {
            Scene scene = labeled.Key;
            var members = MemberNames(scene).ToHashSet(StringComparer.Ordinal);
            string[] groups = labeled.Select(row => row.Group!).Distinct(StringComparer.Ordinal).ToArray();
            if (groups is not [var name] || !IsValidGroup(name, _symbols) || members.Contains(name) ||
                (scene.Name != scene.DefaultName && scene.Name != name) ||
                Scenes.Any(other => !ReferenceEquals(other, scene) && other.Name == name && MemberNames(other).Any(members.Contains)))
            {
                foreach (CutsceneCatalogEntry row in labeled)
                    _outline.Add($"{row.Slot}\t{row.Tag}\tCatalog entry\t\tInvalid or conflicting group name");
                continue;
            }
            if (scene.Name == name)
                continue;
            foreach (string method in scene.Methods.Select(method => method.Identifier.ValueText).Distinct(StringComparer.Ordinal))
                _renamedGroups.Add((scene.Name, method), name);
            if (scene.DirectorMachine?.Choreography != null)
                _renamedGroups.Add((scene.Name, "Perform"), name);
            scene.Name = name;
        }

        static IEnumerable<string> MemberNames(Scene scene)
        {
            foreach (MethodDeclarationSyntax method in scene.Methods)
                yield return method.Identifier.ValueText;
            foreach (MemberDeclarationSyntax binding in scene.Bindings)
            {
                if (binding is PropertyDeclarationSyntax property)
                    yield return property.Identifier.ValueText;
                else if (binding is TypeDeclarationSyntax type)
                    yield return type.Identifier.ValueText;
            }
        }
    }

    private T RenameSceneReferences<T>(T root) where T : SyntaxNode
    {
        var updates = root.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
            .Where(member => member.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "ctx" } } binding &&
                _renamedGroups.ContainsKey((binding.Name.Identifier.ValueText, member.Name.Identifier.ValueText)));
        return root.ReplaceNodes(updates, (original, rewritten) =>
        {
            var binding = (MemberAccessExpressionSyntax)original.Expression;
            var current = (MemberAccessExpressionSyntax)rewritten.Expression;
            string group = _renamedGroups[(binding.Name.Identifier.ValueText, original.Name.Identifier.ValueText)];
            return rewritten.WithExpression(current.WithName(SyntaxFactory.IdentifierName(group).WithTriviaFrom(current.Name)));
        });
    }

    private bool CanBind(InvocationExpressionSyntax call, ActionCall action)
    {
        if (call.Expression is not MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "scripts" } } service ||
            call.ArgumentList.Arguments.FirstOrDefault() is not { NameColon: null, Expression: MemberAccessExpressionSyntax receiver })
            return false;
        if (action.Opcode == Jsm.Opcode.STARTSEQ)
            return service.Name.Identifier.ValueText == "RunSharedScript" && receiver.ToString() == $"evt.{_symbols.Participants[action.Target.Slot].Name}";
        string? method = _symbols.HandlerReceiver(action.Target.Slot, action.Target.Tag);
        ArgumentSyntax? instance = call.ArgumentList.Arguments.FirstOrDefault(argument => argument.NameColon?.Name.Identifier.ValueText == "instance");
        bool matchesUid = instance == null ? action.Uid == action.Target.Slot : instance.Expression is ObjectCreationExpressionSyntax
        {
            Type: IdentifierNameSyntax { Identifier.ValueText: "InstanceUid" },
            ArgumentList.Arguments: [{ Expression: LiteralExpressionSyntax { Token.Value: int uid } }]
        } && uid == action.Uid;
        return matchesUid && receiver.ToString() == method && service.Name.Identifier.ValueText == (action.Opcode switch
        {
            Jsm.Opcode.REQ => "RequestAsync",
            Jsm.Opcode.REQSW => "RequestWhenReady",
            _ => "RequestAndWait"
        });
    }

    private static InvocationExpressionSyntax BindAction(InvocationExpressionSyntax call, ActionCall action, string name)
    {
        var service = (MemberAccessExpressionSyntax)call.Expression;
        var arguments = call.ArgumentList.Arguments;
        var first = arguments[0];
        var member = (MemberAccessExpressionSyntax)first.Expression;
        ExpressionSyntax receiver = action.Opcode == Jsm.Opcode.STARTSEQ ? member : member.Expression;
        ArgumentSyntax? instance = arguments.FirstOrDefault(argument => argument.NameColon?.Name.Identifier.ValueText == "instance");
        int instanceIndex = instance == null ? -1 : arguments.IndexOf(instance);
        string on = instance == null ? $"On({receiver}, {name})" : $"On({receiver}, {name}, {instance.Expression})";
        arguments = arguments.Replace(first, first.WithExpression(SyntaxFactory.ParseExpression(on).WithTriviaFrom(first.Expression)));
        if (instanceIndex >= 0)
            arguments = arguments.RemoveAt(instanceIndex);

        string method = action.Opcode switch
        {
            Jsm.Opcode.STARTSEQ => "RunSharedScript",
            Jsm.Opcode.REQ => "TryStart",
            Jsm.Opcode.REQSW => "StartWhenReady",
            _ => "RunAndWait"
        };
        return call.WithExpression(service.WithName(SyntaxFactory.IdentifierName(method))).WithArgumentList(call.ArgumentList.WithArguments(arguments));
    }

    public void FinalizeParticipantSources(string directory, IReadOnlyDictionary<int, string> sources)
    {
        var owners = Scenes.SelectMany(scene => scene.Actions.Keys).GroupBy(entry => entry.Slot).ToDictionary(group => group.Key, group => group.ToArray());
        if (owners.Count == 0)
            return;
        var files = owners.ToDictionary(pair => sources[pair.Key], pair => (IEnumerable<Entry>)pair.Value);
        if (_renamedGroups.Count != 0)
        {
            foreach (string source in sources.Values)
                files.TryAdd(source, Array.Empty<Entry>());
        }
        if (_symbols.Names is { } names)
            files.Add("Scene_Interactions.cs", owners.Values.SelectMany(group => group).Where(entry => names.ComposedHandlers.Contains((entry.Slot, entry.Tag))));
        foreach (var (file, transfers) in files)
        {
            string path = Path.Combine(directory, file);
            SyntaxNode root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var group in transfers.GroupBy(entry => entry.Slot))
            {
                string name = _symbols.Participants[group.Key].Name;
                var owner = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(node => node.Identifier.ValueText == name);
                var removed = group.Select(entry => _actionSources[entry].Identifier.ValueText).ToHashSet(StringComparer.Ordinal);
                var members = owner.Members.Where(member => member is not MethodDeclarationSyntax method || !removed.Contains(method.Identifier.ValueText));
                if (file == sources[group.Key] && group.Any(entry => _actionSources[entry].DescendantNodes().Any(node =>
                    node is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Local" } })))
                    members = members.Select(ExposeLocalMemory);
                root = root.ReplaceNode(owner, owner.WithMembers(SyntaxFactory.List(members)));
            }
            if (_renamedGroups.Count != 0)
                root = RenameSceneReferences(root);
            File.WriteAllText(path, root.ToFullString());
        }
    }

    private static MemberDeclarationSyntax ExposeLocalMemory(MemberDeclarationSyntax member)
    {
        if (member is not ClassDeclarationSyntax { Identifier.ValueText: "LocalMemory" } &&
            member is not PropertyDeclarationSyntax { Identifier.ValueText: "Local" })
            return member;
        return member.ReplaceTokens(member.ChildTokens().Where(token => token.IsKind(SyntaxKind.PrivateKeyword)),
            (token, _) => SyntaxFactory.Token(token.LeadingTrivia, SyntaxKind.InternalKeyword, token.TrailingTrivia));
    }

    public string ActionKind(Entry entry) => _symbols.Participants[entry.Slot].Kind switch
    {
        1 => "region",
        2 => "actor",
        _ => "thread"
    };
}
