# VariableInterpreter

Inherits: [BaseInterpreter](BaseInterpreter.md)

## Description

`VariableInterpreter` is the variable command interpreter of `Genscript`. It is responsible for parsing and executing various [variable operations](../../Genscript/Category/Variable.md) and provides several utility methods.

## Constants

|[INT_MAX](#variableinterpreterint_max)|Maximum value of int64|
|:---|:---|
|[INT_MIN](#variableinterpreterint_min)|Minimum value of int64|

## Static Fields

|[key_words](#variableinterpreterkey_words)|A list of reserved keywords that cannot be used as variable names.|
|:---|:---|
|[variable_list](#variableinterpretervariable_list)|A dictionary that stores the variables defined in `Genscript`, with variable names as keys and variable values as values.|

## Static Methods

|[handle_variable_assignment](#variableinterpreterhandlevariableassignment)|Handles variable assignment operations.|
|:---|:---|
|[handle_variable_declaration](#variableinterpreterhandlevariabledeclaration)|Handles variable declaration operations.|
|[release_variable](#variableinterpreterreleasevariable)|Handles variable release operations.|
|[intfix_to_postfix](#variableinterpreterintfix_to_postfix)|Converts an infix expression to postfix notation. Used in conjunction with [evaluate_postfix](#variableinterpreterevaluate_postfix).|
|[evaluate_postfix](#variableinterpreterevaluatepostfix)|Evaluates the value of a postfix expression. Used in conjunction with [intfix_to_postfix](#variableinterpreterintfix_to_postfix).|
|[check_int_overflow](#variableinterpretercheck_int_overflow)|Checks if a string is an overflowed integer value.|
|[check_variable_name](#variableinterpretercheck_variable_name)|Checks if a string is a valid variable name.|
|[check_expression](#variableinterpretercheck_expression)|Checks if a string is a mathematical expression.|
|[try_get_variable_value](#variableinterpretertry_get_variable_value)|Attempts to retrieve a value from the variable dictionary.|

---

# VariableInterpreter.INT_MAX

`const INT_MAX = 9223372036854775807`

## Description

The maximum value of `int64`

---

# VariableInterpreter.INT_MIN

`const INT_MAX = -9223372036854775808`

## Description

The minimum value of `int64`

---

# VariableInterpreter.key_words

`static var key_words: PackedStringArray`

## Description

This static list stores all command and parameter keywords in `Genscript`. Its main function is to **validate variable names**, ensuring that user-defined variable names do not conflict with keywords.

A complete list of these fields can be found in the [Genscript documentation](../../Genscript/Start.md/#table-of-contents).

---

# VariableInterpreter.variable_list

`static var variable_list: Dictionary`

## Description

This static dictionary stores the variables defined in `Genscript` during execution, with variable names as keys and variable values as values.

---

# VariableInterpreter.handle_variable_assignment

`static func handle_variable_assignment(code: String) -> void`

## Parameters

|`code`|The command line parsed by the [Script Executor](ScriptExecutor.md).|
|:---|:---|

## Description

Handles variable assignment operations, supports the evaluation of mathematical expressions, automatically converts strings to corresponding types, and supports both variable declaration and initialization.

---

# VariableInterpreter.handle_variable_declaration

`static func handle_variable_declaration(code: String) -> void`

## Parameters

|`code`|The command line parsed by the [Script Executor](ScriptExecutor.md).|
|:---|:---|

## Description

Handles variable declaration operations, checks for the validity of variable names, and ensures there are no duplicate declarations.

---

# VariableInterpreter.release_variable

`static func release_variable(variable: String) -> void`

## Parameters

|`variable`|The variable to remove from the variable dictionary.|
|:---|:---|

## Description

Removes a defined variable from the variable dictionary and frees it from memory. If the given variable is not defined, the method returns immediately.

---

# VariableInterpreter.intfix_to_postfix

`static func intfix_to_postfix(input: String) -> Array[String]`

## Parameters

|`input`|An infix expression.|
|:---|:---|

## Description

Converts the given infix expression (mathematical expression) into a postfix expression.

Should be used in conjunction with [evaluate_postfix](#variableinterpreterevaluate_postfix). The returned value should be passed to the [evaluate_postfix](#variableinterpreterevaluate_postfix) method.

## Returns

The converted postfix expression or an error identifier.

- `InvalidExpression`: Indicates the input expression is invalid.

- `UnknownOperator`: Indicates the expression contains an unknown operator.

- `UndefinedVariable`: Indicates the expression uses an undefined variable.

- `InvalidVariableType`: Indicates the expression uses an invalid variable type.

The error identifier will be the first element in the array.

---

# VariableInterpreter.evaluate_postfix

`static func evaluate_postfix(postfix: Array[String]) -> String`

## Parameters

|`postfix`|A postfix expression|
|:---|:---|

## Description

Evaluates the value of the given postfix expression.

Should be used in conjunction with [intfix_to_postfix](#variableinterpreterintfix_to_postfix). Pass the returned postfix expression this method.

## Returns

Returns a string containing the result of the evaluation or an error identifier.

- `InvalidExpression`: Indicates the input expression is invalid.

- `UnknownOperator`: Indicates the expression contains an unknown operator.

- `UndefinedVariable`: Indicates the expression uses an undefined variable.

- `InvalidVariableType`: Indicates the expression uses an invalid variable type.

- `DivisionInvalid`: Indicates an illegal division operation (division by zero).

---

# VariableInterpreter.check_int_overflow

`static func check_int_overflow(value: String) -> bool`

## Parameters

|`value`|The string to check.|
|:---|:---|

## Description

Checks whether the given string represents an integer value that causes an overflow.

## Returns

Returns `true` if the integer conversion of the string would result in an overflow; otherwise, returns `false`.

---

# VariableInterpreter.check_variable_name

`static func check_variable_name(name: String) -> bool`

## Parameters

|`name`|The string to check.|
|:---|:---|

## Description

Validates whether the given string is a valid variable name based on the following rules:

|Required Rules|
|---|
|Must start with an underscore (`_`) or a Unicode letter|
|Can only contain Unicode letters, Unicode digits, and underscores|
|Cannot be parsed as a boolean value (case-insensitive)|
|Cannot be one of the fields defined in the [key_words](#variableinterpreterkey_words) list|

For detailed naming conventions, refer to the [Genscript Variable Documentation](../../Genscript/Category/Variable.md/#naming-conventions).

## Returns

Returns `true` if the string satisfies all conditions and is a valid variable name, otherwise returns `false`.

---

# VariableInterpreter.check_expression

`static func check_expression(expression: String) -> bool`

## Parameters

|`expression`|The string to check.|
|:---|:---|

## Description

Validates whether the given string is a valid mathematical expression based on the following rules:

|Required Rules|
|---|
|Does not start with an operator other than the minus sign (`-`)|
|Operands must be numbers or valid variable names|
|Operands must not contain escaped operators (e.g., `\+`)|

## Returns

Returns `true` if all the above conditions are met and the string is a valid expression, otherwise returns `false`.

---

# VariableInterpreter.try_get_variable_value

`static func try_get_variable_value(variable: String) -> Dictionary`

## Parameters

|`variable`|The variable name to retrieve the value for.|
|:---|:---|
|`value`|The retrieved variable value. If failed, the value is `null`.|

## Description

Attempts to retrieve the value of the given variable from the variable list and convert it to the corresponding type.

## Returns

A dictionary containing `result` and `success` fields.  

If successful, `result` contains the variable's value, and `success` is `true`.  

If unsuccessful, `result` is `null`, and `success` is `false`.

