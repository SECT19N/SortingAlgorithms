using Sort.Models;

namespace Sort.Algorithms;

public class Gnome : Sorting {
    public override string Name => "Gnome";
    public override string Description => "Walks forward while items are in order; on an inversion it swaps and steps back, like a gnome sorting flower pots.";
    public override string Complexity => "Best O(n) · Avg O(n²) · Worst O(n²) · Space O(1) · Stable";

    public override void Sort(SortContext ctx) {
        int i = 0;
        while (i < ctx.Length) {
            if (i == 0 || ctx.Compare(i - 1, i) <= 0) {
                i++;
            } else {
                ctx.Swap(i - 1, i);
                i--;
            }
        }
    }
}
