using Godot;
using System;

public partial class Hamstring : StatusEffect
{
	public StatModifier stop { set; get; }= new(StatModifier.ModificationType.Nullify);
	public Hamstring()
	{
		name = "hamstrung";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		prevents_movement = true;
		duration = 5;
		max_stacks = 1;
	}
	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		entity_.movement_speed.AddModifier(stop);
		CreateTimerIncrementStack(entity_);
		
		
	}

	public override void timer_timeout(Entity entity_)
    {
		Remove(entity_);
    }

    public override void Remove(Entity entity_)
    {
		if(!removed)
		{
			base.Remove(entity_);
			entity_.movement_speed.RemoveModifier(stop);
		}
        
    }
}
