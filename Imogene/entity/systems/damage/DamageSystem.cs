using Godot;
using System;
using System.Collections.Generic;


public partial class DamageSystem : Node
{
	// Timers
	public SceneTreeTimer health_regeneration_timer { get; set; } = null;
	public float health_tick_duration { get; set; } = 1;

	public bool dead;

	// Damage numbers parameters
	private Node3D spawn_point;
	private float spread = 90;
	private float height = 60;
	[Export] PackedScene damage_number_3d_template;
	private Queue<DamageNumber3D> damage_number_3d_pool = new Queue<DamageNumber3D>();
	private StatModifier remove_health = new(StatModifier.ModificationType.AddCurrent);
	private StatModifier add_health  = new(StatModifier.ModificationType.AddCurrent);
	
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	
	public void SubscribeToHurtboxSignals(Entity entity_)
	{
		entity_.hurtbox.AreaEntered += (area_) => OnHurtboxAreaEntered(area_, entity_);
		entity_.hurtbox.BodyEntered += (body_) => OnHurtboxBodyEntered(body_, entity_);
	}

	public void Subscribe(Entity entity_)
	{

		health_regeneration_timer = GetTree().CreateTimer(health_tick_duration);
		health_regeneration_timer.Timeout += () => OnHealthRegenerationTickTimeout(entity_);
	}

    private void OnHurtboxBodyEntered(Node3D body_, Entity entity_)
    {
        	if(body_ is RangedHitbox ranged_box)
			{
				foreach(StatusEffect status_effect in ranged_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					entity_.entity_controllers.status_effect_controller.AddStatusEffect(entity_, status_effect);
				}
				if(body_ is RangedHitbox)
				{
					GD.Print(entity_.Name + " Is taking " + ranged_box.damage  + " damage");
					TakeDamage(entity_, ranged_box, ranged_box.damage, ranged_box.is_critical);
					entity_.entity_systems.resource_system.ChangePosture(entity_, ranged_box.posture_damage);
				}
			}
    }

    private void OnHurtboxAreaEntered(Node3D area_, Entity entity_)
    {
         if(area_ is MeleeHitbox melee_box)
		{
			// GD.Print(Name + " hurtbox entered by " + melee_box.Name);
			if(area_ is MeleeHitbox)
			{
				foreach(StatusEffect status_effect in melee_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					entity_.entity_controllers.status_effect_controller.AddStatusEffect(entity_, status_effect);
				}
				
				TakeDamage(entity_, melee_box, melee_box.damage, melee_box.is_critical);
				entity_.entity_systems.resource_system.ChangePosture(entity_, melee_box.posture_damage);
			}
		}
    }

