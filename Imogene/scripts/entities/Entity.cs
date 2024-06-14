using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

// The base class for all entities in game
// All stats are used for players
// Not all enemies utilize all these stats

public partial class Entity : CharacterBody3D
{
    // Stats

    public int level = 1; // Level of the entity
    public float speed = 7.0f; // Speed of the entity
    public float fall_speed = 40.0f; // How fast the player falls 
    public float jump_speed = 30.0f; // How fast the player jumps
	public Dictionary<string, float> damage_types_to_deal = new Dictionary<string, float>(9);
	public Dictionary<string, float> damage_by_type = new Dictionary<string, float>(7);
	public float health = 200; // Prelim health number
    public int resource = 100; // prelim resource number

	public int strength = 0; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	public int dexterity = 0; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	public int intellect = 0; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	public int vitality = 0; // Vitality: Primary stat for health
	public int stamina = 0; // Primary stat for resource and regeneration
	public int wisdom = 0; // Increases the damage of abilities that use wisdom, also used for interactions
	public int charisma = 0; // Primary Stat for character interaction

	// Offense

	
	public float total_dps; // Total estimated dps, combination of all 4 types of attacks
	public float physical_melee_dps;
	public float spell_melee_dps;
	public float physical_ranged_dps;
	public float spell_ranged_dps;
    public int main_hand_damage;
	public float off_hand_damage;

	public float physical_melee_power; // Increases physical melee DPS by a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_melee_power; // Increases melee magic DPS a magnitude every 100 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
	public float physical_ranged_power; // Increases physical ranged DPS a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_ranged_power;  // Increases physical ranged DPS by a magnitude every 100 points + 3 for every point of dexterity + 1 for every point of strength
	public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	
	public float physical_melee_power_mod; // 1 + (power/100)
	public float physical_ranged_power_mod; 
	public float spell_melee_power_mod;
	public float spell_ranged_power_mod;
	public float power_mod_avg;

    
    public float attack_speed;
	public int damage_bonus = 0; // Damage bonus from gear
	public float combined_damage = 0; // Main hand, off-hand, and all damage from equipment
	public float base_aps = 0; // Base attacks per second of the entity, determined by weapons/ stance for players/ entities using weapons
	public float aps_modifiers = 0; // Modifiers to attacks per second, from skills or gear
	public float aps = 0; // Attacks per second
	public float base_dps = 0; // Base dps of the entity aps * combined damage
	public float skill_mod = 0; // Damage modification granted from skills or equipment
	public float crit_mod = 0; // 1 + (crit chance * crit damage)



	public float slash_damage = 0; // Percentage of damage given by slash
	public float thrust_damage = 0; // Percentage of damage given by thrust
	public float blunt_damage = 0; // Percentage of damage given by blunt
	public float bleed_damage = 0; // Percentage of damage given by bleed
	public float poison_damage = 0; // Percentage of damage given by poison
	public float fire_damage = 0; // Percentage of damage given by fire
	public float cold_damage = 0; // Percentage of damage given by cold
	public float lightning_damage = 0; // Percentage of damage given by lightning
	public float holy_damage = 0; // Percentage of damage given by holy
	public float critical_hit_chance = 0.5f; // Percentage change for hit to be a critical hit
	public float critical_hit_damage = 0.9f; // Multiplier applied to base damage if a hit is critical
	public float attack_speed_increase = 0; // aps modifiers as a percentage to be displayed
	public float cool_down_reduction = 0; // The percent reduction of cooldown
	public float posture_damage = 0; // How much damage each attack does to opponent posture **** this needs more defining as does the posture system ****
    public float damage; // How much damage the entity does

	// Defense

