using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Ranged : Ability
{
	public int range = 100;
	public bool is_enemy;
	[Signal] public delegate void PlayerFacingEnemySignalEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerFacingEnemySignal += HandlePlayerFacingEnemy;
	}

    private void HandlePlayerFacingEnemy()
    {
        GD.Print("Player is facing enemy from signal");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		
	}

	public Vector3 GetPlayerCollision()
	{
		Vector3 ray_origin = player.GlobalTransform.Origin;
		Vector3 ray_end = ray_origin + -player.Transform.Basis.Z * range;
		var new_intersection = PhysicsRayQueryParameters3D.Create(ray_origin, ray_end);
		new_intersection.CollisionMask = 16;
		new_intersection.CollideWithAreas = true;
		new_intersection.Exclude = player.exclude;
		var intersection = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection);

		if(intersection.Count > 0)
		{
			Vector3 collision_point = intersection["position"].AsVector3();
			Node3D collider = (Node3D)intersection["collider"];
			if(collider is Hurtbox hurtbox)
			{
				if(hurtbox.Owner is Enemy)
				{
					is_enemy = true;
					// GD.Print("Collided with enemy");
				}
				
			}
			
			GD.Print("collided with" + collider.Name);
			
			return collision_point;
		}
		else
		{
			is_enemy = false;
			// GD.Print("did not collide with enemy");
			return ray_end;
		}
	}

	public void PlayerFacingEnemy()
	{
		GD.Print("Shooting raycast to see if player is facing enemy");
		if(!is_enemy)
		{
			GetPlayerCollision();
		}
		else
		{
			GetPlayerCollision();
			EmitSignal(nameof(PlayerFacingEnemySignal));
			GD.Print("Player is facing enemy");
		}
	}
}
