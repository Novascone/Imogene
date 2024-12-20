using Godot;
using System;

public partial class Hamstring : StatusEffect
{
	public StatModifier stop { set; get; }= new(StatModifier.ModificationType.Nullify);
	public Hamstring()
	{
		EffectName = "hamstrung";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		PreventsMovement = true;
		Duration = 5;
		MaxStacks = 1;
	}
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.MovementSpeed.AddModifier(stop);
		CreateTimerIncrementStack(entity);
		
		
	}

	public override void TimerTimeout(Entity entity)
    {
		Remove(entity);
    }

    public override void Remove(Entity entity)
    {
		if(!Removed)
		{
			base.Remove(entity);
			entity.MovementSpeed.RemoveModifier(stop);
		}
        
    }
}
