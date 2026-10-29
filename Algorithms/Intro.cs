using System;
using Sort.Models;

namespace Sort.Algorithms;

public class Intro : Sorting {
    private const int SmallRange = 16;

    public override string Name => "Intro";
    public override string Description => "Quick sort with median-of-three pivots that switches to heap sort when recursion gets too deep and to insertion sort on tiny ranges. The idea behind C++ std::sort.";
    public override string Complexity => "Best O(n log n) · Avg O(n log n) · Worst O(n log n) · Space O(log n) · Unstable";

    public override void Sort(SortContext ctx) {
        int depth = 2 * (int)Math.Log2(Math.Max(ctx.Length, 1));
        IntroSort(ctx, 0, ctx.Length - 1, depth);
    }

    private void IntroSort(SortContext ctx, int left, int right, int depth) {
        while (right - left + 1 > SmallRange) {
            if (depth-- == 0) {
                Heap.HeapSort(ctx, left, right - left + 1);
                return;
            }
            int p = Partition(ctx, left, right);
            IntroSort(ctx, p + 1, right, depth);
            right = p - 1;
        }

        for (int i = left + 1; i <= right; i++) {
            for (int j = i; j > left && ctx.Compare(j - 1, j) > 0; j--) {
                ctx.Swap(j - 1, j);
            }
        }
    }

    private int Partition(SortContext ctx, int left, int right) {
        // Median of three moved to the right end to act as the pivot.
        int mid = left + (right - left) / 2;
        if (ctx.Compare(left, mid) > 0) {
            ctx.Swap(left, mid);
        }
        if (ctx.Compare(left, right) > 0) {
            ctx.Swap(left, right);
        }
        if (ctx.Compare(mid, right) > 0) {
            ctx.Swap(mid, right);
        }
        if (mid != right) {
            ctx.Swap(mid, right);
        }

        int i = left - 1;
        for (int j = left; j < right; j++) {
            if (ctx.Compare(j, right) <= 0) {
                i++;
                if (i != j) {
                    ctx.Swap(i, j);
                }
            }
        }
        if (i + 1 != right) {
            ctx.Swap(i + 1, right);
        }
        return i + 1;
    }
}
