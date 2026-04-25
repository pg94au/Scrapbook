# Fill use cases:

## 1. Fill single pixel image

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
filledPixel = fill image 1,1 1,1 red
output filledPixel
```

Assert that the output image is identical to the input image with the specified pixel at 1,1 filled with a red color.


## 2. Fill area of specified dimensions

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
filledArea = fill image 2,2 5,5 blue
output filledArea
```

Assert that the output image is identical to the input image with the specified area defined by the bounding box with
top-left corner at (2,2) and width-height of (5,5) filled with a blue color.


## 3. Cannot fill area with zero width or height

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
invalidImage = fill image 0,0 0,10 red  # Zero width
output invalidImage
```

```
image = input 0
invalidImage = fill image 0,0 10,0 red  # Zero height
output invalidImage
```

Assert that for both scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and no images should
be output.


## 4. Cannot fill area with negative dimensions

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
invalidImage = fill image -5,0 10,10 red  # Negative width
output invalidImage
```

```
image = input 0
invalidImage = fill image 0,-5 10,10 red  # Negative height
output invalidImage
```

Assert that for both scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and no images should
be output.


## 5. Cannot create an image with non-integer dimensions

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
invalidImage = fill image 0,0 5.5,10 red  # Non-integer width
output invalidImage
```

```
image = input 0
invalidImage = fill image 0,0 10,5.5 red  # Non-integer height
```

```
image = input 0
invalidImage = fill image 0,0 a,b red  # Non-numeric dimensions
output invalidImage
```

Assert that for these scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and no images should
be output.


## 6. Fill area with bounds greater than the input image dimensions

Create a fixed sample input image of dimensions (10,10) that is provided as the single input image.

```
image = input 0
filledArea = fill image 5,5 10,10 red  # The area defined by the bounding box with top-left corner at (5,5) and width-height of (10,10) extends beyond the bounds of the input image.
output filledArea
```

Assert that the output image is identical to the input image with the specified area defined by the bounding box with
top-left corner at (5,5) and width-height of (10,10) filled with a red color, but that the fill operation is correctly
applied only to the portion of the area that overlaps with the input image.  In this case, the area that is filled
should be the portion of the bounding box that overlaps with the input image, which is defined by the top-left corner
at (5,5) and width-height of (5,5).
