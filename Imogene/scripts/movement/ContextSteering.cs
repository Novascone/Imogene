using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

public partial class ContextSteering : CharacterBody3D
{

	[Export] public int max_speed = 4; // How fast the entity will move 
	[Export] public float steer_force = 0.02f; // How fast the entity turns
	[Export] public int look_ahead = 5; // How far the rays will project
	[Export] public int direction_lines_mag = 5;
	[Export] public int direction_line_mag = 7;
	[Export] public int num_rays = 16;

	Vector3 look_at_position;

	public Vector3[] ray_directions; // Directions the rays will be cast in
	public float[] interest; // Interest weight, how interested the entity is in moving toward a location
	public float[] danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it

	public Vector3 chosen_dir = Vector3.Zero; // Direction the entity has chosen
	public Vector3 velocity = Vector3.Zero;
	public Vector3 acceleration = Vector3.Zero;
	public Vector3 direction = Vector3.Zero;

	public NavigationAgent3D navigation_agent;
	public Vector3 target_position;
	public StateMachine state_machine;
	public Area3D detection_area;
	public Area3D herd_detection;
	public Vector3 box_position;
	public Vector3 center_position;
	public Vector3 interest_position;
	public Vector3 herd_mate_position;
	public bool can_see_center;
	public Node3D chaser;
	public Node3D herd_mate;
	public bool running_away_from_chaser;
	public bool near_herd_mate;


	public Node3D ray_position; // Position rays are cast from
	public Vector3 ray_origin;
	public MeshInstance3D collision_lines;
	public StandardMaterial3D collision_lines_material = new StandardMaterial3D();
	public MeshInstance3D ray_lines;
	public StandardMaterial3D ray_lines_material = new StandardMaterial3D();
	public MeshInstance3D direction_lines;
	public StandardMaterial3D direction_line_material = new StandardMaterial3D();
	public MeshInstance3D direction_moving_line;
	public StandardMaterial3D direction_moving_line_material = new StandardMaterial3D();


	Entity entity;

