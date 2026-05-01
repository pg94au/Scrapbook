# Syntax use cases:

## 1. Blank lines are allowed and ignored in the script.

Create a fixed sample input image that is provided as the single input image.

```

first = input 0

output first

```

Assert that the output image is identical to the image that was provided as input.


## 2. Lines that contain only whitespace (or have additional whitespace) are allowed and ignored in the script.

Create a fixed sample input image that is provided as the single input image.

```
    
  first = input 0  
    
    output first    

```

Assert that the output image is identical to the image that was provided as input.


## 3. Lines that start with a `#` character are treated as comments and are ignored in the script.

Create a fixed sample input image that is provided as the single input image.

```
# This is a comment line that should be ignored by the parser.
first = input 0  # This is an inline comment that should also be ignored.
output first  # Another inline comment.
  # A comment line with leading whitespace at the end of the script.
    # Another comment line with leading whitespace that should be ignored.
```

Assert that the output image is identical to the image that was provided as input.
