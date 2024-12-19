using Godot;
using System;

public partial class Virulent : StatusEffect
{
	public VirulentHitbox hitbox { get; set; } 

	public Poison main_poison { get; set; } = new ();
	

	public Virulent()
	{
		name = "virulent";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		adds_additional_effects = true;
		adds_effect_to_additional_entity = true;
		max_stacks = 1;
		duration = 1;
		hitbox = (VirulentHitbox)ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Health/Virulent/VirulentHitbox.tscn").Instantiate();
		
	}

    public override void Apply(Entity entity_)
    {
        base.Apply(entity_);
		EmitSignal(nameof(AddAdditionalStatusEffect), main_poison);
		hitbox.root_infected = entity_;
		entity_.AddChild(hitbox);
		
		CreateTimerIncrementStack(entity_);
    }

    public override void timer_timeout(Entity entity_)
    {
        base.timer_timeout(entity_);
		Remove(entity_);
    }

    public override void Remove(Entity entity_)
    {
		foreach(Enemy _enemy in hitbox.enemies_to_be_infected)
		{
			Poison _infect_poison = new (); 
			_infect_poison.hitbox.Damage *= 0.5f; 
			GD.Print("enemy to be infected " + _enemy.Name);
			EmitSignal(nameof(AddStatusEffectToAdditionalEntity), _enemy, _infect_poison);
			
		}
        base.Remove(entity_);
		entity_.RemoveChild(hitbox);
    }
}
