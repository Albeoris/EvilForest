namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private DirectorComponent IncludeParticipantOnlyPhases(Handler owner, Handler[] handlers, CutscenePhaseGraph.Phase[] arms, DirectorComponent machine)
    {
        var participants = machine.Arms.Select(arm => arm.Entry).ToHashSet();
        if (machine.Cues.Count != 1 || arms.Any(arm => !participants.Contains(arm.Entry) && arm.OwnerKind == 0 &&
            arm.Register == machine.EntryPhase.Register && arm.Successors.Any()))
            return machine;

        var examined = new HashSet<Entry> { owner.Entry };
        var selector = machine.EntryPhase.Selector;
        while (true)
        {
            var phases = new HashSet<int>(machine.Values);
            foreach (var handler in handlers.Where(item => item.Entry.Tag == 1 && participants.Contains(item.Entry) && examined.Add(item.Entry)))
            {
                if (!TryReadParticipantPollingPrefix(handler, selector, out int prefix) ||
                    !TryReadPollingArms(handler, selector, hasPulse: false, out var polling, participantLoop: true, prefix))
                    continue;

                // An external entrance can start the actor in a phase outside the director's
                // switch. Accept only decoded arms of this actor's proven polling switch.
                var proven = arms.Where(arm => arm.Entry == handler.Entry && arm.Selector.Equals(selector)).Select(arm => arm.Value);
                phases.UnionWith(proven.Where(polling.ContainsKey));
            }
            if (phases.SetEquals(machine.Values))
                return machine;
            machine = ExpandDirectorComponent(handlers, arms, machine.EntryPhase, machine.Speaker, phases, [.. machine.Cues]);
            participants.UnionWith(machine.Arms.Select(arm => arm.Entry));
        }
    }
}
