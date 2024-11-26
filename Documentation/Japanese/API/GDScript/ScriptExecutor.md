# ScriptExecutor

継承：[Object](https://docs.godotengine.org/ja/stable/classes/class_object.html)

## 説明

`ScriptExecutor`は`Genscript`のコマンド実行器クラスです。これはブリッジ層として、[基本的なインタープリタ](BaseInterpreter.md)によって解析されたコマンドを受け取り、さらにそのコマンドを解析して、適切なコマンドインタープリタに処理と実行を渡します。

## 静的メソッド

|[execute_command](#scriptexecutorexecute_command)|指定されたコマンドをさらに解析して実行します。|
|:---|:---|

---

# ScriptExecutor.execute_command

`static func execute_command(command: String, node: Node) -> void`

## パラメータ

|`command`|[基本的なインタープリタ](BaseInterpreter.md)によって解析されたコマンドライン。|
|:---|:---|
|`node`|自動読み込みのスクリプト初期化ノードにアタッチされた`Node`オブジェクト。|

## 説明

[基本的なインタープリタ](BaseInterpreter.md)によって解析された、コメントを除外したコマンドラインをさらに解析し、実行します。最初にコマンド内の条件付きパラメータを処理し、その後コマンドのタイプに基づいてさらなる解析と処理を行います。

`node`パラメータには自動読み込みスクリプト初期化ノードにアタッチされた`Node`オブジェクトを渡す必要があります。これにより、その後のスクリプト実行の文脈がサポートされます。通常、自動読み込みスクリプト初期化ノードにアタッチされたノード、または自動読み込みされる任意のノードを渡すべきです。
