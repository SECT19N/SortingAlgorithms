using System.Collections.Generic;

namespace Sort;

public class SortContext(int[] arr) {
    private readonly int[] data = arr;
    public List<SortOp> Ops { get; } = [];
    public int Length => data.Length;

    public int this[int i] => data[i];

    public int Compare(int i, int j) {
        Ops.Add(new SortOp(OpKind.Compare, i, j, 0));
        return data[i].CompareTo(data[j]);
    }

    public void Swap(int i, int j) {
        Ops.Add(new SortOp(OpKind.Swap, i, j, 0));
        (data[i], data[j]) = (data[j], data[i]);
    }

    public void Write(int i, int value) {
        Ops.Add(new SortOp(OpKind.Write, i, -1, value));
        data[i] = value;
    }
}
