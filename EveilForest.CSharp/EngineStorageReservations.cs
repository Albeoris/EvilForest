using EvilForest.Resources;

namespace EveilForest.CSharp;

internal static class EngineStorageReservations
{
    private static readonly (int FirstBit, int Count) ProgressionByte = (24 * 8, 8);

    public static IEnumerable<(int FirstBit, int Count)> ForEvent(string name)
    {
        DBEvent? evt = DB.Events.Values.FirstOrDefault(evt => evt.FileName.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (evt == null)
            yield break;

        // EBin.MoveToNextEntry reads the location's progression byte.
        yield return ProgressionByte;
        var specific = (int)evt.EntryId switch
        {
            64 => new[] { Byte(48, 2), Byte(52), Byte(34, 2) }, // EMinigame.SwordplayAssistance
            100 => new[] { Byte(31) }, // DialogManager.Close
            301 => new[] { (239, 1), (238, 1) }, // EventEngine.ProcessEvents
            302 => new[] { (207, 1), (206, 1) },
            303 or 304 => new[] { (207, 1), (206, 1), (205, 1) },
            656 or 657 or 658 or 659 => new[] { (613, 1) }, // EventCollision.IsNPCTalkable
            1421 => new[] { Byte(47), Byte(56, 2) }, // EMinigame.DigUpMadianRingCheating
            1607 => new[] { Byte(35) }, // EventEngine.DoEventCode, POS3
            1656 => new[] { Byte(28, 2) }, // FieldMap.SceneService3DScroll
            1900 => new[] { Byte(26) }, // EBin.CommandDefault
            2801 => new[] { Byte(46) },
            2921 => new[] { Byte(40, 2) }, // EventEngine.ProcessEvents
            2950 => new[] { Byte(32) }, // EventCollision.IsNPCTalkable
            _ => Array.Empty<(int FirstBit, int Count)>()
        };
        foreach (var reservation in specific)
            yield return reservation;
    }

    private static (int FirstBit, int Count) Byte(int address, int bytes = 1) => (address * 8, bytes * 8);
}
