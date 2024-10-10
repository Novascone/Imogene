using Godot;
using System;

public partial class AnimationController : Node
{

	private Vector3 blend_direction;

	private float _t = 0.2f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

	public void TargetingMovement(Player player_) // Calculates which direction the player is moving in relative to its local direction
	{
		var difference_vector_forward = -player_.Transform.Basis.Z - player_._direction;
		difference_vector_forward = difference_vector_forward.Round();

		var difference_vector_left = -player_.Transform.Basis.X - player_._direction;
		difference_vector_left = difference_vector_left.Round();

		var difference_vector_forward_left = -player_.Transform.Basis.Z + -player_.Transform.Basis.X - player_._direction;
		difference_vector_forward_left = difference_vector_forward_left.Round();

		var difference_vector_right = player_.Transform.Basis.X - player_._direction;
		difference_vector_right = difference_vector_right.Round();

		var difference_vector_forward_right = -player_.Transform.Basis.Z + player_.Transform.Basis.X - player_._direction;
		difference_vector_forward_right = difference_vector_forward_right.Round();

		var difference_vector_backward = player_.Transform.Basis.Z - player_._direction;
		difference_vector_backward = difference_vector_backward.Round();

		var difference_vector_backward_left = player_.Transform.Basis.Z + -player_.Transform.Basis.X - player_._direction;
		difference_vector_backward_left = difference_vector_backward_left.Round();

		var difference_vector_backward_right = player_.Transform.Basis.Z + player_.Transform.Basis.X - player_._direction;
		difference_vector_backward_right = difference_vector_backward_right.Round();

		if(difference_vector_forward == Vector3.Zero) // If the difference between the players forward facing direction (-Transform.Basis.Z) and the direction the play is moving in (direction) rounds to zero, play the walk forward animation, repeat for all animations
		{ 
			// GD.Print("player moving forward");
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
			blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
			
		}
		if(difference_vector_left == Vector3.Zero)
		{
			// GD.Print("Player moving left");
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
			blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
			
		}
		if(difference_vector_forward_left == Vector3.Zero)
		{
			// GD.Print("Player moving forward left");
			// put new animation here
		}
		if(difference_vector_right == Vector3.Zero)
		{
			// GD.Print("Player moving left");
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
			blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
		}
		if(difference_vector_forward_right == Vector3.Zero)
		{
			// GD.Print("Player moving forward right");
			// put new animation here
		}
		if(difference_vector_backward == Vector3.Zero)
		{
			// GD.Print("player moving backward");
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
			blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
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

	public void SetDefaultBlendDirection(Player player_) // Set the default walking direction
	{
		
		// player.targeting = false;
		if(player_._direction != Vector3.Zero)
		{
			if(player_.movement_speed.current_value == player_.movement_speed.base_value)
			{
				blend_direction.X = 0;
				blend_direction.Y = 1;
			}
			else if (player_.movement_speed.current_value == 0)
			{
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0.5f, 0.1f);
			}
			
		}
		else
		{
			blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
		}
		
	}

	public void SetClimbingAnimation(Player player_)
	{
		if(player_._direction.Y == 1.0 && player_._direction.X == 0.0)
			{
				GD.Print("player is moving up");
				// Put animation here
			}
			if(player_._direction.Y == 0.0 && player_._direction.X == 1.0)
			{
				GD.Print("player is moving to the left");
				// Put animation here
			}
			if(player_._direction.Y == 1.0 && player_._direction.X == 1.0)
			{
				GD.Print("player is moving up and to the left");
				// Put animation here
			}
			if(player_._direction.Y == 0.0 && player_._direction.X == -1.0)
			{
				GD.Print("player is moving to the right");
				// Put animation here
			}
			if(player_._direction.Y == 1.0 && player_._direction.X == -1.0)
			{
				GD.Print("player is moving up and to the right");
				// Put animation here
			}
			if(player_._direction.Y == -1.0 && player_._direction.X == 0.0)
			{
				GD.Print("player is moving down");
				// Put animation here
			}
			if(player_._direction.Y == -1.0 && player_._direction.X == 1.0)
			{
				GD.Print("player is moving down and to the left");
				// Put animation here
			}
			if(player_._direction.Y == -1.0 && player_._direction.X == -1.0)
			{
				GD.Print("player is moving down and to the right");
				// Put animation here
			}
	}
	
}
