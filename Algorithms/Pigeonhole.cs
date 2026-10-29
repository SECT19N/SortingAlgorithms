using Sort.Models;

namespace Sort.Algorithms;

public class Pigeonhole : Sorting {
    public override string Name => "Pigeonhole";
    public override string Description => "Drops each item into the hole for its exact value, then collects the holes in order. Like counting sort; best when the range is close to the item count.";
    public override string Complexity => "Best O(n + k) · Avg O(n + k) · Worst O(n + k) · Space O(k)";

    public override void Sort(SortContext ctx) {
        int n = ctx.Length;
        if (n < 2) {
            return;
        }

        int min = ctx[0];
        int max = ctx[0];
        for (int i = 1; i < n; i++) {
            if (ctx[i] < min) {
                min = ctx[i];
            }
            if (ctx[i] > max) {
                max = ctx[i];
            }
        }

        int[] holes = new int[max - min + 1];
        for (int i = 0; i < n; i++) {
            holes[ctx[i] - min]++;
        }

        // Pigeonhole walks every hole in order and empties it back into the array.
        int k = 0;
        for (int h = 0; h < holes.Length; h++) {
            while (holes[h]-- > 0) {
                ctx.Write(k++, h + min);
            }
        }
    }
}
