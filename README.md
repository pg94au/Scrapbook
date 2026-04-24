# Scrapbook

[![NuGet](https://img.shields.io/nuget/v/Scrapbook.svg)](https://www.nuget.org/packages/Scrapbook/)

A DSL-based image processing library for .NET. Write simple scripts to create, compose, crop, flip, paste, resize, rotate, and reverse images using a readable scripting language.


## Installation

```
dotnet add package Scrapbook
```

## Usage

```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Scrapbook;

using var source1 = await Image.LoadAsync<Rgba32>("photo1.png");
using var source1 = await Image.LoadAsync<Rgba32>("photo2.png");

var parser = new ScrapbookParser();
var outputs = parser.Parse("""
    source1 = input 0
    source2 = input 1
 
    flipped = flip source1 horizontal
    output flipped
    
    portion = copy source2 50,50 100,100
    rotatedPortion = rotate portion 90
    output rotatedPortion
    """, new[] { source1, source2 });

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
| `flip` | `var = flip <source> horizontal\|vertical` | Flips an image along an axis |
| `paste` | `var = paste <source> <target> <x,y>` | Pastes one image onto another at the given position |
| `rotate` | `var = rotate <source> <degrees>` | Rotates an image by the given number of degrees |
| `reverse` | `var = reverse <source>` | Inverts the colours of an image |
| `resize` | `var = resize <source> <w,h>` | Resizes an image to the given dimensions |

## Requirements

- .NET 10 or later

## License

MIT
