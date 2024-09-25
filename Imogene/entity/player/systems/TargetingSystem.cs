using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime;
using System.Runtime.CompilerServices;

public partial class TargetingSystem : Node
{

	[Export] public DirectionalRayCast ray_cast;

	// Enemies
	// public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public bool enemy_near = false;
	public bool enemy_far = false;
	public bool facing_enemy = false;
	public Enemy nearest_enemy;
	public Enemy enemy_pointed_toward;
	public	Dictionary<Enemy, Vector3> mobs= new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mobs; // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order; // List of mobs in order
	public Dictionary<Enemy, Vector3> mobs_to_left = new Dictionary<Enemy, Vector3>();
	public Dictionary<Enemy,Vector3> sorted_left; // 
	public List<Enemy> mobs_in_order_to_left;
	public Dictionary<Enemy,Vector3> sorted_right;
	public Dictionary<Enemy, Vector3> mobs_to_right = new Dictionary<Enemy, Vector3>();
	public List<Enemy> mobs_in_order_to_right;
	// public List<Enemy> enemies_in_vision = new List<Enemy>();
	public Enemy mob_looking_at;
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 

	// Targeting
	public bool rotating_to_soft_target = false;
	public bool target_pressed;
	public bool target_released;
	public int frames_held;
	public int held_threshold = 20;
	public bool targeting;
	public bool soft_target_on = true;
	public bool player_rotating;
	public bool soft_targeting;
	public bool enemy_to_soft_target;

	Vector3 direction_to_enemy;
	
	[Signal] public delegate void ShowSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void HideSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyTargetedEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyUntargetedEventHandler();
	[Signal] public delegate void BrightenSoftTargetHUDEventHandler();
	[Signal] public delegate void DimSoftTargetHUDEventHandler();
	[Signal] public delegate void RotatingEventHandler();
	[Signal] public delegate void RotationForAbilityFinishedEventHandler(bool finished);
	[Signal] public delegate void RotationForInputFinishedEventHandler(bool finished);

	public override void _Ready()
	{
		ray_cast.RemoveSoftTargetIcon += HandleRemoveSoftTargetIcon;
	}

