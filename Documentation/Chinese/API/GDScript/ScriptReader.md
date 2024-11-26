# ScriptReader

继承：[Object](https://docs.godotengine.org/zh-cn/stable/classes/class_object.html)

## 描述

用以读取`Genscript`脚本的工具类。

## 静态方法

|[read_script](#scriptreaderread_script)|读取指定路径的脚本。|
|:---|:---|
|[read_and_execute](#scriptreaderread_and_execute)|读取并执行指定名称的脚本。|

---

# ScriptReader.read_script

`static func read_script(file_path: String) -> String`

## 参数

|`file_path`|需要读取的脚本文件路径。|
|:---|:---|

## 描述

读取位于 `file_path` 路径的 `Genscript` 脚本文件，并返回文件内容作为字符串。

虽然此方法的主要设计目的是读取 `Genscript` 脚本文件，但并不限制其只能读取该文件类型。因此，也可用于读取其他任意类型的纯文本文件。

如果文件路径无效或读取失败（例如文件不存在或权限不足），方法会返回空字符串。

## 返回

如果读取文件成功返回读取到的脚本文本内容，否则返回空字符串。

---

# ScriptReader.read_and_execute

`static func read_and_execute(node: Node, script: String = "start") -> void`

## 参数

|`node`|挂载到自动加载的脚本初始化器节点。|
|:---|:---|
|`script`|（可选）需要读取的脚本文件名，不包含扩展名。默认为`"start"`|

## 描述

该方法**仅用于**读取并执行指定名称的`Genscript`脚本文件，默认读取名为`start`的脚本。

`node` 参数需要传入一个挂载到自动加载脚本初始化器节点的 `Node` 对象，支持后续脚本执行的上下文。一般来说，应该传入挂载到自动加载的脚本初始化器节点，或者其他任意被挂载为自动加载的节点。

脚本应在: `res://Scripts/scriptfile.gs`

