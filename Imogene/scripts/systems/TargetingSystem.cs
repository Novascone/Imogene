using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetingSystem : EntitySystem
{

	
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public bool enemy_in_soft_small = false;
	public Enemy closest_enemy_soft_small;
	public bool enemy_in_soft_large = false;
	public	Dictionary<Enemy, Vector3> mobs= new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mobs; // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order; // List of mobs in order
	public Dictionary<Enemy, Vector3> mobs_to_left = new Dictionary<Enemy, Vector3>();
	public Dictionary<Enemy,Vector3> sorted_left; // 
	public List<Enemy> mobs_in_order_to_left;
	public Dictionary<Enemy,Vector3> sorted_right;
	public Dictionary<Enemy, Vector3> mobs_to_right = new Dictionary<Enemy, Vector3>();
	public List<Enemy> mobs_in_order_to_right;
	public Enemy mob_looking_at;
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	public bool looking_at_soft = false;


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		EnemyCheck(); // Check if enemy is targeted

		if(mobs.Count == 0) // Reset enemy_in_vision
		{	
			enemy_in_vision = false;
		}
		if(enemy_in_soft_small)
		{
			closest_enemy_soft_small = mobs_in_order[0];
			closest_enemy_soft_small.soft_target = true;
			player.ui.hud.enemy_health.SetSoftTargetIcon(closest_enemy_soft_small);
			foreach(Enemy enemy in mobs_in_order)
			{
				if(enemy != closest_enemy_soft_small)
				{
					enemy.soft_target = false;
					player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				}
			}
			
		}
		

		if(!looking_at_soft) // If player is not rotation toward the soft target
		{
			Sort(); // sorts the enemies by position
		}

	}

	public void EnemyEnteredVision(Enemy enemy)
	{
		
		if(enemy.IsInGroup("enemy")) 
		{
			enemy_in_vision = true;
		}
	}

	public void EnemyExitedVision(Enemy enemy)
	{
		if(enemy == mob_looking_at)
		{
			player.ui.hud.enemy_health.EnemyUntargeted();
		}
	}

	public void EnemyEnteredSoftSmall(Enemy enemy) // Called when enemy enters the small soft target zone
	{
		enemy.in_soft_target_small = true;
		enemy_in_soft_small = true;
	}

	public void EnemyExitedSoftSmall(Enemy enemy) // Called when enemy exits the small soft target zone, checks to see if anymore enemies remain in the small soft zone
	{
		enemy.in_soft_target_small = false;
		enemy.soft_target = false;
		player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
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
			enemy_in_soft_small = false;
			closest_enemy_soft_small = null;
		}
	}

	public void EnemyEnteredSoftLarge(Enemy	enemy) // Called when enemy enters the large soft zone, adds enemy to the dictionary of enemies
	{
		enemy_in_soft_large = true;
		Vector3 enemy_position = enemy.GlobalTransform.Origin;
		if(!mobs.ContainsKey(enemy))
		{
			mobs.Add(enemy, enemy_position); // adds mob to list and how close it is to the player
		}
	}

	public void EnemyExitedSoftLarge(Enemy enemy) // Called when enemy exits the large soft zone, removes enemy from dictionary, and clears it if it's the last mob
	{
		if(enemy.IsInGroup("enemy")) 
		{
			if (mobs.Count == 1)
			{
				mobs.Remove(enemy);
				sorted_mobs.Clear();
				mobs_in_order.Clear();
				enemy_in_soft_large = false;
			}
			else if(mobs.Count > 0)
			{
				mobs.Remove(enemy);
			}
		}
	}

	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		if(enemy_in_vision)
		{
			if(Input.IsActionJustPressed("Target"))
			{
				if(!player.targeting) // has player look at the closest enemy when targeting
				{
					player.targeting = true;
					mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
					mob_looking_at = mobs_in_order[0];
				}
				else if(player.targeting)
				{
					player.targeting = false;
					player.ui.hud.enemy_health.EnemyUntargeted();
				}
				
			}
		}
	}

	public void LookAtEnemy() // Look at enemy and switch between enemies
	{
		if(player.targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			if(Input.IsActionJustPressed("TargetRight"))
			{
				TargetRight();
			}
			else if (Input.IsActionJustPressed("TargetLeft"))
			{
				TargetLeft();
			}
			
			HardTargetRotation();
		}
		
		SoftTargetRotation();
		// SetDefaultBlendDirection();
		
	}

	public void TargetRight() // Check if there is an enemy to the right and switch to the enemy thats is closest to the current enemy targeted, and to the right of the player
	{
		if(mobs_in_order_to_right.Count >= 1)
		{
			mob_looking_at.targeted = false;
			player.ui.hud.enemy_health.EnemyUntargeted();
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
			player.ui.hud.enemy_health.EnemyUntargeted();
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

	public void HardTargetRotation() // Smoothly rotate to hard target
	{
		player.prev_y_rotation = player.GlobalRotation.Y;
		player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
		player.ui.hud.enemy_health.EnemyTargeted(mob_looking_at);
		player.current_y_rotation = player.GlobalRotation.Y;
		if(player.prev_y_rotation != player.current_y_rotation)
		{
			player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
	}

	public void SoftTargetRotation() // Smoothly rotate to soft target
	{
		if(!player.targeting && closest_enemy_soft_small != null)
		{
			if(player.abilities_in_use.Count > 0)
			{
				if(player.ability_in_use.resource.type == "melee")
				{
					looking_at_soft = true;
					// GD.Print("Look at closest enemy");
					
					mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
					GD.Print("rotating to soft target " + mobs_in_order[0].Name);
					player.prev_y_rotation = player.GlobalRotation.Y;
					player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
					player.current_y_rotation = player.GlobalRotation.Y;
					if(player.prev_y_rotation != player.current_y_rotation)
					{
						player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.33f)}; // smoothly rotates between the previous angle and the new angle!
					}
					
				}
			}
		}
	}

	public void Sort() // Sort mobs by distance
	{
		sorted_mobs = Vector3DictionarySorter.SortByDistance(mobs, player.GlobalTransform.Origin);
		mobs_in_order = new List<Enemy>(sorted_mobs.Keys);

		foreach(Enemy enemy in sorted_mobs.Keys)
		{
			AssignEnemySide(enemy);
		}
		if(mob_looking_at != null) // sort the enemies to the left and right of the player if the player is looking at an enemy
		{
			sorted_right = Vector3DictionarySorter.SortByDistance(mobs_to_right, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_right = new List<Enemy>(sorted_right.Keys);
			sorted_left = Vector3DictionarySorter.SortByDistance(mobs_to_left, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_left = new List<Enemy>(sorted_left.Keys);
		}
	}

	public void AssignEnemySide(Enemy enemy) // Determine if an enemy is to the left or right of the player
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
