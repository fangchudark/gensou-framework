# ScriptExecutor

## Description

`ScriptExecutor` is the command executor class for `Genscript`. As a bridge, it receives commands parsed by the [base interpreter](BaseInterpreter.md) and further parses these commands, passing them to the appropriate command interpreters for processing and execution.

## Static Methods

|[ExecuteCommand](#scriptexecutorexecutecommand)|Further parses and executes the given command.|
|:---|:---|

---

# ScriptExecutor.ExecuteCommand

`public static void ExecuteCommand(string command)`  
`public static void ExecuteCommand(string command, Node node)`

## Parameters

|`command`|The command line parsed by the [base interpreter](BaseInterpreter.md).|
|:---|:---|
|`node`|(For Godot platform) The `Node` object attached to the auto-loaded script initializer.|
  
## Description

Further parses and executes the excluding comments command line parsed by the [base interpreter](BaseInterpreter.md). It first processes the conditional parameters in the command and then proceeds with further parsing and handling based on the command type.

When used on the **Godot** platform, the `node` parameter should be passed as a `Node` object attached to the auto-loaded script initializer to support the context of subsequent script execution. Generally, it should be the node attached to the auto-loader script initializer or any node that is auto-loaded.
