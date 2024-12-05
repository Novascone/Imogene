using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;


// Movement Controller
// Moves the player character around the world, sets basic animations, implements the landing icon, applies smooth rotation, handles looking at enemies, and prevents movement when UI is open
public partial class MovementController : Node
{

	// Climbing
	private float climb_speed { get; set; } = 4.0f;
	private float clamber_speed { get; set; } = 10.0f;
	public bool climbing { get; set; } = false;
	public bool clambering{ get; set; }  = false;
	private float vertical_climbing_input { get; set; } = 0.0f;
	private float horizontal_climbing_input { get; set; } = 0.0f;
	public float move_forward_clamber { get; set; } = 0.0f;

	public float jump_height { get; set; } = 30.0f;
	public float jump_time_to_peak { get; set; } = 2.0f;
	public float jump_time_to_decent { get; set; } = 1.9f;
	public float jump_velocity { get; set; } = 0.0f;
	public float jump_gravity { get; set; } = 0.0f;
	public float fall_gravity { get; set; } = 0.0f;

	// Input
	public bool rotation_finished { get; set; } = true;
	public bool movement_input_prevented { get; set; } = false;
	public bool movement_ability_in_use { get; set; } = false;
	private Vector3 tether_position  { get; set; } = Vector3.Zero;
	public float movement_from_tether  { get; set; } = 0.0f;
	public bool movement_tethered  { get; set; } = false;

    public override void _Ready()
    {
        jump_velocity = (float)(2.0 * jump_height / jump_time_to_peak);
		jump_gravity = (float)(-2.0 * jump_height / jump_time_to_peak * jump_time_to_peak);
		fall_gravity = (float)(-2.0 * jump_height / jump_time_to_decent * jump_time_to_decent);	
    }


    public void MovePlayer(Player player, float inputStrength, double delta)
	{
		
		if(!climbing)
		{
			StandardMovement(player, inputStrength, delta);
		}
		else
		{
			ClimbingMovement(player);
		}
	
		player.Velocity = player.VelocityVector;
		

		if(movement_tethered)
		{
			player.GlobalPosition = (player.GlobalPosition - tether_position).LimitLength(movement_from_tether) + tether_position;
		}

			
	}

    private void StandardMovement(Player player, float inputStrength, double delta)
    {
		if(!movement_ability_in_use && !movement_input_prevented)
		{
			player.VelocityVector.X = player.DirectionVector.X * player.MovementSpeed.CurrentValue * inputStrength;
			player.VelocityVector.Z = player.DirectionVector.Z * player.MovementSpeed.CurrentValue * inputStrength;
			
		}
		else if(movement_input_prevented)
		{
			
			player.VelocityVector = Vector3.Zero with { Y = fall_gravity / 15};
		}
		if(!player.IsOnFloor())
		{
			player.VelocityVector.Y += (float)(SetGravity(player) * delta);
		}

    }

    public float SetGravity(Player player_)
	{
		if(player_.Velocity.Y > 0)
		{

			return jump_gravity;
		}
		else
		{
			return fall_gravity;
		}
	}

	

	public void ClimbingMovement(Player player) // Takes climbing input and moves the character when climbing
	{
		
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
			
		

		ClimbingRotation(player);

		if(player.DirectionVector != Vector3.Zero && climbing)
		{
			player.PreviousYRotation = player.GlobalRotation.Y;
			player.CurrentYRotation = -(MathF.Atan2(player.PlayerControllers.near_wall.GetCollisionNormal().Z, player.PlayerControllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Set the player y rotation to the rotation needed to face the wall
			if(player.PreviousYRotation != player.CurrentYRotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	public void ClimbingRotation(Player player) // Sets the rotation of the player when climbing
	{
		var rot = -(MathF.Atan2(player.PlayerControllers.near_wall.GetCollisionNormal().Z, player.PlayerControllers.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Get the angle of rotation needed to face the object climbing
		
		vertical_climbing_input = Input.GetActionStrength("Forward") - Input.GetActionStrength("Backward");
		horizontal_climbing_input = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		
		if(!clambering)
		{
			player.DirectionVector = new Vector3(horizontal_climbing_input, vertical_climbing_input, move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***

		}
		// player.direction = new Vector3(horizontal_climbing_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***
	}


	// Signals

    internal void HandleMovementPrevented(bool movementPrevented) // Listen for signal from StatusEffectController.cs that prevents movement
    {
        movement_input_prevented = movementPrevented;
    }

	public void HandleClimbing(bool is_climbing_) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun climbing or stopped climbing
	{
		climbing = is_climbing_;
	}

	public void HandleClambering(bool is_clambering_) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun clambering or stopped climbing
	{
		clambering = is_clambering_;
	}

    internal void HandleTethered(Entity entity_, MeshInstance3D tether_, bool tethered_, float tether_length_) // Listen for signal from StatusEffectController.cs that lets this script know if the player is tethered
    {
		movement_tethered = tethered_;
		if(tethered_)
		{
			tether_position = tether_.GlobalPosition;
			movement_from_tether = tether_length_;
		}
		else
		{
			tether_position = Vector3.Zero;
			movement_from_tether = 0.0f;
		}
    }

    internal void HandleRotatePlayer() // Listen to signal from TargetingSystem.cs that lets this script know that the player is being rotated
    {
        movement_input_prevented = true;
    }

    internal void HandleUICapturingInput(bool capturing_input_) // This method listens for a signal emitted in the UI script (NewUI.cs) when the UI is preventing movement
    {
        movement_input_prevented = capturing_input_;
    }

	 internal void OnAbilityExecuting(Ability ability_) // Listen to signal from individual player ability that lets this script know that it is executing
    {
        movement_input_prevented = true;
		if(ability_.general_ability_type == Ability.GeneralAbilityType.Movement)
		{
			movement_ability_in_use = true;
		}
    }

	private void OnMovementAbilityExecuted(bool executing_) // Listen to signal from a movement ability that lets this script know if it has been executed or not
    {
        movement_ability_in_use = executing_;
    }

    internal void OnAbilityFinished(Ability ability_) // Listen to signal from individual player ability that lets this script know that it has finished
    {
        movement_input_prevented = false;
		if(ability_.general_ability_type == Ability.GeneralAbilityType.Movement)
		{
			movement_ability_in_use = false;
		}
    }

	public void Subscribe(Player player)
	{
		player.EntityControllers.status_effect_controller.MovementPrevented += HandleMovementPrevented;
		player.EntityControllers.status_effect_controller.Tethered += HandleTethered;

		player.PlayerSystems.targeting_system.Rotating += HandleRotatePlayer;

		player.PlayerUI.CapturingInput += HandleUICapturingInput;
	}

	public void AbilitySubscribe(Ability ability_)
	{
		ability_.AbilityExecuting += OnAbilityExecuting;
		ability_.AbilityFinished += OnAbilityFinished;
		ability_.MovementAbilityExecuted += OnMovementAbilityExecuted;
	}

    public void Unsubscribe(Player player)
	{
		player.EntityControllers.status_effect_controller.MovementPrevented -= HandleMovementPrevented;
		player.EntityControllers.status_effect_controller.Tethered -= HandleTethered;

		player.PlayerSystems.targeting_system.Rotating -= HandleRotatePlayer;
		player.PlayerUI.CapturingInput -= HandleUICapturingInput;
	}

	public void AbilityUnsubscribe(Ability ability_)
	{
		ability_.AbilityExecuting -= OnAbilityExecuting;
		ability_.AbilityFinished -= OnAbilityFinished;
	}
}
