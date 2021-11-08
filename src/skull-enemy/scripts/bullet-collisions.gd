extends Area2D

export(Resource) onready var _player = _player as mainplayer;
export(NodePath) onready var _bullet = get_node(_bullet) as Node2D;

# Called when the node enters the scene tree for the first time.
func _ready():
	self.connect("body_entered", self, "_on_body_entered");
	
func _on_body_entered(node) -> void:
	if(self.collision_layer == 2): # wall
		_bullet.queue_free();
	if(node.has_method("TakeDamage")):
		node.call("TakeDamage", 5);
