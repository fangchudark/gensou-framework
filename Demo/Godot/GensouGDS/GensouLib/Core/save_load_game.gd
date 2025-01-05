## 保存/加载游戏
class_name SaveLoadGame extends VisualNoveCore

## 最大存档槽位数
static var max_slot: int = 10

## 存档槽位场景
static var save_slot_scene: PackedScene

## 关闭存档界面的按钮
static var close_button: Button

## 存档界面的标题
static var panel_title: Label

## 存档槽位容器
static var save_slot_container: VBoxContainer

## 是否是保存游戏
static var is_save: bool = false

## 显示时间戳的节点路径
static var timestamp_node_path: String = "TimeStamp"

## 显示当前对话文本的节点路径
static var dialogue_node_path: String = "Dialogue"

## 显示当前截图的节点路径
static var screenshot_node_path: String = "ScreenShot"

## 存档/加载界面根节点
static var save_load_scene_root_node: Node

static var _data: Dictionary = {}

static var save_slots: Array[Button] = []

## 初始化
static func init_save_load_game(
	_save_slot_scene: PackedScene,
	_close_button: Button,
	_panel_title: Label,
	_save_slot_container: VBoxContainer,
	_max_slots = 10,
	_timestamp_node_path = "TimeStamp",
	_dialogue_node_path = "Dialogue",
	_screenshot_node_path = "ScreenShot",
) -> void:
	save_slot_scene = _save_slot_scene
	close_button = _close_button
	panel_title = _panel_title
	save_slot_container = _save_slot_container
	max_slot = _max_slots
	timestamp_node_path = _timestamp_node_path
	dialogue_node_path = _dialogue_node_path
	screenshot_node_path = _screenshot_node_path
	if close_button:
		close_button.pressed.connect(close_menu)

static func create_slots() -> void:
	panel_title.text = "Save Game" if is_save else "Load Game"
	save_slots.clear()
	for i in range(max_slot):
		var slot: Button = save_slot_scene.instantiate()
		save_slot_container.add_child(slot)
		save_slots.append(slot)
		var slot_index = i
		if is_save:
			slot.pressed.connect(func():_save_game(slot_index))
		else:
			slot.pressed.connect(func():_load_game(slot_index))
		_read_saves(slot_index)
		await save_load_scene_root_node.get_tree().process_frame

static func _read_saves(slot_index: int) -> void:
	var save_name = "save" + str(slot_index) + ".json"
	if SaveManager.save_exists(save_name):
		var slot: Button = save_slots[slot_index]
		var screenshot: TextureRect = slot.get_node(screenshot_node_path)
		var timestamp: Label = slot.get_node(timestamp_node_path)
		var dialogue: Label = slot.get_node(dialogue_node_path)
		var screenshot_bytes := SaveManager.get_data_from_json(save_name, "screenshot") as PackedByteArray
		screenshot.texture = ScreenshotToTextureRect.load_screenshot_form_bytes(screenshot_bytes)
		timestamp.text = SaveManager.get_data_from_json(save_name, "timestamp")
		dialogue.text = SaveManager.get_data_from_json(save_name, "currentDialogue")

