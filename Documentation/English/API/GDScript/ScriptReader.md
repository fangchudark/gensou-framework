# ScriptReader

Inherits: [Object](https://docs.godotengine.org/en/stable/classes/class_object.html)

## Description

A utility class for reading `Genscript` scripts.

## Static Methods

|[read_script](#scriptreaderread_script)|Reads a script from the specified file path.|
|:---|:---|
|[read_and_execute](#scriptreaderread_and_execute)|Reads and executes the specified script by name.|

---

# ScriptReader.read_script

`static func read_script(file_path: String) -> String`

## Parameters

|`file_path`|The path to the script file to be read.|
|:---|:---|

## Description

Reads a `Genscript` script file located at the `file_path` and returns its content as a string.

Although the primary purpose of this method is to read `Genscript` files, it is not limited to reading only these file types. It can also be used to read any type of plain text file.

If the file path is invalid or the reading fails (e.g., the file does not exist or there are insufficient permissions), the method returns empty string.

## Returns

If the file is successfully read, the method returns the script content as a string. Otherwise, it returns empty string.

---

# ScriptReader.read_and_execute

`static func read_and_execute(node: Node, script: String = "start") -> void`

## Parameters

|`node`|A `Node` object attached to the autoloaded script initializer node.|
|:---|:---|
|`script`|(Optional) The name of the script file to be read, without the extension. Defaults to `"start"`.|

## Description

This method is **only** used to read and execute the specified `Genscript` file by name, defaulting to a script named `start`.

The `node` parameter must be a `Node` object attached to an autoloaded script initializer node, which supports the context for subsequent script executions. Generally, this should be a node attached to the autoloaded script initializer or any other node set as autoloaded.

The script should be located at `res://Scripts/scriptfile.gs`.
