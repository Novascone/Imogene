using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

public partial class ContextSteering : CharacterBody3D
{
	[Export] int max_speed = 4; // How fast the entity will move 
	[Export] float steer_force = 0.1f; // How fast the entity turns
	[Export] int look_ahead = 10; // How far the rays will project
	[Export] int num_rays = 16;

	Vector3[] ray_directions; // Directions the rays will be cast in
	float[] interest; // Interest weight, how interested the entity is in moving toward a location
	float[] danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it

	Vector3 chosen_dir = Vector3.Zero; // Direction the entity has chosen
	Vector3 velocity = Vector3.Zero;
	Vector3 acceleration = Vector3.Zero;
	Vector3 direction = Vector3.Zero;


	Node3D ray_position; // Position rays are cast from
	Vector3 ray_origin;
	MeshInstance3D collision_lines;
	StandardMaterial3D collision_lines_material = new StandardMaterial3D();
	MeshInstance3D ray_lines;
	StandardMaterial3D ray_lines_material = new StandardMaterial3D();
	MeshInstance3D direction_lines;
	StandardMaterial3D direction_line_material = new StandardMaterial3D();
	MeshInstance3D direction_moving_line;
	StandardMaterial3D direction_moving_line_material = new StandardMaterial3D();


	Entity entity;

