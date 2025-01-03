# release

## Description

Releases the specified variable from memory. Once deleted, the variable cannot be used again until it is re-declared.

Genscript does not have automatic garbage collection, so it requires the developer to specify which variable to release.

## Parameters

`variable_name`: The variable to be released.

## Usage

`release:variable_name`

## Example

```genscript
-a = 10; // Declare variable a and set it to 10
-@{a} is 10; // Print the value of a
release:a; // Release variable a from memory
```

## Notes

After deleting a variable, it must be re-declared if it is needed again.

Make sure to delete variables only when you are sure they are no longer needed, to avoid affecting other logic or the debugging process.