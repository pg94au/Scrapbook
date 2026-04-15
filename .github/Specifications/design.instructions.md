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

The commands that are to be included in this DSL include:

Copy:
- This command will copy a portion of an input image defined by a bounding box to a new variable.
- The bounding box will be defined by the top-left corner point and the width and height of the box.
- For example, "copy a 10,20 25,25" will copy a portion of image "a" defined by the bounding box with
  top-left corner at (10,20) and width-height of (25,25) to a new variable.
- This command will return an image that is the same as the portion of the input image defined by the bounding box.
- The returned image can be assigned to a new variable.
- Example:  address = copy form 10,20 25,25

Rotate:
- This command will rotate an image by a specified angle and assign the result to a new variable.
- Angles are expressed in degrees and can be positive (for clockwise rotation) or negative (for counterclockwise rotation).
- Example: rotatedImage = rotate originalImage 90

Flip:
- This command will flip an image either horizontally or vertically and assign the result to a new variable.
- Example: flippedImage = flip originalImage horizontal
- Example: flippedImage = flip originalImage vertical

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
