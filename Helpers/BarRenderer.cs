using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sort.Helpers;

// Draws the array into a WriteableBitmap: one bar per item when they fit, otherwise one
// column per pixel showing the mean of the items it covers. Cost is O(items + pixels) per frame.
public sealed class BarRenderer {
    public const uint Background = 0xFF1E1E1E;
    public const uint Base = 0xFF4682B4;
    public const uint Compare = 0xFFFFD700;
    public const uint Write = 0xFFFF6347;
    public const uint Done = 0xFF32CD32;
    public const uint Error = 0xFFFF0000;

    private WriteableBitmap bitmap;
    private uint[] pixels = [];
    private int[] heights = [];
    private uint[] colors = [];
    private int width, height;

    public ImageSource Source => bitmap;

    public void Resize(int pixelWidth, int pixelHeight, double dpiX, double dpiY) {
        width = Math.Max(0, pixelWidth);
        height = Math.Max(0, pixelHeight);
        pixels = new uint[width * height];
        heights = new int[width];
        colors = new uint[width];
        bitmap = width > 0 && height > 0 ? new WriteableBitmap(width, height, dpiX, dpiY, PixelFormats.Bgra32, null) : null;
    }

    public void Render(int[] data, int maxValue, uint baseColor, int[] hot, int hotCount, uint hotColor) {
        if (bitmap == null) {
            return;
        }

        int n = data.Length;
        Array.Clear(heights);
        Array.Fill(colors, baseColor);

        if (n > 0 && n <= width) {
            bool gap = width / n > 4;
            for (int i = 0; i < n; i++) {
                int x0 = (int)((long)i * width / n);
                int x1 = (int)((long)(i + 1) * width / n);
                if (gap && x1 - x0 > 1) {
                    x1--;
                }
                int h = Scale(data[i], maxValue);
                for (int x = x0; x < x1; x++) {
                    heights[x] = h;
                }
            }
            for (int k = 0; k < hotCount; k++) {
                int i = hot[k];
                int x0 = (int)((long)i * width / n);
                int x1 = Math.Max(x0 + 1, (int)((long)(i + 1) * width / n) - (gap ? 1 : 0));
                for (int x = x0; x < x1; x++) {
                    colors[x] = hotColor;
                }
            }
        } else if (n > 0) {
            for (int x = 0; x < width; x++) {
                int lo = (int)((long)x * n / width);
                int hi = (int)((long)(x + 1) * n / width);
                long sum = 0;
                for (int i = lo; i < hi; i++) {
                    sum += data[i];
                }
                heights[x] = Scale(sum / (hi - lo), maxValue);
            }
            for (int k = 0; k < hotCount; k++) {
                colors[(int)((long)hot[k] * width / n)] = hotColor;
            }
        }

        for (int y = 0; y < height; y++) {
            int level = height - y;
            int row = y * width;
            for (int x = 0; x < width; x++) {
                pixels[row + x] = heights[x] >= level ? colors[x] : Background;
            }
        }

        bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
    }

    private int Scale(long value, int maxValue) =>
        (int)Math.Clamp(value * height / maxValue, 0, height);
}
