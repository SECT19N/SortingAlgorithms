using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Sort.Algorithms;
using Sort.Enums;
using Sort.Helpers;
using Sort.Models;

namespace Sort.Views;

public partial class MainWindow : Window {
    private const double FrameBudgetMs = 12;

    private readonly Sorting[] algorithms = [
        new Bubble(), new Cocktail(), new OddEven(), new Gnome(), new Comb(),
        new Insertion(), new Shell(), new Selection(), new Cycle(), new Pancake(),
        new Quick(), new Intro(), new Merge(), new Tim(), new Heap(), new Smooth(), new Tree(),
        new Bitonic(), new Counting(), new Pigeonhole(), new Bucket(), new RadixLsd(), new RadixMsd(),
    ];
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(16) };
    private readonly BarRenderer renderer = new();
    private readonly ToneSynth synth = new();
    private readonly int[] hot = new int[2];

    private int[] display = [];
    private SortRun run;
    private Sorting running;
    private uint baseColor = BarRenderer.Base, hotColor;
    private int hotCount, maxValue = 1000;
    private long compares, swaps, writes;

    public MainWindow() {
        InitializeComponent();
        AlgoBox.ItemsSource = algorithms;
        AlgoBox.SelectedIndex = 0;
        timer.Tick += (s, e) => Advance(OpsPerTick);
        Loaded += (s, e) => Reset();
        Closed += (s, e) => {
            run?.Cancel();
            synth.Dispose();
        };
    }

    private int OpsPerTick => (int)Math.Min(int.MaxValue, Math.Max(1, Math.Pow(1.12, SpeedSlider.Value)));

    private int SliderSize => (int)Math.Round(Math.Pow(10, SizeSlider.Value));

    private void Reset() {
        StopRun();
        int size = SliderSize;
        SizeText.Text = size.ToString("N0");
        maxValue = Math.Max(size, 1000);
        display = new int[size];
        Functions.GenerateArray(display, maxValue);
        baseColor = BarRenderer.Base;
        hotCount = 0;
        SetRunning(false);
        Redraw();
        StatusText.Text = "Ready";
    }

    private void StopRun() {
        timer.Stop();
        run?.Cancel();
        run = null;
    }

    private void Redraw() => renderer.Render(display, maxValue, baseColor, hot, hotCount, hotColor);

    private void SetRunning(bool isRunning) {
        SortButton.IsEnabled = !isRunning;
        AlgoBox.IsEnabled = !isRunning;
        SizeSlider.IsEnabled = !isRunning;
        ShuffleButton.IsEnabled = !isRunning;
        PauseButton.IsEnabled = isRunning;
        StepButton.IsEnabled = isRunning;
        StopButton.IsEnabled = isRunning;
        PauseButton.Content = "Pause";
    }

    private void Sort_Click(object sender, RoutedEventArgs e) {
        running = (Sorting)AlgoBox.SelectedItem;
        compares = swaps = writes = 0;
        baseColor = BarRenderer.Base;
        hotCount = 0;
        run = new SortRun(running, (int[])display.Clone());
        SetRunning(true);
        StatusText.Text = "Computing...";
        timer.Start();
    }

    private void Pause_Click(object sender, RoutedEventArgs e) {
        if (timer.IsEnabled) {
            timer.Stop();
            PauseButton.Content = "Resume";
        } else if (run != null) {
            timer.Start();
            PauseButton.Content = "Pause";
        }
    }

    private void Step_Click(object sender, RoutedEventArgs e) {
        if (run == null) return;

        timer.Stop();
        PauseButton.Content = "Resume";
        Advance(1);
    }

    private void Stop_Click(object sender, RoutedEventArgs e) {
        StopRun();
        hotCount = 0;
        SetRunning(false);
        Redraw();
        StatusText.Text = "Stopped";
    }

    private void Shuffle_Click(object sender, RoutedEventArgs e) => Reset();

    private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (IsLoaded) Reset();
    }

    private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        synth.Volume = (float)VolumeSlider.Value;
    }

    private void AlgoBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
        if (AlgoBox.SelectedItem is Sorting s) {
            InfoText.Text = $"{s.Name}: {s.Description}  [{s.Complexity}]";
        }
    }

    private void BarHost_SizeChanged(object sender, SizeChangedEventArgs e) {
        DpiScale dpi = VisualTreeHelper.GetDpi(this);
        int pw = (int)Math.Round(BarHost.ActualWidth * dpi.DpiScaleX);
        int ph = (int)Math.Round(BarHost.ActualHeight * dpi.DpiScaleY);
        renderer.Resize(pw, ph, 96 * dpi.DpiScaleX, 96 * dpi.DpiScaleY);
        BarImage.Source = renderer.Source;
        Redraw();
    }

    private void Advance(int count) {
        if (run == null) return;

        Stopwatch budget = Stopwatch.StartNew();
        SortOp last = default;
        int applied = 0;

        while (applied < count && run.TryRead(out SortOp op)) {
            last = op;
            Apply(op);
            applied++;

            // Keep the UI responsive at extreme speeds by stopping the frame early.
            if ((applied & 1023) == 0 && budget.Elapsed.TotalMilliseconds > FrameBudgetMs) break;
        }

        hotCount = 0;
        if (applied > 0) SetHot(last);

        if (run.IsDone) {
            Finish();
            return;
        }

        Redraw();

        if (applied == 0) {
            StatusText.Text = $"{running.Name}: computing...";
            return;
        }

        StatusText.Text = $"{running.Name}: {compares:N0} compares, {swaps:N0} swaps, {writes:N0} writes";

        if (SoundCheck.IsChecked == true) {
            int value = last.Kind == OpKind.Write ? last.Value : display[last.I];
            synth.Note(150 * Math.Pow(2, 3.6 * value / maxValue));
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
                break;
            case OpKind.Write:
                writes++;
                display[op.I] = op.Value;
                break;
        }
    }

    private void SetHot(SortOp op) {
        hotColor = op.Kind == OpKind.Compare ? BarRenderer.Compare : BarRenderer.Write;
        hot[hotCount++] = op.I;
        if (op.J >= 0) hot[hotCount++] = op.J;
    }

    private void Finish() {
        timer.Stop();
        bool sorted = true;

        for (int i = 1; i < display.Length; i++) {
            if (display[i - 1] > display[i]) { sorted = false; break; }
        }

        string error = run.Error;
        double computeMs = run.ComputeMs;
        baseColor = sorted ? BarRenderer.Done : BarRenderer.Error;
        hotCount = 0;
        run = null;
        SetRunning(false);
        Redraw();

        StatusText.Text = error != null
            ? $"Failed: {error}"
            : $"{(sorted ? "Sorted" : "NOT SORTED")}: {compares:N0} compares, {swaps:N0} swaps, {writes:N0} writes, computed in {computeMs:N0} ms";
    }
}
