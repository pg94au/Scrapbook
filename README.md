# Scrapbook

[![NuGet](https://img.shields.io/nuget/v/Scrapbook.svg)](https://www.nuget.org/packages/Scrapbook/)

A DSL-based image processing library for .NET. Write simple scripts to create, compose, crop, fill, flip, paste, resize, rotate, and reverse images using a readable scripting language.


## Installation

```
dotnet add package Scrapbook
```

## Usage

```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Scrapbook;

// Load input images
using var source1 = await Image.LoadAsync<Rgba32>("photo1.png");
using var source1 = await Image.LoadAsync<Rgba32>("photo2.png");

var parser = new ScrapbookParser();

// Call the parser to execute a script, providing the input images that can be referenced in the script
var outputs = parser.Parse("""
    # Assign input images to variables
    source1 = input 0
    source2 = input 1

    # Flip the first image horizontally and add it to the output 
    flipped = flip source1 horizontal
    output flipped
    
    # Copy a portion of the second image, rotate it, and add it to the output
    portion = copy source2 50,50 100,100
    rotatedPortion = rotate portion 90
    output rotatedPortion
    """, new[] { source1, source2 });

// Save the output images
await outputs[0].SaveAsync("result1.png");
await outputs[1].SaveAsync("result2.png");
```

## Script Commands

| Command | Syntax | Description |
|---|---|---|
| `create` | `var = create <w,h> [color]` | Creates a new image with optional background color |
| `input` | `var = input <index>` | Loads an input image by index |
| `output` | `output <var>` | Adds an image to the output list |
| `copy` | `var = copy <source> <x,y> <w,h>` | Copies a rectangular region from an image |
| `fill` | `var = fill <source> <x,y> <w,h> <color>` | Fills a rectangular region of an image with a color |
| `flip` | `var = flip <source> horizontal\|vertical` | Flips an image along an axis |
| `paste` | `var = paste <source> <target> <x,y>` | Pastes one image onto another at the given position |
| `rotate` | `var = rotate <source> <degrees>` | Rotates an image by the given number of degrees |
| `reverse` | `var = reverse <source>` | Inverts the colours of an image |
| `resize` | `var = resize <source> <w,h>` | Resizes an image to the given dimensions |

## Requirements

- .NET 10 or later

## License

MIT
