using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class CopyCommandTests
{
    [Test]
    public void Parse_WhenCopyCommandCopiesSinglePixel_ReturnsImageContainingMatchingPixel()
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
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
    }

    [Test]
    public void Parse_WhenCopyCommandCopiesBoundingBox_ReturnsImageContainingMatchingRegion()
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
    public void Parse_WhenCopyCommandExtendsBeyondImageBounds_ThrowsInvalidOperationException()
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
    public void Parse_WhenCopyCommandUsesUnassignedVariable_ThrowsInvalidOperationException()
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
    public void Parse_WhenCopyCommandOmitsRequiredArguments_ThrowsInvalidOperationException()
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
    public void Parse_WhenCopyCommandStartsOutsideImageBounds_ThrowsInvalidOperationException()
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
    public void Parse_WhenCopyCommandArgumentsAreUnparsable_ThrowsInvalidOperationException()
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
    public void Parse_WhenCopyCommandUsesZeroWidthAndHeight_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(100, 100);
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            portion = copy source 10,20 0,0
            output portion
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy size must be positive"));
    }
}
