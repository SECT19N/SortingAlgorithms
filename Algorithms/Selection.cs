namespace Sort;

public class Selection : Sorting {
    public override string Name => "Selection";
    public override void Sort(SortContext ctx) {
        for (int i = 0; i < ctx.Length - 1; i++) {
            int minIndex = i;

            for (int j = i + 1; j < ctx.Length; j++) {
                if (ctx.Compare(j, minIndex) < 0) {
                    minIndex = j;
                }
            }

            if (minIndex != i) {
                ctx.Swap(i, minIndex);
            }
        }
    }
}
