namespace Sort;

public class Gnome : Sorting {
    public override string Name => "Gnome";

    public override void Sort(SortContext ctx) {
        int i = 0;
        while (i < ctx.Length) {
            if (i == 0 || ctx.Compare(i - 1, i) <= 0) {
                i++;
            } else {
                ctx.Swap(i - 1, i);
                i--;
            }
        }
    }
}
