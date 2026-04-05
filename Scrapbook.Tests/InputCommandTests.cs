using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class InputCommandTests
{
    [Test]
    public void Parse_WhenInputCommandUsesValidIndex_ReturnsImageMatchingExpectedPixels()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            output source
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenInputCommandUsesInvalidIndex_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 3
            output source
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("input index 3 is out of range"));
    }
}
