# ScriptExecutor

Inherits: [Object](https://docs.godotengine.org/en/stable/classes/class_object.html)

## Description

`ScriptExecutor` is the command executor class for `Genscript`. As a bridge, it receives commands parsed by the [base interpreter](BaseInterpreter.md) and further parses these commands, passing them to the appropriate command interpreters for processing and execution.

## Static Methods

|[execute_command](#scriptexecutorexecute_command)|Further parses and executes the given command.|
|:---|:---|

---

# ScriptExecutor.execute_command

`static func execute_command(command: String, node: Node) -> void`  

## Parameters

|`command`|The command line parsed by the [base interpreter](BaseInterpreter.md).|
|:---|:---|
|`node`|The `Node` object attached to the auto-loaded script initializer.|
  
## Description

Further parses and executes the excluding comments command line parsed by the [base interpreter](BaseInterpreter.md). It first processes the conditional parameters in the command and then proceeds with further parsing and handling based on the command type.

The `node` parameter should be passed as a `Node` object attached to the auto-loaded script initializer to support the context of subsequent script execution. Generally, it should be the node attached to the auto-loader script initializer or any node that is auto-loaded.
