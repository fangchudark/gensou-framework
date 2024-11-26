# ConditionInterpreter

继承：[BaseInterpreter](BaseInterpreter.md)

## 描述

`ConditionInterpreter`是`Genscript`的[条件参数](../../../Genscript/KeyWords/when.md)解释器，负责解析并评估条件表达式的结果。

## 静态方法

|[CheckCondition](#conditioninterpretercheckcondition)|解析并评估条件表达式。|
|:---|:---|

---

# ConditionInterpreter.CheckCondition

`public static bool CheckCondition(string condition)`

## 参数

|condition|需要评估的条件表达式。|
|:---|:---|

## 描述

评估给定的条件表达式，并返回其结果。

接受以下形式的条件表达式

- 包含条件运算符的条件表达式（如`==`，`>`，`<`，`!=`等）

- 单个布尔值变量

- 单个布尔值（忽略大小写）

方法内部会根据以下规则评估条件表达式的合法性

|需要全部满足的规则|
|---|
|包含条件运算符（如`==`，`>`，`<`，`!=`等）。|
|仅有两个操作数。|
|操作数不能为空。|
|操作数中不包含等号（`=`）。|
|操作数不以条件运算符开头或结尾。|
|操作数中不包含条件运算符。|

如果不满足上述规则，将视作简单条件（布尔值或布尔值变量）进行评估。

如果简单条件不是布尔值或布尔变量则视作无效的参数，返回`false`

评估规则请见[Genscript文档](../../../Genscript/Category/Condition.md/#条件表达式运算符)

## 返回

条件表达式的评估结果（`true`或`false`），如果条件表达式无效则返回`false`
