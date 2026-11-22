using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private readonly Dictionary<int, HashSet<int>> _possibleDefinitions;
    private readonly string _eventName;
    private readonly CutsceneCatalog? _catalog;

    private sealed record RoleSwitch(SwitchStatementSyntax Switch, string Storage, StorageViewAttribute View, int[] Receivers, int Level, int Tag);

    private BlockSyntax AddRoleDispatch(Scene scene, MethodDeclarationSyntax handler, BlockSyntax body, Entry key, int phase)
    {
        if (scene.Bindings.Count != 0)
            return body;
        RoleSwitch[] roles = body.Statements.OfType<SwitchStatementSyntax>()
            .Select(selection => ReadRoleSwitch(selection, handler)).OfType<RoleSwitch>().ToArray();
        if (roles is not [var first, ..] || roles.Select(role => role.Storage).Distinct().Count() != roles.Length ||
            roles.Any(role => !role.Receivers.SequenceEqual(first.Receivers)))
            return body;

        int[] candidates = first.Receivers.SelectMany(uid => _possibleDefinitions.GetValueOrDefault(uid) ?? [])
            .Distinct().Where(slot => _symbols.Participants.TryGetValue(slot, out var participant) && participant.Kind == 2 &&
                first.Receivers.All(uid => _possibleDefinitions.GetValueOrDefault(uid)?.Contains(slot) == true) &&
                roles.All(role => role.Receivers.All(uid => IsCastHandler(uid, slot, role.Tag)))).ToArray();
        if (candidates is not [var definition])
            return body;

        var actor = _symbols.Participants[definition];
        var roleNames = scene.FragmentKeys.GetValueOrDefault((key, phase)) is { } fragment ? _catalog?.RoleNames(_eventName, key, phase, fragment) : null;
        string jobs = $"RoleAssignments_{key.Slot:D2}_{key.Tag:D2}_{unchecked((uint)phase):X}";
        var names = roles.Select((role, index) => (role.Storage, Name: roleNames?.GetValueOrDefault(role.Storage) ?? $"Role{index + 1}"))
            .ToDictionary(item => item.Storage, item => item.Name);
        if (names.Values.Any(name => !SyntaxFacts.IsValidIdentifier(name) || name == jobs) || names.Values.Distinct().Count() != names.Count)
        {
            names = roles.Select((role, index) => (role.Storage, Name: $"Role{index + 1}")).ToDictionary(item => item.Storage, item => item.Name);
        }
        scene.Bindings.Add(SyntaxFactory.ParseMemberDeclaration($"private {jobs} Jobs {{ get; }} = new(ctx.Globals);")!);
        var declaration = new StringBuilder($"private sealed class {jobs}(GlobalState globals)").AppendLine().AppendLine("{");
        foreach (RoleSwitch role in roles)
        {
            declaration.AppendLine("[RoleStorageAlias]");
            declaration.AppendLine($"internal int {names[role.Storage]}");
            declaration.AppendLine("{");
            declaration.AppendLine($"    get => globals.{role.Storage};");
            declaration.AppendLine($"    set => globals.{role.Storage} = value;");
            declaration.AppendLine("}");
        }
        scene.Bindings.Add(SyntaxFactory.ParseMemberDeclaration(declaration.Append('}').ToString())!);
        var aliases = new RoleAliases(names);
        for (int index = 0; index < scene.Methods.Count; index++)
        {
            MethodDeclarationSyntax existing = scene.Methods[index];
            scene.Methods[index] = existing.WithBody(RewriteAliases(existing.Body!));
        }

        var replacements = roles.ToDictionary(role => role.Switch, role =>
        {
            string entry = actor.Handlers[Array.FindIndex(actor.Scripts, script => script.Id == role.Tag)];
            return SyntaxFactory.ParseStatement($"await scripts.StartWhenReady(cast.Resolve(jobs.{names[role.Storage]}), evt.{actor.Name}.{entry}, level: {role.Level});")
                .WithTriviaFrom(role.Switch);
        });
        string bindings = string.Join(", ", first.Receivers.Select(uid => $"new InstanceUid({uid})"));
        StatementSyntax cast = SyntaxFactory.ParseStatement($"var cast = new SceneRoleCast<{actor.Name}>(evt.{actor.Name}, {bindings});");
        var rewritten = new List<StatementSyntax>();
        foreach (StatementSyntax statement in body.Statements)
        {
            if (statement == first.Switch)
                rewritten.Add(cast);
            if (statement is SwitchStatementSyntax selection && replacements.TryGetValue(selection, out var replacement))
            {
                rewritten.Add(replacement);
            }
            else if (statement is ExpressionStatementSyntax { Expression: AwaitExpressionSyntax { Expression: InvocationExpressionSyntax call } } &&
                call.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Scripts" } } &&
                ReadRequest(call, out int uid, out int level, out int tag) && first.Receivers.Contains(uid) && IsCastHandler(uid, definition, tag))
            {
                string handlerName = actor.Handlers[Array.FindIndex(actor.Scripts, script => script.Id == tag)];
                rewritten.Add(SyntaxFactory.ParseStatement($"await scripts.RequestWhenReady(evt.{actor.Name}.{handlerName}, level: {level}, instance: new InstanceUid({uid}));")
                    .WithTriviaFrom(statement));
            }
            else
            {
                rewritten.Add(statement);
            }
        }
        return RewriteAliases(body.WithStatements(SyntaxFactory.List(rewritten)), force: true);

        BlockSyntax RewriteAliases(BlockSyntax source, bool force = false)
        {
            var result = (BlockSyntax)aliases.Visit(source)!;
            return force || result != source ? result.WithStatements(result.Statements.Insert(0, SyntaxFactory.ParseStatement("var jobs = Jobs;"))) : result;
        }
    }

    private bool IsCastHandler(int uid, int slot, int tag) =>
        _possibleDefinitions.GetValueOrDefault(uid) is { } definitions && definitions.Contains(slot) &&
        _symbols.Participants[slot].Scripts.Count(script => script.Id == tag) == 1 &&
        definitions.All(other => other == slot || !_symbols.Participants.TryGetValue(other, out var competing) ||
            competing.Scripts.All(script => script.Id != tag));

    private RoleSwitch? ReadRoleSwitch(SwitchStatementSyntax selection, MethodDeclarationSyntax handler)
    {
        if (selection.Expression is not MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Globals" } } storage)
            return null;
        string name = storage.Name.Identifier.ValueText;
        if (typeof(GlobalState).GetProperty(name)?.GetCustomAttribute<StorageViewAttribute>() is not { Scope: StorageScope.Global } view ||
            selection.Sections.Count is < 2 or > byte.MaxValue)
            return null;
        EVExpressionToken selector = new EVVariable((EVStorageScope)view.Scope, (EVStorageType)view.Type, view.Address).Encode(view.Wide, view.AlternatePrefix);
        string key = SwitchLayout.Key([selector], Enumerable.Range(0, selection.Sections.Count).Select(value =>
            SwitchLayout.Section([value], fallback: false, returns: false)), directDispatch: false);
        string order = string.Join(",", Enumerable.Range(0, selection.Sections.Count));
        AttributeSyntax? layout = handler.AttributeLists.SelectMany(list => list.Attributes).FirstOrDefault(attribute =>
            attribute.Name.ToString() == "SwitchLayout" && attribute.ArgumentList?.Arguments is
                [{ Expression: LiteralExpressionSyntax selectorKey }, _, _, _] && selectorKey.Token.ValueText == key);
        if (layout != null && (layout.ArgumentList!.Arguments is not [_, { Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Dense" } },
                { Expression: LiteralExpressionSyntax caseOrder }, { Expression.RawKind: (int)SyntaxKind.FalseLiteralExpression }] ||
            caseOrder.Token.ValueText != order))
            return null;

        int[] receivers = new int[selection.Sections.Count];
        int level = -1;
        int tag = -1;
        for (int index = 0; index < receivers.Length; index++)
        {
            SwitchSectionSyntax section = selection.Sections[index];
            if (section.Labels is not [CaseSwitchLabelSyntax { Value: LiteralExpressionSyntax label }] || !Equals(label.Token.Value, index) ||
                section.Statements is not [ExpressionStatementSyntax { Expression: AwaitExpressionSyntax { Expression: InvocationExpressionSyntax call } }, BreakStatementSyntax] ||
                !ReadRequest(call, out int uid, out int priority, out int entry) || (index > 0 && (level != priority || tag != entry)))
                return null;
            receivers[index] = uid;
            level = priority;
            tag = entry;
        }
        return new RoleSwitch(selection, name, view, receivers, level, tag);
    }

    private bool ReadRequest(InvocationExpressionSyntax call, out int uid, out int level, out int tag)
    {
        uid = level = tag = -1;
        if (call.Expression is not MemberAccessExpressionSyntax { Name.Identifier.ValueText: "RequestWhenReady", Expression: IdentifierNameSyntax service } ||
            call.ArgumentList.Arguments is not [var first, var second, var third])
            return false;
        if (service.Identifier.ValueText == "Scripts")
            return ReadNumber(first, "level", out level) && ReadNumber(second, "targetUid", out uid) && ReadNumber(third, "scriptId", out tag) && uid is > 0 and < 250;
        if (service.Identifier.ValueText != "scripts" || first.NameColon != null || !ReadNumber(second, "level", out level) ||
            third.NameColon?.Name.Identifier.ValueText != "instance" ||
            third.Expression is not ObjectCreationExpressionSyntax { Type: IdentifierNameSyntax { Identifier.ValueText: "InstanceUid" }, ArgumentList.Arguments: [var id] } ||
            !ReadNumber(id, null, out uid) || uid is not (> 0 and < 250) ||
            first.Expression is not MemberAccessExpressionSyntax { Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "evt" }, Name: { } actor }, Name: { } action })
            return false;
        EventSceneSymbols.Participant? participant = _symbols.Participants.Values.SingleOrDefault(candidate => candidate.Name == actor.Identifier.ValueText);
        int position = participant?.Handlers.ToList().IndexOf(action.Identifier.ValueText) ?? -1;
        if (position < 0)
            return false;
        tag = participant!.Scripts[position].Id;
        return tag <= byte.MaxValue;

        static bool ReadNumber(ArgumentSyntax argument, string? name, out int value)
        {
            value = -1;
            if (argument.NameColon?.Name.Identifier.ValueText != name || argument.Expression is not LiteralExpressionSyntax { Token.Value: int number } || number is < 0 or > 255)
                return false;
            value = number;
            return true;
        }
    }

    private sealed class RoleAliases(Dictionary<string, string> names) : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node) =>
            node.Expression is IdentifierNameSyntax { Identifier.ValueText: "Globals" } && names.TryGetValue(node.Name.Identifier.ValueText, out string? name)
                ? SyntaxFactory.ParseExpression($"jobs.{name}").WithTriviaFrom(node)
                : base.VisitMemberAccessExpression(node);
    }
}
