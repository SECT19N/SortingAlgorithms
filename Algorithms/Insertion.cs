namespace Sort;

public class Insertion : Sorting {
    public override string Name => "Insertion";

    public override void Sort(SortContext ctx) {
        for (int i = 1; i < ctx.Length; i++) {
            for (int j = i; j > 0 && ctx.Compare(j - 1, j) > 0; j--) {
                ctx.Swap(j - 1, j);
            }
        }
    }
}
