using Godot;
using System;

public partial class Projectile : RangedAbility
{

	[Export] public PackedScene projectile_to_load;
	[Export] public PackedScene slow_effect;
	[Export] public Timer cast_timer;
	public StatusEffect slow;
	public int projectile_velocity = 25;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		slow = (StatusEffect)slow_effect.Instantiate();
		rotate_on_soft = true;
		rotate_on_held = true;
		rotate_on_soft_far = true;
		rotate_on_soft_close = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{

		
		
	}

	public override void Execute(Player player)
	{
		GD.Print("Execute projectile");
		state = States.not_queued;
		if(cast_timer.TimeLeft == 0)
		{
			cast_timer.Start();
			Vector3 collision = GetPlayerCollision(player); // Get the collision from the player to whats in front of it
			LaunchProjectile(player, collision); // Shoot projectile from cast point to the player collision point
		}
	}

    public override void FrameCheck(Player player)
    {
        if(CheckHeld()) // Check if button is held and only allow the player to rotate if it is
		{
			player.controllers.movement_controller.rotation_only = true;
			GD.Print("player can only rotate");
		}
		if(Input.IsActionJustReleased(assigned_button)) // Allow the player to move fully if the button is released
		{
			if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
			{
				EmitSignal(nameof(AbilityFinished),this);
			}
			
			player.controllers.movement_controller.rotation_only = false;
		}
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			EmitSignal(nameof(AbilityQueue),this);
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(cast_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue),this);
				EmitSignal(nameof(AbilityCheck),this);
			}		
		}
		if(cast_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}		
    }

    public virtual void LaunchProjectile(Player player, Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized(); // Get position the projectile needs to go to
		RangedHitbox projectile = (RangedHitbox)projectile_to_load.Instantiate(); // Instantiate the projectile
		
		player.exclude.Add(projectile.GetRid()); // Add the projectile to the players exclude array
		projectile.TreeExited  += () => RemoveFromExclusion(player, projectile.GetRid(), projectile); // When the projectile exits the scene remove it from the exclusion array 
		GetTree().Root.AddChild(projectile); // Add projectile to the scene
		projectile.GlobalPosition = player.cast_point.GlobalPosition; // give the projectile the cast point position
		projectile.GlobalRotation = player.GlobalRotation; // give the projectile the player rotation

		if(player.entity_systems.damage_system.Crit(player)) // check if the play will crit
		{
			projectile.damage = MathF.Round(player.damage * (1 + player.critical_hit_damage), 2); // Set projectile damage
			projectile.posture_damage = player.posture_damage / 3; // Set projectile posture damage 
			projectile.is_critical = true;
		}
		else
		{
			
			projectile.damage = player.damage; // Set projectile damage
			projectile.posture_damage = player.posture_damage / 3; // Set projectile posture damage 
			projectile.is_critical = false;
		}
		// Set projectile damage type
		projectile.LinearVelocity = cast_direction * projectile_velocity; // Set projectile velocity
		projectile.effects.Add(slow);
	}

	public void RemoveFromExclusion(Player player, Rid projectile_rid, RangedHitbox projectile) // Remove projectile from exclusion array
	{
		player.exclude.Remove(projectile_rid);
	}

	public void _on_cast_timer_timeout() // Remove ability from list and allow player movement
	{
		if(button_released)
		{
			EmitSignal(nameof(AbilityFinished),this);
		}
		
		stop_movement_input = false;
	}

}
