using SixLabors.ImageSharp;
using Irony.Parsing;
using Scrapbook.Model;

namespace Scrapbook.Parsing;

internal sealed class ScriptAstBuilder
{
    public IReadOnlyList<ScriptCommand> Build(ParseTree parseTree)
    {
        ArgumentNullException.ThrowIfNull(parseTree);

        if (parseTree.Root is null)
        {
            return [];
        }

        var commands = new List<ScriptCommand>();
        foreach (var statementNode in parseTree.Root.ChildNodes)
        {
            commands.Add(BuildStatement(statementNode));
        }

        return commands;
    }

    private static ScriptCommand BuildStatement(ParseTreeNode statementNode)
    {
        var child = statementNode.ChildNodes[0];
        var lineNumber = child.Span.Location.Line + 1;

        return child.Term.Name switch
        {
            "assignment" => BuildAssignment(child, lineNumber),
            "output" => BuildOutput(child, lineNumber),
            _ => throw new InvalidOperationException($"Line {lineNumber}: unsupported statement '{child.Term.Name}'.")
        };
    }

    private static ScriptCommand BuildAssignment(ParseTreeNode assignmentNode, int lineNumber)
    {
        var variableName = assignmentNode.ChildNodes[0].Token.ValueString;
        var expressionNode = assignmentNode.ChildNodes[1];
        var expression = BuildExpression(expressionNode.ChildNodes[0], lineNumber);
        return new AssignmentCommand(variableName, expression, lineNumber);
    }

    private static ScriptCommand BuildOutput(ParseTreeNode outputNode, int lineNumber)
    {
        var variableName = outputNode.ChildNodes[1].Token.ValueString;
        return new OutputCommand(variableName, lineNumber);
    }

    private static ScriptExpression BuildExpression(ParseTreeNode node, int lineNumber)
    {
        return node.Term.Name switch
        {
            "input" => new InputExpression(ToInt(node.ChildNodes[1], lineNumber, "input index")),
            "copy" => BuildCopy(node, lineNumber),
            "rotate" => new RotateExpression(
                node.ChildNodes[1].Token.ValueString,
                ToFloat(node.ChildNodes[2], lineNumber, "angle")),
            "flip" => BuildFlip(node, lineNumber),
            "reverse" => new ReverseExpression(node.ChildNodes[1].Token.ValueString),
            _ => throw new InvalidOperationException($"Line {lineNumber}: unsupported expression '{node.Term.Name}'.")
        };
    }

    private static ScriptExpression BuildCopy(ParseTreeNode node, int lineNumber)
    {
        var source = node.ChildNodes[1].Token.ValueString;
        var topLeft = ReadPoint(node.ChildNodes[2], lineNumber, "top-left point");
        var regionSize = ReadSize(node.ChildNodes[3], lineNumber, "region size");
        return new CopyExpression(source, topLeft, regionSize);
    }

    private static ScriptExpression BuildFlip(ParseTreeNode node, int lineNumber)
    {
        var source = node.ChildNodes[1].Token.ValueString;
        var directionName = node.ChildNodes[2].Token.ValueString;

        var direction = directionName switch
        {
            "horizontal" => FlipDirection.Horizontal,
            "vertical" => FlipDirection.Vertical,
            _ => throw new InvalidOperationException($"Line {lineNumber}: invalid flip direction '{directionName}'.")
        };

        return new FlipExpression(source, direction);
    }

    private static Point ReadPoint(ParseTreeNode pointNode, int lineNumber, string label)
    {
        var x = ToInt(pointNode.ChildNodes[0], lineNumber, label);
        var y = ToInt(pointNode.ChildNodes[1], lineNumber, label);
        return new Point(x, y);
    }

    private static Size ReadSize(ParseTreeNode sizeNode, int lineNumber, string label)
    {
        var width = ToInt(sizeNode.ChildNodes[0], lineNumber, label);
        var height = ToInt(sizeNode.ChildNodes[1], lineNumber, label);
        return new Size(width, height);
    }

    private static int ToInt(ParseTreeNode node, int lineNumber, string valueName)
    {
        try
        {
            return Convert.ToInt32(node.Token.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception ex) when (ex is OverflowException or FormatException)
        {
            throw new InvalidOperationException($"Line {lineNumber}: invalid integer for {valueName}.", ex);
        }
    }

    private static float ToFloat(ParseTreeNode node, int lineNumber, string valueName)
    {
        try
        {
            return Convert.ToSingle(node.Token.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception ex) when (ex is OverflowException or FormatException)
        {
            throw new InvalidOperationException($"Line {lineNumber}: invalid number for {valueName}.", ex);
        }
    }
}
