using Irony.Parsing;

namespace Scrapbook.Parsing;

internal sealed class ScrapbookGrammar : Grammar
{
    public ScrapbookGrammar()
    {
        // Core lexical tokens
        var identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
        var number = new NumberLiteral("number", NumberOptions.AllowSign);
        var comma = ToTerm(",");

        // Keyword tokens
        var horizontal = ToTerm("horizontal");
        var vertical = ToTerm("vertical");

        // Grammar nodes
        var program = new NonTerminal("program");
        var statement = new NonTerminal("statement");
        var assignment = new NonTerminal("assignment");
        var output = new NonTerminal("output");
        var expression = new NonTerminal("expression");
        var input = new NonTerminal("input");
        var copy = new NonTerminal("copy");
        var rotate = new NonTerminal("rotate");
        var flip = new NonTerminal("flip");
        var reverse = new NonTerminal("reverse");
        var point = new NonTerminal("point");
        var size = new NonTerminal("size");
        var flipDirection = new NonTerminal("flipDirection");

        // Argument structures
        point.Rule = number + comma + number;
        size.Rule = number + comma + number;
        flipDirection.Rule = horizontal | vertical;

        // Command structures
        input.Rule = ToTerm("input") + number;
        copy.Rule = ToTerm("copy") + identifier + point + size;
        rotate.Rule = ToTerm("rotate") + identifier + number;
        flip.Rule = ToTerm("flip") + flipDirection + identifier;
        reverse.Rule = ToTerm("reverse") + identifier;

        // Expression composition
        expression.Rule = input | copy | rotate | flip | reverse;

        // Statement structures
        assignment.Rule = identifier + ToTerm("=") + expression;
        output.Rule = ToTerm("output") + identifier;
        statement.Rule = assignment | output;
        program.Rule = MakePlusRule(program, statement);

        // Comment handling
        var scriptComment = new CommentTerminal("scriptComment", "#", "\n", "\r\n");
        NonGrammarTerminals.Add(scriptComment);

        Root = program;

        MarkPunctuation("=", ",");
    }
}
