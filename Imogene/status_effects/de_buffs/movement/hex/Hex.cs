using Godot;
using System;

public partial class Hex : StatusEffect
{
	public PackedScene hex_mesh_scene = ResourceLoader.Load<PackedScene>("res://status_effects/de_buffs/movement/hex/hex_mesh.tscn");
	public MeshInstance3D hex_mesh;
	public StatModifier stop = new(StatModifier.ModificationType.Nullify);
	public Hex()
    {
		name = "hex";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		prevents_movement = true;
		prevents_input = true;
		duration = 5;
		max_stacks = 1;
		hex_mesh = (MeshInstance3D)hex_mesh_scene.Instantiate();
    }
 
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.movement_speed.AddModifier(stop);
		CreateTimerIncrementStack(entity);
		entity.armature.Hide();
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
			entity.movement_speed.RemoveModifier(stop);
			entity.armature.Show();
			hex_mesh.QueueFree();
		}
        
    }
}
