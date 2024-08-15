using Godot;
using System;

public partial class Projectile : Ranged
{

	public Timer cast_timer;
	public Timer wait_for_rotation_timer;
	public PackedScene projectile_to_load;
	public bool can_cast;
	public int projectile_velocity = 25;
	private bool targeting_system_loaded = false;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		projectile_to_load = GD.Load<PackedScene>("res://scripts/abilities/Projectile/projectile_to_load.tscn");
		cast_timer = GetNode<Timer>("CastTimer");
		wait_for_rotation_timer = GetNode<Timer>("WaitForRotation");
		

    }

    private void HandlePlayerFacingEnemy()
    {
        GD.Print("Player facing enemy");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		// if(player.abilities_in_use.Contains(this))
		// {
		// 	PlayerFacingEnemy();
		// }
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		{
			state = States.queued;
		}
		if(state == States.queued)
		{
			if(player.can_use_abilities && useable && CheckCross() && cast_timer.TimeLeft == 0)
			{

				if(!player.targeting && player.targeting_system.closest_enemy_soft != null && player.targeting_system.soft_target_on)
				{
					if(player.targeting_system.enemy_in_soft_small || player.targeting_system.closest_enemy_soft.in_player_vision)
					{
						player.targeting_system.SoftTargetRotation();
						if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
						{
							Execute();
						}
					}
					else
					{
						Execute();
					}
					
				}
				else
				{
					GD.Print("execute with out targeting");
					Execute();
				}
			}
		
		}		
	}

	public override void Execute()
	{
		// GD.Print("Casting");
		state = States.not_queued;
		AddToAbilityList(this);
		cast_timer.Start();
		Vector3 collision = GetPlayerCollision();
		LaunchProjectile(collision);

	}

	public void LaunchProjectile(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized();
		RangedHitbox projectile = (RangedHitbox)projectile_to_load.Instantiate();
		player.exclude.Add(projectile.GetRid());
		projectile.TreeExited  += () => RemoveFromExclusion(projectile.GetRid(), projectile);
		player.cast_point.AddChild(projectile);
		projectile.TopLevel = true;

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

	public void RemoveFromExclusion(Rid projectile_rid, RangedHitbox projectile)
	{
		player.exclude.Remove(projectile_rid);
		
		
	}

	public void _on_cast_timer_timeout()
	{
		RemoveFromAbilityList(this);
		
	}

}
