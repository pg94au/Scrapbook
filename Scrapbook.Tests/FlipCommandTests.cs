using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class FlipCommandTests
{
    [Test]
    public void Parse_WhenFlipCommandIsHorizontal_FlipsImageLeftToRight()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            flipped = flip source horizontal
            output flipped
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.OceanBlue));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
    }

    [Test]
    public void Parse_WhenFlipCommandIsVertical_FlipsImageTopToBottom()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            flipped = flip source vertical
            output flipped
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(actual.GetPixel(0, 1), Is.EqualTo(ScrapbookTestImageFactory.OceanBlue));
        Assert.That(actual.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
    }

    [Test]
    public void Parse_WhenFlipCommandUsesInvalidDirection_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            flipped = flip source diagonal
            output flipped
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("invalid flip direction 'diagonal'"));
    }

    [Test]
    public void Parse_WhenFlipCommandUsesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            flipped = flip unassignedImage horizontal
            output flipped
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'unassignedImage' was not defined"));
    }

    [Test]
    public void Parse_WhenFlipCommandOmitsRequiredArguments_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            flipped = flip source
            output flipped
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("Line"));
    }
}
