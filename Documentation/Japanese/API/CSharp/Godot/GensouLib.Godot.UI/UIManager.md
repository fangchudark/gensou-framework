# UIManager

## 説明

UI管理器、UIを開閉したり、ボタンのコールバックメソッドをバインドするためのメソッドを提供します。

## 静的プロパティ

|[ResPath](#uimanagerrespath)| リソースパス。 |
|:---|:---|

## 静的メソッド

|[OpenUI](#uimanageropenui)|指定したUIを開く。|
|:---|:---|
|[CloseUI](#uimanagercloseui)|指定したUIを閉じる。|
|[GetInstantiatedUI](#uimanagergetinstantiatedui)|インスタンス化されたUIを取得する。|
|[BindButtonPressedCallback](#uimanagerbindbuttonpressedcallback)|ボタンが押されたシグナルにコールバックメソッドをバインドする。|

---

# UIManager.ResPath

`public static string ResPath`

## 説明

リソース パス。デフォルトは `res://UI/` です。UI シーン ファイルはこのパスにロードされます。

---

# UIManager.OpenUI

`public static Control OpenUI(string ui, Node node)`

## パラメーター

|`ui`|開くUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|
|`node`|ターゲットのルートノード。UIはこのノードの子として追加されます。現在のシーンに追加する場合は`GetTree().CurrentScene`を使用します。|

## 説明

指定したUIを指定したノードの子としてロードおよびインスタンス化します。

## 戻り値

インスタンス化が成功した場合、またはすでにUIが開かれている場合、そのインスタンスを返します。それ以外の場合は `null` を返します。

---

# UIManager.CloseUI

`public static void CloseUI(string ui, bool destroy = false)`

## パラメーター

|`ui`|閉じるUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|
|`destroy`|（オプション）UIインスタンスを破棄するかどうか。デフォルトは`false`。`true`を設定すると、UIを破棄し、そのリソースを解放します。|

## 説明

`destroy` が `false` の場合、シーンから指定したUIを非表示にします。それ以外の場合は、UIを破棄してリソースを解放します。

---

# UIManager.GetInstantiatedUI

`public static Control GetInstantiatedUI(string ui)`

## パラメーター

|`ui`|取得したいUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|

## 説明

指定したインスタンス化されたUIを取得します。

## 戻り値

UIが見つかればそのインスタンスを返します。見つからなければnullを返します。

---

# UIManager.BindButtonPressedCallback

`public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)`

## オーバーロード

|`public static void BindButtonPressedCallback(string buttonName, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)`|ボタン名に基づいてボタン押下シグナルにコールバックメソッドをバインドする。|
|:---|:---|
|`public static void BindButtonPressedCallback(string buttonName, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)`|ボタン名に基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。|
|`public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|ボタンインスタンスに基づいてボタン押下シグナルにコールバックメソッドをバインドする。|
|`public static void BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|ボタンインスタンスに基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。|

## パラメーター

|`buttonName`|ボタンの名前。|
|:---|:---|
|`button`|ボタンノードインスタンス。|
|`buttonNames`|複数のボタン名を含む文字列の配列。|
|`buttons`|複数のボタンノードインスタンスを含む配列。|
|`methodName`|ターゲットメソッドの名前。|
|`target`|ターゲットメソッドを所有するクラスのインスタンス。現在のクラスでは`this`を使用し、他のクラスではそのインスタンスを渡します。名前に基づくオーバーロードを使用する場合、ターゲットクラスは`Node`を継承している必要があります。|
|`includeButtonInstance`|ターゲットメソッドにボタンノードインスタンスを最初のパラメーターとして渡すかどうか。`true`の場合、ターゲットメソッドの最初のパラメーターは`Button`型である必要があります。|
|`parameters`|ターゲットメソッドに必要なパラメーター。|

## 説明

ボタンの異なる参照に基づいてコールバックメソッドをバインドします。

ボタンインスタンスを取得できる場合は、名前ベースのオーバーロードを使用するよりも、ボタンインスタンスを使用することが推奨されます。なぜなら、名前でボタンを検索することはパフォーマンスに一定のオーバーヘッドを生じさせるからです。

名前ベースの検索では、ターゲット(`target`)ノードのノードツリー内でボタンを上下に探しますが、兄弟ノードや他のノードの子ノードは探せません。**ノードツリーが複雑な場合、この方法はパフォーマンスの大幅な低下を引き起こす可能性があります。**
