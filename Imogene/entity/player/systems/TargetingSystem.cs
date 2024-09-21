using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

public partial class TargetingSystem : Node
{

	// Enemies
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public bool enemy_close = false;
	public bool enemy_far = false;
	public Enemy closest_enemy;
	public	Dictionary<Enemy, Vector3> mobs= new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mobs; // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order; // List of mobs in order
	public Dictionary<Enemy, Vector3> mobs_to_left = new Dictionary<Enemy, Vector3>();
	public Dictionary<Enemy,Vector3> sorted_left; // 
	public List<Enemy> mobs_in_order_to_left;
	public Dictionary<Enemy,Vector3> sorted_right;
	public Dictionary<Enemy, Vector3> mobs_to_right = new Dictionary<Enemy, Vector3>();
	public List<Enemy> mobs_in_order_to_right;
	public List<Enemy> enemies_in_vision = new List<Enemy>();
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
	
	[Signal] public delegate void ShowSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void HideSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyTargetedEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyUntargetedEventHandler();
	[Signal] public delegate void BrightenSoftTargetHUDEventHandler();
	[Signal] public delegate void DimSoftTargetHUDEventHandler();



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Target(Player player)
	{
		EnemyCheck();
		SoftTargetCheck(player);
		// if(player.ui.hud.enemy_health.targeted_enemy != null)
		// {
		// 	GD.Print("Targeted enemy " + player.ui.hud.enemy_health.targeted_enemy.Name);
		// }
		SoftTargetToggle();
		
		if(mobs.Count == 0) // Reset enemy_in_vision
		{	
			enemy_in_vision = false;
		}
	
		if(!rotating_to_soft_target) // If player is not rotation toward the soft target
		{
			Sort(player); // sorts the enemies by position
		}
		if(enemy_far)
		{
			closest_enemy = mobs_in_order[0];
			closest_enemy.soft_target = true;
			if(soft_target_on && !targeting)
			{
				if(closest_enemy.in_player_vision || enemy_close)
				{
					if(!closest_enemy.ui.soft_target_icon.Visible)
					{
						EmitSignal(nameof(ShowSoftTargetIcon),closest_enemy);
					}
					
				}
				else
				{
					if(closest_enemy.ui.soft_target_icon.Visible)
					{
						EmitSignal(nameof(HideSoftTargetIcon),closest_enemy);
					}
					
				}
				
			}
			
			if(mobs_in_order.Count > 1)
			{
				foreach(Enemy enemy in mobs_in_order)
				{
					if(enemy != closest_enemy)
					{
						if(enemy.soft_target && enemy.ui.soft_target_icon.Visible)
						{
							enemy.soft_target = false;
							EmitSignal(nameof(HideSoftTargetIcon), enemy);
						}
						
					}
				}
			}
		}

		if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
		{
			rotating_to_soft_target = false;
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
				GD.Print(frames_held);
			}
			if(frames_held >= held_threshold)
			{
				soft_target_on = !soft_target_on;
				if(!soft_target_on)
				{
					EmitSignal(nameof(DimSoftTargetHUD));
					if(closest_enemy != null)
					{
						if(closest_enemy.ui.soft_target_icon.Visible)
						{
							EmitSignal(nameof(HideSoftTargetIcon), closest_enemy);
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

	public void EnemyEnteredVision(Enemy enemy)
	{
		
		enemy_in_vision = true;
		enemy.in_player_vision = true;
		enemies_in_vision.Add(enemy);
	}

	public void EnemyExitedVision(Enemy enemy)
	{
		
		if(enemy == mob_looking_at)
		{
			EmitSignal(nameof(EnemyUntargeted));
			targeting = false;
			
		}
		enemy.in_player_vision = false;
		enemies_in_vision.Remove(enemy);
	}

	public void EnemyEnteredNear(Enemy enemy) // Called when enemy enters the small soft target zone
	{
		
		enemy.in_soft_target_small = true;
		enemy_close = true;
	}

	public void EnemyExitedNear(Enemy enemy) // Called when enemy exits the small soft target zone, checks to see if anymore enemies remain in the small soft zone
	{
		
		enemy.in_soft_target_small = false;
		enemy.soft_target = false;
		
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
			enemy_close = false;
			closest_enemy = null;
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
				enemy_close = false;
				closest_enemy = null;
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
		
		if(enemies_in_vision.Count > 0 && frames_held > 1)
		{
			if(frames_held < held_threshold && target_released)
			{
				if(!targeting) // has player look at the closest enemy when targeting
				{
					targeting = true;
					if(mobs_in_order.Count > 0)
					{
						mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
						mob_looking_at = mobs_in_order[0];
					}
				}
				else if(targeting)
				{
					targeting = false;
					EmitSignal(nameof(EnemyUntargeted));
				}
				
			}
		}
		if (frames_held > held_threshold && enemy_close)
		{
			closest_enemy.soft_target = false;
			EmitSignal(nameof(ShowSoftTargetIcon), closest_enemy);
			GD.Print("Emitting show target icon signal");
		}
		if(closest_enemy != null)
		{
			if(enemy_close || closest_enemy.in_player_vision)
			{
				enemy_to_soft_target = true;
			}
			else
			{
				enemy_to_soft_target = false;
			}
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
		if(targeting && enemies_in_vision.Count > 0 && (mobs_in_order.Count > 0))
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
		
		SoftTarget(player);
		
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

	public void SoftTarget(Player player) // Smoothly rotate to soft target
	{
		if(!targeting && closest_enemy != null && soft_target_on)
		{
			if(player.abilities_in_use.Count > 0)
			{
				// if(player.ability_in_use.resource.type == "melee" && closest_enemy_soft.in_soft_target_small) // This is what will be used when skill categories are implemented
				// {
				// 	SoftTargetRotation();
				// }
				// else if(player.ability_in_use.resource.type == "ranged" && closest_enemy_soft.in_soft_target_large)
				// {
				// 	SoftTargetRotation();
				// }
				// if(player.ability_in_use.resource.type == "melee" && closest_enemy_soft.in_soft_target_large)
				// {
				// 	SoftTargetRotation();
				// }
			}
		}
	}

	public void SoftTargetRotation(Player player)
	{
		
		rotating_to_soft_target = true;
		mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
		player.prev_y_rotation = player.GlobalRotation.Y;
		player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
		player.current_y_rotation = player.GlobalRotation.Y;
		if(player.prev_y_rotation != player.current_y_rotation)
		{
			player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.33f)}; // smoothly rotates between the previous angle and the new angle!
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
