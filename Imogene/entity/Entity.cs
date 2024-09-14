using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

// The base class for all entities in game
// All stats are used for players
// Not all enemies utilize all these stats

public partial class Entity : CharacterBody3D
{

	[Export] public string identifier;

	// Hit and hurt boxes
	[Export] public MeleeHitbox main_hand_hitbox;
	[Export] public MeleeHitbox off_hand_hitbox;
	[Export] public Hurtbox hurtbox;

	// Systems
	[Export] public EntitySystems entity_systems;
	// [Export] public DamageSystem damage_system;
	// [Export] public ResourceSystem resource_system;
	
	// Controllers
	[Export] public EntityControllers entity_controllers;


	// Booleans
	public bool can_move = true; // Boolean to keep track of if the entity is allowed to move
    public bool jumping = false;
    public bool using_movement_ability;
    public bool on_floor;
	public bool hale;
	public bool weak;
    public bool attacking; // Boolean to keep track of if the entity is attacking
    public bool animation_triggered;
	public bool targeted;
	public bool dead = false;
	

	// Dot
	public string dot_damage_type;
	public float dot_in_seconds;
	public int dot_duration;
	public bool taking_dot;

	// Posture
	public bool posture_broken;

	// Slow
	public int slow_duration;
	public bool slowed;

	// Stun
	public int stun_duration;
	public bool stunned;

	public Collectable xp = new("xp", 0);
	public Collectable gold = new("gold", 0);

	

	
	public Stat physical_melee_power = new("physical_melee_power", 0);
	public Stat physical_ranged_power = new("physical_ranged_power", 0);
	public Stat spell_melee_power = new("spell_melee_power", 0);
	public Stat spell_ranged_power = new("spell_ranged_power", 0);

	// Wisdom
	public Stat wisdom_scaler = new("wisdom_scaler", 0);

	// Regeneration
	public Stat health_regeneration = new("health_regeneration", 0);
	public Stat resource_regeneration = new("resource_regeneration", 0);
	public Stat posture_regeneration = new("posture_regeneration", 0);

	
	public List<Stat> damage_stats = new List<Stat>();
	public List<Stat> resistance_stats= new List<Stat>();

	// Base stats
	public Stat level = new("level", 0);
	public Stat strength = new("strength", 0);
	public Stat dexterity = new("dexterity", 0);
	public Stat intellect = new("intellect", 0);
	public Stat vitality = new("vitality", 0);
	public Stat stamina = new("stamina", 0);
	public Stat wisdom = new("wisdom", 0);
	public Stat charisma = new("charisma", 0);

	// Gear Stats
	public Stat main_hand_damage = new("main_hand_damage", 10);
	public Stat off_hand_damage = new("off_hand_damage", 0);
	public Stat damage_bonus = new("damage_bonus", 0);
	public Stat attacks_per_second = new("attacks_per_second", 0);

	// Misc Stats
	
	public Stat health_bonus = new("health_bonus", 0);
	public Stat movement_speed = new("movement_speed", 6f);
	public Stat fall_speed = new("fall_speed", 40);
	public Stat jump_speed = new("jump_speed", 30);

	// Damage these contribute to the multiplier applied to attacks of this type
	public Stat critical_hit_chance = new("critical_hit_chance", 0);
	public Stat critical_hit_damage = new("critical_hit_damage", 0);
	public Stat posture_damage = new("posture_damage", 0);
	
	// Damage stats list
	public Stat power = new("power", 0);
	public Stat physical_damage = new("physical_damage", 0);
	public Stat pierce_damage = new("pierce_damage", 0);
	public Stat slash_damage = new("slash_damage", 0);
	public Stat blunt_damage = new("blunt_damage", 0);
	public Stat bleed_damage = new("bleed_damage", 0);
	public Stat poison_damage = new("poison_damage", 0);
	public Stat curse_damage = new("curse_damage", 0);
	public Stat spell_damage = new("spell_damage", 0);
	public Stat fire_damage = new("fire_damage", 0);
	public Stat cold_damage = new("cold_damage", 0);
	public Stat lightning_damage = new("Lightning_damage", 0);
	public Stat holy_damage = new("holy_damage", 0);

	
	// Defensive Stats
	
	
	public Stat block_amount = new("block_amount", 0);
	public Stat retaliation = new("retaliation", 0);
	
	// Resistance Stats
	public Stat armor = new("armor", 0);
	public Stat poise = new("poise", 0);
	public Stat physical_resistance = new("physical_resistance", 0);
	public Stat pierce_resistance = new("pierce_resistance", 0);
	public Stat slash_resistance = new("slash_resistance", 0);
	public Stat blunt_resistance = new("blunt_resistance", 0);
	public Stat bleed_resistance = new("bleed_resistance", 0);
	public Stat poison_resistance = new("poison_resistance", 0);
	public Stat curse_resistance = new("curse_resistance", 0);
	public Stat spell_resistance = new("spell_resistance", 0);
	public Stat fire_resistance = new("fire_resistance", 0);
	public Stat cold_resistance = new("cold_resistance", 0);
	public Stat lightning_resistance = new("lightning_resistance", 0);
	public Stat holy_resistance = new("holy_resistance", 0);

