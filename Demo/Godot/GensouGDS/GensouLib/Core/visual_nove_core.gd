## 核心功能
class_name VisualNoveCore extends Node

## 显示角色名的标签
static var charater_name: Label

## 显示对话的标签
static var dialogue_text: Label

## 显示左侧立绘的TextureRect
static var figure_left: TextureRect

## 显示中间立绘的TextureRect
static var figure_center: TextureRect

## 显示右侧立绘的TextureRect
static var figure_right: TextureRect

## 显示头像的TextureRect
static var portrait: TextureRect

## 显示背景的TextureRect
static var background: TextureRect

## 任一处于节点树的节点
static var game_manager_node: Node

## BGM播放器
static var bgm: AudioStreamPlayer:
	get: return AudioManager.bgm_player
	set(value): AudioManager.bgm_player = value

## BGS播放器
static var bgs: AudioStreamPlayer:
	get: return AudioManager.bgs_player
	set(value): AudioManager.bgs_player = value

## 音效播放器
static var se: AudioStreamPlayer:
	get: return AudioManager.se_player
	set(value): AudioManager.se_player = value

## 语音播放器
static var voice: AudioStreamPlayer:
	get: return AudioManager.voice_player
	set(value): AudioManager.voice_player = value

## 选择按钮的容器
static var choice_button_container: VBoxContainer

## 选择按钮的场景
static var choice_button_scene: PackedScene

## 对话框
static var text_box: Panel

## 标题界面场景路径
static var title_scene_path: String = "res://Scenes/Title.tscn"

## 存档界面场景路径
static var save_load_scene_path: String = "res://Scenes/SaveLoad.tscn"

## 主界面场景路径
static var main_scene_path: String = "res://Scenes/Main.tscn"

## 系统设置界面场景路径
static var config_scnen_path: String = "res://Scenes/Config.tscn"

## 立绘路径
static var figure_path: String = "res://Assets/Figure/"

## 头像路径
static var portrait_path: String = "res://Assets/Portrait/"

## 背景路径
static var background_path: String = "res://Assets/Background/"

## BGM路径
static var bgm_path: String = "res://Assets/BGM/"

## 声音路径
static var vocal_path: String = "res://Assets/Vocal/"

## 历史对话记录
static var history: Array[String] = []

## 文本显示控制器实例
static var typewriter: DisplayController

## 文本显示速度
static var text_display_speed: float = 0.05

## 日志面板是否激活
static var log_panel_active: bool = false

## 存档界面是否激活
static var save_load_ui_active: bool = false

## 是否在自动播放剧本
static var on_auto_play: bool = false

## 自动播放剧本间隔
static var auto_play_interval: int = 1

## 是否在跳过对话
static var on_skiping: bool = false

## 配置界面是否激活
static var config_ui_active: bool = false

## 配置界面根节点
static var config_scene_root_node: Node

static var _is_mouse_hovering_button: bool = false

## 初始化
static func init(
	_charater_name: Label,
	_dialogue_text: Label,
	_typewriter: DisplayController,
	_figure_left: TextureRect,
	_figure_center: TextureRect,
	_figure_right: TextureRect,
	_portrait: TextureRect,
	_background: TextureRect,
	_bgm: AudioStreamPlayer,
	_bgs: AudioStreamPlayer,
	_se: AudioStreamPlayer,
	_voice: AudioStreamPlayer,
	_game_manager_node: Node,
	_choice_button_container: VBoxContainer,
	_choice_button_scene: PackedScene,
	_text_box: Panel,
	_auto_play_interval: int,
	_title_scene_path: String = "res://Scenes/Title.tscn",
	_save_load_scene_path: String = "res://Scenes/SaveLoad.tscn",
	_main_scene_path: String = "res://Scenes/Main.tscn",
	_figure_path: String = "res://Assets/Figure/",
	_portrait_path: String = "res://Assets/Portrait/",
	_background_path: String = "res://Assets/Background/",
	_bgm_path: String = "res://Assets/BGM/",
	_vocal_path: String = "res://Assets/Vocal/",
) -> void:
	charater_name = _charater_name
	dialogue_text = _dialogue_text
	typewriter = _typewriter
	figure_left = _figure_left
	figure_center = _figure_center
	figure_right = _figure_right
	portrait = _portrait
	background = _background
	bgm = _bgm
	bgs = _bgs
	se = _se
	voice = _voice
	game_manager_node = _game_manager_node
	choice_button_container = _choice_button_container
	choice_button_scene = _choice_button_scene
	text_box = _text_box
	auto_play_interval = _auto_play_interval
	title_scene_path = _title_scene_path
	save_load_scene_path = _save_load_scene_path
	main_scene_path = _main_scene_path
	figure_path = _figure_path
	portrait_path = _portrait_path
	background_path = _background_path
	bgm_path = _bgm_path
	vocal_path = _vocal_path

	var mouse_button = InputEventMouseButton.new()
	mouse_button.button_index = MOUSE_BUTTON_LEFT
	InputMap.action_add_event("ui_accept", mouse_button)

	mouse_button = InputEventMouseButton.new()
	mouse_button.button_index = MOUSE_BUTTON_WHEEL_DOWN
	InputMap.action_add_event("ui_accept", mouse_button)

	if not InputMap.has_action("show_history"):
		InputMap.add_action("show_history")
		mouse_button = InputEventMouseButton.new()
		mouse_button.button_index = MOUSE_BUTTON_WHEEL_UP
		InputMap.action_add_event("show_history", mouse_button)

	if not InputMap.has_action("hide_textbox"):
		InputMap.add_action("hide_textbox")
		mouse_button = InputEventMouseButton.new()
		mouse_button.button_index = MOUSE_BUTTON_RIGHT
		InputMap.action_add_event("hide_textbox", mouse_button)

	AudioManager.init(game_manager_node)

