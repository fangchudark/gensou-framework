# ScriptReader

## 描述

用以读取`Genscript`脚本的工具类。

## 静态方法

|[ReadScript](#scriptreaderreadscript)|读取指定路径的脚本。|
|:---|:---|
|[ReadAndExecute](#scriptreaderreadandexecute)|读取并执行指定名称的脚本。|

---

# ScriptReader.ReadScript

`public static string ReadScript(string filePath)`

## 参数

|`filePath`|需要读取的脚本文件路径。|
|:---|:---|

## 描述

读取位于 `filePath` 路径的 `Genscript` 脚本文件，并返回文件内容作为字符串。

虽然此方法的主要设计目的是读取 `Genscript` 脚本文件，但并不限制其只能读取该文件类型。因此，也可用于读取其他任意类型的纯文本文件。

如果文件路径无效或读取失败（例如文件不存在或权限不足），方法会返回 `null`。

## 返回

如果读取文件成功返回读取到的脚本文本内容，否则返回`null`。

---

# ScriptReader.ReadAndExecute

`public static void ReadAndExecute(string script="start")`  
`public static void ReadAndExecute(Node node, string script="start")`

## 参数

|`node`|（Godot平台）挂载到自动加载的脚本初始化器节点。|
|:---|:---|
|`script`|（可选）需要读取的脚本文件名，不包含扩展名。默认为`"start"`|

## 描述

该方法**仅用于**读取并执行指定名称的`Genscript`脚本文件，默认读取名为`start`的脚本。

如果在 **Godot** 平台上使用，`node` 参数需要传入一个挂载到自动加载脚本初始化器节点的 `Node` 对象，支持后续脚本执行的上下文。一般来说，应该传入挂载到自动加载的脚本初始化器节点，或者其他任意被挂载为自动加载的节点。

不同的平台，脚本存放路径大致相同：

- Godot平台脚本应在: `res://Scripts/scriptfile.gs`

- Unity平台脚本应在: `Assets/Scripts/scriptfile.gs`
