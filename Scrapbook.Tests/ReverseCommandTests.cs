using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ReverseCommandTests
{
    [Test]
    public void Parse_WhenReverseCommandUsesValidSource_ReversesImageColors()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            row = copy source 0,0 1,1
            reversed = reverse row
            output reversed
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.InvertedOceanBlue));
    }

    [Test]
    public void Parse_WhenReverseCommandUsesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            reversed = reverse missing
            output reversed
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'missing' was not defined"));
    }
}
