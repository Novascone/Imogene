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


	

	[Export] public string Identifier { get; set; }
	[Export] public Node3D Armature { get; set; }

	// Hit and hurt boxes
	[Export] public MeleeHitbox MainHandHitbox { get; set; }
	[Export] public MeleeHitbox OffHandHitbox { get; set; }
	[Export] public Hurtbox Hurtbox { get; set; }

	// Systems
	[Export] public EntitySystems EntitySystems { get; set; }

	// Controllers
	[Export] public EntityControllers EntityControllers { get; set; }
	
	// Collectables
	public Collectable XP  { get; set; } = new(Collectable.CollectableType.XP, 0);
	public Collectable Gold { get; set; }  = new(Collectable.CollectableType.Gold, 0);

	
	// Base stats
	public Stat Level { get; set; } = new(Stat.StatType.Level, 0);
	public Stat Strength { get; set; } = new(Stat.StatType.Strength, 0);
	public Stat Dexterity { get; set; } = new(Stat.StatType.Dexterity, 0);
	public Stat Intellect { get; set; } = new(Stat.StatType.Intellect, 0);
	public Stat Vitality { get; set; } = new(Stat.StatType.Vitality, 0);
	public Stat Stamina { get; set; } = new(Stat.StatType.Stamina, 0);
	
	// Regeneration
	public Stat HealthRegeneration { get; set; } = new(Stat.StatType.HealthRegeneration, 0);
	public Stat ResourceRegeneration { get; set; } = new(Stat.StatType.ResourceRegeneration, 0);
	public Stat PostureRegeneration { get; set; } = new(Stat.StatType.PostureRegeneration, 0);

	// Gear Stats
	public Stat MainHandDamage { get; set; } = new(Stat.StatType.MainHandDamage, 10);
	public Stat OffHandDamage { get; set; } = new(Stat.StatType.OffHandDamage, 0);
	public Stat DamageBonus { get; set; } = new(Stat.StatType.DamageBonus, 0);


	// Misc Stats
	public Stat HealthBonus { get; set; } = new(Stat.StatType.HealthBonus, 0);
	public Stat MovementSpeed { get; set; } = new(Stat.StatType.MovementSpeed, 6f);
	public Stat FallSpeed { get; set; } = new(Stat.StatType.FallSpeed, 40);
	public Stat JumpSpeed { get; set; } = new(Stat.StatType.JumpSpeed, 30);


	
	// Damage stats list
	
	// Defensive Stats
	public Stat BlockAmount { get; set; } = new(Stat.StatType.BlockAmount, 0);
	public Stat Retaliation { get; set; } = new(Stat.StatType.Retaliation, 0);
	
	// Resistance Stats
	public Stat Armor { get; set; } = new(Stat.StatType.Armor, 0);
	

	// Health
	public Stat Health { get; set; } = new(Stat.StatType.Health, 200);
	public Stat HealthOnRetaliation { get; set; } = new(Stat.StatType.HealthOnRetaliation, 0);
	public Stat HealthRegenerationBonus { get; set; } = new(Stat.StatType.HealthRegenerationBonus , 0);

	// Resource
	public Stat Resource { get; set; } = new(Stat.StatType.Resource, 100);
	public Stat ResourceCostReduction { get; set; } = new(Stat.StatType.ResourceCostReduction, 0);
	public Stat ResourceRegenerationBonus { get; set; } = new(Stat.StatType.ResourceRegenerationBonus, 0);
	public Stat CooldownReduction { get; set; } = new(Stat.StatType.CooldownReduction, 0);

	public float CombinedDamage { get; set; } = 0.0f;

	public float PreviousYRotation { get; set; } = 0.0f; // Rotation before current rotation
	public float CurrentYRotation { get; set; } = 0.0f; // Current rotation
	public float PreviousXRotation { get; set; } = 0.0f; // Rotation before current rotation
	public float CurrentXRotation { get; set; } = 0.0f; // Current rotation
	public float PreviousRotation { get; set; } = 0.0f; // Rotation before current rotation
	public float CurrentZRotation { get; set; } = 0.0f; // Current rotation

	public List<StatusEffect> StatusEffects { get; set; } = new List<StatusEffect>();

	public Vector3 DirectionVector = Vector3.Zero; // Direction 
	public Vector3 AimVector = Vector3.Zero; // Direction 
	public Vector3 PositionVector  = Vector3.Zero; // Position 
	public Vector3 VelocityVector  = Vector3.Zero; // Velocity 

	
    public override void _Ready()
    {
		Health.MaxValue = 200;
		Health.BaseValue = Health.MaxValue;
		Health.CurrentValue = Health.MaxValue;

		Resource.MaxValue = 200;
		Resource.BaseValue = Resource.MaxValue;
		Resource.CurrentValue = Resource.MaxValue/2;

		ResourceRegeneration.BaseValue = 1;
		ResourceRegeneration.CurrentValue = 1;

		

		

        EntitySystems.damage_system.Subscribe(this);
		EntitySystems.resource_system.Subscribe(this);
		EntityControllers.status_effect_controller.Subscribe(this);
		

    }

	// public override void _ExitTree()
	// {
	// 	entity_systems.damage_system.unsubscribe(this);
	// 	entity_systems.resource_system.unsubscribe(this);
	// 	entity_controllers.status_effect_controller.Unsubscribe(this);
	// }

}