    private void HandleRemoveSoftTargetIcon(Enemy enemy)
    {
        EmitSignal(nameof(HideSoftTargetIcon), enemy);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void Target(Player player)
	{
		// GD.Print("Rotate soft: enemy pointed toward " + enemy_pointed_toward);
		if(mob_looking_at != null && enemy_pointed_toward != null)
		{
			if(mob_looking_at != enemy_pointed_toward)
			{
				EmitSignal(nameof(RotationForAbilityFinished),false);
			}
		}
	
		ray_cast.SetTargetingRayCastDirection();
		ray_cast.GetRayCastCollisions();
		enemy_pointed_toward = ray_cast.GetEnemyWithMostCollisions();
		
		EnemyCheck();
		SoftTargetCheck(player);
		SoftTargetToggle();
		
		if(!rotating_to_soft_target) // If player is not rotation toward the soft target
		{
			Sort(player); // sorts the enemies by position
		}
		else if(rotating_to_soft_target)
		{
			SoftTargetRotation(player);
		}
		if(enemy_pointed_toward != null)
		{
			if(soft_target_on && !targeting)
			{
				enemy_pointed_toward.soft_target = true;
				EmitSignal(nameof(ShowSoftTargetIcon), enemy_pointed_toward);
				if(mobs_in_order.Count > 1)
				{
					foreach(Enemy enemy in mobs_in_order)
					{
						if(enemy != enemy_pointed_toward)
						{
							if(enemy.ui.soft_target_icon.Visible)
							{
								enemy.soft_target = false;
								EmitSignal(nameof(HideSoftTargetIcon), enemy);
								//GD.Print("hiding + " + enemy.Name + " soft target icon");
							}
							
						}
					}
				}
			}
		}
		else if(enemy_near)
		{
			nearest_enemy = mobs_in_order[0];
			nearest_enemy.soft_target = true;
			
			EmitSignal(nameof(ShowSoftTargetIcon), nearest_enemy);
			
			if(mobs_in_order.Count > 1)
			{
				foreach(Enemy enemy in mobs_in_order)
				{
					if(enemy != nearest_enemy)
					{
						if(enemy.ui.soft_target_icon.Visible)
						{
							enemy.soft_target = false;
							EmitSignal(nameof(HideSoftTargetIcon), enemy);
							//GD.Print("hiding + " + enemy.Name + " soft target icon");
						}
						
					}
				}
			}
		}

		LookAtEnemy(player);
	
	}

	public void SoftTargetToggle()
	{
		if(target_pressed)
		{
			frames_held += 1;
		}
		else if(target_released)
		{
			if(frames_held > 0)
			{
				//GD.Print(frames_held);
			}
			if(frames_held >= held_threshold)
			{
				soft_target_on = !soft_target_on;
				if(!soft_target_on)
				{
					EmitSignal(nameof(DimSoftTargetHUD));
					if(nearest_enemy != null)
					{
						if(nearest_enemy.ui.soft_target_icon.Visible)
						{
							EmitSignal(nameof(HideSoftTargetIcon), nearest_enemy);
						}
					}
				}
				else
				{
					EmitSignal(nameof(BrightenSoftTargetHUD));
				}
			}
			
			frames_held = 0;
		}
	}

	public void EnemyEnteredNear(Enemy enemy) // Called when enemy enters the small soft target zone
	{
		
		enemy.in_soft_target_small = true;
		enemy_near = true;
	}

	public void EnemyExitedNear(Enemy enemy) // Called when enemy exits the small soft target zone, checks to see if anymore enemies remain in the small soft zone
	{
		
		enemy.in_soft_target_small = false;
		enemy.soft_target = false;
		EmitSignal(nameof(HideSoftTargetIcon),enemy);
		var mobs_in_small = 0;

		foreach(Enemy enemy_in_mobs in mobs.Keys)
		{
			if(enemy_in_mobs.in_soft_target_small)
			{
				mobs_in_small += 1;
			}
		}
		if(mobs_in_small == 0)
		{
			enemy_near = false;
			nearest_enemy = null;
		}
	}

	public void EnemyEnteredFar(Enemy	enemy) // Called when enemy enters the large soft zone, adds enemy to the dictionary of enemies
	{
		// GD.Print(enemy.Name + " entered soft");
		
		enemy.in_soft_target_large = true;
		enemy_far = true;
		Vector3 enemy_position = enemy.GlobalTransform.Origin;
		if(!mobs.ContainsKey(enemy))
		{
			mobs.Add(enemy, enemy_position); // adds mob to list and how close it is to the player
		}
	}

	public void EnemyExitedFar(Enemy enemy) // Called when enemy exits the large soft zone, removes enemy from dictionary, and clears it if it's the last mob
	{
		enemy.soft_target = false;
		if(enemy.IsInGroup("enemy")) 
		{
			if (mobs.Count == 1)
			{
				mobs.Remove(enemy);
				sorted_mobs.Clear();
				mobs_in_order.Clear();
				enemy_far = false;
			}
			else if(mobs.Count > 0)
			{
				mobs.Remove(enemy);
			}
			var mobs_in_large = 0;

			foreach(Enemy enemy_in_mobs in mobs.Keys)
			{
				if(enemy_in_mobs.in_soft_target_small)
				{
					mobs_in_large += 1;
				}
			}
			if(mobs_in_large == 0)
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy);
				enemy_near = false;
				nearest_enemy = null;
			}
			if(soft_target_on)
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy);
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
		if(@event.IsActionPressed("RS"))
		{
			target_pressed = true;
			target_released = false;
		}
		if(@event.IsActionReleased("RS"))
		{
			target_pressed = false;
			target_released = true;
		}
	}

	
	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		
		if(frames_held > 1)
		{
			if(frames_held < held_threshold && target_released)
			{
				if(!targeting) // has player look at the closest enemy when targeting
				{
					targeting = true;
					if(mobs_in_order.Count > 0 && enemy_pointed_toward == null)
					{
						mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
						mob_looking_at = mobs_in_order[0];
					}
					else if (mob_looking_at == null)
					{
						mob_to_LookAt_pos = enemy_pointed_toward.GlobalPosition;
						mob_looking_at = enemy_pointed_toward;
					}
				}
				else if(targeting)
				{
					targeting = false;
					EmitSignal(nameof(EnemyUntargeted));
				}
				
			}
		}
		if (frames_held > held_threshold && enemy_near)
		{
			nearest_enemy.soft_target = false;
			EmitSignal(nameof(ShowSoftTargetIcon), nearest_enemy);
			//GD.Print("Emitting show target icon signal");
		}
		
		if(enemy_near || enemy_pointed_toward != null)
		{
			enemy_to_soft_target = true;
		}
		else
		{
			enemy_to_soft_target = false;
		}
		
		
	}

	public void SoftTargetCheck(Player player)
	{
		if(!targeting && soft_target_on)
		{
			soft_targeting = true;
		}
		else
		{
			soft_targeting = false;
		}
	}

	public void LookAtEnemy(Player player) // Look at enemy and switch between enemies
	{
		if(targeting && mobs_in_order.Count > 0)
		{
			if(Input.IsActionJustPressed("RS+"))
			{
				TargetRight();
			}
			else if (Input.IsActionJustPressed("RS-"))
			{
				TargetLeft();
			}
			
			HardTargetRotation(player);
		}

	}

	public void TargetRight() // Check if there is an enemy to the right and switch to the enemy thats is closest to the current enemy targeted, and to the right of the player
	{
		if(mobs_in_order_to_right.Count >= 1)
		{
			mob_looking_at.targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(mobs_in_order_to_right.Contains(mob_looking_at))
			{
				if(mob_looking_at == mobs_in_order_to_right[0])
				{
					if(mobs_in_order_to_right.Count >= mobs_in_order_to_right.IndexOf(mob_looking_at) + 2)
					{
						mob_to_LookAt_pos = mobs_in_order_to_right[mobs_in_order_to_right.IndexOf(mob_looking_at) + 1].GlobalPosition;
						mob_looking_at = mobs_in_order_to_right[mobs_in_order_to_right.IndexOf(mob_looking_at) + 1];
						mob_looking_at.targeted = true;
					}
				}
				else
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.targeted = true;
				}
			}
			else
			{
				if(mobs_in_order_to_right.Count >= mobs_in_order_to_right.IndexOf(mob_looking_at) + 1)
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.targeted = true;
				}
			}
		}
	}

	public void TargetLeft() // Check if there is an enemy to the left and switch to the enemy thats is closest to the current enemy targeted, and to the left of the player
	{
		if(mobs_in_order_to_left.Count >= 1)
		{
			mob_looking_at.targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(mobs_in_order_to_left.Contains(mob_looking_at))
			{
				if(mob_looking_at == mobs_in_order_to_left[0])
				{
					if(mobs_in_order_to_left.Count >= mobs_in_order_to_left.IndexOf(mob_looking_at) + 2)
					{
						mob_to_LookAt_pos = mobs_in_order_to_left[mobs_in_order_to_left.IndexOf(mob_looking_at) + 1].GlobalPosition;
						mob_looking_at = mobs_in_order_to_left[mobs_in_order_to_left.IndexOf(mob_looking_at) + 1];
						mob_looking_at.targeted = true;
					}
				}
				else
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.targeted = true;
				}
			}
			else
			{
				if(mobs_in_order_to_left.Count >= mobs_in_order_to_left.IndexOf(mob_looking_at) + 1)
				{
					mob_to_LookAt_pos = mobs_in_order_to_left[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_left[0];
					mob_looking_at.targeted = true;
				}
			}
		}
	}

	public void HardTargetRotation(Player player) // Smoothly rotate to hard target
	{
		player.prev_y_rotation = player.GlobalRotation.Y;
		player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
		EmitSignal(nameof(EnemyTargeted), mob_looking_at);
		player.current_y_rotation = player.GlobalRotation.Y;
		if(player.prev_y_rotation != player.current_y_rotation)
		{
			player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
	}

	public void SoftTargetRotation(Player player)
	{
		float facing_similarity = 0.0f;
		EmitSignal(nameof(Rotating));
		if(enemy_pointed_toward != null)
		{
			direction_to_enemy = player.GlobalPosition.DirectionTo(enemy_pointed_toward.GlobalPosition);
			if(mob_looking_at != enemy_pointed_toward)
			{
				//GD.Print("Rotate soft: mob looking at does not equal enemy pointed toward");
				mob_looking_at = enemy_pointed_toward;
				mob_to_LookAt_pos = enemy_pointed_toward.GlobalPosition;
			}
			
			
			//GD.Print("Rotating toward enemy pointed toward");
		}
		else if(enemy_near)
		{
			
			mob_looking_at = nearest_enemy;
			mob_to_LookAt_pos = nearest_enemy.GlobalPosition;
			direction_to_enemy = player.GlobalPosition.DirectionTo(nearest_enemy.GlobalPosition);
			//GD.Print("Rotating closest enemy");
		}


		player.prev_y_rotation = player.GlobalRotation.Y;
		player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
		player.current_y_rotation = player.GlobalRotation.Y;


		if(player.prev_y_rotation != player.current_y_rotation)
		{
			player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.33f)}; // smoothly rotates between the previous angle and the new angle!
		}
		
		if(player.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 10)
		{
			facing_similarity = 0.98f;
		}
		else if(player.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 10 && player.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 6)
		{
			facing_similarity = 0.7f;
		}
		else if(player.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 6)
		{
			facing_similarity = 0.6f;
		}


		if(enemy_pointed_toward == null && nearest_enemy != null)
		{
			ray_cast.LookAt(nearest_enemy.GlobalPosition with {Y = ray_cast.GlobalPosition.Y});
		}


		if(-player.GlobalBasis.Z.Dot(direction_to_enemy) > facing_similarity && Mathf.IsEqualApprox(MathF.Round(player.prev_y_rotation - player.current_y_rotation, 2), 0))
		{
			EmitSignal(nameof(RotationForAbilityFinished), true);
			rotating_to_soft_target = false;
			facing_enemy = true;
		}
		else
		{
			facing_enemy = false;
			EmitSignal(nameof(RotationForAbilityFinished), false);
		}

		
		
		
}

	public void Sort(Player player) // Sort mobs by distance
	{
		sorted_mobs = Vector3DictionarySorter.SortByDistance(mobs, player.GlobalTransform.Origin);
		mobs_in_order = new List<Enemy>(sorted_mobs.Keys);

		foreach(Enemy enemy in sorted_mobs.Keys)
		{
			AssignEnemySide(player, enemy);
		}
		if(mob_looking_at != null) // sort the enemies to the left and right of the player if the player is looking at an enemy
		{
			sorted_right = Vector3DictionarySorter.SortByDistance(mobs_to_right, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_right = new List<Enemy>(sorted_right.Keys);
			sorted_left = Vector3DictionarySorter.SortByDistance(mobs_to_left, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_left = new List<Enemy>(sorted_left.Keys);
		}
	}

	public void AssignEnemySide(Player player, Enemy enemy) // Determine if an enemy is to the left or right of the player
	{
		Vector3 enemy_position = enemy.GlobalTransform.Origin;
			var angle_between_player_and_enemy = Mathf.RadToDeg(-(-player.Transform.Basis.X.AngleTo(player.GlobalPosition.DirectionTo(enemy.GlobalPosition))));
			if(angle_between_player_and_enemy >= 0 && angle_between_player_and_enemy < 90)
			{
				
				if(!mobs_to_right.ContainsKey(enemy))
				{
					mobs_to_right.Add(enemy, enemy_position);
				}
				if(mobs_to_left.ContainsKey(enemy))
				{
					mobs_to_left.Remove(enemy);
				}
			}
			else if (angle_between_player_and_enemy >= 90 && angle_between_player_and_enemy < 180)
			{
				if(!mobs_to_left.ContainsKey(enemy))
				{
					mobs_to_left.Add(enemy, enemy_position);
				}
				if(mobs_to_right.ContainsKey(enemy))
				{
					mobs_to_right.Remove(enemy);
				}
			}
			else
			{
				if(mobs_to_right.ContainsKey(enemy))                                                                                                                                                                                                                                    
				{
					mobs_to_right.Remove(enemy);
				}
				if(mobs_to_left.ContainsKey(enemy))
				{
					mobs_to_left.Remove(enemy);
				}
			}
	}

    internal void HandleRotatePlayer()
    {
		// GD.Print("Targeting system receiving signal rotate player because of an ability");
        rotating_to_soft_target = true;
    }

    

    public static class Vector3DictionarySorter // Sorts mobs by distance
	{
		public static Dictionary<Enemy, Vector3> SortByDistance(Dictionary<Enemy, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}
