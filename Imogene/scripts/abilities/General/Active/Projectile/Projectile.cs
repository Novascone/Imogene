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
		projectile_to_load = GD.Load<PackedScene>("res://scripts/abilities/General/Active/Projectile/projectile_to_load.tscn");
		cast_timer = GetNode<Timer>("CastTimer");
		rotate_on_soft = true;
		rotate_on_held = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{

		if(CheckHeld()) // Check if button is held and only allow the player to rotate if it is
		{
			player.movement_controller.rotation_only = true;
		}
		if(Input.IsActionJustReleased(assigned_button)) // Allow the player to move fully if the button is released
		{
			player.movement_controller.rotation_only = false;
		}

		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			QueueAbility();	
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(cast_timer.TimeLeft == 0)
			{
				QueueAbility();
				CheckCanUseAbility();
			}		
		}
		if(cast_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			CheckCanUseAbility();
		}		
		
	}

	public override void Execute()
	{
		button_pressed = false;
		state = States.not_queued;
		player.movement_controller.movement_input_allowed = false; // disable player movement
		AddToAbilityList(this); // Add ability to list
		cast_timer.Start();
		Vector3 collision = GetPlayerCollision(); // Get the collision from the player to whats in front of it
		LaunchProjectile(collision); // Shoot projectile from cast point to the player collision point
		

	}

	public void LaunchProjectile(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized(); // Get position the projectile needs to go to
		RangedHitbox projectile = (RangedHitbox)projectile_to_load.Instantiate(); // Instantiate the projectile
		
		player.exclude.Add(projectile.GetRid()); // Add the projectile to the players exclude array
		projectile.TreeExited  += () => RemoveFromExclusion(projectile.GetRid(), projectile); // When the projectile exits the scene remove it from the exclusion array 
		GetTree().Root.AddChild(projectile); // Add projectile to the scene
		projectile.GlobalPosition = player.cast_point.GlobalPosition; // give the projectile the cast point position
		projectile.GlobalRotation = player.GlobalRotation; // give the projectile the player rotation

		if(player.damage_system.Crit()) // check if the play will crit
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
		projectile.damage_type = "cold"; // Set projectile damage type
		projectile.LinearVelocity = cast_direction * projectile_velocity; // Set projectile velocity
	}

	public void RemoveFromExclusion(Rid projectile_rid, RangedHitbox projectile) // Remove projectile from exclusion array
	{
		player.exclude.Remove(projectile_rid);
	}

	public void _on_cast_timer_timeout() // Remove ability from list and allow player movement
	{
		RemoveFromAbilityList(this);
		player.movement_controller.movement_input_allowed = true;
	}

}
