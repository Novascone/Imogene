using Godot;
using System;

public partial class Roll : Node3D
{
	private Vector3 roll_velocity = Vector3.Zero; 
	
	private int roll_time = 13; // How many frames the player can roll for.
	private bool roll_right = false;
	private bool roll_left = false;
	private  bool roll_back = false;
	private bool roll_forward = false;

	

	public void Execute(player s)
	{
		if(roll_time == 13)
		{
			GD.Print("here");
			Roll_(s);
		}
		RollHandler(s);
		GD.Print("blend_direction ", s.blend_direction);
	}
	public void Roll_(player s)	// Increases velocity
	{
		roll_velocity = Vector3.Zero; // resets dash_velocity so it always moves in the right direction
		roll_velocity += s.Velocity * 4;
		
	}

	public void RollHandler(player s)
	{
		if(s.blend_direction.X > 0 && s.blend_direction.Y > 0)
		{
			roll_right = true;
		}
		if(s.blend_direction.X < 0 && s.blend_direction.Y < 0)
		{
			roll_left = true;
		}
		if(s.blend_direction.X > 0 && s.blend_direction.Y < 0)
		{
			roll_right = true;
		}
		if(s.blend_direction.X < 0 && s.blend_direction.Y > 0)
		{
			roll_left = true;
		}
		if(s.blend_direction.X > 0 && s.blend_direction.Y == 0)
		{
			roll_right = true;
		}
		if(s.blend_direction.X < 0 && s.blend_direction.Y == 0)
		{
			roll_left = true;
		}
		if(s.blend_direction.X == 0 && s.blend_direction.Y > 0)
		{
			roll_forward = true;
		}
		if(s.blend_direction.X == 0 && s.blend_direction.Y < 0)
		{
			roll_back = true;
		}
		if(roll_time != 0)
		{
			// Lerps to zero
			if(roll_time < 10)
			{
				s.velocity.X = Mathf.Lerp(roll_velocity.X, 0, 0.1f);
				s.velocity.Z = Mathf.Lerp(roll_velocity.Z, 0, 0.1f);
			}
			else if(roll_time > 10)
			{
				s.velocity.X = Mathf.Lerp(0, roll_velocity.X, 0.7f);
				s.velocity.Z = Mathf.Lerp(0, roll_velocity.Z, 0.7f);
			}
			
			roll_time -= 1;
			// GD.Print(roll_time);
			// GD.Print("velocity in roll ", s.velocity);
		}
		if(roll_time == 1)
		{
			// Sets velocity to 0 after roll
			s.velocity = Vector3.Zero;
			s.blend_direction = Vector2.Zero;
		}
		if(roll_time == 0)
		{
			GD.Print("roll time zero");
			roll_time = 13;
			roll_back = false;
			roll_forward = false;
			roll_right = false;
			roll_left = false;
			s.rolling = false;
		}
		s.tree.Set("parameters/conditions/roll_back", roll_back);
		s.tree.Set("parameters/conditions/roll_forward", roll_forward);
		s.tree.Set("parameters/conditions/roll_left", roll_left);
		s.tree.Set("parameters/conditions/roll_right", roll_right);
	}
}
