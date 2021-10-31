extends ItemAPI;

export(NodePath) onready var _item = get_node(_item) as Area2D;
export(NodePath) onready var _decay_timer = get_node(_decay_timer) as Timer;
export(Resource) var _item_data = _item_data as HpDecayItem;

var _collision_occured = false;

func _ready():
	_item.connect("body_entered", self, "_on_collide");

func _should_use_item() -> bool:
	if(_collision_occured):
		return true;
	return false;

func _apply_item() -> void:
	_entity.shield_hp = _item_data.shield_amount;
	_decay_timer.start();

	_item.visible = false;
	_item.monitoring = false;
	_collision_occured = false;

func _on_collide(collider):
	print(collider);
	# TODO: This might not be the best check for player, depending on if enemies
	# are kinematics we'll have to do a smarter check
	if(collider is KinematicBody2D):
		_collision_occured = true;
