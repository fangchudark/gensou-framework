# ConditionInterpreter

Inherits: [BaseInterpreter](BaseInterpreter.md)

## Description

`ConditionInterpreter` is the interpreter for [conditional parameters](../../Genscript/KeyWords/when.md) in `Genscript`, responsible for parsing and evaluating the result of conditional expressions.

## Static Methods

|[check_condition](#conditioninterpretercheck_condition)|Parses and evaluates the condition expression.|
|:---|:---|

---

# ConditionInterpreter.check_condition

`static func check_condition(condition: String) -> bool`

## Parameters

|condition|The condition expression to evaluate.|
|:---|:---|

## Description

Evaluates the given condition expression and returns its result.

Supports the following forms of condition expressions:

- Condition expressions containing conditional operators (e.g., `==`, `>`, `<`, `!=`, etc.)
- A single boolean variable
- A single boolean value (case-insensitive)

The internal evaluation checks the following rules for the condition expression:

|Rules that must all be satisfied|
|---|
|Contains a conditional operator (e.g., `==`, `>`, `<`, `!=`, etc.)|
|Has exactly two operands|
|Operands must not be empty|
|Operands must not contain an equal sign (`=`)|
|Operands must not start or end with a conditional operator|
|Operands must not contain other conditional operators|

If the above rules are not satisfied, the expression will be treated as a simple condition (boolean value or boolean variable) and evaluated.

If the simple condition is neither a boolean value nor a boolean variable, it will be treated as an invalid parameter and return `false`.

For the evaluation rules, refer to the [Genscript documentation](../../Genscript/Category/Condition.md/#conditional-expression-operators).

## Returns

The evaluation result of the condition expression (`true` or `false`). If the condition expression is invalid, returns `false`.