    public void SpawnDamageNumber(float value_, bool is_critical_)
	{
		DamageNumber3D damage_number = GetDamageNumber();
		Vector3 position = spawn_point.GlobalTransform.Origin;
		AddChild(damage_number, true);
		damage_number.SetValuesAndAnimate(value_, is_critical_, position, height, spread);
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


    public bool Crit(Entity entity_)
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

	public void TakeDamage(Entity entity_, Node3D hitbox_, float amount_, bool is_critical_) // Applies damage to an entity
	{
		
		amount_ = DamageMitigation(entity_, hitbox_, amount_);
		
		if(entity_.health.current_value - amount_ > 0)
		{
			remove_health.modification = StatModifier.ModificationType.AddCurrent;
			remove_health.value_to_add = -amount_;
			entity_.health.AddModifier(remove_health);
			GD.Print(entity_.Name + " current health " + entity_.health.current_value);
			if(entity_ is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.health.current_value;
				GD.Print("enemy ui health value " + enemy.ui.health_bar.Value);
				spawn_point = enemy.head;
				SpawnDamageNumber(amount_, is_critical_);
			}
			
			HealthRegeneration(entity_);
		}
		else
		{
			dead = true;
			GD.Print("dead");
		}
	}


	public void HealthRegeneration(Entity entity_)
	{
		GD.Print("starting health regen timer");
		if(health_regeneration_timer == null || health_regeneration_timer.TimeLeft == 0)
		{
			health_regeneration_timer = GetTree().CreateTimer(health_tick_duration);
			health_regeneration_timer.Timeout += () => OnHealthRegenerationTickTimeout(entity_);
		}
	}

	private void OnHealthRegenerationTickTimeout(Entity entity_)
    {
		GD.Print("Heal regen tick timeout");
        if(entity_.health.current_value < entity_.health.max_value && entity_.health_regeneration.current_value > 0)
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

	public float DamageMitigation(Entity entity_, Node3D hitbox_, float amount_)
	{
		float mitigated_damage = amount_;
		if(hitbox_ is MeleeHitbox melee_hitbox)
		{
			if(melee_hitbox.type == MeleeHitbox.DamageType.Physical)
			{

			}
		}

		if(hitbox_ is RangedHitbox ranged_hitbox)
		{
			GD.Print("Ranged hitbox");
			if(ranged_hitbox.type == RangedHitbox.DamageType.Physical)
			{
				
			}
		}

		
		// GD.Print(mitigated_damage + " of damage going into mitigation ");
		// mitigated_damage *= 1 - (entity_.armor.current_value / 100);
		// // GD.Print("Damage reduced by armor to " + mitigated_damage);
		// if(damage_type == "Slash" || damage_type == "Thrust" || damage_type == "Blunt")
		// {
		// 	mitigated_damage *= entity_.physical_resistance.current_value;
		// 	// GD.Print("Damage reduced by physical resistance to " + mitigated_damage);
		// 	if(damage_type == "Slash")
		// 	{
		// 		mitigated_damage *= entity_.slash_resistance.current_value;
		// 		// GD.Print("Damage reduced by slash resistance to " + mitigated_damage);
		// 		return MathF.Round(mitigated_damage);
				
		// 	}
		// 	if(damage_type == "Pierce")
		// 	{
		// 		mitigated_damage *= entity_.slash_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Blunt")
		// 	{
		// 		mitigated_damage *= entity_.blunt_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// }
		// if(damage_type == "Bleed")
		// {
		// 	mitigated_damage *= entity_.bleed_resistance.current_value;
		// 	GD.Print("Damage reduced by bleed resistance to " + mitigated_damage);
		// 	return MathF.Round(mitigated_damage);
		// }
		// if(damage_type == "Poison")
		// {
		// 	mitigated_damage *= entity_.poison_resistance.current_value;
		// 	return MathF.Round(mitigated_damage);
		// }
		// if(damage_type == "Fire" || damage_type == "Cold" ||  damage_type == "Lightning" || damage_type == "Holy")
		// {
		// 	mitigated_damage *= entity_.poison_resistance.current_value;
		// 	if(damage_type == "Fire")
		// 	{
		// 		mitigated_damage *= entity_.fire_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Cold")
		// 	{
		// 		mitigated_damage *= entity_.cold_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Lightning")
		// 	{
		// 		mitigated_damage *= entity_.lightning_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Holy")
		// 	{
		// 		mitigated_damage *= entity_.holy_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// }
		return MathF.Round(mitigated_damage);
	}

	public float DamageMitigation(Entity entity_, RangedHitbox.DamageType damage_type, float amount)
	{
		float mitigated_damage = amount;
		// GD.Print(mitigated_damage + " of damage going into mitigation ");
		// mitigated_damage *= 1 - (entity_.armor.current_value / 100);
		// // GD.Print("Damage reduced by armor to " + mitigated_damage);
		// if(damage_type == "Slash" || damage_type == "Thrust" || damage_type == "Blunt")
		// {
		// 	mitigated_damage *= entity_.physical_resistance.current_value;
		// 	// GD.Print("Damage reduced by physical resistance to " + mitigated_damage);
		// 	if(damage_type == "Slash")
		// 	{
		// 		mitigated_damage *= entity_.slash_resistance.current_value;
		// 		// GD.Print("Damage reduced by slash resistance to " + mitigated_damage);
		// 		return MathF.Round(mitigated_damage);
				
		// 	}
		// 	if(damage_type == "Pierce")
		// 	{
		// 		mitigated_damage *= entity_.slash_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Blunt")
		// 	{
		// 		mitigated_damage *= entity_.blunt_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// }
		// if(damage_type == "Bleed")
		// {
		// 	mitigated_damage *= entity_.bleed_resistance.current_value;
		// 	GD.Print("Damage reduced by bleed resistance to " + mitigated_damage);
		// 	return MathF.Round(mitigated_damage);
		// }
		// if(damage_type == "Poison")
		// {
		// 	mitigated_damage *= entity_.poison_resistance.current_value;
		// 	return MathF.Round(mitigated_damage);
		// }
		// if(damage_type == "Fire" || damage_type == "Cold" ||  damage_type == "Lightning" || damage_type == "Holy")
		// {
		// 	mitigated_damage *= entity_.poison_resistance.current_value;
		// 	if(damage_type == "Fire")
		// 	{
		// 		mitigated_damage *= entity_.fire_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Cold")
		// 	{
		// 		mitigated_damage *= entity_.cold_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Lightning")
		// 	{
		// 		mitigated_damage *= entity_.lightning_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// 	if(damage_type == "Holy")
		// 	{
		// 		mitigated_damage *= entity_.holy_resistance.current_value;
		// 		return MathF.Round(mitigated_damage);
		// 	}
		// }
		return MathF.Round(mitigated_damage);
	}

}
