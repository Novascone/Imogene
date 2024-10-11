using Godot;
using System;

public partial class Hitscan : RangedAbility
{
	
	public Sprite3D cast_marker;
	
	[Export] public PackedScene cast_marker_scene;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rotate_on_soft = true;
		rotate_on_held = true;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(CheckHeld())
		// {
		// 	player.movement_controller.rotation_only = true;
		// 	// GD.Print("Ability is making player only able to rotate");
		// }
		// if(Input.IsActionJustReleased(assigned_button))
		// {
		// 	player.movement_controller.rotation_only = false;
		// }
		
		// // GD.Print("Projectile held " + button_held);
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		// {
		// 	QueueAbility();
		// }
		// else if (CheckHeld())
		// {
		// 	if(cast_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 		// GD.Print("using and holding ability");
		// 	}		
		// }
		// if(cast_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }	
	}

	public override void Execute(Player player)
	{
		// GD.Print("Casting");
		use_timer.Start();
		Vector3 collision = GetPlayerCollision(player); // Get collision point of raycast from player to object in from of them or 
		HitscanCollision(player, collision); // Create a raycast from cast point to player collision
	}

    public override void FrameCheck(Player player)
    {
        // if(CheckHeld())
		// {
		// 	player.movement_controller.rotation_only = true;
		// 	// GD.Print("Ability is making player only able to rotate");
		// }
		// if(Input.IsActionJustReleased(assigned_button))
		// {
		// 	player.movement_controller.rotation_only = false;
		// }
		
		// // GD.Print("Projectile held " + button_held);
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		// {
		// 	QueueAbility();
		// }
		// else if (CheckHeld())
		// {
		// 	if(cast_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 		// GD.Print("using and holding ability");
		// 	}		
		// }
		// if(cast_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }

		 if(CheckHeld()) // Check if button is held and only allow the player to rotate if it is
		{
			// player.controllers.movement_controller.rotation_only = true;
			GD.Print("player can only rotate");
		}
		if(Input.IsActionJustReleased(assigned_button)) // Allow the player to move fully if the button is released
		{
			if(MathF.Round(player.current_y_rotation - player.previous_y_rotation, 1) == 0)
			{
				EmitSignal(nameof(AbilityFinished),this);
			}
			
			// player.controllers.movement_controller.rotation_only = false;
		}
		if(Input.IsActionJustPressed(assigned_button) && state == States.NotQueued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			EmitSignal(nameof(AbilityQueue), player, this);
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(use_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue), player, this);
				EmitSignal(nameof(AbilityCheck), player, this);
			}		
		}
		if(use_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck), player, this);
		}			
    }

    public void HitscanCollision(Player player, Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized(); // Get the direction for the new raycast to go
		var new_intersection = PhysicsRayQueryParameters3D.Create(player.cast_point.GlobalTransform.Origin, collision_point + cast_direction * 2); // Create a new raycast with the origin being the cast point and the end being the collision point with direction and increase length
		new_intersection.CollisionMask = 16; // set Collision mask to 5
		new_intersection.CollideWithAreas = true; // Set raycast to collide with areas
		new_intersection.Exclude = player.excluded_rids; // Add player exclude
		var cast_collision = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection); // Get raycast collision

		if(cast_collision.Count > 0)
		{
			HitscanDamage((Node3D)cast_collision["collider"]);
			Node3D collider = (Node3D)cast_collision["collider"];
			var scene_node = cast_marker_scene.Instantiate();
			cast_marker = (Sprite3D)scene_node;
			var world = GetTree().Root;
			world.AddChild(cast_marker);
			cast_marker.GlobalTranslate(cast_collision["position"].AsVector3());
			// GD.Print("collided with" + collider.Name);
			
			
		}
	}

	public void HitscanDamage(Node3D collider) // Apply damage from hitscan if the collider is an enemy hurtbox
	{
		if(collider is	Hurtbox hurtbox)
		{
			
			if(hurtbox.Owner is Enemy enemy)
			{
				enemy.entity_systems.damage_system.TakeDamage(enemy, ranged_hitbox, 10.0f, false);
			}
			
		}
	}

	public void _on_cast_timer_timeout()
	{
		if(button_released)
		{
			EmitSignal(nameof(AbilityFinished), this);
		}
		
		// stop_movement_input = false;
	}
}
