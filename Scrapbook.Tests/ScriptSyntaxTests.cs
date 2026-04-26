using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ScriptSyntaxTests
{
    [Test]
    public void Parse_WhenScriptHasMalformedSyntax_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse("a input 0", new[] { input }));

        Assert.That(exception!.Message, Does.Match(@"Line \d+"));
    }
}
