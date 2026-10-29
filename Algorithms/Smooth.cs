using System.Numerics;
using Sort.Models;

namespace Sort.Algorithms;

// Dijkstra's smoothsort: an in-place heap sort over a forest of Leonardo heaps.
public class Smooth : Sorting {
    private static readonly int[] Leonardo = BuildLeonardo();

    public override string Name => "Smooth";
    public override string Description => "Dijkstra's heap sort variant using a forest of Leonardo heaps. In place, and approaches O(n) on nearly sorted input.";
    public override string Complexity => "Best O(n) · Avg O(n log n) · Worst O(n log n) · Space O(1) · Unstable";

    public override void Sort(SortContext ctx) {
        int hi = ctx.Length - 1;
        if (hi < 1) {
            return;
        }

        ulong p = 1;
        int pshift = 1;
        int head = 0;

        while (head < hi) {
            if ((p & 3) == 3) {
                Sift(ctx, pshift, head);
                p >>= 2;
                pshift += 2;
            } else {
                if (Leonardo[pshift - 1] >= hi - head) {
                    Trinkle(ctx, p, pshift, head, false);
                } else {
                    Sift(ctx, pshift, head);
                }

                if (pshift == 1) {
                    p <<= 1;
                    pshift--;
                } else {
                    p <<= pshift - 1;
                    pshift = 1;
                }
            }
            p |= 1;
            head++;
        }

        Trinkle(ctx, p, pshift, head, false);

        while (pshift != 1 || p != 1) {
            if (pshift <= 1) {
                int trail = BitOperations.TrailingZeroCount(p & ~1UL);
                p >>= trail;
                pshift += trail;
            } else {
                p <<= 2;
                p ^= 7;
                pshift -= 2;
                Trinkle(ctx, p >> 1, pshift + 1, head - Leonardo[pshift] - 1, true);
                Trinkle(ctx, p, pshift, head - 1, true);
            }
            head--;
        }
    }

    private static void Sift(SortContext ctx, int pshift, int head) {
        while (pshift > 1) {
            int rt = head - 1;
            int lf = head - 1 - Leonardo[pshift - 2];

            if (ctx.Compare(head, lf) >= 0 && ctx.Compare(head, rt) >= 0) {
                break;
            }
            if (ctx.Compare(lf, rt) >= 0) {
                ctx.Swap(head, lf);
                head = lf;
                pshift -= 1;
            } else {
                ctx.Swap(head, rt);
                head = rt;
                pshift -= 2;
            }
        }
    }

    private static void Trinkle(SortContext ctx, ulong p, int pshift, int head, bool trusty) {
        while (p != 1) {
            int stepson = head - Leonardo[pshift];
            if (ctx.Compare(stepson, head) <= 0) {
                break;
            }

            if (!trusty && pshift > 1) {
                int rt = head - 1;
                int lf = head - 1 - Leonardo[pshift - 2];
                if (ctx.Compare(rt, stepson) >= 0 || ctx.Compare(lf, stepson) >= 0) {
                    break;
                }
            }

            ctx.Swap(head, stepson);
            head = stepson;
            int trail = BitOperations.TrailingZeroCount(p & ~1UL);
            p >>= trail;
            pshift += trail;
            trusty = false;
        }

        if (!trusty) {
            Sift(ctx, pshift, head);
        }
    }

    private static int[] BuildLeonardo() {
        int[] l = new int[46];
        l[0] = l[1] = 1;
        for (int i = 2; i < l.Length; i++) {
            l[i] = l[i - 1] + l[i - 2] + 1;
        }
        return l;
    }
}
