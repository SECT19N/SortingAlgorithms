using System;

namespace Sort;

// Simplified Timsort: insertion-sorted runs merged pairwise (no galloping, fixed run length).
public class Tim : Sorting {
    private const int Run = 32;

    public override string Name => "Tim";

    public override void Sort(SortContext ctx) {
        int n = ctx.Length;
        for (int lo = 0; lo < n; lo += Run) {
            int hi = Math.Min(lo + Run - 1, n - 1);
            for (int i = lo + 1; i <= hi; i++) {
                for (int j = i; j > lo && ctx.Compare(j - 1, j) > 0; j--) {
                    ctx.Swap(j - 1, j);
                }
            }
        }

        int[] buf = new int[n];
        for (int size = Run; size < n; size *= 2) {
            for (int left = 0; left < n - size; left += 2 * size) {
                int mid = left + size - 1;
                int right = Math.Min(left + 2 * size - 1, n - 1);
                Merge.MergeRuns(ctx, buf, left, mid, right);
            }
        }
    }
}
