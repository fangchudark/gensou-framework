# UIBase

Inherits: [MonoBehaviour](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.html)

## Description

The base class for UI management, providing methods to open and close UIs, and bind button callbacks.

## Static Methods

|[OpenUI](#uibaseopenui)|Open the specified UI.|
|:---|:---|
|[CloseUI](#uibasecloseui)|Close the specified UI.|
|[GetInstantiatedUI](#uibasegetinstantiatedui)|Get the instantiated UI.|
|[BindButtonPressedCallback](#uibasebindbuttonpressedcallback)|Bind the callback method for button press events.|

---

# UIBase.OpenUI

`public static GameObject OpenUI(string ui)`

## Parameters

| `ui` | The UI to open. If the UI prefab address is manually modified in Addressables, pass the address. Otherwise, pass the UI prefab file name without the file extension. |
| :---  | :---                                                                                              |

## Description

Loads and instantiates the specified UI into the current scene.

## Returns

Returns the instance of the UI if instantiation is successful or the UI is already open; otherwise, returns `null`.

---

# UIBase.CloseUI

`public static void CloseUI(string ui, bool destroy = false)`

## Parameters

| `ui`     | The UI to close. If the UI prefab address is manually modified in Addressables, pass the address. Otherwise, pass the UI prefab file name without the file extension. |
| :-------  | :---                                                                                                  |
| `destroy`| (Optional) Whether to destroy the UI instance. Default is `false`. Set to `true` to destroy the UI and free its resources.                            |

## Description

When `destroy` is `false`, hides the specified UI from the scene. Otherwise, destroys and releases the UI.

---

# UIBase.GetInstantiatedUI

`public static GameObject GetInstantiatedUI(string ui)`

## Parameters

| `ui` | The UI to get. If the UI prefab address is manually modified in Addressables, pass the address. Otherwise, pass the UI prefab file name without the file extension. |
| :---  | :---                                                                                              |

## Description

Get the specified instantiated UI.

## Returns

Returns the instance if the UI is found; otherwise, returns `null`.

---

# UIBase.BindButtonPressedCallback

`public static void BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)`

## Overloads

| `BindButtonPressedCallback(string buttonName, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to a button press event using the button's name. |
| :--------------------------------------------------------------------------------- | :------------------------------------------------------------------ |
| `BindButtonPressedCallback(string[] buttonNames, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to multiple buttons' press events.              |
| `BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to a button press event using the button object instance. |
| `BindButtonPressedCallback(GameObject[] buttonObjects, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to multiple buttons' press events using their object instances. |
| `BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to a button press event using the button component instance. |
| `BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)` | Binds a callback to multiple buttons' press events using their component instances. |

## Parameters

| `buttonName`    | The name of the button.                          |
| :--------------- | :------------------------------------------------ |
| `buttonObject`  | The GameObject instance of the button.          |
| `button`        | The button component instance.                  |
| `buttonNames`   | An array of strings containing multiple button names.|
| `buttonObjects` | An array of button GameObject instances.        |
| `buttons`       | An array of button component instances.         |
| `methodName`    | The name of the target method.                  |
| `target`        | The instance of the class that owns the target method. Use `this` for the current class, or the instance of another class. |
| `includeButtonInstance` | Whether to pass the button's GameObject instance as the first parameter to the target method. If `true`, the first parameter of the target method must be of type `GameObject`. |
| `parameters`    | Zero or more parameters required by the target method. |

## Description

Bind the callback method for button press events based on different button references.

If the button instance is available, it is recommended to use the button instance overload rather than the button name-based overload, as searching for buttons by name incurs some performance overhead.
