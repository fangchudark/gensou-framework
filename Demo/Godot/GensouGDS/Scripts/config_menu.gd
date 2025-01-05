extends Node

@export var master_volume_slider: HSlider

@export var bgm_volume_slider: HSlider

@export var bgs_volume_slider: HSlider

@export var se_volume_slider: HSlider

@export var voice_volume_slider: HSlider

@export var auto_play_speed_slider: HSlider

@export var text_display_speed_slider: HSlider

@export var display_mode_option_button: OptionButton

@export var close: Button

@export var save: Button

func _ready():
	master_volume_slider.value = AudioManager.master_volume
	bgm_volume_slider.value = AudioManager.bgm_volume
	bgs_volume_slider.value = AudioManager.bgs_volume
	se_volume_slider.value = AudioManager.se_volume
	voice_volume_slider.value = AudioManager.voice_volume

	var current_interval: float = VisualNoveCore.auto_play_interval
	var t: float = (auto_play_speed_slider.max_value - current_interval) / (auto_play_speed_slider.max_value - auto_play_speed_slider.min_value)
	auto_play_speed_slider.value = lerp(auto_play_speed_slider.min_value, auto_play_speed_slider.max_value, t)

	var current_speed: float = VisualNoveCore.text_display_speed
	t = (text_display_speed_slider.max_value - current_speed) / (text_display_speed_slider.max_value - text_display_speed_slider.min_value)
	text_display_speed_slider.value = lerp(text_display_speed_slider.min_value, text_display_speed_slider.max_value, t)

	if DisplayServer.window_get_mode() == DisplayServer.WINDOW_MODE_FULLSCREEN:
		display_mode_option_button.selected = 0
	else:
		display_mode_option_button.selected = 1

	close.pressed.connect(VisualNoveCore.close_config_ui)
	close.pressed.connect(SaveLoadGame.save_config)

func set_display(index: int) -> void:
	match index:
		0:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
		1:
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)

func set_text_display_speed(value: float) -> void:
	var t: float = (value - text_display_speed_slider.min_value) / (text_display_speed_slider.max_value - text_display_speed_slider.min_value)
	VisualNoveCore.text_display_speed = lerp(text_display_speed_slider.max_value, text_display_speed_slider.min_value, t)

func set_auto_play_speed(value: float) -> void:
	var t: float = (value - auto_play_speed_slider.min_value) / (auto_play_speed_slider.max_value - auto_play_speed_slider.min_value)
	VisualNoveCore.auto_play_interval = lerp(auto_play_speed_slider.max_value, auto_play_speed_slider.min_value, t)

func set_master_volume(value: float) -> void:
	AudioManager.master_volume = value

func set_bgm_volume(value: float) -> void:
	AudioManager.bgm_volume = value

func set_bgs_volume(value: float) -> void:
	AudioManager.bgs_volume = value

func set_se_volume(value: float) -> void:
	AudioManager.se_volume = value

func set_voice_volume(value: float) -> void:
	AudioManager.voice_volume = value
