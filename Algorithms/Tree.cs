using System.Collections.Generic;

namespace Sort;

public class Tree : Sorting {
    public override string Name => "Tree";

    public override void Sort(SortContext ctx) {
        int n = ctx.Length;
        if (n < 2) {
            return;
        }

        // Node k holds the value that started at index k.
        int[] vals = new int[n];
        for (int i = 0; i < n; i++) {
            vals[i] = ctx[i];
        }

        int[] left = new int[n];
        int[] right = new int[n];
        System.Array.Fill(left, -1);
        System.Array.Fill(right, -1);

        for (int i = 1; i < n; i++) {
            int node = 0;
            while (true) {
                if (ctx.CompareValues(vals[i], vals[node], i, node) < 0) {
                    if (left[node] < 0) {
                        left[node] = i;
                        break;
                    }
                    node = left[node];
                } else {
                    if (right[node] < 0) {
                        right[node] = i;
                        break;
                    }
                    node = right[node];
                }
            }
        }

        // Iterative in-order traversal (the tree can be n deep on sorted input).
        Stack<int> stack = new();
        int cur = 0;
        int k = 0;
        while (cur >= 0 || stack.Count > 0) {
            while (cur >= 0) {
                stack.Push(cur);
                cur = left[cur];
            }
            cur = stack.Pop();
            ctx.Write(k++, vals[cur]);
            cur = right[cur];
        }
    }
}
