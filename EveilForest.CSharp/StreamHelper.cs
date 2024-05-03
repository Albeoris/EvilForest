using System.Runtime.InteropServices;

namespace EveilForest.CSharp;

internal static class StreamHelper
{
    public static void WriteStruct<T>(this Stream stream, in T value) where T : unmanaged
    {
        ReadOnlySpan<T> span = new(in value);
        ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(span);
        stream.Write(bytes);
    }

    public static void WriteStructs<T>(this Stream stream, T[] values) where T : unmanaged
    {
        ReadOnlySpan<T> span = new(values);
        ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(span);
        stream.Write(bytes);
    }
}