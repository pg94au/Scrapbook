using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ParsingBehaviorTests
{
    [Test]
    public void Parse_WhenScriptContainsCommentsAndOutputsInput_ReturnsImageMatchingExpectedPixels()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """
            # comment
            a = input 0 # inline comment
            output a
            """;

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenScriptContainsMultipleOutputStatements_ReturnsOutputsInScriptOrder()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """
            source = input 0
            pixel = copy source 1,1 1,1
            output source
            output pixel
            """;

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(2));

        using var first = (Bitmap)outputs[0];
        Assert.That(first.Width, Is.EqualTo(2));

        using var second = (Bitmap)outputs[1];
        Assert.That(second.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.PlumPurple));
    }
}
