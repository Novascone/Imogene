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
        if(current_stacks == 0)
		{
			 GD.Print("Applying fear");
			//  GD.Print(caster.Name);
			//  entity.direction = -caster.GlobalTransform.Basis.Z;
			 GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			entity.status_effect_controller.SetEffectBooleans(this);
		}
    }

    private void timer_timeout(Entity entity)
    {
        current_stacks -= 1;
		entity.status_effect_controller.RemoveStatusEffect(this);
    }
}
