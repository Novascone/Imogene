using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public partial class DamageSystem : Node
{
	// Timers
	public SceneTreeTimer health_regeneration_timer { get; set; } = null;
	public float health_tick_duration { get; set; } = 1;

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

	
	public static float DamageMitigation(Entity entity_, Node3D hitbox_, float amount_)
	{
		float mitigated_damage = amount_;
		mitigated_damage *= 1 - (entity_.armor.current_value / 100);

		if(hitbox_ is MeleeHitbox _melee_hitbox)
		{
			
			mitigated_damage = MitigateMelee(_melee_hitbox, mitigated_damage, entity_);
			
		}

		if(hitbox_ is RangedHitbox _ranged_hitbox)
		{
			mitigated_damage = MitigateRanged(_ranged_hitbox, mitigated_damage, entity_);
		}

		
		return MathF.Round(mitigated_damage);
	}

	public static float MitigateMelee(MeleeHitbox melee_hitbox_, float mitigated_damage_, Entity entity_)
	{
		if(melee_hitbox_.type == MeleeHitbox.DamageType.Physical)
		{
			mitigated_damage_ *= entity_.physical_resistance.current_value;
			if(melee_hitbox_.physical_damage_type == MeleeHitbox.PhysicalDamageType.Pierce)
			{
				mitigated_damage_ *= entity_.blunt_resistance.current_value;
			}
			if(melee_hitbox_.physical_damage_type == MeleeHitbox.PhysicalDamageType.Slash)
			{
				mitigated_damage_ *= entity_.blunt_resistance.current_value;
			}
			if(melee_hitbox_.physical_damage_type == MeleeHitbox.PhysicalDamageType.Blunt)
			{
				mitigated_damage_ *= entity_.blunt_resistance.current_value;
			}
		}
		else if(melee_hitbox_.type == MeleeHitbox.DamageType.Spell)
		{
			mitigated_damage_ *= entity_.spell_resistance.current_value;
			if(melee_hitbox_.spell_damage_type == MeleeHitbox.SpellDamageType.Fire)
			{
				mitigated_damage_ *= entity_.fire_resistance.current_value;
			}
			if(melee_hitbox_.spell_damage_type == MeleeHitbox.SpellDamageType.Cold)
			{
				mitigated_damage_ *= entity_.cold_resistance.current_value;
			}
			if(melee_hitbox_.spell_damage_type == MeleeHitbox.SpellDamageType.Lightning)
			{
				mitigated_damage_ *= entity_.lightning_resistance.current_value;
			}
			if(melee_hitbox_.spell_damage_type == MeleeHitbox.SpellDamageType.Holy)
			{
				mitigated_damage_ *= entity_.holy_resistance.current_value;
			}
		}
		else if(melee_hitbox_.type == MeleeHitbox.DamageType.Other)
		{
			if(melee_hitbox_.other_damage_type == MeleeHitbox.OtherDamageType.Bleed)
			{
				mitigated_damage_ *= 1 - entity_.bleed_resistance.current_value;
			}
			if(melee_hitbox_.other_damage_type == MeleeHitbox.OtherDamageType.Poison)
			{
				mitigated_damage_ *= entity_.poison_resistance.current_value;
			}
			if(melee_hitbox_.other_damage_type == MeleeHitbox.OtherDamageType.Curse)
			{
				mitigated_damage_ *= entity_.curse_resistance.current_value;
			}
		}

		return mitigated_damage_;
	}

	public static float MitigateRanged(RangedHitbox ranged_hitbox_, float mitigated_damage_, Entity entity_)
	{
		if(ranged_hitbox_.type == RangedHitbox.DamageType.Physical)
			{
				mitigated_damage_ *= entity_.physical_resistance.current_value;
				if(ranged_hitbox_.physical_damage_type == RangedHitbox.PhysicalDamageType.Pierce)
				{
					mitigated_damage_ *= entity_.blunt_resistance.current_value;
				}
				if(ranged_hitbox_.physical_damage_type == RangedHitbox.PhysicalDamageType.Slash)
				{
					mitigated_damage_ *= entity_.blunt_resistance.current_value;
				}
				if(ranged_hitbox_.physical_damage_type == RangedHitbox.PhysicalDamageType.Blunt)
				{
					mitigated_damage_ *= entity_.blunt_resistance.current_value;
				}
			}
			else if(ranged_hitbox_.type == RangedHitbox.DamageType.Spell)
			{
				mitigated_damage_ *= entity_.spell_resistance.current_value;
				if(ranged_hitbox_.spell_damage_type == RangedHitbox.SpellDamageType.Fire)
				{
					mitigated_damage_ *= entity_.fire_resistance.current_value;
				}
				if(ranged_hitbox_.spell_damage_type == RangedHitbox.SpellDamageType.Cold)
				{
					mitigated_damage_ *= entity_.cold_resistance.current_value;
				}
				if(ranged_hitbox_.spell_damage_type == RangedHitbox.SpellDamageType.Lightning)
				{
					mitigated_damage_ *= entity_.lightning_resistance.current_value;
				}
				if(ranged_hitbox_.spell_damage_type == RangedHitbox.SpellDamageType.Holy)
				{
					mitigated_damage_ *= entity_.holy_resistance.current_value;
				}
			}
			else if(ranged_hitbox_.type == RangedHitbox.DamageType.Other)
			{
				if(ranged_hitbox_.other_damage_type == RangedHitbox.OtherDamageType.Bleed)
				{
					mitigated_damage_ *= entity_.bleed_resistance.current_value;
				}
				if(ranged_hitbox_.other_damage_type == RangedHitbox.OtherDamageType.Poison)
				{
					mitigated_damage_ *= entity_.poison_resistance.current_value;
				}
				if(ranged_hitbox_.other_damage_type == RangedHitbox.OtherDamageType.Curse)
				{
					mitigated_damage_ *= entity_.curse_resistance.current_value;
				}
			}

		return mitigated_damage_;
	}

	public static bool Critical(Entity entity_)
	{
		float random_float = GD.Randf();
		if(random_float < entity_.critical_hit_chance.current_value)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void TakeDamage(Entity entity_, Node3D hitbox_, float amount_, bool is_critical_)
	{
		
		amount_ = DamageMitigation(entity_, hitbox_, amount_);
		GD.Print("damage amount after mitigation " + amount_);
		
		if(entity_.health.current_value - amount_ > 0)
		{
			remove_health.value_to_add = -amount_;
			entity_.health.AddModifier(remove_health);

			if(entity_ is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.health.current_value;
				damage_number_spawn_point = enemy.head;
				SpawnDamageNumber(amount_, is_critical_);
			}
			
			HealthRegeneration(entity_);
		}
		else
		{
			entity_.health.AddModifier(null_health);
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

	private void OnHealthRegenerationTickTimeout(Entity entity_)
    {
        if(entity_.health.current_value < entity_.health.max_value)
		{
			add_health.modification = StatModifier.ModificationType.AddCurrent;
			add_health.value_to_add = entity_.health_regeneration.current_value;
			entity_.health.AddModifier(add_health);
			if(health_regeneration_timer == null || health_regeneration_timer.TimeLeft == 0)
			{
				health_regeneration_timer = GetTree().CreateTimer(health_tick_duration);
				health_regeneration_timer.Timeout += () => OnHealthRegenerationTickTimeout(entity_);
			}
			if(entity_ is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.health.current_value;
			}
		}
    }

	 public void SpawnDamageNumber(float value_, bool is_critical_)
	{
		DamageNumber3D damage_number = GetDamageNumber();
		Vector3 position = damage_number_spawn_point.GlobalTransform.Origin;
		AddChild(damage_number, true);
		damage_number.SetValuesAndAnimate(value_, is_critical_, position, damage_number_height, damage_number_spread);
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

	public void Subscribe(Entity entity_)
	{
		entity_.hurtbox.AreaEntered += (area_) => OnHurtboxAreaEntered(area_, entity_);
		entity_.hurtbox.BodyEntered += (body_) => OnHurtboxBodyEntered(body_, entity_);
	}
	public void unsubscribe(Entity entity_)
	{
		entity_.hurtbox.AreaEntered -= (area_) => OnHurtboxAreaEntered(area_, entity_);
		entity_.hurtbox.BodyEntered -= (body_) => OnHurtboxBodyEntered(body_, entity_);
	}

}
