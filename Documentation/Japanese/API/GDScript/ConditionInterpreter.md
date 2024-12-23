# ConditionInterpreter

継承：[BaseInterpreter](BaseInterpreter.md)

## 説明

`ConditionInterpreter`は、`Genscript`の[条件パラメーター](../../Genscript/KeyWords/when.md)インタープリタで、条件式を解析し、評価結果を返します。

## 静的メソッド

|[check_condition](#conditioninterpretercheck_condition)|条件式を解析して評価します。|
|:---|:---|

---

# ConditionInterpreter.check_condition

`static func check_condition(condition: String) -> bool`

## パラメーター

|condition|評価する条件式。|
|:---|:---|

## 説明

指定された条件式を評価し、その結果を返します。

次の形式の条件式をサポートします：

- 条件演算子を含む条件式（例：`==`、`>`、`<`、`!=`など）
- 単一のブール値変数
- 単一のブール値（大文字小文字を区別しません）

内部で、条件式の妥当性を次の規則に従って評価します：

|全て満たすべき規則|
|---|
|条件演算子（例：`==`、`>`、`<`、`!=`など）を含む|
|オペランドは2つのみ|
|オペランドに空白が含まれていない|
|オペランドに等号（`=`）が含まれていない|
|オペランドが条件演算子で始まったり、終わったりしない|
|オペランドに他の条件演算子が含まれていない|

上記の規則を満たさない場合、単純な条件（ブール値またはブール変数）として評価されます。

単純な条件がブール値でもブール変数でもない場合、無効なパラメーターとして扱い、`false`を返します。

評価規則については、[Genscriptドキュメント](../../Genscript/Category/Condition.md/#条件式演算子)を参照してください。

## 戻り値

条件式の評価結果（`true`または`false`）。条件式が無効な場合は`false`を返します。
