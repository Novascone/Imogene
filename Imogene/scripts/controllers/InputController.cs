using Godot;
using System;

public partial class InputController : Controller
{
	public float input_strength;
	

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
		else
		{
			GD.Print("Input prevented");
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
		// GD.Print("Input not prevented");
		return false;
	}
}
