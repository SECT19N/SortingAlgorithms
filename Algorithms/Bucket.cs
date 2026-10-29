using System.Collections.Generic;

namespace Sort;

public class Bucket : Sorting {
    public override string Name => "Bucket";

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

        long span = (long)max - min + 1;
        List<int>[] buckets = new List<int>[n];
        for (int b = 0; b < n; b++) {
            buckets[b] = [];
        }
        for (int i = 0; i < n; i++) {
            buckets[(int)((ctx[i] - (long)min) * n / span)].Add(ctx[i]);
        }

        int k = 0;
        foreach (List<int> bucket in buckets) {
            // Insertion sort inside each bucket; buckets are small on average.
            for (int i = 1; i < bucket.Count; i++) {
                int v = bucket[i];
                int j = i - 1;
                while (j >= 0 && bucket[j] > v) {
                    bucket[j + 1] = bucket[j];
                    j--;
                }
                bucket[j + 1] = v;
            }
            foreach (int v in bucket) {
                ctx.Write(k++, v);
            }
        }
    }
}
