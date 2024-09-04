using Godot;
using System;

public partial class InputController : Controller
{
	public float input_strength;
	[Signal] public delegate void CrossChangedEventHandler(string cross);
	[Signal] public delegate void UsableChangedEventHandler();
	[Signal] public delegate void UsableUsedEventHandler();
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GetInputStrength();
	}

	public void SetInput(Player player) // Basic movement controller, takes the input and gives the player direction, also changes speed based on the strength of the input
	{
		player.direction.X = 0.0f;
		player.direction.Z = 0.0f;

		if(!InputPrevented(player))
		{
			if(!player.is_climbing)
			{
				if (Input.IsActionPressed("Right"))
				{
					player.direction.X -= 1.0f;	
				}
				if (Input.IsActionPressed("Left"))
				{
					player.direction.X += 1.0f;
				}
				if (Input.IsActionPressed("Backward"))
				{
					player.direction.Z -= 1.0f;
				}
				if (Input.IsActionPressed("Forward"))
				{
					player.direction.Z += 1.0f;
				}
			}
			else
			{
				ClimbingMovement(player);
			}
			
		}

		CheckDPadInput(player);
		
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



	public bool InputPrevented(Player player)
	{
		if(player.ability_in_use != null)
		{
			if(player.ability_in_use.stop_movement_input)
			{
				// GD.Print("Input prevented");
				return true;
			}
			else
			{
				// GD.Print("Input not prevented");
				return false;
			}
		}
		if(player.targeting_system.rotating_to_soft_target)
		{
			GD.Print("Input prevented");
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
}
