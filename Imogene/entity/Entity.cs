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
	public Collectable gold { get; set; }  = new(Collectable.CollectableType.Gold, 0);

	
	// Base stats
	public Stat level { get; set; } = new(Stat.StatType.Level, 0);
	public Stat strength { get; set; } = new(Stat.StatType.Strength, 0);
	public Stat dexterity { get; set; } = new(Stat.StatType.Dexterity, 0);
	public Stat intellect { get; set; } = new(Stat.StatType.Intellect, 0);
	public Stat vitality { get; set; } = new(Stat.StatType.Vitality, 0);
	public Stat stamina { get; set; } = new(Stat.StatType.Stamina, 0);
	public Stat wisdom { get; set; } = new(Stat.StatType.Wisdom, 0);
	public Stat charisma { get; set; } = new(Stat.StatType.Charisma, 0);
	
	public Stat physical_melee_power { get; set; } = new(Stat.StatType.PhysicalMeleePower, 0);
	public Stat spell_melee_power { get; set; } = new(Stat.StatType.SpellMeleePower, 0);
	public Stat physical_ranged_power { get; set; } = new(Stat.StatType.PhysicalRangedPower, 0);
	public Stat spell_ranged_power { get; set; } = new(Stat.StatType.SpellRangedPower, 0);

	// Wisdom
	public Stat wisdom_scaler { get; set; } = new(Stat.StatType.WisdomScaler, 0);

	// Regeneration
	public Stat health_regeneration { get; set; } = new(Stat.StatType.HealthRegeneration, 0);
	public Stat resource_regeneration { get; set; } = new(Stat.StatType.ResourceRegeneration, 0);
	public Stat posture_regeneration { get; set; } = new(Stat.StatType.PostureRegeneration, 0);

	// Gear Stats
	public Stat main_hand_damage { get; set; } = new(Stat.StatType.MainHandDamage, 10);
	public Stat off_hand_damage { get; set; } = new(Stat.StatType.OffHandDamage, 0);
	public Stat damage_bonus { get; set; } = new(Stat.StatType.DamageBonus, 0);
	public Stat main_hand_attacks_per_second { get; set; } = new(Stat.StatType.MainHandAttacksPerSecond, 0);
	public Stat off_hand_attacks_per_second { get; set; } = new(Stat.StatType.OffHandAttacksPerSecond, 0);
	public Stat attack_speed_increase { get; set; } = new(Stat.StatType.AttackSpeedIncrease, 0);

	// Misc Stats
	public Stat health_bonus { get; set; } = new(Stat.StatType.HealthBonus, 0);
	public Stat movement_speed { get; set; } = new(Stat.StatType.MovementSpeed, 6f);
	public Stat fall_speed { get; set; } = new(Stat.StatType.FallSpeed, 40);
	public Stat jump_speed { get; set; } = new(Stat.StatType.JumpSpeed, 30);

	// Damage these contribute to the multiplier applied to attacks of this type
	public Stat critical_hit_chance { get; set; } = new(Stat.StatType.CriticalHitChance, 0);
	public Stat critical_hit_damage { get; set; } = new(Stat.StatType.CriticalHitDamage, 0);
	public Stat posture_damage { get; set; } = new(Stat.StatType.PostureDamage, 0);
	
	// Damage stats list
	public Stat power { get; set; } = new(Stat.StatType.Power, 0);
	public Stat physical_damage { get; set; } = new(Stat.StatType.PhysicalDamage, 0);
	public Stat pierce_damage { get; set; } = new(Stat.StatType.PierceDamage, 0);
	public Stat slash_damage { get; set; } = new(Stat.StatType.SlashDamage, 0);
	public Stat blunt_damage { get; set; } = new(Stat.StatType.BluntDamage, 0);
	public Stat bleed_damage { get; set; } = new(Stat.StatType.BleedDamage, 0);
	public Stat poison_damage { get; set; } = new(Stat.StatType.PoisonDamage, 0);
	public Stat curse_damage { get; set; } = new(Stat.StatType.CurseDamage, 0);
	public Stat spell_damage { get; set; } = new(Stat.StatType.SpellDamage, 0);
	public Stat fire_damage { get; set; } = new(Stat.StatType.FireDamage, 0);
	public Stat cold_damage { get; set; } = new(Stat.StatType.ColdDamage, 0);
	public Stat lightning_damage { get; set; } = new(Stat.StatType.LightningDamage, 0);
	public Stat holy_damage { get; set; } = new(Stat.StatType.HolyDamage, 0);

	
	// Defensive Stats
	public Stat block_amount { get; set; } = new(Stat.StatType.BlockAmount, 0);
	public Stat retaliation { get; set; } = new(Stat.StatType.Retaliation, 0);
	
	// Resistance Stats
	public Stat armor { get; set; } = new(Stat.StatType.Armor, 0);
	public Stat poise { get; set; } = new(Stat.StatType.Poise, 0);
	public Stat physical_resistance { get; set; } = new(Stat.StatType.PhysicalResistance, 0);
	public Stat pierce_resistance { get; set; } = new(Stat.StatType.PierceResistance, 0);
	public Stat slash_resistance { get; set; } = new(Stat.StatType.SlashResistance, 0);
	public Stat blunt_resistance { get; set; } = new(Stat.StatType.BluntResistance, 0);
	public Stat bleed_resistance { get; set; } = new(Stat.StatType.BleedResistance, 0);
	public Stat poison_resistance { get; set; } = new(Stat.StatType.PoisonResistance, 0);
	public Stat curse_resistance { get; set; } = new(Stat.StatType.CurseResistance, 0);
	public Stat spell_resistance { get; set; } = new(Stat.StatType.SpellResistance, 0);
	public Stat fire_resistance { get; set; } = new(Stat.StatType.FireResistance, 0);
	public Stat cold_resistance { get; set; } = new(Stat.StatType.ColdResistance, 0);
	public Stat lightning_resistance { get; set; } = new(Stat.StatType.LightningResistance, 0);
	public Stat holy_resistance { get; set; } = new(Stat.StatType.HolyResistance, 0);

	// Health
	public Stat health { get; set; } = new(Stat.StatType.Health, 200);
	public Stat health_on_retaliation { get; set; } = new(Stat.StatType.HealthOnRetaliation, 0);
	public Stat health_regeneration_bonus { get; set; } = new(Stat.StatType.HealthRegenerationBonus , 0);

	// Resource
	public Stat resource { get; set; } = new(Stat.StatType.Resource, 100);
	public Stat posture { get; set; } = new(Stat.StatType.Posture, 0);
	public Stat resource_cost_reduction { get; set; } = new(Stat.StatType.ResourceCostReduction, 0);
	public Stat resource_regeneration_bonus { get; set; } = new(Stat.StatType.ResourceRegenerationBonus, 0);
	public Stat cooldown_reduction { get; set; } = new(Stat.StatType.CooldownReduction, 0);

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
        entity_systems.damage_system.Subscribe(this);
		entity_systems.resource_system.Subscribe(this);
		entity_controllers.status_effect_controller.Subscribe(this);
    }

	// public override void _ExitTree()
	// {
	// 	entity_systems.damage_system.unsubscribe(this);
	// 	entity_systems.resource_system.unsubscribe(this);
	// 	entity_controllers.status_effect_controller.Unsubscribe(this);
	// }

}
