# Rotate use cases:

## 1. Can rotate an image and export it

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated = rotate first 90
output rotated
```

Assert that the output image is the 90-degree rotated version of the image that was provided as input.


## 2. Rotating an image four times by 90 degrees returns the original image

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated_90 = rotate first 90
rotated_180 = rotate rotated_90 90
rotated_270 = rotate rotated_180 90
rotated_360 = rotate rotated_270 90
output rotated_360
```

Assert that the output image is identical to the image that was provided as input.


## 3. Rotating an image specifying a non-existent image variable should fail

```
rotated = rotate non_existent_image 90
output rotated
```

Assert that the operation fails with an appropriate error message indicating that the specified
image variable does not exist.


## 4. Rotating an image by an invalid angle should fail

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated = rotate first fred
output rotated
```

Assert that the operation fails with an appropriate error message indicating that the specified
angle is invalid and should be a numeric value representing the degrees of rotation.


## 5. Can rotate by an aribtrary angle

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated = rotate first 45
output rotated
```

Assert that the output image is the 45-degree rotated version of the image that was provided as input.
If the original image was rectangular, the output image should be larger than the original image to
accommodate the rotated corners of the image.  The additional areas of the output image that are not covered
by the rotated original image should be a transparent background.


## 6. Can rotate by a negative angle

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated = rotate first -90
output rotated
```

Assert that the output image is the -90-degree (or 270-degree) rotated version of the image that was provided as input.


## 7. Can rotate by an angle greater than 360 degrees

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
rotated = rotate first 450
output rotated
```

Assert that the output image is the 450-degree (or 90-degree) rotated version of the image that was provided as input.

