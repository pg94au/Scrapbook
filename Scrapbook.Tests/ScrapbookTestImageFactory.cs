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

    internal static Bitmap CreateSampleImage()
    {
        var bitmap = new Bitmap(2, 2);
        bitmap.SetPixel(0, 0, OceanBlue);
        bitmap.SetPixel(1, 0, MeadowGreen);
        bitmap.SetPixel(0, 1, SunsetOrange);
        bitmap.SetPixel(1, 1, PlumPurple);
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
}
