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
	private float ClimbSpeed { get; set; } = 4.0f;
	private float ClamberSpeed { get; set; } = 10.0f;
	public bool Climbing { get; set; } = false;
	public bool Clambering { get; set; }  = false;
	private float VerticalClimbingInput { get; set; } = 0.0f;
	private float HorizontalClimbingInput { get; set; } = 0.0f;
	public float MoveForwardClamber { get; set; } = 0.0f;

	public float JumpHeight { get; set; } = 30.0f;
	public float JumpTimeToPeak { get; set; } = 2.0f;
	public float JumpTimeToDecent { get; set; } = 1.9f;
	public float JumpVelocity { get; set; } = 0.0f;
	public float JumpGravity { get; set; } = 0.0f;
	public float FallGravity { get; set; } = 0.0f;

	// Input
	public bool RotationFinished { get; set; } = true;
	public bool MovementInputPrevented { get; set; } = false;
	public bool MovementAbilityInUse { get; set; } = false;
	private Vector3 TetherPosition  { get; set; } = Vector3.Zero;
	public float MovementFromTether  { get; set; } = 0.0f;
	public bool MovementTethered  { get; set; } = false;

    public override void _Ready()
    {
        JumpVelocity = (float)(2.0 * JumpHeight / JumpTimeToPeak);
		JumpGravity = (float)(-2.0 * JumpHeight / JumpTimeToPeak * JumpTimeToPeak);
		FallGravity = (float)(-2.0 * JumpHeight / JumpTimeToDecent * JumpTimeToDecent);	
    }


    public void MovePlayer(Player player, float inputStrength, double delta)
	{
		
		if(!Climbing)
		{
			StandardMovement(player, inputStrength, delta);
		}
		else
		{
			ClimbingMovement(player);
		}
	
		player.Velocity = player.VelocityVector;
		

		if(MovementTethered)
		{
			player.GlobalPosition = (player.GlobalPosition - TetherPosition).LimitLength(MovementFromTether) + TetherPosition;
		}

			
	}

    private void StandardMovement(Player player, float inputStrength, double delta)
    {
		if(!MovementAbilityInUse && !MovementInputPrevented)
		{
			player.VelocityVector.X = player.DirectionVector.X * player.MovementSpeed.CurrentValue * inputStrength;
			player.VelocityVector.Z = player.DirectionVector.Z * player.MovementSpeed.CurrentValue * inputStrength;
			
		}
		else if(MovementInputPrevented)
		{
			
			player.VelocityVector = Vector3.Zero with { Y = FallGravity / 15};
		}
		if(!player.IsOnFloor())
		{
			player.VelocityVector.Y += (float)(SetGravity(player) * delta);
		}

    }

    public float SetGravity(Player player)
	{
		if(player.Velocity.Y > 0)
		{

			return JumpGravity;
		}
		else
		{
			return FallGravity;
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

		if(player.DirectionVector != Vector3.Zero && Climbing)
		{
			player.PreviousYRotation = player.GlobalRotation.Y;
			player.CurrentYRotation = -(MathF.Atan2(player.PlayerControllers.NearWall.GetCollisionNormal().Z, player.PlayerControllers.NearWall.GetCollisionNormal().X) - MathF.PI/2); // Set the player y rotation to the rotation needed to face the wall
			if(player.PreviousYRotation != player.CurrentYRotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	public void ClimbingRotation(Player player) // Sets the rotation of the player when climbing
	{
		var rot = -(MathF.Atan2(player.PlayerControllers.NearWall.GetCollisionNormal().Z, player.PlayerControllers.NearWall.GetCollisionNormal().X) - MathF.PI/2); // Get the angle of rotation needed to face the object climbing
		
		VerticalClimbingInput = Input.GetActionStrength("Forward") - Input.GetActionStrength("Backward");
		HorizontalClimbingInput = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		
		if(!Clambering)
		{
			player.DirectionVector = new Vector3(HorizontalClimbingInput, VerticalClimbingInput, MoveForwardClamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***

		}
		// player.direction = new Vector3(horizontal_climbing_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***
	}


	// Signals

    internal void HandleMovementPrevented(bool movementPrevented) // Listen for signal from StatusEffectController.cs that prevents movement
    {
        MovementInputPrevented = movementPrevented;
    }

	public void HandleClimbing(bool isClimbing) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun climbing or stopped climbing
	{
		Climbing = isClimbing;
	}

	public void HandleClambering(bool isClambering) // Listen for signal from Jump.cs/AbilityController.cs that lets this script know the player has begun clambering or stopped climbing
	{
		Clambering = isClambering;
	}

    internal void HandleTethered(Entity entity, MeshInstance3D tether, bool tethered, float tetherLength) // Listen for signal from StatusEffectController.cs that lets this script know if the player is tethered
    {
		MovementTethered = tethered;
		if(tethered)
		{
			TetherPosition = tether.GlobalPosition;
			MovementFromTether = tetherLength;
		}
		else
		{
			TetherPosition = Vector3.Zero;
			MovementFromTether = 0.0f;
		}
    }

    internal void HandleRotatePlayer() // Listen to signal from TargetingSystem.cs that lets this script know that the player is being rotated
    {
        MovementInputPrevented = true;
    }

    internal void HandleUICapturingInput(bool capturingInput) // This method listens for a signal emitted in the UI script (NewUI.cs) when the UI is preventing movement
    {
        MovementInputPrevented = capturingInput;
    }

	 internal void OnAbilityExecuting(Ability ability) // Listen to signal from individual player ability that lets this script know that it is executing
    {
        MovementInputPrevented = true;
		if(ability.AbilityGeneralType == Ability.GeneralAbilityType.Movement)
		{
			MovementAbilityInUse = true;
		}
    }

	private void OnMovementAbilityExecuted(bool executing) // Listen to signal from a movement ability that lets this script know if it has been executed or not
    {
        MovementAbilityInUse = executing;
    }

    internal void OnAbilityFinished(Ability ability) // Listen to signal from individual player ability that lets this script know that it has finished
    {
        MovementInputPrevented = false;
		if(ability.AbilityGeneralType == Ability.GeneralAbilityType.Movement)
		{
			MovementAbilityInUse = false;
		}
    }

	public void Subscribe(Player player)
	{
		player.EntityControllers.EntityStatusEffectsController.MovementPrevented += HandleMovementPrevented;
		player.EntityControllers.EntityStatusEffectsController.Tethered += HandleTethered;

		player.PlayerSystems.TargetingSystem.Rotating += HandleRotatePlayer;

		player.PlayerUI.CapturingInput += HandleUICapturingInput;
	}

	public void AbilitySubscribe(Ability ability)
	{
		ability.AbilityExecuting += OnAbilityExecuting;
		ability.AbilityFinished += OnAbilityFinished;
		ability.MovementAbilityExecuted += OnMovementAbilityExecuted;
	}

    public void Unsubscribe(Player player)
	{
		player.EntityControllers.EntityStatusEffectsController.MovementPrevented -= HandleMovementPrevented;
		player.EntityControllers.EntityStatusEffectsController.Tethered -= HandleTethered;

		player.PlayerSystems.TargetingSystem.Rotating -= HandleRotatePlayer;
		player.PlayerUI.CapturingInput -= HandleUICapturingInput;
	}

	public void AbilityUnsubscribe(Ability ability)
	{
		ability.AbilityExecuting -= OnAbilityExecuting;
		ability.AbilityFinished -= OnAbilityFinished;
	}
}
