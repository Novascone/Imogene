using Godot;
using System;

public partial class VirulentHitbox : Area3D
{
	public Entity root_infected { get; set; } = null;
	public Poison poison { get; set; } = new ();
	[Signal] public delegate void AddAdditionalStatusEffectEventHandler(Entity entity_, StatusEffect status_effect_);

	public void _on_body_entered(Node3D body_)
	{
		if(body_ is Enemy enemy && enemy != root_infected)
		{
			poison.hitbox.damage = poison.hitbox.damage / 2;
			EmitSignal(nameof(AddAdditionalStatusEffect), body_, poison);
			GD.Print("enemy entered virulent hitbox");
			
		}
	}
}
