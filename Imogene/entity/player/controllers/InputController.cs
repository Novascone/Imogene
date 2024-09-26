using Godot;
using System;

public partial class InputController : Node
{
	public float input_strength;
	[Signal] public delegate void CrossChangedEventHandler(string cross);
	[Signal] public delegate void UsableChangedEventHandler();
	[Signal] public delegate void UsableUsedEventHandler();
	public bool directional_input_prevented = false;
	public bool d_pad_right_pressed;
	public bool d_pad_right_released;
	public bool d_pad_left_pressed;
	public bool d_pad_left_released;
	public bool d_pad_up_pressed;
	public bool d_pad_up_released;
	public bool d_pad_down_pressed;
	public bool d_pad_down_released;
	public int d_pad_frames_held;
	public int d_pad_frames_held_threshold = 10;
	public bool d_pad_held;
	public bool x_rotation_not_zero;
	public Vector2 raw_vector;
	public Vector2 deadzoned_vector;
	public float deadzone = 0.25f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GetInputStrength();
	}

	public void SetInput(Player player) // Basic movement controller, takes the input and gives the player direction, also changes speed based on the strength of the input
	{
		
		if(!directional_input_prevented)
		{
			player.direction.X = 0.0f;
			player.direction.Z = 0.0f;
		}
		

		if(!InputPrevented(player))
		{
			if(!player.is_climbing)
			{
				// if (Input.IsActionPressed("Right"))
				// {
				// 	player.direction.Z -= 0.3f;
				// 	player.direction.X -= 0.7f;
				// }
				// if (Input.IsActionPressed("Left"))
				// {
				// 	player.direction.Z += 0.3f;
				// 	player.direction.X += 0.7f;
				// }
				// if (Input.IsActionPressed("Backward"))
				// {
				// 	player.direction.Z -= 0.6f;
				// 	player.direction.X += 0.4f;

				// }systems.targeting_system.RotationForInputFinished += controllers.input_controller.HandleRotationFinished;
				// if (Input.IsActionPressed("Forward"))
				// {
				// 	player.direction.Z += 0.6f;
				// 	player.direction.X -= 0.4f;
				// }
				
				// player.direction = player.direction.Normalized();

				raw_vector = Input.GetVector("Left", "Right", "Forward", "Backward");
				deadzoned_vector = raw_vector;
				if(deadzoned_vector.Length() < deadzone)
				{
					
					deadzoned_vector = Vector2.Zero;
				}
				else
				{
					
					deadzoned_vector = deadzoned_vector.Normalized() * ((deadzoned_vector.Length() - deadzone) / (1 - deadzone));
				}
				player.direction.X = -deadzoned_vector.X;
				player.direction.Z = -deadzoned_vector.Y;
			}
			else
			{
				ClimbingMovement(player);
			}
			
		}

		CheckDPadInput(player);
		if(!directional_input_prevented)
		{
			GD.Print("looking forward");
			LookForward(player,player.direction);
		}
		if(x_rotation_not_zero)
		{
			RotateXBack(player);
		}
		
	}

	public void GetInputStrength()
	{
		input_strength = Input.GetActionStrength("Right") + Input.GetActionStrength("Forward") + Input.GetActionStrength("Left") + Input.GetActionStrength("Backward");
		// GD.Print("Input strength " + input_strength);
	}
	
	public void ClimbingMovement(Player player) // Takes climbing input and moves the character when climbing
	{
		{
			player.direction = Vector3.Zero;
			
			if (Input.IsActionPressed("Right"))
			{
				player.direction.X -= 1.0f;	
				// GD.Print("Action strength right " + Input.GetActionStrength("Right"));	
			}
			if (Input.IsActionPressed("Left"))
			{
				player.direction.X += 1.0f;
				// GD.Print("Action strength left " + Input.GetActionStrength("Left"));	
			}
			if (Input.IsActionPressed("Backward"))
			{
				player.direction.Y -= 1.0f;
				// GD.Print("Action strength back " + Input.GetActionStrength("Backward"));	
			}
			if (Input.IsActionPressed("Forward"))
			{
				player.direction.Y += 1.0f;
				// GD.Print("Action strength forward " + Input.GetActionStrength("Forward"));	
			}
		}
	}

	public void LookForward(Player player, Vector3 direction) // Rotates the player character smoothly with lerp
	{
		// GD.Print("Rotating smoothly");
		if(!player.systems.targeting_system.targeting && !player.systems.targeting_system.rotating_to_soft_target && !player.is_climbing)
		{
			player.previous_y_rotation = player.GlobalRotation.Y;
			if (player.GlobalTransform.Origin != player.GlobalPosition + direction with {Y = 0}) // looks at direction the player is moving
			{
				player.LookAt(player.GlobalPosition + direction with { Y = 0 });
			}
			player.current_y_rotation = player.GlobalRotation.Y;
			if(player.previous_y_rotation != player.current_y_rotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.previous_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	public void RotateXBack(Player player)
	{
		GD.Print("Rotating player x back");
		if(player.GlobalRotation.X != -0)
		{
			player.GlobalRotation = player.GlobalRotation.Lerp(player.GlobalRotation with {X = -0}, 0.2f);
		}
		else
		{
			x_rotation_not_zero = false;
		}
	}



	public bool InputPrevented(Player player)
	{
		if(directional_input_prevented)
		{
			return true;
		}
		// if(player.ability_in_use != null)
		// {
		// 	if(player.ability_in_use.stop_movement_input)
		// 	{
		// 		GD.Print("Input prevented");
		// 		return true;
		// 	}
		// 	else
		// 	{
		// 		GD.Print("Input not prevented");
		// 		return false;
		// 	}
		// }
		if(player.systems.targeting_system.rotating_to_soft_target)
		{
			return true;
		}
		if(player.ui.preventing_movement)
		{
			return true;
		}
		// GD.Print("Input not prevented");
		return false;
	}

	public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        
		if(@event.IsActionPressed("D-PadLeft")) {d_pad_left_pressed = true; d_pad_left_released = false;}
		if(@event.IsActionReleased("D-PadLeft")) {d_pad_left_pressed = false; d_pad_left_released = true;}
		if(@event.IsActionPressed("D-PadRight")){d_pad_right_pressed = true; d_pad_right_released = false;}
		if(@event.IsActionReleased("D-PadRight")){d_pad_right_pressed = false; d_pad_right_released = true;}
		if(@event.IsActionPressed("D-PadUp")){d_pad_up_pressed = true; d_pad_up_released = false;}
		if(@event.IsActionReleased("D-PadUp")){d_pad_up_pressed = false; d_pad_up_released = true;}
		if(@event.IsActionPressed("D-PadDown")){d_pad_down_pressed = true; d_pad_down_released = false;}
		if(@event.IsActionReleased("D-PadDown")){d_pad_down_pressed = false;d_pad_down_released = true;}
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
	

	public void CheckDPadInput(Player player)
	{
		if(d_pad_frames_held > d_pad_frames_held_threshold)
		{
			d_pad_held = true;
		}
		else
		{
			d_pad_held = false;
		}
		if(player.can_use_abilities)
		{
			if(d_pad_left_pressed || d_pad_right_pressed)
			{
				d_pad_frames_held += 1;
			}
			
			if(d_pad_frames_held < d_pad_frames_held_threshold && d_pad_left_released) // If the left D-Pad has been released, and the frames amount of frames it was less than the threshold change crosses
			{
				// player.l_cross_primary_selected = !player.l_cross_primary_selected;
				// player.ui.hud.LCrossPrimaryOrSecondary(player.l_cross_primary_selected);
				EmitSignal(nameof(CrossChanged),"Left");
				d_pad_left_released = false;
				d_pad_frames_held = 0;
				d_pad_held = false;
				// _customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
			}
			else if(d_pad_frames_held >= d_pad_frames_held_threshold && d_pad_left_released) // If the frames held was greater than or equal to the threshold switch off-hand
			{
				GD.Print("Switch off-hand");
				d_pad_left_released = false;
				d_pad_frames_held = 0;
			}
				
			if(d_pad_frames_held < d_pad_frames_held_threshold && d_pad_right_released) // If the right D-Pad has been released, and the frames amount of frames it was less than the threshold change crosses
			{
				
				// player.r_cross_primary_selected = !player.r_cross_primary_selected;
				// player.ui.hud.RCrossPrimaryOrSecondary(player.r_cross_primary_selected);
				EmitSignal(nameof(CrossChanged),"Right");
				d_pad_right_released = false;
				d_pad_frames_held = 0;
				d_pad_held = false;
				// _customSignals.EmitSignal(nameof(CustomSignals.	RCrossPrimaryOrSecondary), r_cross_primary_selected);
			}
			else if(d_pad_frames_held >= d_pad_frames_held_threshold && d_pad_right_released) // If the frames held was greater than or equal to the threshold switch main-hand
			{
				GD.Print("Switch main-hand");
				d_pad_right_released = false;
				d_pad_frames_held = 0;
			}
				
			if(d_pad_up_pressed)
			{
				// switch consumable
				if(player.consumable < 4)
				{
					player.consumable += 1;
				}
				else if(player.consumable == 4)
				{
					player.consumable = 1;
				}
				d_pad_up_pressed = false;

				EmitSignal(nameof(UsableChanged));
				// _customSignals.EmitSignal(nameof(CustomSignals.WhichConsumable), player.consumable);
				// player.ui.hud.WhichConsumable(player.consumable);
			}
			if(d_pad_down_pressed)
			{
				// use consumable
				EmitSignal(nameof(UsableUsed));
				player.consumables[player.consumable]?.UseItem();
				d_pad_down_pressed = false;
			}
		}
	}

    internal void HandleInputPrevented(bool input_prevented)
    {
        directional_input_prevented = input_prevented;
    }

    internal void HandleRotatePlayer()
    {
        directional_input_prevented = true;
    }

    internal void HandleRotationFinished(Player player)
    {
		if(player.GlobalRotation.X != -0)
		{
			x_rotation_not_zero = true;
		}
    }

    internal void OnAbilityFinished(Ability ability)
    {
		
        // if(!ability.button_held)
		// {
			GD.Print("Received ability finished signal");
			directional_input_prevented = false;
		// }
    }

    internal void OnAbilityExecuting(Ability ability)
    {
		GD.Print("Ability preventing input");
        directional_input_prevented = true;
    }

    internal void HandleReleaseInputControl()
    {
        directional_input_prevented = false;
    }
}
