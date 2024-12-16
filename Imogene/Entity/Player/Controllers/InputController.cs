using Godot;
using System;

public partial class InputController : Node
{
	// Input variables
	public float LeftInputStrength { get; set; } = 0.0f;
	public float RightInputStrength { get; set; } = 0.0f;
	public float deadzone { get; set; } = 0.25f;
	public Vector2 DirectionRawVector { get; set; } = Vector2.Zero;
	public Vector2 AimRawVector { get; set; } = Vector2.Zero;
	public Vector2 DirectionDeadzonedVector { get; set; } = Vector2.Zero;
	public Vector2 AimDeadzonedVector { get; set; } = Vector2.Zero;
	

	// Checking bools
	public bool directional_input_prevented { get; set; } = false;
	public bool targeting { get; set; }  = false;
	public bool ability_rotating_player { get; set; }  = false;
	public bool abilities_prevented { get; set; } = false;
	public bool climbing { get; set; } = false;
	public bool clambering { get; set; } = false;

	// D-Pad properties
	public bool d_pad_right_pressed = false;

	private bool _d_pad_right_released = false;
	public bool d_pad_right_released
	{
		get => _d_pad_right_released;
		set
		{
			_d_pad_right_released = value;
			if(d_pad_right_released)
			{
				if(d_pad_frames_held < switch_weapon_threshold)
				{
					EmitSignal(nameof(CrossChanged),"Right");
					_d_pad_right_released = false;
					d_pad_frames_held = 0;
				}
				else if(d_pad_frames_held >= switch_weapon_threshold && d_pad_frames_held < sheath_weapon_threshold)
				{
					GD.Print("Switch main-hand");
					_d_pad_right_released = false;
					d_pad_frames_held = 0;
				}
				else
				{
					_d_pad_right_released = false;
					d_pad_frames_held = 0;
				}
			}
		}
	}

	public bool d_pad_left_pressed { get; set; }= false;
	
	private bool _d_pad_left_released = false;
	public bool d_pad_left_released
	{
		get => _d_pad_left_released;
		set
		{
			_d_pad_left_released = value;
			if(d_pad_left_released)
			{
				if(d_pad_frames_held < switch_weapon_threshold)
				{
					EmitSignal(nameof(CrossChanged),"Left");
					_d_pad_left_released = false;
					d_pad_frames_held = 0;
				}
				else if(d_pad_frames_held >= switch_weapon_threshold && d_pad_frames_held < sheath_weapon_threshold)
				{
					GD.Print("Switch off-hand");
					_d_pad_left_released = false;
					d_pad_frames_held = 0;
				}
				else
				{
					_d_pad_left_released = false;
					d_pad_frames_held = 0;
				}
			}
		}
	}
	
	private bool _d_pad_up_pressed = false;
	public bool d_pad_up_pressed
	{
		get => _d_pad_up_pressed;
		set
		{
			_d_pad_up_pressed = value;
			if(d_pad_up_pressed)
			{
				GD.Print("D-Pad up pressed");
				EmitSignal(nameof(UsableChanged));	
				_d_pad_up_pressed = false;
			}
			
		}
	}

	public bool d_pad_up_released { get; set; } = false;
	
	private bool _d_pad_down_pressed = false;
	public bool d_pad_down_pressed
	{
		get => _d_pad_down_pressed;
		set
		{
			_d_pad_down_pressed = value;
			if(d_pad_down_pressed)
			{
				GD.Print("D-Pad down pressed");
				EmitSignal(nameof(UsableUsed));
				_d_pad_down_pressed = false;
			}
		}
	}

	public bool d_pad_down_released { get; set; } = false;

	public int d_pad_frames_held { get; set; } = 0;

	public int switch_weapon_threshold { get; set; } = 20;
	public int sheath_weapon_threshold { get; set; } = 60;

	// Rotation variables
	public bool x_rotation_not_zero { get; set; } = false;
	public float max_x_rotation { get; set; } = 0.3f;
	public float min_x_rotation { get; set; } = -0.3f;

