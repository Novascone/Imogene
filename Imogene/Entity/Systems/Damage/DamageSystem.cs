using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public partial class DamageSystem : Node
{
	// Timers
	public SceneTreeTimer HealthRegenerationTimer { get; set; } = null;
	public float HealthTickDuration { get; set; } = 1;
	private bool EntityWeak { get; set; } = false;
	public bool Dead { get; set; } = false;

	// Damage numbers parameters
	private Node3D DamageNumberSpawnPoint { get; set; } = null;
	private float DamageNumberSpread { get; set; } = 90;
	private float DamageNumberHeight { get; set; } = 60;
	[Export] PackedScene DamageNumber3DTemplate { get; set; }
	private Queue<DamageNumber3D> DamageNumber3DPool { get; set; } = new Queue<DamageNumber3D>();
	private StatModifier AddHealth { get; set; } = new(StatModifier.ModificationType.AddCurrent);
	private StatModifier RemoveHealth { get; set; } =  new(StatModifier.ModificationType.AddCurrent);
	private StatModifier NullHealth { get; set; } = new(StatModifier.ModificationType.Nullify);

	[Signal] public delegate void AddStatusEffectEventHandler(Entity entity, StatusEffect statusEffect);
	[Signal] public delegate void ChangePostureEventHandler(Entity entity, float postureDamage);
	[Signal] public delegate void HealthChangedEventHandler(Entity entity, float health);
	[Signal] public delegate void WeakEventHandler(Entity entity, bool weak);

	
	public static float DamageMitigation(Entity entity, Node3D hitbox, float amount)
	{
		float mitigatedDamage = amount;
		mitigatedDamage *= 1 - (entity.Armor.CurrentValue / 100);

		if(hitbox is MeleeHitbox meleeHitbox)
		{
			
			mitigatedDamage = MitigateMelee(meleeHitbox, mitigatedDamage, entity);
			
		}

		if(hitbox is RangedHitbox rangedHitbox)
		{
			mitigatedDamage = MitigateRanged(rangedHitbox, mitigatedDamage, entity);
		}

		
		return MathF.Round(mitigatedDamage);
	}

	public static float MitigateMelee(MeleeHitbox meleeHitbox, float mitigatedDamage, Entity entity)
	{
		

		return mitigatedDamage;
	}

	public static float MitigateRanged(RangedHitbox rangedHitbox, float mitigatedDamage, Entity entity)
	{
		

		return mitigatedDamage;
	}

	

	public void TakeDamage(Entity entity, Node3D hitbox, float amount, bool isCritical)
	{
		GD.Print(entity.Name + " taking damage");
		amount = DamageMitigation(entity, hitbox, amount);
		
		if(entity.Health.CurrentValue - amount > 0)
		{
			RemoveHealth.ValueToAdd = -amount;
			entity.Health.AddModifier(RemoveHealth);

			if(entity.Health.CurrentValue < (entity.Health.MaxValue / 2) && !EntityWeak)
			{
				EntityWeak = true;
				EmitSignal(nameof(Weak), entity,  EntityWeak);
			}
			

			if(entity is Enemy enemy)
			{
				enemy.UI.HealthBar.Value = enemy.Health.CurrentValue;
				DamageNumberSpawnPoint = enemy.Head;
				SpawnDamageNumber(amount, isCritical);
			}
			
			HealthRegeneration(entity);
		}
		else
		{
			entity.Health.AddModifier(NullHealth);
			Dead = true;
		}
	}

	public void HealthRegeneration(Entity entity)
	{
		if(HealthRegenerationTimer == null || HealthRegenerationTimer.TimeLeft == 0)
		{
			HealthRegenerationTimer = GetTree().CreateTimer(HealthTickDuration);
			HealthRegenerationTimer.Timeout += () => OnHealthRegenerationTickTimeout(entity);
		}
	}

	private void OnHealthRegenerationTickTimeout(Entity entity)
    {
        if(entity.Health.CurrentValue < entity.Health.MaxValue && entity.Health.HandicapValue == 0)
		{
			AddHealth.Modification = StatModifier.ModificationType.AddCurrent;
			AddHealth.ValueToAdd = entity.HealthRegeneration.CurrentValue;
			entity.Health.AddModifier(AddHealth);
			if(HealthRegenerationTimer == null || HealthRegenerationTimer.TimeLeft == 0)
			{
				HealthRegenerationTimer = GetTree().CreateTimer(HealthTickDuration);
				HealthRegenerationTimer.Timeout += () => OnHealthRegenerationTickTimeout(entity);
			}
			if(entity is Enemy enemy)
			{
				enemy.UI.HealthBar.Value = enemy.Health.CurrentValue;
			}
		}
    }

	 public void SpawnDamageNumber(float value, bool isCritical)
	{
		DamageNumber3D damageNumber = GetDamageNumber();
		Vector3 position = DamageNumberSpawnPoint.GlobalTransform.Origin;
		AddChild(damageNumber, true);
		damageNumber.SetValuesAndAnimate(value, isCritical, position, DamageNumberHeight, DamageNumberSpread);
	}

	public DamageNumber3D GetDamageNumber()
	{
		if(DamageNumber3DPool.Count > 0)
		{
			return DamageNumber3DPool.Dequeue();
		}

		else
		{
			DamageNumber3D newDamageNumber = (DamageNumber3D)DamageNumber3DTemplate.Instantiate();
			newDamageNumber.TreeExiting += () => DamageNumber3DPool.Enqueue(newDamageNumber);
			return newDamageNumber;
		}
	}

    private void OnHurtboxBodyEntered(Node3D body, Entity entity)
    {
		if(body is RangedHitbox rangedHitbox)
		{
			foreach(StatusEffect statusEffect in rangedHitbox.Effects)
			{
				
				EmitSignal(nameof(AddStatusEffect),entity, statusEffect);
			}

			TakeDamage(entity, rangedHitbox, rangedHitbox.Damage, rangedHitbox.IsCritical);
			EmitSignal(nameof(ChangePosture), entity, rangedHitbox.PostureDamage);
			
		}
    }

    private void OnHurtboxAreaEntered(Node3D area, Entity entity)
    {
         if(area is MeleeHitbox meleeHitbox)
		{
			
			foreach(StatusEffect statusEffect in meleeHitbox.Effects)
			{
				EmitSignal(nameof(AddStatusEffect),entity, statusEffect);
			}
			
			TakeDamage(entity, meleeHitbox, meleeHitbox.Damage, meleeHitbox.IsCritical);
			EmitSignal(nameof(ChangePosture),entity, meleeHitbox.PostureDamage);
			
		}
    }

	public void Subscribe(Entity entity)
	{
		entity.Hurtbox.AreaEntered += (area) => OnHurtboxAreaEntered(area, entity);
		entity.Hurtbox.BodyEntered += (body) => OnHurtboxBodyEntered(body, entity);
	}
	public void unsubscribe(Entity entity)
	{
		entity.Hurtbox.AreaEntered -= (area) => OnHurtboxAreaEntered(area, entity);
		entity.Hurtbox.BodyEntered -= (body) => OnHurtboxBodyEntered(body, entity);
	}

}
