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

        // Hex color terminal: matches #RRGGBB (6 hex digits) — must be defined before comment terminal
        var hexColor = new RegexBasedTerminal("hexColor", @"#[0-9A-Fa-f]{6}");

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
        var create = new NonTerminal("create");
        var createColor = new NonTerminal("createColor");
        var paste = new NonTerminal("paste");
        var point = new NonTerminal("point");
        var size = new NonTerminal("size");

        // Argument structures
        point.Rule = number + comma + number;
        size.Rule = number + comma + number;

        // Color argument: named color (identifier) or hex color
        createColor.Rule = identifier | hexColor;

        // Command structures
        input.Rule = ToTerm("input") + number;
        copy.Rule = ToTerm("copy") + identifier + point + size;
        rotate.Rule = ToTerm("rotate") + identifier + number;
        flip.Rule = ToTerm("flip") + identifier + identifier;
        reverse.Rule = ToTerm("reverse") + identifier;
        create.Rule = ToTerm("create") + size
                    | ToTerm("create") + size + createColor;
        paste.Rule = ToTerm("paste") + identifier + identifier + point;

        // Expression composition
        expression.Rule = input | copy | rotate | flip | reverse | create | paste;

        // Statement structures
        assignment.Rule = identifier + ToTerm("=") + expression;
        output.Rule = ToTerm("output") + identifier;
        statement.Rule = assignment | output;
        program.Rule = MakePlusRule(program, statement);

        // Comment handling — "# " (hash + space) so that #RRGGBB hex colors are not treated as comments
        var scriptComment = new CommentTerminal("scriptComment", "# ", "\n", "\r\n");
        NonGrammarTerminals.Add(scriptComment);

        Root = program;

        MarkPunctuation("=", ",");
    }
}
