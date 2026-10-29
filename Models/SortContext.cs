using System;
using Sort.Enums;

namespace Sort.Models;

// Wraps the working array and streams every operation to a sink in fixed-size chunks.
public class SortContext(int[] arr, Action<SortOp[], int> sink) {
    public const int ChunkSize = 1 << 16;

    private readonly int[] data = arr;
    private SortOp[] chunk = new SortOp[ChunkSize];
    private int count;

    public int Length => data.Length;

    public int this[int i] => data[i];

    public int Compare(int i, int j) {
        Log(OpKind.Compare, i, j, 0);
        return data[i].CompareTo(data[j]);
    }

    public int CompareValues(int a, int b, int i, int j) {
        Log(OpKind.Compare, i, j, 0);
        return a.CompareTo(b);
    }

    public void Swap(int i, int j) {
        Log(OpKind.Swap, i, j, 0);
        (data[i], data[j]) = (data[j], data[i]);
    }

    public void Write(int i, int value) {
        Log(OpKind.Write, i, -1, value);
        data[i] = value;
    }

    // Hands the partly filled chunk to the sink; call once after the algorithm returns.
    public void Flush() {
        if (count > 0) {
            sink(chunk, count);
            chunk = new SortOp[ChunkSize];
            count = 0;
        }
    }

    private void Log(OpKind kind, int i, int j, int value) {
        if (count == chunk.Length) {
            Flush();
        }
        chunk[count++] = new SortOp(kind, i, j, value);
    }
}
