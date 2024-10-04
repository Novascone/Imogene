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


	

	[Export] public string identifier { get; set; }
	[Export] public Node3D armature { get; set; }

	// Hit and hurt boxes
	[Export] public MeleeHitbox main_hand_hitbox { get; set; }
	[Export] public MeleeHitbox off_hand_hitbox { get; set; }
	[Export] public Hurtbox hurtbox { get; set; }

	// Systems
	[Export] public EntitySystems entity_systems { get; set; }

	// Controllers
	[Export] public EntityControllers entity_controllers { get; set; }
	
	// Collectables
	public Collectable xp  { get; set; } = new(Collectable.CollectableType.XP, 0);
	public Collectable gold { get; set; }  = new(Collectable.CollectableType.GOLD, 0);

	
	// Base stats
	public Stat level { get; set; } = new(Stat.StatType.level, 0);
	public Stat strength { get; set; } = new(Stat.StatType.strength, 0);
	public Stat dexterity { get; set; } = new(Stat.StatType.dexterity, 0);
	public Stat intellect { get; set; } = new(Stat.StatType.intellect, 0);
	public Stat vitality { get; set; } = new(Stat.StatType.vitality, 0);
	public Stat stamina { get; set; } = new(Stat.StatType.stamina, 0);
	public Stat wisdom { get; set; } = new(Stat.StatType.wisdom, 0);
	public Stat charisma { get; set; } = new(Stat.StatType.charisma, 0);
	
	public Stat physical_melee_power { get; set; } = new(Stat.StatType.physical_melee_power, 0);
	public Stat spell_melee_power { get; set; } = new(Stat.StatType.spell_melee_power, 0);
	public Stat physical_ranged_power { get; set; } = new(Stat.StatType.physical_ranged_power, 0);
	public Stat spell_ranged_power { get; set; } = new(Stat.StatType.spell_melee_power, 0);

	// Wisdom
	public Stat wisdom_scaler { get; set; } = new(Stat.StatType.wisdom_scaler, 0);

	// Regeneration
	public Stat health_regeneration { get; set; } = new(Stat.StatType.health_regeneration, 0);
	public Stat resource_regeneration { get; set; } = new(Stat.StatType.resource_regeneration, 0);
	public Stat posture_regeneration { get; set; } = new(Stat.StatType.posture_regeneration, 0);

	// Gear Stats
	public Stat main_hand_damage { get; set; } = new(Stat.StatType.main_hand_damage, 10);
	public Stat off_hand_damage { get; set; } = new(Stat.StatType.off_hand_damage, 0);
	public Stat damage_bonus { get; set; } = new(Stat.StatType.damage_bonus, 0);
	public Stat main_hand_attacks_per_second { get; set; } = new(Stat.StatType.main_hand_attacks_per_second, 0);
	public Stat off_hand_attacks_per_second { get; set; } = new(Stat.StatType.off_hand_attacks_per_second, 0);
	public Stat attack_speed_increase { get; set; } = new(Stat.StatType.attack_speed_increase, 0);

	// Misc Stats
	public Stat health_bonus { get; set; } = new(Stat.StatType.health_bonus, 0);
	public Stat movement_speed { get; set; } = new(Stat.StatType.movement_speed, 6f);
	public Stat fall_speed { get; set; } = new(Stat.StatType.fall_speed, 40);
	public Stat jump_speed { get; set; } = new(Stat.StatType.jump_speed, 30);

	// Damage these contribute to the multiplier applied to attacks of this type
	public Stat critical_hit_chance { get; set; } = new(Stat.StatType.critical_hit_chance, 0);
	public Stat critical_hit_damage { get; set; } = new(Stat.StatType.critical_hit_damage, 0);
	public Stat posture_damage { get; set; } = new(Stat.StatType.posture_damage, 0);
	
	// Damage stats list
	public Stat power { get; set; } = new(Stat.StatType.power, 0);
	public Stat physical_damage { get; set; } = new(Stat.StatType.physical_damage, 0);
	public Stat pierce_damage { get; set; } = new(Stat.StatType.pierce_damage, 0);
	public Stat slash_damage { get; set; } = new(Stat.StatType.slash_damage, 0);
	public Stat blunt_damage { get; set; } = new(Stat.StatType.blunt_damage, 0);
	public Stat bleed_damage { get; set; } = new(Stat.StatType.bleed_damage, 0);
	public Stat poison_damage { get; set; } = new(Stat.StatType.poison_damage, 0);
	public Stat curse_damage { get; set; } = new(Stat.StatType.curse_damage, 0);
	public Stat spell_damage { get; set; } = new(Stat.StatType.spell_damage, 0);
	public Stat fire_damage { get; set; } = new(Stat.StatType.fire_damage, 0);
	public Stat cold_damage { get; set; } = new(Stat.StatType.cold_damage, 0);
	public Stat lightning_damage { get; set; } = new(Stat.StatType.lightning_damage, 0);
	public Stat holy_damage { get; set; } = new(Stat.StatType.holy_damage, 0);

	
	// Defensive Stats
	public Stat block_amount { get; set; } = new(Stat.StatType.block_amount, 0);
	public Stat retaliation { get; set; } = new(Stat.StatType.retaliation, 0);
	
	// Resistance Stats
	public Stat armor { get; set; } = new(Stat.StatType.armor, 0);
	public Stat poise { get; set; } = new(Stat.StatType.poise, 0);
	public Stat physical_resistance { get; set; } = new(Stat.StatType.physical_resistance, 0);
	public Stat pierce_resistance { get; set; } = new(Stat.StatType.pierce_resistance, 0);
	public Stat slash_resistance { get; set; } = new(Stat.StatType.slash_resistance, 0);
	public Stat blunt_resistance { get; set; } = new(Stat.StatType.blunt_resistance, 0);
	public Stat bleed_resistance { get; set; } = new(Stat.StatType.bleed_resistance, 0);
	public Stat poison_resistance { get; set; } = new(Stat.StatType.poison_resistance, 0);
	public Stat curse_resistance { get; set; } = new(Stat.StatType.curse_resistance, 0);
	public Stat spell_resistance { get; set; } = new(Stat.StatType.spell_resistance, 0);
	public Stat fire_resistance { get; set; } = new(Stat.StatType.fire_resistance, 0);
	public Stat cold_resistance { get; set; } = new(Stat.StatType.cold_resistance, 0);
	public Stat lightning_resistance { get; set; } = new(Stat.StatType.lightning_resistance, 0);
	public Stat holy_resistance { get; set; } = new(Stat.StatType.holy_resistance, 0);

	// Health
	public Stat health { get; set; } = new(Stat.StatType.health, 200);
	public Stat health_on_retaliation { get; set; } = new(Stat.StatType.health_on_retaliation, 0);
	public Stat health_regeneration_bonus { get; set; } = new(Stat.StatType.health_regeneration_bonus , 0);

	// Resource
	public Stat resource { get; set; } = new(Stat.StatType.resource, 100);
	public Stat posture { get; set; } = new(Stat.StatType.posture, 0);
	public Stat resource_cost_reduction { get; set; } = new(Stat.StatType.resource_cost_reduction, 0);
	public Stat resource_regeneration_bonus { get; set; } = new(Stat.StatType.resource_regeneration_bonus, 0);
	public Stat cooldown_reduction { get; set; } = new(Stat.StatType.cooldown_reduction, 0);

	public float combined_damage { get; set; } = 0.0f;

	public float previous_y_rotation { get; set; } = 0.0f; // Rotation before current rotation
	public float current_y_rotation { get; set; } = 0.0f; // Current rotation
	public float previous_x_rotation { get; set; } = 0.0f; // Rotation before current rotation
	public float current_x_rotation { get; set; } = 0.0f; // Current rotation
	public float previous_z_rotation { get; set; } = 0.0f; // Rotation before current rotation
	public float current_z_rotation { get; set; } = 0.0f; // Current rotation

	public List<StatusEffect> status_effects { get; set; } = new List<StatusEffect>();

	public Vector3 _direction = Vector3.Zero; // Direction 
	public Vector3 _position  = Vector3.Zero; // Position 
	public Vector3 _velocity  = Vector3.Zero; // Velocity 

	
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

}
