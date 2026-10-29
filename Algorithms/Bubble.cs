using Sort.Models;

namespace Sort.Algorithms;

public class Bubble : Sorting {
    public override string Name => "Bubble";
    public override string Description => "Repeatedly steps through the list and swaps adjacent items that are out of order, so the largest values bubble to the end. This version has no early exit.";
    public override string Complexity => "Best O(n²) · Avg O(n²) · Worst O(n²) · Space O(1) · Stable";

    public override void Sort(SortContext ctx) {
        for (int i = 0; i <= ctx.Length - 2; i++) {
            for (int j = 0; j <= ctx.Length - 2; j++) {
                if (ctx.Compare(j, j + 1) > 0) {
                    ctx.Swap(j, j + 1);
                }
            }
        }
    }
}
