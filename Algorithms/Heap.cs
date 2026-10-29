using Sort.Models;

namespace Sort.Algorithms;

public class Heap : Sorting {
    public override string Name => "Heap";
    public override string Description => "Builds a max-heap in place, then repeatedly swaps the maximum to the end and restores the heap.";
    public override string Complexity => "Best O(n log n) · Avg O(n log n) · Worst O(n log n) · Space O(1) · Unstable";

    public override void Sort(SortContext ctx) {
        HeapSort(ctx, 0, ctx.Length);
    }

    // Sorts the range [start, start + count) in place; shared with Intro.
    public static void HeapSort(SortContext ctx, int start, int count) {
        for (int i = count / 2 - 1; i >= 0; i--) {
            SiftDown(ctx, start, i, count);
        }
        for (int end = count - 1; end > 0; end--) {
            ctx.Swap(start, start + end);
            SiftDown(ctx, start, 0, end);
        }
    }

    private static void SiftDown(SortContext ctx, int start, int root, int count) {
        while (true) {
            int child = 2 * root + 1;
            if (child >= count) {
                return;
            }
            if (child + 1 < count && ctx.Compare(start + child, start + child + 1) < 0) {
                child++;
            }
            if (ctx.Compare(start + root, start + child) >= 0) {
                return;
            }
            ctx.Swap(start + root, start + child);
            root = child;
        }
    }
}
