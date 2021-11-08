class_name SkullEnemy
extends Node2D;

export(int) onready var hp;
export(String, "GREEN", "RED") onready var type;

var COLOR_GREEN: Color = Color(1, 1, 1, 1);
var COLOR_RED: Color = Color(.91, .08, .08, 1);

export(int) onready var DISTANCE;

var _origin: Vector2;
var _reverse_dir: bool = false;
func _ready():
	_origin = self.position;

func get_color() -> Color:
	if(type == "RED"): return COLOR_RED;
	return COLOR_GREEN;
