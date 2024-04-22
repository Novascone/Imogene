using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;


public partial class player : Entity
{
	Roll roll;
	 // Speed of character
	private float roll_speed = 10.0f; // Speed character moves when rolling
	// private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	// private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	private Vector3 roll_velocity = Vector3.Zero; // Velocity of roll, allows roll to scale up current velocity and be reset without affecting current velocity
	private Area3D hurtbox; // Player hurbox
	public bool rolling; // Boolean to keep track of if the player is rolling
	// public int roll_time = 13; // How many frames the player can roll for.
	public AnimationTree tree; // Animation Tree of the player
	private Area3D hitbox; // weapon hitbox
	private Area3D vision; // Area in which the player can detect an enemy
	private Area3D target; // Targets that enter the vision area
	private bool enemy_in_vision = false; // Boolean to track if enemy is in vision
	private Vector3 player_position; // Position of the player
	private bool targeting = false; // Boolean to track if the player is targeting an enemy
	private bool player_Z_more_than_target_Z; // Booleans to see where player is relative the the object it is targeting
	private bool player_Z_less_than_target_Z; 
	private bool player_X_more_than_target_X; 
	private bool player_X_less_than_target_X; 
	private CustomSignals _customSignals; // Instance of CustomSignals
	private Vector3 mob_to_LookAt_pos;
	private List<Vector3> mob_distance_from_player;
	private MeshInstance3D targeting_icon;
	private	Dictionary<Area3D, Vector3> mob_pos; // Gets mob and the distance from the player 
	private Dictionary<Area3D,Vector3> sorted_mob_pos; // Calls  SortByDistance from Vector3DictionarySorter and sorts the mobs based on how far away they are from the player
	private List<Area3D> mobs_in_order; // Takes all the keys from sorted_mob_pos and puts them in a list
	private int mob_index = 0; // Index of mob that we want to look at
	private bool max_health_changed = true;
	public Vector2 blend_direction = Vector2.Zero;
	float _t = 0.4f;
	
	public Vector3 velocity;

