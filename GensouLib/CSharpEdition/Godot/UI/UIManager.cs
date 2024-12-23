using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace GensouLib.Godot.UI
{
    /// <summary>
    /// UI管理器<br/>
    /// UI Manager
    /// </summary>
    /// <remarks>
    /// 提供打开和关闭UI，以及绑定按钮回调的方法。<br/>
    /// Providing methods to open and close UI elements and bind button callbacks.
    /// </remarks>
    public class UIManager
    {
        /// 已实例化的UI
        private static readonly Dictionary<string, Control> InstantiatedUI = new();

        /// <summary>
        /// 资源路径<br/>
        /// Resource path.
        /// </summary>
        public static string ResPath {get; set;} = "res://UI/";

        /// <summary>
        /// 打开指定UI,添加到指定节点。<br/>
        /// Opens the specified UI and adds it to the specified node.
        /// </summary>
        /// <param name="ui">
        /// 要打开的UI，传入UI场景的文件名，不包含扩展名。<br/>
        /// The filename of the UI to open, Pass the UI scene file name without extension.<br/>
        /// 修改 <see cref="ResPath"/> 属性来更改资源路径<br/>
        /// Modify the <see cref="ResPath"/> property to change the resource path.
        /// </param>
        /// <param name="node">
        /// 目标根节点,UI将被添加为该节点的子节点。添加到当前场景使用<c>GetTree().CurrentScene</c>。<br/>
        /// The target root node. The UI will be added as a child of this node. To add it to the current scene, use <c>GetTree().CurrentScene</c>.
        /// </param>
        /// <returns>
        /// 若实例化成功或是已开启的UI则返回该实例，否则返回null。<br/>
        /// Returns the instance if instantiation is successful or the UI is already opened; otherwise, returns null.
        /// </returns>
        public static Control OpenUI(string ui, Node node)
        {
            if (InstantiatedUI.TryGetValue(ui, out Control existingInstance))
            {
                if (!existingInstance.Visible)
                {
                    existingInstance.Visible = true;
                }
                else
                {
                    GD.PushWarning($"UI: {ui} exists and is active. (UI: {ui} 已存在且处于激活状态");
                }
                return existingInstance;
            }
            string path = $"{ResPath}{ui}.tscn";
            PackedScene packedScene = ResourceLoader.Load<PackedScene>(path);

            if (packedScene == null)
            {
                GD.PushError($"UI failed to load, possibly with wrong path: {ui}. (UI加载失败，可能路径错误：{ui})");
                return null;
            }

            Control instance = packedScene.Instantiate<Control>();

            if (instance == null)
            {
                GD.PushError($"Failed to instantiate UI: {ui}.(实例化UI失败：{ui})");
                return null;            
            }
            node.AddChild(instance);

            InstantiatedUI.Add(ui, instance);
            return instance;
        }

         /// <summary>
        /// 关闭指定UI<br/>
        /// Closes the specified UI.
        /// </summary>
        /// <param name="ui">
        /// 要关闭的 UI，传入UI场景的文件名，不包含扩展名。<br/>
        /// The filename of the UI to close, Pass the UI scene file name without extension.
        /// </param>
        /// <param name="destroy">
        /// 是否销毁 UI 实例，默认为 false。设置为 true 将销毁 UI 并释放其资源。<br/>
        /// Whether to destroy the UI instance. Default is false. Set to true to destroy and release its resources.
        /// </param>
        public static void CloseUI(string ui,  bool destroy = false)
        {
            if (InstantiatedUI.TryGetValue(ui, out Control instance))
            {
                if (instance == null)
                {
                    GD.PushError($"UI:{ui} instance is null, cannot close. (UI: {ui} 实例为空，无法关闭)");
                    return;
                }
                
                if (destroy)
                {
                    instance.QueueFree();
                    InstantiatedUI.Remove(ui);
                }
                else
                {
                    instance.Visible = false;
                }
            }
            else
            {
                GD.PushError($"UI:{ui}, does not exist or is not opened. (UI:{ui}，不存在或未开启)");
            }
        }

        /// <summary>
        /// 获取已实例化的UI <br/>
        /// Gets the instantiated UI
        /// </summary>
        /// <param name="ui">
        /// 要获取实例的 UI，传入UI场景的文件名，不包含扩展名。<br/>
        /// The filename of the UI to get the UI instance, Pass the UI scene file name without extension.
        /// </param>
        /// <returns>
        /// 如果获取到UI，则返回其实例，否则返回null <br/>
        /// If UI is retrieved, the instance is returned; otherwise, null is returned
        /// </returns>
        public static Control GetInstantiatedUI(string ui)
        {
            if (InstantiatedUI.ContainsKey(ui)) 
                return InstantiatedUI[ui];
            return null;
        }

        /// <summary>
        /// 根据按钮名称为按钮按下信号绑定回调方法。在能够获取按钮实例的情况下，推荐使用基于按钮实例的绑定方式（例如 <see cref="BindButtonPressedCallback(Button, string, object, bool, object[])"/>）。<br/>
        /// Binds a callback to the button press signal by button name. If the button instance can be accessed directly, it is recommended to use the instance-based binding method (e.g., <see cref="BindButtonPressedCallback(Button, string, object, bool, object[])"/>).<br/>
        /// 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。<br/>
        /// This method should only be used when the button instance is not directly accessible.<br/>
        /// 此方法会尝试从<see cref="target"/>开始，沿着节点树上下查找名为<see cref="buttonName"/>的目标按钮节点。无法查找兄弟节点或其他节点的子节点。<br/>
        /// This method attempts to find the target button node named <see cref="target"/> by searching up or down the node tree starting from <see cref="buttonName"/>. It does not search sibling nodes or other child nodes of different parents.
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
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。该类必须继承自<c>Node</c>。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.The target class must extend from <c>Node</c>
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>Button</c> 类型。<br/>
        /// Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>Button</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>
        public static void BindButtonPressedCallback(string buttonName, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)
        {
            if (target == null)
            {
                GD.PushError("Target class cannot be null! (目标类不能为空)");
                return;
            }
            if (FindEndNode(target, buttonName) is not Button buttonObj)
            {
                buttonObj = FindStartNode(target, buttonName) as Button;
                if (buttonObj == null)
                {
                    GD.PushError($"Button with name {buttonName} not found! (没有找到名为{buttonName}的按钮)");
                    return;
                }
            }
            buttonObj.Pressed += () => 
            {
                Callable callable = new(target, methodName);
                if (includeButtonInstance)
                {
                    Variant[] allParameters = new Variant[parameters.Length + 1];
                    allParameters[0] = buttonObj; 

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        allParameters[i + 1] = parameters[i];
                    }

                    callable.Call(allParameters);
                    return;
                }
                callable.Call(parameters);
            };
        }

        /// <summary>
        /// 根据按钮名称为多个按钮的按下信号绑定到同一个回调方法。在能够获取按钮实例的情况下，推荐使用基于按钮实例的绑定方式（例如 <see cref="BindButtonPressedCallback(Button, string, object, bool, object[])"/>）。<br/>
        /// Binds a callback to multiple buttons press signal by button name. If the button instance can be accessed directly, it is recommended to use the instance-based binding method (e.g., <see cref="BindButtonPressedCallback(Button, string, object, bool, object[])"/>).<br/>
        /// 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。<br/>
        /// It is suggested to use this method only when the button instance is not directly accessible.
        /// 此方法会尝试从<see cref="target"/>开始，沿着节点树上下查找名为<see cref="buttonName"/>的目标按钮节点。无法查找兄弟节点或其他节点的子节点。<br/>
        /// This method attempts to find the target button node named <see cref="target"/> by searching up or down the node tree starting from <see cref="buttonName"/>. It does not search sibling nodes or other child nodes of different parents.
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
        /// 目标方法所属类的实例。当前类使用 <c>this</c>，其他类传入该类的实例。该类必须继承自<c>Node</c>。<br/>
        /// The instance of the class containing the target method. Use <c>this</c> for the current class or provide an instance of another class.The target class must extend from <c>Node</c>
        /// </param>
        /// <param name="includeButtonInstance">
        /// 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>Button</c> 类型。<br/>
        /// Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>Button</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>        
        public static void BindButtonPressedCallback(string[] buttonNames, string methodName, Node target, bool includeButtonInstance, params Variant[] parameters)
        {
            if (buttonNames == null || buttonNames.Length == 0)
            {
                GD.PushError("No button names provided.(没有提供按钮名)");
                return;
            }

            foreach (string button in buttonNames)
            {
                BindButtonPressedCallback(button, methodName, target, includeButtonInstance, parameters);
            }
        }

        /// <summary>
        /// 根据按钮实例为按钮按下信号绑定回调方法。<br/>
        /// Binds a callback to the button press signal by button instance. 
        /// </summary>
        /// <param name="button">
        /// 按钮实例。<br/>
        /// button instance.
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
        /// 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>Button</c> 类型。<br/>
        /// Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>Button</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>      
        public static void BindButtonPressedCallback(Button button, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {
            if (target == null)
            {
                GD.PushError("Target class cannot be null!(目标类不能为空)");
                return;
            }

            if (button == null)
            {
                GD.PushError("Button cannot be null!(按钮不能为空)");
                return;
            }
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                GD.PushError($"Method {methodName} not found in target class!(在目标类没有找到名为{methodName}的方法)");
                return;
            }

            if (includeButtonInstance) 
            {
                object[] allParameters = new object[parameters.Length + 1];
                allParameters[0] = button;

                for (int i = 0; i < parameters.Length; i++)
                {
                    allParameters[i + 1] = parameters[i];
                }
                button.Pressed += () => {method.Invoke(target, allParameters);};
                return;
            }

            button.Pressed += () => {method.Invoke(target, parameters);};        
        }

        /// <summary>
        /// 根据按钮实例为多个按钮按下信号绑定到同一个回调方法。<br/>
        /// Binds a callback to multiple buttons press signal by button instance. 
        /// </summary>
        /// <param name="buttons">
        /// 一个包含多个按钮实例的数组。<br/>
        /// An array of button instances.
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
        /// 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是 <c>Button</c> 类型。<br/>
        /// Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type <c>Button</c>.
        /// </param>
        /// <param name="parameters">
        /// 目标方法需要的零个或多个参数。<br/>
        /// Zero or more additional parameters required by the target method.
        /// </param>      
        public static void BindButtonPressedCallback(Button[] buttons, string methodName, object target, bool includeButtonInstance, params object[] parameters)
        {

            if (buttons == null || buttons.Length == 0)
            {
                GD.PushError("No button instances provided. (没有提供按钮实例)");
                return;
            }

            foreach (Button button in  buttons)
            {
                BindButtonPressedCallback(button, methodName, target, includeButtonInstance, parameters);
            }
        }

        // 广度优先查找子节点
        private static Node FindEndNode(Node startNode, string endNodeName)
        {
            Queue<Node> queue = new();
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current.Name == endNodeName)
                {
                    return current;
                }

                foreach (Node child in current.GetChildren())
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        // 查找父节点
        private static Node FindStartNode(Node endNode, string startNodeName)
        {
            Node current = endNode;
            while (current != null)
            {
                if ((startNodeName == "ROOT" && current == current.GetTree().Root) ||
                    current.Name == startNodeName)
                {
                    return current;
                }
                current = current.GetParent();
            }
            return null;
        }

        // 获取节点绝对路径
        // private static NodePath GetNodeAbsolutePath(Node node)
        // {
        //     NodePath path = "";
        //     while(node != null)
        //     {
        //         string name = node.Name;
        //         path = "/" + name + path;
        //         if (name == "root" && node == node.GetTree().Root) 
        //         {
        //             return path;
        //         }
        //         node = node.GetParent();
        //     }
        //     return null;
        // }   
    }
}