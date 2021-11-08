using Godot;
using System;

public class FSMTest : Node2D
{
	public override void _Ready()
	{
		// reverse inject object to state machine
		StateMachineTest tester = new StateMachineTest(this);
		GD.Print("TESTING");
		this.AddChild(tester);
	}
}

public class StateMachineTest : StateMachine
{
	public StateMachineTest(Godot.Node2D playerEntity)
	{
		this.addState("Idling");
		this.addState("Walking");
		this.addState("Running");
	}
	public override void enterState(string newState, string oldState)
	{
		GD.Print($"Entered to {newState} from {oldState}");
	}

	public override void exitState(string oldState, string newState)
	{
		GD.Print($"Leaving to {newState} from {oldState}");
	}

	public override string getTransition(float delta)
	{
		switch (this.state)
		{
			case "Idling":				// State Graph
				return "Walking";		// Idling -> Walking -> Running
			case "Walking":				// State will always settle to running
				return "Running";
			case "Running":
				if(true) { // not walking code wont build with it being false
					return "Idling";
				}
			default:
				return null;
		}
	}

	public override void stateLogic(float delta)
	{
		if (this.state == "Idling")
		{
			// set player movement to 0, idle animate
		}
		if (this.state == "Walking")
		{
			// set player movement to 1, walk animate
		}
		if (this.state == "Running")
		{
			// set player movement to 2, run animate
		}
	}
}