	 [Export] public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export] public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
	float prev_y_rotation;
	float current_y_rotation;

	Vector2 blend_direction = Vector2.Zero;
	private AnimationTree tree;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ray_position = GetNode<Node3D>("RayPosition");
		tree = GetNode<AnimationTree>("AnimationTree");
		collision_lines = GetNode<MeshInstance3D>("CollisionLines");
		ray_lines = GetNode<MeshInstance3D>("RayLines");
		direction_lines = GetNode<MeshInstance3D>("DirectionLines");
		direction_moving_line = GetNode<MeshInstance3D>("DirectionMovingLine");

		// Resize arrays to given number of arrays
		Array.Resize(ref interest, num_rays);
		Array.Resize(ref danger, num_rays);
		Array.Resize(ref ray_directions, num_rays);
		
		// Get the angles that the ray casts will be emitted, in this case a circle, and populate the directions array
		for( int i = 0; i < num_rays; i++)
		{
			float angle = i * 2 * MathF.PI / num_rays; // <-- circle divided into number of rays
			ray_directions[i] = Vector3.Forward.Rotated(GlobalTransform.Basis.Y.Normalized(), angle); // <-- set the ray directions
			GD.Print(ray_directions[i]);
		}
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		ray_origin = ray_position.GlobalPosition;
		var direction = Vector3.Zero;
		if(collision_lines.Mesh is ImmediateMesh collision_lines_mesh)
		{
			collision_lines_mesh.ClearSurfaces();
		}
		if(ray_lines.Mesh is ImmediateMesh ray_lines_mesh)
		{
			ray_lines_mesh.ClearSurfaces();
		}
		if(direction_lines.Mesh is ImmediateMesh direction_lines_mesh)
		{
			direction_lines_mesh.ClearSurfaces();
		}
		if(direction_moving_line.Mesh is ImmediateMesh direction_moving_line_mesh)
		{
			direction_moving_line_mesh.ClearSurfaces();
		}


		// We check for each move input and update the direction accordingly.
		if (Input.IsActionPressed("Right"))
		{
			direction.X -= 1.0f;		
		}
		if (Input.IsActionPressed("Left"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("Backward"))
		{
			direction.Z -= 1.0f;
		}
		if (Input.IsActionPressed("Forward"))
		{
			direction.Z += 1.0f;
		}
		// direction.Z -= 1;
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			// Setting the basis property will affect the rotation of the node.
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}

		if (direction != Vector3.Zero)
		{
			// ...
		}

		// Ground velocity
		// _targetVelocity.X = direction.X * Speed;
		// _targetVelocity.Z = direction.Z * Speed;

		// Vertical velocity
		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

    	// Moving the character
    
		// Populate interest arrays 
		SetInterest();
		// foreach(float i in interest)
		// {
		// 	GD.Print("interest " + i);
		// }
		
		SetDanger();
		// foreach(float i in danger)
		// {
		// 	GD.Print("danger " + i);
		// }
		
		ChooseDirection();
		
		GD.Print("direction  " + chosen_dir);
		// GD.Print("chosen dir " + chosen_dir);
		// Movement
		_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		if(Velocity > Vector3.Zero)
		{
			blend_direction.Y = 1; // Sets animation to walk
		}	
		SmoothRotation();
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();


		
		// var desired_velocity = chosen_dir.Rotated(entity.direction, ) * max_speed;

	}

	public void SetInterest()
	{
		for(int i = 0; i < num_rays; i++)
		{
			// GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

			// Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move (in this base case forward which is Transform.Basis.Z)
			var d = ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(Transform.Basis.Z);

			// GD.Print("Equals " + d);
			// GD.Print("d " + d);

			// If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
			interest[i] = MathF.Max(0, d);

			// GD.Print(interest[i]);
		}
	}

	public void SetDanger()
	{
		// Create a space state to cast rays
		var space_state = GetWorld3D().DirectSpaceState;
		for(int i = 0; i < num_rays; i++)
		{
			// Cast a ray from the ray origin, in the ray direction(rotated with player .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) with a magnitude of our look_ahead variable
			var ray_query = PhysicsRayQueryParameters3D.Create(ray_origin, ray_origin + ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * look_ahead);
			var ray_target = ray_origin + ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * look_ahead; // Used in SetRayCastLines
			var result = space_state.IntersectRay(ray_query); // Result dictionary from the ray cast

			// Uncomment to show ray casts before collision
			// SetRayCastLines(ray_lines,ray_target);

			// GD.Print("Result " + result);

			// If the ray has collided
			if(result.Count > 0)
			{
				// Set danger in the direction of the ray cast to 1
				danger[i] = 1.0f;
				// Uncomment to show ray casts when they collide
				SetCollisionLines(collision_lines, result);
				
			}
			else
			{
				danger[i] = 0.0f;
			}
			// GD.Print("Danger i: " + i + " " + danger[i]);
		}
		
	}

	public void ChooseDirection()
	{
		for(int i = 0; i < num_rays; i++)
		{
			// If there is danger where the ray was cast, set the interest to zero
			if(danger[i] > 0.0)
			{
				interest[i] = 0.0f;
			}
		}

		chosen_dir = Vector3.Zero;
		
		for(int i = 0; i < num_rays; i++)
		{

			// Sum up all of the directions where there is interest (if the interest is zero at a given direction that direction will not factor into the chosen direction)
			chosen_dir += ray_directions[i] * interest[i];
			GD.Print("directions: " + ray_directions[i] * interest[i]);

			// Uncomment to show lines the represent the weight of the directions the entity can move in
			SetDirectionLines(direction_lines, (ray_directions[i] * interest[i]) * look_ahead);
		}

		// Normalize the chosen direction
		chosen_dir = chosen_dir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		SetDirectionMovingLine(direction_moving_line, chosen_dir * look_ahead);

		// GD.Print("chosen dir " + chosen_dir);
	}

	// Lines representing where the ray cast is emitted from and to
	public void SetRayCastLines(MeshInstance3D meshInstance3D, Vector3 ray_target)
	{
		if(meshInstance3D.Mesh is ImmediateMesh ray_lines_mesh)
		{
			ray_lines_mesh.SurfaceBegin(Mesh.PrimitiveType.Lines, ray_lines_material);
			ray_lines_material.EmissionEnabled = true;
			ray_lines_mesh.SurfaceAddVertex(ToLocal(ray_origin));
			ray_lines_mesh.SurfaceAddVertex(ToLocal(ray_target));
			ray_lines_mesh.SurfaceEnd();
		}
			
	}

	// Lines representing when the ray cast makes contact with an object
	public void SetCollisionLines(MeshInstance3D meshInstance3D, Godot.Collections.Dictionary result)
	{
		if(meshInstance3D.Mesh is ImmediateMesh collision_lines_mesh)
		{
			
			collision_lines_mesh.SurfaceBegin(Mesh.PrimitiveType.Lines, collision_lines_material);
			collision_lines_material.EmissionEnabled = true;
			collision_lines_material.Emission = Colors.Red;
			collision_lines_material.AlbedoColor = Colors.Red;
			collision_lines_mesh.SurfaceAddVertex(ToLocal(ray_origin));
			collision_lines_mesh.SurfaceAddVertex(ToLocal(result["position"].AsVector3()));
			collision_lines_mesh.SurfaceEnd();
		}
	}

	// Lines representing the weight of which direction the entity will move
	public void SetDirectionLines(MeshInstance3D meshInstance3D, Vector3 directions)
	{
		if(meshInstance3D.Mesh is ImmediateMesh direction_lines_mesh)
		{
			direction_lines_mesh.SurfaceBegin(Mesh.PrimitiveType.Lines, direction_line_material);
			direction_line_material.EmissionEnabled = true;
			direction_line_material.Emission = Colors.Yellow;
			direction_line_material.AlbedoColor = Colors.Yellow;
			direction_lines_mesh.SurfaceAddVertex(ToLocal(ray_origin));
			direction_lines_mesh.SurfaceAddVertex(ToLocal(ray_origin + directions.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			direction_lines_mesh.SurfaceEnd();
		}
	}

	// Line representing the direction the entity is moving
	public void SetDirectionMovingLine(MeshInstance3D meshInstance3D, Vector3 direction)
	{
		if(meshInstance3D.Mesh is ImmediateMesh direction_moving_line_mesh)
		{
			direction_moving_line_mesh.SurfaceBegin(Mesh.PrimitiveType.Lines, direction_moving_line_material);
			direction_moving_line_material.EmissionEnabled = true;
			direction_moving_line_material.Emission = Colors.Green;
			direction_moving_line_material.AlbedoColor = Colors.Green;
			direction_moving_line_mesh.SurfaceAddVertex(ToLocal(ray_origin));
			direction_moving_line_mesh.SurfaceAddVertex(ToLocal(ray_origin + direction.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			direction_moving_line_mesh.SurfaceEnd();
		}
	}

	// Rotate the entity smoothly in the direction it is looking
	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
		prev_y_rotation = GlobalRotation.Y;
		if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + chosen_dir)) // looks at direction the player is moving
		{
			LookAt(GlobalPosition + chosen_dir);
		}
		current_y_rotation = GlobalRotation.Y;
		if(prev_y_rotation != current_y_rotation)
		{
			GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.05f)}; // smoothly rotates between the previous angle and the new angle!
		}
	}
	


}
