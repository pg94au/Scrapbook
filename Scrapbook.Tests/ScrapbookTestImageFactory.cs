using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

internal static class ScrapbookTestImageFactory
{
    internal static readonly Color OceanBlue = Color.FromArgb(10, 20, 30);
    internal static readonly Color MeadowGreen = Color.FromArgb(40, 50, 60);
    internal static readonly Color SunsetOrange = Color.FromArgb(70, 80, 90);
    internal static readonly Color PlumPurple = Color.FromArgb(100, 110, 120);

    internal static readonly Color InvertedOceanBlue = Color.FromArgb(245, 235, 225);
    internal static readonly Color InvertedMeadowGreen = Color.FromArgb(215, 205, 195);
    internal static readonly Color InvertedSunsetOrange = Color.FromArgb(185, 175, 165);
    internal static readonly Color InvertedPlumPurple = Color.FromArgb(155, 145, 135);

    internal static Bitmap CreateSampleImage()
    {
        var bitmap = new Bitmap(2, 2);
        bitmap.SetPixel(0, 0, OceanBlue);
        bitmap.SetPixel(1, 0, MeadowGreen);
        bitmap.SetPixel(0, 1, SunsetOrange);
        bitmap.SetPixel(1, 1, PlumPurple);
        return bitmap;
    }

    internal static Bitmap CreatePatternImage(int width, int height)
    {
        var bitmap = new Bitmap(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var red = (x * 37 + y * 17) % 256;
                var green = (x * 67 + y * 29) % 256;
                var blue = (x * 97 + y * 43) % 256;
                bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
            }
        }

        return bitmap;
    }

    internal static void AssertImagesEqual(Image expected, Image actual)
    {
        using var expectedBitmap = new Bitmap(expected);
        using var actualBitmap = new Bitmap(actual);

        Assert.That(actualBitmap.Width, Is.EqualTo(expectedBitmap.Width));
        Assert.That(actualBitmap.Height, Is.EqualTo(expectedBitmap.Height));

        for (var y = 0; y < expectedBitmap.Height; y++)
        {
            for (var x = 0; x < expectedBitmap.Width; x++)
            {
                Assert.That(actualBitmap.GetPixel(x, y), Is.EqualTo(expectedBitmap.GetPixel(x, y)));
            }
        }
    }

    internal static void AssertRegionMatches(Image source, Rectangle region, Image actual)
    {
        using var sourceBitmap = new Bitmap(source);
        using var actualBitmap = new Bitmap(actual);

        Assert.That(actualBitmap.Width, Is.EqualTo(region.Width));
        Assert.That(actualBitmap.Height, Is.EqualTo(region.Height));

        for (var y = 0; y < region.Height; y++)
        {
            for (var x = 0; x < region.Width; x++)
            {
                Assert.That(actualBitmap.GetPixel(x, y), Is.EqualTo(sourceBitmap.GetPixel(region.X + x, region.Y + y)));
            }
        }
    }
}
