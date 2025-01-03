# when

## 描述

`when` 是所有命令的可选参数。用于在指定条件为 `true` 时执行该行命令。如果条件为 `false`，命令将被跳过。

## 值

默认值：`true`

可以是以下任意类型：
- 一个条件表达式（如 `a > b`）
- 一个布尔值变量（如 `is_ready`）
- 一个布尔值常量（如 `true` 或 `false`）

## 使用

`command -when=condition_expression`

## 示例

```genscript
-a = 10
-b = 100
-@1000 -when=a * b == 1000; // 只有当 a * b 等于 1000 时才会打印 1000
