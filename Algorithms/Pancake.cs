using Sort.Models;

namespace Sort.Algorithms;

public class Pancake : Sorting {
    public override string Name => "Pancake";
    public override string Description => "Sorts using only prefix reversals: flip the largest item to the front, then flip it into its final spot, and repeat on the shorter prefix.";
    public override string Complexity => "Best O(n²) · Avg O(n²) · Worst O(n²) · Space O(1) · Unstable";

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
