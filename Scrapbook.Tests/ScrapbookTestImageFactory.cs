using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

internal static class ScrapbookTestImageFactory
{
    internal static readonly Rgba32 OceanBlue = new(10, 20, 30);
    internal static readonly Rgba32 MeadowGreen = new(40, 50, 60);
    internal static readonly Rgba32 SunsetOrange = new(70, 80, 90);
    internal static readonly Rgba32 PlumPurple = new(100, 110, 120);

    internal static readonly Rgba32 InvertedOceanBlue = new(245, 235, 225);
    internal static readonly Rgba32 InvertedMeadowGreen = new(215, 205, 195);
    internal static readonly Rgba32 InvertedSunsetOrange = new(185, 175, 165);
    internal static readonly Rgba32 InvertedPlumPurple = new(155, 145, 135);

    internal static Image<Rgba32> CreateSampleImage()
    {
        var image = new Image<Rgba32>(2, 2);
        image.SetPixel(0, 0, OceanBlue);
        image.SetPixel(1, 0, MeadowGreen);
        image.SetPixel(0, 1, SunsetOrange);
        image.SetPixel(1, 1, PlumPurple);
        return image;
    }

    internal static Image<Rgba32> CreatePatternImage(int width, int height)
    {
        var image = new Image<Rgba32>(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var red = (x * 37 + y * 17) % 256;
                var green = (x * 67 + y * 29) % 256;
                var blue = (x * 97 + y * 43) % 256;
                image.SetPixel(x, y, new Rgba32((byte)red, (byte)green, (byte)blue));
            }
        }

        return image;
    }

    internal static Rgba32 GetPixel(this Image<Rgba32> image, int x, int y)
        => image.Frames.RootFrame[x, y];

    internal static void SetPixel(this Image<Rgba32> image, int x, int y, Rgba32 pixel)
        => image.Frames.RootFrame[x, y] = pixel;

    internal static void AssertImagesEqual(Image<Rgba32> expected, Image<Rgba32> actual)
    {
        Assert.That(actual.Width, Is.EqualTo(expected.Width));
        Assert.That(actual.Height, Is.EqualTo(expected.Height));

        for (var y = 0; y < expected.Height; y++)
        {
            for (var x = 0; x < expected.Width; x++)
            {
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(expected.GetPixel(x, y)));
            }
        }
    }

    internal static void AssertRegionMatches(Image<Rgba32> source, Rectangle region, Image<Rgba32> actual)
    {
        Assert.That(actual.Width, Is.EqualTo(region.Width));
        Assert.That(actual.Height, Is.EqualTo(region.Height));

        for (var y = 0; y < region.Height; y++)
        {
            for (var x = 0; x < region.Width; x++)
            {
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(source.GetPixel(region.X + x, region.Y + y)));
            }
        }
    }
}
