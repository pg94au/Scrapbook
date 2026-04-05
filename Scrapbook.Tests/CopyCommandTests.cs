using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class CopyCommandTests
{
    [Test]
    public void Parse_WhenCopyCommandUsesValidBounds_ReturnsCopiedRegion()
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
    public void Parse_WhenCopyCommandUsesOutOfBoundsRegion_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            region = copy source 1,1 2,2
            output region
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy bounds exceed source image dimensions"));
    }

    [Test]
    public void Parse_WhenCopyCommandUsesZeroSize_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            source = input 0
            region = copy source 0,0 0,1
            output region
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy size must be positive"));
    }
}