	// Signals
	[Signal] public delegate void CrossChangedEventHandler(string cross_);
	[Signal] public delegate void UsableChangedEventHandler();
	[Signal] public delegate void UsableUsedEventHandler();

	public override void _UnhandledInput(InputEvent @event_) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        
		if(@event_.IsActionPressed("D-PadLeft")) {d_pad_left_pressed = true; d_pad_left_released = false;}
		if(@event_.IsActionReleased("D-PadLeft")) {d_pad_left_pressed = false; d_pad_left_released = true;}
		if(@event_.IsActionPressed("D-PadRight")){d_pad_right_pressed = true; d_pad_right_released = false;}
		if(@event_.IsActionReleased("D-PadRight")){d_pad_right_pressed = false; d_pad_right_released = true;}
		if(@event_.IsActionPressed("D-PadUp")){d_pad_up_pressed = true; d_pad_up_released = false;}
		if(@event_.IsActionReleased("D-PadUp")){d_pad_up_pressed = false; d_pad_up_released = true;}
		if(@event_.IsActionPressed("D-PadDown")){d_pad_down_pressed = true; d_pad_down_released = false;}
		if(@event_.IsActionReleased("D-PadDown")){d_pad_down_pressed = false;d_pad_down_released = true;}
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
		if(!directional_input_prevented)
		{
			player.DirectionVector.X = 0.0f;
			player.DirectionVector.Z = 0.0f;
			if(!climbing)
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
		
		if(x_rotation_not_zero)
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
		if(DirectionDeadzonedVector.Length() < deadzone)
		{
			DirectionDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			DirectionDeadzonedVector = DirectionDeadzonedVector.Normalized() * ((DirectionDeadzonedVector.Length() - deadzone) / (1 - deadzone));
		}
		player.DirectionVector.X = -DirectionDeadzonedVector.X;
		player.DirectionVector.Z = -DirectionDeadzonedVector.Y;
	}

