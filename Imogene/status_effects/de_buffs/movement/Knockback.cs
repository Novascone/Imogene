using Godot;
using System;
using System.Threading.Tasks;

public partial class Knockback : StatusEffect
{
	
	public Knockback()
	{
		name = "knockback";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		prevents_input = true;
		prevents_movement = true;
		duration = 0.1f;
		max_stacks = 1;
	}
	public Knockback(Entity entity_)
	{
		caster = entity_;
		name = "knockback";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		prevents_input = true;
		prevents_movement = true;
		duration = 0.1f;
		max_stacks = 1;
	}
	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		CreateTimerIncrementStack(entity_);
		KnockBackEffect(entity_);
		
		
	}

	public void KnockBackEffect(Entity entity_)
	{
		var knock_back_movement = entity_.GlobalTransform.Origin + new Vector3(0,2f,0); // Move the plater up bt 1.85 units
		knock_back_movement += -caster.Transform.Basis.Z * 6f;
		var knock_back_time = 0.4; // set how long the tween to take to move upward
		var Knock_back_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut); // Create the tween and sets its transition and ease
		Knock_back_tween.SetProcessMode(0);
		Knock_back_tween.TweenProperty(entity_, "global_transform:origin", knock_back_movement, knock_back_time); // Tell the tween which object should be moved and what property of that object should be changed, how it should be changed, and how long it should take

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
		}
        
    }
}
