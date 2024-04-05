using Godot;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{

	private float _t = 0.0f;
	private float speed = 5.0f; // speed of character
	private float dash_speed = 10.0f;
	private int damage = 10;
	private int health = 20;
	private int resource = 20;
	private TextureProgressBar health_icon;
	private TextureProgressBar resource_icon;
	private Vector3 _targetVelocity = Vector3.Zero;
	private Vector3 dash_velocity = Vector3.Zero;
	private Node3D player_body;
	private bool can_move = true;
	bool dashing;
	private int dash_time = 0;
	private int attack_freeze = 0;
	private AnimationTree tree;
	private Area3D weapon_hitbox; // weapon hitbox
	private Area3D player_hitbox;
	private Area3D vision;
	private Area3D target;
	private bool enemy_in_vision = false;
	private Vector3 player_position;
	private Vector3 enemy_position;
	private Vector3 vision_position;
	private bool targeting = false;
	private bool player_Z_more_than_target_Z; 
	private bool player_Z_less_than_target_Z; 
	private bool player_X_more_than_target_X; 
	private bool player_X_less_than_target_X; 
	private CustomSignals _customSignals;

	

	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		health_icon = GetNode<TextureProgressBar>("CanvasLayer/HBoxContainer/PanelHealthContainer/HealthContainer/HealthIcon");
		resource_icon = GetNode<TextureProgressBar>("CanvasLayer/HBoxContainer/PanelResourceContainer/ResourceContainer/ResourceIcon");
		health_icon.MaxValue = health;
		resource_icon.MaxValue = resource;
		tree = GetNode<AnimationTree>("AnimationTree");
		player_body = GetNode<Node3D>("PlayerHitbox");
		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnAreaEntered;
		vision.AreaExited += OnAreaExited;
		tree.AnimationFinished += OnAnimationFinsihed;
		weapon_hitbox = (Area3D)GetNode("Skeleton3D/BoneAttachment3D/axe/weapon_area");
		weapon_hitbox.AreaEntered += OnHitboxEntered;
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyTargeted += HandleEnemyTargeted;
		_customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		var direction = Vector3.Zero;
        Vector3 velocity = Velocity;
		Vector3 player_position = player_body.GlobalPosition;
		// float current_y_rotation;
		// float target_y_rotation;
		resource = 0;
		Vector2 blend_direction;
		GD.Print("Enemy Position", enemy_position);
		
		
		bool dashing = false;
		bool dash_right = false;
		bool dash_left = false;
		bool dash_back = false;
		bool dash_forward = false;
		
		
		if(can_move)
		{
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
		}

		
		UpdateHealth();
		UpdateResource();

		if(enemy_in_vision)
			{
				// GD.Print(enemy_position);
				lock_on(enemy_position);
				
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

    		
			// current_y_rotation = GlobalRotation.Y;
			// target_y_rotation  = Basis.LookingAt(GlobalPosition + direction).GetEuler().Y;
			
			// if(current_y_rotation - target_y_rotation > 1.5 || current_y_rotation - target_y_rotation < -1.5)
			// {
			// 	_t = 0.9f;
			// 	GD.Print("Over 180");
			// }
			// else
			// {
			// 	_t = 0.6f;
			// 	GD.Print(current_y_rotation - target_y_rotation);
			// }
			// GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(current_y_rotation, target_y_rotation, _t)};
		
	
		}
		
		if(targeting)
		{
			// GD.Print("Targeting");
			
			LookAt(enemy_position with {Y = GlobalPosition.Y});
						
			blend_direction.X = direction.X;
			blend_direction.Y = direction.Z;
			
			if(player_position.Z - enemy_position.Z > 0)
			{
				player_Z_more_than_target_Z = true;
				player_Z_less_than_target_Z = false;
				// GD.Print("player_Z_more_than_target_Z");
			}
			if(player_position.Z - enemy_position.Z < 0)
			{
				player_Z_less_than_target_Z = true;
				player_Z_more_than_target_Z = false;
				// GD.Print("player_Z_less_than_target_Z ");
			}
			if((player_position.X - enemy_position.X) > 0)
			{
				player_X_more_than_target_X = true;
				player_X_less_than_target_X = false;
				// GD.Print("player_X_more_than_target_X");
				
			}
			if((player_position.X - enemy_position.X) < 0)
			{
				player_X_less_than_target_X = true;
				player_X_more_than_target_X = false;
				// GD.Print("player_X_less_than_target_X");
			}

			if(player_Z_more_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("player_Z_more_than_target_Z && player_X_more_than_target_X");
				if (direction.Z > 0 && direction.X > 0) // +Z +X 
				{
					blend_direction.Y = -1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk away");
				}
				if (direction.Z < 0 && direction.X < 0) // -Z -X 
				{
					blend_direction.Y = 1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk toward");
				}
				if (direction.Z < 0 && direction.X > 0) // -Z +X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X = 1.0f;
					// GD.Print("strafe right");
				}
				if (direction.Z > 0 && direction.X < 0) // +Z -X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X = -1.0f;
					// GD.Print("strafe left");
				}
				if (direction.Z > 0 && direction.X == 0) // +Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.25f;
						blend_direction.X = -0.75f;
					}
					
					// GD.Print("strafe left 75 ");
				}
				if (direction.Z < 0 && direction.X == 0) // -Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.25f;
						blend_direction.X = 0.75f;
					}
					
					// GD.Print("strafe right 75");
				}
				if (direction.Z == 0 && direction.X > 0) // 0Z +X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.75f;
						blend_direction.X = 0.25f;
					}
					
					// GD.Print("split");
				}
				if (direction.Z == 0 && direction.X < 0) // 0Z -X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.75f;
						blend_direction.X = -0.250f;
					}
					
					// GD.Print("split");
				}
			}
			if(player_Z_more_than_target_Z && player_X_less_than_target_X)
			{
				// GD.Print("player_Z_more_than_target_Z && player_X_less_than_target_X");
				if (direction.Z > 0 && direction.X > 0) // +Z +X 
				{
					blend_direction.Y = -0.25f;
					blend_direction.X = 1.0f;
					// GD.Print("strafe right");
				}
				if (direction.Z < 0 && direction.X < 0) // -Z -X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X = -1.0f;
					// GD.Print("strafe left");
				}
				if (direction.Z < 0 && direction.X > 0) // -Z +X 
				{
					blend_direction.Y = 1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk toward");
				}
				if (direction.Z > 0 && direction.X < 0) // +Z -X 
				{
					blend_direction.Y = -1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk away");
				}
				if (direction.Z > 0 && direction.X == 0) // +Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.25f;
						blend_direction.X = 0.75f;
					}
		
					// GD.Print("strafe right 75");
				}
				if (direction.Z < 0 && direction.X == 0) // -Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.25f;
						blend_direction.X = -0.75f;
					}
					// GD.Print("strafe left");
				}
				if (direction.Z == 0 && direction.X > 0) // 0Z +X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.5f;
						blend_direction.X = -0.5f;
					}
					
					// GD.Print("split");
				}
				if (direction.Z == 0 && direction.X < 0) // 0Z -X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.5f;
						blend_direction.X = 0.5f;
					}
					
					// GD.Print("split");
				}
			}
			if(player_Z_less_than_target_Z && player_X_less_than_target_X)
			{
				// GD.Print("player_Z_less_than_target_Z && player_X_less_than_target_X");
				if (direction.Z > 0 && direction.X > 0) // +Z +X 
				{
					blend_direction.Y = 1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk toward");
				}
				if (direction.Z < 0 && direction.X < 0) // -Z -X 
				{
					blend_direction.Y = -1.0f;
					blend_direction.X =  0.0f;
					// GD.Print("walk away");
				}
				if (direction.Z < 0 && direction.X > 0) // -Z +X 
				{
					blend_direction.Y = -0.0f;
					blend_direction.X = -1.0f;
					// GD.Print("strafe left");
				}
				if (direction.Z > 0 && direction.X < 0) // +Z -X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X = 1.0f;
					// GD.Print("strafe right 75");
				}
				if (direction.Z > 0 && direction.X == 0) // +Z 0X 
				{
					
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.75f;
						blend_direction.X = -0.25f;
					}
					
					// GD.Print("strafe right");
				}
				if (direction.Z < 0 && direction.X == 0) // -Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.75f;
						blend_direction.X = 0.25f;
					}
					
					// GD.Print("strafe left");
				}
				if (direction.Z == 0 && direction.X > 0) // 0Z +X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.5f;
						blend_direction.X = -0.5f;
					}
				
					// GD.Print("split");
				}
				if (direction.Z == 0 && direction.X < 0) // 0Z -X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.5f;
						blend_direction.X = 0.5f;
					}
					// GD.Print("split");
				}
			}
			if(player_Z_less_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("player_Z_less_than_target_Z && player_X_more_than_target_X");
				if (direction.Z > 0 && direction.X > 0) // +Z +X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X = -1.0f;
					// GD.Print("strafe left");
				}
				if (direction.Z < 0 && direction.X < 0) // -Z -X 
				{
					blend_direction.Y = 0.0f;
					blend_direction.X =  1.0f;
					// GD.Print("strafe right");
				}
				if (direction.Z < 0 && direction.X > 0) // -Z +X 
				{
					blend_direction.Y = -1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk away");
				}
				if (direction.Z > 0 && direction.X < 0) // +Z -X 
				{
					blend_direction.Y = 1.0f;
					blend_direction.X = 0.0f;
					// GD.Print("walk toward");
				}
				if (direction.Z > 0 && direction.X == 0) // +Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.75f;
						blend_direction.X = -0.25f;
					}
					blend_direction.Y = 0.0f;
					blend_direction.X = -1.0f;
					// GD.Print("strafe left");
				}
				if (direction.Z < 0 && direction.X == 0) // -Z 0X 
				{
					if(MathF.Abs(MathF.Abs(player_position.X) - MathF.Abs(enemy_position.X)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.75f;
						blend_direction.X = 0.25f;
					}
					// GD.Print("strafe right");
				}
				if (direction.Z == 0 && direction.X > 0) // 0Z +X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = -1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = -0.5f;
						blend_direction.X = 0.0f;
					}
					// GD.Print("split");
				}
				if (direction.Z == 0 && direction.X < 0) // 0Z -X 
				{
					if(MathF.Abs(MathF.Abs(player_position.Z) - MathF.Abs(enemy_position.Z)) < 0.5)
					{
						blend_direction.Y = 1.0f;
						blend_direction.X = 0f;
					}
					else
					{
						blend_direction.Y = 0.5f;
						blend_direction.X = 0.0f;
					}
					
					// GD.Print("split");
				}
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
		if(attack_freeze != 0)
		{
			can_move = false;
			attack_freeze -= 1;
		}
		else
		{
			can_move = true;
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
			
			weapon_hitbox.AddToGroup("attacking");
			weapon_hitbox.Monitoring = true;
			can_move = false;
			attack_freeze = 13;
			return true;
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

	private void OnAreaExited(Area3D interactable) // handler for area exited signal
	{
		if(interactable.IsInGroup("enemy"))
		{
			enemy_in_vision = false;
		}
		
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerDamage), damage);
			weapon_hitbox.RemoveFromGroup("attacking");
			// GD.Print("enemy hit");
		
		}
		
	}

	private void OnAnimationFinsihed(StringName animName)
    {
	
		if(animName == "Attack")
		{
			weapon_hitbox.Monitoring = false;
			can_move = true;
			attack_freeze = 0;
			weapon_hitbox.RemoveFromGroup("attacking");
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
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyTargeted));
				targeting = true;
			}
			else
			{
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyUnTargeted));
				targeting = false;
			}
			
		}

	}

	private void UpdateHealth()
	{
		health_icon.Value = health;
	}

	private void UpdateResource()
	{
		resource_icon.Value = resource;
	}

	private void HandlePlayerDamage(int DamageAmount)
		{
			DamageAmount += damage;
		}

	private void HandleEnemyPosition(Vector3 position)
    {
        enemy_position = position;
    }

	private void HandleEnemyTargeted(){}	

	private void HandleEnemyUnTargeted(){}


}
