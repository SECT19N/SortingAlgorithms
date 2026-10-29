using Sort.Models;

namespace Sort.Algorithms;

public class Merge : Sorting {
    public override string Name => "Merge";
    public override string Description => "Splits the list in halves, sorts each half, then merges them through a temporary buffer. Predictable and stable.";
    public override string Complexity => "Best O(n log n) · Avg O(n log n) · Worst O(n log n) · Space O(n) · Stable";

    public override void Sort(SortContext ctx) {
        MergeSort(ctx, new int[ctx.Length], 0, ctx.Length - 1);
    }

    private void MergeSort(SortContext ctx, int[] buf, int left, int right) {
        if (left >= right) {
            return;
        }
        int mid = left + (right - left) / 2;
        MergeSort(ctx, buf, left, mid);
        MergeSort(ctx, buf, mid + 1, right);
        MergeRuns(ctx, buf, left, mid, right);
    }

    // Merges the sorted runs [left, mid] and [mid + 1, right]; shared with Tim.
    public static void MergeRuns(SortContext ctx, int[] buf, int left, int mid, int right) {
        for (int k = left; k <= right; k++) {
            buf[k] = ctx[k];
        }

        int i = left;
        int j = mid + 1;
        for (int k = left; k <= right; k++) {
            if (i > mid) {
                ctx.Write(k, buf[j++]);
            } else if (j > right) {
                ctx.Write(k, buf[i++]);
            } else if (ctx.CompareValues(buf[j], buf[i], j, i) < 0) {
                ctx.Write(k, buf[j++]);
            } else {
                ctx.Write(k, buf[i++]);
            }
        }
    }
}