## 恢复脚本全局变量
static func recover_global_variables() -> void:
	if SaveManager.save_exists("globalVariables.json"):
		var global_variables = SaveManager.load_from_json("globalVariables.json")
		for item in global_variables:
			VariableInterpreter.variable_list.get_or_add(item, global_variables[item])
			if not VariableInterpreter.global_variable_list.has(item):
				VariableInterpreter.global_variable_list.append(item)
		SaveManager.clear_loaded_json_data()

## 恢复数据 [br]
## [br]
## [param save_name] : [br]
## 存档名
static func recover_data(save_name: String) -> void:
	if not SaveManager.save_exists(save_name): return
	var script_name := SaveManager.get_data_from_json(save_name, "currentScript") as String
	var line_index := SaveManager.get_data_from_json(save_name, "currentLine") as int
	ScriptReader.read_and_execute(script_name, line_index)

	VariableInterpreter.variable_list = SaveManager.get_data_from_json(save_name, "scriptVariables") as Dictionary

	recover_global_variables()

	DialogueInterpreter.current_dialogue = SaveManager.get_data_from_json(save_name, "currentDialogue") as String
	dialogue_text.text = DialogueInterpreter.current_dialogue
	dialogue_text.visible_characters = -1

	DialogueInterpreter.current_speaker = SaveManager.get_data_from_json(save_name, "currentSpeaker") as String
	charater_name.text = DialogueInterpreter.current_speaker

	ChoiceInterpreter.on_choosing = SaveManager.get_data_from_json(save_name, "onChoosing") as bool
	if not ChoiceInterpreter.on_choosing: clear_choice_buttons()

	var bgm_name := SaveManager.get_data_from_json(save_name, "currentBgm") as String

	var bgs_name := SaveManager.get_data_from_json(save_name, "currentBgs") as String

	var se_name := SaveManager.get_data_from_json(save_name, "currentSe") as String 

	var voice_name := SaveManager.get_data_from_json(save_name, "currentVoice") as String

	if not bgm_name.is_empty():
		AudioManager.play_bgm(bgm_path.path_join(bgm_name))
	else:
		AudioManager.stop_bgm()

	if not bgs_name.is_empty():
		AudioManager.play_bgs(bgm_path.path_join(bgs_name))
	else:
		AudioManager.stop_bgs()

	if not se_name.is_empty():
		AudioManager.play_se(vocal_path.path_join(se_name))

	if not voice_name.is_empty():
		AudioManager.play_voice(vocal_path.path_join(voice_name))
	else:
		AudioManager.stop_voice()

	var left_figure_name := SaveManager.get_data_from_json(save_name, "leftFigure") as String
	
	var center_figure_name := SaveManager.get_data_from_json(save_name, "centerFigure") as String
	
	var right_figure_name := SaveManager.get_data_from_json(save_name, "rightFigure") as String
	
	var portrait_name := SaveManager.get_data_from_json(save_name, "portrait") as String
	
	var background_name := SaveManager.get_data_from_json(save_name, "background") as String

	if not left_figure_name.is_empty():
		figure_left.texture = ResourceLoader.load(figure_path.path_join(left_figure_name))
	else:
		figure_left.texture = null

	if not center_figure_name.is_empty():
		figure_center.texture = ResourceLoader.load(figure_path.path_join(center_figure_name))
	else:
		figure_center.texture = null

	if not right_figure_name.is_empty():
		figure_right.texture = ResourceLoader.load(figure_path.path_join(right_figure_name))
	else:
		figure_right.texture = null

	if not portrait_name.is_empty():
		portrait.texture = ResourceLoader.load(portrait_path.path_join(portrait_name))
	else:
		portrait.texture = null

	if not background_name.is_empty():
		background.texture = ResourceLoader.load(background_path.path_join(background_name))
	else:
		background.texture = null

