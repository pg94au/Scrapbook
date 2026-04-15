# Input use cases:

## 1. Can input only image and export it

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
output first
```

Assert that the output image is identical to the image that was provided as input.


## 2. Can input multiple images and reference them by index

Create two fixed sample input images that are provided as the list of input images.

```
first = input 0
second = input 1
output first
output second
```

Assert that the output images exactly match the ones that were provided as inputs.


## 3. Referencing an input index that does not exist

Create a fixed sample input image that is provided as the single input image.

```
missing = input 1  # There is only one input image at index 0, so index 1 does not exist.
output missing
```

Assert that an error is thrown indicating that the input index is invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to access any input image in this case,
and the output should not contain any pixels.
