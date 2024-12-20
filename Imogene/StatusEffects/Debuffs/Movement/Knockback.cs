using Godot;
using System;
using System.Threading.Tasks;

public partial class Knockback : StatusEffect
{
	
	public Knockback()
	{
		EffectName = "knockback";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		PreventsInput = true;
		PreventsMovement = true;
		Duration = 0.1f;
		MaxStacks = 1;
	}
	public Knockback(Entity entity)
	{
		Caster = entity;
		Name = "knockback";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		PreventsInput = true;
		PreventsMovement = true;
		Duration = 0.1f;
		MaxStacks = 1;
	}
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		CreateTimerIncrementStack(entity);
		KnockBackEffect(entity);
		
		
	}

	public void KnockBackEffect(Entity entity)
	{
		var knock_back_movement = entity.GlobalTransform.Origin + new Vector3(0,2f,0); // Move the plater up bt 1.85 units
		knock_back_movement += -Caster.Transform.Basis.Z * 6f;
		var knock_back_time = 0.4; // set how long the tween to take to move upward
		var Knock_back_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut); // Create the tween and sets its transition and ease
		Knock_back_tween.SetProcessMode(0);
		Knock_back_tween.TweenProperty(entity, "global_transform:origin", knock_back_movement, knock_back_time); // Tell the tween which object should be moved and what property of that object should be changed, how it should be changed, and how long it should take

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
		}
        
    }
}
