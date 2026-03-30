using System.Drawing;

namespace Scrapbook.Model;

internal abstract record ScriptCommand(int LineNumber);

internal sealed record AssignmentCommand(string VariableName, ScriptExpression Expression, int LineNumber)
    : ScriptCommand(LineNumber);

internal sealed record OutputCommand(string VariableName, int LineNumber)
    : ScriptCommand(LineNumber);

internal abstract record ScriptExpression;

internal sealed record InputExpression(int InputIndex) : ScriptExpression;

internal sealed record CopyExpression(string SourceVariable, Point TopLeft, Size RegionSize) : ScriptExpression;

internal sealed record RotateExpression(string SourceVariable, float Angle) : ScriptExpression;

internal enum FlipDirection
{
    Horizontal,
    Vertical
}

internal sealed record FlipExpression(string SourceVariable, FlipDirection Direction) : ScriptExpression;

internal sealed record ReverseExpression(string SourceVariable) : ScriptExpression;
