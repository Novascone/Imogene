using Godot;
using System;

public partial class Virulent : StatusEffect
{
	public VirulentHitbox Hitbox { get; set; } 

	public Poison MainPoison { get; set; } = new ();
	

	public Virulent()
	{
		EffectName = "virulent";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		AddsAdditionalEffects = true;
		AddsEffectToAdditionalEntity = true;
		MaxStacks = 1;
		Duration = 1;
		Hitbox = (VirulentHitbox)ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Health/Virulent/VirulentHitbox.tscn").Instantiate();
		
	}

    public override void Apply(Entity entity)
    {
        base.Apply(entity);
		EmitSignal(nameof(AddAdditionalStatusEffect), MainPoison);
		Hitbox.RootInfected = entity;
		entity.AddChild(Hitbox);
		
		CreateTimerIncrementStack(entity);
    }

    public override void TimerTimeout(Entity entity)
    {
        base.TimerTimeout(entity);
		Remove(entity);
    }

    public override void Remove(Entity entity)
    {
		foreach(Enemy enemy in Hitbox.EnemiesToBeInfected)
		{
			Poison infectPoison = new (); 
			infectPoison.Hitbox.Damage *= 0.5f; 
			GD.Print("enemy to be infected " + enemy.Name);
			EmitSignal(nameof(AddStatusEffectToAdditionalEntity), enemy, infectPoison);
			
		}
        base.Remove(entity);
		entity.RemoveChild(Hitbox);
    }
}
