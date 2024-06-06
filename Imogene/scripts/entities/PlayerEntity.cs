using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerEntity : Entity
{


	// Stats

	public int strength = 1; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	public int dexterity = 1; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	public int intellect = 1; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	public int vitality = 1; // Vitality: Primary stat for health
	public int stamina = 1; // Primary stat for resource and regeneration
	public int wisdom = 1; // Increases the damage of abilities that use wisdom, also used for interactions
	public int charisma = 1; // Primary Stat for character interaction

	// Offense

	
	public float total_dps;
	public float physical_melee_dps;
	public float spell_melee_dps;
	public float physical_ranged_dps;
	public float spell_ranged_dps;
	public float offhand_damage;

	public float physical_melee_power; // Increases physical melee DPS by a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_melee_power; // Increases melee magic DPS a magnitude every 100 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
	public float physical_ranged_power; // Increases physical ranged DPS a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_ranged_power;  // Increases physical ranged DPS by a magnitude every 100 points + 3 for every point of dexterity + 1 for every point of strength
	public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	
 	public float physical_melee_damage; 
	public float physical_ranged_damage; 
	public float spell_melee_damage;
	public float spell_ranged_damage;

	public float physical_melee_power_mod; 
	public float physical_ranged_power_mod; 
	public float spell_melee_power_mod;
	public float spell_ranged_power_mod;
	public float power_mod_avg;

	public int damage_bonus;
	public float combined_damage;
	public float base_aps;
	public float aps_modifiers;
	public float aps;
	public float aps_mod;
	public float base_dps;
	public float skill_mod;
	public float crit_mod;



	public float slash_damage;
	public float thrust_damage;
	public float blunt_damage;
	public float bleed_damage;
	public float poison_damage;
	public float fire_damage;
	public float cold_damage;
	public float lightning_damage;
	public float holy_damage;
	public float critical_hit_chance; // Percentage change for hit to be a critical hit
	public float critical_hit_damage; // Multiplier applied to base damage if a hit is critical
	public float attack_speed_increase;
	public float cool_down_reduction;
	public float posture_damage;

	// Defense

	public int armor;
	public int poise;
	public int block_amount;
	public int retaliation;
	public int physical_resistance;
	public int thrust_resistance;
	public int slash_resistance;
	public int blunt_resistance;
	public int bleed_resistance;
	public int poison_resistance;
	public int curse_resistance;
	public int spell_resistance;
	public int fire_resistance;
	public int cold_resistance;
	public int lightning_resistance;
	public int holy_resistance;

	// Health

	public float maximum_health => health;
	public float health_bonus;
	public float health_regen;
	public float health_on_retaliate;

	// Resources

	public float maximum_resource => resource;
	public float resource_regen;
	public float resource_cost_reduction;

	// Misc
	public float movement_speed;

	// Materials

	public int maixmum_gold => gold;



	public bool max_health_changed = true; // Has the entities heath changed?
	public bool stats_updated = true; // Have the entities stats changed?
	
	// Equipment
	public string resource_path;
	public string weapon_type = "one_handed_axe";
	public Node3D head_slot; // Head slot for the player
	public MeshInstance3D helm; // Temp helm
	public Node3D main_node; // Temp helm node
	public bool remove_equipped = false;

	public Vector2 blend_direction = Vector2.Zero; // Blend Direction of the player for changing animation
	
	// Player orientation
	public Vector3 direction; // Direction of the player
	public Vector3 player_position; // Position of the player
	public Vector3 velocity; // Velocity of the player
	public float prev_y_rotation; // Rotation of the player before current rotation
	public float current_y_rotation; // Rotation of the player

	public Godot.Collections.Array<Rid> exclude = new Godot.Collections.Array<Rid>();
	

	// Targeting variables
	public Area3D vision; // Area where the player can target enemies
	public bool targeting = false; // Is the entity targeting?
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	private int mob_index = 0; // Index of mobs in list
	private	Dictionary<Area3D, Vector3> mob_pos = new Dictionary<Area3D, Vector3>();  // Dictionary of mob positions
	private Dictionary<Area3D,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	private List<Area3D> mobs_in_order; // List of mobs in order
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	private List<Vector3> mob_distance_from_player; // Distance from targeted mob to player

	// Interact variables
	public Area3D interact_area; // Radius of where the player can interact
	public bool in_interact_area; // Is the entity in an interact area
	public bool entered_interact; // Has the entity entered the an interact area?
	public bool left_interact; // has the entity left the interact area?
	public bool interacting; // Is the entity interacting?
	

	// Ability Variables
	public bool recovery_1 = false;
	public bool recovery_2 = false;
	public bool action_1_set;
	public bool action_2_set;
	public bool using_ability; // Is the entity using an ability?
	public bool can_use_abilities = true;


	// Attached objects
	public Area3D hurtbox; // Area where the player takes damage
	public Area3D hitbox; // Area where the player does damage
	public MeshInstance3D land_point;
	public Vector3 land_point_position;
	

	

	
	public CustomSignals _customSignals; // Custom signal instance

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		vision  = (Area3D)GetNode("Vision");
		land_point = GetNode<MeshInstance3D>("LandPoint");
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

	public void UpdateStats() // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{
		level = 0;
		strength = 20;
		dexterity = 30;
		intellect = 10;
		vitality = 1;
		stamina = 1;
		wisdom = 20;
		charisma = 1;



		weapon_damage = 10;
		offhand_damage = 9;
		attack_speed = 2f;
		
		
		slash_damage = 0;
		thrust_damage = 0;
	 	blunt_damage = 0;
	 	bleed_damage = 0;
	 	poison_damage = 0;
		fire_damage= 0;
	 	cold_damage = 0;
		lightning_damage = 0;
		holy_damage = 0;
	 	critical_hit_chance = 0.5f; // Percentage change for hit to be a critical hit
	 	critical_hit_damage = 0.9f; // Multiplier applied to base damage if a hit is critical
	 	attack_speed_increase = 0;
	 	cool_down_reduction = 0;
		posture_damage = 0;

		armor = 0;
		poise = 0;
		block_amount = 0;
		retaliation = 0;
		physical_resistance = 0;
		thrust_resistance = 0;
		slash_resistance = 0;
		blunt_resistance = 0;
		bleed_resistance = 0;
		poison_resistance = 0;
		curse_resistance = 0;
		spell_resistance = 0;
		fire_resistance = 0;
		cold_resistance = 0;
		lightning_resistance = 0;
		holy_resistance = 0;



		// Calculates stats
		combined_damage = weapon_damage + damage_bonus;
		aps = base_aps * (1 + aps_modifiers);
		base_dps = aps * combined_damage;

		physical_melee_power = (2 * strength) + dexterity;
		physical_ranged_power = strength + (3 * dexterity);
		spell_melee_power = strength + dexterity + (3 * intellect);
		spell_ranged_power = (2 * dexterity) + (3 * intellect);

		// physical_melee_damage = physical_melee_power/15;
		// physical_ranged_damage = physical_ranged_power/15;
		// spell_melee_damage = spell_melee_power/15;
		// spell_ranged_damage = spell_ranged_power/15;
		wisdom_scaler = wisdom/20;

		combined_damage = weapon_damage + offhand_damage + 2 + 1 + 1;

		aps = 1.71f;
		base_dps = aps * combined_damage;
		skill_mod = 1.15f;
		crit_mod = 1 + (critical_hit_chance * critical_hit_damage);

		physical_melee_power_mod = 1 + (physical_melee_power/100);
		spell_melee_power_mod = 1 + (spell_melee_power/100);
		physical_ranged_power_mod = 1 + (physical_ranged_power/100);
		spell_ranged_power_mod = 1 + (spell_ranged_power/100);

		power_mod_avg = (physical_melee_power_mod + spell_melee_power_mod + physical_ranged_power_mod + spell_ranged_power_mod) / 4;

		physical_melee_dps = base_dps * physical_melee_power_mod * skill_mod * crit_mod;
		spell_melee_dps = base_dps * spell_melee_power_mod * skill_mod * crit_mod;
		physical_ranged_dps = base_dps * physical_ranged_power_mod * skill_mod * crit_mod;
		spell_ranged_dps = base_dps * spell_ranged_power_mod * skill_mod * crit_mod;

		physical_melee_dps = (float)Math.Round(physical_melee_dps,2);
		spell_melee_dps = (float)Math.Round(spell_melee_dps,2);
		physical_ranged_dps = (float)Math.Round(physical_ranged_dps,2);
		spell_ranged_dps = (float)Math.Round(spell_ranged_dps,2);

		total_dps = base_dps * power_mod_avg * skill_mod * crit_mod;
		total_dps = (float)Math.Round(total_dps,2);
		damage = total_dps;
		GD.Print("combined damage " + combined_damage);
		GD.Print("base dps " + base_dps);
		GD.Print("aps " + aps);
		GD.Print("skill mod " + skill_mod);
		GD.Print("crit mod " + crit_mod);
		GD.Print("physical melee power mod " + physical_melee_power_mod);
		GD.Print("spell melee power mod " + spell_melee_power_mod);
		GD.Print("physical ranged power mod " + physical_ranged_power_mod);
		GD.Print("spell ranged power mod " + spell_ranged_power_mod);
		GD.Print("physical melee dps " + physical_melee_dps);
		GD.Print("spell melee dps " + spell_melee_dps);
		GD.Print("physical ranged dps " + physical_ranged_dps);
		GD.Print("spell ranged dps " + spell_ranged_dps);
	}

	public void Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
			// land_point.Position = player_position;
			// player_position.DistanceTo()
			// land_point_position.Y = -4;
			// land_point.Position = land_point_position;
			land_point.Show();
		}
		else
		{
			// GD.Print("on floor");
			land_point.Hide();
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
