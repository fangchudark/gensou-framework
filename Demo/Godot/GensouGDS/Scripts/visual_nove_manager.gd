extends Node

@export var character_name: Label

@export var dialogue_text: Label

@export var figure_left: TextureRect

@export var figure_center: TextureRect

@export var figure_right: TextureRect

@export var portrait: TextureRect

@export var background: TextureRect

@export var bgm: AudioStreamPlayer

@export var bgs: AudioStreamPlayer

@export var se: AudioStreamPlayer

@export var voice: AudioStreamPlayer

@export var choice_button_container: VBoxContainer

@export var choice_button_scene: PackedScene

@export var text_box: Panel

@export var skip_button: Button

@export var auto_button: Button

@export var save_button: Button

@export var load_button: Button

@export var log_button: Button

@export var system_button: Button

@export var title_button: Button

@export var log_container: VBoxContainer

@export var log_text_scene: PackedScene

@export var scroll_view: ScrollContainer

@export var typewriter_effect: DisplayController

@export var log_panel: Panel

@export var close_log_panel_button: Button

@export var auto_play_interval: int = 1

func _enter_tree():
	VisualNoveCore.init(
		character_name,
		dialogue_text,
		typewriter_effect,
		figure_left,
		figure_center,
		figure_right,
		portrait,
		background,
		bgm,
		bgs,
		se,
		voice,
		self,
		choice_button_container,
		choice_button_scene,
		text_box,
		auto_play_interval
	)

	TextboxFunctions.init_textbox_functions(
		skip_button,
		auto_button,
		save_button,
		load_button,
		log_button,
		system_button,
		title_button,
		log_container,
		log_text_scene,
		scroll_view,
		log_panel,
		close_log_panel_button
	)

func _ready():
	ScriptReader.read_and_execute("demo")
