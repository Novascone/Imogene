using Godot;
using System;

public partial class Hitscan : Ability
{
	public int range = 100;
	public Sprite3D cast_marker;
	PackedScene scene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scene = GD.Load<PackedScene>("res://scenes/debug/cast_marker.tscn");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(player.can_use_abilities && useable && button_pressed && CheckCross())
		{
			Execute();
		}
	}

	public override void Execute()
	{
		// GD.Print("Casting");
		Vector3 collision = GetPlayerCollision();
		HitscanCollision(collision);
	}

	public Vector3 GetPlayerCollision()
	{
		Vector3 ray_origin = player.GlobalTransform.Origin;
		Vector3 ray_end = ray_origin + -player.Transform.Basis.Z * range;
		var new_intersection = PhysicsRayQueryParameters3D.Create(ray_origin, ray_end);
		new_intersection.Exclude = player.exclude;
		var intersection = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection);

		if(intersection.Count > 0)
		{
			Vector3 collision_point = intersection["position"].AsVector3();
			Node3D collider = (Node3D)intersection["collider"];
			// GD.Print("collided with" + collider.Name);
			
			return collision_point;
		}
		else
		{
			return ray_end;
		}
	}

	public void HitscanCollision(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized();
		var new_intersection = PhysicsRayQueryParameters3D.Create(player.cast_point.GlobalTransform.Origin, collision_point + cast_direction * 2);
		new_intersection.Exclude = player.exclude;
		var cast_collision = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection);

		if(cast_collision.Count > 0)
		{
			HitscanDamage((Node3D)cast_collision["collider"]);
			Node3D collider = (Node3D)cast_collision["collider"];
			var scene_node = scene.Instantiate();
			cast_marker = (Sprite3D)scene_node;
			var world = GetTree().Root;
			world.AddChild(cast_marker);
			cast_marker.GlobalTranslate(cast_collision["position"].AsVector3());
			// GD.Print("collided with" + collider.Name);
			
			
		}
	}

	public void HitscanDamage(Node3D collider)
	{
		if(collider.IsInGroup("enemy"))
		{
			// GD.Print("enemy hit");
		}
	}
}
