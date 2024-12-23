# UIManager

继承：[Object](https://docs.godotengine.org/zh-cn/stable/classes/class_object.html)

## 描述

UI管理器，提供打开和关闭UI，以及绑定按钮回调的方法。

## 静态属性

|[res_path](#uimanagerres_path)|资源路径。|
|:---|:---|

## 静态方法

|[open_ui](#uimanageropen_ui)|打开指定UI。|
|:---|:---|
|[close_ui](#uimanagerclose_ui)|关闭指定UI。|
|[get_instantiated_ui](#uimanagerget_instantiated_ui)|获取已实例化的UI。|
|[bind_button_pressed_callback_byname](#uimanagerbind_button_pressed_callback_byname)|根据按钮名称为按钮按下信号绑定回调方法。|
|[bind_buttons_pressed_callback_byname](#uimanagerbind_buttons_pressed_callback_byname)|根据按钮名称为多个按钮的按下信号绑定到同一个回调方法。|
|[bind_button_pressed_callback](#uimanagerbind_button_pressed_callback)|根据按钮实例为按钮按下信号绑定回调方法。|
|[bind_buttons_pressed_callback](#uimanagerbind_buttons_pressed_callback)|根据按钮实例为多个按钮按下信号绑定到同一个回调方法。|

---

# UIManager.res_path

`static var res_path: String`

## 描述

资源路径，默认为`res://UI/`，将在该路径下加载UI场景文件。

---

# UIManager.open_ui

`static func open_ui(ui: String, node: Node) -> Control`

## 参数

|`ui`|要打开的UI。传入UI场景的文件名，不包含扩展名。|
|:---|:---|
|`node`|目标根节点,UI将被添加为该节点的子节点。添加到当前场景使用`get_tree().current_scene`。|

## 描述

加载并实例化指定的UI作为指定节点的子节点

## 返回

若实例化成功或是已开启的UI则返回该实例，否则返回`null`。

---

# UIManager.close_ui

`static func close_ui(ui: String, destroy: bool = false) -> void`

## 参数

|`ui`|要关闭的 UI。传入UI场景的文件名，不包含扩展名。|
|:---|:---|
|`destroy`|（可选）是否销毁 UI 实例，默认为  `false`。设置为 `true` 将销毁 UI 并释放其资源。|

## 描述

当 `destroy` 为 `false` 时，从场景隐藏指定UI，否则销毁指定UI

---

# UIManager.get_instantiated_ui

`static func get_instantiated_ui(ui: String) -> Control`

## 参数

|`ui`|要获取的 UI。传入UI场景的文件名，不包含扩展名。|
|:---|:---|

## 描述

获取指定的已实例化UI

## 返回

如果获取到UI，则返回其实例，否则返回`null`

---

# UIManager.bind_button_pressed_callback_byname

`static func bind_button_pressed_callback_byname(button_name: String, method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## 参数

|`button_name`|按钮名称。|
|:---|:---|
|`method_name`|目标方法的名称。|
|`target`|目标方法所属类的实例。当前类使用 `self`，其他类传入该类的实例。其他类传入该类的实例。目标类必须继承自`Node`。|
|`include_button_instance`|是否将按钮节点实例作为第一个参数传递给目标方法。如果为`true`，目标方法的第一个参数必须是`Button`类型|
|`parameters`|目标方法需要的零个或多个参数。|

## 描述

根据按钮名称为按钮按下信号绑定回调方法。

在能获取到按钮实例的情况下，不推荐使用基于按钮名称的重载，因为通过名称查找按钮会带来一定的性能损失。

基于按钮名称的查找会在目标`target`所在的节点树内上下查找按钮，但无法查找兄弟节点或其他节点的子节点。**如果节点树结构复杂，使用这种方法可能导致显著的性能损失**。

---

# UIManager.bind_buttons_pressed_callback_byname

`static func bind_buttons_pressed_callback_byname(button_names: Array[String], method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## 参数

|`button_names`|一个包含多个按钮名称的字符串数组。|
|:---|:---|
|`method_name`|目标方法的名称。|
|`target`|目标方法所属类的实例。当前类使用 `self`，其他类传入该类的实例。其他类传入该类的实例。目标类必须继承自`Node`。|
|`include_button_instance`|是否将按钮节点实例作为第一个参数传递给目标方法。如果为`true`，目标方法的第一个参数必须是`Button`类型|
|`parameters`|目标方法需要的零个或多个参数。|

## 描述

根据按钮名称为多个按钮的按下信号绑定到同一个回调方法。

在能获取到按钮实例的情况下，不推荐使用基于按钮名称的重载，因为通过名称查找按钮会带来一定的性能损失。

基于按钮名称的查找会在目标`target`所在的节点树内上下查找按钮，但无法查找兄弟节点或其他节点的子节点。**如果节点树结构复杂，使用这种方法可能导致显著的性能损失**。

---

# UIManager.bind_button_pressed_callback

`static func bind_button_pressed_callback(button: Button, method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## 参数

|`button`|按钮节点实例。|
|:---|:---|
|`method_name`|目标方法的名称。|
|`target`|目标方法所属类的实例。当前类使用 `self`，其他类传入该类的实例。其他类传入该类的实例。|
|`include_button_instance`|是否将按钮节点实例作为第一个参数传递给目标方法。如果为`true`，目标方法的第一个参数必须是`Button`类型|
|`parameters`|目标方法需要的零个或多个参数。|

## 描述

根据按钮实例为按钮按下信号绑定回调方法。

---

# UIManager.bind_buttons_pressed_callback

`static func bind_buttons_pressed_callback(buttons: Array[Button], method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## 参数

|`buttons`|一个包含多个按钮节点实例的数组。|
|:---|:---|
|`method_name`|目标方法的名称。|
|`target`|目标方法所属类的实例。当前类使用 `self`，其他类传入该类的实例。其他类传入该类的实例。|
|`include_button_instance`|是否将按钮节点实例作为第一个参数传递给目标方法。如果为`true`，目标方法的第一个参数必须是`Button`类型|
|`parameters`|目标方法需要的零个或多个参数。|

## 描述

根据按钮实例为多个按钮按下信号绑定到同一个回调方法。
