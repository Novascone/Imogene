using Godot;
using System;

public partial class InputController : Node
{
	// Input variables
	public float LeftInputStrength { get; set; } = 0.0f;
	public float RightInputStrength { get; set; } = 0.0f;
	public float Deadzone { get; set; } = 0.25f;
	public Vector2 DirectionRawVector { get; set; } = Vector2.Zero;
	public Vector2 AimRawVector { get; set; } = Vector2.Zero;
	public Vector2 DirectionDeadzonedVector { get; set; } = Vector2.Zero;
	public Vector2 AimDeadzonedVector { get; set; } = Vector2.Zero;
	

	// Checking bools
	public bool DirectionalInputPrevented { get; set; } = false;
	public bool Targeting { get; set; }  = false;
	public bool AbilityRotatingPlayer { get; set; }  = false;
	public bool AbilitiesPrevented { get; set; } = false;
	public bool Climbing { get; set; } = false;
	public bool Clambering { get; set; } = false;

	// D-Pad properties
	public bool DPadRightPressed = false;

	private bool _dPadRightReleased = false;
	public bool DPadRightReleased
	{
		get => DPadRightReleased;
		set
		{
			_dPadRightReleased = value;
			if(DPadRightReleased)
			{
				if(DPadFramesHeld < SwitchWeaponsThreshold)
				{
					EmitSignal(nameof(CrossChanged),"Right");
					_dPadRightReleased = false;
					DPadFramesHeld = 0;
				}
				else if(DPadFramesHeld >= SwitchWeaponsThreshold && DPadFramesHeld < SheathWeaponThreshold)
				{
					GD.Print("Switch main-hand");
					_dPadRightReleased = false;
					DPadFramesHeld = 0;
				}
				else
				{
					_dPadRightReleased = false;
					DPadFramesHeld = 0;
				}
			}
		}
	}

	public bool DPadLeftPressed { get; set; }= false;
	
	private bool _dPadLeftReleased = false;
	public bool DPadLeftReleased
	{
		get => _dPadLeftReleased;
		set
		{
			_dPadLeftReleased = value;
			if(DPadLeftReleased)
			{
				if(DPadFramesHeld < SwitchWeaponsThreshold)
				{
					EmitSignal(nameof(CrossChanged),"Left");
					_dPadLeftReleased = false;
					DPadFramesHeld = 0;
				}
				else if(DPadFramesHeld >= SwitchWeaponsThreshold && DPadFramesHeld < SheathWeaponThreshold)
				{
					GD.Print("Switch off-hand");
					_dPadLeftReleased = false;
					DPadFramesHeld = 0;
				}
				else
				{
					_dPadLeftReleased = false;
					DPadFramesHeld = 0;
				}
			}
		}
	}
	
	private bool _dPadUpPressed = false;
	public bool DPadUpPressed
	{
		get => _dPadUpPressed;
		set
		{
			_dPadUpPressed = value;
			if(DPadUpPressed)
			{
				GD.Print("D-Pad up pressed");
				EmitSignal(nameof(UsableChanged));	
				_dPadUpPressed = false;
			}
			
		}
	}

	public bool DPadUpReleased { get; set; } = false;
	
	private bool _dPadDownPressed = false;
	public bool DPadDownPressed
	{
		get => _dPadDownPressed;
		set
		{
			_dPadDownPressed = value;
			if(DPadDownPressed)
			{
				GD.Print("D-Pad down pressed");
				EmitSignal(nameof(UsableUsed));
				_dPadDownPressed = false;
			}
		}
	}

	public bool DPadDownReleased { get; set; } = false;

	public int DPadFramesHeld { get; set; } = 0;

	public int SwitchWeaponsThreshold { get; set; } = 20;
	public int SheathWeaponThreshold { get; set; } = 60;

