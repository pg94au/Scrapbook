using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class RotateCommandTests
{
    [Test]
    public void Parse_WhenRotateCommandUsesValidSource_RotatesImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            source = input 0
            row = copy source 0,0 2,1
            rotated = rotate row 180
            output rotated
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.MeadowGreen));
    }

    [Test]
    public void Parse_WhenRotateCommandUsesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            rotated = rotate missing 90
            output rotated
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("variable 'missing' was not defined"));
    }
}
