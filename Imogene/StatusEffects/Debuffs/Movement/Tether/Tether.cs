using Godot;
using System;

public partial class Tether : StatusEffect
{	
	public PackedScene TetherScene { get; set; } = ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Movement/Tether/TetherMesh.tscn");
	public MeshInstance3D TetherMesh { get; set; } = null;
	public float TetherLength { get; set; } = 10f;
	
	
	public Tether()
	{
		TetherMesh = (MeshInstance3D)TetherScene.Instantiate();
		EffectName = "tether";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		Duration = 5;
		MaxStacks = 1;
		
	
	}
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		CreateTimerIncrementStack(entity);
		AddChild(TetherMesh);
		TetherMesh.Show();
		TetherMesh.GlobalPosition = new Vector3(0,0,0);
		
		
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
			TetherMesh.QueueFree();
		}
        
    }
	
}
