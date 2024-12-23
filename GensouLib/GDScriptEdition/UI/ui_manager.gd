## UI管理器[br]
## UI Manager
##
## 提供打开和关闭UI，以及绑定按钮回调的方法。[br]
## Providing methods to open and close UI elements and bind button callbacks.
class_name UIManager extends Object

# 已实例化的UI
static var _instantiated_ui: Dictionary = {}

## 资源路径。[br]
## Resource path. [br]
static var res_path: String = "res://UI/"

## 打开指定UI,添加到指定节点。[br]
## Opens the specified UI and adds it to the specified node. [br]
## [br]
## [param ui] : [br]
## 要打开的UI，传入UI场景的文件名，不包含扩展名。[br]
## The filename of the UI to open, Pass the UI scene file name without extension.[br]
## 修改 [member res_path] 属性来更改资源路径[br]
## Modify the [member res_path] property to change the resource path.[br]
## [param node] : [br]
## 目标根节点,UI将被添加为该节点的子节点。添加到当前场景使用[code]get_tree().current_scene[/code]。[br]
## The target root node. The UI will be added as a child of this node. To add it to the current scene, use [code]get_tree().current_scene[/code].[br]
## [br]
## 若实例化成功或是已开启的UI则返回该实例，否则返回null。[br]
## Returns the instance if instantiation is successful or the UI is already opened; otherwise, returns null.[br]
static func open_ui(ui: String, node: Node) -> Control:
	if _instantiated_ui.has(ui):
		if not _instantiated_ui[ui].visible:
			_instantiated_ui[ui].visible = true
		else:
			push_error("UI: %s exists and is active. (UI: %s 已存在且处于激活状态)" % [ui,ui])
		return _instantiated_ui[ui]
	var path = res_path + ui + ".tscn"
	var packed_scence = ResourceLoader.load(path)
	if packed_scence == null:
		push_error("UI failed to load, possibly with wrong path: %s. (UI加载失败，可能路径错误或资源未加载：%s)" % [ui,ui])
		return null

	var instance = packed_scence.instantiate()
	if instance == null:
		push_error("Failed to instantiate UI: %s.(实例化UI失败：%s)" % [ui,ui])
		return null
	node.add_child(instance)

	_instantiated_ui[ui] = instance
	return instance


## 关闭指定UI。[br]
## Closes the specified UI. [br]
## [br]
## [param ui] : [br] 
## 要关闭的 UI，传入UI场景的文件名，不包含扩展名。[br]
## The filename of the UI to close, Pass the UI scene file name without extension.[br]
## [param destroy] : [br]
## 是否销毁 UI 实例，默认为 false。设置为 true 将销毁 UI 并释放其资源。[br]
## Whether to destroy the UI instance. Default is false. Set to true to destroy and release its resources.[br]
static func close_ui(ui: String, destroy: bool = false) -> void:
	if _instantiated_ui.has(ui):
		var instance = _instantiated_ui[ui]
		if instance == null:
			push_error("UI: %s instance is null, cannot close. (UI: %s 实例为空，无法关闭)" % [ui,ui])
			return
		if destroy:
			instance.queue_free()
			_instantiated_ui.erase(ui)
		else:
			instance.visible = false
	else:
		push_error("UI: %s , does not exist or is not opened. (UI: %s ，不存在或未开启)" % [ui,ui])


## 获取已实例化的UI [br]
## Gets the instantiated UI [br]
## [br]
## [param ui"] : [br]
## 要获取实例的 UI，传入UI场景的文件名，不包含扩展名。 [br]
## The filename of the UI to get instance, Pass the UI scene file name without extension.[br]
## [br]
## 如果获取到UI，则返回其实例，否则返回null [br]
## If UI is retrieved, the instance is returned; otherwise, null is returned
static func get_instantiated_ui(ui: String) -> Control:
	if _instantiated_ui.has(ui):
		return _instantiated_ui[ui]
	return null


