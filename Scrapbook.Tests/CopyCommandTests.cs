using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class CopyCommandTests
{
    [Test]
    public void Parse_WhenCopySinglePixelFromInputImage_ReturnsMatchingPixel()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            pixel = copy source 1,1 1,1
            output pixel
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.Width, Is.EqualTo(1));
        Assert.That(actual.Height, Is.EqualTo(1));
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
    }

    [Test]
    public void Parse_WhenCopyBoundingBoxFromInputImage_ReturnsMatchingRegion()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();
        var expectedRegion = new Rectangle(10, 20, 25, 25);

        var outputs = parser.Parse("""
            source = input 0
            portion = copy source 10,20 25,25
            output portion
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertRegionMatches(input, expectedRegion, actual);
    }

    [Test]
    public void Parse_WhenCopyExtendsBeyondBounds_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source 90,90 20,20
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy bounds exceed source image dimensions"));
    }

    [Test]
    public void Parse_WhenCopyUsesUnassignedImageVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            portion = copy unassignedImage 10,20 25,25
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'unassignedImage' was not defined"));
    }

    [Test]
    public void Parse_WhenCopyMissingRequiredArguments_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("Line"));
    }

    [Test]
    public void Parse_WhenCopyTopCornerOutsideBounds_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source 150,150 10,10
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy bounds exceed source image dimensions"));
    }

    [Test]
    public void Parse_WhenCopyArgumentsAreUnparsable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source ten,twenty 25,25
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("Line"));
    }

    [Test]
    public void Parse_WhenCopyWidthAndHeightAreZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source 10,20 0,0
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy size must be positive"));
    }

    [Test]
    public void Parse_WhenCopyFromSecondInputImageWithTwoInputs_ReturnsMatchingRegionFromSecondInput()
    {
        using var firstInput = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        using var secondInput = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        secondInput.SetPixel(10, 20, ScrapbookTestImageFactory.PlumPurple);

        var parser = new ScrapbookParser();
        var expectedRegion = new Rectangle(10, 20, 25, 25);

        var outputs = parser.Parse("""
            source = input 1
            portion = copy source 10,20 25,25
            output portion
            """, new Image[] { firstInput, secondInput });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertRegionMatches(secondInput, expectedRegion, actual);
    }
}
