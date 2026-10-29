namespace Sort;

public class Counting : Sorting {
    public override string Name => "Counting";

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