## 根据按钮名称为按钮的按下信号绑定回调方法。如果能够直接获取到按钮实例，推荐使用基于实例的绑定方法（例如 [method bind_button_pressed_callback]）。[br]
## Binds a callback to the button press signal by button name. If the button instance can be accessed directly, it is recommended to use the instance-based binding method (e.g., [method bind_button_pressed_callback]).[br]
## 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。[br]
## This method should only be used when the button instance is not directly accessible.[br]
## 此方法会尝试从[param target]开始，沿着节点树上下查找名为[param button_name]的目标按钮节点。无法查找兄弟节点或其他节点的子节点。[br]
## This method attempts to find the target button node named [param button_name] by searching up or down the node tree starting from [param target]. It does not search sibling nodes or other child nodes of different parents.[br]
## [br]
## [param button_name] : [br]
## 按钮名称 [br]
## button name [br]
## [param method_name] : [br]
## 目标方法的名称。[br]
## The name of the target method to bind. [br]
## [param target] : [br]
## 目标方法所属类的实例。当前类使用 [code]self[/code]，其他类传入该类的实例。该类必须继承自[code]Node[/code]。[br]
## The instance of the class containing the target method. Use [code]self[/code] for the current class or provide an instance of another class.The target class must extend from [code]Node[/code].[br]
## [param include_button_instance] : [br]
## 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是[code]Button[/code]类型。[br]
## Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type [code]Button[/code].[br]
## [param parameters] : [br]
## 目标方法需要的零个或多个参数。[br]
## Zero or more additional parameters required by the target method.
static func bind_button_pressed_callback_byname(button_name: String, method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void:
	if target == null:
		push_error("Target class cannot be null! (目标类不能为空)")
		return
	
	var button_obj: Button = _find_end_node(target, button_name) as Button
	if button_obj == null:
		button_obj = _find_start_node(target, button_name) as Button
		if button_obj == null:	
			push_error("Button with name %s not found! (没有找到名为 %s 的按钮)" % [button_name,button_name])
			return
	
	var callable: Callable = Callable(target, method_name)
	if callable == null:
		push_error("Method %s not found in target class!(在目标类没有找到名为 %s 的方法)" % [method_name,method_name])
		return

	if include_button_instance:
		var all_parameters: Array = [button_obj]
		all_parameters.append_array(parameters)

		button_obj.pressed.connect(callable.bindv(all_parameters))
		return
	button_obj.pressed.connect(callable.bindv(parameters))


## 根据按钮名称为多个按钮的按下信号绑定到同一个回调方法。在能够获取按钮实例的情况下，推荐使用基于按钮实例的绑定方式（例如 [method bind_buttons_pressed_callback]）。[br]
## Binds a callback to multiple buttons press signal by button name. If the button instance can be accessed directly, it is recommended to use the instance-based binding method (e.g., [method bind_button_pressed_callback]).[br]
## 只有在无法直接访问按钮实例的情况下，才建议使用此方法进行绑定。[br]
## This method should only be used when the button instance is not directly accessible.[br]
## 此方法会尝试从[param target]开始，沿着节点树上下查找名为[param button_name]的目标按钮节点。无法查找兄弟节点或其他节点的子节点。[br]
## This method attempts to find the target button node named [param button_name] by searching up or down the node tree starting from [param target]. It does not search sibling nodes or other child nodes of different parents.[br]
## [br]
## [param button_names] : [br]
## 一个包含多个按钮名称的字符串数组。 [br]
## An array of button names. [br]
## [param method_name] : [br]
## 目标方法的名称。[br]
## The name of the target method to bind. [br]
## [param target] : [br]
## 目标方法所属类的实例。当前类使用 [code]self[/code]，其他类传入该类的实例。该类必须继承自[code]Node[/code]。[br]
## The instance of the class containing the target method. Use [code]self[/code] for the current class or provide an instance of another class.The target class must extend from [code]Node[/code].[br]
## [param include_button_instance] : [br]
## 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是[code]Button[/code]类型。[br]
## Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type [code]Button[/code].[br]
## [param parameters] : [br]
## 目标方法需要的零个或多个参数。[br]
## Zero or more additional parameters required by the target method.
static func bind_buttons_pressed_callback_byname(button_names: Array[String], method_name: String, target: Node, include_button_instance: bool, parameters: Array = []) -> void:
	if button_names == null or button_names.size() == 0:
		push_error("No button names provided.(没有提供按钮名)")
		return
	
	for button in button_names:
		bind_button_pressed_callback_byname(button, method_name, target, include_button_instance, parameters)


## 根据按钮实例为按钮按下信号绑定回调方法。[br]
## Binds a callback to the button press signal by button instance. [br]
## [br]
## [param button] : [br]
## 按钮实例。 [br]
## button instance.[br]
## [param method_name] : [br]
## 目标方法的名称。[br]
## The name of the target method to bind. [br]
## [param target] : [br]
## 目标方法所属类的实例。当前类使用 [code]self[/code]，其他类传入该类的实例。[br]
## The instance of the class containing the target method. Use [code]self[/code] for the current class or provide an instance of another class.[br]
## [param include_button_instance] : [br]
## 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是[code]Button[/code]类型。[br]
## Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type [code]Button[/code].[br]
## [param parameters] : [br]
## 目标方法需要的零个或多个参数。[br]
## Zero or more additional parameters required by the target method.
static func bind_button_pressed_callback(button: Button, method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void:
	if target == null:
		push_error("Target class cannot be null!(目标类不能为空)")
		return
	
	if button == null:
		push_error("Button cannot be null!(按钮不能为空)")
		return
	
	var callable: Callable = Callable(target, method_name)
	if callable == null:
		push_error("Method %s not found in target class!(在目标类没有找到名为 %s 的方法)" % [method_name,method_name])
		return
	
	if include_button_instance:
		var all_parameters: Array = [button]
		all_parameters.append_array(parameters) 

		button.pressed.connect(callable.bindv(all_parameters))
		return
	button.pressed.connect(callable.bindv(parameters))


## 根据按钮实例为多个按钮按下信号绑定到同一个回调方法。[br]
## Binds a callback to multiple buttons press signal by button instance. [br]
## [br]
## [param buttons] : [br]
## 一个包含多个按钮实例的数组。 [br]
## An array of button instances.[br]
## [param method_name] : [br]
## 目标方法的名称。[br]
## The name of the target method to bind. [br]
## [param target] : [br]
## 目标方法所属类的实例。当前类使用 [code]self[/code]，其他类传入该类的实例。[br]
## The instance of the class containing the target method. Use [code]self[/code] for the current class or provide an instance of another class.[br]
## [param include_button_instance] : [br]
## 是否将按钮实例作为第一个参数传递给目标方法。如果是，目标方法的第一个参数必须是[code]Button[/code]类型。[br]
## Whether to pass the button instance as the first parameter to the target method. If true, the target method's first parameter must be of type [code]Button[/code].[br]
## [param parameters] : [br]
## 目标方法需要的零个或多个参数。[br]
## Zero or more additional parameters required by the target method.
static func bind_buttons_pressed_callback(buttons: Array[Button], method_name: String, target: Object, include_button_instance: bool, parameters: Array = []) -> void:
	if buttons == null or buttons.size() == 0:
		push_error("No button instances provided. (没有提供按钮实例)")
		return
	
	for button in buttons:
		bind_button_pressed_callback(button, method_name, target, include_button_instance, parameters)


# 广度优先查找子节点
static func _find_end_node(start_node: Node, end_node_name: String) -> Node:
	var queue: Array[Node]
	queue.append(start_node)

	while queue.size() > 0:
		var current: Node = queue.pop_front()

		if (current.name == end_node_name):
			return current
		
		for child: Node in current.get_children():
			queue.append(child)

	return null


# 查找父节点
static func _find_start_node(end_node: Node, start_node_name: String) -> Node:
	var current: Node = end_node
	while current != null:
		if ((start_node_name == "ROOT" and current == current.get_tree().root) or 
		current.name == start_node_name):
			return current
		current = current.get_parent()
	return null
				
