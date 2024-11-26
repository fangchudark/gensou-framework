# VariableInterpreter

Inherits: [BaseInterpreter](BaseInterpreter.md)

## Description

`VariableInterpreter` is the variable command interpreter of `Genscript`. It is responsible for parsing and executing various [variable operations](../../../Genscript/Category/Variable.md) and provides several utility methods.

## Static Fields

|[KeyWords](#variableinterpreterkeywords)|A list of reserved keywords that cannot be used as variable names.|
|:---|:---|
|[VariableList](#variableinterpretervariablelist)|A dictionary that stores the variables defined in `Genscript`, with variable names as keys and variable values as values.|

## Static Methods

|[HandleVariableAssignment](#variableinterpreterhandlevariableassignment)|Handles variable assignment operations.|
|:---|:---|
|[HandleVariableDeclaration](#variableinterpreterhandlevariabledeclaration)|Handles variable declaration operations.|
|[CheckExpression](#variableinterpretercheckexpression)|Checks if a string is a mathematical expression.|
|[ReleaseVariable](#variableinterpreterreleasevariable)|Handles variable release operations.|
|[CheckVariableName](#variableinterpretercheckvariablename)|Checks if a string is a valid variable name.|
|[TryGetVariableValue](#variableinterpretertrygetvariablevalue)|Attempts to retrieve a value from the variable dictionary.|
|[InfixToPostfix](#variableinterpreterinfixtopostfix)|Converts an infix expression to postfix notation. Used in conjunction with [EvaluatePostfix](#variableinterpreterevaluatepostfix).|
|[EvaluatePostfix](#variableinterpreterevaluatepostfix)|Evaluates the value of a postfix expression. Used in conjunction with [InfixToPostfix](#variableinterpreterinfixtopostfix).|

---

# VariableInterpreter.KeyWords

`public static List<string> KeyWords`

## Description

This static list stores all command and parameter keywords in `Genscript`. Its main function is to **validate variable names**, ensuring that user-defined variable names do not conflict with keywords.

A complete list of these fields can be found in the [Genscript documentation](../../../Genscript/Start.md/#table-of-contents).

---

# VariableInterpreter.VariableList

`public static Dictionary<string, object> VariableList`

## Description

This static dictionary stores the variables defined in `Genscript` during execution, with variable names as keys and variable values as values.

---

# VariableInterpreter.HandleVariableAssignment

`public static void HandleVariableAssignment(string code)`

## Parameters

|`code`|The command line parsed by the [Script Executor](ScriptExecutor.md).|
|:---|:---|

## Description

Handles variable assignment operations, supports the evaluation of mathematical expressions, automatically converts strings to corresponding types, and supports both variable declaration and initialization.

---

# VariableInterpreter.HandleVariableDeclaration

`public static void HandleVariableDeclaration(string code)`

## Parameters

|`code`|The command line parsed by the [Script Executor](ScriptExecutor.md).|
|:---|:---|

## Description

Handles variable declaration operations, checks for the validity of variable names, and ensures there are no duplicate declarations.

---

# VariableInterpreter.CheckExpression

`public static bool CheckExpression(string expression)`

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

# VariableInterpreter.ReleaseVariable

`public static void ReleaseVariable(string variable)`

## Parameters

|`variable`|The variable to remove from the variable dictionary.|
|:---|:---|

## Description

Removes a defined variable from the variable dictionary and frees it from memory. If the given variable is not defined, the method returns immediately.

---

# VariableInterpreter.CheckVariableName

`public static bool CheckVariableName(string name)`

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
|Cannot be one of the fields defined in the [KeyWords](#variableinterpreterkeywords) list|

For detailed naming conventions, refer to the [Genscript Variable Documentation](../../Genscript/Category/Variable.md/#naming-conventions).

## Returns

Returns `true` if the string satisfies all conditions and is a valid variable name, otherwise returns `false`.

---

# VariableInterpreter.TryGetVariableValue

`public static bool TryGetVariableValue(string variable, out object value)`

## Parameters

|`variable`|The variable name to retrieve the value for.|
|:---|:---|
|`value`|The retrieved variable value. If failed, the value is `null`.|

## Description

Attempts to retrieve the value of the given variable from the variable list and convert it to the corresponding type.

## Returns

Returns `true` if the variable is defined and its type matches (`double`, `bool`, `int64`, `string`), and outputs the corresponding value via `value`. Otherwise, returns `false`, and `value` will be `null`.

---

# VariableInterpreter.InfixToPostfix

`public static List<string> InfixToPostfix(string input)`

## Parameters

|`input`|An infix expression.|
|:---|:---|

## Description

Converts the given infix expression (mathematical expression) into a postfix expression.

Should be used in conjunction with [EvaluatePostfix](#variableinterpreterevaluatepostfix). The returned value should be passed to the [EvaluatePostfix](#variableinterpreterevaluatepostfix) method.

## Returns

The converted postfix expression or an error identifier.

- `InvalidExpression`: Indicates the input expression is invalid.

- `UnknownOperator`: Indicates the expression contains an unknown operator.

- `UndefinedVariable`: Indicates the expression uses an undefined variable.

- `InvalidVariableType`: Indicates the expression uses an invalid variable type.

The error identifier will be the first element in the list.

---

# VariableInterpreter.EvaluatePostfix

`public static string EvaluatePostfix(List<string> postfix)`

## Parameters

|`postfix`|A postfix expression|
|:---|:---|

## Description

Evaluates the value of the given postfix expression.

Should be used in conjunction with [InfixToPostfix](#variableinterpreterinfixtopostfix). Pass the returned postfix expression this method.

## Returns

Returns a string containing the result of the evaluation or an error identifier.

- `InvalidExpression`: Indicates the input expression is invalid.

- `UnknownOperator`: Indicates the expression contains an unknown operator.

- `UndefinedVariable`: Indicates the expression uses an undefined variable.

- `InvalidVariableType`: Indicates the expression uses an invalid variable type.

- `DivisionInvalid`: Indicates an illegal division operation (division by zero).
