using Godot;
using System;

public partial class Hex : StatusEffect
{
	public PackedScene hex_mesh_scene { get; set; } = ResourceLoader.Load<PackedScene>("res://StatusEffects/Debuffs/Movement/Hex/HexMesh.tscn");
	public MeshInstance3D  hex_mesh { get; set; } = null;
	public StatModifier stop { get; set; } = new(StatModifier.ModificationType.Nullify);
	public Hex()
    {
		name = "hex";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		prevents_movement = true;
		prevents_input = true;
		duration = 5;
		max_stacks = 1;
		hex_mesh = (MeshInstance3D)hex_mesh_scene.Instantiate();
    }
 
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.MovementSpeed.AddModifier(stop);
		CreateTimerIncrementStack(entity);
		entity.Armature.Hide();
		entity.AddChild(hex_mesh);
		hex_mesh.GlobalPosition = hex_mesh.GlobalPosition with {Y = 0.5f};
		
		
		
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
			entity.MovementSpeed.RemoveModifier(stop);
			entity.Armature.Show();
			hex_mesh.QueueFree();
		}
        
    }
}
