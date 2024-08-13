using Godot;
using System;

public partial class Ranged : Ability
{
	public int range = 100;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Vector3 GetPlayerCollision()
	{
		Vector3 ray_origin = player.GlobalTransform.Origin;
		Vector3 ray_end = ray_origin + -player.Transform.Basis.Z * range;
		var new_intersection = PhysicsRayQueryParameters3D.Create(ray_origin, ray_end);
		new_intersection.CollisionMask = 16;
		new_intersection.CollideWithAreas = true;
		new_intersection.Exclude = player.exclude;
		var intersection = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection);

		if(intersection.Count > 0)
		{
			Vector3 collision_point = intersection["position"].AsVector3();
			Node3D collider = (Node3D)intersection["collider"];
			GD.Print("collided with" + collider.Name);
			
			return collision_point;
		}
		else
		{
			return ray_end;
		}
	}
}
