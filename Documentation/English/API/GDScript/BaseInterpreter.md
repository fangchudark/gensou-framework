# BaseInterpreter

Inherits: [Object](https://docs.godotengine.org/en/stable/classes/class_object.html)

Inherited By: [ConditionInterpreter](ConditionInterpreter.md), [VariableInterpreter](VariableInterpreter.md)

## Description

`BaseInterpreter` is the core interpreter class of `Genscript`, primarily responsible for splitting script content into commands and removing comments. It also provides the capability to handle [console commands](../../Genscript/Category/Console.md) and includes some common string processing utility methods.

## Static Methods

|[parse_script](#baseinterpreterparse_script)|Splits the entire script file into commands, excluding the comment content.|
|:--|:--|
|[handle_debug_output](#baseinterpreterhandle_debug_output)|Handles console output commands.|
|[replace_placeholders](#baseinterpreterreplace_placeholders)|Replaces escape characters and variables in the text.|
|[try_parse_str_to_bool](#baseinterpretertry_parse_str_to_bool)|Attempts to parse a string as a boolean value.|
|[try_parse_numeric](#baseinterpretertry_parse_numeric)|Attempts to parse a string as a number.|

---

# BaseInterpreter.parse_script

`static func parse_script(script_content: String, node: Node) -> void`  

## Parameters

|`script_content`|The script content obtained by the script reader.|
|:--|:--|
|`node`|The `Node` object mounted on the auto-loaded script initializer node.|

## Description

Responsible for splitting the provided script content into individual commands and processing each line. Comments are removed, and each line of code is passed to the [command executor](ScriptExecutor.md) for further execution. Specifically, the method handles [multi-line log commands](../../Genscript/Category/Console.md/#-n), continuing to process the commands only after exiting the multi-line log mode.

The `node` parameter should be passed as a `Node` object mounted on the auto-loaded script initializer node, providing context for subsequent script execution. Generally, it should be the auto-loaded script initializer node or any other node mounted as auto-loaded.

---

# BaseInterpreter.handle_debug_output

`static func handle_debug_output(code: String) -> void`

## Parameters

|`code`|The command line further parsed by the [command executor](ScriptExecutor.md).|
|---|---|

## Description

Console command interpreter for `Genscript`, responsible for parsing and executing console commands. It supports multi-line log output, escape character replacement, mathematical expression evaluation (for single-line console output only), and variable interpolation.

---

# BaseInterpreter.replace_placeholders

`static func replace_placeholders(input: String) -> String`

## Parameters

|`input`|The string that contains placeholders to be replaced.|
|---|---|

## Description

Replace placeholders in the input string. It processes all defined escape characters and supports replacing valid variables in `Genscript` with their corresponding values.

The escape characters defined within the method are:

|Escape Character|Replacement|  
|---|---|  
|`\}`|Replaced with `}`|  
|`\{`|Replaced with `{`|  
|`\/`|Replaced with `/`|  
|`\+`|Replaced with `+`|  
|`\-`|Replaced with `-`|  
|`\*`|Replaced with `*`|  
|`\%`|Replaced with `%`|  
|`\\`|Replaced with `\`|  
|`\"`|Replaced with `"`|  

---

# BaseInterpreter.try_parse_str_to_bool

`static func try_parse_str_to_bool(value: String) -> Dictionary`

## Parameters

|`value`|The string to be parsed as a boolean value.|
|---|---|

## Description

Attempts to parse the input string as a boolean value (case-insensitive) and returns the result.

## Returns

A dictionary containing `result` and `success` fields.  

If the parsing is successful, `result` contains the parsed boolean value, and `success` is `true`.  

Otherwise, `result` is `null`, and `success` is `false`.

---

# BaseInterpreter.try_parse_numeric

`static func try_parse_numeric(input: String) -> Dictionary`

## Parameters

|`input`|The string to be parsed as a number.|
|---|---|

## Description

Attempts to parse the input string as an integer or floating-point number and returns the parsing result.

## Returns

A dictionary containing `result` and `success` fields.  

If the parsing is successful, `result` contains the numeric value (integer or float), and `success` is `true`. 

Otherwise, `result` is `null`, and `success` is `false`. 