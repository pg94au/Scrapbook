# Scrapbook

A DSL-based image processing library for .NET. Write simple scripts to compose, crop, flip, rotate, and reverse images using a readable scripting language.

## Installation

```
dotnet add package Scrapbook
```

## Usage

```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Scrapbook;

using var source = await Image.LoadAsync<Rgba32>("photo.png");

var parser = new ScrapbookParser();
var outputs = parser.Parse("""
    source = input 0
    flipped = flip source horizontal
    output flipped
    """, new[] { source });

await outputs[0].SaveAsync("result.png");
```

## Script Commands

| Command | Syntax | Description |
|---|---|---|
| `input` | `var = input <index>` | Loads an input image by index |
| `output` | `output <var>` | Adds an image to the output list |
| `copy` | `var = copy <source> <x,y> <w,h>` | Copies a rectangular region from an image |
| `flip` | `var = flip <source> horizontal\|vertical` | Flips an image along an axis |
| `rotate` | `var = rotate <source> <degrees>` | Rotates an image by the given number of degrees |
| `reverse` | `var = reverse <source>` | Inverts the colours of an image |

## Requirements

- .NET 10 or later

## License

MIT
