# UIBase

継承: [MonoBehaviour](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.html)

## 説明

UI管理の基底クラスで、UIを開閉したり、ボタンのコールバックメソッドをバインドするためのメソッドを提供します。

## 静的メソッド

|[OpenUI](#uibaseopenui)|指定したUIを開く。|
|:---|:---|
|[CloseUI](#uibasecloseui)|指定したUIを閉じる。|
|[GetInstantiatedUI](#uibasegetinstantiatedui)|インスタンス化されたUIを取得する。|
|[BindButtonPressedCallback](#uibasebindbuttonpressedcallback)|ボタンが押されたイベントにコールバックメソッドをバインドする。|

---

# UIBase.OpenUI

`public static GameObject OpenUI(string ui)`

## パラメータ

| `ui` | 開くUI。AddressablesでUIプレハブのアドレスを手動で変更した場合、そのアドレスを渡します。変更していない場合は、拡張子を除いたUIプレハブのファイル名を渡します。 |
| :---  | ---                                                                                              |

## 説明

指定されたUIを現在のシーンに読み込み、インスタンス化します。

## 戻り値

インスタンス化が成功した場合、またはすでにUIが開かれている場合、そのインスタンスを返します。それ以外の場合は `null` を返します。

---

# UIBase.CloseUI

`public static void CloseUI(string ui, bool destroy = false)`

## パラメータ

| `ui`     | 閉じるUI。AddressablesでUIプレハブのアドレスを手動で変更した場合、そのアドレスを渡します。変更していない場合は、拡張子を除いたUIプレハブのファイル名を渡します。 |
| :-------  | :---                                                                                                  |
| `destroy`| （オプション）UIインスタンスを破棄するかどうか。デフォルトは`false`。`true`を設定すると、UIを破棄し、そのリソースを解放します。|

## 説明

`destroy` が `false` の場合、シーンから指定したUIを非表示にします。それ以外の場合は、UIを破棄してリソースを解放します。

---

# UIBase.GetInstantiatedUI

`public static GameObject GetInstantiatedUI(string ui)`

## パラメータ

| `ui` | 取得するUI。AddressablesでUIプレハブのアドレスを手動で変更した場合、そのアドレスを渡します。変更していない場合は、拡張子を除いたUIプレハブのファイル名を渡します。 |
| :---  | :---                                                                                              |

## 説明

指定されたUIのインスタンスを取得します。

## 戻り値

UIが見つかった場合、そのインスタンスを返します。それ以外の場合は `null` を返します。

---

# UIBase.BindButtonPressedCallback

`public static void BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)`

## オーバーロード

| `BindButtonPressedCallback(string buttonName, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | ボタン名称でボタンの押下イベントにコールバックをバインドします。 |
| :---------------------------------------------------------------------------- | :----------------------------------------------------------- |
| `BindButtonPressedCallback(string[] buttonNames, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | 複数のボタンの押下イベントに同一のコールバックをバインドします。 |
| `BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | ボタンのGameObjectインスタンスでボタン押下イベントにコールバックをバインドします。 |
| `BindButtonPressedCallback(GameObject[] buttonObjects, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | 複数のボタンのGameObjectインスタンスで押下イベントに同一のコールバックをバインドします。 |
| `BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | ボタンのコンポーネントインスタンスでボタン押下イベントにコールバックをバインドします。 |
| `BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | 複数のボタンコンポーネントインスタンスで押下イベントに同一のコールバックをバインドします。 |

## パラメータ

| `buttonName`    | ボタンの名前。                                 |
| :--------------- | :---------------------------------------------- |
| `buttonObject`  | ボタンのGameObjectインスタンス。               |
| `button`        | ボタンのコンポーネントインスタンス。           |
| `buttonNames`   | ボタン名の配列。                              |
| `buttonObjects` | ボタンGameObjectインスタンスの配列。           |
| `buttons`       | ボタンコンポーネントインスタンスの配列。       |
| `methodName`    | ターゲットメソッドの名前。                     |
| `target`        | ターゲットメソッドを含むクラスのインスタンス。`this` を使う場合は現在のクラスインスタンスを渡します。 |
| `includeButtonInstance` | ボタンのGameObjectインスタンスをターゲットメソッドの最初のパラメータとして渡すかどうか。`true` にすると、ターゲットメソッドの最初のパラメータは `GameObject` 型でなければなりません。 |
| `parameters`    | ターゲットメソッドで必要なパラメータ。          |

## 説明

ボタンの異なる参照に基づいてコールバックメソッドをバインドします。

ボタンインスタンスを取得できる場合は、名前ベースのオーバーロードを使用するよりも、ボタンインスタンスを使用することが推奨されます。なぜなら、名前でボタンを検索することはパフォーマンスに一定のオーバーヘッドを生じさせるからです。