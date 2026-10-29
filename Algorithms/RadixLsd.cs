namespace Sort;

public class RadixLsd : Sorting {
    public override string Name => "Radix LSD";

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

        int[] buf = new int[n];
        long range = (long)max - min;
        for (long exp = 1; range / exp > 0; exp *= 10) {
            int[] counts = new int[10];
            for (int i = 0; i < n; i++) {
                counts[(int)((ctx[i] - (long)min) / exp % 10)]++;
            }
            for (int d = 1; d < 10; d++) {
                counts[d] += counts[d - 1];
            }
            for (int i = n - 1; i >= 0; i--) {
                int digit = (int)((ctx[i] - (long)min) / exp % 10);
                buf[--counts[digit]] = ctx[i];
            }
            for (int i = 0; i < n; i++) {
                ctx.Write(i, buf[i]);
            }
        }
    }
}
