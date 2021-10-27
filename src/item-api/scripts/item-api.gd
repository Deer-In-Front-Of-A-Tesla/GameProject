class_name ItemAPI
extends Node;

export(Resource) var _entity = _entity as mainplayer;

signal item_applied;

func _process(delta):
	if(_should_use_item()):
		_apply_item();
		emit_signal("item_applied");
	
func _should_use_item() -> bool:
	print("[ItemAPI] _should_use_item() is undefined, ensure all functions are declared.");
	queue_free();
	return false;

func _apply_item() -> void:
	print("[ItemAPI] _apply_item() is undefined, ensure all functions are declared.");