	// Health
	public Stat health = new("health", 200);
	public Stat health_on_retaliation = new("health_on_retaliation", 0);
	public Stat health_regeneration_bonus = new("health_regeneration_bonus" , 0);

	// Resource
	public Stat resource = new("resource", 100);
	public Stat posture = new("posture", 0);
	public Stat resource_cost_reduction = new("resource_cost_reduction", 0);
	public Stat resource_regeneration_bonus = new("resource_regeneration_bonus", 0);


	public float critical_hit_modifier;
	public float combined_damage;

	public List<float> damage_modifiers = new List<float>();
	public float power_modifier;
	public float physical_damage_modifier;
	public float slash_damage_modifier;
	public float pierce_damage_modifier;
	public float blunt_damage_modifier;
	public float bleed_damage_modifier;
	public float poison_damage_modifier;
	public float curse_damage_modifier;
	public float spell_damage_modifier;
	public float fire_damage_modifier;
	public float cold_damage_modifier;
	public float lightning_damage_modifier;
	public float holy_damage_modifier;

	

	// Damage resistance
	public List<float> resistances = new List<float>();
	public List<float> physical_resistances = new List<float>();
	public float damage_resistance_armor;
	public float damage_resistance_poise;
	public float damage_resistance_physical;
	public float damage_resistance_slash;
	public float damage_resistance_pierce;
	public float damage_resistance_blunt;
	public float damage_resistance_bleed;
	public float damage_resistance_poison;
	public float damage_resistance_curse;
	public float damage_resistance_spell;
	public float damage_resistance_fire;
	public float damage_resistance_cold;
	public float damage_resistance_lightning;
	public float damage_resistance_holy;


	
    // Stats
    // public float level = 1; // Level of the entity
	// public float xp = 0;
	// public float xp_to_level = 100;
    // public float speed; // Speed of the entity
	// public float walk_speed = 3.5f;
	// public float run_speed = 6.0f;
    // public float fall_speed = 40.0f; // How fast the player falls 
    // public float jump_speed = 30.0f; // How fast the player jumps
	// public float health = 200; // Prelim health number
    // public float resource = 100; // prelim resource number
	// public float posture;

	// public int strength = 0; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	// public int dexterity = 0; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	// public int intellect = 0; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	// public int vitality = 0; // Vitality: Primary stat for health
	// public int stamina = 0; // Primary stat for resource and regeneration
	// public int wisdom = 0; // Increases the damage of abilities that use wisdom, also used for interactions
	// public int charisma = 0; // Primary Stat for character interaction


	
	// Offense
	// public float total_dps; // Total estimated dps, combination of all 4 types of attacks
	// public float physical_melee_dps;
	// public float spell_melee_dps;
	// public float physical_ranged_dps;
	// public float spell_ranged_dps;
    // public float main_hand_damage;
	// public float off_hand_damage;

	

	



	// public float physical_melee_power; // Increases physical melee DPS by a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	// public float spell_melee_power; // Increases melee magic DPS a magnitude every 100 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
	// public float physical_ranged_power; // Increases physical ranged DPS a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	// public float spell_ranged_power;  // Increases physical ranged DPS by a magnitude every 100 points + 3 for every point of dexterity + 1 for every point of strength
	// public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	
	// public float physical_melee_power_mod; // 1 + (power/100)
	// public float physical_ranged_power_mod; 
	// public float spell_melee_power_mod;
	// public float spell_ranged_power_mod;
	// public float power_mod_avg;

    
    // public float attack_speed;
	// public float damage_bonus = 0; // Damage bonus from gear
	// public float combined_damage = 0; // Main hand, off-hand, and all damage from equipment
	// public float base_aps = 0; // Base attacks per second of the entity, determined by weapons/ stance for players/ entities using weapons
	// public float aps_modifiers = 0; // Modifiers to attacks per second, from skills or gear
	// public float aps = 0; // Attacks per second
	// public float base_dps = 0; // Base dps of the entity aps * combined damage
	// public float skill_mod = 0; // Damage modification granted from skills or equipment
	// public float crit_mod = 0; // 1 + (crit chance * crit damage)



	// public float slash_damage = 0; // Percentage of damage given by slash
	// public float pierce_damage = 0; // Percentage of damage given by thrust
	// public float blunt_damage = 0; // Percentage of damage given by blunt
	// public float bleed_damage = 0; // Percentage of damage given by bleed
	// public float poison_damage = 0; // Percentage of damage given by poison
	// public float fire_damage = 0; // Percentage of damage given by fire
	// public float cold_damage = 0; // Percentage of damage given by cold
	// public float lightning_damage = 0; // Percentage of damage given by lightning
	// public float holy_damage = 0; // Percentage of damage given by holy
	// public float critical_hit_chance = 0.5f; // Percentage change for hit to be a critical hit
	// public float critical_hit_damage = 0.9f; // Multiplier applied to base damage if a hit is critical
	// public float attack_speed_increase = 0; // aps modifiers as a percentage to be displayed
	// public float cooldown_reduction = 0; // The percent reduction of cooldown
	// public float posture_damage = 34; // How much damage each attack does to opponent posture **** this needs more defining as does the posture system ****
    // public float damage; // How much damage the entity does

