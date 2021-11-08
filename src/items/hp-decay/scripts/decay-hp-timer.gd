extends Timer


export(NodePath) onready var _item = get_node(_item) as Area2D;
export(Resource) var _entity = _entity as mainplayer;

var _another_item_taken: bool = false;
var _previous_tick_shield_hp: int = 999;

# Called when the node enters the scene tree for the first time.
func _ready():
	connect("timeout", self, "_on_timeout");
	
func _on_timeout() -> void:
	
	# if a user picks up 3 shields, we don't want them to fall 3x faster
	if(_previous_tick_shield_hp < _entity.shield_hp):
		_item.queue_free();

	if(_entity.shield_hp <= 0):
		_item.queue_free();
	else:
		_previous_tick_shield_hp = _entity.shield_hp;
		_entity.shield_hp -= 1;
