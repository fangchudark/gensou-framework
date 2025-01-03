# BaseInterpreter

派生：[ConditionInterpreter](ConditionInterpreter.md), [VariableInterpreter](VariableInterpreter.md)

## 説明

`BaseInterpreter`は`Genscript`のコアインタープリタークラスで、主にスクリプト内容をコマンドに分割し、コメントを削除する役割を担います。また、いくつかの一般的な文字列処理ユーティリティメソッドを含んでいます。

## 静的メソッド

|[ParseScript](#baseinterpreterparsescript)|スクリプトファイル全体をコマンドに分割し、コメントを除外します。|
|:--|:--|
|[ReplacePlaceholders](#baseinterpreterreplaceplaceholders)|テキスト中のエスケープ文字と変数を置換します。|
|[TryParseNumeric](#baseinterpretertryparsenumeric)|文字列を数値として解析しようとします。|

---

# BaseInterpreter.ParseScript

`public static void ParseScript(string scriptContent)`  
`public static void ParseScript(string scriptContent, Node node)`

## パラメーター

|`scriptContent`|スクリプトリーダーが取得したスクリプトのテキスト内容。|
|:--|:--|
|`node`|（Godotプラットフォーム）自動読み込みスクリプト初期化ノードにマウントされている`Node`オブジェクト。|

## 説明

渡されたスクリプトの内容をコマンドに分割し、各行を処理します。コメントは削除され、各コマンド行は[コマンド実行器](ScriptExecutor.md)に渡され、さらに実行されます。特に、[マルチラインログコマンド](../../../Genscript/Category/Console.md/#-n)を処理し、マルチラインログモードが終了するまで各コマンドを処理し続けます。

**Godot**プラットフォームで使用する場合、`node`パラメーターは自動読み込みスクリプト初期化ノードにマウントされた`Node`オブジェクトとして渡す必要があります。これにより、後続のスクリプト実行コンテキストをサポートします。通常は、自動読み込みスクリプト初期化ノード、または他の自動読み込みノードとしてマウントされた任意のノードを渡すべきです。

---

# BaseInterpreter.ReplacePlaceholders

`public static string ReplacePlaceholders(string input)`

## パラメーター

|`input`|置換するプレースホルダを含む文字列。|
|---|---|

## 説明

入力文字列内のプレースホルダを置換します。定義されたエスケープ文字を処理し、`Genscript`内で定義された有効な変数を対応する値に置き換えることをサポートします。

メソッド内で定義されたエスケープ文字：

|エスケープ文字|置換内容|  
|---|---|  
|`\}`|`}`に置換|  
|`\{`|`{`に置換|  
|`\/`|`/`に置換|  
|`\+`|`+`に置換|  
|`\-`|`-`に置換|  
|`\*`|`*`に置換|  
|`\%`|`%`に置換|  
|`\\`|`\`に置換|  
|`\"`|`"`に置換|  

---

# BaseInterpreter.TryParseNumeric

`public static bool TryParseNumeric(string input, out object result)`

## パラメーター

|`input`|数値に解析する文字列。|
|---|---|
|`result`|文字列を解析して得られる数値。解析に失敗した場合、`null`になります。|

## 説明

入力文字列を整数または浮動小数点数として解析し、結果を返します。


## 戻り値

文字列が有効な整数または浮動小数点数形式であれば、`true`と対応する数値を返します。それ以外の場合は、`false`と`null`を返します。
