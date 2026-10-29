namespace Sort;

public class Shell : Sorting {
    public override string Name => "Shell";

    public override void Sort(SortContext ctx) {
        for (int gap = ctx.Length / 2; gap > 0; gap /= 2) {
            for (int i = gap; i < ctx.Length; i++) {
                for (int j = i; j >= gap && ctx.Compare(j - gap, j) > 0; j -= gap) {
                    ctx.Swap(j - gap, j);
                }
            }
        }
    }
}
