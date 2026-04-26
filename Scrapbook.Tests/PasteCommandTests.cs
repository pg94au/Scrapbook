using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class PasteCommandTests
{
    // Spec 1: Paste smaller image into the top corner of a larger image.
    [Test]
    public void Paste_WhenSourceIsSmallerThanTarget_OutputIsSameSizeAsTargetWithSourceInTopCorner()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(20, 20);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(40, 40);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second - input 1
            pasted = paste first second 0,0
            output pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(second.Width));
        Assert.That(actual.Height, Is.EqualTo(second.Height));

        // Top-left region matches first image
        for (var y = 0; y < first.Height; y++)
            for (var x = 0; x < first.Width; x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(first.GetPixel(x, y)));

        // Remainder matches second image
        for (var y = 0; y < second.Height; y++)
            for (var x = 0; x < second.Width; x++)
                if (x >= first.Width || y >= first.Height)
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(second.GetPixel(x, y)));
    }

    // Spec 2: Paste image of identical size into the top corner of another image.
    [Test]
    public void Paste_WhenSourceAndTargetAreIdenticalSize_OutputMatchesSource()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(30, 30);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(30, 30);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            pasted = paste first second 0,0
            output pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        ScrapbookTestImageFactory.AssertImagesEqual(first, actual);
    }

    // Spec 3: Paste source image that is wider than the target into the top left corner.
    [Test]
    public void Paste_WhenSourceIsWiderThanTarget_OutputContainsClippedSourceAndRemainingTarget()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(100, 20);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(20, 100);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            pasted = paste first second 0,0
            output pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(second.Width));
        Assert.That(actual.Height, Is.EqualTo(second.Height));

        // Overlapping region (0,0 to 20,20) matches first image
        for (var y = 0; y < Math.Min(first.Height, second.Height); y++)
            for (var x = 0; x < Math.Min(first.Width, second.Width); x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(first.GetPixel(x, y)));

        // Remainder (rows beyond first.Height) matches second image
        for (var y = first.Height; y < second.Height; y++)
            for (var x = 0; x < second.Width; x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(second.GetPixel(x, y)));
    }

    // Spec 4: Paste source image that is taller than the target into the top left corner.
    [Test]
    public void Paste_WhenSourceIsTallerThanTarget_OutputContainsClippedSourceAndRemainingTarget()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(20, 100);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(100, 20);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            pasted = paste first second 0,0
            output pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(second.Width));
        Assert.That(actual.Height, Is.EqualTo(second.Height));

        // Overlapping region (0,0 to 20,20) matches first image
        for (var y = 0; y < Math.Min(first.Height, second.Height); y++)
            for (var x = 0; x < Math.Min(first.Width, second.Width); x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(first.GetPixel(x, y)));

        // Remainder (columns beyond first.Width) matches second image
        for (var y = 0; y < second.Height; y++)
            for (var x = first.Width; x < second.Width; x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(second.GetPixel(x, y)));
    }

    // Spec 5: Paste source image into an offset position of the target.
    [Test]
    public void Paste_WhenSourceIsPastedAtOffset_OutputContainsSourceAtOffsetAndTargetElsewhere()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(20, 20);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(50, 50);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            pasted = paste first second 10,10
            output = pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(second.Width));
        Assert.That(actual.Height, Is.EqualTo(second.Height));

        // Region at offset 10,10 matches first image
        for (var y = 0; y < first.Height; y++)
            for (var x = 0; x < first.Width; x++)
                Assert.That(actual.GetPixel(x + 10, y + 10), Is.EqualTo(first.GetPixel(x, y)));

        // Pixels outside the pasted region match second image
        for (var y = 0; y < second.Height; y++)
            for (var x = 0; x < second.Width; x++)
                if (x < 10 || x >= 10 + first.Width || y < 10 || y >= 10 + first.Height)
                    Assert.That(actual.GetPixel(x, y), Is.EqualTo(second.GetPixel(x, y)));
    }

    // Spec 6: Paste image into a negative offset position of the target.
    [Test]
    public void Paste_WhenSourceIsPastedAtNegativeOffset_OutputContainsClippedSourceFromTopLeft()
    {
        using var first = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        using var second = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            pasted = paste first second -20,-20
            output = pasted
            """, [first, second]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(second.Width));
        Assert.That(actual.Height, Is.EqualTo(second.Height));

        // Top-left of output matches first image offset by 20,20 (first 20 rows/cols cut off)
        for (var y = 0; y < 80; y++)
            for (var x = 0; x < 80; x++)
                Assert.That(actual.GetPixel(x, y), Is.EqualTo(first.GetPixel(x + 20, y + 20)));
    }

    // Spec 7: Paste non-existing image into another image.
    [Test]
    public void Paste_WhenSourceVariableDoesNotExist_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            second = create 10,10
            pasted = paste first second 0,0
            output pasted
            """, []));

        Assert.That(exception!.Message, Does.Match(@"Line \d+"));
        Assert.That(exception!.Message, Does.Contain("variable 'first' was not defined"));
    }

    // Spec 8: Paste image into a non-existing image.
    [Test]
    public void Paste_WhenTargetVariableDoesNotExist_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = create 10,10
            pasted = paste first second 0,0
            output pasted
            """, []));

        Assert.That(exception!.Message, Does.Match(@"Line \d+"));
        Assert.That(exception!.Message, Does.Contain("variable 'second' was not defined"));
    }

    // Spec 9: Paste image into invalid coordinates of another image.
    [Test]
    public void Paste_WhenCoordinatesAreInvalid_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = create 10,10
            second = create 10,10
            pasted = paste first second hello
            output pasted
            """, []));

        Assert.That(exception!.Message, Does.Match(@"Line \d+"));
        Assert.That(exception!.Message, Does.Contain("Syntax error"));
    }
}
