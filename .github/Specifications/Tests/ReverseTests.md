# Reverse use cases:

## 1. Can reverse an image and export it

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
reversed = reverse first
output reversed
```

Assert that the output image is the reversed version of the image that was provided as input.


## 2. Reversing an image twice returns the original image

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
reversed = reverse first
reversed_twice = reverse reversed
output reversed_twice
```

Assert that the output image is identical to the image that was provided as input.


## 3. Reversing an image specifying an non-existent image variable should fail

```
reversed = reverse non_existent_image
output reversed
```

Assert that the operation fails with an appropriate error message indicating that the specified
image variable does not exist.
