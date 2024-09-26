using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class DirectionalRayCast : Node3D
{
	[Export] public TargetingRaycast raycast_1;
	[Export] public TargetingRaycast raycast_2;
	[Export] public TargetingRaycast raycast_3;
	[Export] public TargetingRaycast raycast_4;
	[Export] public TargetingRaycast raycast_5;
	[Export] public TargetingRaycast raycast_6;
	[Export] public TargetingRaycast raycast_7;
	[Export] public TargetingRaycast raycast_8;
	[Export] public TargetingRaycast raycast_9;
	
	[Signal] public delegate void RemoveSoftTargetIconEventHandler(Enemy enemy);

	Vector3 position = Vector3.Zero;
	public float previous_y_rotation;
	public float current_y_rotation;
	
	public Vector3 ray_cast_input;
	public int range = 100;
	public bool enemy_hit;
	public Node3D collider;
	public Vector3 ray_origin;
	public Vector2 raw_vector;
	public Vector2 deadzoned_vector;
	public float input_strength;
	public float deadzone = 0.25f;
	// public List<Enemy> enemies = new List<Enemy>();
	public Dictionary<Enemy,int> enemies = new Dictionary<Enemy, int>();




    public void SetTargetingRayCastDirection()
	{
		ray_cast_input.X = 0.0f;
		ray_cast_input.Z = 0.0f;

		// if (Input.IsActionPressed("Right"))
		// {
		// 	ray_cast_input.Z -= 0.3f;
		// 	ray_cast_input.X -= 0.7f;
		// }
		// if (Input.IsActionPressed("Left"))
		// {
		// 	ray_cast_input.Z += 0.3f;
		// 	ray_cast_input.X += 0.7f;
		// }
		// if (Input.IsActionPressed("Backward"))
		// {
		// 	ray_cast_input.Z -= 0.6f;
		// 	ray_cast_input.X += 0.4f;

		// }
		// if (Input.IsActionPressed("Forward"))
		// {
		// 	ray_cast_input.Z += 0.6f;
		// 	ray_cast_input.X -= 0.4f;
		// }
		
		// ray_cast_input = ray_cast_input.Normalized();
		

		raw_vector = Input.GetVector("Left", "Right", "Forward", "Backward");
		input_strength = Input.GetActionStrength("Right") + Input.GetActionStrength("Forward") + Input.GetActionStrength("Left") + Input.GetActionStrength("Backward");
		deadzoned_vector = raw_vector;
		if(deadzoned_vector.Length() < deadzone)
		{
			
			deadzoned_vector = Vector2.Zero;
		}
		else
		{
			
			deadzoned_vector = deadzoned_vector.Normalized() * ((deadzoned_vector.Length() - deadzone) / (1 - deadzone));
		}
		ray_cast_input.X = -deadzoned_vector.X;
		ray_cast_input.Z = -deadzoned_vector.Y;
		// if(ray_cast_input != Vector3.Zero)
		// {
		// 	GD.Print("Ray cast Direction: " + ray_cast_input);
		// }
		
	}
   


    public void GetRayCastCollisions()
	{
		// if(ray_cast_input != Vector3.Zero)
		// {
		foreach(Node3D node3D in GetChildren())
		{
			foreach(TargetingRaycast ray_cast in node3D.GetChildren())
			{
				if (ray_cast.IsColliding() && ray_cast.enemy == null)
				{
					ray_cast.enemy = (Enemy)ray_cast.GetCollider();
					if(!enemies.ContainsKey(ray_cast.enemy))
					{
						enemies.Add(ray_cast.enemy, 1);
						// GD.Print("Enemy not in  dictionary");
					}
					else
					{
						enemies[ray_cast.enemy] += 1;
						// GD.Print("Enemy in  dictionary adding one to its value");
					}
					// GD.Print("Enemies in dictionary " + ray_cast.enemy.Name +  " " + enemies[ray_cast.enemy]);
				}
				else if (!ray_cast.IsColliding() && ray_cast.enemy != null)
				{
					// GD.Print("Decrementing enemy");
					enemies[ray_cast.enemy] -= 1;
					// GD.Print("Enemies in dictionary " + ray_cast.enemy.Name +  " " + enemies[ray_cast.enemy]);
					if(enemies[ray_cast.enemy] == 0)
					{
						// GD.Print("Removing enemy");
						enemies.Remove(ray_cast.enemy);
						EmitSignal(nameof(RemoveSoftTargetIcon), ray_cast.enemy);
					}
					
					ray_cast.enemy = null;
				}
			}
		}
			
		// }
		
	}

	

	public Enemy GetEnemyWithMostCollisions()
	{
		if(enemies.Count > 0)
		{
			// GD.Print("Enemy with the most collisions " + enemies.MaxBy(entry => entry.Value).Key.Name + " collisions: " + enemies.MaxBy(entry => entry.Value).Value);
			return enemies.MaxBy(entry => entry.Value).Key;
		}
		return null;
	}

	public void FollowPlayer(Player player)
	{
		position = player.GlobalPosition;
		GlobalPosition = position;
		LookForward(ray_cast_input);
	}

	public void LookForward(Vector3 direction) // Rotates the player character smoothly with lerp
	{
		// GD.Print("Rotating smoothly");
	
		previous_y_rotation = GlobalRotation.Y;
		if (GlobalTransform.Origin != GlobalPosition + ray_cast_input with {Y = 0}) // looks at direction the player is moving
		{
			LookAt(GlobalPosition + ray_cast_input with { Y = 0 });
		}
		current_y_rotation = GlobalRotation.Y;
		if(previous_y_rotation != current_y_rotation)
		{
			GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(previous_y_rotation, current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
		
	}



	// public Vector3 GetRayCastCollision(Player player) // Gets the collision point of the player and the object in front of the player, or the end of the ray if there is no collision
	// {
	// 	Vector3 ray_origin = player.GlobalTransform.Origin with {Y = player.GlobalPosition.Y}; // Set ray origin
	// 	Vector3 ray_end = ray_origin + -player.Transform.Basis.Z * range; // Set ray end
	// 	var new_intersection = PhysicsRayQueryParameters3D.Create(ray_origin, ray_end); // Create a raycast
	// 	new_intersection.CollisionMask = 16; // Set the collision mask of the raycast 16 -> mask 5
	// 	new_intersection.CollideWithAreas = true; // Set the raycast to collide with areas
	// 	new_intersection.Exclude = player.exclude; // Add the player exclude to the array of exclusions
	// 	var intersection = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection); // Get raycast intersection

	// 	if(intersection.Count > 0)
	// 	{
	// 		Vector3 collision_point = intersection["position"].AsVector3();
	// 		Node3D collider = (Node3D)intersection["collider"];
	// 		if(collider is Hurtbox hurtbox) // Detect if the collider is an enemies hurtbox
	// 		{
	// 			if(hurtbox.Owner is Enemy)
	// 			{
	// 				enemy_hit = true;
	// 				GD.Print("Directional raycast hit " + hurtbox.Owner.Name);
	// 			}
				
	// 		}
	// 		return collision_point;
	// 	}
	// 	else
	// 	{
	// 		enemy_hit = false;
	// 		GD.Print("Directional raycast hit nothing");
	// 		return ray_end;
	// 	}
	// }
	
}