	// Rotation variables
	public bool XRotationNotZero { get; set; } = false;
	public float MaxXRotation { get; set; } = 0.3f;
	public float MinXRotation { get; set; } = -0.3f;

	// Signals
	[Signal] public delegate void CrossChangedEventHandler(string cross);
	[Signal] public delegate void UsableChangedEventHandler();
	[Signal] public delegate void UsableUsedEventHandler();

	public override void _UnhandledInput(InputEvent @event_) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        
		if(@event_.IsActionPressed("D-PadLeft")) {DPadLeftPressed = true; DPadLeftReleased = false;}
		if(@event_.IsActionReleased("D-PadLeft")) {DPadLeftPressed = false; DPadLeftReleased = true;}
		if(@event_.IsActionPressed("D-PadRight")){DPadRightPressed = true; DPadRightReleased = false;}
		if(@event_.IsActionReleased("D-PadRight")){DPadRightPressed = false; DPadRightReleased = true;}
		if(@event_.IsActionPressed("D-PadUp")){DPadUpPressed = true; DPadUpReleased = false;}
		if(@event_.IsActionReleased("D-PadUp")){DPadUpPressed = false; DPadUpReleased = true;}
		if(@event_.IsActionPressed("D-PadDown")){DPadDownPressed = true; DPadDownReleased = false;}
		if(@event_.IsActionReleased("D-PadDown")){DPadDownPressed = false;DPadDownReleased = true;}
		// if(@event.IsActionPressed("one"))
		// {
		// 	player.damage_system.TakeDamage("Physical", 10, false);
		// 	GD.Print("Health test");
		// 	GD.Print("Health : " + player.health);
		// }
		// if(@event.IsActionPressed("two"))
		// {
		// 	player.resource_system.Resource(10);
		// }
        // if(@event.IsActionPressed("three"))
		// {
		// 	player.resource_system.Posture(5);
		// }
		// if(@event.IsActionPressed("four"))
		// {
		// 	player.xp_system.GainXP(11);
		// }
		
	}	

	public void SetInput(Player player) // Basic movement controller, takes the input and gives the player direction, also changes speed based on the strength of the input
	{
		GetInputStrength();
		if(!DirectionalInputPrevented)
		{
			player.DirectionVector.X = 0.0f;
			player.DirectionVector.Z = 0.0f;
			if(!Climbing)
			{
				SetDeadzonedDirectionVector(player);
				SetDeadzonedAimVector(player);
			}
			else
			{
				ClimbingMovement(player);
			}
		}
		
		CheckDPadInput();
		if(RightInputStrength <= 0)
		{
			LookForward(player, player.DirectionVector);
		}
		else
		{
			LookForward(player, player.AimVector);
		}
		
		
		
		// player.GlobalRotation = player.GlobalRotation with {X = (float)Mathf.Clamp(player.GlobalRotation.X, min_x_rotation, max_x_rotation)};
		
		if(XRotationNotZero)
		{
			RotateXBack(player);
		}
		
	}

	public void GetInputStrength()
	{
		LeftInputStrength = Input.GetActionStrength("MoveRight") + Input.GetActionStrength("MoveForward") + Input.GetActionStrength("MoveLeft") + Input.GetActionStrength("MoveBackward");
		RightInputStrength = Input.GetActionStrength("AimRight") + Input.GetActionStrength("AimForward") + Input.GetActionStrength("AimLeft") + Input.GetActionStrength("AimBackward");
		// GD.Print("Input strength " + input_strength);
	}

	public void SetDeadzonedDirectionVector(Player player)
	{
		DirectionRawVector = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		DirectionDeadzonedVector = DirectionRawVector;
		if(DirectionDeadzonedVector.Length() < Deadzone)
		{
			DirectionDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			DirectionDeadzonedVector = DirectionDeadzonedVector.Normalized() * ((DirectionDeadzonedVector.Length() - Deadzone) / (1 - Deadzone));
		}
		player.DirectionVector.X = -DirectionDeadzonedVector.X;
		player.DirectionVector.Z = -DirectionDeadzonedVector.Y;
	}

	public void SetDeadzonedAimVector(Player player)
	{
		AimRawVector = Input.GetVector("AimLeft", "AimRight", "AimForward", "AimBackward");
		AimDeadzonedVector = AimRawVector;
		if(AimDeadzonedVector.Length() < Deadzone)
		{
			AimDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			AimDeadzonedVector = AimDeadzonedVector.Normalized() * ((AimDeadzonedVector.Length() - Deadzone) / (1 - Deadzone));
		}
		player.AimVector.X = -AimDeadzonedVector.X;
		player.AimVector.Z = -AimDeadzonedVector.Y;
	}

	
	public void ClimbingMovement(Player player) // Takes climbing input and moves the character when climbing
	{
		{
			player.DirectionVector = Vector3.Zero;
			
			if (Input.IsActionPressed("MoveRight"))
			{
				player.DirectionVector.X -= 1.0f;	
				// GD.Print("Action strength right " + Input.GetActionStrength("Right"));	
			}
			if (Input.IsActionPressed("MoveLeft"))
			{
				player.DirectionVector.X += 1.0f;
				// GD.Print("Action strength left " + Input.GetActionStrength("Left"));	
			}
			if (Input.IsActionPressed("MoveBackward"))
			{
				player.DirectionVector.Y -= 1.0f;
				// GD.Print("Action strength back " + Input.GetActionStrength("Backward"));	
			}
			if (Input.IsActionPressed("MoveForward"))
			{
				player.DirectionVector.Y += 1.0f;
				// GD.Print("Action strength forward " + Input.GetActionStrength("Forward"));	
			}
		}
	}

	public void CheckDPadInput()
	{

		if(!AbilitiesPrevented)
		{
			if(DPadLeftPressed || DPadRightPressed)
			{
				DPadFramesHeld += 1;
			}
			if(DPadFramesHeld >= SheathWeaponThreshold)
			{
				GD.Print("Sheath weapons");
			}
		}
	}


	public void LookForward(Player player, Vector3 direction) // Rotates the player character smoothly with lerp
	{
		if(!DirectionalInputPrevented)
		{
			if(!Targeting && !AbilityRotatingPlayer && !Climbing)
			{
				player.PreviousYRotation = player.GlobalRotation.Y;
				if (player.GlobalTransform.Origin != player.GlobalPosition + direction with {Y = 0}) // looks at direction the player is moving
				{
					player.LookAt(player.GlobalPosition + direction with { Y = 0 });
				}
				player.CurrentYRotation = player.GlobalRotation.Y;
				if(player.PreviousYRotation != player.CurrentYRotation)
				{
					player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
				}
			}
		}
	}

	public void Aim(Player player, Vector3 direction)
	{
		if(!DirectionalInputPrevented)
		{
			if(!Targeting && !AbilityRotatingPlayer && !Climbing)
			{
				player.PreviousYRotation = player.GlobalRotation.Y;
				if (player.GlobalTransform.Origin != player.GlobalPosition + direction with {Y = 0}) // looks at direction the player is moving
				{
					player.LookAt(player.GlobalPosition + direction with { Y = 0 });
				}
				player.CurrentYRotation = player.GlobalRotation.Y;
				if(player.PreviousYRotation != player.CurrentYRotation)
				{
					player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
				}
			}
		}
	}


	public void RotateXBack(Player player)
	{
		
		if(player.GlobalRotation.X != -0)
		{
			// player.GlobalRotation = player.GlobalRotation.Lerp(player.GlobalRotation with {X = -0}, 0.2f); <-- figure out a way to use this with hard target 
			player.PreviousXRotation = 0f;
			player.CurrentXRotation = 0f;
			
		}
		else
		{
			XRotationNotZero = false;
		}
	}


	// Signals

	public void Subscribe(Player player)
	{
		player.EntityControllers.EntityStatusEffectsController.InputPrevented += HandleStatusEffectPreventingInput;

		player.PlayerSystems.TargetingSystem.RotationForInputFinished += HandleTargetingRotationFinished;
		player.PlayerSystems.TargetingSystem.PlayerTargeting += HandlePlayerTargeting;
		player.PlayerSystems.TargetingSystem.Rotating += HandleTargetingSystemRotatingPlayer;

		player.PlayerControllers.AbilityController.ReleaseInputControl += HandleAbilityReleaseInputControl;
		player.PlayerControllers.AbilityController.RotatePlayer += HandleAbilityRotatingPlayer;

		player.PlayerUI.CapturingInput += HandleUICapturingInput;
	}

	public void AbilitySubscribe(Ability ability)
	{
		ability.AbilityExecuting += OnAbilityExecuting;
		ability.AbilityFinished += OnAbilityFinished;
		ability.AbilityReleaseInputControl += OnAbilityFinished;
	}


    public void Unsubscribe(Player player)
	{
		player.EntityControllers.EntityStatusEffectsController.InputPrevented -= HandleStatusEffectPreventingInput;

		player.PlayerSystems.TargetingSystem.RotationForInputFinished -= HandleTargetingRotationFinished;
		player.PlayerSystems.TargetingSystem.PlayerTargeting += HandlePlayerTargeting;
		player.PlayerSystems.TargetingSystem.Rotating -= HandleTargetingSystemRotatingPlayer;

		player.PlayerControllers.AbilityController.ReleaseInputControl -= HandleAbilityReleaseInputControl;
		player.PlayerControllers.AbilityController.RotatePlayer -= HandleAbilityRotatingPlayer;

		player.PlayerUI.CapturingInput -= HandleUICapturingInput;
	}

	public void AbilityUnsubscribe(Ability ability)
	{
		ability.AbilityExecuting -= OnAbilityExecuting;
		ability.AbilityFinished -= OnAbilityFinished;
		ability.AbilityReleaseInputControl -= OnAbilityFinished;
	}

	private void HandleAbilityRotatingPlayer() // Listens for signal from AbilityController.cs
    {
        AbilityRotatingPlayer = true;
    }

	private void HandlePlayerTargeting(bool targeting) // Listens for player targeting signal from TargetingSystem.cs
    {
        Targeting = targeting;
    }
	 public void HandleTargetingSystemRotatingPlayer() // Listens for signal from TargetingSystem.cs
    {
        DirectionalInputPrevented = true;
    }
	 public void HandleTargetingRotationFinished(Player player) // Listens for a signal from TargetingSystem.cs
    {
		if(player.GlobalRotation.X != -0)
		{
			XRotationNotZero = true;
		}
		AbilityRotatingPlayer = false;
    }

    public void HandleStatusEffectPreventingInput(bool InputPrevented) // Listens for signal from StatusEffectController.cs 
    {
        DirectionalInputPrevented = InputPrevented;
    }

	public void OnAbilityExecuting(Ability ability) // Listens for a signal emitted from one of the players abilities
    {
        DirectionalInputPrevented = true;
    }

    public void OnAbilityFinished(Ability ability) // Listens for a signal emitted from one of the players abilities
    {
		DirectionalInputPrevented = false;
    }
	
    public void HandleAbilityReleaseInputControl() // Listens for a signal emitted from AbilityController.cs
    {
        DirectionalInputPrevented = false;
    }

    public void HandleUICapturingInput(bool capturingInput) // This method listens for a signal emitted in the UI script (NewUI.cs) when the UI is preventing movement
    {
        DirectionalInputPrevented = capturingInput;
    }

	public void HandleAbilitiesPrevented(bool abilitiesPrevented) // Not yet implemented
	{
		AbilitiesPrevented = abilitiesPrevented;
	}
}
