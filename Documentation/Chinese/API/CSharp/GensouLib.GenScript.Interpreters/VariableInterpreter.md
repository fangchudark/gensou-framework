# VariableInterpreter

继承：[BaseInterpreter](BaseInterpreter.md)

## 描述

`VariableInterpreter`是`Genscript`的变量命令解释器，负责解析并执行各种[变量操作](../../../Genscript/Category/Variable.md)，并提供了一些工具方法。

## 静态字段

|[KeyWords](#variableinterpreterkeywords)|不可用作变量名的关键字列表。|
|:---|:---|
|[VariableList](#variableinterpretervariablelist)|存储`Genscript`中所定义变量的字典，键为变量名，值为变量值。|

## 静态方法

|[HandleVariableAssignment](#variableinterpreterhandlevariableassignment)|处理变量赋值操作。|
|:---|:---|
|[HandleVariableDeclaration](#variableinterpreterhandlevariabledeclaration)|处理变量声明操作。|
|[CheckExpression](#variableinterpretercheckexpression)|检查字符串是否为数学表达式|
|[ReleaseVariable](#variableinterpreterreleasevariable)|处理变量释放操作。|
|[CheckVariableName](#variableinterpretercheckvariablename)|检查字符串是否为合法变量名。|
|[TryGetVariableValue](#variableinterpretertrygetvariablevalue)|尝试从变量字典中获取值。|
|[InfixToPostfix](#variableinterpreterinfixtopostfix)|将中缀表达式转换为后缀表达式。请结合[EvaluatePostfix](#variableinterpreterevaluatepostfix)方法一起使用。|
|[EvaluatePostfix](#variableinterpreterevaluatepostfix)|计算后缀表达式的值。请结合[InfixToPostfix](#variableinterpreterinfixtopostfix)方法一起使用。|

---

# VariableInterpreter.KeyWords

`public static List<string> KeyWords`

## 描述

该静态列表储存了`Genscript`中的所有命令和参数关键字，此列表的主要功能是**验证变量名是否合法**，确保用户定义的变量名不会与关键字冲突。

[Genscript文档目录](../../../Genscript/Start.md/#目录)中列出了所有此列表中定义的字段。

---

# VariableInterpreter.VariableList

`public static Dictionary<string, object> VariableList`

## 描述

该静态字典储存了`Genscript`在执行时所定义的变量，字典键为变量名，字典值为变量值。

---

# VariableInterpreter.HandleVariableAssignment

`public static void HandleVariableAssignment(string code)`

## 参数

|`code`|经由[命令执行器](ScriptExecutor.md)进一步解析后的命令行。|
|:---|:---|

## 描述

处理变量的赋值操作，支持计算数学表达式，能够自动将字符串转换为对应类型的值，同时也支持声明变量并初始化值。

---

# VariableInterpreter.HandleVariableDeclaration

`public static void HandleVariableDeclaration(string code)`

## 参数

|`code`|经由[命令执行器](ScriptExecutor.md)进一步解析后的命令行。|
|:---|:---|

## 描述

处理变量的声明操作，支持识别变量名的合法性以及是否重复声明

---

# VariableInterpreter.CheckExpression

`public static bool CheckExpression(string expression)`

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

# VariableInterpreter.ReleaseVariable

`public static void ReleaseVariable(string variable)`

## 参数

|`variable`|需要从变量字典移除的变量。|
|:---|:---|

## 描述

从变量字典中移除一个已定义变量，将该变量从内存中释放，如果给定的变量是未定义的变量则直接返回。

---

# VariableInterpreter.CheckVariableName

`public static bool CheckVariableName(string name)`

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
|不是[KeyWords](#variableinterpreterkeywords)列表中已定义的字段|

详细的命名规范请见[Genscript变量文档](../../../Genscript/Category/Variable.md/#命名规范)

## 返回

如果字符串全部满足上述条件则视作合法变量名，返回`true`，否则返回`false`

---

# VariableInterpreter.TryGetVariableValue

`public static bool TryGetVariableValue(string variable, out object value)`

## 参数

|`variable`|要获取值的变量名。|
|:---|:---|
|`value`|获取到的变量值。获取失败时值为`null`|

## 描述

尝试从变量列表中获取给定的变量名的值，并转换为对应类型。

## 返回

如果变量已定义且类型匹配（`double`，`bool`，`int64`，`string`），则返回`true`，并通过`value`输出对应的值；否则返回`false`，并且`value`为`null`。

---

# VariableInterpreter.InfixToPostfix

`public static List<string> InfixToPostfix(string input)`

## 参数

|`input`|中缀表达式。|
|:---|:---|

## 描述

将输入的中缀表达式（数学表达式）转换为后缀表达式。

请结合[EvaluatePostfix](#variableinterpreterevaluatepostfix)方法使用，将返回的值作为参数传递进[EvaluatePostfix](#variableinterpreterevaluatepostfix)方法。

## 返回

转换后的后缀表达式或一个错误标识

- `InvalidExpression` ：表示输入的表达式无效。

- `UnknownOperator` ：表示表达式中包含未知运算符。

- `UndefinedVariable` ：表示表达式中使用了未定义的变量。

- `InvalidVariableType` ：表示表达式中使用了不合法的变量类型。

错误标识将作为列表中的第一个元素返回。

---

# VariableInterpreter.EvaluatePostfix

`public static string EvaluatePostfix(List<string> postfix)`

## 参数

|`postfix`|后缀表达式|
|:---|:---|

## 描述

计算输入的后缀表达式的值。

请结合[InfixToPostfix](#variableinterpreterinfixtopostfix)方法使用，将返回的后缀表达式作为参数传递进来。

## 返回

包含计算结果的字符串或一个错误标识

- `InvalidExpression` ：表示输入的表达式无效。

- `UnknownOperator` ：表示表达式中包含未知运算符。

- `UndefinedVariable` ：表示表达式中使用了未定义的变量。

- `InvalidVariableType` ：表示表达式中使用了不合法的变量类型。

- `DivisionInvalid` ：表示尝试进行非法的除法运算（除数为0）。
