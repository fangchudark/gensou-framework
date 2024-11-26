# UIBase

Inherits: [Node](https://docs.godotengine.org/en/stable/classes/class_node.html)

## Description

The base class for UI management, providing methods to open and close UIs, and bind button callbacks.

## Static Methods

|[OpenUI](#uibaseopenui)|Open the specified UI.|
|:---|:---|
|[CloseUI](#uibasecloseui)|Close the specified UI.|
|[GetInstantiatedUI](#uibasegetinstantiatedui)|Get the instantiated UI.|
|[BindButtonPressedCallback](#uibasebindbuttonpressedcallback)|Bind the callback method for button press signal.|

---

# UIBase.OpenUI

`public static Control OpenUI(string ui, Node node)`

## Parameters

|`ui`|The UI to open. The UI should be located under `res://UI/`. Pass the UI scene file name without extension.|
|:---|:---|
|`node`|The target root node. The UI will be added as a child of this node. For adding to the current scene, use `GetTree().CurrentScene`.|

## Description

Load and instantiate the specified UI as a child of the specified node.

## Returns

Returns the instance of the UI if instantiation is successful or the UI is already open; otherwise, returns `null`.

---

# UIBase.CloseUI

`public static void CloseUI(string ui, bool destroy = false)`

## Parameters

|`ui`|The UI to close. The UI should be located under `res://UI/`. Pass the UI scene file name without extension.|
|:---|:---|
|`destroy`|(Optional) Whether to destroy the UI instance. Defaults to `false`. Set to `true` to destroy the UI and free its resources.|

## Description

When `destroy` is `false`, the specified UI is hidden from the scene. Otherwise, destroys and releases the UI.

---

# UIBase.GetInstantiatedUI

`public static Control GetInstantiatedUI(string ui)`

## Parameters

|`ui`|The UI to get. Pass the UI scene file name without extension.|
|:---|:---|

## Description

Get the specified instantiated UI.

## Returns

Returns the instance if the UI is found; otherwise, returns `null`.

---

# UIBase.BindButtonPressedCallback

`public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)`

## Overloads

|`public static void BindButtonPressedCallback(string buttonName, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)`|Bind the callback method for a button press signal based on button name.|
|:---|:---|
|`public static void BindButtonPressedCallback(string buttonName, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)`|Bind the callback method for multiple buttons press signal based on button name.|
|`public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|Bind the callback method for a button press signal on button instance.|
|`public static void BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|Bind the callback method for multiple buttons press signal based on button instances.|

## Parameters

|`buttonName`|The name of the button.|
|:---|:---|
|`button`|The button node instance.|
|`buttonNames`|An array of strings containing multiple button names.|
|`buttons`|An array of button node instances.|
|`methodName`|The name of the target method.|
|`target`|The instance of the class that owns the target method. Use `this` for the current class, or the instance of another class. If using the name-based overload, the target class must inherit from `Node`.|
|`includeButtonInstance`|Whether to pass the button node instance as the first parameter to the target method. If `true`, the target method's first parameter must be of type `Button`.|
|`parameters`|Zero or more parameters required by the target method.|

## Description

Bind the callback method for button press signal based on different button references.

If the button instance is available, it is recommended to use the button instance overload rather than the button name-based overload, as searching for buttons by name incurs some performance overhead.

Button name-based search will look up buttons within the `target` node's node tree, but it cannot find buttons in sibling nodes or child nodes of other nodes. **If the node tree is complex, using this method may cause significant performance degradation.**
