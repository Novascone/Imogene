using Godot;
using System;
using System.Net.Mime;

public partial class State : Node3D
{
	public StateMachine fsm;
	public string name;
	public ContextSteering entity;
	public Vector3 target_position;
	public Node3D collider;
	public CustomSignals _customSignals; // Custom signal instance
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Enter()
	{
		
	}

	public virtual void SetInterest()
	{

	}

	public virtual void SetDanger()
	{
		
	}

	public virtual void ChooseDirection()
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
