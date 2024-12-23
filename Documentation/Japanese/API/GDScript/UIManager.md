# UIManager

継承: [Object](https://docs.godotengine.org/ja/stable/classes/class_object.html)

## 説明

UI管理器、UIを開閉したり、ボタンのコールバックメソッドをバインドするためのメソッドを提供します。

## 静的プロパティ

|[res_path](#uimanagerres_path)| リソースパス。 |
|:---|:---|

## 静的メソッド

|[open_ui](#uimanageropen_ui)|指定したUIを開く。|
|:---|:---|
|[close_ui](#uimanagerclose_ui)|指定したUIを閉じる。|
|[get_instantiated_ui](#uimanagerget_instantiated_ui)|インスタンス化されたUIを取得する。|
|[bind_button_pressed_callback_byname](#uimanagerbind_button_pressed_callback_byname)|ボタン名に基づいてボタン押下シグナルにコールバックメソッドをバインドする。|
|[bind_buttons_pressed_callback_byname](#uimanagerbind_buttons_pressed_callback_byname)|ボタン名に基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。|
|[bind_button_pressed_callback](#uimanagerbind_button_pressed_callback)|ボタンインスタンスに基づいてボタン押下シグナルにコールバックメソッドをバインドする。|
|[bind_buttons_pressed_callback](#uimanagerbind_buttons_pressed_callback)|ボタンインスタンスに基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。|

---

# UIManager.res_path

`static var res_path: String`

## 説明

リソース パス。デフォルトは `res://UI/` です。UI シーン ファイルはこのパスにロードされます。

---

# UIManager.open_ui

`static func open_ui(ui: String, node: Node) -> Control`

## パラメーター

|`ui`|開くUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|
|`node`|ターゲットのルートノード。UIはこのノードの子として追加されます。現在のシーンに追加する場合は`get_tree().current_scene`を使用します。|

## 説明

指定したUIを指定したノードの子としてロードおよびインスタンス化します。

## 戻り値

インスタンス化が成功した場合、またはすでにUIが開かれている場合、そのインスタンスを返します。それ以外の場合は `null` を返します。

---

# UIManager.close_ui

`static func close_ui(ui: String, destroy: bool = false) -> void`

## パラメーター

|`ui`|閉じるUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|
|`destroy`|（オプション）UIインスタンスを破棄するかどうか。デフォルトは`false`。`true`を設定すると、UIを破棄し、そのリソースを解放します。|

## 説明

`destroy` が `false` の場合、シーンから指定したUIを非表示にします。それ以外の場合は、UIを破棄してリソースを解放します。

---

# UIManager.get_instantiated_ui

`static func get_instantiated_ui(ui: String) -> Control`

## パラメーター

|`ui`|取得したいUI。拡張子を除いたUIシーンのファイル名を渡します。|
|:---|:---|

## 説明

指定したインスタンス化されたUIを取得します。

## 戻り値

UIが見つかればそのインスタンスを返します。見つからなければnullを返します。

---

# UIManager.bind_button_pressed_callback_byname

`static func bind_button_pressed_callback_byname(button_name: String, method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## パラメーター

|`button_name`|ボタンの名前。|
|:---|:---|
|`method_name`|ターゲットメソッドの名前。|
|`target`|ターゲットメソッドを所有するクラスのインスタンス。現在のクラスでは `self`を使用し、他のクラスではそのインスタンスを渡します。ターゲットクラスは`Node`を継承している必要があります。|
|`include_button_instance`|ターゲットメソッドにボタンノードインスタンスを最初のパラメーターとして渡すかどうか。`true`の場合、ターゲットメソッドの最初のパラメーターは`Button`型である必要があります。|
|`parameters`|ターゲットメソッドに必要なパラメーター。|

## 説明

ボタン名に基づいてボタン押下シグナルにコールバックメソッドをバインドする。

ボタンインスタンスを取得できる場合は、名前ベースのオーバーロードを使用するよりも、ボタンインスタンスを使用することが推奨されます。なぜなら、名前でボタンを検索することはパフォーマンスに一定のオーバーヘッドを生じさせるからです。

名前ベースの検索では、ターゲット(`target`)ノードのノードツリー内でボタンを上下に探しますが、兄弟ノードや他のノードの子ノードは探せません。**ノードツリーが複雑な場合、この方法はパフォーマンスの大幅な低下を引き起こす可能性があります。**

---

# UIManager.bind_buttons_pressed_callback_byname

`static func bind_buttons_pressed_callback_byname(button_names: Array[String], method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## パラメーター

|`button_names`|複数のボタン名を含む文字列の配列。|
|:---|:---|
|`method_name`|ターゲットメソッドの名前。|
|`target`|ターゲットメソッドを所有するクラスのインスタンス。現在のクラスでは `self`を使用し、他のクラスではそのインスタンスを渡します。ターゲットクラスは`Node`を継承している必要があります。|
|`include_button_instance`|ターゲットメソッドにボタンノードインスタンスを最初のパラメーターとして渡すかどうか。`true`の場合、ターゲットメソッドの最初のパラメーターは`Button`型である必要があります。|
|`parameters`|ターゲットメソッドに必要なパラメーター。|

## 説明

ボタン名に基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。

ボタンインスタンスを取得できる場合は、名前ベースのオーバーロードを使用するよりも、ボタンインスタンスを使用することが推奨されます。なぜなら、名前でボタンを検索することはパフォーマンスに一定のオーバーヘッドを生じさせるからです。

名前ベースの検索では、ターゲット(`target`)ノードのノードツリー内でボタンを上下に探しますが、兄弟ノードや他のノードの子ノードは探せません。**ノードツリーが複雑な場合、この方法はパフォーマンスの大幅な低下を引き起こす可能性があります。**

---

# UIManager.bind_button_pressed_callback

`static func bind_button_pressed_callback(button: Button, method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## パラメーター

|`button`|ボタンノードインスタンス。|
|:---|:---|
|`method_name`|ターゲットメソッドの名前。|
|`target`|ターゲットメソッドを所有するクラスのインスタンス。現在のクラスでは `self`を使用し、他のクラスではそのインスタンスを渡します。|
|`include_button_instance`|ターゲットメソッドにボタンノードインスタンスを最初のパラメーターとして渡すかどうか。`true`の場合、ターゲットメソッドの最初のパラメーターは`Button`型である必要があります。|
|`parameters`|ターゲットメソッドに必要なパラメーター。|

## 説明

ボタンインスタンスに基づいてボタン押下シグナルにコールバックメソッドをバインドする。

---

# UIManager.bind_buttons_pressed_callback

`static func bind_buttons_pressed_callback(buttons: Array[Button], method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## パラメーター

|`buttons`|複数のボタンノードインスタンスを含む配列。|
|:---|:---|
|`method_name`|ターゲットメソッドの名前。|
|`target`|ターゲットメソッドを所有するクラスのインスタンス。現在のクラスでは `self`を使用し、他のクラスではそのインスタンスを渡します。|
|`include_button_instance`|ターゲットメソッドにボタンノードインスタンスを最初のパラメーターとして渡すかどうか。`true`の場合、ターゲットメソッドの最初のパラメーターは`Button`型である必要があります。|
|`parameters`|ターゲットメソッドに必要なパラメーター。|

## 説明

ボタンインスタンスに基づいて複数のボタン押下シグナルに同じコールバックメソッドをバインドする。
