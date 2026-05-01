using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Scrapbook.App;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length < 1)
        {
            await Console.Error.WriteLineAsync("Usage: Scrapbook.App <script-file> [image-file ...]");
            return 1;
        }

        var scriptFile = args[0];
        var imageFiles = args[1..];

        if (!File.Exists(scriptFile))
        {
            await Console.Error.WriteLineAsync($"Script file not found: {scriptFile}");
            return 1;
        }

        var script = await File.ReadAllTextAsync(scriptFile);

        var inputImages = new List<Image<Rgba32>>();
        try
        {
            foreach (var imageFile in imageFiles)
            {
                if (!File.Exists(imageFile))
                {
                    await Console.Error.WriteLineAsync($"Image file not found: {imageFile}");
                    return 1;
                }
                inputImages.Add(await Image.LoadAsync<Rgba32>(imageFile));
            }

            IReadOnlyList<Image<Rgba32>> outputImages;
            try
            {
                outputImages = new Scrapbook.ScrapbookParser().Parse(script, inputImages);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Script error: {ex.Message}");
                return 1;
            }

            for (var i = 0; i < outputImages.Count; i++)
            {
                var outputFile = $"output{i}.png";
                await outputImages[i].SaveAsPngAsync(outputFile);
                Console.WriteLine($"Wrote {outputFile}");
            }
        }
        finally
        {
            foreach (var image in inputImages)
            {
                image.Dispose();
            }
        }

        return 0;
    }
}
