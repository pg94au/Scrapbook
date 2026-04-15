# Flip use cases:

## 1. Flip an image horizontally

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
flipped = flip source horizontal
output flipped
```

Assert that the output image is a horizontally flipped version of the input image, meaning that the
left and right sides of the image are swapped while the top and bottom remain unchanged.


## 2. Flip an image vertically

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
flipped = flip source vertical
output flipped
```

Assert that the output image is a vertically flipped version of the input image, meaning that the
top and bottom sides of the image are swapped while the left and right remain unchanged.


## 3. Flip an image with an invalid direction argument

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
flipped = flip source diagonal  # "diagonal" is not a valid direction argument for the flip command.
output flipped
```

Assert that an error is thrown indicating that the direction argument is invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to flip the image in this case,
and the output should not contain any pixels.


## 4. Flip an image variable that has not been assigned a value

Create a fixed sample input image that is provided as the single input image.

```
flipped = flip unassignedImage horizontal  # "unassignedImage" has not been assigned a value in the script.
output flipped
```

Assert that an error is thrown indicating that the input index is invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to flip any image in this case,
and the output should not contain any pixels.


## 5. Flip without providing the required arguments

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
flipped = flip source  # Missing the required direction argument for the flip command.
output flipped
```

Assert that an error is thrown indicating that the script syntax is malformed, and that the library provides
appropriate feedback for this error.  The library should not attempt to flip any image in this case,
and the output should not contain any pixels.
