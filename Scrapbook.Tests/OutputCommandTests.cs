using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class OutputCommandTests
{
    [Test]
    public void Parse_WhenOutputCommandAppearsMultipleTimes_ReturnsImagesInScriptOrder()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            # Return original source first
            source = input 0
            # Return a second image that is just one copied pixel
            copiedPixel = copy source 1,1 1,1
            output source
            output copiedPixel
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(2));

        using var first = (Bitmap)outputs[0];
        using var second = (Bitmap)outputs[1];

        Assert.That(first.GetPixel(1, 1), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
        Assert.That(second.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
    }

    [Test]
    public void Parse_WhenOutputCommandUsesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("output missing", new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'missing' was not defined"));
    }
}
