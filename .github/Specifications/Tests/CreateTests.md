# Create use cases:

## 1. Create single pixel image

Provide no input images.

```
pixel = create 1,1
output pixel
```

Assert that the output image contains a single pixel with a transparent color.


## 2. Create an image of specified dimensions

Provide no input images.

```
image = create 10,20
output image
```

Assert that the output image contains 10 pixels in width and 20 pixels in height, and that all pixels have a transparent color.


## 3. Cannot create an image with zero width or height

Provide no input images.

```
invalidImage = create 0,10  # Zero width
```

```
invalidImage = create 10,0  # Zero height
```

Assert that for both scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and the
output should not contain any pixels.


## 4. Cannot create an image with negative dimensions

Provide no input images.

```
invalidImage = create -5,10  # Negative width
```

```
invalidImage = create 10,-5  # Negative height
```

Assert that for both scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and the
output should not contain any pixels.


## 5. Cannot create an image with non-integer dimensions

Provide no input images.

```
invalidImage = create 10.5,20  # Non-integer width
```

```
invalidImage = create 10,20.5  # Non-integer height
```

```
invalidImage = create a,b  # Non-numeric dimensions
```

Assert that for these scripts an error is thrown indicating that the image dimensions are invalid, and that the library provides
appropriate feedback for this error.  The library should not attempt to create an image in this case, and the
output should not contain any pixels.


## 6. Create an image with a specified background color

Provide no input images.

```
coloredImage = create 10,20 red
output coloredImage
```

```
coloredImage = create 10,20 0xFF0000
output coloredImage
```

Assert that in both scripts the output image contains 10 pixels in width and 20 pixels in height, and that all pixels
have the specified red color as the background color.
