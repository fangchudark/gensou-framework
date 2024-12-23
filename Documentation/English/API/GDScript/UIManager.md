# UIManager

Inherits: [Object](https://docs.godotengine.org/en/stable/classes/class_object.html)

## Description

UI Manager, providing methods to open and close UIs, and bind button callbacks.

## Static properties

|[res_path](#uimanagerres_path)| Resource path. |
|:---|:---|

## Static Methods

|[open_ui](#uimanageropen_ui)|Open the specified UI.|
|:---|:---|
|[close_ui](#uimanagerclose_ui)|Close the specified UI.|
|[get_instantiated_ui](#uimanagerget_instantiated_ui)|Get the instantiated UI.|
|[bind_button_pressed_callback_byname](#uimanagerbind_button_pressed_callback_byname)|Bind the callback method for a button press signal based on button name.|
|[bind_buttons_pressed_callback_byname](#uimanagerbind_buttons_pressed_callback_byname)|Bind the callback method for multiple buttons press signal based on button name.|
|[bind_button_pressed_callback](#uimanagerbind_button_pressed_callback)|Bind the callback method for a button press signal on button instance.|
|[bind_buttons_pressed_callback](#uimanagerbind_buttons_pressed_callback)|Bind the callback method for multiple buttons press signal based on button instances.|

---

# UIManager.res_path

`static var res_path: String`

## Description

Resource path, defaults to `res://UI/` , where the UI scene files will be loaded.

---

# UIManager.open_ui

`static func open_ui(ui: String, node: Node) -> Control`

## Parameters

|`ui`|The UI to open. Pass the UI scene file name without extension.|
|:---|:---|
|`node`|The target root node. The UI will be added as a child of this node. For adding to the current scene, use`get_tree().current_scene`。|

## Description

Load and instantiate the specified UI as a child of the specified node.

## Returns

Returns the instance of the UI if instantiation is successful or the UI is already open; otherwise, returns `null`.

---

# UIManager.close_ui

`static func close_ui(ui: String, destroy: bool = false) -> void`

## Parameters

|`ui`|The UI to close. Pass the UI scene file name without extension.|
|:---|:---|
|`destroy`|(Optional) Whether to destroy the UI instance. Defaults to `false`. Set to `true` to destroy the UI and free its resources.|

## Description

When `destroy` is `false`, the specified UI is hidden from the scene. Otherwise, destroys and releases the UI.

---

# UIManager.get_instantiated_ui

`static func get_instantiated_ui(ui: String) -> Control`

## Parameters

|`ui`|The UI to get. Pass the UI scene file name without extension.|
|:---|:---|

## Description

Get the specified instantiated UI.

## Returns

Returns the instance if the UI is found; otherwise, returns `null`.

---

# UIManager.bind_button_pressed_callback_byname

`static func bind_button_pressed_callback_byname(button_name: String, method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## Parameters

|`button_name`|The name of the button.|
|:---|:---|
|`method_name`|The name of the target method.|
|`target`|The instance of the class that owns the target method. Use `self` for the current class, or the instance of another class. The target class must inherit from `Node`.|
|`include_button_instance`|Whether to pass the button node instance as the first parameter to the target method. If `true`, the target method's first parameter must be of type `Button`.|
|`parameters`|Zero or more parameters required by the target method.|

## Description

Bind the callback method for a button press signal based on button name.

If the button instance is available, it is recommended to use the button instance overload rather than the button name-based overload, as searching for buttons by name incurs some performance overhead.

Button name-based search will look up buttons within the `target` node's node tree, but it cannot find buttons in sibling nodes or child nodes of other nodes. **If the node tree is complex, using this method may cause significant performance degradation.**

---

# UIManager.bind_buttons_pressed_callback_byname

`static func bind_buttons_pressed_callback_byname(button_names: Array[String], method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void`

## Parameters

|`button_names`|An array of strings containing multiple button names.|
|:---|:---|
|`method_name`|The name of the target method.|
|`target`|The instance of the class that owns the target method. Use `self` for the current class, or the instance of another class. The target class must inherit from `Node`.|
|`include_button_instance`|Whether to pass the button node instance as the first parameter to the target method. If `true`, the target method's first parameter must be of type `Button`.|
|`parameters`|Zero or more parameters required by the target method.|

## Description

Bind the callback method for multiple buttons press signal based on button name.

If the button instance is available, it is recommended to use the button instance overload rather than the button name-based overload, as searching for buttons by name incurs some performance overhead.

Button name-based search will look up buttons within the `target` node's node tree, but it cannot find buttons in sibling nodes or child nodes of other nodes. **If the node tree is complex, using this method may cause significant performance degradation.**

---

# UIManager.bind_button_pressed_callback

`static func bind_button_pressed_callback(button: Button, method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## Parameters

|`button`|The button node instance.|
|:---|:---|
|`method_name`|The name of the target method.|
|`target`|The instance of the class that owns the target method. Use `self` for the current class, or the instance of another class.|
|`include_button_instance`|Whether to pass the button node instance as the first parameter to the target method. If `true`, the target method's first parameter must be of type `Button`.|
|`parameters`|Zero or more parameters required by the target method.|

## Description

Bind the callback method for a button press signal on button instance.

---

# UIManager.bind_buttons_pressed_callback

`static func bind_buttons_pressed_callback(buttons: Array[Button], method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void`

## Parameters

|`buttons`|An array of button node instances.|
|:---|:---|
|`method_name`|The name of the target method.|
|`target`|The instance of the class that owns the target method. Use `self` for the current class, or the instance of another class.|
|`include_button_instance`|Whether to pass the button node instance as the first parameter to the target method. If `true`, the target method's first parameter must be of type `Button`.|
|`parameters`|Zero or more parameters required by the target method.|

## Description

Bind the callback method for multiple buttons press signal based on button instances.
