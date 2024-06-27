using Godot;
using System;
using System.Net.Mime;

public partial class State : Node3D
{
	public StateMachine fsm;
	public string name;
	public ContextSteering entity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Enter()
	{
		
	}

	public virtual void Exit(string next_state)
	{
		fsm.ChangeTo(next_state);
	}

	public void GetEntityInfo(ContextSteering s)
	{
		entity = s;
	}



}
