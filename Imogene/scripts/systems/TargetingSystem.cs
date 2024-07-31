using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetingSystem : EntitySystem
{

	
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public bool enemy_in_soft_small = false;
	public bool enemy_in_soft_large = false;
	public int mob_index = 0; // Index of mobs in list
	public	Dictionary<Enemy, Vector3> mob_pos = new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order; // List of mobs in order
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	public List<Vector3> mob_distance_from_player; // Distance from targeted mob to player
	public Enemy strong_targeted_enemy;
	public Enemy soft_targeted_enemy;
	public bool target_enemy_set;
	public bool looking_at_soft = false;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mob_distance_from_player = new List<Vector3>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print(mob_pos.Count);
		// GD.Print("enemy in vision " + enemy_in_vision);
		// GD.Print("mob index " + mob_index);

		GD.Print("looking at soft " + looking_at_soft);
		EnemyCheck();
		if(mob_pos.Count == 0) // Reset enemy_in_vision
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
		else
		{
			if(!player.targeting && !looking_at_soft)
			{
				Sort(); // sorts the enemies by position
			}
			
			// GD.Print("The mob closest to the player is " + mobs_in_order[0].Name);
		}
		if(mobs_in_order != null)
		{
			// if(player.ability_list != null)
			// {
			GD.Print(mobs_in_order.Count);
			// }
			
			
		}
		// GD.Print("enemy in vision " + enemy_in_vision);
		// GD.Print("player targeting " + player.targeting);
		
	}

	public void EnemyEnteredVision(Enemy enemy)
	{
		
		if(enemy.IsInGroup("enemy")) 
			{
				// GD.Print(enemy.Name + " entered vision ");
				enemy_in_vision = true;
			}
	}

	public void EnemyExitedVision(Enemy enemy)
	{
		
		if(enemy == strong_targeted_enemy)
		{
			player.ui.hud.enemy_health.EnemyUntargeted();
			strong_targeted_enemy.targeted = false;
		}
	}

	public void EnemyEnteredSoftSmall(Enemy enemy)
	{
		// GD.Print(enemy.Name + " Entered soft target small");
		enemy.in_soft_target_small = true;
		enemy_in_soft_small = true;

	}

	public void EnemyExitedSoftSmall(Enemy enemy)
	{
		// GD.Print(enemy.Name + " Exited soft target small");
		enemy.in_soft_target_small = false;
		var mobs_in_small = 0;
		foreach(Enemy enemy_in_mob_pos in mob_pos.Keys)
		{
			if(enemy_in_mob_pos.in_soft_target_small)
			{
				mobs_in_small += 1;
			}
		}
		if(mobs_in_small == 0)
		{
			enemy_in_soft_small = false;
		}
	}

	public void EnemyEnteredSoftLarge(Enemy	enemy)
	{
		enemy_in_soft_large = true;
		// GD.Print(enemy.Name + " Entered soft target large");
		Vector3 enemy_position = enemy.GlobalTransform.Origin;
		if(player.targeting && mob_pos.Count > 0)
		{
			mob_index += 1; // increments index when enemy enters so the player stays looking at the current enemy
		}
		if(!mob_pos.ContainsKey(enemy))
		{
			mob_pos.Add(enemy, enemy_position); // adds mob to list and how close it is to the player
		}
	}

	public void EnemyExitedSoftLarge(Enemy enemy)
	{
		// GD.Print(enemy.Name + " Exited soft target large");
		if(enemy.IsInGroup("enemy")) 
		{
			
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(enemy);
				// GD.Print("removed " + enemy.Name);
				sorted_mob_pos.Clear();
				mobs_in_order.Clear();
				enemy_in_soft_large = false;
			}
			else if(mob_pos.Count > 0)
			{
				if(mob_index > 0) 
				{
					mob_index -= 1; // decrements index when enemy leaves so the player keeps looking at the current enemy
				}
				
				mob_pos.Remove(enemy);
				// Sort();
				// GD.Print("removed " + enemy.Name);
				
			}
				
		}
	}

	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		// GD.Print("Enemy check");
		if(enemy_in_vision)
		{
			if(Input.IsActionJustPressed("Target"))
			{
				target_enemy_set = false;
				if(!player.targeting)
				{
					player.targeting = true;
					// GD.Print("player targeting");
				}
				else if(player.targeting)
				{
					player.targeting = false;
					// _customSignals.EmitSignal(nameof(CustomSignals.EnemyUntargetedUI));
					player.ui.hud.enemy_health.EnemyUntargeted();
					strong_targeted_enemy.targeted = false;
					
					
				}
				
			}
		}
	}

	public void LookAtOver() // Look at enemy and switch
	{
		
		if(player.targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			// GD.Print("targeting");
			
			// target_ability.Execute(this);
			if(Input.IsActionJustPressed("TargetNext"))
			{
				target_enemy_set = false;
				if(mob_index < mob_pos.Count - 1)
				{
					mob_index += 1;
					mobs_in_order[mob_index -1].targeted = false;
					// GD.Print("mob to be untargeted" + mobs_in_order[mob_index - 1].Name);
					player.ui.hud.enemy_health.EnemyUntargeted();
					GD.Print(mobs_in_order[mob_index - 1].targeted);
				}
				
			}
			else if (Input.IsActionJustPressed("TargetLast"))
			{
				target_enemy_set = false;
				if(mob_index > 0)
				{
					mob_index -= 1;
					mobs_in_order[mob_index + 1].targeted = false;
					// GD.Print("mob to be untargeted" + mobs_in_order[mob_index + 1].Name);
					player.ui.hud.enemy_health.EnemyUntargeted();
					// GD.Print(mobs_in_order[mob_index + 1].targeted);
				}
				
			}
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition;
			player.prev_y_rotation = player.GlobalRotation.Y;
			player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
			player.current_y_rotation = player.GlobalRotation.Y;
			if(player.prev_y_rotation != player.current_y_rotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.prev_y_rotation, player.current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
			
			if(!target_enemy_set)
			{
				strong_targeted_enemy = mobs_in_order[mob_index];
				// GD.Print("Targeted enemy " + targeted_enemy.identifier);
				target_enemy_set = true;
				player._customSignals.EmitSignal(nameof(CustomSignals.EnemyTargetedUI), strong_targeted_enemy);
				player.ui.hud.enemy_health.EnemyTargeted(strong_targeted_enemy);
				strong_targeted_enemy.targeted = true;
				// GD.Print( player.Name + " is targeting " + strong_targeted_enemy.identifier);
			}
			
		}
		if(!player.targeting && enemy_in_soft_small)
		{
			if(player.abilities_in_use.Count > 1)
			{
				if(player.ability_list[0].resource.type == "melee")
				{
					looking_at_soft = true;
					// GD.Print("Look at closest enemy");
					
					mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
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
		if(!enemy_in_vision || !player.targeting)
		{
			player.targeting = false;
			if(player.direction != Vector3.Zero)
			{
				player.blend_direction.X = 0;
				player.blend_direction.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, 0.1f);
				player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0, 0.1f);
			}
		}
			
		// else
		// {
		// 	// player.targeting = false;
		// 	// GD.Print("setting targeting to false");
		// 	// Sets the animation to walk forward when not targeting
			
		// }
	}

	public void Sort() // Sort mobs by distance
	{
		// GD.Print("Sorting");
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player.GlobalTransform.Origin);
		mobs_in_order = new List<Enemy>(sorted_mob_pos.Keys);
		// foreach( Enemy enemy in mobs_in_order)
		// {
		// 	GD.Print(mob_pos.Count);
		// 	GD.Print(enemy.Name);
		// }
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
