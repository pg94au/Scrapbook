using System.Drawing;
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
            row = copy source 0,0 2,1
            flipped = flip horizontal row
            output flipped
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
    }

    [Test]
    public void Parse_WhenFlipCommandIsVertical_FlipsImageTopToBottom()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            column = copy source 0,0 1,2
            flipped = flip vertical column
            output flipped
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.SunsetOrange));
    }

    [Test]
    public void Parse_WhenFlipCommandUsesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            flipped = flip horizontal missing
            output flipped
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'missing' was not defined"));
    }
}
