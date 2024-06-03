using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerEntity : Entity
{

	public int strength = 1; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	public int dexterity = 1; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	public int intellect = 1; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	public int vitality = 1; // Vitality: Primary stat for health
	public int stamina = 1; // Primary stat for resource and regeneration
	public int wisdom = 1; // Increases the damage of abilities that use wisdom, also used for interactions
	public int charisma = 1; // Primary Stat for character interaction

	public float physical_melee_power; // Increases physical melee DPS by 1 every 15 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_melee_power; // Increases melee magic DPS by 1 every 15 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
	public float physical_ranged_power; // Increases physical ranged DPS by 1 every 15 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_ranged_power;  // Increases physical ranged DPS by 1 every 15 points + 3 for every point of dexterity + 1 for every point of strength
	public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	public float critical_hit_chance; // Percentage change for hit to be a critical hit
	public float critical_hit_damage; // Multiplier applied to base damage if a hit is critical
 	public float physical_melee_damage; 
	public float physical_ranged_damage; 
	public float spell_melee_damage;
	public float spell_ranged_damage;
	

	public int physical_melee_attack_abilities; // Total number of attack abilities in each category
	public int physical_ranged_attack_abilities;
	public int spell_melee_attack_abilities;
	public int spell_ranged_attack_abilities;
	public int wisdom_attack_abilities;

	public float physical_melee_attack_ability_ratio; 
	public float physical_ranged_attack_ability_ratio;
	public float spell_melee_attack_ability_ratio;
	public float spell_ranged_attack_ability_ratio;
	public float wisdom_attack_ability_ratio;
	public int total_attack_abilities;

	public float slash_damage;
	public float thrust_damage;
	public float blunt_damage;

	public Vector2 blend_direction = Vector2.Zero; // Blend Direction of the player for changing animation
	
	//Player equipment details
	public string weapon_type = "one_handed_axe";

	public bool targeting = false; // Is the entity targeting?
	public float prev_y_rotation; // Rotation of the player before current rotation
	public float current_y_rotation; // Rotation of the player
	public Vector3 direction; // Direction of the player
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	private int mob_index = 0; // Index of mobs in list
	private Area3D vision; // Area where the player can target enemies
	private	Dictionary<Area3D, Vector3> mob_pos = new Dictionary<Area3D, Vector3>();  // Dictionary of mob positions
	private Dictionary<Area3D,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	private List<Area3D> mobs_in_order; // List of mobs in order

	public Vector3 player_position; // Position of the player
	public Vector3 velocity; // Velocity of the player

	public bool in_interact_area; // Is the entity in an interact area
	public bool entered_interact; // Has the entity entered the an interact area?
	public bool left_interact; // has the entity left the interact area?
	public bool interacting; // Is the entity interacting?
	public Area3D interact_area; // Radius of where the player can interact

	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	private List<Vector3> mob_distance_from_player; // Distance from targeted mob to player

	public bool recovery_1 = false;
	public bool recovery_2 = false;
	public bool attack_1_set;
	public bool attack_2_set;

	public Area3D hurtbox; // Area where the player takes damage
	public Area3D hitbox; // Area where the player does damage
	// private Area3D vision; // Area where the player can target enemies
	public Node3D head_slot; // Head slot for the player


	public CustomSignals _customSignals; // Custom signal instance

	public bool using_ability; // Is the entity using an ability?

	
	
	public bool max_health_changed = true; // Has the entities heath changed?
	public bool stats_updated = true; // Have the entities stats changed?
	
	
	
	public bool can_use_abilities = true;

	// Add all player stat handling here
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnVisionEntered;
		vision.AreaExited += OnVisionExited;
		mob_distance_from_player = new List<Vector3>();

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		direction = Vector3.Zero;
		if(mob_pos.Count == 0) // Reset enemy_in_vision
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
	}

	public void UpdateStats() // Updates stats 															*** NEEDS ADDITIONS ***
	{
		level = 0;
		strength = 10;
		dexterity = 10;
		intellect = 5;
		vitality = 1;
		stamina = 1;
		wisdom = 20;
		charisma = 1;

		weapon_damage = 10;
		attack_speed = 2f;

		critical_hit_chance = 0.05f;
		critical_hit_damage = 1.2f;

		physical_melee_attack_abilities = 3;
		physical_ranged_attack_abilities = 3;
		spell_melee_attack_abilities = 3;
		spell_ranged_attack_abilities = 3;
		wisdom_attack_abilities = 3;
		total_attack_abilities = physical_melee_attack_abilities + physical_ranged_attack_abilities + spell_melee_attack_abilities + spell_ranged_attack_abilities;

		// Calculates stats
		physical_melee_attack_ability_ratio = (float)physical_melee_attack_abilities / total_attack_abilities;
		physical_ranged_attack_ability_ratio = (float)physical_ranged_attack_abilities / total_attack_abilities;
		spell_melee_attack_ability_ratio = (float)spell_melee_attack_abilities / total_attack_abilities;
		spell_ranged_attack_ability_ratio = (float)spell_ranged_attack_abilities / total_attack_abilities;
		wisdom_attack_ability_ratio = (float)wisdom_attack_abilities / total_attack_abilities;


		physical_melee_power = (2 * strength) + dexterity;
		physical_ranged_power = strength + (3 * dexterity);
		spell_melee_power = strength + dexterity + (3 * intellect);
		spell_ranged_power = (2 * dexterity) + (3 * intellect);

		physical_melee_damage = physical_melee_power/15;
		physical_ranged_damage = physical_ranged_power/15;
		spell_melee_damage = spell_melee_power/15;
		spell_ranged_damage = spell_ranged_power/15;
		wisdom_scaler = wisdom/20;

		damage = ((physical_melee_attack_ability_ratio * physical_melee_damage) + 
				 (physical_ranged_attack_ability_ratio * physical_ranged_damage) + 
				 (spell_melee_attack_ability_ratio * spell_melee_damage) + 
				 (spell_ranged_attack_ability_ratio * spell_ranged_damage) +
				 (wisdom_attack_ability_ratio * wisdom_scaler) +
				 weapon_damage) * 
				 attack_speed * 
				 (1 + (critical_hit_chance * critical_hit_damage));

		damage = (float)Math.Round(damage,2);
	}

	public void Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
		}
	}

	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
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
	}

	private void OnVisionEntered(Area3D interactable) // handler for area entered signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			enemy_in_vision = true;
			Vector3 dist_vec = player_position - interactable.GlobalPosition;
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
	public void LookAtOver() // Look at enemy and switch
	{
		if(targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			
			// target_ability.Execute(this);
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
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition;
			LookAt(mob_to_LookAt_pos with {Y = GlobalPosition.Y});
			
		}
		else
		{
			
			targeting = false;
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
	}

	public void CheckInteract() // Checks if within interact and handles input
	{
		if(in_interact_area)
		{
			if(entered_interact)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
				entered_interact = false;
			}
			
			if(Input.IsActionJustPressed("Interact") && !interacting)
			{
				interacting = true;
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			}
			else if(Input.IsActionJustPressed("Interact") && interacting)
			{
				interacting = false;
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			}
		}
		else if(left_interact)
		{
			interacting = false;
			_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			left_interact = false;
		}

	}

	public void OnHurtboxEntered(Area3D area) // handler for area entered signal
	{
		if(area.IsInGroup("enemy_hitbox"))
		{
			GD.Print("player hit");
			TakeDamage(1);
			GD.Print(health);
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealthUpdate), 1);
		}
		else if(area.IsInGroup("interactive"))
		{
			entered_interact = true;
			in_interact_area = true;
			interact_area = area;
		}
		
	}
	public void OnHurtboxExited(Area3D area) // Check if interact area has come in contact with the player
	{
		if(area.IsInGroup("interactive"))
		{
			left_interact = true;
			in_interact_area = false;
			interact_area = null;
		}
	}

	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		if(enemy_in_vision)
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
	}
	private void Sort() // Sort mobs by distance
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
		mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
	}

	public static class Vector3DictionarySorter // Sorts mobs by distance
	{
		public static Dictionary<Area3D, Vector3> SortByDistance(Dictionary<Area3D, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
	
}
