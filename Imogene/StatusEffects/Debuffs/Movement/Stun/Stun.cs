using Godot;
using System;

public partial class Stun : StatusEffect
{
	public StatModifier Stop { get; set; } = new(StatModifier.ModificationType.Nullify);
	public Stun()
    {
		EffectName = "stun";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		PreventsMovement = true;
		PreventsInput = true;
		Duration = 5;
		MaxStacks = 1;
    }
 
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.MovementSpeed.AddModifier(Stop);
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
			entity.MovementSpeed.RemoveModifier(Stop);
		}
        
    }
}
