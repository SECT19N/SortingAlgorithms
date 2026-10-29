using Sort.Models;

namespace Sort.Algorithms;

public abstract class Sorting {
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Complexity { get; }
    public abstract void Sort(SortContext ctx);
}
