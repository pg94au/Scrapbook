# Resize use cases:

## 1. Can resize an image and export it

Create a fixed sample input image of dimensions 200x200 that is provided as the single input image.

```
first = input 0
resized = resize first 100,100
output resized
```

Assert that the output image is the resized version of the image that was provided as input.


## 2. Can resize an image to shrink it horizontally and export it

Create a fixed sample input image of dimensions 200x200 that is provided as the single input image.

```
first = input 0
resized = resize first 50,200
output resized
```

Assert that the output image is the resized version of the image that was provided as input,
with the width reduced to 50 and the height remaining at 200.


## 3. Can resize an image to shrink it vertically and export it

Create a fixed sample input image of dimensions 200x200 that is provided as the single input image.

```
first = input 0
resized = resize first 200,50
output resized
```

Assert that the output image is the resized version of the image that was provided as input,
with the width remaining at 200 and the height reduced to 50.


## 4. Can resize and image to stretch it horizontally and export it

Create a fixed sample input image of dimensions 200x200 that is provided as the single input image.

```
first = input 0
resized = resize first 300,200
output resized
```

Assert that the output image is the resized version of the image that was provided as input,
with the width increased to 300 and the height remaining at 200.


## 5. Can resize and image to stretch it vertically and export it

Create a fixed sample input image of dimensions 200x200 that is provided as the single input image.

```
first = input 0
resized = resize first 200,300
output resized
```

Assert that the output image is the resized version of the image that was provided as input,
with the width remaining at 200 and the height increased to 300.


## 6. Cannot resize to zero dimensions

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
resized = resize first 0,0
output resized
```

```
first = input 0
resized = resize first 0,100
output resized
```

```
first = input 0
resized = resize first 100,0
output resized
```

Assert for each of the scripts above that an error is thrown indicating that the dimensions cannot be zero.


## 7. Cannot resize to negative dimensions

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
resized = resize first -100,100
output resized
```

```
first = input 0
resized = resize first 100,-100
output resized
```

```
first = input 0
resized = resize first -100,-100
output resized
```

Assert for each of the scripts above that an error is thrown indicating that the dimensions cannot be negative.


## 8. Cannot resize referencing an image that does not exist

```
resized = resize unassigned 100,100
output resized
```

Assert that an error is thrown indicating that the image being resized does not exist.


## 9. Cannot resize with non-integer dimensions

Create a fixed sample input image that is provided as the single input image.

```
first = input 0
resized = resize first 100.5,100
output resized
```

```
first = input 0
resized = resize first 100,100.5
output resized
```

```
first = input 0
resized = resize first five,six
output resized
```

Assert for each of the scripts above that an error is thrown indicating that the dimensions must be integers.
