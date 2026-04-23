# Paste use cases:

## 1. Paste smaller image into the top corner of a larger image.

Create two different fixed sample input images (where both dimensions of the first are smaller than those of the second)
that are provided as the input images.

```
first = input 0
second - input 1
pasted = paste first second 0,0
output pasted
```

Assert that the output image is the same size as the second image, and that the top left content of this image is identical
to the first image provided.  The remainder of the output image must be identical to the content that was in the second
image provided.


## 2. Paste image of identical size into the top corner of another image.

Create two different fixed sample input images of the same size that are provided as the input images.

```
first = input 0
second = input 1
pasted = paste first second 0,0
output pasted
```

Assert that the output image is identical to the first image.


## 3. Paste source image that is wider than the target image into the top left corner of the target.

Create two different images where the first is 100x20 and the second is 20x100 and provide these as input images.

```
first = input 0
second = input 1
pasted = paste first second 0,0
output pasted
```

Assert that the output image bounded by 0,0 and 20,0 is identical to this portion of the first input image.
The remainder of the output image must be identical to the second input image.


## 4. Paste source image that is taller than the target image into the top left corner of the target.

Create two different images where the first is 20x100 and the second is 100x20 and provide these as input images.

```
first = input 0
second = input 1
pasted = paste first second 0,0
output pasted
```

Assert that the output image bounded by 0,0 and 20,0 is identical to this portion of the first input image.
The remainder of the output image must be identical to the second input image.


## 5. Paste source image into an offset position of the target.

Create two different images that are provided as the input images.

```
first = input 0
second = input 1
pasted = paste first second 10,10
output = pasted
```

Assert that the second image contents are contained in the output image, offset 10,10 from the top left corner.
The remaining of the output image must be identical to the second image.


## 6. Paste image into a negative offset position of the target.

Create two different images, both with dimensions 100x100 that are provided as inputs.

```
first = input 0
second = input 1
pasted = paste first second -20,-20
output = pasted
```

Assert that the output image, from the top left corner, contains the content of the first image, offset at position
20,20 (so the left 20 pixel columns and top 20 pixel rows are cut off).


## 7. Paste non-existing image into another image.

```
second = create 10,10
pasted = paste first second 0,0
output pasted
```

Assert that an error is thrown indicating that the target image does not exist, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 8. Paste image into a non-existing image.

```
first = create 10,10
pasted = paste first second 0,0
output pasted
```

Assert that an error is thrown indicating that the source image does not exist, and that the library provides
appropriate feedback for this error.  The library should not attempt to copy any portion of the image in this case,
and the output should not contain any pixels.


## 9. Paste image into invalid coordinates of another image.

```
first = create 10,10
second = create 10,10
pasted = paste first second hello
output pasted
```

Assert that an error is thrown indicating that the script syntax is malformed, and that the library provides
appropriate feedback for this error.  The library should not attempt to paste any portion of the image in this case,
and the output should not contain any pixels.
