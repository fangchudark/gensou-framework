# ScriptConsole

## 描述

`ScriptConsole` 是专为 `Genscript` 设计的跨平台脚本控制台类。该类能够根据当前运行环境（Godot 或 Unity）自动将日志信息和错误信息输出到对应平台的控制台。无论在哪个平台，开发者无需调用平台特定的控制台输出函数。

## 静态方法

|[PrintLog](#scriptconsoleprintlog)|将日志信息输出到当前平台的控制台。|
|:---|:---|
|[PrintErr](#scriptconsoleprinterr)|将错误信息输出到当前平台的控制台。|

---

# ScriptConsole.PrintLog

`public static void PrintLog(params object[] message)`

## 参数

|`message`|需要输出到控制台的日志信息，接受一个或多个任意类型的参数。|
|:---|:---|

## 描述

此方法用于将日志信息输出到当前使用的控制台（支持 Godot 或 Unity）。

---

# ScriptConsole.PrintErr

`public static void PrintErr(params object[] message)`

## 参数

|`message`|需要输出到控制台的错误信息，接受一个或多个任意类型的参数。|
|:---|:---|

## 描述

此方法用于将错误信息输出到当前使用的控制台（支持 Godot 或 Unity）。对于 Godot 平台，错误信息将**不会**显示在控制台，而是显示在调试器和操作系统终端中。

