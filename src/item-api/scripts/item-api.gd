"""
Authors: Emil Choparinov,
Created On: 27/10/2021
Description::
	Use this small framework to manage item creation logic.
	
	Note: This API does not manage deletion, your code will have to handle this
	yourself.
"""

class_name ItemAPI
extends Node;

export(Resource) var _entity = _entity as mainplayer;

signal item_applied;

# DONT EXTEND
# If you do you will break the framework unless you extend it with these three
# lines as well
func _process(_delta):
	if(_should_use_item()):
		_apply_item();
		emit_signal("item_applied");
	
func _should_use_item() -> bool:
	print("[ItemAPI] _should_use_item() is undefined, ensure all functions are declared.");
	queue_free();
	return false;

func _apply_item() -> void:
	print("[ItemAPI] _apply_item() is undefined, ensure all functions are declared.");
