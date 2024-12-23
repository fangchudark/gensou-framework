# ScriptReader

## 説明

`Genscript` スクリプトを読み込むためのユーティリティクラス。

## 静的メソッド

|[ReadScript](#scriptreaderreadscript)|指定したファイルパスからスクリプトを読み込みます。|
|:---|:---|
|[ReadAndExecute](#scriptreaderreadandexecute)|指定した名前のスクリプトを読み込み、実行します。|

---

# ScriptReader.ReadScript

`public static string ReadScript(string filePath)`

## パラメーター

|`filePath`|読み込むスクリプトファイルのパス。|
|:---|:---|

## 説明

指定された `filePath` にある `Genscript` スクリプトファイルを読み込み、その内容を文字列として返します。

このメソッドは `Genscript` スクリプトファイルを読み込むために設計されていますが、必ずしもそのファイルタイプに限定されるわけではありません。任意のテキストファイルも読み込むことができます。

ファイルパスが無効であるか、読み込みに失敗した場合（例えばファイルが存在しない、または権限が不足している場合）、メソッドは `null` を返します。

## 戻り値

ファイルの読み込みが成功した場合、読み取ったスクリプトの内容を文字列として返します。それ以外の場合は `null` を返します。

---

# ScriptReader.ReadAndExecute

`public static void ReadAndExecute(string script="start")`  
`public static void ReadAndExecute(Node node, string script="start")`

## パラメーター

|`node`|（Godotプラットフォーム）自動読み込みされるスクリプト初期化ノードに取り付けられた `Node` オブジェクト。|
|:---|:---|
|`script`|（オプション）読み込むスクリプトファイルの名前（拡張子なし）。デフォルトは `"start"`。|

## 説明

このメソッドは、指定された名前の `Genscript` スクリプトを読み込み、実行するためのもので、デフォルトでは `"start"` というスクリプトを読み込みます。

**Godot** プラットフォームで使用する場合、`node` パラメーターには自動読み込みされるスクリプト初期化ノードに取り付けられた `Node` オブジェクトを渡す必要があります。これにより、後続のスクリプト実行のためのコンテキストが提供されます。通常、これは自動読み込みされるスクリプト初期化ノードに取り付けられたノード、または他の任意の自動読み込みされるノードを指定します。

異なるプラットフォームでは、スクリプトファイルの保存場所は以下の通りです：

- **Godot** プラットフォーム：スクリプトは `res://Scripts/scriptfile.gs` に保存する必要があります。
- **Unity** プラットフォーム：スクリプトは `Assets/Scripts/scriptfile.gs` に保存する必要があります。
