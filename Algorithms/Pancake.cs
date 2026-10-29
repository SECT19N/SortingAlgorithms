namespace Sort;

public class Pancake : Sorting {
    public override string Name => "Pancake";

    public override void Sort(SortContext ctx) {
        for (int size = ctx.Length; size > 1; size--) {
            int max = 0;
            for (int i = 1; i < size; i++) {
                if (ctx.Compare(i, max) > 0) {
                    max = i;
                }
            }

            if (max != size - 1) {
                Flip(ctx, max);
                Flip(ctx, size - 1);
            }
        }
    }

    private static void Flip(SortContext ctx, int end) {
        for (int i = 0, j = end; i < j; i++, j--) {
            ctx.Swap(i, j);
        }
    }
}
