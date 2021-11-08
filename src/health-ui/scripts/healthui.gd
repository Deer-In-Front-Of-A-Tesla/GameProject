extends Node

export(NodePath) onready var _hp = get_node(_hp) as Label;
export(NodePath) onready var _shield = get_node(_shield) as Label;

export(Resource) onready var _player = _player as mainplayer;

# Called when the node enters the scene tree for the first time.
func _ready():
	draw_health_ui();
	_player.connect("changed", self, "draw_health_ui");

func draw_health_ui() -> void:
	_hp.text = str(_player.hp) + " HP";
	_shield.text = str(_player.shield_hp) + " SHIELD";
