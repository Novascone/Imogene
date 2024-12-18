using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StateMachine : Node3D
{
	public State CurrentState;
	public Enemy Enemy;
	// public ContextSteering this_entity_context;
	public List<string> history = new List<string>();
	public Dictionary<string, State> states = new Dictionary<string, State>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(State state in GetChildren().Cast<State>())
		{
			state.FSM = this;
			// GD.Print("state " + state.name + " set");
			states[state.StateName] = state;

			if(CurrentState != null)
			{
				RemoveChild(state);
			}
			else
			{
				CurrentState = state;
			}
		}
		CurrentState.Enter(Enemy);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void ChangeTo(string stateName)
	{
		// GD.Print("State machine changing");
		history.Add(CurrentState.StateName);
		SetState(stateName);
	}

	public void Back()
	{
		if (history.Count > 0)
		{
			SetState(history.Last());
			history.RemoveAt(history.Count - 1);
		}
	}

	public void SetState(string stateName)
	{
		RemoveChild(CurrentState);
		CurrentState = states[stateName];
		AddChild(CurrentState);
		CurrentState.Enter(Enemy);
	}
	

	// public void GetEntityInfoContextTest(ContextSteering s)
	// {
	// 	this_entity_context = s;
	// }
	
}
