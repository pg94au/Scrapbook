using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ReverseCommandTests
{
    [Test]
    public void Parse_WhenImageIsReversedAndOutput_ReturnsReversedImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            reversed = reverse first
            output reversed
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.InvertedOceanBlue));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.InvertedMeadowGreen));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.InvertedSunsetOrange));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.InvertedPlumPurple));
    }

    [Test]
    public void Parse_WhenImageIsReversedTwice_ReturnsOriginalImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            reversed = reverse first
            reversed_twice = reverse reversed
            output reversed_twice
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenReverseUsesNonExistentVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            reversed = reverse non_existent_image
            output reversed
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Match(@"Line \d+"));
        Assert.That(exception!.Message, Does.Contain("variable 'non_existent_image' was not defined"));
    }
}