	public int armor = 20; // Total armor the entity has increases armor damage reduction 
	public int poise = 0; // Decreases how fast posture depletes **** this needs more defining as does the posture system ****
	public int block_amount = 0; // How much damage the entity can block when blocking
 	public int retaliation = 0; // Increases retaliation period
	public int physical_resistance = 0; // Increases physical damage resistance
	public int thrust_resistance = 0; // Increases thrust damage resistance
	public int slash_resistance = 0; // Increases slash damage resistance
	public int blunt_resistance = 0; // Increases blunt damage resistance
	public int bleed_resistance = 0; // Increases bleed damage resistance
	public int poison_resistance = 0; // Increases poison damage resistance
	public int curse_resistance = 0; // Increases curse resistance
	public int spell_resistance = 0; // Increases spell damage resistance
	public int fire_resistance = 0; // Increases fire damage resistance
	public int cold_resistance = 0; // Increases cold damage resistance
	public int lightning_resistance = 0; // Increases lightning damage resistance
	public int holy_resistance = 0; // Increases holy damage resistance

	public float dr_armor; // 1 + (armor / (dr_level_scale)) repeat for all resistances
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

	public float dr_lvl_scale; // level * 50 <-- set in player entity
	public float avg_res_dr; // Average of all resistances

	public float resistance; // Estimated total resistance stat



	// Health

	public float maximum_health => health;
	public float health_bonus = 0; // Health bonus from skills and gear
	public float health_regen = 0; // Health regenerated every second 1 + (stamina/rec_lvl_scale * health_regen_bonus)
	public float health_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	public float health_on_retaliate = 0; // How much health is gain from a successful attack during retaliation period

	// Resource

	public float maximum_resource => resource;
	public float resource_regen = 0; // Resource regenerated every second 1 + (stamina/rec_lvl_scale *  resource_regen_bonus)
	public float resource_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	public float posture_regen = 0; // Posture regenerated every second 1 + (stamina/rec_lvl_scale * (1 + poise/100)
	public float posture_regen_bonus = 0; // Bonus to posture regen from skills and gear
	public float resource_cost_reduction = 0; // Resource cost reduction from skills and gear
	public float rec_lvl_scale; // level * 100 <-- set in player entity

	public float recovery; // Estimated total recovery stat, average of the three types of recovery

	// Misc
	public float movement_speed = 0; // How fast the entity moves

	// Materials

	public int maximum_gold => gold; // How much gold the entity can carry


    public Vector3 direction; // Direction 
	public Vector3 position; // Position 
	public Vector3 velocity; // Velocity 
	public float prev_y_rotation; // Rotation before current rotation
	public float current_y_rotation; // Current rotation


    // Possessions
    public int gold = 0;

    public bool can_move = true; // Boolean to keep track of if the entity is allowed to move
    public bool jumping = false;
    public bool using_movement_ability;
    public bool on_floor;
    public bool attacking; // Boolean to keep track of if the entity is attacking
    public bool animation_triggered;
    public bool dead = false;
    public Vector3 enemy_position;

	float incoming_physical = 0;
	float incoming_bleed = 0;
	float incoming_poison = 0;
	float incoming_fire = 0;
	float incoming_cold = 0;
	float incoming_lightning = 0;
	float incoming_holy = 0;

	float outgoing_physical = 0;
	float outgoing_bleed = 0;
	float outgoing_poison = 0;
	float outgoing_fire = 0;
	float outgoing_cold = 0;
	float outgoing_lightning = 0;
	float outgoing_holy = 0;

 
    public void TakeDamage(float amount) // Applies damage to an entity
    {
		
        if(health - amount > 0)
        {
            health -= amount;
        }
        else
        {
            dead = true;
            GD.Print("dead");
        }
    }

	public void DealDamageUpdate()
	{
		CalculateDamageTypes();
		foreach(KeyValuePair<string, float> type in damage_by_type)
		{
			GD.Print(type.Key + " " + type.Value);
			damage_by_type[type.Key] = 0;
		}
		foreach(KeyValuePair<string, float> type in damage_types_to_deal)
		{
			damage_types_to_deal[type.Key] = 0;
		}
		ResetDamageTypes();

	}

