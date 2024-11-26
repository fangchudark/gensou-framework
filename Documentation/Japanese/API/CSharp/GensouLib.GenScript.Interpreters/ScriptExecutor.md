# ScriptExecutor

## 説明

`ScriptExecutor`は`Genscript`のコマンド実行器クラスです。これはブリッジ層として、[基本的なインタープリタ](BaseInterpreter.md)によって解析されたコマンドを受け取り、さらにそのコマンドを解析して、適切なコマンドインタープリタに処理と実行を渡します。

## 静的メソッド

|[ExecuteCommand](#scriptexecutorexecutecommand)|指定されたコマンドをさらに解析して実行します。|
|:---|:---|

---

# ScriptExecutor.ExecuteCommand

`public static void ExecuteCommand(string command)`  
`public static void ExecuteCommand(string command, Node node)`

## パラメータ

|`command`|[基本的なインタープリタ](BaseInterpreter.md)によって解析されたコマンドライン。|
|:---|:---|
|`node`|（Godotプラットフォーム）自動読み込みのスクリプト初期化ノードにアタッチされた`Node`オブジェクト。|

## 説明

[基本的なインタープリタ](BaseInterpreter.md)によって解析された、コメントを除外したコマンドラインをさらに解析し、実行します。最初にコマンド内の条件付きパラメータを処理し、その後コマンドのタイプに基づいてさらなる解析と処理を行います。

**Godot**プラットフォームで使用する場合、`node`パラメータには自動読み込みスクリプト初期化ノードにアタッチされた`Node`オブジェクトを渡す必要があります。これにより、その後のスクリプト実行の文脈がサポートされます。通常、自動読み込みスクリプト初期化ノードにアタッチされたノード、または自動読み込みされる任意のノードを渡すべきです。
