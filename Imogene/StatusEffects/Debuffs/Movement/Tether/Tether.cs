using Godot;
using System;

public partial class Tether : StatusEffect
{	
	public PackedScene tether_mesh { get; set; } = ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Movement/Tether/TetherMesh.tscn");
	public MeshInstance3D tether { get; set; } = null;
	public float tether_length { get; set; } = 10f;
	
	
	public Tether()
	{
		tether = (MeshInstance3D)tether_mesh.Instantiate();
		name = "tether";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		duration = 5;
		max_stacks = 1;
		
	
	}
	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		CreateTimerIncrementStack(entity_);
		AddChild(tether);
		tether.Show();
		tether.GlobalPosition = new Vector3(0,0,0);
		
		
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
			tether.QueueFree();
		}
        
    }
	
}
