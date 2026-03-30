using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ErrorHandlingBehaviorTests
{
    [Test]
    public void Parse_WhenScriptReferencesUnknownVariable_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            parser.Parse("output missing", new[] { input }));

        Assert.That(exception!.Message, Does.Contain("missing"));
    }

    [Test]
    public void Parse_WhenScriptUsesInvalidInputIndex_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            parser.Parse("""
                source = input 3
                output source
                """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("input index 3 is out of range"));
    }

    [Test]
    public void Parse_WhenScriptUsesInvalidCopyBounds_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            parser.Parse("""
                source = input 0
                region = copy source 1,1 2,2
                output region
                """, new[] { input }));

        Assert.That(exception!.Message, Does.Contain("copy bounds exceed source image dimensions"));
    }

    [Test]
    public void Parse_WhenScriptHasMalformedSyntax_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            parser.Parse("a input 0", new[] { input }));

        Assert.That(exception!.Message, Does.Contain("Line"));
    }
}