	// Defense
	// public float armor = 20; // Total armor the entity has increases armor damage reduction 
	// public float poise = 0; // Decreases how fast posture depletes **** this needs more defining as does the posture system ****
	// public float block_amount = 0; // How much damage the entity can block when blocking
 	// public float retaliation = 0; // Increases retaliation period
	// public float physical_resistance = 0; // Increases physical damage resistance
	// public float pierce_resistance = 0; // Increases thrust damage resistance
	// public float slash_resistance = 0; // Increases slash damage resistance
	// public float blunt_resistance = 0; // Increases blunt damage resistance
	// public float bleed_resistance = 0; // Increases bleed damage resistance
	// public float poison_resistance = 0; // Increases poison damage resistance
	// public float curse_resistance = 0; // Increases curse resistance
	// public float spell_resistance = 0; // Increases spell damage resistance
	// public float fire_resistance = 0; // Increases fire damage resistance
	// public float cold_resistance = 0; // Increases cold damage resistance
	// public float lightning_resistance = 0; // Increases lightning damage resistance
	// public float holy_resistance = 0; // Increases holy damage resistance

	

	// public float dr_armor; // 1 + (armor / (dr_level_scale)) repeat for all resistances
	// public float dr_phys;
	// public float dr_slash;
	// public float dr_thrust;
	// public float dr_blunt;
	// public float dr_bleed;
	// public float dr_poison;
	// public float dr_curse;
	// public float dr_spell;
	// public float dr_fire;
	// public float dr_cold;
	// public float dr_lightning;
	// public float dr_holy;

	// public float dr_lvl_scale; // level * 50 <-- set in player entity / on enemy script
	// public float avg_res_dr; // Average of all resistances

	// public float resistance; // Estimated total resistance stat



	// Health

	// public float maximum_health;
	// public float health_bonus = 0; // Health bonus from skills and gear
	// public float health_regen = 0; // Health regenerated every second 1 + (stamina/rec_lvl_scale * health_regen_bonus)
	// public float health_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	// public float health_on_retaliate = 0; // How much health is gain from a successful attack during retaliation period

	// Resource

	// public float maximum_resource = 100;
	// public float resource_regen = 0; // Resource regenerated every second 1 + (stamina/rec_lvl_scale *  resource_regen_bonus)
	// public float resource_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	// public float maximum_posture = 100;
	// public float posture_regen = 0; // Posture regenerated every second 1 + (stamina/rec_lvl_scale * (1 + poise/100)
	// public float posture_regen_bonus = 0; // Bonus to posture regen from skills and gear
	// public float resource_cost_reduction = 0; // Resource cost reduction from skills and gear
	// public float rec_lvl_scale; // level * 100 <-- set in player entity

	// public float recovery; // Estimated total recovery stat, average of the three types of recovery

	// Misc
	// public float movement_speed = 0; // How fast the entity moves

	// Materials

	// public float maximum_gold; // How much gold the entity can carry


    public Vector3 direction; // Direction 
	public Vector3 position; // Position 
	public Vector3 velocity; // Velocity 
	public float prev_y_rotation; // Rotation before current rotation
	public float current_y_rotation; // Current rotation

	public string weapon_type;


    // Possessions
    // public int gold = 0;

	// Status effects
	public List<StatusEffect> status_effects = new List<StatusEffect>();


    // public Vector3 enemy_position;
	

    public override void _Ready()
    {
		
	
		// public float damage_resistance_poise;
		// public float damage_resistance_physical;
		// public float damage_resistance_slash;
		// public float damage_resistance_pierce;
		// public float damage_resistance_blunt;
		// public float damage_resistance_bleed;
		// public float damage_resistance_poison;
		// public float damage_resistance_curse;
		// public float damage_resistance_spell;
		// public float damage_resistance_fire;
		// public float damage_resistance_cold;
		// public float damage_resistance_lightning;
		// public float damage_resistance_holy;
		
        entity_systems.damage_system.SubscribeEntityToHealthRegen(this);
    }

   

    private void OnCastTickTimeout()
    {
        throw new NotImplementedException();
    }

    
    // public bool Fall(double delta) // bring the player back to the ground
	// {
	// 	if(!IsOnFloor())
	// 	{
	// 		if(this is Player player)
	// 		{
	// 			if(!player.is_climbing)
	// 			{
	// 				velocity.Y -= fall_speed * (float)delta;
    //         		return true;
	// 			}
	// 			else
	// 			{
	// 				return false;
	// 			}
	// 		}
	// 		else
	// 		{
	// 			velocity.Y -= fall_speed * (float)delta;
    //         	return true;
	// 		}
			
	// 	}
	// 	else
	// 	{
    //         return false;
	// 	}
		
	// }
}
