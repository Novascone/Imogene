using Godot;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{
	
	private float speed = 5.0f; // speed of character
	private float dash_speed = 10.0f;
	private Vector3 _targetVelocity = Vector3.Zero;
	private Vector3 dash_velocity = Vector3.Zero;
	private Node3D player_body;
	private bool can_move = true;
	bool dashing;
	private int dash_time = 0;
	private AnimationTree tree;
	private Area3D weapon_hitbox; // weapon hitbox
	private Area3D player_hitbox;
	private Area3D vision;
	private Area3D target;
	private bool enemy_in_vision = false;
	private Vector3 player_position;
	private Vector3 enemy_position;
	private bool targeting = false;
	private bool above;
	private bool below;
	

	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		tree = GetNode<AnimationTree>("AnimationTree");
		player_body = GetNode<Node3D>("PlayerCollision");
		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnAreaEntered;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		if(enemy_in_vision)
			{
				// GD.Print(enemy_position);
				lock_on(enemy_position);
			}

		
		
	}

    public override void _PhysicsProcess(double delta)
    {
		
		var direction = Vector3.Zero;
        Vector3 velocity = Velocity;
		Vector3 player_position = player_body.GlobalPosition;
		bool dashing = false;
		bool dash_right = false;
		bool dash_left = false;
		bool dash_back = false;
		bool dash_forward = false;
		
		

		if (Input.IsActionPressed("Right"))
		{
			direction.X -= 1.0f;		
		}
		if (Input.IsActionPressed("Left"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("Backward"))
		{
			direction.Z -= 1.0f;

		}
		if (Input.IsActionPressed("Forward"))
		{
			direction.Z += 1.0f;
		}
		

		
		if (Input.IsActionJustPressed("Dash"))
		{
			dash();
			dashing = true;
			dash_time = 10;
		}

		if(!targeting)
		{
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + direction))

    		{
				LookAt(GlobalPosition + direction);
				
    		}
	
		}
		if(targeting)
		{
			// GD.Print("Targeting");
			LookAt(enemy_position with {Y = 2.5f});
		}
		Vector2 blend_direction;
		if(targeting)
		{
			
			blend_direction.X = direction.X;
			blend_direction.Y = direction.Z;
			
			if((Math.Abs(player_position.Z) - Math.Abs(enemy_position.Z)) < 0)
			{
				above = true;
				below = false;
			}
			if((Math.Abs(player_position.Z) - Math.Abs(enemy_position.Z)) > 0)
			{
				below = true;
				above = false;
			}

			if(below)
			{
				above = false;
				blend_direction.X *= -1;
			}
			if(above)
			{
				below = false;
				blend_direction.Y *= -1;
			}
		}
		
		else
		{
			if(direction != Vector3.Zero)
			{
				blend_direction.X = 0;
				blend_direction.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				blend_direction.X = 0;
				blend_direction.Y = 0;
			}
		}

		if(dashing)
		{
			if(blend_direction.X > 0 && blend_direction.Y > 0)
			{
				dash_right = true;
			}
			if(blend_direction.X < 0 && blend_direction.Y < 0)
			{
				dash_left = true;
			}
			if(blend_direction.X > 0 && blend_direction.Y < 0)
			{
				dash_right = true;
			}
			if(blend_direction.X < 0 && blend_direction.Y > 0)
			{
				dash_left = true;
			}
			if(blend_direction.X > 0 && blend_direction.Y == 0)
			{
				dash_right = true;
			}
			if(blend_direction.X < 0 && blend_direction.Y == 0)
			{
				dash_left = true;
			}
			if(blend_direction.X == 0 && blend_direction.Y > 0)
			{
				dash_forward = true;
			}
			if(blend_direction.X == 0 && blend_direction.Y < 0)
			{
				dash_back = true;
			}
		}

		
		

		
		velocity.X = direction.X * speed;
		velocity.Z = direction.Z * speed;
		
		
		

		if(dash_time != 0)
		{
			velocity.X += Mathf.Lerp(dash_velocity.X, 0, 0.1f);
			velocity.Z += Mathf.Lerp(dash_velocity.Z, 0, 0.1f);
			dash_time -= 1;
		}
		if(dash_time == 1)
		{
			velocity = Vector3.Zero;
		}
		Velocity = velocity;
		tree.Set("parameters/IW/blend_position", blend_direction);
		tree.Set("parameters/conditions/dash_back", dash_back);
		tree.Set("parameters/conditions/dash_forward", dash_forward);
		tree.Set("parameters/conditions/dash_left", dash_left);
		tree.Set("parameters/conditions/dash_right", dash_right);
		tree.Set("parameters/conditions/attacking", attack_check());
		MoveAndSlide();

    }


	public void dash()
	{
		dash_velocity = Vector3.Zero; // resets dash_velocity so it always moves in the right direction
		dash_velocity += Velocity * 4;
	}

    public bool attack_check() // changes weapon hitbox monitoring based on animation
	{
		if(Input.IsActionPressed("Attack"))
		{
			// weapon_hitbox.Monitoring = true;
			return true;
		}
		
		else if(Input.IsActionJustReleased("Attack"))
		{
			// weapon_hitbox.Monitoring = false;
			can_move = false;
			return false;
		}

		return false;
	}



	private void OnAreaEntered(Area3D interactable) // handler for area entered signal
	{
		if(interactable.IsInGroup("enemy"))
		{
			enemy_in_vision = true;
			enemy_position = interactable.GlobalPosition ;
			// GD.Print(enemy_position);
			get_enemy_position(interactable);
	
		}
		
	}

	private Vector3 get_enemy_position(Area3D interactable)
	{
		return interactable.GlobalPosition;
	}

	private void lock_on(Vector3 enemy_position)
	{
		
		if(Input.IsActionJustPressed("Target"))
		{
			if(!targeting)
			{
				targeting = true;
			}
			else
			{
				targeting = false;
			}
			
			// GD.Print("Targeting");
			
		}

	}



}