## 显示当前行对话
static func display_current_line() -> void:
	if not charater_name or not dialogue_text or not typewriter:
		push_error("VisualNoveCore: Missing instance")
		return
	if typewriter.is_typing:
		typewriter.display_complete_line()
	else:
		charater_name.text = DialogueInterpreter.current_speaker
		typewriter.display_line(DialogueInterpreter.current_dialogue)

## 创建选择按钮 [br]
## [br]
## [param choices_text] : [br]
## 选择文本
## [br]
## [param spacing] : [br]
## 间隔
static func create_choice_buttons(choices_text: PackedStringArray, spacing: int) -> void:
	if not choice_button_container or not choice_button_scene:
		push_error("VisualNoveCore: Missing instance")
		return
	clear_choice_buttons()
	choice_button_container.add_theme_constant_override("separation", spacing)

	for i in range(choices_text.size()):
		var button: Button = choice_button_scene.instantiate()
		choice_button_container.add_child(button)
		button.name = "choice_button_" + str(i)
		button.text = choices_text[i]
		var choice_index = i
		button.pressed.connect(func():ChoiceInterpreter.select_choice(choice_index))

## 显示或隐藏文本框 [br]
## [br]
## [param show] : [br]
## 是否显示
static func show_text_box(show: bool) -> void:
	if not text_box:
		push_error("VisualNoveCore: Missing instance")
		return
	text_box.visible = show

## 清除选择按钮
static func clear_choice_buttons() -> void:
	if not choice_button_container:
		push_error("VisualNoveCore: Missing instance")
		return

	for child in choice_button_container.get_children():
		child.queue_free()

## 设置字体大小 [br]
## [br]
## [param size] : [br]
## 字体大小
static func set_font_size(size: int) -> void:
	if not dialogue_text:
		push_error("VisualNoveCore: Missing instance")
		return
	dialogue_text.add_theme_font_size_override("font_size", size)

func _input(_event):
	if should_execute_next_line():
		BaseInterpreter.execute_next_line()
	if should_show_history():
		TextboxFunctions.show_history()
	if should_switch_textbox_visibility():
		TextboxFunctions.switch_textbox_visibility()

## 鼠标进入按钮
static func mouse_entered_button() -> void:
	_is_mouse_hovering_button = true

## 鼠标退出按钮
static func mouse_exited_button() -> void:
	_is_mouse_hovering_button = false

## 是否应该执行下一行脚本
static func should_execute_next_line() -> bool:
	return (
		Input.is_action_just_pressed("ui_accept") and
		not _is_mouse_hovering_button and
		ParseScript.wait_click and
		not ChoiceInterpreter.on_choosing and
		not log_panel_active and
		text_box.visible and
		not save_load_ui_active and
		not config_ui_active
	)

## 是否应该显示历史记录
static func should_show_history() -> bool:
	return(
		Input.is_action_just_pressed("show_history") and
		not log_panel_active and
		not save_load_ui_active and
		not config_ui_active
	)

## 是否应该切换对话框可见性
static func should_switch_textbox_visibility() -> bool:
	return(
		Input.is_action_just_pressed("hide_textbox") and
		not log_panel_active and
		not save_load_ui_active and
		not config_ui_active and
		not ParseScript.script_hided_textbox
	)

## 停止自动播放和跳过对话
static func stop_auto_play_and_skip() -> void:
	on_auto_play = false
	on_skiping = false

## 关闭配置界面
static func close_config_ui() -> void:
	config_ui_active = false
	config_scene_root_node.queue_free()
