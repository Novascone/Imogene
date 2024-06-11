using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Intrinsics.X86;

public partial class PlayerEntity : Entity
{


	// Stats

	public int strength = 0; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	public int dexterity = 0; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	public int intellect = 0; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	public int vitality = 0; // Vitality: Primary stat for health
	public int stamina = 0; // Primary stat for resource and regeneration
	public int wisdom = 0; // Increases the damage of abilities that use wisdom, also used for interactions
	public int charisma = 0; // Primary Stat for character interaction

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

	
	public float physical_melee_power_mod; 
	public float physical_ranged_power_mod; 
	public float spell_melee_power_mod;
	public float spell_ranged_power_mod;
	public float power_mod_avg;

	public int damage_bonus = 0;
	public float combined_damage = 0;
	public float base_aps = 0;
	public float aps_modifiers = 0;
	public float aps = 0;
	public float aps_mod = 0;
	public float base_dps = 0;
	public float skill_mod = 0;
	public float crit_mod = 0;



	public float slash_damage = 0;
	public float thrust_damage = 0;
	public float blunt_damage = 0;
	public float bleed_damage = 0;
	public float poison_damage = 0;
	public float fire_damage = 0;
	public float cold_damage = 0;
	public float lightning_damage = 0;
	public float holy_damage = 0;
	public float critical_hit_chance = 0.5f; // Percentage change for hit to be a critical hit
	public float critical_hit_damage = 0.9f; // Multiplier applied to base damage if a hit is critical
	public float attack_speed_increase = 0;
	public float cool_down_reduction = 0;
	public float posture_damage = 0;

	// Defense

	public int armor = 20;
	public int poise = 0;
	public int block_amount = 0;
	public int retaliation = 0;
	public int physical_resistance = 0;
	public int thrust_resistance = 0;
	public int slash_resistance = 0;
	public int blunt_resistance = 0;
	public int bleed_resistance = 0;
	public int poison_resistance = 0;
	public int curse_resistance = 0;
	public int spell_resistance = 0;
	public int fire_resistance = 0;
	public int cold_resistance = 0;
	public int lightning_resistance = 0;
	public int holy_resistance = 0;

	public float dr_armor;
	public float dr_phys;
	public float dr_slash;
	public float dr_thrust;
	public float dr_blunt;
	public float dr_bleed;
	public float dr_poison;
	public float dr_curse;
	public float dr_spell;
	public float dr_fire;
	public float dr_cold;
	public float dr_lightning;
	public float dr_holy;

	public float dr_lvl_scale;
	public float avg_res_dr;

	public float resistance;

	public float recovery;

	// Health

	public float maximum_health => health;
	public float health_bonus = 0;
	public float health_regen = 0;
	public float health_regen_bonus = 0;
	public float health_on_retaliate = 0;

	// Resources

	public float maximum_resource => resource;
	public float resource_regen = 0;
	public float resource_regen_bonus = 0;
	public float posture_regen = 0;
	public float posture_regen_bonus = 0;
	public float resource_cost_reduction = 0;
	public float rec_lvl_scale;

	// Misc
	public float movement_speed = 0;

	// Materials

	public int maximum_gold => gold;



	public bool max_health_changed = true; // Has the entities heath changed?
	public bool stats_updated = true; // Have the entities stats changed?
	
	// Equipment
	public string resource_path;
	public string weapon_type = "one_handed_axe";
	public Node3D main_node; // Temp helm node
	public Node3D right_node;
	public Node3D left_node;
	public Node3D head_slot; // Head slot for the player
	public MeshInstance3D helm; // Temp helm
	public Node3D shoulder_right_slot; 
	public MeshInstance3D shoulder_right;
	public Node3D shoulder_left_slot;
	public MeshInstance3D shoulder_left;
	public Node3D chest_slot;
	public MeshInstance3D chest;
	public Node3D mark_slot;
	public MeshInstance3D mark;
	public Node3D belt_slot;
	public MeshInstance3D belt; 
	public Node3D glove_right_slot; 
	public MeshInstance3D glove_right; 
	public Node3D glove_left_slot;
	public MeshInstance3D glove_left;
	public Node3D main_hand_slot;
	public MeshInstance3D main_hand;
	public Node3D off_hand_slot;
	public MeshInstance3D off_hand;
	public Node3D leg_right_slot;
	public MeshInstance3D leg_right;
	public Node3D leg_left_slot;
	public MeshInstance3D leg_left;
	public Node3D foot_right_slot;
	public MeshInstance3D foot_right;
	public Node3D foot_left_slot;
	public MeshInstance3D foot_left;
	
	
	
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
	public bool targeting = false; // Is the entity targeting?= 1 - (50 * level / (50 * level + poison_resistance));
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
		
