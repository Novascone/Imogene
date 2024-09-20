using Godot;
using System;

public partial class Tether : StatusEffect
{	
	public PackedScene tether_mesh = ResourceLoader.Load<PackedScene>("res://status_effects/de_buffs/movement/tether/tether_mesh.tscn");
	public MeshInstance3D tether;
	
	
	public Tether()
	{
		tether = (MeshInstance3D)tether_mesh.Instantiate();
		name = "tether";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		duration = 5;
		max_stacks = 1;
	
	}
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		CreateTimerIncrementStack(entity);
		AddChild(tether);
		tether.Show();
		tether.GlobalPosition = new Vector3(0,0,0);
		
		
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
			tether.QueueFree();
		}
        
    }
	
}
