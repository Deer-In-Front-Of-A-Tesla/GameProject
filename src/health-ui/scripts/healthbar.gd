"""
Authors: Emil Choparinov,
Created On: 10/07/2021
Description::
	Calculates and creates the health bar.
"""

extends Node

#---- IMPORTS ------------------------------------------------------------------
export(NodePath) onready var _redhealth = get_node(_redhealth) as TextureRect;
export(NodePath) onready var _bluehealth = get_node(_bluehealth) as TextureRect;
export(Resource) onready var _player = _player as mainplayer;

#---- INTERNAL VARS ------------------------------------------------------------
# ŷ = 0.1134X + 0.10567 from reg test
var STEP_SIZE = 0.113;

# ŷ = 6.94743X + 0.80643 from reg test
var BLUE_START_STEP_AMOUNT =  6.948;
var BLUE_START = 0.81;

func _ready():
	draw_health_bar();
	_player.connect("changed", self, "draw_health_bar");

#---- IMPLEMENTATION -----------------------------------------------------------
func draw_health_bar() -> void:
	# calculate red health bar
	var red_step = convert_health_to_step(_player.hp);
	_redhealth.rect_scale = Vector2(STEP_SIZE * red_step, _redhealth.rect_scale.y);
	
	# calculate blue health bar
	var blue_step = convert_health_to_step(_player.shield_hp);
	
	blue_step = clamp(blue_step, 0, 9 - red_step);
	
	_bluehealth.rect_position = Vector2(BLUE_START + BLUE_START_STEP_AMOUNT * red_step,_bluehealth.rect_position.y);
	_bluehealth.rect_scale = Vector2(STEP_SIZE * blue_step, _redhealth.rect_scale.y);

# Pure conversion function that takes in a health value between [0-any) and
# returns between [1-9]. Useful for rendering health bars
func convert_health_to_step(health: int) -> int:
	var step = ceil(float(health) / 10);
	if(step > 9): step = 9;
	print(step);
	return step;
