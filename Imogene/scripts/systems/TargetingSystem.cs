using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetingSystem : EntitySystem
{

	
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public int mob_index = 0; // Index of mobs in list
	public	Dictionary<Enemy, Vector3> mob_pos = new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order; // List of mobs in order
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	public List<Vector3> mob_distance_from_player; // Distance from targeted mob to player
	public Enemy targeted_enemy;
	public bool target_enemy_set;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mob_distance_from_player = new List<Vector3>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(mob_pos.Count == 0) // Reset enemy_in_vision
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
		
	}

	public void EnemyEntered(Enemy enemy)
	{
		
		if(enemy.IsInGroup("enemy")) 
			{
				GD.Print(enemy.Name + " entered vision ");
				enemy_in_vision = true;
				Vector3 enemy_position = enemy.GlobalTransform.Origin;
				if(player.targeting && mob_pos.Count > 0)
				{
					mob_index += 1; // increments index when enemy enters so the player stays looking at the current enemy
				}
				if(!mob_pos.ContainsKey(enemy))
				{
					mob_pos.Add(enemy, enemy_position); // adds mob to list and how close it is to the player
					Sort(); // sorts the enemies by position
				}
		
			}
	}

	public void EnemyExited(Enemy enemy)
	{
		if(enemy.IsInGroup("enemy")) 
		{
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(enemy);
				GD.Print("removed " + enemy.Name);
				sorted_mob_pos.Clear();
				mobs_in_order.Clear();
			}
			else if(mob_pos.Count > 0)
			{
				if(mob_index > 0) 
				{
					mob_index -= 1; // decrements index when enemy leaves so the player keeps looking at the current enemy
				}
				
				mob_pos.Remove(enemy);
				Sort();
				GD.Print("removed " + enemy.Name);
				
			}
				
		}
		if(enemy == targeted_enemy)
		{
			player.ui.hud.enemy_health.EnemyUntargeted();
			targeted_enemy.targeted = false;
		}
	}

	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		if(enemy_in_vision)
		{
			if(Input.IsActionJustPressed("Target"))
			{
				target_enemy_set = false;
				if(!player.targeting)
				{
					player.targeting = true;
				}
				else if(player.targeting)
				{
					player.targeting = false;
					// _customSignals.EmitSignal(nameof(CustomSignals.EnemyUntargetedUI));
					player.ui.hud.enemy_health.EnemyUntargeted();
					targeted_enemy.targeted = false;
					
				}
				
			}
		}
	}

	public void LookAtOver() // Look at enemy and switch
	{
		if(player.targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			
			// target_ability.Execute(this);
			if(Input.IsActionJustPressed("TargetNext"))
			{
				target_enemy_set = false;
				if(mob_index < mob_pos.Count - 1)
				{
					mob_index += 1;
					mobs_in_order[mob_index -1].targeted = false;
					GD.Print("mob to be untargeted" + mobs_in_order[mob_index - 1].Name);
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
					GD.Print("mob to be untargeted" + mobs_in_order[mob_index + 1].Name);
					player.ui.hud.enemy_health.EnemyUntargeted();
					GD.Print(mobs_in_order[mob_index + 1].targeted);
				}
				
			}
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition;
			player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
			if(!target_enemy_set)
			{
				targeted_enemy = mobs_in_order[mob_index];
				GD.Print("Targeted enemy " + targeted_enemy.identifier);
				target_enemy_set = true;
				player._customSignals.EmitSignal(nameof(CustomSignals.EnemyTargetedUI), targeted_enemy);
				player.ui.hud.enemy_health.EnemyTargeted(targeted_enemy);
				targeted_enemy.targeted = true;
				GD.Print( player.identifier + " is targeting " + targeted_enemy.identifier);
			}
			
		}
		else
		{
			
			player.targeting = false;
			// Sets the animation to walk forward when not targeting
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
	}

	public void Sort() // Sort mobs by distance
	{
		GD.Print("Sorting");
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player.GlobalTransform.Origin);
		mobs_in_order = new List<Enemy>(sorted_mob_pos.Keys);
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
