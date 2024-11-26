# ScriptConsole

## 説明

`ScriptConsole` は `Genscript` 用に設計されたクロスプラットフォーム対応のスクリプトコンソールクラスです。このクラスは、現在の実行環境（Godot または Unity）に応じて、ログメッセージやエラーメッセージを適切なプラットフォームのコンソールに自動的に出力します。開発者は、プラットフォーム固有のコンソール出力関数を呼び出す必要がありません。

## 静的メソッド

|[PrintLog](#scriptconsoleprintlog)|現在のプラットフォームのコンソールにログメッセージを出力します。|
|:---|:---|
|[PrintErr](#scriptconsoleprinterr)|現在のプラットフォームのコンソールにエラーメッセージを出力します。|
---

# ScriptConsole.PrintLog

`public static void PrintLog(params object[] message)`

## パラメータ

|`message`|コンソールに出力するログメッセージ。任意の型のパラメータを1つ以上受け取ります。|
|:---|:---|

## 説明

このメソッドは、現在使用しているプラットフォーム（Godot または Unity）のコンソールにログメッセージを出力します。

---

# ScriptConsole.PrintErr

`public static void PrintErr(params object[] message)`

## パラメータ

|`message`|コンソールに出力するエラーメッセージ。任意の型のパラメータを1つ以上受け取ります。|
|:---|:---|

## 説明

このメソッドは、現在使用しているプラットフォーム（Godot または Unity）のコンソールにエラーメッセージを出力します。Godot プラットフォームでは、エラーメッセージは**コンソールに表示されません**が、デバッガやOSターミナルに表示されます。
