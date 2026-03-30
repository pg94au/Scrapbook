using System.Drawing;
using Irony.Parsing;
using Scrapbook.Execution;
using Scrapbook.Parsing;

namespace Scrapbook;

public sealed class ScrapbookParser
{
    /// <summary>
    /// Parses and executes a scrapbook script against the supplied input images.
    /// </summary>
    /// <param name="script">The script to parse and execute.</param>
    /// <param name="inputImages">Input images available to the script through zero-based indexes.</param>
    /// <returns>Output images in the same order as encountered <c>output</c> statements.</returns>
    public IReadOnlyList<Image> Parse(string script, IReadOnlyList<Image> inputImages)
    {
        if (string.IsNullOrWhiteSpace(script))
        {
            throw new ArgumentException("Script is required.", nameof(script));
        }

        ArgumentNullException.ThrowIfNull(inputImages);

        var parser = new Parser(new ScrapbookGrammar());
        var parseTree = parser.Parse(script);

        if (parseTree.HasErrors())
        {
            var firstError = parseTree.ParserMessages[0];
            var lineNumber = firstError.Location.Line + 1;
            throw new InvalidOperationException($"Line {lineNumber}: {firstError.Message}");
        }

        var commandList = new ScriptAstBuilder().Build(parseTree);
        return new ScriptExecutor().Execute(commandList, inputImages);
    }
}
