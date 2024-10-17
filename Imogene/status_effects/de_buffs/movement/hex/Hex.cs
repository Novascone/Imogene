using Godot;
using System;

public partial class Hex : StatusEffect
{
	public PackedScene hex_mesh_scene { get; set; } = ResourceLoader.Load<PackedScene>("res://status_effects/de_buffs/movement/hex/hex_mesh.tscn");
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
 
	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		entity_.movement_speed.AddModifier(stop);
		CreateTimerIncrementStack(entity_);
		entity_.armature.Hide();
		entity_.AddChild(hex_mesh);
		hex_mesh.GlobalPosition = hex_mesh.GlobalPosition with {Y = 0.5f};
		
		
		
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
			entity_.movement_speed.RemoveModifier(stop);
			entity_.armature.Show();
			hex_mesh.QueueFree();
		}
        
    }
}
