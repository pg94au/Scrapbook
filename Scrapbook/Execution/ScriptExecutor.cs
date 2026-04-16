using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Scrapbook.Model;

namespace Scrapbook.Execution;

internal sealed class ScriptExecutor
{
    public IReadOnlyList<Image<Rgba32>> Execute(IReadOnlyList<ScriptCommand> commands, IReadOnlyList<Image<Rgba32>> inputImages)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(inputImages);

        var variables = new Dictionary<string, Image<Rgba32>>(StringComparer.Ordinal);
        var outputs = new List<Image<Rgba32>>();

        try
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case AssignmentCommand assignment:
                        var assigned = EvaluateExpression(assignment.Expression, inputImages, variables, assignment.LineNumber);
                        AssignVariable(variables, assignment.VariableName, assigned);
                        break;
                    case OutputCommand output:
                        outputs.Add(ResolveVariable(variables, output.VariableName, output.LineNumber).Clone());
                        break;
                    default:
                        throw new InvalidOperationException($"Line {command.LineNumber}: unsupported command type.");
                }
            }

            return outputs;
        }
        finally
        {
            foreach (var image in variables.Values)
            {
                image.Dispose();
            }
        }
    }

    private static Image<Rgba32> EvaluateExpression(
        ScriptExpression expression,
        IReadOnlyList<Image<Rgba32>> inputImages,
        IReadOnlyDictionary<string, Image<Rgba32>> variables,
        int lineNumber)
    {
        return expression switch
        {
            InputExpression input => EvaluateInput(input, inputImages, lineNumber),
            CopyExpression copy => EvaluateCopy(copy, variables, lineNumber),
            RotateExpression rotate => EvaluateRotate(rotate, variables, lineNumber),
            FlipExpression flip => EvaluateFlip(flip, variables, lineNumber),
            ReverseExpression reverse => EvaluateReverse(reverse, variables, lineNumber),
            _ => throw new InvalidOperationException($"Line {lineNumber}: unsupported expression type.")
        };
    }

    private static Image<Rgba32> EvaluateInput(InputExpression expression, IReadOnlyList<Image<Rgba32>> inputImages, int lineNumber)
    {
        if (expression.InputIndex < 0 || expression.InputIndex >= inputImages.Count)
        {
            throw new InvalidOperationException($"Line {lineNumber}: input index {expression.InputIndex} is out of range.");
        }

        return inputImages[expression.InputIndex].Clone();
    }

    private static Image<Rgba32> EvaluateCopy(CopyExpression expression, IReadOnlyDictionary<string, Image<Rgba32>> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);

        if (expression.RegionSize.Width <= 0 || expression.RegionSize.Height <= 0)
        {
            throw new InvalidOperationException($"Line {lineNumber}: copy size must be positive.");
        }

        var rectangle = new Rectangle(expression.TopLeft.X, expression.TopLeft.Y, expression.RegionSize.Width, expression.RegionSize.Height);

        if (rectangle.Left < 0 || rectangle.Top < 0 || rectangle.Right > source.Width || rectangle.Bottom > source.Height)
        {
            throw new InvalidOperationException($"Line {lineNumber}: copy bounds exceed source image dimensions.");
        }

        return source.Clone(ctx => ctx.Crop(rectangle));
    }

    private static Image<Rgba32> EvaluateRotate(RotateExpression expression, IReadOnlyDictionary<string, Image<Rgba32>> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);
        return RotateImage(source, expression.Angle);
    }

    private static Image<Rgba32> EvaluateFlip(FlipExpression expression, IReadOnlyDictionary<string, Image<Rgba32>> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);

        return source.Clone(ctx => ctx.Flip(expression.Direction switch
        {
            FlipDirection.Horizontal => FlipMode.Horizontal,
            FlipDirection.Vertical => FlipMode.Vertical,
            _ => throw new InvalidOperationException($"Line {lineNumber}: invalid flip direction.")
        }));
    }

    private static Image<Rgba32> EvaluateReverse(ReverseExpression expression, IReadOnlyDictionary<string, Image<Rgba32>> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);
        var result = source.Clone();

        result.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (var x = 0; x < row.Length; x++)
                {
                    ref var pixel = ref row[x];
                    pixel = new Rgba32(
                        (byte)(255 - pixel.R),
                        (byte)(255 - pixel.G),
                        (byte)(255 - pixel.B),
                        pixel.A);
                }
            }
        });

        return result;
    }

    private static Image<Rgba32> RotateImage(Image<Rgba32> source, float angle)
    {
        var normalized = ((angle % 360f) + 360f) % 360f;
        if (Math.Abs(normalized % 90f) < 0.0001f)
        {
            var quarterTurns = ((int)Math.Round(normalized / 90f, MidpointRounding.AwayFromZero) % 4 + 4) % 4;
            var rotateMode = quarterTurns switch
            {
                0 => RotateMode.None,
                1 => RotateMode.Rotate90,
                2 => RotateMode.Rotate180,
                _ => RotateMode.Rotate270
            };
            return source.Clone(ctx => ctx.Rotate(rotateMode));
        }

        return source.Clone(ctx => ctx.Rotate(angle));
    }

    private static Image<Rgba32> ResolveVariable(IReadOnlyDictionary<string, Image<Rgba32>> variables, string variableName, int lineNumber)
    {
        if (!variables.TryGetValue(variableName, out var image))
        {
            throw new InvalidOperationException($"Line {lineNumber}: variable '{variableName}' was not defined.");
        }

        return image;
    }

    private static void AssignVariable(IDictionary<string, Image<Rgba32>> variables, string variableName, Image<Rgba32> image)
    {
        if (variables.TryGetValue(variableName, out var existing))
        {
            existing.Dispose();
        }

        variables[variableName] = image;
    }
}
