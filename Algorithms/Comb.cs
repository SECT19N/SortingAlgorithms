using Sort.Models;

namespace Sort.Algorithms;

public class Comb : Sorting {
    public override string Name => "Comb";
    public override string Description => "Bubble sort with a gap that shrinks by a factor of 1.3, so far-apart inversions (like small values near the end) are fixed early.";
    public override string Complexity => "Best O(n log n) · Avg ~O(n²) · Worst O(n²) · Space O(1) · Unstable";

    public override void Sort(SortContext ctx) {
        int gap = ctx.Length;
        bool swapped = true;

        while (gap > 1 || swapped) {
            gap = gap * 10 / 13;
            if (gap < 1) {
                gap = 1;
            }

            swapped = false;
            for (int i = 0; i + gap < ctx.Length; i++) {
                if (ctx.Compare(i, i + gap) > 0) {
                    ctx.Swap(i, i + gap);
                    swapped = true;
                }
            }
        }
    }
}
