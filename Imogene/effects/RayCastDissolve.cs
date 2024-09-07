using Godot;
using System;
using System.ComponentModel;

public partial class RayCastDissolve : Node3D
{
	// Called when the node enters the scene tree for the first time.

	[Export] Camera3D camera;
	[Export] Player character;
	[Signal] public delegate void  collidingEventHandler(int id);
	[Signal] public delegate void  notcollidingEventHandler();
	

	public override void _Ready()
	{
		// _customSignals.UIPreventingMovement += HandleUIPreventingMovement;
		
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		var from = camera.GlobalPosition;
		var to = character.GlobalPosition;
		var space = GetWorld3D().DirectSpaceState;
		var ray_query = PhysicsRayQueryParameters3D.Create(from, to);
		ray_query.From = from;
		ray_query.To = to;
		ray_query.Exclude = character.exclude;
		// ray_query.CollideWithAreas = true;
		// ray_query.Exclude.Add(character.GetRid());
		
		
		var result = space.IntersectRay(ray_query);

		int id = 0;
		if(result.ContainsKey("collider_id"))
		{
			id = (int)result["collider_id"];
			StaticBody3D static_body = (StaticBody3D)result["collider"];
			// GD.Print(static_body.Name);
			// GD.Print("collider_id");
			EmitSignal(nameof(colliding), id);
		}
		else
		{
			EmitSignal(nameof(notcolliding));
		}

	}
}
