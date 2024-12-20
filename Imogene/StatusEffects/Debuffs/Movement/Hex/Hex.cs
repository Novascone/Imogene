using Godot;
using System;

public partial class Hex : StatusEffect
{
	public PackedScene HexMeshScene { get; set; } = ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Movement/Hex/HexMesh.tscn");
	public MeshInstance3D  HexMesh { get; set; } = null;
	public StatModifier Stop { get; set; } = new(StatModifier.ModificationType.Nullify);
	public Hex()
    {
		EffectName = "hex";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		PreventsMovement = true;
		PreventsMovement = true;
		Duration = 5;
		MaxStacks = 1;
		HexMesh = (MeshInstance3D)HexMeshScene.Instantiate();
    }
 
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.MovementSpeed.AddModifier(Stop);
		CreateTimerIncrementStack(entity);
		entity.Armature.Hide();
		entity.AddChild(HexMesh);
		HexMesh.GlobalPosition = HexMesh.GlobalPosition with {Y = 0.5f};
		
		
		
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
			entity.Armature.Show();
			HexMesh.QueueFree();
		}
        
    }
}
