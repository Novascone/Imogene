using Godot;
using System;

public partial class Fear : StatusEffect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		duration = 5;
		effect_type = "movement";
		max_stacks = 1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

    public override void Apply(Entity entity)
    {
        CreateTimerIncrementStack(entity);
    }

    public override void timer_timeout(Entity entity)
    {
        current_stacks -= 1;
		EmitSignal(nameof(StatusEffectFinished));
    }
}
