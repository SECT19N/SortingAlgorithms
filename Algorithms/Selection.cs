using Sort.Models;

namespace Sort.Algorithms;

public class Selection : Sorting {
    public override string Name => "Selection";
    public override string Description => "Finds the smallest remaining item and swaps it to the front on every pass. Few swaps, but always O(n²) comparisons.";
    public override string Complexity => "Best O(n²) · Avg O(n²) · Worst O(n²) · Space O(1) · Unstable";
    public override void Sort(SortContext ctx) {
        for (int i = 0; i < ctx.Length - 1; i++) {
            int minIndex = i;

            for (int j = i + 1; j < ctx.Length; j++) {
                if (ctx.Compare(j, minIndex) < 0) {
                    minIndex = j;
                }
            }

            if (minIndex != i) {
                ctx.Swap(i, minIndex);
            }
        }
    }
}
