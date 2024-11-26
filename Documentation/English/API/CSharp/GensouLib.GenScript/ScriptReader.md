# ScriptReader

## Description

A utility class for reading `Genscript` scripts.

## Static Methods

|[ReadScript](#scriptreaderreadscript)|Reads a script from the specified file path.|
|:---|:---|
|[ReadAndExecute](#scriptreaderreadandexecute)|Reads and executes the specified script by name.|

---

# ScriptReader.ReadScript

`public static string ReadScript(string filePath)`

## Parameters

|`filePath`|The path to the script file to be read.|
|:---|:---|

## Description

Reads a `Genscript` script file located at the `filePath` and returns its content as a string.

Although the primary purpose of this method is to read `Genscript` files, it is not limited to reading only these file types. It can also be used to read any type of plain text file.

If the file path is invalid or the reading fails (e.g., the file does not exist or there are insufficient permissions), the method returns `null`.

## Returns

If the file is successfully read, the method returns the script content as a string. Otherwise, it returns `null`.

---

# ScriptReader.ReadAndExecute

`public static void ReadAndExecute(string script="start")`  
`public static void ReadAndExecute(Node node, string script="start")`

## Parameters

|`node`|(On Godot platform) A `Node` object attached to the autoloaded script initializer node.|
|:---|:---|
|`script`|(Optional) The name of the script file to be read, without the extension. Defaults to `"start"`.|

## Description

This method is **only** used to read and execute the specified `Genscript` file by name, defaulting to a script named `start`.

If used on the **Godot** platform, the `node` parameter must be a `Node` object attached to an autoloaded script initializer node, which supports the context for subsequent script executions. Generally, this should be a node attached to the autoloaded script initializer or any other node set as autoloaded.

On different platforms, the script file paths are as follows:

- **Godot** platform: The script should be located at `res://Scripts/scriptfile.gs`.
- **Unity** platform: The script should be located at `Assets/Scripts/scriptfile.gs`.
