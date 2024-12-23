using System.Collections.Generic;
using UnityEngine;
using GensouLib.Unity.ResourceLoader;
using UnityEngine.UI;
using System.Reflection;

namespace GensouLib.Unity.UI
{
    /// <summary>
    /// UI管理器<br/>
    /// UI Manager
    /// </summary>
    /// <remarks>
    /// 提供打开和关闭UI，以及绑定按钮回调的方法。<br/>
    /// Providing methods to open and close UI elements and bind button callbacks.
    /// </remarks>
    public class UIManager : MonoBehaviour
    {
        // 已实例化的UI
        private static readonly Dictionary<string, GameObject> InstantiatedUI = new();

        /// <summary>
        /// 打开指定UI。<br/>
        /// Opens the specified UI.
        /// </summary>
        /// <param name="ui">
        /// 要打开的 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。<br/>
        /// The UI to open. If the UI prefab's address has been manually modified in Addressables, pass the address; otherwise, pass the UI prefab's filename without the file extension.
        /// </param>
        /// <remarks>
        /// 加载并实例化UI到当前场景。<br/>
        /// Loads and instantiates the UI into the current scene.
        /// </remarks>
        /// <returns>
        /// 若实例化成功或是已开启的UI则返回该实例，否则返回null。<br/>
        /// Returns the instance if instantiation is successful or the UI is already opened; otherwise, returns null.
        /// </returns>
        public static GameObject OpenUI(string ui)
        {
            if (InstantiatedUI.TryGetValue(ui, out GameObject existingInstance))
            {
                if (!existingInstance.activeSelf)
                {
                    existingInstance.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"UI: {ui} exists and is active. (UI: {ui} 已存在且处于激活状态)");
                }
                return existingInstance;
            }

            string resourcePath = AssetLoader.GetResourcePath("UI", ui);

            GameObject instance = AssetLoader.Instantiate(resourcePath, true);

            if (instance != null)
            {
                InstantiatedUI.Add(ui, instance);
                return instance;
            }
            else
            {
                Debug.LogError($"UI: {ui} failed to open, possibly with wrong path or resource not loaded: {resourcePath}. (UI: {ui} 打开失败，可能路径错误或资源未加载：{resourcePath})");
                return null;
            }
        }
        
        /// <summary>
        /// 关闭指定UI。<br/>
        /// Closes the specified UI.
        /// </summary>
        /// <param name="ui">
        /// 要关闭的已开启 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。<br/>
        /// The UI to close.If the UI prefab's address has been manually modified in Addressables, pass the address; otherwise, pass the UI prefab's filename without the file extension.
        /// </param>
        /// <param name="destroy">
        /// 是否销毁 UI 实例，默认为 false。设置为 true 将销毁 UI 并释放其资源。<br/>
        /// Whether to destroy the UI instance. The default is false. Setting it to true will destroy the UI and release its resources.
        /// </param>
        public static void CloseUI(string ui, bool destroy = false)
        {
            if (InstantiatedUI.TryGetValue(ui, out GameObject instance))
            {
                if (instance == null)
                {
                    Debug.LogError($"UI:{ui} instance is null, cannot close(UI: {ui} 实例为空，无法关闭)");
                    return;
                }
                
                if (destroy)
                {
                    string resourcePath = AssetLoader.GetResourcePath("UI", ui);
                    AssetLoader.ReleaseResource(resourcePath);
                    Destroy(instance);
                    InstantiatedUI.Remove(ui);
                }
                else
                {
                    instance.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning($"UI:{ui}, does not exist or is not opened (UI:{ui}，不存在或未开启)");
            }
        }

        /// <summary>
        /// 获取已实例化的UI <br/>
        /// Gets the instantiated UI
        /// </summary>
        /// <param name="ui">
        /// 要获取的 UI。若在Addressables中手动修改了UI预制件的地址则传入该地址，否则传入不带文件扩展名的UI预制件文件名。< <br/>
        /// The UI instance to get. If the UI prefab's address has been manually modified in Addressables, pass the address; otherwise, pass the UI prefab's filename without the file extension.
        /// </param>
        /// <returns>
        /// 如果获取到UI，则返回其实例，否则返回null <br/>
        /// If UI is retrieved, the instance is returned; otherwise, null is returned
        /// </returns>
        public static GameObject GetInstantiatedUI(string ui)
        {
            if (InstantiatedUI.ContainsKey(ui))
                return InstantiatedUI[ui];
            return null;
        }

        /// <summary>
        /// 根据按钮名称为按钮按下事件绑定回调方法。在能够获取按钮实例的情况下，推荐使用基于按钮实例的绑定方式（例如 <see cref="BindButtonPressedCallback(GameObject, string, object, bool, object[])"/>）。<br/>
        /// Binds a callback to the button press event by button name. If the button instance can be accessed, it is recommended to use the instance-based binding method (e.g., <see cref="BindButtonPressedCallback(GameObject, string, object, bool, object[])"/>).<br/>
        /// 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。<br/>
        /// It is suggested to use this method only when the button instance is not directly accessible.
        /// </summary>
        /// <param name="buttonName">
        /// 按钮名称。<br/>
        /// button name.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button GameObject instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(string buttonName, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            GameObject buttonObj = GameObject.Find(buttonName);
            if (buttonObj == null)
            {
                Debug.LogError($"Button GameObjet with name {buttonName} not found!(没有找到名为 {buttonName} 的按钮GameObjet)");
                return;
            }

            if (!buttonObj.TryGetComponent<Button>(out var button))
            {
                Debug.LogError("Button component not found!(没有找到按钮组件)");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Target class cannot be null!(目标类不能为空)");
                return;
            }

            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Debug.LogError($"Method {methodName} not found in target class!(在目标类没有找到名为{methodName}的方法)");
                return;
            }