	[Export] public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export] public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
	float prev_y_rotation;
	float current_y_rotation;

	Vector2 blend_direction = Vector2.Zero;
	private AnimationTree tree;

	// ***************************** change this to state machine controlled **********************************
	public bool entity_in_detection;
	public bool switch_to_state2;
	public Node3D collider;

	public CustomSignals _customSignals; // Custom signal instance
	// *********************************************************************************************************
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ray_position = GetNode<Node3D>("RayPosition");
		tree = GetNode<AnimationTree>("AnimationTree");
		collision_lines = GetNode<MeshInstance3D>("CollisionLines");
		ray_lines = GetNode<MeshInstance3D>("RayLines");
		direction_lines = GetNode<MeshInstance3D>("DirectionLines");
		direction_moving_line = GetNode<MeshInstance3D>("DirectionMovingLine");

		navigation_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		state_machine = GetNode<StateMachine>("StateMachine");
		state_machine.GetEntityInfo(this);

		detection_area = GetNode<Area3D>("DetectionArea");
		detection_area.BodyEntered += OnDetectionBodyEntered;
		detection_area.AreaEntered += OnDetectionAreaEntered;
		detection_area.BodyExited += OnDetectionAreaExited;

		herd_detection = GetNode<Area3D>("HerdDetection");
		herd_detection.AreaEntered += OnHerdDetectionEntered;
		herd_detection.AreaExited += OnHerdDetectionExited;
		herd_detection.BodyEntered += OnHerdBodyEntered;

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

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.FinishedCircling += HandleFinishedCircling;
	}

    private void OnHerdBodyEntered(Node3D body)
    {
		
        if(body is ContextSteering herd_entity && body.Name != Name)
		{
			GD.Print(body.Name + " entered detection area of " + Name);
			GD.Print("Herd mate in detection");
			herd_mate = herd_entity;
		}
    }

    private void OnHerdDetectionExited(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Away from herd mate");
			near_herd_mate = false;
			
		}
    }

    private void OnHerdDetectionEntered(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Near herd mate");
			near_herd_mate = true;
			herd_mate_position = area.GlobalPosition;
		}
    }

    private void OnDetectionAreaEntered(Area3D area)
    {
        if(area.IsInGroup("RotateBox"))
		{
			GD.Print("In contact with rotate box");
			entity_in_detection = true;
			box_position = area.GlobalPosition;
		}
		if(area.IsInGroup("Center"))
		{
			GD.Print("within range of center");
			can_see_center = true;
			center_position = area.GlobalPosition with {Y = 0};
			GD.Print(center_position);
		}
		if(area.IsInGroup("InterestPoint"))
		{
			GD.Print("within range of interest position");
			interest_position = area.GlobalPosition with {Y = 0};
			GD.Print(interest_position);
		}
    }

    private void HandleFinishedCircling()
    {
        state_machine.current_state.Exit("State2");
    }



    private void OnDetectionBodyEntered(Node3D area)
    {
		GD.Print(area.Name + " entered detection area");

        
		if(area.IsInGroup("Center"))
		{
			GD.Print("within range of center");
			can_see_center = true;
			center_position = area.GlobalPosition;
			GD.Print(center_position);
		}
		if(area is Chaser chaser_entered )
		{
			GD.Print("Chaser in detection");
			chaser = chaser_entered;
		}
    }
    private void OnDetectionAreaExited(Node3D area)
    {
         if(area.IsInGroup("RotateBox"))
		{
			GD.Print("out of contact with rotate box");
			box_position = Vector3.Zero;
			entity_in_detection = false;
			
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

		if (Input.IsActionJustPressed("A"))
		{
			state_machine.current_state.Exit("State4");
		}
		if (Input.IsActionJustPressed("B"))
		{
			state_machine.current_state.Exit("State5");
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
			// GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(look_at_position);
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

		if(herd_mate_position != Vector3.Zero)
		{
			GD.Print("herd mate position " + herd_mate_position + " from " + Name);
		}

		SmoothRotation();
		LookAtOver();

    	// Moving the character
    
		// Populate interest arrays 
		// SetInterest();
		// // foreach(float i in interest)
		// // {
		// // 	GD.Print("interest " + i);
		// // }
		
		// SetDanger();
		// // foreach(float i in danger)
		// // {
		// // 	GD.Print("danger " + i);
		// // }
		
		// ChooseDirection();

		// GD.Print("chosen dir " + chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y));
		// GD.Print("Navigation point " + navigation_agent.GetNextPathPosition());
		// GD.Print("Box position " + box_position);

		// GD.Print("target position " + target_position);
		// if(collider != null)
		// {
		// 	GD.Print("collider position " + collider.GlobalPosition);
		// }

	
	
	
		
	
		// Movement
		// It is very important that the ray_directions, and the final chosen direction get rotated around the global Y axis by the Y rotation of the node they are attached to
		// If they don't get rotated then the direction the entity faces and moves will be incorrect
		

		// If there is a chaser and its distance from the entity is less than 5 set running away from chaser to true and increase speed
		// when the distance from the chaser is greater than 15 set running away from chaser to false and reduce speed
		if(chaser != null) 
		{
			if(GlobalPosition.DistanceTo(chaser.GlobalPosition) < 5)
			{
				running_away_from_chaser = true;
				max_speed = 8;
				
			}
			else if (GlobalPosition.DistanceTo(chaser.GlobalPosition) > 15)
			{
				running_away_from_chaser = false;
				max_speed = 4;
			
			}
			GD.Print("Distance from chaser " + GlobalPosition.DistanceTo(chaser.GlobalPosition));
		}
		if(herd_mate != null) 
		{
			herd_mate_position = herd_mate.GlobalPosition;
			// if(GlobalPosition.DistanceTo(herd_mate.GlobalPosition) < 5)
			// {
			// 	running_away_from_chaser = true;
			// 	max_speed = 8;
				
			// }
			// else if (GlobalPosition.DistanceTo(chaser.GlobalPosition) > 15)
			// {
			// 	running_away_from_chaser = false;
			// 	max_speed = 4;
			
			// }
			
		}
		
		
		// GD.Print("Velocity " + Velocity);
		// GD.Print("running away from chaser " + running_away_from_chaser);
		// GD.Print("can see center " + can_see_center);

		if(can_see_center && GlobalPosition.DistanceTo(center_position) < 2 && !running_away_from_chaser) // If the entity is under the center stop moving
		{
			GD.Print("Stop moving");
			_targetVelocity = Vector3.Zero;
			Velocity = _targetVelocity;
		}
		else
		{
			_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
			Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		}
		
		// _targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		// Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		if(Velocity > Vector3.Zero )
		{
			blend_direction.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			blend_direction.Y = 0;
		}
		
		
	
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();


		
		// var desired_velocity = chosen_dir.Rotated(entity.direction, ) * max_speed;

	}

	public void LookAtOver() // Look at enemy and switch
	{
		if(!can_see_center)
		{
			if(entity_in_detection)
			{
				
				// target_ability.Execute(this);
				look_at_position = box_position;
				LookAt(look_at_position with {Y = GlobalPosition.Y});
				
			}
		}
		
		else
		{

			entity_in_detection = false;
			// Sets the animation to walk forward when not targeting
			if(chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) != Vector3.Zero)
			{
				blend_direction.X = 0;
				blend_direction.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
			}
		}
	}

	public void SetInterest()
	{
		// *************** comment/uncomment for test behavior ***************

		// // ******** make state machine controlled **********
		if(state_machine.current_state.name == "State3")
		{
			
			SetObjectInterest();
		}
		else if (state_machine.current_state.name == "State2")
		{
			
			SetDefaultInterest();
		}

		// ******************************************************************************************


		// *************** comment/uncomment for base behavior ***************
		// if(state_machine.current_state.name == "State2")
		// {
		// 	navigation_agent.TargetPosition = Vector3.Forward; 
		// 	target_position = navigation_agent.GetNextPathPosition();
		// 	SetDefaultInterest();
		// }
		

		// ***************************************************************************
	}

	public void SetObjectInterest()
	{
			navigation_agent.TargetPosition = box_position; 
			target_position = navigation_agent.GetNextPathPosition();

			for(int i = 0; i < num_rays; i++)
			{
				// GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

				// Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
				// (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
				var d = ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(Transform.Basis.X + GlobalPosition.DirectionTo(target_position)); 
				// If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
				interest[i] = MathF.Max(0, d);

				// GD.Print(interest[i]);
			}
	}

	public void SetDefaultInterest()
	{
		// navigation_agent.TargetPosition = Vector3.Forward; 
		// target_position = navigation_agent.GetNextPathPosition();

		for(int i = 0; i < num_rays; i++)
		{
			// Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move (in this base case forward which is Transform.Basis.Z)
			var d = ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(target_position.Normalized());
			// If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
			// GD.Print("d " + d);
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

			// *************** Comment/ Uncomment for test behavior ***************
			if(result.Count > 0)
			{
				collider = (Node3D)result["collider"];
				SetCollisionLines(collision_lines, result);
				danger[i] = 1.0f;
				// if(i == 0)
				// {
				// 	danger[i + 1] = 0.5f;
				// 	danger[i + 2] = 0.5f;
				// }
				// else if(i == num_rays - 1)
				// {
				// 	danger[i - 1] = 0.5f;
				// 	danger[i - 2] = 0.5f;
				// }
				// else
				// {
				// 	danger[i - 1] = 0.5f;
				// 	danger[i + 1] = 0.5f;
				// }
			
				
			}
			else
			{
				danger[i] = 0;
			}
		
			
			

			// ******************************************************************************************

			// *************** Comment/ Uncomment for base behavior ***************
			// If the ray has collided
			
			// if(result.Count > 0)
			// {
				
	
			// 	// Set danger in the direction of the ray cast to 1
			// 	danger[i] = 1.0f;
			// 	// Uncomment to show ray casts when they collide
			// 	SetCollisionLines(collision_lines, result);
				
			// }
			// else
			// {
			// 	danger[i] = 0.0f;
			// }

			// ***************************************************************************
		}
		
	}

	public void ChooseDirection()
	{
		for(int i = 0; i < num_rays; i++)
		{
			// If there is danger where the ray was cast, set the interest to zero
			// Need to change this to make the changing direction more versatile
			if(danger[i] > 0.5f)
			{
				interest[i] = 0.0f;
			}
		}

		chosen_dir = Vector3.Zero;
		
		for(int i = 0; i < num_rays; i++)
		{

			// Sum up all of the directions where there is interest (if the interest is zero at a given direction that direction will not factor into the chosen direction)
			chosen_dir += ray_directions[i] * interest[i];
			// GD.Print("directions: " + ray_directions[i] * interest[i]);
			// GD.Print("Interest[i] " + interest[i]);

			// Uncomment to show lines the represent the weight of the directions the entity can move in
			SetDirectionLines(direction_lines, ray_directions[i] * interest[i] * direction_lines_mag);
		}

		// Normalize the chosen direction
		chosen_dir = chosen_dir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		SetDirectionMovingLine(direction_moving_line, chosen_dir * direction_line_mag);

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
		if(!entity_in_detection)
		{
			prev_y_rotation = GlobalRotation.Y;
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y))) // looks at direction the player is moving
			{
				LookAt(GlobalPosition + chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y));
				
			}
			current_y_rotation = GlobalRotation.Y;
			if(prev_y_rotation != current_y_rotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}
	


}