	public override void _Ready()
	{
		roll = (Roll)LoadAbility("Roll");

		damage = 2;
		health = 20;
	
		hurtbox = GetNode<Area3D>("Hurtbox");
		hurtbox.AreaEntered += OnHurtboxEntered;

		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnVisionEntered;
		vision.AreaExited += OnVisionExited;

		tree = GetNode<AnimationTree>("AnimationTree");
		tree.AnimationFinished += OnAnimationFinsihed;

		hitbox = (Area3D)GetNode("Skeleton3D/BoneAttachment3D/axe/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;

		
		mob_distance_from_player = new List<Vector3>();
		mob_pos = new Dictionary<Area3D, Vector3>();
		
		
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPosition;
		_customSignals.Targeting += HandleTargeting;
		_customSignals.UIHealthUpdate += HandleUIHealth;
		_customSignals.UIHealthUpdate += HandleUIResource;
		_customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		_customSignals.UIHealthUpdate += HandleUIResourceUpdate;
		

		
	}

    


    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		if(max_health_changed)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealth), health);
			max_health_changed = false;
		}
		
		_customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		_customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
		var direction = Vector3.Zero;
		player_position = GlobalPosition;
        // Vector3 velocity = Velocity;
		float prev_y_rotation;
		float current_y_rotation;
		resource = 0;
		
		if(velocity == Vector3.Zero)
			{
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
			}
		
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


		if(enemy_in_vision)
			{
				// GD.Print(enemy_position);
				Targeting();

			}

		
		if (Input.IsActionPressed("Roll"))
		{
			rolling = true;
		}
		if(rolling)
		{
			roll.Execute(this);
		}
		else
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}

		
		
		if(!targeting)
		{
			
			prev_y_rotation = GlobalRotation.Y;
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + direction)) // looks at direction the player is moving
			{
				LookAt(GlobalPosition + direction);
			}
			current_y_rotation = GlobalRotation.Y;
			if(prev_y_rotation != current_y_rotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
			
	
		}
		
		
		// GD.Print("Enemies in sight: ",mob_pos.Count);
		
		if(mob_pos.Count == 0)
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
		
		if(targeting && enemy_in_vision)
		{
			
			if(player_Z_more_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("player_Z_more_than_target_Z && player_X_more_than_target_X");
				
				if(direction.X == 1 && direction.Z == 1) // walk away
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
				}
				if(direction.X == -1 && direction.Z == -1) // walk toward
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
				}
				if(direction.Z == -1 && direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.X, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
				}
				if(direction.Z == 1 && direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5) 
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
				}
				if(direction.X == 1 && direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
					
				}
				if(direction.X == -1 && direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
					
				}
				if(direction.X == 1 && direction.Z == -1) // strafe right
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.X == -1 && direction.Z == 1) // strafe left
				{
					
					blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
			
			}
			if(player_Z_less_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("here");
				if(direction.X == 1 && direction.Z == -1) // walk away
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
				}
				if(direction.X == -1 && direction.Z == 1) // walk toward
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
				}
				if(direction.X == 1 && direction.Z == 1) // strafe left
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.X == -1 && direction.Z == -1) // strafe right
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.Z == 1 && direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
					
				}
				if(direction.Z == -1 && direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
				}
				if(direction.X == 1 && direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					} 
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
				}
				if(direction.X == -1 && direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					}
				}
				
			}
			if(player_Z_less_than_target_Z && player_X_less_than_target_X)
			{
				if(direction.X == 1 && direction.Z == 1) // walk toward
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
				}
				if(direction.X == -1 && direction.Z == -1) // walk away
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
				}
				if(direction.Z == -1 && direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.Z == 1 && direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.X == 1 && direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.X == -1 && direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = -1; Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = 0; Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.X == 1 && direction.Z == -1) // strafe left
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.X == -1 && direction.Z == 1) // strafe right
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
			}
			if(player_Z_more_than_target_Z && player_X_less_than_target_X)
			{
				
				if(direction.X == 1 && direction.Z == -1) // walk toward
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
				}
				if(direction.X == -1 && direction.Z == 1) // walk away
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
				}
				if(direction.X == 1 && direction.Z == 1) // strafe right
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.X == -1 && direction.Z == -1) // strafe left
				{
					blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
					blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
				}
				if(direction.Z == 1 && direction.X == 0) // strafe right unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.Z == -1 && direction.X == 0) // strafe left unless player is directly perpendicular to enemy by the x axis
				{
					if(player_position.X - mob_to_LookAt_pos.X < 0.5)
					{
						
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.X == 1 && direction.Z == 0) // strafe right unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
				if(direction.X == -1 && direction.Z == 0) // strafe left unless player is directly perpendicular to enemy by the z axis
				{
					if(player_position.Z - mob_to_LookAt_pos.Z < 0.5)
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, 0, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, -1, _t);
					}
					else
					{
						blend_direction.X = Mathf.Lerp(blend_direction.X, -1, _t);
						blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, _t);
					} 
				}
			}
		
			
				
			if(Input.IsActionJustPressed("TargetNext"))
			{
				if(mob_index < mob_pos.Count - 1)
				{
					mob_index += 1;
				}
				
			}
			else if (Input.IsActionJustPressed("TargetLast"))
			{
				if(mob_index > 0)
				{
					mob_index -= 1;
				}
				
			}
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition; // assigns the mob to look at
			
			LookAt(mob_to_LookAt_pos with {Y = GlobalPosition.Y});

			// Checks player position relative to enemy
			// Changes the animation based on where the player is relative to the enemy, I'm sure there is a better way to handle this tho
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
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
			}
		}

		
		
		// Set Velocity
		
		
		if(player_position.Z - mob_to_LookAt_pos.Z > 0)
			{
				player_Z_more_than_target_Z = true;
				player_Z_less_than_target_Z = false;
				// GD.Print("player_Z_more_than_target_Z");
				
			}
			if(player_position.Z - mob_to_LookAt_pos.Z < 0)
			{
				player_Z_less_than_target_Z = true;
				player_Z_more_than_target_Z = false;
				// GD.Print("player_Z_less_than_target_Z ");
			}
			if((player_position.X - mob_to_LookAt_pos.X) > 0)
			{
				player_X_more_than_target_X = true;
				player_X_less_than_target_X = false;
				// GD.Print("player_X_more_than_target_X");
				
			}
			if((player_position.X - mob_to_LookAt_pos.X) < 0)
			{
				player_X_less_than_target_X = true;
				player_X_more_than_target_X = false;
				// GD.Print("player_X_less_than_target_X");
			}
		
		// Set global velocity
		Velocity = velocity;
		// GD.Print("velocity ", velocity);
		// GD.Print("Velocity ", Velocity);

		// Set animations
		tree.Set("parameters/IW/blend_position", blend_direction);
		tree.Set("parameters/conditions/attacking", attack_check());
		MoveAndSlide();

    }
    public bool attack_check() // changes weapon hitbox monitoring based on animation
	{
		if(Input.IsActionPressed("Attack"))
		{
			hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
			hitbox.Monitoring = true;
			can_move = false;
			return true;
		}
		
		return false;
	}

	private void OnVisionEntered(Area3D interactable) // handler for area entered signal
	{
		if(interactable.IsInGroup("enemy")) 
		{

			
			enemy_in_vision = true;
			Vector3 dist_vec = player_position - interactable.GlobalPosition;
			// if(mob_index >= mob_pos.Count - 1)
			// {
			// 	mob_index += 1;
			// }
			if(targeting && mob_pos.Count > 0)
			{
				mob_index += 1; // increments index when enemy enters so the player stays looking at the current enemy
			}
			if(!mob_pos.ContainsKey(interactable))
			{
				mob_pos.Add(interactable, dist_vec); // adds mob to list and how close it is to the player
				Sort(); // sorts the enemies by position
			}
	
		}
		
	}

	private void OnVisionExited(Area3D interactable) // handler for area exited signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(interactable);
				sorted_mob_pos.Clear();
				mobs_in_order.Clear();
			}
			else if(mob_pos.Count > 0)
			{
				if(mob_index > 0) 
				{
					mob_index -= 1; // decrements index when enemy leaves so the player keeps looking at the current enemy
				}
				
				mob_pos.Remove(interactable);
				
			}
			
		}
		
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerDamage), damage); // Sends how much damage the player does to the enemy
			hitbox.RemoveFromGroup("player_hitbox"); // Removes weapon from attacking group
			// GD.Print("enemy hit");
		}
		
	}

	private void OnAnimationFinsihed(StringName animName) // when animation is finished
    {
	
		if(animName == "Attack")
		{
			hitbox.Monitoring = false;
			can_move = true;
			hitbox.RemoveFromGroup("player_hitbox");
		}

    }

	private void OnHurtboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy_hitbox"))
		{
			GD.Print("player hit");
			TakeDamage(1);
			GD.Print(health);
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealthUpdate), 1);
		}
		
	}

	private void Targeting() // Emits signal when enemy is targeted/ untargeted
	{
		
		if(Input.IsActionJustPressed("Target"))
		{
			if(!targeting)
			{
				targeting = true;
			}
			else if(targeting)
			{
				targeting = false;
			}
			
		}

	}

	private void Sort()
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
		mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
		
	}

	private void HandlePlayerDamage(int DamageAmount) // Sends damage amount to enemy
	{
			DamageAmount += damage;
	}

	private void HandleEnemyPosition(Vector3 position) // Gets enemy position from enemy
    {
        enemy_position = position;
    }

	private void HandlePlayerPosition(Vector3 position){} // Sends player position to enemy
	private void HandleTargeting(bool targeting, Vector3 position){}
	private void HandleUIResource(int amount){}
    private void HandleUIHealth(int amount){}
	private void HandleUIHealthUpdate(int amount){}
	private void HandleUIResourceUpdate(int amount){}

	public static class Vector3DictionarySorter 
	{
		public static Dictionary<Area3D, Vector3> SortByDistance(Dictionary<Area3D, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}


}
