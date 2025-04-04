using Godot;
using System;
using System.Net.Mime;

public partial class State : Node3D
{
	public StateMachine FSM;
	public string StateName;
	// public ContextSteering entity;
	public Vector3 TargetPosition1;
	public Vector3 TargetPosition2;
	public Node3D Collider;
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

	public virtual void Enter(Enemy enemy)
	{
		
	}

	public virtual void SetInterest(Enemy enemy)
	{

	}

	public virtual void SetDanger(Enemy enemy)
	{
		
	}

	public virtual void ChooseDirection(Enemy enemy)
	{
		
	}

	public virtual void Exit(string nextState)
	{
		FSM.ChangeTo(nextState);
	}




}