	public void SetDeadzonedAimVector(Player player)
	{
		AimRawVector = Input.GetVector("AimLeft", "AimRight", "AimForward", "AimBackward");
		AimDeadzonedVector = AimRawVector;
		if(AimDeadzonedVector.Length() < deadzone)
		{
			AimDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			AimDeadzonedVector = AimDeadzonedVector.Normalized() * ((AimDeadzonedVector.Length() - deadzone) / (1 - deadzone));
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

		if(!abilities_prevented)
		{
			if(d_pad_left_pressed || d_pad_right_pressed)
			{
				d_pad_frames_held += 1;
			}
			if(d_pad_frames_held >= sheath_weapon_threshold)
			{
				GD.Print("Sheath weapons");
			}
		}
	}


	public void LookForward(Player player, Vector3 direction_) // Rotates the player character smoothly with lerp
	{
		if(!directional_input_prevented)
		{
			if(!targeting && !ability_rotating_player && !climbing)
			{
				player.PreviousYRotation = player.GlobalRotation.Y;
				if (player.GlobalTransform.Origin != player.GlobalPosition + direction_ with {Y = 0}) // looks at direction the player is moving
				{
					player.LookAt(player.GlobalPosition + direction_ with { Y = 0 });
				}
				player.CurrentYRotation = player.GlobalRotation.Y;
				if(player.PreviousYRotation != player.CurrentYRotation)
				{
					player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
				}
			}
		}
	}

	public void Aim(Player player, Vector3 direction_)
	{
		if(!directional_input_prevented)
		{
			if(!targeting && !ability_rotating_player && !climbing)
			{
				player.PreviousYRotation = player.GlobalRotation.Y;
				if (player.GlobalTransform.Origin != player.GlobalPosition + direction_ with {Y = 0}) // looks at direction the player is moving
				{
					player.LookAt(player.GlobalPosition + direction_ with { Y = 0 });
				}
				player.CurrentYRotation = player.GlobalRotation.Y;
				if(player.PreviousYRotation != player.CurrentYRotation)
				{
					player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
				}
			}
		}
	}


	public void RotateXBack(Player player_)
	{
		
		if(player_.GlobalRotation.X != -0)
		{
			// player.GlobalRotation = player.GlobalRotation.Lerp(player.GlobalRotation with {X = -0}, 0.2f); <-- figure out a way to use this with hard target 
			player_.PreviousXRotation = 0f;
			player_.CurrentXRotation = 0f;
			
		}
		else
		{
			x_rotation_not_zero = false;
		}
	}


	// Signals

	public void Subscribe(Player player)
	{
		player.EntityControllers.status_effect_controller.InputPrevented += HandleStatusEffectPreventingInput;

		player.PlayerSystems.targeting_system.RotationForInputFinished += HandleTargetingRotationFinished;
		player.PlayerSystems.targeting_system.PlayerTargeting += HandlePlayerTargeting;
		player.PlayerSystems.targeting_system.Rotating += HandleTargetingSystemRotatingPlayer;

		player.PlayerControllers.ability_controller.ReleaseInputControl += HandleAbilityReleaseInputControl;
		player.PlayerControllers.ability_controller.RotatePlayer += HandleAbilityRotatingPlayer;

		player.PlayerUI.CapturingInput += HandleUICapturingInput;
	}

	public void AbilitySubscribe(Ability ability_)
	{
		ability_.AbilityExecuting += OnAbilityExecuting;
		ability_.AbilityFinished += OnAbilityFinished;
		ability_.AbilityReleaseInputControl += OnAbilityFinished;
	}


    public void Unsubscribe(Player player)
	{
		player.EntityControllers.status_effect_controller.InputPrevented -= HandleStatusEffectPreventingInput;

		player.PlayerSystems.targeting_system.RotationForInputFinished -= HandleTargetingRotationFinished;
		player.PlayerSystems.targeting_system.PlayerTargeting += HandlePlayerTargeting;
		player.PlayerSystems.targeting_system.Rotating -= HandleTargetingSystemRotatingPlayer;

		player.PlayerControllers.ability_controller.ReleaseInputControl -= HandleAbilityReleaseInputControl;
		player.PlayerControllers.ability_controller.RotatePlayer -= HandleAbilityRotatingPlayer;

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
        ability_rotating_player = true;
    }

	private void HandlePlayerTargeting(bool targeting_) // Listens for player targeting signal from TargetingSystem.cs
    {
        targeting = targeting_;
    }
	 public void HandleTargetingSystemRotatingPlayer() // Listens for signal from TargetingSystem.cs
    {
        directional_input_prevented = true;
    }
	 public void HandleTargetingRotationFinished(Player player_) // Listens for a signal from TargetingSystem.cs
    {
		if(player_.GlobalRotation.X != -0)
		{
			x_rotation_not_zero = true;
		}
		ability_rotating_player = false;
    }

    public void HandleStatusEffectPreventingInput(bool input_prevented_) // Listens for signal from StatusEffectController.cs 
    {
        directional_input_prevented = input_prevented_;
    }

	public void OnAbilityExecuting(Ability ability_) // Listens for a signal emitted from one of the players abilities
    {
        directional_input_prevented = true;
    }

    public void OnAbilityFinished(Ability ability_) // Listens for a signal emitted from one of the players abilities
    {
		directional_input_prevented = false;
    }
	
    public void HandleAbilityReleaseInputControl() // Listens for a signal emitted from AbilityController.cs
    {
        directional_input_prevented = false;
    }

    public void HandleUICapturingInput(bool capturing_input_) // This method listens for a signal emitted in the UI script (NewUI.cs) when the UI is preventing movement
    {
        directional_input_prevented = capturing_input_;
    }

	public void HandleAbilitiesPrevented(bool abilities_prevented_) // Not yet implemented
	{
		abilities_prevented = abilities_prevented_;
	}
}
