using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NUnit.Framework;

namespace Scrapbook.Tests;

[TestFixture]
public class ResizeCommandTests
{
    // Spec 1: Can resize an image and export it
    [Test]
    public void Resize_WhenResizingImageToSmallerDimensions_OutputHasExpectedDimensions()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            resized = resize first 100,100
            output resized
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(100));
        Assert.That(actual.Height, Is.EqualTo(100));
    }

    // Spec 2: Can resize an image to shrink it horizontally and export it
    [Test]
    public void Resize_WhenShrinkingHorizontally_OutputHasReducedWidthAndOriginalHeight()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            resized = resize first 50,200
            output resized
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(50));
        Assert.That(actual.Height, Is.EqualTo(200));
    }

    // Spec 3: Can resize an image to shrink it vertically and export it
    [Test]
    public void Resize_WhenShrinkingVertically_OutputHasOriginalWidthAndReducedHeight()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            resized = resize first 200,50
            output resized
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(200));
        Assert.That(actual.Height, Is.EqualTo(50));
    }

    // Spec 4: Can resize an image to stretch it horizontally and export it
    [Test]
    public void Resize_WhenStretchingHorizontally_OutputHasIncreasedWidthAndOriginalHeight()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            resized = resize first 300,200
            output resized
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(300));
        Assert.That(actual.Height, Is.EqualTo(200));
    }

    // Spec 5: Can resize an image to stretch it vertically and export it
    [Test]
    public void Resize_WhenStretchingVertically_OutputHasOriginalWidthAndIncreasedHeight()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        var outputs = parser.Parse("""
            first = input 0
            resized = resize first 200,300
            output resized
            """, [input]);

        Assert.That(outputs, Has.Count.EqualTo(1));
        using var actual = outputs[0];

        Assert.That(actual.Width, Is.EqualTo(200));
        Assert.That(actual.Height, Is.EqualTo(300));
    }

    // Spec 6: Cannot resize to zero dimensions
    [Test]
    public void Resize_WhenBothDimensionsAreZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 0,0
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenWidthIsZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 0,100
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenHeightIsZero_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 100,0
            output resized
            """, [input]));
    }

    // Spec 7: Cannot resize to negative dimensions
    [Test]
    public void Resize_WhenWidthIsNegative_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first -100,100
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenHeightIsNegative_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 100,-100
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenBothDimensionsAreNegative_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first -100,-100
            output resized
            """, [input]));
    }

    // Spec 8: Cannot resize referencing an image that does not exist
    [Test]
    public void Resize_WhenSourceVariableIsNotDefined_ThrowsInvalidOperationException()
    {
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            resized = resize unassigned 100,100
            output resized
            """, []));
    }

    // Spec 9: Cannot resize with non-integer dimensions
    [Test]
    public void Resize_WhenWidthIsDecimal_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 100.5,100
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenHeightIsDecimal_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first 100,100.5
            output resized
            """, [input]));
    }

    [Test]
    public void Resize_WhenDimensionsAreWords_ThrowsInvalidOperationException()
    {
        using var input = ScrapbookTestImageFactory.CreatePatternImage(200, 200);
        var parser = new ScrapbookParser();

        Assert.Throws<InvalidOperationException>(() => parser.Parse("""
            first = input 0
            resized = resize first five,six
            output resized
            """, [input]));
    }
}
