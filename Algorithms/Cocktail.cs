using Sort.Models;

namespace Sort.Algorithms;

public class Cocktail : Sorting {
    public override string Name => "Cocktail";
    public override string Description => "Bubble sort that alternates direction each pass, so small values also travel quickly to the front.";
    public override string Complexity => "Best O(n) · Avg O(n²) · Worst O(n²) · Space O(1) · Stable";

    public override void Sort(SortContext ctx) {
        int start = 0;
        int end = ctx.Length - 1;
        bool swapped = true;

        while (swapped && start < end) {
            swapped = false;
            for (int i = start; i < end; i++) {
                if (ctx.Compare(i, i + 1) > 0) {
                    ctx.Swap(i, i + 1);
                    swapped = true;
                }
            }
            end--;

            if (!swapped) {
                break;
            }

            swapped = false;
            for (int i = end; i > start; i--) {
                if (ctx.Compare(i - 1, i) > 0) {
                    ctx.Swap(i - 1, i);
                    swapped = true;
                }
            }
            start++;
        }
    }
}
