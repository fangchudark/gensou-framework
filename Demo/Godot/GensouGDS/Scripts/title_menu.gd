extends Node

@export var main_scene_path := "res://Scenes/Main.tscn"

@export var save_load_scene_path := "res://Scenes/SaveLoad.tscn"

@export var config_scnen_path := "res://Scenes/Config.tscn"

func _ready():
	SaveLoadGame.load_config()
	VisualNoveCore.recover_global_variables()

func _on_new_game_button_pressed():
	get_tree().change_scene_to_file(main_scene_path)

func _on_load_game_button_pressed():
	SaveLoadGame.show_save_load_menu(get_tree().root, save_load_scene_path)

func _on_config_button_pressed():
	TextboxFunctions.open_config_ui(config_scnen_path, get_tree().root)

func _on_quit_button_pressed():
	get_tree().quit()
