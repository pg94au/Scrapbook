using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class TransformationBehaviorTests
{
    [Test]
    public void Parse_WhenScriptAppliesTransformPipeline_ReturnsImageMatchingExpectedPixels()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """
            src = input 0
            part = copy src 0,0 2,1
            rot = rotate part 180
            flipped = flip horizontal rot
            reversed = reverse flipped
            output reversed
            """;

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));

        using var actual = (Bitmap)outputs[0];
        Assert.That(actual.GetPixel(0, 0), Is.EqualTo(ScrapbookTestImageFactory.InvertedOceanBlue));
        Assert.That(actual.GetPixel(1, 0), Is.EqualTo(ScrapbookTestImageFactory.InvertedMeadowGreen));
    }
}
