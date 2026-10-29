using Sort.Models;

namespace Sort.Algorithms;

public class OddEven : Sorting {
    public override string Name => "Odd-Even";
    public override string Description => "Compares odd-indexed pairs, then even-indexed pairs, in alternating phases until nothing swaps. Designed for parallel hardware.";
    public override string Complexity => "Best O(n) · Avg O(n²) · Worst O(n²) · Space O(1) · Stable";

    public override void Sort(SortContext ctx) {
        bool sorted = false;
        while (!sorted) {
            sorted = true;
            for (int start = 1; start >= 0; start--) {
                for (int i = start; i + 1 < ctx.Length; i += 2) {
                    if (ctx.Compare(i, i + 1) > 0) {
                        ctx.Swap(i, i + 1);
                        sorted = false;
                    }
                }
            }
        }
    }
}
