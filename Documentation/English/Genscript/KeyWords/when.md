# when

## Description

`when` is an optional parameter for all commands. It is used to execute the specified command line when the given condition evaluates to `true`. If the condition evaluates to `false`, the command will be skipped.

## Value

Default value: `true`

The value can be any of the following types:
- A conditional expression (e.g., `a > b`)
- A boolean variable (e.g., `is_ready`)
- A boolean constant (e.g., `true` or `false`)

## Usage

`command -when=condition_expression`

## Example

```genscript
-a = 10
-b = 100
-@1000 -when=a * b == 1000  |: Prints 1000 only when a * b equals 1000
```