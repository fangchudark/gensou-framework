# VariableInterpreter

継承：[BaseInterpreter](BaseInterpreter.md)

## 説明

`VariableInterpreter`は`Genscript`の変数コマンドインタープリタで、さまざまな[変数操作](../../Genscript/Category/Variable.md)を解析して実行し、いくつかのツールメソッドを提供します。

## 定数

|[INT_MAX](#variableinterpreterint_max)|`int64`の最大値|
|:---|:---|
|[INT_MIN](#variableinterpreterint_min)|`int64`の最小値|

## 静的フィールド

|[key_words](#variableinterpreterkey_words)|変数名として使用できないキーワードのリスト。|
|:---|:---|
|[variable_list](#variableinterpretervariable_list)|`Genscript`で定義された変数を格納する辞書。キーは変数名、値は変数の値。|

## 静的メソッド

|[handle_variable_assignment](#variableinterpreterhandle_variable_assignment)|変数代入操作を処理。|
|:---|:---|
|[handle_variable_declaration](#variableinterpreterhandle_variable_declaration)|変数宣言操作を処理。|
|[release_variable](#variableinterpreterrelease_variable)|変数解放操作を処理。|
|[intfix_to_postfix](#variableinterpreterintfix_to_postfix)|中置記法の式を後置記法に変換。[evaluate_postfix](#variableinterpreterevaluate_postfix)メソッドと組み合わせて使用。|
|[evaluate_postfix](#variableinterpreterevaluate_postfix)|後置記法の式を計算。[intfix_to_postfix](#variableinterpreterintfix_to_postfix)メソッドと組み合わせて使用。|
|[check_int_overflow](#variableinterpretercheck_int_overflow)|检查字符串是否是溢出的整数值。|
|[check_variable_name](#variableinterpretercheck_variable_name)|文字列が有効な変数名かどうかを確認。|
|[check_expression](#variableinterpretercheck_expression)|文字列が数式かどうかを確認。|
|[try_get_variable_value](#variableinterpretertry_get_variable_value)|辞書から変数の値を取得しようと試みる。|

---

# VariableInterpreter.INT_MAX

`const INT_MAX = 9223372036854775807`

## 説明

`int64`の最大値

---

# VariableInterpreter.INT_MIN

`const INT_MAX = -9223372036854775808`

## 説明

`int64`の最小値


---

# VariableInterpreter.key_words

`static var key_words: PackedStringArray`

## 説明

この静的リストは、`Genscript`のすべてのコマンドと引数のキーワードを格納します。このリストの主な機能は**変数名の有効性の検証**であり、ユーザーが定義した変数名がキーワードと衝突しないことを保証します。

[Genscriptのドキュメント](../../Genscript/Start.md/#目次)には、このリストに定義されたすべてのフィールドが一覧されています。

---

# VariableInterpreter.variable_list

`static var variable_list: Dictionary`

## 説明

この静的辞書は、`Genscript`が実行時に定義した変数を格納します。辞書のキーは変数名、辞書の値は変数の値です。

---

# VariableInterpreter.handle_variable_assignment

`static func handle_variable_assignment(code: String) -> void`

## パラメータ

|`code`|[コマンド実行器](ScriptExecutor.md)によって解析されたコマンドライン。|
|:---|:---|

## 説明

変数代入操作を処理します。数式の計算をサポートし、文字列を対応する型の値に自動的に変換します。また、変数の宣言と初期化もサポートします。

---

# VariableInterpreter.handle_variable_declaration

`static func handle_variable_declaration(code: String) -> void`

## パラメータ

|`code`|[コマンド実行器](ScriptExecutor.md)によって解析されたコマンドライン。|
|:---|:---|

## 説明

変数宣言操作を処理します。変数名の有効性と重複宣言の確認も行います。

---

# VariableInterpreter.release_variable

`static func release_variable(variable: String) -> void`

## パラメータ

|`variable`|変数辞書から削除する変数。|
|:---|:---|

## 説明

定義された変数を辞書から削除し、メモリから解放します。指定された変数が未定義の場合は、そのまま戻ります。

---

# VariableInterpreter.intfix_to_postfix

`static func intfix_to_postfix(input: String) -> Array[String]`

## パラメータ

|`input`|中置記法の数式。|
|:---|:---|

## 説明

入力された中置記法の数式（数学の式）を後置記法に変換します。

後置記法に変換された式は、[evaluate_postfix](#variableinterpreterevaluate_postfix)メソッドと組み合わせて使用できます。

## 戻り値

変換後の後置記法の式またはエラー識別子。

- `InvalidExpression` ：入力された式が無効。

- `UnknownOperator` ：式に未知の演算子が含まれている。

- `UndefinedVariable` ：未定義の変数が式に含まれている。

- `InvalidVariableType` ：不正な変数タイプが式に含まれている。

エラー識別子は配列の最初の要素として返されます。

---

# VariableInterpreter.evaluate_postfix

`static func evaluate_postfix(postfix: Array[String]) -> String`

## パラメータ

|`postfix`|後置記法の式。|
|:---|:---|

## 説明

入力された後置記法の式の値を計算します。

後置記法の式は、[intfix_to_postfix](#variableinterpreterintfix_to_postfix)メソッドを使って変換し、その後、このメソッドに渡して計算します。

## 戻り値

計算結果を示す文字列またはエラー識別子。

- `InvalidExpression` ：入力された式が無効。

- `UnknownOperator` ：式に未知の演算子が含まれている。

- `UndefinedVariable` ：未定義の変数が式に含まれている。

- `InvalidVariableType` ：不正な変数タイプが式に含まれている。

---

# VariableInterpreter.check_int_overflow

`static func check_int_overflow(value: String) -> bool`

## パラメータ

|`value`|チェックする文字列。|
|:---|:---|

## 説明

指定された文字列が、オーバーフローを引き起こす整数値を表しているかどうかを確認します。

## 戻り値

文字列が整数に変換された際にオーバーフローを引き起こす場合は `true` を返し、それ以外の場合は `false` を返します。

---

# VariableInterpreter.check_variable_name

`static func check_variable_name(name: String) -> bool`

## パラメータ

|`name`|検証する文字列。|
|:---|:---|

## 説明

内部で定義されたルールに基づき、与えられた文字列が有効な変数名であるかを検証します。検証ルールは以下の通りです：

|必要なルール|
|---|
|変数名はアンダースコア（`_`）またはUnicode文字で始まること|
|変数名はUnicode文字、Unicode数字、またはアンダースコアのみを含むこと|
|変数名は論理値（大文字小文字を区別しない）として解釈できないこと|
|変数名は[key_words](#variableinterpreterkey_words)リストに定義されたフィールドでないこと|

詳細な命名規則については、[Genscript変数文書](../../../Genscript/Category/Variable.md/#命名規則)を参照してください。

## 戻り値

文字列が上記の条件をすべて満たす場合は有効な変数名として`true`を返し、それ以外の場合は`false`を返します。

---

# VariableInterpreter.check_expression

`static func check_expression(expression: String) -> bool`

## パラメータ

|`expression`|検証する文字列。|
|:---|:---|

## 説明

内部で定義されたルールに基づき、与えられた文字列が有効な数式であるかを検証します。検証ルールは以下の通りです：

|必要なルール|
|---|
|式は`-`以外の演算子で始まらないこと|
|オペランドは数値または有効な変数名であること|
|オペランドにエスケープされた演算子（例：`\+`）が含まれていないこと|

## 戻り値

文字列が上記のルールを満たしていれば有効な式として`true`を返し、それ以外の場合は`false`を返します。

---

# VariableInterpreter.try_get_variable_value

`static func try_get_variable_value(variable: String) -> Dictionary`

## パラメータ

|`variable`|値を取得する変数名。|
|:---|:---|

## 説明

指定された変数名の値を変数リストから取得し、その型に変換します。

## 戻り値

`result` と `success` フィールドを含む辞書を返します。  

成功した場合、`result` は変数の値を含み、`success` は `true` です。  

失敗した場合、`result` は `null`、`success` は `false` です。