            if (includeButtonInstance) 
            {
                object[] allParameters = new object[parameters.Length + 1];
                allParameters[0] = buttonObj;

                for (int i = 0; i < parameters.Length; i++)
                {
                    allParameters[i + 1] = parameters[i];
                }
                button.onClick.AddListener(() => method.Invoke(target, allParameters));
                return;
            }

            button.onClick.AddListener(() => method.Invoke(target, parameters));
        }

        /// <summary>
        /// 根据按钮名称为多个按钮的按下事件绑定到同一个回调方法。在能够获取按钮实例的情况下，推荐使用基于按钮实例的绑定方式（例如 <see cref="BindButtonPressedCallback(GameObject[], string, object, bool, object[])"/>）。<br/>
        /// Binds a callback to multiple buttons press event by button name.If the button instance can be accessed, it is recommended to use the instance-based binding method (e.g., <see cref="BindButtonPressedCallback(GameObject[], string, object, bool, object[])"/>).<br/>
        /// 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。<br/>
        /// It is suggested to use this method only when the button instance is not directly accessible.
        /// </summary>
        /// <param name="buttonNames">
        /// 一个包含多个按钮名称的字符串数组。<br/>
        /// An array of button names.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button GameObject instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(string[] buttonNames, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (buttonNames == null || buttonNames.Length == 0)
            {
                Debug.LogError("No button names provided.(没有提供按钮名)");
                return;
            }

            foreach (string button in buttonNames)
            {
                BindButtonPressedCallback(button, methodName, target, includeButtonInstance, parameters);
            }
        }

        /// <summary>
        /// 根据按钮GameObject实例为按钮按下事件绑定回调方法。<br/>
        /// Binds a callback to the button press event by button GameObject instance.
        /// </summary>
        /// <param name="buttonObject">
        /// 按钮GameObject实例。<br/>
        /// button GameObject instance.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button GameObject instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(GameObject buttonObject, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (buttonObject == null)
            {
                Debug.LogError($"Button GameObject cannot be null!(按钮GameObject不能为空)");
                return;
            }

            if (!buttonObject.TryGetComponent<Button>(out var button))
            {
                Debug.LogError("Button component not found!(没有找到按钮组件)");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Target class cannot be null!(目标类不能为空)");
                return;
            }

            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Debug.LogError($"Method {methodName} not found in target class!(在目标类没有找到名为{methodName}的方法)");
                return;
            }

            if (includeButtonInstance) 
            {
                object[] allParameters = new object[parameters.Length + 1];
                allParameters[0] = buttonObject;

                for (int i = 0; i < parameters.Length; i++)
                {
                    allParameters[i + 1] = parameters[i];
                }
                button.onClick.AddListener(() => method.Invoke(target, allParameters));
                return;
            }

            button.onClick.AddListener(() => method.Invoke(target, parameters));
        }

        /// <summary>
        /// 根据按钮GameObject实例为多个按钮的按下事件绑定到同一个回调方法。<br/>
        /// Binds a callback to multiple buttons press event by button GameObject instance.
        /// </summary>
        /// <param name="buttonObjects">
        /// 一个包含多个按钮GameObject实例的数组。<br/>
        /// An array of button GameObject instances.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button GameObject instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(GameObject[] buttonObjects, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (buttonObjects == null || buttonObjects.Length == 0)
            {
                Debug.LogError("No button GameObject instances provided.(没有提供按钮GameObject实例)");
                return;
            }

            foreach (GameObject button in buttonObjects)
            {
                BindButtonPressedCallback(button, methodName, target, includeButtonInstance, parameters);
            }
        }

        /// <summary>
        /// 根据按钮组件实例为按钮按下事件绑定回调方法。<br/>
        /// Binds a callback to the button press event by button component instance.
        /// </summary>
        /// <param name="button">
        /// 按钮组件实例。<br/>
        /// button component instance.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (button == null)
            {
                Debug.LogError($"Button cannot be null!(按钮不能为空)");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Target class cannot be null!(目标类不能为空)");
                return;
            }

            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Debug.LogError($"Method {methodName} not found in target class!(在目标类没有找到名为{methodName}的方法)");
                return;
            }

            if (includeButtonInstance) 
            {
                object[] allParameters = new object[parameters.Length + 1];
                allParameters[0] = button.gameObject;

                for (int i = 0; i < parameters.Length; i++)
                {
                    allParameters[i + 1] = parameters[i];
                }
                button.onClick.AddListener(() => method.Invoke(target, allParameters));
                return;
            }

            button.onClick.AddListener(() => method.Invoke(target, parameters));
        }

        /// <summary>
        /// 根据按钮组件实例为多个按钮的按下事件绑定到同一个回调方法。<br/>
        /// Binds a callback to multiple buttons press event by button component instance.
        /// </summary>
        /// <param name="buttons">
        /// 一个包含多个按钮组件实例的数组。<br/>
        /// An array of button component instances.
        /// </param>
        /// <param name="methodName">
        /// 目标方法的名称。<br/>
        /// The name of the target method to bind.
        /// </param>
        /// <param name="target">
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮GameObject实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>GameObject</c> 类型。<br/>
        /// Whether to pass the button GameObject instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>GameObject</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (buttons == null || buttons.Length == 0)
            {
                Debug.LogError("No button component instances provided.(没有提供按钮组件实例)");
                return;
            }

            foreach (Button button in buttons)
            {
                BindButtonPressedCallback(button, methodName, target, includeButtonInstance, parameters);
            }
        }

    }
}