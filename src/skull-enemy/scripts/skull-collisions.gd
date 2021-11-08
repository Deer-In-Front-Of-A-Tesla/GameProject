extends StaticBody2D

export(NodePath) onready var _skull = get_node(_skull) as Node2D;

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func TakeDamage(value):
	_skull.hp -= 5;
	if(_skull.hp <= 0):
		_skull.queue_free();
