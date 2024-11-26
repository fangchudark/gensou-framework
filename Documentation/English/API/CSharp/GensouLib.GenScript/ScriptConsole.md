# ScriptConsole

## Description

`ScriptConsole` is a cross-platform script console class designed for `Genscript`. This class automatically outputs log and error messages to the appropriate platform's console based on the current runtime environment (Godot or Unity). Developers do not need to call platform-specific console output functions.

## Static Methods

|[PrintLog](#scriptconsoleprintlog)|Outputs log messages to the current platform's console.|
|:---|:---|
|[PrintErr](#scriptconsoleprinterr)|Outputs error messages to the current platform's console.|
---

# ScriptConsole.PrintLog

`public static void PrintLog(params object[] message)`

## Parameters

|`message`|The log messages to be output to the console, accepts one or more parameters of any type.|
|:---|:---|

## Description

This method is used to output log messages to the console on the current platform (either Godot or Unity).

---

# ScriptConsole.PrintErr

`public static void PrintErr(params object[] message)`

## Parameters

|`message`|The error messages to be output to the console, accepts one or more parameters of any type.|
|:---|:---|

## Description

This method is used to output error messages to the console on the current platform (either Godot or Unity). On the Godot platform, error messages will **not** appear in the console but will instead be shown in the debugger and the OS terminal.