	public void TakeDamageUpdate(Dictionary<string, float> types) // Applies damage to an entity
    {
		
		foreach(KeyValuePair<string, float> type in types)
		{
			if(type.Value > 0.0)
			{
				if(type.Key == "physical")
				{
					incoming_physical = type.Value;
					incoming_physical *= 1 - dr_armor;
					incoming_physical *= 1 - dr_phys;
				}
				if(type.Key == "bleed")
				{
					incoming_bleed = type.Value;
					incoming_bleed *= 1 - dr_armor;
					incoming_bleed *= 1 - dr_bleed;
				}
				if(type.Key == "poison")
				{
					incoming_poison = type.Value;
					incoming_poison *= 1 - dr_armor;
					incoming_poison *= 1 - dr_poison;
				}
				if(type.Key == "fire")
				{
					incoming_fire = type.Value;
					incoming_fire *= 1 - dr_armor;
					incoming_fire *= 1 - dr_fire;
				}
				if(type.Key == "cold")
				{
					incoming_cold = type.Value;
					incoming_cold *= 1 - dr_armor;
					incoming_cold *= 1 - dr_cold;
				}
				if(type.Key == "lightning")
				{
					incoming_lightning = type.Value;
					incoming_lightning *= 1 - dr_armor;
					incoming_lightning *= 1 - dr_lightning;
				}
				if(type.Key == "holy")
				{
					incoming_holy = type.Value;
					incoming_holy *= 1 - dr_armor;
					incoming_holy *= 1 - dr_holy;
				}

			}
		}


		float damage_to_take = incoming_physical + incoming_bleed + incoming_poison + incoming_fire + incoming_cold + incoming_lightning + incoming_holy;

        if(health - damage_to_take > 0)
        {
            health -= damage_to_take;
        }
        else
        {
            dead = true;
            GD.Print("dead");
        }
    }

	public void CalculateDamageTypes()
	{
		
		foreach(KeyValuePair<string, float> type in damage_types_to_deal)
		{
			if(type.Key == "slash")
			{
				outgoing_physical += damage * type.Value;
			}
			if(type.Key == "thrust")
			{
				outgoing_physical += damage * type.Value;
			}
			if(type.Key == "blunt")
			{
				outgoing_physical += damage * type.Value;
			}
			if(type.Key == "bleed")
			{
				outgoing_bleed = damage * type.Value;
			}
			if(type.Key == "poison")
			{
				outgoing_poison = damage * type.Value;
			}
			if(type.Key == "fire")
			{
				outgoing_fire = damage * type.Value;
			}
			if(type.Key == "cold")
			{
				outgoing_cold = damage * type.Value;
			}
			if(type.Key == "lightning")
			{
				outgoing_lightning = damage * type.Value;
			}
			if(type.Key == "holy")
			{
				outgoing_holy = damage * type.Value;
			}
			
		}
		
		damage_by_type["physical"] = outgoing_physical;
		damage_by_type["bleed"] = outgoing_bleed;
		damage_by_type["poison"] = outgoing_poison;
		damage_by_type["fire"] = outgoing_fire;
		damage_by_type["cold"] = outgoing_cold;
		damage_by_type["lightning"] = outgoing_lightning;
		damage_by_type["holy"] = outgoing_holy;
	}

	public void SetDamageTypesToDeal()
	{
		damage_types_to_deal["slash"] = slash_damage;
		damage_types_to_deal["thrust"] = thrust_damage;
		damage_types_to_deal["blunt"] = blunt_damage;
		damage_types_to_deal["bleed"] = bleed_damage;
		damage_types_to_deal["poison"] = poison_damage;
		damage_types_to_deal["fire"] = fire_damage;
		damage_types_to_deal["cold"] = cold_damage;
		damage_types_to_deal["lightning"] = lightning_damage;

		GD.Print(damage_types_to_deal["slash"]);
	}

	public void ResetDamageTypes()
	{
		slash_damage = 0;
		thrust_damage = 0;
		blunt_damage = 0;
		bleed_damage = 0;
		poison_damage = 0;
		fire_damage = 0;
		cold_damage = 0;
		lightning_damage = 0;

		outgoing_physical = 0;
	}


    public  Node LoadAbility(String name) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://scripts/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }

    public bool Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
			// land_point.Position = player_position;
			// player_position.DistanceTo()
			// land_point_position.Y = -4;
			// land_point.Position = land_point_position;
            return true;
		}
		else
		{
			// GD.Print("on floor");
            return false;
		}
		
	}
}
