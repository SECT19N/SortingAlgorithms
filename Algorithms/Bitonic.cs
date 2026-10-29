using Sort.Models;

namespace Sort.Algorithms;

public class Bitonic : Sorting {
    public override string Name => "Bitonic";
    public override string Description => "Sorting network that builds bitonic sequences and merges them with fixed compare-exchange patterns. Ideal for GPUs and parallel hardware.";
    public override string Complexity => "Best O(n log² n) · Avg O(n log² n) · Worst O(n log² n) · Space O(log n) · Unstable";

    public override void Sort(SortContext ctx) {
        BitonicSort(ctx, 0, ctx.Length, true);
    }

    private void BitonicSort(SortContext ctx, int lo, int n, bool ascending) {
        if (n > 1) {
            int m = n / 2;
            BitonicSort(ctx, lo, m, !ascending);
            BitonicSort(ctx, lo + m, n - m, ascending);
            BitonicMerge(ctx, lo, n, ascending);
        }
    }

    // Works for any n by splitting at the greatest power of two below n.
    private void BitonicMerge(SortContext ctx, int lo, int n, bool ascending) {
        if (n > 1) {
            int m = 1;
            while (m < n) {
                m <<= 1;
            }
            m >>= 1;

            for (int i = lo; i < lo + n - m; i++) {
                if ((ctx.Compare(i, i + m) > 0) == ascending) {
                    ctx.Swap(i, i + m);
                }
            }
            BitonicMerge(ctx, lo, m, ascending);
            BitonicMerge(ctx, lo + m, n - m, ascending);
        }
    }
}
