using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class RangedAbility : Ability
{
	public int range = 100;
	public bool is_enemy;

	public Vector3 GetPlayerCollision(Player player) // Gets the collision point of the player and the object in front of the player, or the end of the ray if there is no collision
	{
		Vector3 ray_origin = player.GlobalTransform.Origin; // Set ray origin
		Vector3 ray_end = ray_origin + -player.Transform.Basis.Z * range; // Set ray end
		var new_intersection = PhysicsRayQueryParameters3D.Create(ray_origin, ray_end); // Create a raycast
		new_intersection.CollisionMask = 16; // Set the collision mask of the raycast 16 -> mask 5
		new_intersection.CollideWithAreas = true; // Set the raycast to collide with areas
		new_intersection.Exclude = player.exclude; // Add the player exclude to the array of exclusions
		var intersection = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection); // Get raycast intersection

		if(intersection.Count > 0)
		{
			Vector3 collision_point = intersection["position"].AsVector3();
			Node3D collider = (Node3D)intersection["collider"];
			if(collider is Hurtbox hurtbox) // Detect if the collider is an enemies hurtbox
			{
				if(hurtbox.Owner is Enemy)
				{
					is_enemy = true;
				}
				
			}
			return collision_point;
		}
		else
		{
			is_enemy = false;
			return ray_end;
		}
	}
}
