using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;


// Movement Controller
// Moves the player character around the world, sets basic animations, implements the landing icon, applies smooth rotation, handles looking at enemies, and prevents movement when UI is open
public partial class MovementController : Node
{

	// Landing point 
	Vector3 ray_origin;
	Vector3 ray_target = new  Vector3(0, -4, 0);
	int ray_range = 2000;

	// Climbing
	private float climb_speed = 4.0f;
	private float clamber_speed = 10.0f;
	public bool climbing = false;
	public bool clambering = false;
	private float vertical_climbing_input;
	private float horizontal_climbing_input;

	// Input
	public bool rotation_finished;
	public bool movement_input_prevented = false;
	public bool movement_ability_in_use = false;
	Vector3 tether_position;
	public float movement_from_tether = 0;
	public bool movement_tethered = false;
	

    public void MovePlayer(Player player, float input_strength, double delta)
	{
		
		if(!climbing)
		{
			StandardMovement(player, input_strength, delta);
		}
		else
		{
			ClimbingMovement(player);
		}
	
		player.Velocity = player.velocity;
		

		if(movement_tethered)
		{
			GD.Print("movement tethered");
			player.GlobalPosition = (player.GlobalPosition - tether_position).LimitLength(movement_from_tether) + tether_position;
		}

			
	}

    private void StandardMovement(Player player, float input_strength, double delta)
    {
		if(!movement_ability_in_use && !movement_input_prevented)
		{
			player.velocity.X = player.direction.X * player.movement_speed.current_value * input_strength;
			player.velocity.Z = player.direction.Z * player.movement_speed.current_value * input_strength;
			
		}
		else if(movement_input_prevented)
		{
			
			player.velocity = Vector3.Zero with { Y = player.fall_gravity / 15};
		}
		if(!player.IsOnFloor())
		{
			player.velocity.Y += (float)(SetGravity(player) * delta);
			GD.Print("Player not on floor");
		}

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

		if(player.direction != Vector3.Zero && climbing)
		{
			player.previous_y_rotation = player.GlobalRotation.Y;
			player.current_y_rotation = -(MathF.Atan2(player.controllers.near_wall.GetCollisionNormal().Z, player.controllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Set the player y rotation to the rotation needed to face the wall
			if(player.previous_y_rotation != player.current_y_rotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.previous_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	public void ClimbingRotation(Player player) // Sets the rotation of the player when climbing
	{
		var rot = -(MathF.Atan2(player.controllers.near_wall.GetCollisionNormal().Z, player.controllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Get the angle of rotation needed to face the object climbing
		
		vertical_climbing_input = Input.GetActionStrength("Forward") - Input.GetActionStrength("Backward");
		horizontal_climbing_input = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		
		if(!clambering)
		{
			player.direction = new Vector3(horizontal_climbing_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***

		}
		// player.direction = new Vector3(horizontal_climbing_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***
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

	// Signals

	public void Subscribe(Player player)
	{
		player.entity_controllers.status_effect_controller.MovementPrevented += HandleMovementPrevented;
		player.entity_controllers.status_effect_controller.Tethered += HandleTethered;

		player.systems.targeting_system.Rotating += HandleRotatePlayer;

		player.ui.CapturingInput += HandleUICapturingInput;
	}

	public void AbilitySubscribe(Ability ability)
	{
		ability.AbilityExecuting += OnAbilityExecuting;
		ability.AbilityFinished += OnAbilityFinished;
		ability.MovementAbilityExecuted += OnMovementAbilityExecuted;
	}

    public void Unsubscribe(Player player)
	{
		player.entity_controllers.status_effect_controller.MovementPrevented -= HandleMovementPrevented;
		player.entity_controllers.status_effect_controller.Tethered -= HandleTethered;

		player.systems.targeting_system.Rotating -= HandleRotatePlayer;

		player.ui.CapturingInput -= HandleUICapturingInput;
	}

	public void AbilityUnsubscribe(Ability ability)
	{
		ability.AbilityExecuting -= OnAbilityExecuting;
		ability.AbilityFinished -= OnAbilityFinished;
	}

    internal void HandleMovementPrevented(bool movement_prevented) // Listen for signal from StatusEffectController.cs that prevents movement
    {
        movement_input_prevented = movement_prevented;
    }

	public void HandleClimbing(bool is_climbing) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun climbing or stopped climbing
	{
		climbing = is_climbing;
	}

	public void HandleClambering(bool is_clambering) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun clambering or stopped climbing
	{
		clambering = is_clambering;
	}

    internal void HandleTethered(Entity entity, MeshInstance3D tether, bool tethered, float tether_length) // Listen for signal from StatusEffectController.cs that lets this script know if the player is tethered
    {
		movement_tethered = tethered;
		if(tethered)
		{
			tether_position = tether.GlobalPosition;
			movement_from_tether = tether_length;
		}
		else
		{
			tether_position = Vector3.Zero;
			movement_from_tether = 0;
		}
    }

    internal void HandleRotatePlayer() // Listen to signal from TargetingSystem.cs that lets this script know that the player is being rotated
    {
		// GD.Print("Movement controller receiving signal to stop movement because of an ability");
        movement_input_prevented = true;
    }

    internal void HandleUICapturingInput(bool capturing_input) // This method listens for a signal emitted in the UI script (NewUI.cs) when the UI is preventing movement
    {
        movement_input_prevented = capturing_input;
    }

	 internal void OnAbilityExecuting(Ability ability) // Listen to signal from individual player ability that lets this script know that it is executing
    {
		GD.Print("received signal ability executing " + ability.Name);
        movement_input_prevented = true;
    }

	private void OnMovementAbilityExecuted(bool executing) // Listen to signal from a movement ability that lets this script know if it has been executed or not
    {
        movement_ability_in_use = executing;
    }

    internal void OnAbilityFinished(Ability ability) // Listen to signal from individual player ability that lets this script know that it has finished
    {
        movement_input_prevented = false;
    }
}
