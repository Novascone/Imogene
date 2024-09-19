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
    public bool attacking; // Boolean to keep track of if the entity is attacking
    public bool animation_triggered;
	public bool targeted;
	public bool dead = false;
	

	public Collectable xp = new("xp", 0);
	public Collectable gold = new("gold", 0);

	

	
	public Stat physical_melee_power = new(Stat.StatType.physical_melee_power, 0);
	public Stat physical_ranged_power = new(Stat.StatType.physical_ranged_power, 0);
	public Stat spell_melee_power = new(Stat.StatType.spell_melee_power, 0);
	public Stat spell_ranged_power = new(Stat.StatType.spell_melee_power, 0);

	// Wisdom
	public Stat wisdom_scaler = new(Stat.StatType.wisdom_scaler, 0);

	// Regeneration
	public Stat health_regeneration = new(Stat.StatType.health_regeneration, 0);
	public Stat resource_regeneration = new(Stat.StatType.resource_regeneration, 0);
	public Stat posture_regeneration = new(Stat.StatType.posture_regeneration, 0);


	// Base stats
	public Stat level = new(Stat.StatType.level, 0);
	public Stat strength = new(Stat.StatType.strength, 0);
	public Stat dexterity = new(Stat.StatType.dexterity, 0);
	public Stat intellect = new(Stat.StatType.intellect, 0);
	public Stat vitality = new(Stat.StatType.vitality, 0);
	public Stat stamina = new(Stat.StatType.stamina, 0);
	public Stat wisdom = new(Stat.StatType.wisdom, 0);
	public Stat charisma = new(Stat.StatType.charisma, 0);

	// Gear Stats
	public Stat main_hand_damage = new(Stat.StatType.main_hand_damage, 10);
	public Stat off_hand_damage = new(Stat.StatType.off_hand_damage, 0);
	public Stat damage_bonus = new(Stat.StatType.damage_bonus, 0);
	public Stat main_hand_attacks_per_second = new(Stat.StatType.main_hand_attacks_per_second, 0);
	public Stat off_hand_attacks_per_second = new(Stat.StatType.off_hand_attacks_per_second, 0);
	public Stat attack_speed_increase = new(Stat.StatType.attack_speed_increase, 0);

	// Misc Stats
	
	public Stat health_bonus = new(Stat.StatType.health_bonus, 0);
	public Stat movement_speed = new(Stat.StatType.movement_speed, 6f);
	public Stat fall_speed = new(Stat.StatType.fall_speed, 40);
	public Stat jump_speed = new(Stat.StatType.jump_speed, 30);

	// Damage these contribute to the multiplier applied to attacks of this type
	public Stat critical_hit_chance = new(Stat.StatType.critical_hit_chance, 0);
	public Stat critical_hit_damage = new(Stat.StatType.critical_hit_damage, 0);
	public Stat posture_damage = new(Stat.StatType.posture_damage, 0);
	
	// Damage stats list
	public Stat power = new(Stat.StatType.power, 0);
	public Stat physical_damage = new(Stat.StatType.physical_damage, 0);
	public Stat pierce_damage = new(Stat.StatType.pierce_damage, 0);
	public Stat slash_damage = new(Stat.StatType.slash_damage, 0);
	public Stat blunt_damage = new(Stat.StatType.blunt_damage, 0);
	public Stat bleed_damage = new(Stat.StatType.bleed_damage, 0);
	public Stat poison_damage = new(Stat.StatType.poison_damage, 0);
	public Stat curse_damage = new(Stat.StatType.curse_damage, 0);
	public Stat spell_damage = new(Stat.StatType.spell_damage, 0);
	public Stat fire_damage = new(Stat.StatType.fire_damage, 0);
	public Stat cold_damage = new(Stat.StatType.cold_damage, 0);
	public Stat lightning_damage = new(Stat.StatType.lightning_damage, 0);
	public Stat holy_damage = new(Stat.StatType.holy_damage, 0);

	
	// Defensive Stats
	
	
	public Stat block_amount = new(Stat.StatType.block_amount, 0);
	public Stat retaliation = new(Stat.StatType.retaliation, 0);
	
	// Resistance Stats
	public Stat armor = new(Stat.StatType.armor, 0);
	public Stat poise = new(Stat.StatType.poise, 0);
	public Stat physical_resistance = new(Stat.StatType.physical_resistance, 0);
	public Stat pierce_resistance = new(Stat.StatType.pierce_resistance, 0);
	public Stat slash_resistance = new(Stat.StatType.slash_resistance, 0);
	public Stat blunt_resistance = new(Stat.StatType.blunt_resistance, 0);
	public Stat bleed_resistance = new(Stat.StatType.bleed_resistance, 0);
	public Stat poison_resistance = new(Stat.StatType.poison_resistance, 0);
	public Stat curse_resistance = new(Stat.StatType.curse_resistance, 0);
	public Stat spell_resistance = new(Stat.StatType.spell_resistance, 0);
	public Stat fire_resistance = new(Stat.StatType.fire_resistance, 0);
	public Stat cold_resistance = new(Stat.StatType.cold_resistance, 0);
	public Stat lightning_resistance = new(Stat.StatType.lightning_resistance, 0);
	public Stat holy_resistance = new(Stat.StatType.holy_resistance, 0);

	// Health
	public Stat health = new(Stat.StatType.health, 200);
	public Stat health_on_retaliation = new(Stat.StatType.health_on_retaliation, 0);
	public Stat health_regeneration_bonus = new(Stat.StatType.health_regeneration_bonus , 0);

	// Resource
	public Stat resource = new(Stat.StatType.resource, 100);
	public Stat posture = new(Stat.StatType.posture, 0);
	public Stat resource_cost_reduction = new(Stat.StatType.resource_cost_reduction, 0);
	public Stat resource_regeneration_bonus = new(Stat.StatType.resource_regeneration_bonus, 0);
	public Stat cooldown_reduction = new(Stat.StatType.cooldown_reduction, 0);


	public float critical_hit_modifier;
	public float combined_damage;

	
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


	
 
    public Vector3 direction; // Direction 
	public Vector3 position; // Position 
	public Vector3 velocity; // Velocity 
	public float prev_y_rotation; // Rotation before current rotation
	public float current_y_rotation; // Current rotation

	public string weapon_type;

	public int fear_duration = 5;


    // Possessions
    // public int gold = 0;

	// Status effects
	public List<StatusEffect> status_effects = new List<StatusEffect>();


    // public Vector3 enemy_position;
	
	public List<Stat> stats = new List<Stat>();
    public override void _Ready()
    {
		health.max_value = 200;
		health.base_value = health.max_value;
		health.current_value = health.max_value;

		resource.max_value = 200;
		resource.base_value = resource.max_value;
		resource.current_value = resource.max_value/2;

		resource_regeneration.base_value = 1;
		resource_regeneration.current_value = 1;

		posture.max_value = 100;
		posture.current_value = 0;

		posture_regeneration.base_value = 2;
		posture_regeneration.current_value = 2;

		posture_damage.current_value = 10;
        entity_systems.damage_system.SubscribeEntityToHealthRegen(this);
    }

   

    private void OnCastTickTimeout()
    {
        throw new NotImplementedException();
    }
}
