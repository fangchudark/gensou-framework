# VariableInterpreter

继承：[BaseInterpreter](BaseInterpreter.md)

## 描述

`VariableInterpreter`是`Genscript`的变量命令解释器，负责解析并执行各种[变量操作](../../Genscript/Category/Variable.md)，并提供了一些工具方法。

## 常量

|[INT_MAX](#variableinterpreterint_max)|`int64`最大值|
|:---|:---|
|[INT_MIN](#variableinterpreterint_min)|`int64`最小值|

## 静态字段

|[key_words](#variableinterpreterkey_words)|不可用作变量名的关键字列表。|
|:---|:---|
|[variable_list](#variableinterpretervariable_list)|存储`Genscript`中所定义变量的字典，键为变量名，值为变量值。|

## 静态方法

|[handle_variable_assignment](#variableinterpreterhandle_variable_assignment)|处理变量赋值操作。|
|:---|:---|
|[handle_variable_declaration](#variableinterpreterhandle_variable_declaration)|处理变量声明操作。|
|[release_variable](#variableinterpreterrelease_variable)|处理变量释放操作。|
|[intfix_to_postfix](#variableinterpreterintfix_to_postfix)|将中缀表达式转换为后缀表达式。请结合[evaluate_postfix](#variableinterpreterevaluate_postfix)方法一起使用。|
|[evaluate_postfix](#variableinterpreterevaluate_postfix)|计算后缀表达式的值。请结合[intfix_to_postfix](#variableinterpreterintfix_to_postfix)方法一起使用。|
|[check_int_overflow](#variableinterpretercheck_int_overflow)|检查字符串是否是溢出的整数值。|
|[check_variable_name](#variableinterpretercheck_variable_name)|检查字符串是否为合法变量名。|
|[check_expression](#variableinterpretercheck_expression)|检查字符串是否为数学表达式|
|[try_get_variable_value](#variableinterpretertry_get_variable_value)|尝试从变量字典中获取值。|

---

# VariableInterpreter.INT_MAX

`const INT_MAX = 9223372036854775807`

## 描述

`int64`的最大值

---

# VariableInterpreter.INT_MIN

`const INT_MAX = -9223372036854775808`

## 描述

`int64`的最小值

---

# VariableInterpreter.key_words

`static var key_words: PackedStringArray`

## 描述

该静态列表储存了`Genscript`中的所有命令和参数关键字，此列表的主要功能是**验证变量名是否合法**，确保用户定义的变量名不会与关键字冲突。

[Genscript文档目录](../../Genscript/Start.md/#目录)中列出了所有此列表中定义的字段。

---

# VariableInterpreter.variable_list

`static var variable_list: Dictionary`

## 描述

该静态字典储存了`Genscript`在执行时所定义的变量，字典键为变量名，字典值为变量值。

---

# VariableInterpreter.handle_variable_assignment

`static func handle_variable_assignment(code: String) -> void`

## 参数

|`code`|经由[命令执行器](ScriptExecutor.md)进一步解析后的命令行。|
|:---|:---|

## 描述

处理变量的赋值操作，支持计算数学表达式，能够自动将字符串转换为对应类型的值，同时也支持声明变量并初始化值。

---

# VariableInterpreter.handle_variable_declaration

`static func handle_variable_declaration(code: String) -> void`

## 参数

|`code`|经由[命令执行器](ScriptExecutor.md)进一步解析后的命令行。|
|:---|:---|

## 描述

处理变量的声明操作，支持识别变量名的合法性以及是否重复声明

---

# VariableInterpreter.release_variable

`static func release_variable(variable: String) -> void`

## 参数

|`variable`|需要从变量字典移除的变量。|
|:---|:---|

## 描述

从变量字典中移除一个已定义变量，将该变量从内存中释放，如果给定的变量是未定义的变量则直接返回。

---

# VariableInterpreter.intfix_to_postfix

`static func intfix_to_postfix(input: String) -> Array[String]`

## 参数

|`input`|中缀表达式。|
|:---|:---|

## 描述

将输入的中缀表达式（数学表达式）转换为后缀表达式。

请结合[evaluate_postfix](#variableinterpreterevaluate_postfix)方法使用，将返回的值作为参数传递进[evaluate_postfix](#variableinterpreterevaluate_postfix)方法。

## 返回

转换后的后缀表达式或一个错误标识

- `InvalidExpression` ：表示输入的表达式无效。

- `UnknownOperator` ：表示表达式中包含未知运算符。

- `UndefinedVariable` ：表示表达式中使用了未定义的变量。

- `InvalidVariableType` ：表示表达式中使用了不合法的变量类型。

错误标识将作为数组中的第一个元素返回。

---

# VariableInterpreter.evaluate_postfix

`static func evaluate_postfix(postfix: Array[String]) -> String`

## 参数

|`postfix`|后缀表达式|
|:---|:---|

## 描述

计算输入的后缀表达式的值。

请结合[intfix_to_postfix](#variableinterpreterintfix_to_postfix)方法使用，将返回的后缀表达式作为参数传递进来。

## 返回

包含计算结果的字符串或一个错误标识

- `InvalidExpression` ：表示输入的表达式无效。

- `UnknownOperator` ：表示表达式中包含未知运算符。

- `UndefinedVariable` ：表示表达式中使用了未定义的变量。

- `InvalidVariableType` ：表示表达式中使用了不合法的变量类型。

- `DivisionInvalid` ：表示尝试进行非法的除法运算（除数为0）。

---

# VariableInterpreter.check_int_overflow

`static func check_int_overflow(value: String) -> bool`

## 参数

|`value`|要检查的字符串。|
|:---|:---|

## 描述

检查给定的字符串是否为溢出的整数值。

## 返回

如果字符串转换为整数后会导致溢出，返回 `true`；否则返回 `false`。

---

# VariableInterpreter.check_variable_name

`static func check_variable_name(name: String) -> bool`

## 参数

|`name`|需要检查的字符串。|
|:---|:---|

## 描述

根据内部定义的规则验证给定的字符串是否为合法变量名，检查规则如下：

|需要全部满足的规则|
|---|
|以下划线（`_`）或Unicode字母开头|
|只包含Unicode字母和Unicode数字以及下划线|
|不能被解析为布尔值（忽略大小写）|
|不是[key_words](#variableinterpreterkey_words)数组中已定义的字段|

详细的命名规范请见[Genscript变量文档](../../Genscript/Category/Variable.md/#命名规范)

## 返回

如果字符串全部满足上述条件则视作合法变量名，返回`true`，否则返回`false`

---

# VariableInterpreter.check_expression

`static func check_expression(expression: String) -> bool`

## 参数

|`expression`|需要检查的字符串。|
|:---|:---|

## 描述

根据内部定义的规则验证给定的字符串是否为有效数学表达式，检查规则如下：

|需要全部满足的规则|
|---|
|不以减号（`-`）外的运算符开头|
|操作数是数字或合法变量名|
|操作数不包含被转义的运算符（如`\+`）|

## 返回

如果字符串全部满足上述规则则视作有效表达式，返回`true`，否则返回`false`

---

# VariableInterpreter.try_get_variable_value

`static func try_get_variable_value(variable: String) -> Dictionary`

## 参数

|`variable`|要获取值的变量名。|
|:---|:---|

## 描述

尝试从变量列表中获取给定的变量名的值，并转换为对应类型。

## 返回

返回一个字典，包含`result`和`success`字段。

如果解析成功，`result`为获取到的变量值，且`success`为`true`；否则，`result`为`null`，且`success`为`false`。 

