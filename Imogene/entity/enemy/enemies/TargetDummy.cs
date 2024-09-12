using Godot;
using System;

public partial class TargetDummy : Enemy
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		movement_stats["speed"] = movement_stats["walk_speed"];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		// if(Name == "TargetDummy2")
		// {
		// 	GD.Print("Effects count" + status_effects.Count);
		// 	GD.Print(" effect already applied " + status_effect_controller.effect_already_applied );
		// }
		
	}
}
