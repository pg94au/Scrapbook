# Output use cases:

## 1. Can output the same image that was provided as input

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
output first
```

Assert that the output image is identical to the image that was provided as input.


## 2. Can output multiple images that were provided as input

Create two fixed sample input images that are provided as the list of input images.

```
first = input 0
second = input 1
output first
output second
```

Assert that the output images exactly match the ones that were provided as inputs.


## 3. Can repeatedly output the same image

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
output first
output first
```

Assert that both output images are identical to the image that was provided as input.


## 4. Cannot output an image that has not been assigned to a variable

Create a fixed sample input image that is provided as the single input image.

```
output first  # 'first' has not been assigned to any variable, so this should result in an error.
```

Assert that an error is thrown indicating that the variable 'first' is not defined, and that the
library provides appropriate feedback for this error.  The library should not attempt to output
any image in this case, and the output should not contain any pixels.


## 5. Can output a modified image

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
modified = rotate first 90  # Rotate the input image by 90 degrees to create a modified image.
output modified
```

Assert that the output image is a correctly rotated version of the input image, and that it
does not match the original input image.
