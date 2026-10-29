using Sort.Models;

namespace Sort.Algorithms;

public class Counting : Sorting {
    public override string Name => "Counting";
    public override string Description => "Counts how often each value occurs, then writes the values back in order. No comparisons; needs a limited value range k.";
    public override string Complexity => "Best O(n + k) · Avg O(n + k) · Worst O(n + k) · Space O(k)";

    public override void Sort(SortContext ctx) {
        if (ctx.Length < 2) {
            return;
        }

        int min = ctx[0];
        int max = ctx[0];
        for (int i = 1; i < ctx.Length; i++) {
            if (ctx[i] < min) {
                min = ctx[i];
            }
            if (ctx[i] > max) {
                max = ctx[i];
            }
        }

        int[] counts = new int[max - min + 1];
        for (int i = 0; i < ctx.Length; i++) {
            counts[ctx[i] - min]++;
        }

        int k = 0;
        for (int v = 0; v < counts.Length; v++) {
            for (int c = 0; c < counts[v]; c++) {
                ctx.Write(k++, v + min);
            }
        }
    }
}
