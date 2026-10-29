using Sort.Models;

namespace Sort.Algorithms;

public class Shell : Sorting {
    public override string Name => "Shell";
    public override string Description => "Insertion sort over shrinking gaps (n/2, n/4, ... 1), so items jump long distances early and the final pass has little to do.";
    public override string Complexity => "Best O(n log n) · Avg ~O(n^1.5) · Worst O(n²) · Space O(1) · Unstable";

    public override void Sort(SortContext ctx) {
        for (int gap = ctx.Length / 2; gap > 0; gap /= 2) {
            for (int i = gap; i < ctx.Length; i++) {
                for (int j = i; j >= gap && ctx.Compare(j - gap, j) > 0; j -= gap) {
                    ctx.Swap(j - gap, j);
                }
            }
        }
    }
}
