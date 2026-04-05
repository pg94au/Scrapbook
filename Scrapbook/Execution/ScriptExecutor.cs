using System.Drawing;
using System.Drawing.Drawing2D;
using Scrapbook.Model;

namespace Scrapbook.Execution;

internal sealed class ScriptExecutor
{
    public IReadOnlyList<Image> Execute(IReadOnlyList<ScriptCommand> commands, IReadOnlyList<Image> inputImages)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(inputImages);

        var variables = new Dictionary<string, Bitmap>(StringComparer.Ordinal);
        var outputs = new List<Image>();

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
                        outputs.Add(new Bitmap(ResolveVariable(variables, output.VariableName, output.LineNumber)));
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

    private static Bitmap EvaluateExpression(
        ScriptExpression expression,
        IReadOnlyList<Image> inputImages,
        IReadOnlyDictionary<string, Bitmap> variables,
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

    private static Bitmap EvaluateInput(InputExpression expression, IReadOnlyList<Image> inputImages, int lineNumber)
    {
        if (expression.InputIndex < 0 || expression.InputIndex >= inputImages.Count)
        {
            throw new InvalidOperationException($"Line {lineNumber}: input index {expression.InputIndex} is out of range.");
        }

        return new Bitmap(inputImages[expression.InputIndex]);
    }

    private static Bitmap EvaluateCopy(CopyExpression expression, IReadOnlyDictionary<string, Bitmap> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);

        if (expression.RegionSize.Width <= 0 || expression.RegionSize.Height <= 0)
        {
            throw new InvalidOperationException($"Line {lineNumber}: copy size must be positive.");
        }

        var rectangle = new Rectangle(expression.TopLeft, expression.RegionSize);

        if (rectangle.Left < 0 || rectangle.Top < 0 || rectangle.Right > source.Width || rectangle.Bottom > source.Height)
        {
            throw new InvalidOperationException($"Line {lineNumber}: copy bounds exceed source image dimensions.");
        }

        return source.Clone(rectangle, source.PixelFormat);
    }

    private static Bitmap EvaluateRotate(RotateExpression expression, IReadOnlyDictionary<string, Bitmap> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);
        return RotateImage(source, expression.Angle);
    }

    private static Bitmap EvaluateFlip(FlipExpression expression, IReadOnlyDictionary<string, Bitmap> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);
        var result = new Bitmap(source);

        result.RotateFlip(expression.Direction switch
        {
            FlipDirection.Horizontal => RotateFlipType.RotateNoneFlipX,
            FlipDirection.Vertical => RotateFlipType.RotateNoneFlipY,
            _ => throw new InvalidOperationException($"Line {lineNumber}: invalid flip direction.")
        });

        return result;
    }

    private static Bitmap EvaluateReverse(ReverseExpression expression, IReadOnlyDictionary<string, Bitmap> variables, int lineNumber)
    {
        var source = ResolveVariable(variables, expression.SourceVariable, lineNumber);
        var result = new Bitmap(source.Width, source.Height, source.PixelFormat);

        for (var y = 0; y < source.Height; y++)
        {
            for (var x = 0; x < source.Width; x++)
            {
                var pixel = source.GetPixel(x, y);
                result.SetPixel(x, y, Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
            }
        }

        return result;
    }

    private static Bitmap RotateImage(Bitmap source, float angle)
    {
        var normalized = ((angle % 360f) + 360f) % 360f;
        if (Math.Abs(normalized % 90f) < 0.0001f)
        {
            var rightAngle = new Bitmap(source);
            var quarterTurns = ((int)Math.Round(normalized / 90f, MidpointRounding.AwayFromZero) % 4 + 4) % 4;
            rightAngle.RotateFlip(quarterTurns switch
            {
                0 => RotateFlipType.RotateNoneFlipNone,
                1 => RotateFlipType.Rotate90FlipNone,
                2 => RotateFlipType.Rotate180FlipNone,
                _ => RotateFlipType.Rotate270FlipNone
            });

            return rightAngle;
        }

        var radians = Math.Abs(angle) * Math.PI / 180d;
        var cos = Math.Abs(Math.Cos(radians));
        var sin = Math.Abs(Math.Sin(radians));
        var width = (int)Math.Ceiling((source.Width * cos) + (source.Height * sin));
        var height = (int)Math.Ceiling((source.Width * sin) + (source.Height * cos));

        var result = new Bitmap(width, height);
        result.SetResolution(source.HorizontalResolution, source.VerticalResolution);

        using var graphics = Graphics.FromImage(result);
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.TranslateTransform(width / 2f, height / 2f);
        graphics.RotateTransform(angle);
        graphics.TranslateTransform(-source.Width / 2f, -source.Height / 2f);
        graphics.DrawImage(source, new PointF(0, 0));

        return result;
    }

    private static Bitmap ResolveVariable(IReadOnlyDictionary<string, Bitmap> variables, string variableName, int lineNumber)
    {
        if (!variables.TryGetValue(variableName, out var bitmap))
        {
            throw new InvalidOperationException($"Line {lineNumber}: variable '{variableName}' was not defined.");
        }

        return bitmap;
    }

    private static void AssignVariable(IDictionary<string, Bitmap> variables, string variableName, Bitmap bitmap)
    {
        if (variables.TryGetValue(variableName, out var existing))
        {
            existing.Dispose();
        }

        variables[variableName] = bitmap;
    }
}
