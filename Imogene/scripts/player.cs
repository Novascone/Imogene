using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
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
	private Vector3 mob_to_LookAt_pos;
	private List<Vector3> mob_distance_from_player;
	private MeshInstance3D targeting_icon;
	private	Dictionary<Area3D, Vector3> mob_pos; // Gets mob and the distance from the player 
	private Dictionary<Area3D,Vector3> sorted_mob_pos; // Calls  SortByDistance from Vector3DictionarySorter and sorts the mobs based on how far away they are from the player
	private List<Area3D> mobs_in_order; // Takes all the keys from sorted_mob_pos and puts them in a list
	private int mob_index = 0; // Index of mob that we want to look at
	


	

	
	public override void _Ready()
	{
		// Setting instances of nodes and subscribing to events
		// Input.MouseMode = Input.MouseModeEnum.Captured;
		// Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;

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

		
		mob_distance_from_player = new List<Vector3>();
		mob_pos = new Dictionary<Area3D, Vector3>();
		
		
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

		


		UpdateHealth();
		UpdateResource();


		if(enemy_in_vision)
			{
				// GD.Print(enemy_position);
				EnemyInVision();

			}

		
		if (Input.IsActionJustPressed("Roll"))
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
		
		
		// GD.Print("Enemies in sight: ",mob_pos.Count);
		
		if(mob_pos.Count == 0)
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
		// GD.Print("mob pos: ", mob_pos.Count);
		// GD.Print("mobs in order: ", mobs_in_order.Count);
		// GD.Print("mob index: ", mob_index);
		// GD.Print("mob index: ",mob_index);
		if(targeting && enemy_in_vision)
		{

			blend_direction.X = direction.X;
			blend_direction.Y = direction.Z;

			if(player_Z_more_than_target_Z && player_X_more_than_target_X)
			{
				// GD.Print("player_Z_more_than_target_Z && player_X_more_than_target_X");
				if(direction.X == 1 && direction.Z == 1) // walk away
				{
					blend_direction.X = 0;
					blend_direction.Y = -1;
				}
				if(direction.X == -1 && direction.Z == -1) // walk toward
				{
					blend_direction.X = 0;
					blend_direction.Y = 1;
				}
				if(direction.Z == -1 && direction.X == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.Z == 1 && direction.X == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 0) // strafe left 
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == -1) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 1) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
			
			}
			if(player_Z_less_than_target_Z && player_X_more_than_target_X)
			{
				if(direction.X == 1 && direction.Z == -1) // walk away
				{
					blend_direction.X = 0;
					blend_direction.Y = -1;
				}
				if(direction.X == -1 && direction.Z == 1) // walk toward
				{
					blend_direction.X = 0;
					blend_direction.Y = 1;
				}
				if(direction.X == 1 && direction.Z == 1) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == -1) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.Z == 1 && direction.X == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.Z == -1 && direction.X == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				
			}
			if(player_Z_less_than_target_Z && player_X_less_than_target_X)
			{
				if(direction.X == 1 && direction.Z == 1) // walk toward
				{
					blend_direction.X = 0;
					blend_direction.Y = 1;
				}
				if(direction.X == -1 && direction.Z == -1) // walk away
				{
					blend_direction.X = 0;
					blend_direction.Y = -1;
				}
				if(direction.Z == -1 && direction.X == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.Z == 1 && direction.X == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == -1) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 1) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
			}
			if(player_Z_more_than_target_Z && player_X_less_than_target_X)
			{
				
				if(direction.X == 1 && direction.Z == -1) // walk toward
				{
					blend_direction.X = 0;
					blend_direction.Y = 1;
				}
				if(direction.X == -1 && direction.Z == 1) // walk away
				{
					blend_direction.X = 0;
					blend_direction.Y = -1;
				}
				if(direction.X == 1 && direction.Z == 1) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == -1) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.Z == 1 && direction.X == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.Z == -1 && direction.X == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
				}
				if(direction.X == 1 && direction.Z == 0) // strafe right
				{
					blend_direction.X = 1;
					blend_direction.Y = 0;
				}
				if(direction.X == -1 && direction.Z == 0) // strafe left
				{
					blend_direction.X = -1;
					blend_direction.Y = 0;
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
			targeting_icon.GlobalPosition = mob_to_LookAt_pos with	{Y = 4};
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

		// Set animations
		// GD.Print("blend direction X ", blend_direction.X);
		// GD.Print("blend direction Y ", blend_direction.Y);
		GD.Print("direction X ", direction.X);
		GD.Print("direction Z ", direction.Z);
	
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
		
			
			
			
			// GD.Print("enemy entered");
			// GD.Print("Added", interactable);
			// GD.Print("mob count: ", mob_pos.Count);
			// GD.Print(enemy_position);
	
		}
		
	}

	private void OnAreaExited(Area3D interactable) // handler for area exited signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(interactable);
				// sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
				// mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
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
				// sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
				// mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
				// GD.Print("mob count: ", mob_pos.Count);
				// GD.Print("removed", interactable);
				// GD.Print("enemy exited");
			}
			
			
			
			
			
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
				// GD.Print("Targeted");
			}
			else if(targeting)
			{
				// _customSignals.EmitSignal(nameof(CustomSignals.EnemyUnTargeted));
				targeting = false;
				targeting_icon.Visible = false;
				// GD.Print("Untargeted");
				
			}
			
		}

	}

	private void Sort()
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
		mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
		
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
