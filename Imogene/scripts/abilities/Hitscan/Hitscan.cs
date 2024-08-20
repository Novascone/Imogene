using Godot;
using System;

public partial class Hitscan : Ranged
{
	
	public Sprite3D cast_marker;
	public PackedScene scene;
	public Timer cast_timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scene = GD.Load<PackedScene>("res://scenes/debug/cast_marker.tscn");
		cast_timer = GetNode<Timer>("CastTimer");
		rotate_on_soft = true;
		rotate_on_held = true;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(CheckHeld())
		{
			player.movement_controller.rotation_only = true;
			GD.Print("Ability is making player only able to rotate");
		}
		if(Input.IsActionJustReleased(assigned_button))
		{
			player.movement_controller.rotation_only = false;
		}
		
		// GD.Print("Projectile held " + button_held);
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		{
			QueueAbility();
		}
		else if (CheckHeld())
		{
			if(cast_timer.TimeLeft == 0)
			{
				QueueAbility();
				CheckCanUseAbility();
				GD.Print("using and holding ability");
			}		
		}
		if(cast_timer.TimeLeft == 0)
		{
			CheckCanUseAbility();
		}	
	}

	public override void Execute()
	{
		// GD.Print("Casting");
		state = States.not_queued;
		player.movement_controller.movement_input_allowed = false;
		AddToAbilityList(this);
		cast_timer.Start();
		Vector3 collision = GetPlayerCollision(); // Get collision point of raycast from player to object in from of them or 
		HitscanCollision(collision); // Create a raycast from cast point to player collision
	}

	public void HitscanCollision(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized(); // Get the direction for the new raycast to go
		var new_intersection = PhysicsRayQueryParameters3D.Create(player.cast_point.GlobalTransform.Origin, collision_point + cast_direction * 2); // Create a new raycast with the origin being the cast point and the end being the collision point with direction and increase length
		new_intersection.CollisionMask = 16; // set Collision mask to 5
		new_intersection.CollideWithAreas = true; // Set raycast to collide with areas
		new_intersection.Exclude = player.exclude; // Add player exclude
		var cast_collision = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection); // Get raycast collision

		if(cast_collision.Count > 0)
		{
			HitscanDamage((Node3D)cast_collision["collider"]);
			Node3D collider = (Node3D)cast_collision["collider"];
			var scene_node = scene.Instantiate();
			cast_marker = (Sprite3D)scene_node;
			var world = GetTree().Root;
			world.AddChild(cast_marker);
			cast_marker.GlobalTranslate(cast_collision["position"].AsVector3());
			GD.Print("collided with" + collider.Name);
			
			
		}
	}

	public void HitscanDamage(Node3D collider) // Apply damage from hitscan if the collider is an enemy hurtbox
	{
		if(collider is	Hurtbox hurtbox)
		{
			
			if(hurtbox.Owner is Enemy enemy)
			{
				enemy.damage_system.TakeDamage("cold",10.0f,false);
			}
			
		}
	}

	public void _on_cast_timer_timeout()
	{
		RemoveFromAbilityList(this);
		player.movement_controller.movement_input_allowed = true;
	}
}
