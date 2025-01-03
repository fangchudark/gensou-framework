# BaseInterpreter

Inherited By: [ConditionInterpreter](ConditionInterpreter.md), [VariableInterpreter](VariableInterpreter.md)

## Description

`BaseInterpreter` is the core interpreter class of `Genscript`, primarily responsible for splitting script content into commands and removing comments. It also provides some common string processing utility methods.

## Static Methods

|[ParseScript](#baseinterpreterparsescript)|Splits the entire script file into commands, excluding the comment content.|
|:--|:--|
|[ReplacePlaceholders](#baseinterpreterreplaceplaceholders)|Replaces escape characters and variables in the text.|
|[TryParseNumeric](#baseinterpretertryparsenumeric)|Attempts to parse a string as a number.|

---

# BaseInterpreter.ParseScript

`public static void ParseScript(string scriptContent)`  
`public static void ParseScript(string scriptContent, Node node)`

## Parameters

|`scriptContent`|The script content obtained by the script reader.|
|:--|:--|
|`node`|(On Godot platform) The `Node` object mounted on the auto-loaded script initializer node.|

## Description

Responsible for splitting the provided script content into individual commands and processing each line. Comments are removed, and each line of code is passed to the [command executor](ScriptExecutor.md) for further execution. Specifically, the method handles [multi-line log commands](../../../Genscript/Category/Console.md/#-n), continuing to process the commands only after exiting the multi-line log mode.

If used on the **Godot** platform, the `node` parameter should be passed as a `Node` object mounted on the auto-loaded script initializer node, providing context for subsequent script execution. Generally, it should be the auto-loaded script initializer node or any other node mounted as auto-loaded.

---

# BaseInterpreter.ReplacePlaceholders

`public static string ReplacePlaceholders(string input)`

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

# BaseInterpreter.TryParseNumeric

`public static bool TryParseNumeric(string input, out object result)`

## Parameters

|`input`|The string to be parsed as a number.|
|---|---|
|`result`|The numeric value after parsing the string. If parsing fails, the value will be `null`.|

## Description

Attempts to parse the input string as an integer or floating-point number and returns the parsing result.

## Returns

If the string is a valid integer or floating-point format, it returns `true` along with the corresponding value; otherwise, it returns `false` and `null`.