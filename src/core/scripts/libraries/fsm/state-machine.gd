"""
Authors: Emil Choparinov,
Created On: 26/10/2021
Description::
	An simple state machine implementation to be extended and used to monitor
	shallow states (prev, cu).
"""

# Note: May need extension to to include networking bandwidth later on 
# go ask Emil if we do so
extends Node
class_name StateMachine

# ==============================================================================
# VIRTUAL METHODS TO IMPLEMENT WHEN USING STATE MACHINE CLASS
# ==============================================================================

# method runs on physics process if state exists, manually edit your stateful
# interactions here
func _state_logic(delta: float) -> void:
	pass

# method runs on physics process, set next or keep current state alive here
func _get_transition(delta: float):
	return null;

# if a state change has occured, this will handle state changes
func _enter_state(new_state, old_state) -> void:
	pass

# if a state change has occured, this will handle the exiting of the previous
# state
func _exit_state(old_state, new_state):
	pass

# ==============================================================================
# CAREFUL: CORE STATE MACHINE CODE HERE, DONT FUCKING TOUCH
# ==============================================================================

var state = null setget set_state;
var previous_state = null;

var states = { };
onready var subject = get_parent();
	
func _physics_process(delta: float):
	if (state != null):
		_state_logic(delta);
		var trans = _get_transition(delta);
		if(trans != null):
			set_state(trans);

func set_state(new_state):
	previous_state = state;
	state = new_state;
	
	if(previous_state != null):
		_exit_state(previous_state, state);
	if(new_state != null):
		_enter_state(state, previous_state);
		
func add_state(state_name: String):
	states[state_name] = state_name;
