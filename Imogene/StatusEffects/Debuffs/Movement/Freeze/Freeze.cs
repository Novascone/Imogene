using Godot;
using System;

public partial class Freeze : StatusEffect
{

	public StatModifier stop { get; set; }= new(StatModifier.ModificationType.Nullify);

    public Freeze()
    {
		name = "freeze";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		prevents_movement = true;
		prevents_input = true;
		duration = 5;
		max_stacks = 1;
    }
 
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.MovementSpeed.AddModifier(stop);
		CreateTimerIncrementStack(entity);
	}

	public override void timer_timeout(Entity entity)
    {
		Remove(entity);
    }

    public override void Remove(Entity entity)
    {
		if(!removed)
		{
			base.Remove(entity);
			entity.MovementSpeed.RemoveModifier(stop);
		}
        
    }

}
