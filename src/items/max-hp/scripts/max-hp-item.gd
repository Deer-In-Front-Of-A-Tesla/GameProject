"""
Authors: Emil Choparinov,
Created On: 27/10/2021
Description::
	Max HP Item Implementiaton.
"""
extends ItemAPI;

export(NodePath) onready var _item = get_node(_item) as Area2D;

var _collision_occured = false;

func _ready():
	_item.connect("body_entered", self, "_on_collide");

func _should_use_item() -> bool:
	if(_collision_occured):
		return true;
	return false;

func _apply_item() -> void:
	# once we apply, we delete ourselves
	_entity.hp = _entity.maxhp;
	_item.queue_free();

func _on_collide(collider):
	# TODO: This might not be the best check for player, depending on if enemies
	# are kinematics we'll have to do a smarter check
	if(collider is KinematicBody2D):
		_collision_occured = true;