		dr_lvl_scale = 50 * (float)level;
		rec_lvl_scale = 100 * (float)level;
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


		// Calculates stats
		

		physical_melee_power = (2 * strength) + dexterity;
		physical_ranged_power = strength + (3 * dexterity);
		spell_melee_power = strength + dexterity + (3 * intellect);
		spell_ranged_power = (2 * dexterity) + (3 * intellect);

		// physical_melee_damage = physical_melee_power/15;
		// physical_ranged_damage = physical_ranged_power/15;
		// spell_melee_damage = spell_melee_power/15;
		// spell_ranged_damage = spell_ranged_power/15;
		wisdom_scaler = wisdom/20;

		// damage
		combined_damage = weapon_damage + damage_bonus;
		aps = base_aps * (1 + aps_modifiers);
		base_dps = aps * combined_damage;
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

		total_dps = (float)Math.Round(base_dps * power_mod_avg * skill_mod * crit_mod,2);
		// total_dps = (float)Math.Round(total_dps,2);
		damage = total_dps;

		// mitigation
		dr_armor = (float)Math.Round(1 + (armor/dr_lvl_scale), 2);
		dr_phys = (float)Math.Round(1 + (physical_resistance/dr_lvl_scale), 2);
		dr_slash = (float)Math.Round(1 + (slash_resistance/dr_lvl_scale), 2);
		dr_thrust = (float)Math.Round(1 + (thrust_resistance/dr_lvl_scale), 2);
		dr_blunt = (float)Math.Round(1 + (blunt_resistance/dr_lvl_scale), 2);
		dr_bleed = (float)Math.Round(1 + (bleed_resistance/dr_lvl_scale), 2);
		dr_poison = (float)Math.Round(1 + (poison_resistance/dr_lvl_scale), 2);
		dr_spell = (float)Math.Round(1 + (spell_resistance/dr_lvl_scale), 2);
		dr_fire = (float)Math.Round(1 + (fire_resistance/dr_lvl_scale), 2);
		dr_cold = (float)Math.Round(1 + (cold_resistance/dr_lvl_scale), 2);
		dr_lightning = (float)Math.Round(1 + (lightning_resistance/dr_lvl_scale), 2);
		dr_holy = (float)Math.Round(1 + (holy_resistance/dr_lvl_scale), 2);
		
		avg_res_dr = (dr_phys + dr_slash + dr_thrust + dr_blunt + dr_bleed + dr_poison + dr_spell + dr_fire + dr_cold + dr_poison + dr_holy) / 11;

		resistance = (float)Math.Round(maximum_health * (dr_armor * avg_res_dr),2);

		// recovery

		health_regen = (float)Math.Round(1 + stamina/rec_lvl_scale * health_regen_bonus, 2);
		resource_regen = (float)Math.Round(1 + stamina/rec_lvl_scale * resource_regen_bonus, 2);
		posture_regen = (float)Math.Round(1 + stamina/rec_lvl_scale * (1 + poise/100), 2);
		recovery = (float)Math.Round((health_regen + resource_regen + posture_regen) / 3, 2);

		// GD.Print("combined damage " + combined_damage);
		// GD.Print("base dps " + base_dps);
		// GD.Print("aps " + aps);
		// GD.Print("skill mod " + skill_mod);
		// GD.Print("crit mod " + crit_mod);
		// GD.Print("physical melee power mod " + physical_melee_power_mod);
		// GD.Print("spell melee power mod " + spell_melee_power_mod);
		// GD.Print("physical ranged power mod " + physical_ranged_power_mod);
		// GD.Print("spell ranged power mod " + spell_ranged_power_mod);
		// GD.Print("physical melee dps " + physical_melee_dps);
		// GD.Print("spell melee dps " + spell_melee_dps);
		// GD.Print("physical ranged dps " + physical_ranged_dps);
		// GD.Print("spell ranged dps " + spell_ranged_dps);

