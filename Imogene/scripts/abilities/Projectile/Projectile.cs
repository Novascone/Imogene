using Godot;
using System;

public partial class Projectile : Ranged
{

	public Timer cast_timer;
	public PackedScene projectile_to_load;
	public int projectile_velocity = 25;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		projectile_to_load = GD.Load<PackedScene>("res://scripts/abilities/Projectile/projectile_to_load.tscn");
		cast_timer = GetNode<Timer>("CastTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(player.can_use_abilities && useable && button_pressed && CheckCross() && cast_timer.TimeLeft == 0)
		{
			Execute();
		}
	}

	public override void Execute()
	{
		// GD.Print("Casting");
		cast_timer.Start();
		Vector3 collision = GetPlayerCollision();
		LaunchProjectile(collision);
		
	}

	public void LaunchProjectile(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized();
		RangedHitbox projectile = (RangedHitbox)projectile_to_load.Instantiate();
		player.exclude.Add(projectile.GetRid());
		projectile.TreeExited  += () => RemoveFromExclusion(projectile.GetRid());
		player.cast_point.AddChild(projectile);
		
		if(player.damage_system.Crit())
		{
			projectile.damage = MathF.Round(player.damage * (1 + player.critical_hit_damage), 2);
			projectile.posture_damage = player.posture_damage / 3;
			projectile.is_critical = true;
		}
		else
		{
			
			projectile.damage = player.damage;
			projectile.posture_damage = player.posture_damage / 3;
			projectile.is_critical = false;
		}
		projectile.damage_type = "cold";
		projectile.LinearVelocity = cast_direction * projectile_velocity;
	}

	public void RemoveFromExclusion(Rid projectile_rid)
	{
		player.exclude.Remove(projectile_rid);
	}
}
