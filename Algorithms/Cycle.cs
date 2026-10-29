namespace Sort;

public class Cycle : Sorting {
    public override string Name => "Cycle";

    public override void Sort(SortContext ctx) {
        int n = ctx.Length;
        for (int start = 0; start < n - 1; start++) {
            int item = ctx[start];
            int pos = CountSmaller(ctx, item, start);
            if (pos == start) {
                continue;
            }

            // Rotate the cycle: drop the held item into place and pick up what was there.
            pos = SkipEqual(ctx, item, pos);
            (item, _) = Place(ctx, item, pos);
            while (pos != start) {
                pos = SkipEqual(ctx, item, CountSmaller(ctx, item, start));
                (item, _) = Place(ctx, item, pos);
            }
        }
    }

    private static int CountSmaller(SortContext ctx, int item, int start) {
        int pos = start;
        for (int i = start + 1; i < ctx.Length; i++) {
            if (ctx.CompareValues(ctx[i], item, i, start) < 0) {
                pos++;
            }
        }
        return pos;
    }

    private static int SkipEqual(SortContext ctx, int item, int pos) {
        while (item == ctx[pos]) {
            pos++;
        }
        return pos;
    }

    private static (int item, int pos) Place(SortContext ctx, int item, int pos) {
        int old = ctx[pos];
        if (item != old) {
            ctx.Write(pos, item);
        }
        return (old, pos);
    }
}
