using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text.RegularExpressions;
using Irony.Parsing;
using Scrapbook.Execution;
using Scrapbook.Parsing;

namespace Scrapbook;

public sealed class ScrapbookParser
{
    // Matches a comment starting with '#', including inline comments.
    private static readonly Regex CommentPattern = new(@"#.*$", RegexOptions.Compiled);

    private sealed record PreprocessResult(string Script, IReadOnlyList<int> ProcessedToOriginalLineNumbers);

    /// <summary>
    /// Preprocesses the script by stripping comments and blank lines.
    /// </summary>
    private static PreprocessResult PreprocessScript(string script)
    {
        var lines = script.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var processedLines = new List<string>();
        var lineMap = new List<int>();

        for (var i = 0; i < lines.Length; i++)
        {
            var processedLine = CommentPattern.Replace(lines[i], string.Empty).TrimEnd();
            if (string.IsNullOrWhiteSpace(processedLine))
            {
                continue;
            }

            processedLines.Add(processedLine);
            lineMap.Add(i + 1);
        }

        return new PreprocessResult(string.Join('\n', processedLines), lineMap);
    }

    /// <summary>
    /// Parses and executes a scrapbook script against the supplied input images.
    /// </summary>
    /// <param name="script">The script to parse and execute.</param>
    /// <param name="inputImages">Input images available to the script through zero-based indexes.</param>
    /// <returns>Output images in the same order as encountered <c>output</c> statements.</returns>
    public IReadOnlyList<Image<Rgba32>> Parse(string script, IReadOnlyList<Image<Rgba32>> inputImages)
    {
        if (string.IsNullOrWhiteSpace(script))
        {
            throw new ArgumentException("Script is required.", nameof(script));
        }

        ArgumentNullException.ThrowIfNull(inputImages);

        var preprocessResult = PreprocessScript(script);

        var parser = new Parser(new ScrapbookGrammar());
        var parseTree = parser.Parse(preprocessResult.Script);

        if (parseTree.HasErrors())
        {
            var firstError = parseTree.ParserMessages[0];
            var processedLineNumber = firstError.Location.Line + 1;
            var originalLineNumber = processedLineNumber >= 1 && processedLineNumber <= preprocessResult.ProcessedToOriginalLineNumbers.Count
                ? preprocessResult.ProcessedToOriginalLineNumbers[processedLineNumber - 1]
                : processedLineNumber;

            throw new InvalidOperationException($"Line {originalLineNumber}: {firstError.Message}");
        }

        var commandList = new ScriptAstBuilder().Build(parseTree);
        return new ScriptExecutor().Execute(commandList, inputImages);
    }
}
