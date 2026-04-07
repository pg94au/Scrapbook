# Copy use cases:

## 1. Copy single pixel from input image

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
pixel = copy source 1,1 1,1
output pixel
```

Assert that the output image contains a single pixel with the same color as the pixel at coordinates (1,1) in the input image.


## 2. Copy a portion of the input image defined by a bounding box

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
portion = copy source 10,20 25,25
output portion
```

Assert that the output image contains the same pixel values as the corresponding region in the input image.


## 3. Copy a portion of the input image that extends beyond the bounds of the image

Create a fixed sample 100x100 input image that is provided as the single input image.

```
source = input 0
portion = copy source 90,90 20,20  # This bounding box extends beyond the bounds of the input image.
output portion
```

Assert that an error is thrown indicating that the copy bounds are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


4. Copy from an image variable that has not been assigned a value

```
portion = copy unassignedImage 10,20 25,25  # "unassignedImage" has not been assigned a value in the script.
output portion
```

Assert that an error is thrown indicating that the input index is invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 5. Copy without providing the required arguments

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
portion = copy source # Missing the required arguments for the copy command.
output portion
```

Assert that an error is thrown indicating that the script syntax is malformed, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 6. Copy with top corner coordinates that are outside the bounds of the input image

Create a fixed 100x100 sample input image that is provided as the single input image.

```
source = input 0
portion = copy source 150,150 10,10  # The top corner coordinates (150,150) are outside the bounds of the input image.
output portion
```

Assert that an error is thrown indicating that the copy bounds are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 7. Copy with unparsable arguments

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
portion = copy source ten,twenty 25,25  # The arguments "ten" and "twenty" are not parsable as integers.
output portion
```

Assert that an error is thrown indicating that the script syntax is malformed, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 8. Copy with zero width and height

Create a fixed sample input image that is provided as the single input image.

```
source = input 0
portion = copy source 10,20 0,0  # The width and height of the bounding box are zero.
output portion
```

Assert that an error is thrown indicating that the script syntax is malformed, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.
