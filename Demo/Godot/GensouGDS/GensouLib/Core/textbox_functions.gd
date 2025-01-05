class_name TextboxFunctions extends VisualNoveCore

## 跳过对话按钮
static var skip_button: Button

## 自动播放按钮
static var auto_button: Button

## 保存游戏按钮
static var save_button: Button

## 加载游戏按钮
static var load_button: Button

## 历史记录按钮
static var log_button: Button

## 系统设置按钮
static var system_button: Button

## 返回标题按钮
static var title_button: Button

## 历史记录容器
static var log_containter: VBoxContainer

## 历史记录文本场景
static var log_text_scene: PackedScene

## 历史记录滚动视图
static var scroll_view: ScrollContainer

## 历史记录面板
static var log_panel: Panel

## 关闭历史记录面板按钮
static var close_log_panel_button: Button

## 初始化对话框功能按钮
static func init_textbox_functions(
	_skip_button: Button,
	_auto_button: Button,
	_save_button: Button,
	_load_button: Button,
	_log_button: Button,
	_system_button: Button,
	_title_button: Button,
	_log_containter: VBoxContainer,
	_log_text_scene: PackedScene,
	_scroll_view: ScrollContainer,
	_log_panel: Panel,
	_close_log_panel_button: Button
) -> void:
	skip_button = _skip_button
	auto_button = _auto_button
	save_button = _save_button
	load_button = _load_button
	log_button = _log_button
	system_button = _system_button
	title_button = _title_button
	log_containter = _log_containter
	log_text_scene = _log_text_scene
	scroll_view = _scroll_view
	log_panel = _log_panel
	close_log_panel_button = _close_log_panel_button
	if save_button:
		save_button.pressed.connect(func():save_load_game_button_click(true))
		_connect_button_hover_signal(save_button)
	if load_button:
		load_button.pressed.connect(func():save_load_game_button_click(false))
		_connect_button_hover_signal(load_button)
	if log_button:
		log_button.pressed.connect(show_history)
		_connect_button_hover_signal(log_button)
	if close_log_panel_button:
		close_log_panel_button.pressed.connect(hide_history)
		_connect_button_hover_signal(close_log_panel_button)
	if title_button:
		title_button.pressed.connect(back_to_title)
		_connect_button_hover_signal(title_button)
	if auto_button:
		auto_button.pressed.connect(func():switch_auto_play(true))
		_connect_button_hover_signal(auto_button)
	if skip_button:
		skip_button.pressed.connect(func():switch_skip(true))
		_connect_button_hover_signal(skip_button)
	if system_button:
		system_button.pressed.connect(open_config_ui)
		_connect_button_hover_signal(system_button)

static func _connect_button_hover_signal(button: Button) -> void:
	button.mouse_entered.connect(mouse_entered_button)
	button.mouse_exited.connect(mouse_exited_button)

## 返回标题
static func back_to_title() -> void:
	on_auto_play = false
	on_skiping = false
	game_manager_node.get_tree().change_scene_to_file(title_scene_path)

## 唤起保存或加载游戏界面 [br]
## [br]
## [param is_save] : [br]
## 是否是保存
static func save_load_game_button_click(is_save: bool) -> void:
	on_auto_play = false
	on_skiping = false
	if is_save: ScreenshotToTextureRect.capture_screenshot()
	SaveLoadGame.is_save = is_save
	save_load_ui_active = true
	var save_load_scene: PackedScene = ResourceLoader.load(save_load_scene_path)
	SaveLoadGame.save_load_scene_root_node = save_load_scene.instantiate()
	game_manager_node.get_tree().root.add_child(SaveLoadGame.save_load_scene_root_node)

## 打开系统设置界面 [br]
## [br]
## [param config_scene_path] : [br]
## 系统设置场景路径 [br]
## [br]
## [param node] : [br]
## 任意节点
static func open_config_ui(config_scene_path: String = "res://Scenes/Config.tscn", node: Node = null) -> void:
	config_ui_active = true
	on_auto_play = false
	on_skiping = false
	if node:
		var config_scene: PackedScene = ResourceLoader.load(config_scene_path)
		config_scene_root_node = config_scene.instantiate()
		node.get_tree().root.add_child(config_scene_root_node)
	else:
		var config_scene: PackedScene = ResourceLoader.load(config_scene_path)
		config_scene_root_node = config_scene.instance()
		game_manager_node.get_tree().root.add_child(config_scene_root_node)

## 显示历史记录面板
static func show_history() -> void:
	on_auto_play = false
	on_skiping = false
	for child in log_containter.get_children():
		child.queue_free()
	
	for h in history:
		var log_text: Label = log_text_scene.instantiate()
		log_containter.add_child(log_text)
		log_text.text = h.replace("\n", " ")
	log_panel_active = true
	log_panel.visible = true

## 隐藏历史记录面板
static func hide_history() -> void:
	log_panel_active = false
	log_panel.visible = false

## 切换对话框可见性
static func switch_textbox_visibility() -> void:
	on_auto_play = false
	on_skiping = false
	text_box.visible = not text_box.visible

## 切换自动播放 [br]
## [br]
## [param is_on] : [br]
## 是否开启
static func switch_auto_play(is_on: bool) -> void:
	on_auto_play = is_on
	if on_auto_play and not typewriter.is_typing and not ChoiceInterpreter.on_choosing:
		BaseInterpreter.execute_next_line()

## 切换跳过对话 [br]
## [br]
## [param is_on] : [br]
## 是否开启
static func switch_skip(is_on: bool) -> void:
	on_skiping = is_on
	if on_skiping and not typewriter.is_typing and not ChoiceInterpreter.on_choosing:
		BaseInterpreter.execute_next_line()
