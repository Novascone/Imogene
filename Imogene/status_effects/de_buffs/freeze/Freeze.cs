using Godot;
using System;

public partial class Freeze : StatusEffect
{

	public StatModifier stop = new(StatModifier.ModificationType.nullify);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		prevents_movement = true;
		duration = 5;
		effect_type = "movement";
		max_stacks = 1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}


	public override void Apply(Entity entity)
	{
		
		CreateTimerIncrementStack(entity);
		
	}

	public override void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout");
		current_stacks -= 1;
		EmitSignal(nameof(StatusEffectFinished));
		entity.movement_speed.RemoveModifier(stop);
		GD.Print("entity is no longer frozen");
		GD.Print("current stacks of " + this.Name + " " + current_stacks);
    }

}
