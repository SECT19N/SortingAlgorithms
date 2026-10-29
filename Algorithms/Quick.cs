namespace Sort;

public class Quick : Sorting {
    public override string Name => "Quick";
    public override void Sort(SortContext ctx) {
        QuickSort(ctx, 0, ctx.Length - 1);
    }
    private void QuickSort(SortContext ctx, int left, int right) {
        if (left < right) {
            int pivot = Partition(ctx, left, right);
            QuickSort(ctx, left, pivot - 1);
            QuickSort(ctx, pivot + 1, right);
        }
    }
    private int Partition(SortContext ctx, int left, int right) {
        int i = left - 1;
        for (int j = left; j <= right - 1; j++) {
            if (ctx.Compare(j, right) <= 0) {
                i++;
                
                if (i != j) {
                    ctx.Swap(i, j);
                }
            }
        }

        if (i + 1 != right) {
            ctx.Swap(i + 1, right);
        }
        
        return i + 1;
    }
}
