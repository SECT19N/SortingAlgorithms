using System;
using System.Threading;
using NAudio.Wave;

namespace Sort.Helpers;

// Monophonic sine synth: Note() restarts a short decaying tone at the given pitch.
// If no audio device is available it silently does nothing.
public sealed class ToneSynth : ISampleProvider, IDisposable {
    private const int Rate = 44100;

    private readonly WaveOutEvent output;
    private float targetFreq = 440, freq = 440, amp, phase;
    private int trigger;

    public float Volume { get; set; } = 0.25f;
    public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(Rate, 1);

    public ToneSynth() {
        try {
            output = new WaveOutEvent { DesiredLatency = 80 };
            output.Init(this);
            output.Play();
        } catch (Exception) {
            output?.Dispose();
            output = null;
        }
    }

    public bool Available => output != null;

    public void Note(double hz) {
        if (output == null) {
            return;
        }
        Volatile.Write(ref targetFreq, (float)hz);
        Interlocked.Exchange(ref trigger, 1);
    }

    public int Read(float[] buffer, int offset, int count) {
        float target = Volatile.Read(ref targetFreq);
        float volume = Volume;
        if (Interlocked.Exchange(ref trigger, 0) == 1) {
            amp = 1;
        }

        for (int i = 0; i < count; i++) {
            freq += (target - freq) * 0.01f;
            buffer[offset + i] = MathF.Sin(phase) * amp * volume;
            phase += MathF.Tau * freq / Rate;
            if (phase > MathF.Tau) {
                phase -= MathF.Tau;
            }
            amp *= 0.9997f;
        }
        return count;
    }

    public void Dispose() => output?.Dispose();
}
