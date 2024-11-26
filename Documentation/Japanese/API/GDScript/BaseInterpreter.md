# BaseInterpreter

継承：[Object](https://docs.godotengine.org/ja/stable/classes/class_object.html)

派生：[ConditionInterpreter](ConditionInterpreter.md), [VariableInterpreter](VariableInterpreter.md)

## 説明

`BaseInterpreter`は`Genscript`のコアインタープリタークラスで、主にスクリプト内容をコマンドに分割し、コメントを削除する役割を担います。また、コマンドラインでの[コンソールコマンド](../../Genscript/Category/Console.md)の処理を提供します、いくつかの一般的な文字列処理ユーティリティメソッドを含んでいます。

## 静的メソッド

|[parse_script](#baseinterpreterparse_script)|スクリプトファイル全体をコマンドに分割し、コメントを除外します。|
|:--|:--|
|[handle_debug_output](#baseinterpreterhandle_debug_output)|コンソール出力コマンドを処理します。|
|[replace_placeholders](#baseinterpreterreplace_placeholders)|テキスト中のエスケープ文字と変数を置換します。|
|[try_parse_str_to_bool](#baseinterpretertry_parse_str_to_bool)|文字列をブール値ですとして解析しようとします。|
|[try_parse_numeric](#baseinterpretertry_parse_numeric)|文字列を数値として解析しようとします。|

---

# BaseInterpreter.parse_script

`static func parse_script(script_content: String, node: Node) -> void`  

## パラメーター

|`script_content`|スクリプトリーダーが取得したスクリプトのテキスト内容。|
|:--|:--|
|`node`|自動読み込みスクリプト初期化ノードにマウントされている`Node`オブジェクト。|

## 説明

渡されたスクリプトの内容をコマンドに分割し、各行を処理します。コメントは削除され、各コマンド行は[コマンド実行器](ScriptExecutor.md)に渡され、さらに実行されます。特に、[マルチラインログコマンド](../../Genscript/Category/Console.md/#-n)を処理し、マルチラインログモードが終了するまで各コマンドを処理し続けます。

`node`パラメーターは自動読み込みスクリプト初期化ノードにマウントされた`Node`オブジェクトとして渡す必要があります。これにより、後続のスクリプト実行コンテキストをサポートします。通常は、自動読み込みスクリプト初期化ノード、または他の自動読み込みノードとしてマウントされた任意のノードを渡すべきです。

---

# BaseInterpreter.handle_debug_output

`static func handle_debug_output(code: String) -> void`

## パラメーター

|`code`|[コマンド実行器](ScriptExecutor.md)によってさらに解析されたコマンド行。|
|---|---|

## 説明

`Genscript`のコンソール命令インタープリターで、コンソール命令を解析して実行します。マルチラインログ出力、エスケープ文字の置換、数学的な式の計算（単一行コンソール出力のみ）、および変数のインターポレーションをサポートします。

---

# BaseInterpreter.replace_placeholders

`static func replace_placeholders(input: String) -> String`

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

# BaseInterpreter.try_parse_str_to_bool

`static func try_parse_str_to_bool(value: String) -> Dictionary`

## パラメーター

|`input`|ブール値に解析する文字列。|
|---|---|

## 説明

入力文字列を（大文字と小文字を区別せずに）ブール値として解析し、結果を返します。

## 戻り値

`result` と `success` フィールドを含む辞書を返します。  

解析が成功した場合、`result` には解析されたブール値が含まれ、`success` は `true` です。  

失敗した場合、`result` は `null`、`success` は `false` です。

---

# BaseInterpreter.try_parse_numeric

`static func try_parse_numeric(input: String) -> Dictionary`

## パラメーター

|`input`|数値に解析する文字列。|
|---|---|

## 説明

入力文字列を整数または浮動小数点数として解析し、結果を返します。

## 戻り値

`result` と `success` フィールドを含む辞書を返します。  

解析が成功した場合、`result` には数値（整数または浮動小数点数）が含まれ、`success` は `true` です。  

失敗した場合、`result` は `null`、`success` は `false` です。
