using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class OutputCommandTests
{
    [Test]
    public void Parse_WhenSameInputImageIsOutput_ReturnsIdenticalImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            output first
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenMultipleInputImagesAreOutput_ReturnsMatchingImages()
    {
        using var firstInput = ScrapbookTestImageFactory.CreateSampleImage();
        using var secondInput = ScrapbookTestImageFactory.CreatePatternImage(4, 4);
        secondInput.SetPixel(0, 0, ScrapbookTestImageFactory.PlumPurple);

        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            output first
            output second
            """, new Image[] { firstInput, secondInput });

        Assert.That(outputs, Has.Count.EqualTo(2));

        using var firstActual = (Bitmap)outputs[0];
        using var secondActual = (Bitmap)outputs[1];

        ScrapbookTestImageFactory.AssertImagesEqual(firstInput, firstActual);
        ScrapbookTestImageFactory.AssertImagesEqual(secondInput, secondActual);
    }

    [Test]
    public void Parse_WhenSameImageIsOutputRepeatedly_ReturnsIdenticalImagesEachTime()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            output first
            output first
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(2));

        using var firstActual = (Bitmap)outputs[0];
        using var secondActual = (Bitmap)outputs[1];

        ScrapbookTestImageFactory.AssertImagesEqual(input, firstActual);
        ScrapbookTestImageFactory.AssertImagesEqual(input, secondActual);
    }

    [Test]
    public void Parse_WhenOutputImageHasNotBeenAssigned_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            output first
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'first' was not defined"));
    }

    [Test]
    public void Parse_WhenModifiedImageIsOutput_ReturnsRotatedImageThatDiffersFromInput()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            modified = rotate first 90
            output modified
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
        Assert.That(actual.GetPixel(0, 0), Is.Not.EqualTo(ScrapbookTestImageFactory.OceanBlue));
    }
}
