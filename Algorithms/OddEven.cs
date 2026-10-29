namespace Sort;

public class OddEven : Sorting {
    public override string Name => "Odd-Even";

    public override void Sort(SortContext ctx) {
        bool sorted = false;
        while (!sorted) {
            sorted = true;
            for (int start = 1; start >= 0; start--) {
                for (int i = start; i + 1 < ctx.Length; i += 2) {
                    if (ctx.Compare(i, i + 1) > 0) {
                        ctx.Swap(i, i + 1);
                        sorted = false;
                    }
                }
            }
        }
    }
}
