using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sort;

public partial class MainWindow : Window {
    private const int MaxValue = 10000;
    private static readonly Brush BaseBrush = Brushes.SteelBlue;
    private static readonly Brush CompareBrush = Brushes.Gold;
    private static readonly Brush WriteBrush = Brushes.Tomato;
    private static readonly Brush DoneBrush = Brushes.LimeGreen;
    private static readonly Brush ErrorBrush = Brushes.Red;

    private readonly Sorting[] algorithms = [
        new Bubble(), new Cocktail(), new OddEven(), new Gnome(), new Comb(),
        new Insertion(), new Shell(), new Selection(), new Cycle(), new Pancake(),
        new Quick(), new Intro(), new Merge(), new Tim(), new Heap(), new Smooth(), new Tree(),
        new Bitonic(), new Counting(), new Pigeonhole(), new Bucket(), new RadixLsd(), new RadixMsd(),
    ];
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(16) };
    private readonly List<int> highlighted = [];

    private int[] display = [];
    private Rectangle[] bars = [];
    private List<SortOp> ops;
    private int opIndex, compares, swaps, runId;
    private double computeMs;

    public MainWindow() {
        InitializeComponent();
        AlgoBox.ItemsSource = algorithms;
        AlgoBox.SelectedIndex = 0;
        timer.Tick += Timer_Tick;
        Loaded += (s, e) => Reset();
    }

    private int OpsPerTick => Math.Max(1, (int)Math.Pow(1.12, SpeedSlider.Value));

    private void Reset() {
        runId++;
        timer.Stop();
        ops = null;
        SetRunning(false);
        int size = (int)SizeSlider.Value;
        SizeText.Text = size.ToString();
        display = new int[size];
        Functions.GenerateArray(display, 0, size - 1, size);
        BuildBars();
        StatusText.Text = "Ready";
    }

    private void BuildBars() {
        BarCanvas.Children.Clear();
        highlighted.Clear();
        
        double w = BarCanvas.ActualWidth;
        
        if (w <= 0 || display.Length == 0) {
            bars = Array.Empty<Rectangle>();
            return;
        }

        double slot = w / display.Length;
        double gap = slot > 4 ? 1 : 0;
        
        bars = new Rectangle[display.Length];
        
        for (int i = 0; i < display.Length; i++) {
            bars[i] = new Rectangle { Width = Math.Max(1, slot - gap), Fill = BaseBrush };
            Canvas.SetLeft(bars[i], i * slot);
            BarCanvas.Children.Add(bars[i]);
            UpdateBar(i);
        }
    }

    private void UpdateBar(int i) {
        double h = BarCanvas.ActualHeight * display[i] / MaxValue;
        bars[i].Height = h;
        
        Canvas.SetTop(bars[i], BarCanvas.ActualHeight - h);
    }

    private void SetRunning(bool running) {
        SortButton.IsEnabled = !running;
        AlgoBox.IsEnabled = !running;
        SizeSlider.IsEnabled = !running;
        PauseButton.IsEnabled = running;
        StepButton.IsEnabled = running;
        PauseButton.Content = "Pause";
    }

    private async void Sort_Click(object sender, RoutedEventArgs e) {
        Sorting algorithm = (Sorting)AlgoBox.SelectedItem;
        int[] snapshot = (int[])display.Clone();
        int id = ++runId;
        
        SetRunning(true);
        StatusText.Text = "Sorting...";
        
        Stopwatch sw = Stopwatch.StartNew();
        
        List<SortOp> result = await Task.Run(() => {
            SortContext ctx = new SortContext(snapshot);
            algorithm.Sort(ctx);
            return ctx.Ops;
        });
        
        if (id != runId) return;
        
        computeMs = sw.Elapsed.TotalMilliseconds;
        ops = result;
        opIndex = compares = swaps = 0;
        timer.Start();
    }

    private void Pause_Click(object sender, RoutedEventArgs e) {
        if (timer.IsEnabled) {
            timer.Stop();
            PauseButton.Content = "Resume";
        } else if (ops != null) {
            timer.Start();
            PauseButton.Content = "Pause";
        }
    }

    private void Step_Click(object sender, RoutedEventArgs e) {
        if (ops == null) return;
        
        timer.Stop();
        PauseButton.Content = "Resume";
        Advance(1);
    }

    private void Shuffle_Click(object sender, RoutedEventArgs e) => Reset();

    private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (IsLoaded) Reset();
    }

    private void BarCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => BuildBars();

    private void Timer_Tick(object sender, EventArgs e) => Advance(OpsPerTick);

    private void Advance(int count) {
        foreach (int i in highlighted) {
            if (i < bars.Length) bars[i].Fill = BaseBrush;
        }
        
        highlighted.Clear();
        SortOp last = default;
        int applied = 0;
        
        while (applied < count && opIndex < ops.Count) {
            last = ops[opIndex++];
            Apply(last);
            applied++;
        }

        if (applied > 0) Highlight(last);
        
        if (opIndex >= ops.Count) {
            Finish();
        } else {
            StatusText.Text = $"{algorithms[AlgoBox.SelectedIndex].Name}: {opIndex:N0}/{ops.Count:N0} ops, {compares:N0} compares, {swaps:N0} swaps";
        }
    }

    private void Apply(SortOp op) {
        switch (op.Kind) {
            case OpKind.Compare:
                compares++;
                break;
            case OpKind.Swap:
                swaps++;
                (display[op.I], display[op.J]) = (display[op.J], display[op.I]);
                UpdateBar(op.I);
                UpdateBar(op.J);
                break;
            case OpKind.Write:
                display[op.I] = op.Value;
                UpdateBar(op.I);
                break;
        }
    }

    private void Highlight(SortOp op) {
        Brush brush = op.Kind == OpKind.Compare ? CompareBrush : WriteBrush;
        bars[op.I].Fill = brush;
        highlighted.Add(op.I);
        
        if (op.J >= 0) {
            bars[op.J].Fill = brush;
            highlighted.Add(op.J);
        }
    }

    private void Finish() {
        timer.Stop();
        bool sorted = true;
        
        for (int i = 1; i < display.Length; i++) {
            if (display[i - 1] > display[i]) { sorted = false; break; }
        }
        
        foreach (Rectangle bar in bars) bar.Fill = sorted ? DoneBrush : ErrorBrush;
        
        highlighted.Clear();
        ops = null;
        SetRunning(false);
        StatusText.Text = $"{(sorted ? "Sorted" : "NOT SORTED")}: {compares:N0} compares, {swaps:N0} swaps, computed in {computeMs:N2} ms";
    }
}
