using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    internal sealed record RegionEpisode(int HandlerOrdinal, int RequestLabel, Entry Target, bool Guarded);

    private void AddRegionEpisodes(Handler[] handlers, List<Scene> scenes)
    {
        var entries = handlers.ToLookup(handler => handler.Entry);
        // Regions can enter the episode through their range or interaction callbacks.
        foreach (Handler handler in handlers.Where(handler => handler.Owner.Type == (byte)ParticipantKind.Region && handler.Entry.Tag is 2 or 3))
        {
            if (entries[handler.Entry].Count() != 1 || scenes.Any(scene => scene.For(handler.Entry) != null) ||
                handler.Body.Instructions.Where(IsRequest).ToArray() is not
                    [{ Opcode: Jsm.Opcode.REQEW, Operands: [_, { Expression: null } receiver, { Expression: null } tag] } request] ||
                EpisodeActorSlot(receiver.Value) is not int slot || tag.Value <= LastEngineCallback)
                continue;

            var target = new Entry(slot, (ushort)tag.Value);
            if (entries[target].ToArray() is not [{ Owner.Type: (byte)ParticipantKind.Actor } action] || scenes.Any(scene => scene.For(target) != null) ||
                !TryReadRegionEnvelope(handler, request, out bool guarded, out string fragment) || !HasRegionChoreography(action.Body))
                continue;

            var scene = new Scene(Fallback(handler.Entry), handler.Entry, false, new() { [0] = "Play" })
            {
                Rule = "region-initiated choreography",
                Region = new(handler.Ordinal, request.Label, target, guarded)
            };
            scene.FragmentKeys.Add((handler.Entry, 0), fragment);
            scenes.Add(scene);
        }
    }

    private static bool TryReadRegionEnvelope(Handler handler, EVInstruction request, out bool guarded, out string fragment)
    {
        guarded = false;
        fragment = "";
        var flow = new EpisodeFlow(handler.Body);
        IList<EVInstruction> code = flow.Code;
        if (code.Count < 5 || code[0] is not { Opcode: Jsm.Opcode.EXPR, Expression: [{ Opcode: (byte)Jsm.Excode.B_SYSVAR, Value: (int)SystemVariable.IsMovementControlEnabled }] } ||
            code[1].Opcode != Jsm.Opcode.JMP_IF || code[2].Opcode != Jsm.Opcode.Return || flow.Successors[1].Length != 2 || !flow.Successors[1].Contains(3))
            return false;

        int end = code.Count - 1;
        while (end > 2 && code[end].Opcode == Jsm.Opcode.NOP)
            end--;
        if (code[end].Opcode != Jsm.Opcode.Return)
            return false;

        int start = 3;
        if (IsStateGate(code[start]) && flow.ForwardBranch(start) == end)
        {
            start += 2;
            guarded = true;
        }
        int cue = flow.Position(request.Label);
        if (cue <= start || cue + 1 >= end ||
            !flow.Closed(start, end, start - (guarded ? 1 : 2)) || Enumerable.Range(start, end - start).Any(index => flow.Successors[index].Any(next => next <= index)) ||
            !flow.Requires(start, end, cue))
            return false;

        EVInstruction[] setup = code.Skip(start).Take(cue - start).ToArray();
        EVInstruction[] cleanup = code.Skip(cue + 1).Take(end - cue - 1).ToArray();
        if (!ReadAvailability(setup, 0, out int close, out var gate) || !ReadAvailability(cleanup, 1, out int reopen, out var restoredGate) ||
            !gate.Equals(restoredGate) || !flow.Requires(start, cue, close) || !flow.Requires(cue, end, reopen) ||
            setup.Any(step => !IsRegionBitWrite(step, 0, out _) && !IsRegionControl(step, Jsm.Opcode.UCOFF, Jsm.Opcode.MENUOFF)) ||
            cleanup.Any(step => !IsRegionBitWrite(step, 1, out _) && !IsRegionControl(step, Jsm.Opcode.UCON, Jsm.Opcode.MENUON)))
            return false;

        int playerRestriction = (int)TriangleAttributeMask.RestrictPlayer;
        if (setup.Count(step => step.Opcode == Jsm.Opcode.UCOFF) != 1 || setup.Count(step => step.Opcode == Jsm.Opcode.MENUOFF) != 1 ||
            cleanup.Where(step => step.Opcode == Jsm.Opcode.UCON).ToArray() is not [var enableMove] || cleanup.Count(step => step.Opcode == Jsm.Opcode.MENUON) != 1 ||
            setup.Where(step => step.Opcode == Jsm.Opcode.BGIMASK).ToArray() is not [{ Operands: [{ Expression: null } restricted] } mask] ||
            cleanup.Where(step => step.Opcode == Jsm.Opcode.BGIMASK).ToArray() is not [{ Operands: [{ Expression: null } restored] } restoreMask] ||
            (restricted.Value & playerRestriction) != 0 || (restricted.Value | playerRestriction) != restored.Value ||
            !flow.Requires(start, cue, flow.Position(mask.Label)) || !flow.Requires(flow.Position(enableMove.Label), end, flow.Position(restoreMask.Label)))
            return false;

        EVInstruction[] instructions = code.Skip(start).Take(end - start).ToArray();
        var register = CutscenePhaseGraph.Cell.Of(EVVariable.Decode(gate), handler.Entry.Slot);
        fragment = new CutscenePhaseGraph.Phase(handler.Entry, handler.Owner.Type, gate, register, 0, instructions[0].Label, instructions).Identity();
        return true;

        bool ReadAvailability(EVInstruction[] steps, int value, out int position, out EVExpressionToken variable)
        {
            position = default;
            variable = default;
            if (steps.Where(step => IsRegionBitWrite(step, value, out _)).ToArray() is not [var write])
                return false;
            position = flow.Position(write.Label);
            return IsRegionBitWrite(write, value, out variable);
        }

        static bool IsRegionControl(EVInstruction step, Jsm.Opcode move, Jsm.Opcode menu) =>
            step.Opcode == move || step.Opcode == menu || step.Opcode == Jsm.Opcode.BGIMASK ||
            step.Opcode is Jsm.Opcode.JMP or Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN || IsStateGate(step) || IsRegionDelay(step);
    }

    private static bool IsRegionBitWrite(EVInstruction step, int value, out EVExpressionToken target)
    {
        target = default;
        if (step is not { Opcode: Jsm.Opcode.EXPR, Expression: [var cell, var literal, { Opcode: (byte)Jsm.Excode.Let }] } ||
            !StorageDeclarations.IsStorage(cell) || EVVariable.Decode(cell) is not { Scope: EVStorageScope.Map, Type: EVStorageType.Bit } ||
            !TryReadLiteral(literal, out int assigned) || assigned != value)
            return false;
        target = cell;
        return true;
    }

    private static bool HasRegionChoreography(EVInstructionBody body)
    {
        var flow = new EpisodeFlow(body);
        var reachable = flow.Reachable(0, 0, flow.Code.Count + 1).Order().ToArray();
        if (reachable.Length == 0 || reachable[^1] == flow.Code.Count ||
            reachable.Where(index => flow.Code[index].Opcode == Jsm.Opcode.Return).ToArray() is not [int end] ||
            reachable.Any(index => !IsChoreographyStep(flow.Code[index])))
            return false;

        int[] dialogue = reachable.Where(index => flow.Code[index].Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESN).ToArray();
        return dialogue.Length >= 2 && dialogue.All(index => flow.Requires(0, end, index)) &&
            HasAwaitedAnimation(flow, 0, dialogue[^1]) && HasAwaitedAnimation(flow, dialogue[^1] + 1, end);

        static bool IsChoreographyStep(EVInstruction step) => step.Opcode is
            Jsm.Opcode.MES or Jsm.Opcode.MESN or Jsm.Opcode.WAITMES or Jsm.Opcode.CLOSE or
            Jsm.Opcode.ANIM or Jsm.Opcode.AIDLE or Jsm.Opcode.AMODE or Jsm.Opcode.WAITANIM or Jsm.Opcode.ENDANIM or
            Jsm.Opcode.TURNTO or Jsm.Opcode.TSPEED or Jsm.Opcode.WAITTURN or Jsm.Opcode.CLRDIST or Jsm.Opcode.MOVE or
            Jsm.Opcode.JMP or Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN or Jsm.Opcode.Return || IsRegionDelay(step) ||
            step is { Opcode: Jsm.Opcode.EXPR, Expression: [_, _, { Opcode: (byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence }] };
    }

    private static bool IsRegionDelay(EVInstruction step) => step.Opcode == Jsm.Opcode.NOP ||
        step is { Opcode: Jsm.Opcode.WAIT, Operands: [{ Expression: null }] };

    private static bool HasAwaitedAnimation(EpisodeFlow flow, int start, int finish)
    {
        var pending = new Stack<(int Index, bool Playing, bool Awaited)>();
        var visited = new HashSet<(int Index, bool Playing, bool Awaited)>();
        bool arrived = false;
        pending.Push((start, false, false));
        while (pending.TryPop(out var path))
        {
            if (!visited.Add(path))
                continue;
            if (path.Index == finish)
            {
                if (!path.Awaited)
                    return false;
                arrived = true;
                continue;
            }
            if (path.Index < start || path.Index > finish)
                return false;

            // A wait only supplies evidence when this path still has a live performance that began in this span.
            Jsm.Opcode opcode = flow.Code[path.Index].Opcode;
            bool playing = opcode switch
            {
                Jsm.Opcode.ANIM => true,
                Jsm.Opcode.ENDANIM or Jsm.Opcode.AIDLE or Jsm.Opcode.AMODE or Jsm.Opcode.MOVE or Jsm.Opcode.TURNTO or Jsm.Opcode.WAITANIM => false,
                _ => path.Playing
            };
            bool awaited = path.Awaited || opcode == Jsm.Opcode.WAITANIM && path.Playing;
            foreach (int next in flow.Successors[path.Index])
                pending.Push((next, playing, awaited));
        }
        return arrived;
    }

    private MethodDeclarationSyntax RewriteRegionEpisode(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter, RewriteBody rewriter)
    {
        RegionEpisode region = scene.Region!;
        if (!_actionCandidates.TryGetValue(region.Target, out var incoming) || incoming.Count != 1 ||
            !_callSites.TryGetValue((entry.Slot, region.HandlerOrdinal, region.RequestLabel), out var site) || !incoming.Contains(site) ||
            AnnotatedCalls(method).Where(pair => pair.Site == site).ToArray() is not [var source] || !CanBind(source.Call, _actionCalls[site]) ||
            source.Call.Parent is not AwaitExpressionSyntax { Parent: ExpressionStatementSyntax { Parent: BlockSyntax block } })
            return method;

        BlockSyntax root = method.Body!;
        var locals = root.Statements.TakeWhile(statement => statement is LocalDeclarationStatementSyntax).ToArray();
        if (root.Statements.Count <= locals.Length + 2 || root.Statements[^1] is not ReturnStatementSyntax ||
            root.Statements[locals.Length] is not IfStatementSyntax { Else: null, Statement: BlockSyntax { Statements: [ReturnStatementSyntax] } })
            return method;

        int start = 0;
        int end = block.Statements.Count;
        if (region.Guarded)
        {
            if (root.Statements.Count != locals.Length + 3 || block.Parent is not IfStatementSyntax { Else: null } guard || root.Statements[locals.Length + 1] != guard)
                return method;
        }
        else
        {
            if (block != root)
                return method;
            start = locals.Length + 1;
            end--;
        }

        StatementSyntax[] fragment = block.Statements.Skip(start).Take(end - start).ToArray();
        if (fragment.Length < 3 || !IsClosedFragment(method, fragment, block) || fragment.SelectMany(statement => statement.DescendantNodesAndSelf())
            .Any(node => node is LocalDeclarationStatementSyntax or DeclarationExpressionSyntax))
            return method;

        string name = scene.Chapters[0];
        BlockSyntax body = SyntaxFactory.Block(locals.Concat(fragment));
        scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter).WithAttributeLists(default)
            .WithModifiers(Modifiers("internal", method)).WithBody((BlockSyntax)rewriter.Visit(body)!));
        scene.ExtractedChapters.Add((entry, 0));
        var call = SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.{name}(this);");
        return method.ReplaceNode(block, block.WithStatements(SyntaxFactory.List(block.Statements.Take(start).Append(call).Concat(block.Statements.Skip(end)))));
    }
}
