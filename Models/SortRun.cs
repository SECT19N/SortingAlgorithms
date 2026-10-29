using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Sort.Algorithms;

namespace Sort.Models;

// Runs an algorithm on a background thread and streams its operations through a bounded queue,
// so memory stays flat no matter how many operations the algorithm produces.
public sealed class SortRun : IDisposable {
    private const int QueuedChunks = 32;
    private const int StackBytes = 256 * 1024 * 1024;

    private readonly BlockingCollection<(SortOp[] Buf, int Count)> queue = new(QueuedChunks);
    private readonly CancellationTokenSource cts = new();
    private SortOp[] current = [];
    private int currentCount, currentPos;

    public double ComputeMs { get; private set; }
    public string Error { get; private set; }

    public SortRun(Sorting algorithm, int[] snapshot) {
        Thread thread = new(() => Produce(algorithm, snapshot), StackBytes) { IsBackground = true };
        thread.Start();
    }

    // True once every operation has been read.
    public bool IsDone => currentPos >= currentCount && queue.IsCompleted;

    public bool TryRead(out SortOp op) {
        while (currentPos >= currentCount) {
            if (!queue.TryTake(out var chunk)) {
                op = default;
                return false;
            }
            current = chunk.Buf;
            currentCount = chunk.Count;
            currentPos = 0;
        }
        op = current[currentPos++];
        return true;
    }

    public void Cancel() => cts.Cancel();

    public void Dispose() => cts.Cancel();

    private void Produce(Sorting algorithm, int[] snapshot) {
        Stopwatch sw = Stopwatch.StartNew();
        try {
            SortContext ctx = new(snapshot, (buf, count) => queue.Add((buf, count), cts.Token));
            algorithm.Sort(ctx);
            ctx.Flush();
            ComputeMs = sw.Elapsed.TotalMilliseconds;
        } catch (OperationCanceledException) {
        } catch (Exception e) {
            Error = e.Message;
        } finally {
            queue.CompleteAdding();
        }
    }
}
