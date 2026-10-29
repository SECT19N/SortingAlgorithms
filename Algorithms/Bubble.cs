namespace Sort;

public class Bubble : Sorting {
    public override string Name => "Bubble";

    public override void Sort(SortContext ctx) {
        for (int i = 0; i <= ctx.Length - 2; i++) {
            for (int j = 0; j <= ctx.Length - 2; j++) {
                if (ctx.Compare(j, j + 1) > 0) {
                    ctx.Swap(j, j + 1);
                }
            }
        }
    }
}
