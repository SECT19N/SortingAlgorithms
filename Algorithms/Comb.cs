namespace Sort;

public class Comb : Sorting {
    public override string Name => "Comb";

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
