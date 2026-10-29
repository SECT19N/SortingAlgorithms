using Sort.Models;

namespace Sort.Algorithms;

public class Insertion : Sorting {
    public override string Name => "Insertion";
    public override string Description => "Grows a sorted prefix by sliding each new item left into place. Very fast on small or nearly sorted data.";
    public override string Complexity => "Best O(n) · Avg O(n²) · Worst O(n²) · Space O(1) · Stable";

    public override void Sort(SortContext ctx) {
        for (int i = 1; i < ctx.Length; i++) {
            for (int j = i; j > 0 && ctx.Compare(j - 1, j) > 0; j--) {
                ctx.Swap(j - 1, j);
            }
        }
    }
}