static func _save_game(slot_index: int) -> void:
	var slot: Button = save_slots[slot_index]
	var screenshot: TextureRect = slot.get_node(screenshot_node_path)
	screenshot.texture = ScreenshotToTextureRect.screenshot
	var timestamp: Label = slot.get_node(timestamp_node_path)
	timestamp.text = Time.get_datetime_string_from_system()
	var dialogue: Label = slot.get_node(dialogue_node_path)
	dialogue.text = DialogueInterpreter.current_dialogue

	_data["slotIndex"] = slot_index

	var screenshot_bytes: PackedByteArray = ScreenshotToTextureRect.get_screenshot_bytes()
	
	_data["screenshot"] = screenshot_bytes

	_data["timestamp"] = timestamp.text

	_data["currentScript"] = ScriptReader.current_script_name

	_data["currentLine"] = BaseInterpreter.current_line - 1

	_data["scriptVariables"] = VariableInterpreter.get_local_variables()

	_data["currentDialogue"] = DialogueInterpreter.current_dialogue

	_data["currentSpeaker"] = DialogueInterpreter.current_speaker

	_data["onChoosing"] = ChoiceInterpreter.on_choosing

	if not AudioManager.bgm_player.stream: _data["currentBgm"] = ""
	else: _data["currentBgm"] = AudioManager.bgm_player.stream.resource_path.get_file()
	
	if not AudioManager.bgs_player.stream: _data["currentBgs"] = ""
	else: _data["currentBgs"] = AudioManager.bgs_player.stream.resource_path.get_file()

	if not AudioManager.se_player.stream: _data["currentSe"] = ""
	else: _data["currentSe"] = AudioManager.se_player.stream.resource_path.get_file()

	if not AudioManager.voice_player.stream: _data["currentVoice"] = ""
	else: _data["currentVoice"] = AudioManager.voice_player.stream.resource_path.get_file()

	if not figure_left.texture: _data["leftFigure"] = ""
	else: _data["leftFigure"] = figure_left.texture.get_path().get_file()

	if not figure_right.texture: _data["rightFigure"] = ""
	else: _data["rightFigure"] = figure_right.texture.get_path().get_file()

	if not figure_center.texture: _data["centerFigure"] = ""
	else: _data["centerFigure"] = figure_center.texture.get_path().get_file()

	if not portrait.texture: _data["portrait"] = ""
	else: _data["portrait"] = portrait.texture.get_path().get_file()

	if not background.texture: _data["background"] = ""
	else: _data["background"] = background.texture.get_path().get_file()

	SaveManager.save_as_json(_data, "save" + str(slot_index) + ".json")
	SaveManager.save_as_json(VariableInterpreter.get_global_variables(), "globalVariables.json")
	_data.clear()

static func _load_game(slot_index: int) -> void:
	var save_name = "save" + str(slot_index) + ".json"
	if SaveManager.save_exists(save_name):
		if save_load_scene_root_node.get_tree().current_scene.scene_file_path != main_scene_path:
			save_load_scene_root_node.get_tree().current_scene.queue_free()
			var scene: PackedScene = ResourceLoader.load(main_scene_path)
			var node: Node = scene.instantiate()
			save_load_scene_root_node.get_tree().root.add_child(node)
			save_load_scene_root_node.get_tree().current_scene = node
			recover_data(save_name)
			BaseInterpreter.execute_next_line()
			save_load_ui_active = false
			save_load_scene_root_node.queue_free()
		else:
			recover_data(save_name)
			save_load_ui_active = false
			save_load_scene_root_node.queue_free()

## 关闭存档界面
static func close_menu() -> void:
	save_load_ui_active = false
	save_load_scene_root_node.queue_free()

## 保存游戏配置
static func save_config() -> void:
	var config: Dictionary = {
		"masterVolume" : AudioManager.master_volume,
		"bgmVolume" : AudioManager.bgm_volume,
		"bgsVolume" : AudioManager.bgs_volume,
		"seVolume" : AudioManager.se_volume,
		"voiceVolume" : AudioManager.voice_volume,
		"autoPlaySpeed" : auto_play_interval,
		"textDisplaySpeed" : text_display_speed,
		"displayMode" : DisplayServer.window_get_mode(0)
	}
	SaveManager.save_as_json(config, "config.json")

## 加载游戏配置
static func load_config() -> void:
	var file_name = "config.json"
	if (SaveManager.save_exists(file_name)):
		var config: Dictionary = SaveManager.load_from_json(file_name)
		if config.has("masterVolume"): AudioManager.master_volume = config["masterVolume"]
		if config.has("bgmVolume"): AudioManager.bgm_volume = config["bgmVolume"]
		if config.has("bgsVolume"): AudioManager.bgs_volume = config["bgsVolume"]
		if config.has("seVolume"): AudioManager.se_volume = config["seVolume"]
		if config.has("voiceVolume"): AudioManager.voice_volume = config["voiceVolume"]
		if config.has("autoPlaySpeed"): auto_play_interval = config["autoPlaySpeed"]
		if config.has("textDisplaySpeed"): text_display_speed = config["textDisplaySpeed"]
		if config.has("displayMode"): DisplayServer.window_set_mode(config["displayMode"])

## 打开存档/加载界面 [br]
## [br]
## [param node] : [br]
## 父节点 [br]
## [param scene_path] : [br]
## 存档/加载界面场景路径 [br]
static func show_save_load_menu(node: Node, scene_path: String) -> void:
	var scene: PackedScene = ResourceLoader.load(scene_path)
	save_load_scene_root_node = scene.instantiate()
	node.add_child(save_load_scene_root_node)
	save_load_ui_active = true
