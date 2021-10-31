extends Timer


export(NodePath) onready var _item = get_node(_item) as Area2D;
export(Resource) var _entity = _entity as mainplayer;


# Called when the node enters the scene tree for the first time.
func _ready():
	print("in scene");
	connect("timeout", self, "_on_timeout");
	
func _on_timeout():
	if(_entity.shield_hp <= 0):
		_item.queue_free();
	else:
		_entity.shield_hp -= 1;
