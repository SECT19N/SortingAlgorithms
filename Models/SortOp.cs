using Sort.Enums;

namespace Sort.Models;

public readonly struct SortOp {
    public OpKind Kind { get; }
    public int I { get; }
    public int J { get; }
    public int Value { get; }

    public SortOp(OpKind kind, int i, int j, int value) {
        Kind = kind;
        I = i;
        J = j;
        Value = value;
    }
}
