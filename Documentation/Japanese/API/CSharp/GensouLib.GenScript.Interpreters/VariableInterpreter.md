# VariableInterpreter

継承：[BaseInterpreter](BaseInterpreter.md)

## 説明

`VariableInterpreter`は`Genscript`の変数コマンドインタープリタで、さまざまな[変数操作](../../../Genscript/Category/Variable.md)を解析して実行し、いくつかのツールメソッドを提供します。

## 静的フィールド

|[KeyWords](#variableinterpreterkeywords)|変数名として使用できないキーワードのリスト。|
|:---|:---|
|[VariableList](#variableinterpretervariablelist)|`Genscript`で定義された変数を格納する辞書。キーは変数名、値は変数の値。|

## 静的メソッド

|[HandleVariableAssignment](#variableinterpreterhandlevariableassignment)|変数代入操作を処理。|
|:---|:---|
|[HandleVariableDeclaration](#variableinterpreterhandlevariabledeclaration)|変数宣言操作を処理。|
|[CheckExpression](#variableinterpretercheckexpression)|文字列が数式かどうかを確認。|
|[ReleaseVariable](#variableinterpreterreleasevariable)|変数解放操作を処理。|
|[CheckVariableName](#variableinterpretercheckvariablename)|文字列が有効な変数名かどうかを確認。|
|[TryGetVariableValue](#variableinterpretertrygetvariablevalue)|辞書から変数の値を取得しようと試みる。|
|[InfixToPostfix](#variableinterpreterinfixtopostfix)|中置記法の式を後置記法に変換。[EvaluatePostfix](#variableinterpreterevaluatepostfix)メソッドと組み合わせて使用。|
|[EvaluatePostfix](#variableinterpreterevaluatepostfix)|後置記法の式を計算。[InfixToPostfix](#variableinterpreterinfixtopostfix)メソッドと組み合わせて使用。|

---

# VariableInterpreter.KeyWords

`public static List<string> KeyWords`

## 説明

この静的リストは、`Genscript`のすべてのコマンドと引数のキーワードを格納します。このリストの主な機能は**変数名の有効性の検証**であり、ユーザーが定義した変数名がキーワードと衝突しないことを保証します。

[Genscriptのドキュメント](../../../Genscript/Start.md/#目次)には、このリストに定義されたすべてのフィールドが一覧されています。

---

# VariableInterpreter.VariableList

`public static Dictionary<string, object> VariableList`

## 説明

この静的辞書は、`Genscript`が実行時に定義した変数を格納します。辞書のキーは変数名、辞書の値は変数の値です。

---

# VariableInterpreter.HandleVariableAssignment

`public static void HandleVariableAssignment(string code)`

## パラメーター

|`code`|[コマンド実行器](ScriptExecutor.md)によって解析されたコマンドライン。|
|:---|:---|

## 説明

変数代入操作を処理します。数式の計算をサポートし、文字列を対応する型の値に自動的に変換します。また、変数の宣言と初期化もサポートします。

---

# VariableInterpreter.HandleVariableDeclaration

`public static void HandleVariableDeclaration(string code)`

## パラメーター

|`code`|[コマンド実行器](ScriptExecutor.md)によって解析されたコマンドライン。|
|:---|:---|

## 説明

変数宣言操作を処理します。変数名の有効性と重複宣言の確認も行います。

---

# VariableInterpreter.CheckExpression

`public static bool CheckExpression(string expression)`

## パラメーター

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

# VariableInterpreter.ReleaseVariable

`public static void ReleaseVariable(string variable)`

## パラメーター

|`variable`|変数辞書から削除する変数。|
|:---|:---|

## 説明

定義された変数を辞書から削除し、メモリから解放します。指定された変数が未定義の場合は、そのまま戻ります。

---

# VariableInterpreter.CheckVariableName

`public static bool CheckVariableName(string name)`

## パラメーター

|`name`|検証する文字列。|
|:---|:---|

## 説明

内部で定義されたルールに基づき、与えられた文字列が有効な変数名であるかを検証します。検証ルールは以下の通りです：

|必要なルール|
|---|
|変数名はアンダースコア（`_`）またはUnicode文字で始まること|
|変数名はUnicode文字、Unicode数字、またはアンダースコアのみを含むこと|
|変数名は論理値（大文字小文字を区別しない）として解釈できないこと|
|変数名は[KeyWords](#variableinterpreterkeywords)リストに定義されたフィールドでないこと|

詳細な命名規則については、[Genscript変数文書](../../../Genscript/Category/Variable.md/#命名規則)を参照してください。

## 戻り値

文字列が上記の条件をすべて満たす場合は有効な変数名として`true`を返し、それ以外の場合は`false`を返します。

---

# VariableInterpreter.TryGetVariableValue

`public static bool TryGetVariableValue(string variable, out object value)`

## パラメーター

|`variable`|値を取得する変数名。|
|:---|:---|
|`value`|取得した変数の値。取得に失敗した場合、`null`となる。|

## 説明

指定された変数名の値を変数リストから取得し、その型に変換します。

## 戻り値

変数が定義されており、型が一致する場合（`double`、`bool`、`int64`、`string`）、`true`を返し、`value`に対応する値を出力します。それ以外の場合は`false`を返し、`value`は`null`となります。

---

# VariableInterpreter.InfixToPostfix

`public static List<string> InfixToPostfix(string input)`

## パラメーター

|`input`|中置記法の数式。|
|:---|:---|

## 説明

入力された中置記法の数式（数学の式）を後置記法に変換します。

後置記法に変換された式は、[EvaluatePostfix](#variableinterpreterevaluatepostfix)メソッドと組み合わせて使用できます。

## 戻り値

変換後の後置記法の式またはエラー識別子。

- `InvalidExpression` ：入力された式が無効。

- `UnknownOperator` ：式に未知の演算子が含まれている。

- `UndefinedVariable` ：未定義の変数が式に含まれている。

- `InvalidVariableType` ：不正な変数タイプが式に含まれている。

エラー識別子はリストの最初の要素として返されます。

---

# VariableInterpreter.EvaluatePostfix

`public static string EvaluatePostfix(List<string> postfix)`

## パラメーター

|`postfix`|後置記法の式。|
|:---|:---|

## 説明

入力された後置記法の式の値を計算します。

後置記法の式は、[InfixToPostfix](#variableinterpreterinfixtopostfix)メソッドを使って変換し、その後、このメソッドに渡して計算します。

## 戻り値

計算結果を示す文字列またはエラー識別子。

- `InvalidExpression` ：入力された式が無効。

- `UnknownOperator` ：式に未知の演算子が含まれている。

- `UndefinedVariable` ：未定義の変数が式に含まれている。

- `InvalidVariableType` ：不正な変数タイプが式に含まれている。
