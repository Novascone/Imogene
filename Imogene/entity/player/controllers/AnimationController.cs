using Godot;
using System;

public partial class AnimationController : Controller
{

	private float _t = 0.2f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

	public void TargetingMovement() // Calculates which direction the player is moving in relative to its local direction
	{
		var difference_vector_forward = -player.Transform.Basis.Z - player.direction;
		difference_vector_forward = difference_vector_forward.Round();

		var difference_vector_left = -player.Transform.Basis.X - player.direction;
		difference_vector_left = difference_vector_left.Round();

		var difference_vector_forward_left = -player.Transform.Basis.Z + -player.Transform.Basis.X - player.direction;
		difference_vector_forward_left = difference_vector_forward_left.Round();

		var difference_vector_right = player.Transform.Basis.X - player.direction;
		difference_vector_right = difference_vector_right.Round();

		var difference_vector_forward_right = -player.Transform.Basis.Z + player.Transform.Basis.X - player.direction;
		difference_vector_forward_right = difference_vector_forward_right.Round();

		var difference_vector_backward = player.Transform.Basis.Z - player.direction;
		difference_vector_backward = difference_vector_backward.Round();

		var difference_vector_backward_left = player.Transform.Basis.Z + -player.Transform.Basis.X - player.direction;
		difference_vector_backward_left = difference_vector_backward_left.Round();

		var difference_vector_backward_right = player.Transform.Basis.Z + player.Transform.Basis.X - player.direction;
		difference_vector_backward_right = difference_vector_backward_right.Round();

		if(difference_vector_forward == Vector3.Zero) // If the difference between the players forward facing direction (-Transform.Basis.Z) and the direction the play is moving in (direction) rounds to zero, play the walk forward animation, repeat for all animations
		{ 
			// GD.Print("player moving forward");
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 1, _t);
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, _t);
			
		}
		if(difference_vector_left == Vector3.Zero)
		{
			// GD.Print("Player moving left");
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0, _t);
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, -1, _t);
			
		}
		if(difference_vector_forward_left == Vector3.Zero)
		{
			// GD.Print("Player moving forward left");
			// put new animation here
		}
		if(difference_vector_right == Vector3.Zero)
		{
			// GD.Print("Player moving left");
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0, _t);
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 1, _t);
		}
		if(difference_vector_forward_right == Vector3.Zero)
		{
			// GD.Print("Player moving forward right");
			// put new animation here
		}
		if(difference_vector_backward == Vector3.Zero)
		{
			// GD.Print("player moving backward");
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, -1, _t);
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, _t);
		}
		if(difference_vector_backward_left == Vector3.Zero)
		{
			// GD.Print("Player moving backward left");
			// put new animation here
		}
		if(difference_vector_backward_right == Vector3.Zero)
		{
			// GD.Print("Player moving backward right");
			// put new animation here
		}
	}

	public void SetDefaultBlendDirection() // Set the default walking direction
	{
		
		// player.targeting = false;
		if(player.direction != Vector3.Zero)
		{
			if(player.movement_speed.current_value == player.movement_speed.base_value)
			{
				player.blend_direction.X = 0;
				player.blend_direction.Y = 1;
			}
			else if (player.movement_speed.current_value == 0)
			{
				player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, 0.1f);
				player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0.5f, 0.1f);
			}
			
		}
		else
		{
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, 0.1f);
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0, 0.1f);
		}
		
	}

	public void SetClimbingAnimation()
	{
		if(player.direction.Y == 1.0 && player.direction.X == 0.0)
			{
				GD.Print("player is moving up");
				// Put animation here
			}
			if(player.direction.Y == 0.0 && player.direction.X == 1.0)
			{
				GD.Print("player is moving to the left");
				// Put animation here
			}
			if(player.direction.Y == 1.0 && player.direction.X == 1.0)
			{
				GD.Print("player is moving up and to the left");
				// Put animation here
			}
			if(player.direction.Y == 0.0 && player.direction.X == -1.0)
			{
				GD.Print("player is moving to the right");
				// Put animation here
			}
			if(player.direction.Y == 1.0 && player.direction.X == -1.0)
			{
				GD.Print("player is moving up and to the right");
				// Put animation here
			}
			if(player.direction.Y == -1.0 && player.direction.X == 0.0)
			{
				GD.Print("player is moving down");
				// Put animation here
			}
			if(player.direction.Y == -1.0 && player.direction.X == 1.0)
			{
				GD.Print("player is moving down and to the left");
				// Put animation here
			}
			if(player.direction.Y == -1.0 && player.direction.X == -1.0)
			{
				GD.Print("player is moving down and to the right");
				// Put animation here
			}
	}
	
}
