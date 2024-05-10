using Godot;
using System;
using System.Runtime.Intrinsics.X86;

public partial class Target : Ability
{
	private bool player_Z_more_than_target_Z; // Booleans to see where player is relative the the object it is targeting
	private bool player_Z_less_than_target_Z; 
	private bool player_X_more_than_target_X; 
	private bool player_X_less_than_target_X; 
	float _t = 0.4f;
	public override void Execute(player s)
	{
			Rotate(s);
	}
	public void Rotate(player s)
	{
		if(s.player_position.Z - s.mob_to_LookAt_pos.Z > 0)
			{
				player_Z_more_than_target_Z = true;
				player_Z_less_than_target_Z = false;
				// GD.Print("player_Z_more_than_target_Z");
			}
			if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0)
			{
				player_Z_less_than_target_Z = true;
				player_Z_more_than_target_Z = false;
				// GD.Print("player_Z_less_than_target_Z ");
			}
			if((s.player_position.X - s.mob_to_LookAt_pos.X) > 0)
			{
				player_X_more_than_target_X = true;
				player_X_less_than_target_X = false;
				// GD.Print("player_X_more_than_target_X");
				
			}
			if((s.player_position.X - s.mob_to_LookAt_pos.X) < 0)
			{
				player_X_less_than_target_X = true;
				player_X_more_than_target_X = false;
				// GD.Print("player_X_less_than_target_X");
			}

			if(player_Z_more_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("player_Z_more_than_target_Z && player_X_more_than_target_X");
				
				if(s.direction.X == 1 && s.direction.Z == 1) // walk away
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == -1) // walk toward
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
				}
				if(s.direction.Z == -1 && s.direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.X, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
				}
				if(s.direction.Z == 1 && s.direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5) 
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
				}
				if(s.direction.X == 1 && s.direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
					
				}
				if(s.direction.X == -1 && s.direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
					
				}
				if(s.direction.X == 1 && s.direction.Z == -1) // strafe right
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == 1) // strafe left
				{
					
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
			
			}
			if(player_Z_less_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("here");
				if(s.direction.X == 1 && s.direction.Z == -1) // walk away
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == 1) // walk toward
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
				}
				if(s.direction.X == 1 && s.direction.Z == 1) // strafe left
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == -1) // strafe right
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.Z == 1 && s.direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
					
				}
				if(s.direction.Z == -1 && s.direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
				}
				if(s.direction.X == 1 && s.direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					} 
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
				}
				if(s.direction.X == -1 && s.direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					}
				}
				
			}
			if(player_Z_less_than_target_Z && player_X_less_than_target_X)
			{
				if(s.direction.X == 1 && s.direction.Z == 1) // walk toward
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == -1) // walk away
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
				}
				if(s.direction.Z == -1 && s.direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.Z == 1 && s.direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.X == 1 && s.direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.X == -1 && s.direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = -1; Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = 0; Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.X == 1 && s.direction.Z == -1) // strafe left
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == 1) // strafe right
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
			}
			if(player_Z_more_than_target_Z && player_X_less_than_target_X)
			{
				
				if(s.direction.X == 1 && s.direction.Z == -1) // walk toward
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == 1) // walk away
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
				}
				if(s.direction.X == 1 && s.direction.Z == 1) // strafe right
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.X == -1 && s.direction.Z == -1) // strafe left
				{
					s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
					s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
				}
				if(s.direction.Z == 1 && s.direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.Z == -1 && s.direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(s.player_position.X - s.mob_to_LookAt_pos.X < 0.5)
					{
						
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.X == 1 && s.direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
				if(s.direction.X == -1 && s.direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(s.player_position.Z - s.mob_to_LookAt_pos.Z < 0.5)
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, 0, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, -1, _t);
					}
					else
					{
						s.blend_direction.X = Mathf.Lerp(s.blend_direction.X, -1, _t);
						s.blend_direction.Y = Mathf.Lerp(s.blend_direction.Y, 0, _t);
					} 
				}
			}
	}
}
