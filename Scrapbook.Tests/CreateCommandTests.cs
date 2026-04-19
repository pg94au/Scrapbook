using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class CreateCommandTests
{
    // Spec 1: Create single pixel image
    [Test]
    public void Create_WhenDimensionsAreOneByOne_ReturnsSingleTransparentPixel()
    {
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            pixel = create 1,1
            output pixel
            """, []);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(1));
        Assert.That(actual.Height, Is.EqualTo(1));
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(new Rgba32(0, 0, 0, 0)));
    }

    // Spec 2: Create an image of specified dimensions
    [Test]
    public void Create_WhenDimensionsAreSpecified_ReturnsTransparentImageOfCorrectSize()
    {
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            image = create 10,20
            output image
            """, []);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(10));
        Assert.That(actual.Height, Is.EqualTo(20));

        for (var y = 0; y < actual.Height; y++)
        {
            for (var x = 0; x < actual.Width; x++)
            {
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(new Rgba32(0, 0, 0, 0)));
            }
        }
    }

    // Spec 3: Cannot create an image with zero width
    [Test]
    public void Create_WhenWidthIsZero_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create 0,10
            """, []));

        Assert.That(exception!.Message, Does.Contain("dimensions"));
    }

    // Spec 3: Cannot create an image with zero height
    [Test]
    public void Create_WhenHeightIsZero_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create 10,0
            """, []));

        Assert.That(exception!.Message, Does.Contain("dimensions"));
    }

    // Spec 4: Cannot create an image with negative width
    [Test]
    public void Create_WhenWidthIsNegative_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create -5,10
            """, []));

        Assert.That(exception!.Message, Does.Contain("dimensions"));
    }

    // Spec 4: Cannot create an image with negative height
    [Test]
    public void Create_WhenHeightIsNegative_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create 10,-5
            """, []));

        Assert.That(exception!.Message, Does.Contain("dimensions"));
    }

    // Spec 5: Cannot create with non-integer width (float)
    [Test]
    public void Create_WhenWidthIsFloat_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create 10.5,20
            """, []));
    }

    // Spec 5: Cannot create with non-integer height (float)
    [Test]
    public void Create_WhenHeightIsFloat_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create 10,20.5
            """, []));
    }

    // Spec 5: Cannot create with non-numeric dimensions
    [Test]
    public void Create_WhenDimensionsAreNonNumeric_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            invalidImage = create a,b
            """, []));
    }

    // Spec 6: Create with named color
    [Test]
    public void Create_WhenNamedColorIsSpecified_ReturnsImageFilledWithThatColor()
    {
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            coloredImage = create 10,20 red
            output coloredImage
            """, []);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(10));
        Assert.That(actual.Height, Is.EqualTo(20));

        var expectedRed = new Rgba32(255, 0, 0, 255);
        for (var y = 0; y < actual.Height; y++)
        {
            for (var x = 0; x < actual.Width; x++)
            {
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(expectedRed));
            }
        }
    }

    // Spec 6: Create with hex color
    [Test]
    public void Create_WhenHexColorIsSpecified_ReturnsImageFilledWithThatColor()
    {
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            coloredImage = create 10,20 #FF0000
            output coloredImage
            """, []);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(10));
        Assert.That(actual.Height, Is.EqualTo(20));

        var expectedRed = new Rgba32(255, 0, 0, 255);
        for (var y = 0; y < actual.Height; y++)
        {
            for (var x = 0; x < actual.Width; x++)
            {
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(expectedRed));
            }
        }
    }
}
