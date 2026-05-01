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

    [Test]
    public void Parse_WhenScriptHasBlankLines_IgnoresBlankLinesAndProducesCorrectOutput()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """

            first = input 0

            output first

            """;

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        ScrapbookTestImageFactory.AssertImagesEqual(input, outputs[0]);
    }

    [Test]
    public void Parse_WhenScriptHasWhitespaceOnlyLines_IgnoresThoseLinesAndProducesCorrectOutput()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = "    \n  first = input 0  \n    \n    output first    \n";

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        ScrapbookTestImageFactory.AssertImagesEqual(input, outputs[0]);
    }

    [Test]
    public void Parse_WhenScriptHasCommentLines_IgnoresCommentsAndProducesCorrectOutput()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """
            # This is a comment line that should be ignored by the parser.
            first = input 0  # This is an inline comment that should also be ignored.
            output first  # Another inline comment.
              # A comment line with leading whitespace at the end of the script.
                # Another comment line with leading whitespace that should be ignored.
            """;

        var outputs = parser.Parse(script, new[] { input });

        Assert.That(outputs, Has.Count.EqualTo(1));
        ScrapbookTestImageFactory.AssertImagesEqual(input, outputs[0]);
    }

    [Test]
    public void Parse_WhenMalformedSyntaxAppearsAfterBlankAndCommentLines_ReportsOriginalScriptLineNumber()
    {
        using var input = ScrapbookTestImageFactory.CreateSampleImage();
        var parser = new ScrapbookParser();

        var script = """

            # comment line ignored

            first = input 0
            bad input 0
            output first
            """;

        var exception = Assert.Throws<InvalidOperationException>(() => parser.Parse(script, new[] { input }));

        Assert.That(exception!.Message, Does.StartWith("Line 5:"));
    }
}
