Structure this project so that it looks familiar to an experienced software developer.  The project will
be organized into multiple classes and namespaces to provide a clear separation of concerns and to make
it easy for developers to navigate and understand the codebase.  Note you should be using the modern slnx
Visual Studio solution file format, not the old sln format.  The name of the solution file is Scrapbook.slnx.

When working with a problem that involves parsing a domain-specific language, use the Irony library to
define the grammar of the language and to create a parser that can interpret the scripts written in that
language.  The Irony library provides a powerful and flexible way to define the syntax and semantics of
a domain-specific language, and it can be used to create a robust parser that can handle a wide range of
scripts and scenarios.  The class in which the grammar is described will be divided into commented sections
for the different components of the grammar, such as variable assignment, image transformations, and output
commands.  This will make it obvious that, for example, "rotate" is a command, whereas "point" is an argument
to a command.

The library will be built in a project named Scrapbook.

This library should be functional when used on either Windows or Linux.  Support for image processing on
both platforms can be achieved using the SixLabors.ImageSharp library, which provides cross-platform image
processing capabilities.  By utilizing this library, the Scrapbook library can perform image transformations
and manipulations on both Windows and Linux without relying on platform-specific APIs or libraries.

The commands that are to be included in this DSL include:

Copy:
- This command will copy a portion of an input image defined by a bounding box to a new variable.
- The bounding box will be defined by the top-left corner point and the width and height of the box.
- For example, "copy a 10,20 25,25" will copy a portion of image "a" defined by the bounding box with
  top-left corner at (10,20) and width-height of (25,25) to a new variable.
- This command will return an image that is the same as the portion of the input image defined by the bounding box.
- The returned image can be assigned to a new variable.
- Example:  address = copy form 10,20 25,25

Create:
- This command will create a new blank image with specified dimensions and assign it to a new variable.
- The color of the blank image can be specified as an optional argument, and if not provided, it will default to transparent.
- Colors can be specified using named colors (e.g., "white", "red", "blue") or using hexadecimal color codes
- (e.g., "#FFFFFF" for white, "#FF0000" for red).
- Color names come from the list supported by the SixLabors.ImageSharp library.
- Example: blankImage = create 200,300
- Example: blankImage = create 200,300 white
- Example: blankImage = create 200,300 #FF0000  (for red)

Fill:
- This command will fill a rectangular region of an image with a specified color and assign the result to a new variable.
- The color of the blank image can be specified as an optional argument, and if not provided, it will default to transparent.
- Colors can be specified using named colors (e.g., "white", "red", "blue") or using hexadecimal color codes
- (e.g., "#FFFFFF" for white, "#FF0000" for red).
- Color names come from the list supported by the SixLabors.ImageSharp library.
- Example: filledImage = fill originalImage 10,20 25,25 white
- Example: filledImage = fill originalImage 10,20 25,25 #FF0000 (for red)

Rotate:
- This command will rotate an image by a specified angle and assign the result to a new variable.
- Angles are expressed in degrees and can be positive (for clockwise rotation) or negative (for counterclockwise rotation).
- Example: rotatedImage = rotate originalImage 90

Flip:
- This command will flip an image either horizontally or vertically and assign the result to a new variable.
- Example: flippedImage = flip originalImage horizontal
- Example: flippedImage = flip originalImage vertical

Paste:
- This command will paste one image onto another image at a specified position and assign the result to a new variable.
- The position is specified by the top-left corner of the source image relative to the target image.
- Example: pastedImage = paste sourceImage targetImage 10,10  # Will paste "sourceImage" onto "targetImage" with the top-left corner of "sourceImage" positioned at (10,10) on "targetImage".

Resize:
- This command will stretch or shrink the specified image to the new dimensions and assign the result to a new variable.
- Example: resized = resize sourceImage 100,100 # Will resize "sourceImage" to be of dimensions 100x100 and assign the result to a new variable.

Reverse:
- This command will reverse the colors of an image and assign the result to a new variable.
- Reversing the colors of an image means that each pixel's color is replaced with its complementary color.
- For example, if a pixel is red, it will be replaced with cyan; if a pixel is green, it will be replaced
- with magenta; and if a pixel is blue, it will be replaced with yellow.  Black will become white.
- Example: reversedImage = reverse originalImage

Resize:
- This command will resize an image to specified dimensions and assign the result to a new variable.
- Example: resizedImage = resize originalImage 100,150

Output:
- This command will specify which variable's image should be returned as a final output image from the library.
- When multiple images are output, the library will return a list of output images corresponding to each "output"
  command in the order they appear in the script.
- Example: output label
