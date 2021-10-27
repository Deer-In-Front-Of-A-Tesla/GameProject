"""
Authors: Emil Choparinov,
Created On: 26/10/2021
Description::
	This GameState resource creates a state that the game will be in at a given
	moment. It emits a changed event whenever the state is modified.
"""

extends Resource
class_name GameState

enum GameState {
	PAUSED,
	PLAY
};

export(GameState) var state = GameState.PLAY setget _set_state;

func _set_state(new_state: int) -> void:
	state = new_state;
	emit_changed();
