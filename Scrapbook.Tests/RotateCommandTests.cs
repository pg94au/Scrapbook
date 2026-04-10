using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class RotateCommandTests
{
    [Test]
    public void Parse_WhenImageIsRotatedByNinetyDegrees_ReturnsNinetyDegreeRotatedImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            rotated = rotate first 90
            output rotated
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.OceanBlue));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
    }

    [Test]
    public void Parse_WhenImageIsRotatedFourTimesByNinetyDegrees_ReturnsOriginalImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            rotated_90 = rotate first 90
            rotated_180 = rotate rotated_90 90
            rotated_270 = rotate rotated_180 90
            rotated_360 = rotate rotated_270 90
            output rotated_360
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenRotateUsesNonExistentImageVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            rotated = rotate non_existent_image 90
            output rotated
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'non_existent_image' was not defined"));
    }

    [Test]
    public void Parse_WhenRotateAngleIsInvalid_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            rotated = rotate first fred
            output rotated
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("Line"));
    }

    [Test]
    public void Parse_WhenImageIsRotatedByArbitraryAngle_ReturnsRotatedImageWithTransparentBackground()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(2, 3);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            rotated = rotate first 45
            output rotated
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];

        Assert.That(actual.Width, Is.GreaterThan(input.Width));
        Assert.That(actual.Height, Is.GreaterThan(input.Height));
        Assert.That(actual.GetPixel(0, 0).A, Is.EqualTo(0));
    }

    [Test]
    public void Parse_WhenImageIsRotatedByNegativeNinetyDegrees_ReturnsNegativeNinetyDegreeRotatedImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            rotated = rotate first -90
            output rotated
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.OceanBlue));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
    }

    [Test]
    public void Parse_WhenImageIsRotatedByAngleGreaterThanThreeSixtyDegrees_ReturnsEquivalentRotation()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            rotated = rotate first 450
            output rotated
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.OceanBlue));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
    }
}
