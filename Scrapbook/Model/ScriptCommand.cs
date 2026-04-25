using SixLabors.ImageSharp;

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

internal sealed record CreateExpression(int Width, int Height, string? Color) : ScriptExpression;

internal sealed record PasteExpression(string SourceVariable, string TargetVariable, Point Position) : ScriptExpression;

internal sealed record ResizeExpression(string SourceVariable, int Width, int Height) : ScriptExpression;

internal sealed record FillExpression(string SourceVariable, Point TopLeft, Size RegionSize, string Color) : ScriptExpression;
