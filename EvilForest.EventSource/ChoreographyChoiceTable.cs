namespace EvilForest.EventSource;

internal static class ChoreographyChoiceTable
{
    internal const string Diagnostic = "Choice keys must fit the selected map cell and a supported event switch table.";
    // The wide table's header and two-byte entries must fit before its first relative target.
    internal const int MaxWideSpan = (ushort.MaxValue - 6) / 2;

    internal static bool CanStore(StorageType type, int value) => type switch
    {
        StorageType.Byte => value is >= byte.MinValue and <= byte.MaxValue,
        StorageType.SByte => value is >= sbyte.MinValue and <= sbyte.MaxValue,
        StorageType.UInt16 => value is >= ushort.MinValue and <= ushort.MaxValue,
        StorageType.Int16 => value is >= short.MinValue and <= short.MaxValue,
        // The event engine sign-extends both encodings when loading a 24-bit value.
        StorageType.Int24 or StorageType.UInt24 => value >= -(1 << 23) && value < (1 << 23),
        _ => false
    };

    internal static SwitchTable? Infer(IReadOnlyCollection<int> keys, StorageType type)
    {
        if (keys.Count == 0)
            return null;
        int first = keys.Min();
        int last = keys.Max();
        if (!CanStore(type, first) || !CanStore(type, last))
            return null;

        int start = DenseStart(first);
        int span = last - start + 1;
        bool dense = start >= short.MinValue && span <= byte.MaxValue;
        if (dense && span == keys.Count)
            return SwitchTable.Dense;
        // Explicit switch tables compare the low 16 bits of the selector in the event engine.
        if (keys.Count <= byte.MaxValue && first >= 0 && type is not (StorageType.Int24 or StorageType.UInt24))
            return SwitchTable.Explicit;
        if (dense)
            return SwitchTable.Dense;

        return start >= short.MinValue && span <= MaxWideSpan ? SwitchTable.Wide : null;
    }

    internal static int DenseStart(int first) => Math.Min(first, short.MaxValue);
}
