# ScriptExecutor

## 描述

`ScriptExecutor` 是 `Genscript` 的命令执行器类。它作为一个桥接层，负责接收由[基础解释器](BaseInterpreter.md)解析出的命令，并进一步解析这些命令，将其传递给相应的命令解释器进行处理和执行。

## 静态方法

|[ExecuteCommand](#scriptexecutorexecutecommand)|进一步解析并执行传入的命令。|
|:---|:---|

---

# ScriptExecutor.ExecuteCommand

`public static void ExecuteCommand(string command)`  
`public static void ExecuteCommand(string command, Node node)`

## 参数

|`command`|[基础解释器](BaseInterpreter.md)解析出的命令行。|
|:---|:---|
|`node`|（Godot平台）挂载到自动加载的脚本初始化器节点。|

## 描述

进一步解析并执行[基础解释器](BaseInterpreter.md)排除注释后的命令行，首先处理命令中的条件参数，然后根据命令类型进行后续解析和处理。

如果在 **Godot** 平台上使用，`node` 参数需要传入一个挂载到自动加载脚本初始化器节点的 `Node` 对象，支持后续脚本执行的上下文。一般来说，应该传入挂载到自动加载的脚本初始化器节点，或者其他任意被挂载为自动加载的节点。