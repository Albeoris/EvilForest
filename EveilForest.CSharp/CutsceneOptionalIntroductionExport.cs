using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private enum InvitationChapter { Stage, Accept, Decline }

    internal readonly record struct InvitationCondition(EVExpressionToken[] Tokens, bool Inverted);
    internal sealed record OptionalIntroduction(InvitationCondition Gate, InvitationCondition Choice, int PollLabel);

    private void AddOptionalIntroductions(IEnumerable<Handler> handlers, List<Scene> scenes)
    {
        Handler[] entries = handlers.ToArray();
        foreach (Handler handler in entries.Where(candidate => candidate.Owner.Type == 0 && candidate.Entry.Tag == 1)
            .GroupBy(candidate => candidate.Entry).Where(group => group.Count() == 1).Select(group => group.Single()))
        {
            if (scenes.Any(scene => scene.For(handler.Entry) != null) ||
                !handler.Body.Instructions.Any(IsDialogChoice))
                continue;

            var flow = new EpisodeFlow(handler.Body);
            var reachable = flow.Reachable(0, 0, flow.Code.Count);
            Scene[] candidates = Enumerable.Range(0, flow.Code.Count - 1).Where(index => reachable.Contains(index) && IsDialogChoice(flow.Code[index]))
                .Select(index => ReadInvitation(handler, flow, index, entries)).OfType<Scene>().Take(2).ToArray();
            if (candidates is [var scene])
                scenes.Add(scene);
        }
    }

    private Scene? ReadInvitation(Handler handler, EpisodeFlow flow, int choice, IReadOnlyList<Handler> handlers)
    {
        IList<EVInstruction> code = flow.Code;
        int decline = flow.ForwardBranch(choice);
        if (decline < 0 || choice == 0 || !flow.Incoming[choice + 1].SequenceEqual([choice]) ||
            !TryFindActivity(flow, choice + 2, decline, out var activity))
            return null;

        int accept = choice + 2;
        var (launch, poll, join, mode, state) = activity;
        if (!flow.Closed(accept, launch, choice + 1) || !flow.Closed(decline, join, choice + 1))
            return null;

        if (ReadModeExits(flow, accept, launch, mode, initial: null) is not [true] ||
            code.Skip(decline).Take(join - decline).Any(step => MayWriteMode(step, mode)) ||
            handlers.Where(other => other != handler).Any(other => other.Body.Instructions.Any(step => MayWriteMode(step, mode))))
            return null;

        // Admission and the active dispatcher can use different state. Only the affirmative arm enters the dispatcher.
        int[] gates = Enumerable.Range(0, choice - 1).Where(index => flow.ForwardBranch(index) >= join &&
            IsStateGate(code[index]) && flow.Closed(index + 2, choice, index + 1) &&
            flow.Incoming[index + 1].SequenceEqual([index]) && IsStagedInvitation(flow, index + 2, choice)).Take(2).ToArray();
        if (gates is not [int gate])
            return null;

        var scene = new Scene($"{Fallback(handler.Entry)}_OptionalIntroduction", handler.Entry, false, new()
        {
            [(int)InvitationChapter.Stage] = "StageInvitation",
            [(int)InvitationChapter.Accept] = "AcceptInvitation",
            [(int)InvitationChapter.Decline] = "DeclineInvitation"
        })
        {
            Rule = "optional minigame introduction",
            Introduction = new(Condition(gate), Condition(choice), code[poll].Label)
        };
        AddFragment(InvitationChapter.Stage, gate + 2, choice);
        AddFragment(InvitationChapter.Accept, accept, launch);
        AddFragment(InvitationChapter.Decline, decline, join);
        return scene;

        InvitationCondition Condition(int index) => new(code[index].Expression, code[index + 1].Opcode == Jsm.Opcode.JMP_IF);

        void AddFragment(InvitationChapter chapter, int start, int end)
        {
            EVInstruction[] steps = code.Skip(start).Take(end - start).ToArray();
            var fragment = new CutscenePhaseGraph.Phase(handler.Entry, handler.Owner.Type, state,
                CutscenePhaseGraph.Cell.Of(EVVariable.Decode(state), handler.Entry.Slot), (int)chapter, code[start].Label, steps);
            scene.FragmentKeys.Add((handler.Entry, (int)chapter), fragment.Identity());
        }
    }

    private static bool IsDialogChoice(EVInstruction step) => step.Opcode == Jsm.Opcode.EXPR &&
        step.Expression is [var left, var right, { Opcode: (byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence }] &&
        (IsChoiceRegister(left) && TryReadLiteral(right, out _) || IsChoiceRegister(right) && TryReadLiteral(left, out _));

    private static bool IsChoiceRegister(EVExpressionToken token) => token.Opcode == (byte)Jsm.Excode.B_SYSVAR &&
        token.Value == (int)EvilForest.EventSource.SystemVariable.DialogChoice;

    private static bool IsStateGate(EVInstruction step) => step.Opcode == Jsm.Opcode.EXPR && (step.Expression switch
    {
        [var flag] => IsState(flag),
        [var left, var right, { Opcode: (byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence }] =>
            IsState(left) && TryReadLiteral(right, out _) || IsState(right) && TryReadLiteral(left, out _),
        _ => false
    });

    private static bool IsState(EVExpressionToken token) => StorageDeclarations.IsStorage(token) && EVVariable.Decode(token).Scope != EVStorageScope.Instance;

    private static bool OpensMessage(EVInstruction step) => step.Opcode is
        Jsm.Opcode.MES or Jsm.Opcode.MESN or Jsm.Opcode.MESA or Jsm.Opcode.MESAN or Jsm.Opcode.MESB;

    private static bool WaitsForSpeaker(EVInstruction speech, EVInstruction wait) => speech.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN &&
        wait.Opcode == Jsm.Opcode.WAITMES && speech.Operands[1].Expression == null &&
        wait.Operands[0] is { Expression: null } window && speech.Operands[1].Value == window.Value;

    private bool IsStagedInvitation(EpisodeFlow flow, int start, int choice)
    {
        int[] messages = Enumerable.Range(start, choice - start).Where(index => OpensMessage(flow.Code[index])).ToArray();
        if (messages.Length < 2 || !HasStagedSpeech(messages[^1], choice, out int promptActor))
            return false;

        for (int index = 0; index + 1 < messages.Length; index++)
        {
            if (HasStagedSpeech(messages[index], messages[index + 1], out int otherActor) && otherActor != promptActor)
                return true;
        }
        return false;

        bool HasStagedSpeech(int line, int next, out int uid)
        {
            uid = default;
            if (flow.Code[line] is not { Opcode: Jsm.Opcode.MESA or Jsm.Opcode.MESAN, Operands: [{ Expression: null } speaker, ..] } speech ||
                ResolveInvitationActor(speaker.Value) is not { } actor || !flow.Requires(start, choice, line))
                return false;

            uid = speaker.Value;
            return Enumerable.Range(line + 1, next - line - 1).Any(index => WaitsForSpeaker(speech, flow.Code[index]) && flow.Requires(start, choice, index)) &&
                Enumerable.Range(start, line - start).Any(index => flow.Code[index] is
                    { Opcode: Jsm.Opcode.REQSW or Jsm.Opcode.REQEW, Operands: [_, { Expression: null } receiver, { Expression: null } tag] } &&
                    receiver.Value == speaker.Value && actor.Scripts.Count(script => script.Id == tag.Value) == 1 && flow.Requires(start, line, index));
        }
    }

    private EventSceneSymbols.Participant? ResolveInvitationActor(int uid) =>
        _possibleDefinitions.TryGetValue(uid, out HashSet<int>? definitions) && definitions.Count == 1 &&
        _symbols.Participants.GetValueOrDefault(definitions.Single()) is { Kind: 2 } actor ? actor : null;

    private readonly record struct Activity(int Launch, int Poll, int Join, CutscenePhaseGraph.Cell Mode, EVExpressionToken State);

    private static bool TryFindActivity(EpisodeFlow flow, int accept, int decline, out Activity activity)
    {
        activity = default;
        IList<EVInstruction> code = flow.Code;
        // The accepting arm reaches a tail branch. Its true edge returns to the active dispatcher,
        // and its false edge reaches the same owner continuation as refusal.
        int exit = decline - 1;
        if (flow.JumpTarget(exit) is not int join || join <= decline || join >= code.Count ||
            flow.Incoming[exit].ToArray() is not [int branch] || code[branch] is not { Opcode: Jsm.Opcode.JMP_IF, Targets: [int dispatchLabel] } ||
            flow.Incoming[branch].ToArray() is not [int poll] || code[poll] is not { Opcode: Jsm.Opcode.EXPR, Expression: [var active] } ||
            !StorageDeclarations.IsStorage(active) || EVVariable.Decode(active) is not { Scope: EVStorageScope.Map, Type: EVStorageType.Bit } activeBit)
            return false;

        int dispatch = flow.Position(dispatchLabel);
        if (dispatch <= accept || dispatch >= poll || code[dispatch] is not { Opcode: Jsm.Opcode.EXPR, Expression: [var state] } || !IsState(state) ||
            flow.Successors[dispatch] is not [int selection] || flow.Incoming[selection].ToArray() is not [var selectionSource] || selectionSource != dispatch ||
            code[selection] is not { Opcode: Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d, Targets: [int idleLabel, ..] } table)
            return false;

        int[] entries = flow.Incoming[poll].Where(index => index < dispatch).Take(2).ToArray();
        if (entries is not [int launch] || launch <= accept || flow.JumpTarget(launch) != poll)
            return false;

        var mode = CutscenePhaseGraph.Cell.Of(activeBit, -1);
        int idle = flow.Position(idleLabel);
        if (idle <= selection || idle >= poll || ReadModeExits(flow, idle, poll, mode, initial: true) is not [true] ||
            !flow.Reachable(idle, idle, poll).Any(index => code[index] is { Opcode: Jsm.Opcode.WAIT, Operands: [{ Expression: null, Value: > 0 }] } &&
                flow.Requires(idle, poll, index)))
            return false;

        // A live session can keep waiting with the mode set, and a dispatched action can end that session.
        if (!table.Targets.Skip(1).Distinct().Select(flow.Position).Any(arm => arm > selection && arm < poll && arm != idle &&
            ReadModeExits(flow, arm, poll, mode, initial: true).Contains(false)))
            return false;

        activity = new(launch, poll, join, mode, state);
        return true;
    }

    private static bool?[] ReadModeExits(EpisodeFlow flow, int start, int end, CutscenePhaseGraph.Cell mode, bool? initial)
    {
        var pending = new Stack<(int Index, bool? Active)>();
        var visited = new HashSet<(int Index, bool? Active)>();
        var exits = new HashSet<bool?>();
        pending.Push((start, initial));
        while (pending.TryPop(out var path))
        {
            if (path.Index == end)
            {
                exits.Add(path.Active);
                continue;
            }
            if (path.Index < start || path.Index >= end || flow.Successors[path.Index].Length == 0)
                return [];
            if (!visited.Add(path))
                continue;

            EVInstruction step = flow.Code[path.Index];
            bool? active = path.Active;
            if (MayWriteMode(step, mode))
            {
                if (step.Expression is not [var cell, var literal, { Opcode: (byte)Jsm.Excode.Let }] || !StorageDeclarations.IsStorage(cell) ||
                    CutscenePhaseGraph.Cell.Of(EVVariable.Decode(cell), -1) != mode || !TryReadLiteral(literal, out int value) || value is not (0 or 1))
                    return [];
                active = Convert.ToBoolean(value);
            }

            foreach (int next in flow.Successors[path.Index])
                pending.Push((next, active));
        }
        return exits.ToArray();
    }

    private static bool MayWriteMode(EVInstruction step, CutscenePhaseGraph.Cell mode) => CutsceneMemoryIndex.Accesses(step).Any(access =>
        (access.Write || !access.Read) && CutscenePhaseGraph.Cell.Of(access.Variable, -1).Overlaps(mode));

    private static MethodDeclarationSyntax RewriteOptionalIntroduction(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter,
        RewriteBody rewriter, StorageDeclarations storage, Func<int, string> jumpLabel)
    {
        OptionalIntroduction introduction = scene.Introduction!;
        ExpressionSyntax? gateCondition = Format(introduction.Gate);
        ExpressionSyntax? choiceCondition = Format(introduction.Choice);
        if (gateCondition == null || choiceCondition == null)
            return method;
        var body = method.Body!;
        IfStatementSyntax[] gates = body.Statements.OfType<IfStatementSyntax>().Where(statement => SyntaxFactory.AreEquivalent(statement.Condition, gateCondition)).Take(2).ToArray();
        if (gates is not [{ Statement: BlockSyntax guard } gate])
            return method;
        IfStatementSyntax[] choices = guard.Statements.OfType<IfStatementSyntax>().Where(statement => SyntaxFactory.AreEquivalent(statement.Condition, choiceCondition)).Take(2).ToArray();
        if (choices is not [{ Statement: BlockSyntax accepted, Else.Statement: BlockSyntax declined } choice])
            return method;

        string poll = jumpLabel(introduction.PollLabel);
        string pollIdentity = introduction.PollLabel.ToString(System.Globalization.CultureInfo.InvariantCulture);
        StatementSyntax[] launches = accepted.Statements.Where(statement =>
            statement is GotoStatementSyntax { Expression: IdentifierNameSyntax target } && target.Identifier.ValueText == poll ||
            statement is ForStatementSyntax && statement.GetAnnotations(DecodedConditionAnnotation).Any(annotation => annotation.Data == pollIdentity)).Take(2).ToArray();
        if (launches is not [var launch])
            return method;

        var fragments = new[]
        {
            (Chapter: InvitationChapter.Stage, Block: guard, Length: guard.Statements.IndexOf(choice)),
            (Chapter: InvitationChapter.Accept, Block: accepted, Length: accepted.Statements.IndexOf(launch)),
            (Chapter: InvitationChapter.Decline, Block: declined, Length: declined.Statements.Count)
        };
        if (fragments.Any(fragment => fragment.Length == 0 || !IsClosedFragment(method, fragment.Block.Statements.Take(fragment.Length), fragment.Block) ||
            fragment.Block.Statements.Take(fragment.Length).SelectMany(statement => statement.DescendantNodesAndSelf())
                .Any(node => node is LocalDeclarationStatementSyntax or DeclarationExpressionSyntax or SwitchStatementSyntax)))
            return method;

        string predicate = "AcceptsInvitation";
        while (scene.Name == predicate || scene.Chapters.Values.Contains(predicate))
            predicate += "Choice";
        scene.Methods.Add((MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"internal bool {predicate}() => {choice.Condition.WithoutTrivia()};")!);
        var locals = body.Statements.OfType<LocalDeclarationStatementSyntax>();
        foreach (var (chapter, block, length) in fragments)
        {
            var statements = SyntaxFactory.Block(locals.Cast<StatementSyntax>().Concat(block.Statements.Take(length)));
            scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(scene.Chapters[(int)chapter])).WithParameterList(parameter)
                .WithAttributeLists(default).WithModifiers(Modifiers("internal", method)).WithBody((BlockSyntax)rewriter.Visit(statements)!));
            scene.ExtractedChapters.Add((entry, (int)chapter));
        }

        var rewrittenChoice = choice.WithCondition(SyntaxFactory.ParseExpression($"ctx.{scene.Name}.{predicate}()"))
            .WithStatement(Replace(accepted, InvitationChapter.Accept, fragments[1].Length))
            .WithElse(choice.Else!.WithStatement(Replace(declined, InvitationChapter.Decline, fragments[2].Length)));
        var rewrittenGuard = Replace(guard, InvitationChapter.Stage, fragments[0].Length);
        rewrittenGuard = rewrittenGuard.WithStatements(rewrittenGuard.Statements.Replace(rewrittenGuard.Statements[1], rewrittenChoice));
        return method.ReplaceNode(gate, gate.WithStatement(rewrittenGuard));

        ExpressionSyntax? Format(InvitationCondition condition) => SourceExpression.TryFormatCondition(condition.Tokens, storage, out string? text)
            ? SyntaxFactory.ParseExpression(condition.Inverted ? $"!({text})" : text)
            : null;

        BlockSyntax Replace(BlockSyntax block, InvitationChapter chapter, int length) => block.WithStatements(SyntaxFactory.List(block.Statements.Skip(length)
            .Prepend(SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.{scene.Chapters[(int)chapter]}(this);"))));
    }
}
