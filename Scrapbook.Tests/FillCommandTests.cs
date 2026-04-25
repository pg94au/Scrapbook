using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class FillCommandTests
{
    // Spec 1: Fill single pixel image
    [Test]
    public void Fill_WhenFillingSinglePixel_OutputMatchesInputWithThatPixelFilled()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            image = input 0
            filledPixel = fill image 1,1 1,1 red
            output filledPixel
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(input.Width));
        Assert.That(actual.Height, Is.EqualTo(input.Height));

        for (var y = 0; y < input.Height; y++)
        {
            for (var x = 0; x < input.Width; x++)
            {
                if (x == 1 && y == 1)
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(new Rgba32(255, 0, 0, 255)));
                else
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(input.GetPixel(x, y)));
            }
        }
    }

    // Spec 2: Fill area of specified dimensions
    [Test]
    public void Fill_WhenFillingArea_OutputMatchesInputWithThatAreaFilled()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            image = input 0
            filledArea = fill image 2,2 5,5 blue
            output filledArea
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(input.Width));
        Assert.That(actual.Height, Is.EqualTo(input.Height));

        var blue = new Rgba32(0, 0, 255, 255);

        for (var y = 0; y < input.Height; y++)
        {
            for (var x = 0; x < input.Width; x++)
            {
                if (x >= 2 && x < 7 && y >= 2 && y < 7)
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(blue));
                else
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(input.GetPixel(x, y)));
            }
        }
    }

    // Spec 3: Cannot fill area with zero width
    [Test]
    public void Fill_WhenWidthIsZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,0 0,10 red
            output invalidImage
            """, [input]));
    }

    // Spec 3: Cannot fill area with zero height
    [Test]
    public void Fill_WhenHeightIsZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,0 10,0 red
            output invalidImage
            """, [input]));
    }

    // Spec 4: Cannot fill area with negative width
    [Test]
    public void Fill_WhenWidthIsNegative_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image -5,0 10,10 red
            output invalidImage
            """, [input]));
    }

    // Spec 4: Cannot fill area with negative height
    [Test]
    public void Fill_WhenHeightIsNegative_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,-5 10,10 red
            output invalidImage
            """, [input]));
    }

    // Spec 5: Cannot fill with non-integer width
    [Test]
    public void Fill_WhenWidthIsDecimal_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,0 5.5,10 red
            output invalidImage
            """, [input]));
    }

    // Spec 5: Cannot fill with non-integer height
    [Test]
    public void Fill_WhenHeightIsDecimal_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,0 10,5.5 red
            output invalidImage
            """, [input]));
    }

    // Spec 5: Cannot fill with non-numeric dimensions
    [Test]
    public void Fill_WhenDimensionsAreNonNumeric_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            image = input 0
            invalidImage = fill image 0,0 a,b red
            output invalidImage
            """, [input]));
    }

    // Spec 6: Fill area with bounds greater than the input image dimensions
    [Test]
    public void Fill_WhenBoundsExceedImageDimensions_OnlyOverlappingRegionIsFilled()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(10, 10);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            image = input 0
            filledArea = fill image 5,5 10,10 red
            output filledArea
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(input.Width));
        Assert.That(actual.Height, Is.EqualTo(input.Height));

        var red = new Rgba32(255, 0, 0, 255);

        for (var y = 0; y < input.Height; y++)
        {
            for (var x = 0; x < input.Width; x++)
            {
                if (x >= 5 && y >= 5)
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(red));
                else
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(input.GetPixel(x, y)));
            }
        }
    }
}
