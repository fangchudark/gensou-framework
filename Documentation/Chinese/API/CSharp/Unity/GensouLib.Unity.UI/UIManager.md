# UIManager

继承：[MonoBehaviour](https://docs.unity.cn/cn/2022.3/ScriptReference/MonoBehaviour.html)

## 描述

UI管理的基类，提供打开和关闭UI，以及绑定按钮回调的方法。

## 静态方法

|[OpenUI](#uimanageropenui)|打开指定UI。|
|:---|:---|
|[CloseUI](#uimanagercloseui)|关闭指定UI。|
|[GetInstantiatedUI](#uimanagergetinstantiatedui)|获取已实例化的UI|
|[BindButtonPressedCallback](#uimanagerbindbuttonpressedcallback)|为按钮按下事件绑定回调方法|

---

# UIManager.OpenUI

`public static GameObject OpenUI(string ui)`

## 参数

|`ui`|要打开的 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。|
|:---|:---|

## 描述

加载并实例化指定的UI到当前场景

## 返回

若实例化成功或是已开启的UI则返回该实例，否则返回`null`。

---

# UIManager.CloseUI

`public static void CloseUI(string ui, bool destroy = false)`

## 参数

|`ui`|要关闭的 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。|
|:---|:---|
|`destroy`|（可选）是否销毁 UI 实例，默认为  `false`。设置为 `true` 将销毁 UI 并释放其资源。|

## 描述

当 `destroy` 为 `false` 时，从场景隐藏指定UI，否则销毁指定UI

---

# UIManager.GetInstantiatedUI

`public static GameObject GetInstantiatedUI(string ui)`

## 参数

|`ui`|要获取的 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。|
|:---|:---|

## 描述

获取指定的已实例化UI

## 返回

如果获取到UI，则返回其实例，否则返回`null`

---

# UIManager.BindButtonPressedCallback

`public static void BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)`

## 重载

|`BindButtonPressedCallback(string buttonName, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮名称为按钮按下事件绑定回调方法。|
|:---|:---|
|`BindButtonPressedCallback(string[] buttonNames, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮名称为多个按钮的按下事件绑定到同一个回调方法。|
|`BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮GameObject实例为按钮按下事件绑定回调方法。|
|`BindButtonPressedCallback(GameObject[] buttonObjects, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮GameObject实例为多个按钮的按下事件绑定到同一个回调方法。|
|`BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮组件实例为按钮按下事件绑定回调方法。|
|`BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)`|根据按钮组件实例为多个按钮的按下事件绑定到同一个回调方法。|

## 参数

|`buttonName`|按钮名称。|
|:---|:---|
|`buttonObject`|按钮GameObejct实例。|
|`button`|按钮组件实例。|
|`buttonNames`|一个包含多个按钮名称的字符串数组。|
|`buttonObjects`|一个包含多个按钮GameObject实例的数组。|
|`buttons`|一个包含多个按钮组件实例的数组。|
|`methodName`|目标方法的名称。|
|`target`|目标方法所属类的实例。当前类使用 `this`，其他类传入该类的实例。|
|`includeButtonInstance`|是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是`GameObject`类型|
|`parameters`|目标方法需要的零个或多个参数。|

## 描述

根据按钮的不同引用绑定所需的回调函数

在能获取到按钮实例的情况下，不推荐使用基于按钮名称的重载，因为通过名称查找按钮会带来一定的性能损失。