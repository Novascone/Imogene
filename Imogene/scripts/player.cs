using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{

	private float speed = 5.0f; // Speed of character
	private float dash_speed = 10.0f; // Speed character moves when dashing
	private int damage = 10; // Prelim damage number
	private int health = 20; // Prelim health number
	private int resource = 20; // Relim resource number
	private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	private Vector3 dash_velocity = Vector3.Zero; // Velocity of dash, allows dash to scale up current velocity and be reset without affecting current velocity
	private Area3D player_hurtbox; // Player hurbox
	private bool can_move = true; // Boolean to keep track of if the play is allowed to move
	bool dashing; // Boolean to keep track of if the player is dashing
	private int dash_time = 0; // How many frames the player can dash for.
	private int attack_freeze = 0; // How many frames the player has to wait when attacking. Should probably make this a timer
	private AnimationTree tree; // Animation Tree of the player
	private Area3D weapon_hitbox; // weapon hitbox
	private Area3D vision; // Area in which the player can detect an enemy
	private Area3D target; // Targets that enter the vision area
	private bool enemy_in_vision = false; // Boolean to track if enemy is in vision
	private Vector3 player_position; // Position of the player
	private Vector3 enemy_position;
	private bool targeting = false; // Boolean to track if the player is targeting an enemy
	private bool player_Z_more_than_target_Z; // Booleans to see where player is relative the the object it is targeting
	private bool player_Z_less_than_target_Z; 
	private bool player_X_more_than_target_X; 
	private bool player_X_less_than_target_X; 
	private CustomSignals _customSignals; // Instance of CustomSignals
	private List<Area3D> mobs_in_vision;
	private List<Vector3> mob_distance_from_player;
	private MeshInstance3D targeting_icon;

	

	
	public override void _Ready()
	{
		// Setting instances of nodes and subscribing to events
		Input.MouseMode = Input.MouseModeEnum.Captured;

		health_icon = GetNode<TextureProgressBar>("CanvasLayer/HBoxContainer/PanelHealthContainer/HealthContainer/HealthIcon");
		resource_icon = GetNode<TextureProgressBar>("CanvasLayer/HBoxContainer/PanelResourceContainer/ResourceContainer/ResourceIcon");
		health_icon.MaxValue = health;
		resource_icon.MaxValue = resource;
		targeting_icon = GetNode<MeshInstance3D>("TargetingIcon");

		

		player_hurtbox = GetNode<Area3D>("PlayerHitbox");

		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnAreaEntered;
		vision.AreaExited += OnAreaExited;

		tree = GetNode<AnimationTree>("AnimationTree");
		tree.AnimationFinished += OnAnimationFinsihed;

		weapon_hitbox = (Area3D)GetNode("Skeleton3D/BoneAttachment3D/axe/weapon_area");
		weapon_hitbox.AreaEntered += OnHitboxEntered;

		mobs_in_vision = new List<Area3D>();
		mob_distance_from_player = new List<Vector3>();
		
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		// _customSignals.EnemyTargeted += HandleEnemyTargeted;
		// _customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPositon;
		
	}

   




    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		_customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		var direction = Vector3.Zero;
		Vector3 mob_to_LookAt_pos = Vector3.Zero;
		player_position = GlobalPosition;
        Vector3 velocity = Velocity;
		// float current_y_rotation;
		// float target_y_rotation;
		resource = 0;
		Vector2 blend_direction;
		
		
		bool dashing = false;
		bool dash_right = false;
		bool dash_left = false;
		bool dash_back = false;
		bool dash_forward = false;
		
		
		if(can_move) // Basic movement controller
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


		UpdateHealth();
		UpdateResource();


		if(enemy_in_vision)
			{
				// GD.Print(enemy_position);
				EnemyInVision();

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
		if(enemy_in_vision)
		{
			
		}
		if(targeting)
		{
			// GD.Print("Targeting");
			// if(targeting && Input.IsActionJustPressed("Target"))
			// {
			// 	targeting = false;
			// }
			
			// GD.Print("mob 1", mob_distance_from_player[0]);
			// GD.Print("mob 2", mob_distance_from_player[1]);

			Vector3 minima = Vector3.Zero;
			int mindex = 0;
			if (mobs_in_vision.Count > 0)
			{
				for ( int i = 0; i < mob_distance_from_player.Count; i++)
				{
					if (mob_distance_from_player[i] < minima)
					{
						minima = mob_distance_from_player[i];
						mindex = i;
						
					}
				}
				mob_to_LookAt_pos = mobs_in_vision[mindex].GlobalPosition;
			}
			else
			{
				mob_to_LookAt_pos = Vector3.Zero;
				targeting_icon.Visible = false;
			}
			
			
	
			LookAt(mob_to_LookAt_pos with {Y = GlobalPosition.Y});
			targeting_icon.GlobalPosition = mob_to_LookAt_pos with {Y = 4};
			if(mobs_in_vision.Count >= 1)
			{
				targeting_icon.Visible = true;
			}
			else
			{
				targeting_icon.Visible = false;
			}
			

						
			blend_direction.X = direction.X;
			blend_direction.Y = direction.Z;
			
			// Checks player position relative to enemy
			

			// Changes the animation based on where the player is relative to the enemy, I'm sure there is a better way to handle this tho
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
			// Sets the animation to walk forward when not targeting
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
			// Matches dash animations to the direction of the player
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

		
		

		// Set Velocity
		velocity.X = direction.X * speed;
		velocity.Z = direction.Z * speed;
		
	
		if(dash_time != 0)
		{
			// Lerps to zero
			velocity.X += Mathf.Lerp(dash_velocity.X, 0, 0.1f);
			velocity.Z += Mathf.Lerp(dash_velocity.Z, 0, 0.1f);
			dash_time -= 1;
		}
		if(dash_time == 1)
		{
			// Sets velocity to 0 after dash
			velocity = Vector3.Zero;
		}
		if(attack_freeze != 0)
		{
			// Stops player from moving when attacking
			can_move = false;
			attack_freeze -= 1;
		}
		else
		{
			can_move = true;
		}
		
		// Set global velocity
		Velocity = velocity;
		// Set animations
		tree.Set("parameters/IW/blend_position", blend_direction);
		tree.Set("parameters/conditions/dash_back", dash_back);
		tree.Set("parameters/conditions/dash_forward", dash_forward);
		tree.Set("parameters/conditions/dash_left", dash_left);
		tree.Set("parameters/conditions/dash_right", dash_right);
		tree.Set("parameters/conditions/attacking", attack_check());
		MoveAndSlide();

    }


	public void dash()	// Increases velocity
	{
		dash_velocity = Vector3.Zero; // resets dash_velocity so it always moves in the right direction
		dash_velocity += Velocity * 4;
	}

    public bool attack_check() // changes weapon hitbox monitoring based on animation
	{
		if(Input.IsActionPressed("Attack"))
		{
			
			weapon_hitbox.AddToGroup("attacking"); // Adds weapon to attacking group
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
			mobs_in_vision.Add(interactable);
			GD.Print("Mobs in vision ",mobs_in_vision.Count);
			foreach( Area3D mob in mobs_in_vision)
			{
				mob_distance_from_player.Add(player_position - mob.GlobalPosition);
			}
			GD.Print(mobs_in_vision);
			
			// GD.Print(enemy_position);
	
		}
		
	}

	private void OnAreaExited(Area3D interactable) // handler for area exited signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			if(mobs_in_vision.Count >= 1)
			{
				mobs_in_vision.Remove(interactable);
				GD.Print("Mobs in vision ",mobs_in_vision.Count);
				GD.Print("removed", interactable);
			}
			else
			{
				GD.Print("Mobs in vision ",mobs_in_vision.Count);
				enemy_in_vision = false;
			}
			
			
			// GD.Print(mobs_in_vision);
		}
		
		
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerDamage), damage); // Sends how much damage the player does to the enemy
			weapon_hitbox.RemoveFromGroup("attacking"); // Removes weapon from attacking group
			// GD.Print("enemy hit");
		
		}
		
	}

	private void OnAnimationFinsihed(StringName animName) // when animation is finished
    {
	
		if(animName == "Attack")
		{
			weapon_hitbox.Monitoring = false;
			can_move = true;
			attack_freeze = 0;
			weapon_hitbox.RemoveFromGroup("attacking");
		}
        
    }

	private void EnemyInVision() // Emits signal when enemy is targeted/ untargeted
	{
		
		if(Input.IsActionJustPressed("Target"))
		{
			if(!targeting)
			{
				// _customSignals.EmitSignal(nameof(CustomSignals.EnemyTargeted), targeted_mob);
				targeting = true;
				targeting_icon.Visible = true;
				GD.Print("Targeted");
			}
			else
			{
				// _customSignals.EmitSignal(nameof(CustomSignals.EnemyUnTargeted));
				targeting = false;
				targeting_icon.Visible = false;
				GD.Print("Untargeted");
			}
			
		}

	}

	private void UpdateHealth() // Updates UI health
	{
		health_icon.Value = health;
	}

	private void UpdateResource() // Updates UI resource
	{
		resource_icon.Value = resource;
	}

	private void HandlePlayerDamage(int DamageAmount) // Sends damage amount to enemy
		{
			DamageAmount += damage;
		}

	private void HandleEnemyPosition(Vector3 position) // Gets enemy position from enemy
    {
        enemy_position = position;
    }

	private void HandlePlayerPositon(Vector3 position){} // Sends player position to enemy


	// private void HandleEnemyTargeted(Area3D targeted_mob){}	

	// private void HandleEnemyUnTargeted(){}


}
