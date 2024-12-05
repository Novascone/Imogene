using Godot;
using System;

public partial class Projectile : RangedAbility
{

	[Export] public PackedScene projectile_to_load;
	
	
	public int projectile_velocity = 25;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		rotate_on_soft = true;
		rotate_on_held = true;
		DamageModifier = 1;
    }


	public override void Execute(Player player)
	{
		base.Execute(player);
		GD.Print("Execute projectile");
		
		
		if(use_timer.TimeLeft == 0)
		{
			use_timer.Start();
			Vector3 collision = GetRayCastCollision(player); // Get the collision from the player to whats in front of it
			LaunchProjectile(player, collision); // Shoot projectile from cast point to the player collision point
		}
	}

   

    public virtual void LaunchProjectile(Player player, Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.CastPoint.GlobalTransform.Origin).Normalized(); // Get position the projectile needs to go to
		RangedHitbox projectile = (RangedHitbox)projectile_to_load.Instantiate(); // Instantiate the projectile
		
		player.ExcludedRIDs.Add(projectile.GetRid()); // Add the projectile to the players exclude array
		projectile.TreeExited  += () => RemoveFromExclusion(player, projectile.GetRid(), projectile); // When the projectile exits the scene remove it from the exclusion array 
		GetTree().Root.AddChild(projectile); // Add projectile to the scene
		projectile.GlobalPosition = player.CastPoint.GlobalPosition; // give the projectile the cast point position
		projectile.GlobalRotation = player.GlobalRotation; // give the projectile the player rotation
		
		// Set projectile damage type
		projectile.LinearVelocity = cast_direction * projectile_velocity; // Set projectile velocity
		Slow slow = new();
		projectile.effects.Add(slow);
		ranged_hitbox = projectile;
		DealDamage(player, DamageModifier);
	}

	public void RemoveFromExclusion(Player player, Rid projectile_rid, RangedHitbox projectile) // Remove projectile from exclusion array
	{
		player.ExcludedRIDs.Remove(projectile_rid);
	}

	public void _on_cast_timer_timeout() // Remove ability from list and allow player movement
	{
		if(button_released && !ability_finished)
		{
			EmitSignal(nameof(AbilityFinished),this);
			ability_finished = true;
		}
		EmitSignal(nameof(AbilityReleaseInputControl),this);
		
	}

}