		// GD.Print("dr_armor " + dr_armor);
		// GD.Print("dr_phys " + dr_phys);
		// GD.Print("dr_slash " + dr_slash);
		// GD.Print("dr_thrust " + dr_thrust);
		// GD.Print("dr_blunt " + dr_blunt);
		// GD.Print("dr_bleed " + dr_bleed);
		// GD.Print("dr_poison " + dr_poison);
		// GD.Print("dr_spell " + dr_spell);
		// GD.Print("dr_fire " + dr_fire);
		// GD.Print("dr_cold " + dr_cold);
		// GD.Print("dr_lightning " + dr_lightning);
		// GD.Print("dr_holy " + dr_holy);

		// GD.Print("avg_res " + avg_res_dr);
		// GD.Print("maximum health " + maximum_health);
		// GD.Print("resistance " + resistance);

		GD.Print("health_regen " + health_regen);
		GD.Print("resource_regen " + resource_regen);
		GD.Print("posture_regen " + posture_regen);
		GD.Print("recovery " + recovery);
		SendStats();
		
	}

	public void SendStats()
	{
		GD.Print("sending stats");
		_customSignals.EmitSignal(nameof(CustomSignals.SendStats),
																level, strength, dexterity, intellect, vitality, stamina, wisdom, charisma, total_dps,

																physical_melee_dps, spell_melee_dps, physical_ranged_dps, spell_ranged_dps, physical_melee_power, 

																spell_melee_power, physical_ranged_power, spell_ranged_power, wisdom_scaler, physical_melee_power_mod,

																physical_ranged_power_mod, spell_ranged_power_mod, power_mod_avg, damage_bonus, combined_damage, base_aps,

																aps_modifiers, aps, aps_mod, base_dps, skill_mod, crit_mod, slash_damage, thrust_damage, blunt_damage, bleed_damage,

																poison_damage, fire_damage, cold_damage, lightning_damage, holy_damage, critical_hit_chance, critical_hit_damage, attack_speed_increase,

																cool_down_reduction, posture_damage, armor, poise, block_amount, retaliation, physical_resistance, thrust_resistance, slash_resistance,

																blunt_resistance, bleed_resistance, poison_resistance, curse_resistance, spell_resistance, fire_resistance, cold_resistance, lightning_resistance,
																
																holy_resistance, maximum_health, health_bonus, health_regen, health_regen_bonus, posture_regen, posture_regen_bonus, health_on_retaliate, resistance, maximum_resource, resource_regen, resource_cost_reduction, recovery, movement_speed, maximum_gold
																);
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

	public void AddEquipableStats(ArmsResource item)
	{
		strength += item.strength;
		dexterity += item.dexterity;
		intellect += item.intellect;
		vitality += item.vitality;
		stamina += item.stamina;
		wisdom += item.wisdom;
		charisma += item.charisma;
		critical_hit_chance += item.critical_hit_chance;
		critical_hit_damage += item.critical_hit_damage;
		armor += item.armor;
		poise += item.poise;	
		block_amount += item.block;
		retaliation += item.retaliation;
		physical_resistance += item.physical_resistance;
		thrust_resistance += item.thrust_resistance;
		slash_resistance += item.slash_resistance;
		blunt_resistance += item.blunt_resistance;
		bleed_resistance += item.bleed_resistance;
		poison_resistance += item.poison_resistance;
		curse_resistance += item.curse_resistance;
		spell_resistance += item.spell_resistance;
		fire_resistance += item.fire_resistance;
		cold_resistance += item.cold_resistance;
		lightning_resistance += item.lightning_resistance;
		holy_resistance += item.holy_resistance;
		health_bonus += item.health_bonus;
		health_regen += item.health_regen;
		health_on_retaliate += item.health_retaliate;
		resource_regen += item.resource_regen;
		resource_cost_reduction += resource_cost_reduction;
	}

	
	
	
}
