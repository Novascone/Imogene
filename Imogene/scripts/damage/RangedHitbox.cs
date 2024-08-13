using Godot;
using System;

public partial class RangedHitbox : RigidBody3D
{
	[Export] public string damage_type { get; set; }
	[Export] public float damage { get; set; }
	[Export] public float posture_damage { get; set; }
	public bool is_critical;

	public bool hit;

    public override void _PhysicsProcess(double delta)
    {
        if(hit)
		{
			QueueFree();
		}
    }

	public void _on_body_entered(Node3D body)
	{
		GD.Print("body entered " + body.Name);
		QueueFree();
	}

	public void _on_despawn_timeout()
	{
		QueueFree();
	}
}
