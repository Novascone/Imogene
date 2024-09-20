using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;


// Movement Controller
// Moves the player character around the world, sets basic animations, implements the landing icon, applies smooth rotation, handles looking at enemies, and prevents movement when UI is open
public partial class MovementController : Node
{

	Vector3 ray_origin;
	Vector3 ray_target = new  Vector3(0, -4, 0);
	int ray_range = 2000;
	private float climb_speed = 4.0f;
	private float clamber_speed = 10.0f;
	public bool rotation_only;
	public bool rotation_finished;
	public bool movement_input_prevented = false;
	Vector3 tether_position;
	public bool movement_tethered = false;
	StatModifier walk = new (StatModifier.ModificationType.multiply_current);

    public override void _Ready()
    {
        walk.mod = -0.5f;
    }



    public void MovePlayer(Player player, float input_strength, double delta)
	{
		
		if(!player.is_climbing)
		{
			StandardMovement(player, input_strength, delta);
		}
		else
		{
			ClimbingMovement(player);
		}
		if(!player.using_movement_ability && !rotation_only &&!movement_input_prevented)
		{
			player.velocity.X = player.direction.X * player.movement_speed.current_value;
			player.velocity.Z = player.direction.Z * player.movement_speed.current_value;			
		}
		else if(rotation_only || movement_input_prevented)
		{
			player.velocity = Vector3.Zero;
		}
			
		player.Velocity = player.velocity;

		if(movement_tethered)
		{
			GD.Print("movement tethered");
			player.GlobalPosition = (player.GlobalPosition - tether_position).LimitLength(10) + tether_position;
		}

			
	}

    private void StandardMovement(Player player, float input_strength, double delta)
    {
        if(player.IsOnFloor())
		{
			if(input_strength < 0.75f)
			{
				if(player.movement_speed.current_value == player.movement_speed.base_value)
				{
					player.movement_speed.AddModifier(walk);
				}
			}
			else
			{
				if(player.movement_speed.current_value < player.movement_speed.base_value)
				{
					player.movement_speed.RemoveModifier(walk);
				}
				
			}
		}
		// else
		// {
		// 	player.movement_stats["speed"] = Mathf.Lerp(player.movement_stats["speed"], 2.0f, 0.1f);
		// }

		if(!player.IsOnFloor())
		{
			player.velocity.Y += (float)(SetGravity(player) * delta);
		}
		
		LookForward(player,player.direction);
    }

    public float SetGravity(Player player)
	{
		if(player.Velocity.Y > 0)
		{

			return player.jump_gravity;
		}
		else
		{
			return player.fall_gravity;
		}
	}

	public void LookForward(Player player, Vector3 direction) // Rotates the player character smoothly with lerp
	{
		// GD.Print("Rotating smoothly");
		if(!player.systems.targeting_system.targeting && !player.is_climbing)
		{
			player.prev_y_rotation = player.GlobalRotation.Y;
			if (player.GlobalTransform.Origin != player.GlobalPosition + direction with {Y = 0}) // looks at direction the player is moving
			{
				player.LookAt(player.GlobalPosition + direction with { Y = 0 });
			}
			player.current_y_rotation = player.GlobalRotation.Y;
			if(player.prev_y_rotation != player.current_y_rotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}


	public bool StatusEffectsPreventingMovement(Player player)
	{
		if(player.entity_controllers.status_effect_controller.movement_prevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool StatusEffectsAffectingSpeed(Player player)
	{
		if (player.entity_controllers.status_effect_controller.speed_altered)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	// public bool CanMove(Player player)
	// {
		
	// 	if(StatusEffectsPreventingMovement(player))
	// 	{
	// 		return false;
	// 	}

	// 	// if(player.ui.inventory.Visible) 
	// 	// {
	// 	// 	return false;
	// 	// }
	// 	// else if (!player.ui.inventory_open || !player.ui.abilities_open && player.ui.abilities_secondary_ui_open)
	// 	// {
	// 	// 	return true;
	// 	// }

	// 	return true;
		
	// }

	public void ClimbingMovement(Player player) // Takes climbing input and moves the character when climbing
	{
		{
			GD.Print("Climbing movement");
			// player.direction = Vector3.Zero;
			// player.velocity.Y = player.direction.Y * player.movement_stats["speed"];
			// if(!player.is_clambering)
			// {
			// 	player.movement_stats["speed"]= climb_speed;
			// }
			// else
			// {
			// 	player.movement_stats["speed"] = clamber_speed;
			// }
			
		}

		ClimbingRotation(player);

		if(player.direction != Vector3.Zero && player.is_climbing)
		{
			player.prev_y_rotation = player.GlobalRotation.Y;
			player.current_y_rotation = -(MathF.Atan2(player.controllers.near_wall.GetCollisionNormal().Z, player.controllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Set the player y rotation to the rotation needed to face the wall
			if(player.prev_y_rotation != player.current_y_rotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	public void ClimbingRotation(Player player) // Sets the rotation of the player when climbing
	{
		var rot = -(MathF.Atan2(player.controllers.near_wall.GetCollisionNormal().Z, player.controllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Get the angle of rotation needed to face the object climbing
		
		player.vertical_input = Input.GetActionStrength("Forward") - Input.GetActionStrength("Backward");
		
		var horizontal_input = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		if(!player.is_clambering)
		{
			player.direction = new Vector3(horizontal_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***

		}
		player.direction = new Vector3(horizontal_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***
	}

	public Godot.Collections.Dictionary LandPosition(Player player) // Sends out raycast to determine where the player will land
	{
		ray_origin = player.GlobalTransform.Origin;
		ray_target = player.GlobalTransform.Origin + new Vector3(0, -20, 0);
		var spaceState = player.GetWorld3D().DirectSpaceState;
		var ray_query = PhysicsRayQueryParameters3D.Create(ray_origin, ray_target);
		ray_query.CollideWithAreas = true;
		ray_query.Exclude = player.exclude;
		var ray = spaceState.IntersectRay(ray_query);
		return ray;
	}

    internal void HandleMovementPrevented(bool movement_prevented)
    {
        movement_input_prevented = movement_prevented;
    }

    internal void HandleTethered(Entity entity, MeshInstance3D tether, bool tethered)
    {
		movement_tethered = tethered;
		if(tethered)
		{
			tether_position = tether.GlobalPosition;
		}
		else
		{
			tether_position = Vector3.Zero;
		}
    }
}
