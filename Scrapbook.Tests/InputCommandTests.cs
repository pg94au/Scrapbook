using System.Drawing;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class InputCommandTests
{
    [Test]
    public void Parse_WhenOnlyImageIsInputAndExported_ReturnsIdenticalImage()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            output first
            """, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = (Bitmap)outputs[0];
        ScrapbookTestImageFactory.AssertImagesEqual(input, actual);
    }

    [Test]
    public void Parse_WhenMultipleImagesAreInputAndReferencedByIndex_ReturnsMatchingOutputs()
    {
        using var firstInput = ScrapbookTestImageFactory.CreateSampleImage();
        using var secondInput = ScrapbookTestImageFactory.CreatePatternImage(4, 4);
        secondInput.SetPixel(0, 0, ScrapbookTestImageFactory.PlumPurple);

        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            second = input 1
            output first
            output second
            """, new Image[] { firstInput, secondInput });

        Assert.That(outputs, Has.Count.EqualTo(2));

        using var firstActual = (Bitmap)outputs[0];
        using var secondActual = (Bitmap)outputs[1];

        ScrapbookTestImageFactory.AssertImagesEqual(firstInput, firstActual);
        ScrapbookTestImageFactory.AssertImagesEqual(secondInput, secondActual);
    }

    [Test]
    public void Parse_WhenInputIndexDoesNotExist_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            missing = input 1
            output missing
            """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("input index 1 is out of range"));
    }
}
