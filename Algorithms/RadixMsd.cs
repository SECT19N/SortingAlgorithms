namespace Sort;

public class RadixMsd : Sorting {
    public override string Name => "Radix MSD";

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

        long exp = 1;
        while (((long)max - min) / exp >= 10) {
            exp *= 10;
        }
        Distribute(ctx, new int[n], min, 0, n - 1, exp);
    }

    private void Distribute(SortContext ctx, int[] buf, int min, int lo, int hi, long exp) {
        if (lo >= hi || exp == 0) {
            return;
        }

        int[] start = new int[11];
        for (int i = lo; i <= hi; i++) {
            start[Digit(ctx[i], min, exp) + 1]++;
        }
        for (int d = 0; d < 10; d++) {
            start[d + 1] += start[d];
        }

        int[] next = (int[])start.Clone();
        for (int i = lo; i <= hi; i++) {
            buf[lo + next[Digit(ctx[i], min, exp)]++] = ctx[i];
        }
        for (int i = lo; i <= hi; i++) {
            ctx.Write(i, buf[i]);
        }

        for (int d = 0; d < 10; d++) {
            Distribute(ctx, buf, min, lo + start[d], lo + start[d + 1] - 1, exp / 10);
        }
    }

    private static int Digit(int value, int min, long exp) {
        return (int)((value - (long)min) / exp % 10);
    }
}
