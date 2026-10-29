using Sort.Models;

namespace Sort.Algorithms;

public class Quick : Sorting {
    public override string Name => "Quick";
    public override string Description => "Partitions around a pivot (the last item, Lomuto scheme) then recurses on both sides. Degrades to O(n²) on sorted input.";
    public override string Complexity => "Best O(n log n) · Avg O(n log n) · Worst O(n²) · Space O(log n) · Unstable";
    public override void Sort(SortContext ctx) {
        QuickSort(ctx, 0, ctx.Length - 1);
    }
    private void QuickSort(SortContext ctx, int left, int right) {
        // Recurse into the smaller side and loop on the larger one, so the stack stays O(log n)
        // even on sorted input at a million items.
        while (left < right) {
            int pivot = Partition(ctx, left, right);
            if (pivot - left < right - pivot) {
                QuickSort(ctx, left, pivot - 1);
                left = pivot + 1;
            } else {
                QuickSort(ctx, pivot + 1, right);
                right = pivot - 1;
            }
        }
    }
    private int Partition(SortContext ctx, int left, int right) {
        int i = left - 1;
        for (int j = left; j <= right - 1; j++) {
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
