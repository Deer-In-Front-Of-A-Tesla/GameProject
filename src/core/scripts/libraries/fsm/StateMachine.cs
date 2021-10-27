/**
Authors: Emil Choparinov,
Created On: 26/10/2021
Description:
	Go look at the GD script version for documentation
*/

using Godot;
using System.Collections.Generic;

public abstract class StateMachine : Node
{
	private Dictionary<string, string> states = new Dictionary<string, string>();
	public string state = null;

	public abstract void stateLogic(float delta);
	public abstract string getTransition(float delta);
	public abstract void enterState(string newState, string oldState);
	public abstract void exitState(string oldState, string newState);

	public void addState(string newState)
	{
		if (state == null)
			this.state = newState;
		this.states.Add(newState, newState);
	}
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		if (state != null)
		{
			this.stateLogic(delta);
			string trans = getTransition(delta);
			if (trans != null)
				setState(trans);
		}
	}

	private void setState(string newState)
	{
		string previousState = this.state;
		this.state = newState;

		if (previousState != null)
			exitState(previousState, this.state);
		if (newState != null)
			enterState(this.state, previousState);
	}
}
