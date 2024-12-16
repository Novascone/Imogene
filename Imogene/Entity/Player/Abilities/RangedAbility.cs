using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class RangedAbility : Ability
{
	public int Range { get; set; } = 100;

    public Vector3 GetRayCastCollision(Player player) // Gets the collision point of the player and the object in front of the player, or the end of the ray if there is no collision
	{
		Vector3 rayOrigin = player.GlobalTransform.Origin; // Set ray origin
		Vector3 rayEnd = rayOrigin + -player.Transform.Basis.Z * Range; // Set ray end
		var newIntersection = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd); // Create a raycast
		newIntersection.CollisionMask = 16; // Set the collision mask of the raycast 16 -> mask 5
		newIntersection.CollideWithAreas = true; // Set the raycast to collide with areas
		newIntersection.Exclude = player.ExcludedRIDs; // Add the player exclude to the array of exclusions
		var intersection = GetWorld3D().DirectSpaceState.IntersectRay(newIntersection); // Get raycast intersection

		if(intersection.Count > 0)
		{
			Vector3 collisionPoint = intersection["position"].AsVector3();
			return collisionPoint;
		}
		else
		{
			return rayEnd;
		}
	}
}
