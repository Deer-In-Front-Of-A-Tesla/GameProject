"""
Authors: Emil Choparinov,
Created On: 21/10/2021
Description::
		The Navigator API helps manage scene history for swapping within menus. 
		Useful for embedded settings screens for pause menus as an example.
"""
extends Node

var _history: Array = [];

func _ready():
	var current_scene = PackedScene.new();
	current_scene.pack(get_tree().get_current_scene());
	_history.append(current_scene);

func previous() -> void:
	# we can only really delete until the first scene added, if we delete any
	# further undefined behaviour can happen. Also at that point just quit the
	# game
	if(_history.size() == 1):
		print("[Navigator] error: Cannot return to previous scene. Scene History is empty");
		return;
	var prev_scene = _history[_history.size() - 2];
	_history.pop_back();
	get_tree().change_scene_to(prev_scene);

func navigate(nextScene: PackedScene) -> void:
	_history.append(nextScene);
	get_tree().change_scene_to(_history.back());
