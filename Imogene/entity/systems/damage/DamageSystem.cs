using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public partial class DamageSystem : Node
{
	// Timers
	public SceneTreeTimer health_regeneration_timer { get; set; } = null;
	public float health_tick_duration { get; set; } = 1;
	private bool weak { get; set; } = false;
	public bool dead { get; set; } = false;

	// Damage numbers parameters
	private Node3D damage_number_spawn_point { get; set; } = null;
	private float damage_number_spread { get; set; } = 90;
	private float damage_number_height { get; set; } = 60;
	[Export] PackedScene damage_number_3d_template { get; set; }
	private Queue<DamageNumber3D> damage_number_3d_pool { get; set; } = new Queue<DamageNumber3D>();
	private StatModifier add_health { get; set; } = new(StatModifier.ModificationType.AddCurrent);
	private StatModifier remove_health { get; set; } =  new(StatModifier.ModificationType.AddCurrent);
	private StatModifier null_health { get; set; } = new(StatModifier.ModificationType.Nullify);

	[Signal] public delegate void AddStatusEffectEventHandler(Entity entity_, StatusEffect status_effect_);
	[Signal] public delegate void ChangePostureEventHandler(Entity entity_, float posture_damage_);
	[Signal] public delegate void HealthChangedEventHandler(Entity entity_, float health_);
	[Signal] public delegate void WeakEventHandler(Entity entity_, bool weak_);

	
	public static float DamageMitigation(Entity entity, Node3D hitbox, float amount)
	{
		float mitigated_damage = amount;
		mitigated_damage *= 1 - (entity.Armor.CurrentValue / 100);

		if(hitbox is MeleeHitbox _melee_hitbox)
		{
			
			mitigated_damage = MitigateMelee(_melee_hitbox, mitigated_damage, entity);
			
		}

		if(hitbox is RangedHitbox _ranged_hitbox)
		{
			mitigated_damage = MitigateRanged(_ranged_hitbox, mitigated_damage, entity);
		}

		
		return MathF.Round(mitigated_damage);
	}

	public static float MitigateMelee(MeleeHitbox melee_hitbox_, float mitigated_damage_, Entity entity_)
	{
		

		return mitigated_damage_;
	}

	public static float MitigateRanged(RangedHitbox ranged_hitbox_, float mitigated_damage_, Entity entity_)
	{
		

		return mitigated_damage_;
	}

	

	public void TakeDamage(Entity entity, Node3D hitbox, float amount, bool isCritical)
	{
		GD.Print(entity.Name + " taking damage");
		amount = DamageMitigation(entity, hitbox, amount);
		
		if(entity.Health.CurrentValue - amount > 0)
		{
			remove_health.ValueToAdd = -amount;
			entity.Health.AddModifier(remove_health);

			if(entity.Health.CurrentValue < (entity.Health.MaxValue / 2) && !weak)
			{
				weak = true;
				EmitSignal(nameof(Weak), entity,  weak);
			}
			

			if(entity is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.Health.CurrentValue;
				damage_number_spawn_point = enemy.head;
				SpawnDamageNumber(amount, isCritical);
			}
			
			HealthRegeneration(entity);
		}
		else
		{
			entity.Health.AddModifier(null_health);
			dead = true;
		}
	}

	public void HealthRegeneration(Entity entity_)
	{
		if(health_regeneration_timer == null || health_regeneration_timer.TimeLeft == 0)
		{
			health_regeneration_timer = GetTree().CreateTimer(health_tick_duration);
			health_regeneration_timer.Timeout += () => OnHealthRegenerationTickTimeout(entity_);
		}
	}

	private void OnHealthRegenerationTickTimeout(Entity entity)
    {
        if(entity.Health.CurrentValue < entity.Health.MaxValue && entity.Health.HandicapValue == 0)
		{
			add_health.Modification = StatModifier.ModificationType.AddCurrent;
			add_health.ValueToAdd = entity.HealthRegeneration.CurrentValue;
			entity.Health.AddModifier(add_health);
			if(health_regeneration_timer == null || health_regeneration_timer.TimeLeft == 0)
			{
				health_regeneration_timer = GetTree().CreateTimer(health_tick_duration);
				health_regeneration_timer.Timeout += () => OnHealthRegenerationTickTimeout(entity);
			}
			if(entity is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.Health.CurrentValue;
			}
		}
    }

	 public void SpawnDamageNumber(float value, bool isCritical)
	{
		DamageNumber3D damage_number = GetDamageNumber();
		Vector3 position = damage_number_spawn_point.GlobalTransform.Origin;
		AddChild(damage_number, true);
		damage_number.SetValuesAndAnimate(value, isCritical, position, damage_number_height, damage_number_spread);
	}

	public DamageNumber3D GetDamageNumber()
	{
		if(damage_number_3d_pool.Count > 0)
		{
			return damage_number_3d_pool.Dequeue();
		}

		else
		{
			DamageNumber3D new_damage_number = (DamageNumber3D)damage_number_3d_template.Instantiate();
			new_damage_number.TreeExiting += () => damage_number_3d_pool.Enqueue(new_damage_number);
			return new_damage_number;
		}
	}

    private void OnHurtboxBodyEntered(Node3D body_, Entity entity_)
    {
		if(body_ is RangedHitbox _ranged_hitbox)
		{
			foreach(StatusEffect _status_effect in _ranged_hitbox.effects)
			{
				
				EmitSignal(nameof(AddStatusEffect),entity_, _status_effect);
			}

			TakeDamage(entity_, _ranged_hitbox, _ranged_hitbox.damage, _ranged_hitbox.is_critical);
			EmitSignal(nameof(ChangePosture), entity_, _ranged_hitbox.posture_damage);
			
		}
    }

    private void OnHurtboxAreaEntered(Node3D area_, Entity entity_)
    {
         if(area_ is MeleeHitbox _melee_hitbox)
		{
			
			foreach(StatusEffect _status_effect in _melee_hitbox.effects)
			{
				EmitSignal(nameof(AddStatusEffect),entity_, _status_effect);
			}
			
			TakeDamage(entity_, _melee_hitbox, _melee_hitbox.damage, _melee_hitbox.is_critical);
			EmitSignal(nameof(ChangePosture),entity_, _melee_hitbox.posture_damage);
			
		}
    }

	public void Subscribe(Entity entity)
	{
		entity.Hurtbox.AreaEntered += (area_) => OnHurtboxAreaEntered(area_, entity);
		entity.Hurtbox.BodyEntered += (body_) => OnHurtboxBodyEntered(body_, entity);
	}
	public void unsubscribe(Entity entity)
	{
		entity.Hurtbox.AreaEntered -= (area_) => OnHurtboxAreaEntered(area_, entity);
		entity.Hurtbox.BodyEntered -= (body_) => OnHurtboxBodyEntered(body_, entity);
	}

}
