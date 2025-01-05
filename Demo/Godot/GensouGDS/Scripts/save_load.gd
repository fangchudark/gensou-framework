extends Node

@export var max_slots := 20

@export var save_slots_scene: PackedScene

@export var close_button: Button

@export var panel_title: Label

@export var save_slots_container: VBoxContainer

@export var timestamp_node_path := "Timestamp"

@export var dialogue_node_path := "Dialogue"

@export var screenshot_node_path := "Screenshot"

func _enter_tree():
	SaveLoadGame.init_save_load_game(
		save_slots_scene,
		close_button,
		panel_title,
		save_slots_container,
		max_slots,
		timestamp_node_path,
		dialogue_node_path,
		screenshot_node_path
	)

func _ready():
	await SaveLoadGame.create_slots()
