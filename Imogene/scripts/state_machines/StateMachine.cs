using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StateMachine : Node3D
{
	public State current_state;
	public ContextSteering this_entity;
	public List<string> history = new List<string>();
	public Dictionary<string, State> states = new Dictionary<string, State>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(State state in GetChildren())
		{
			state.fsm = this;
			GD.Print("state " + state.name + " set");
			states[state.name] = state;

			if(current_state != null)
			{
				RemoveChild(state);
			}
			else
			{
				current_state = state;
			}
		}
		current_state.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void ChangeTo(string state_name)
	{
		history.Add(current_state.name);
		SetState(state_name);
	}

	public void Back()
	{
		if (history.Count > 0)
		{
			SetState(history.Last());
			history.RemoveAt(history.Count - 1);
		}
	}

	public void SetState(string state_name)
	{
		RemoveChild(current_state);
		current_state = states[state_name];
		AddChild(current_state);
		current_state.Enter();
	}
	
	public void GetEntityInfo(ContextSteering s)
	{
		this_entity = s;
	}
	
}
