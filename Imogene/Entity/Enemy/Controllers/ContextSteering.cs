using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection.Metadata;

// *********************************************** This is a preliminary script that is being adapted into the Enemy class ************************************************************************************

public partial class ContextSteering : CharacterBody3D
{

	[Export] public int MaxSpeed = 4; // How fast the entity will move 
	[Export] public float SteerForce = 0.02f; // How fast the entity turns
	[Export] public int LookAhead = 5; // How far the rays will project
	[Export] public int DirectionLinesMag = 5;
	[Export] public int MovingLineMag = 7;
	[Export] public int NumRays = 16;

	Vector3 LookAtPosition;

	public Vector3[] RayDirections; // Directions the rays will be cast in
	public float[] Interest; // Interest weight, how interested the entity is in moving toward a location
	public float[] Danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it

	public Vector3 ChosenDir = Vector3.Zero; // Direction the entity has chosen
	public Vector3 VelocityVector = Vector3.Zero;
	public Vector3 AccelerationVector = Vector3.Zero;
	public Vector3 DirectionVector = Vector3.Zero;

	public NavigationAgent3D NavigationAgent;
	public Vector3 TargetPosition;
	public StateMachine ContextStateMachine;
	public Area3D DetectionArea;
	public Area3D HerdDetection;
	public Vector3 BoxPosition;
	public Vector3 CenterPosition;
	public Vector3 InterestPosition;
	public Vector3 HerdMatePosition;
	public bool CanSeeCenter;
	public Node3D Chaser;
	public Node3D HerdMate;
	public bool RunningAwayFromChaser;
	public bool NearHerdMate;


	public Node3D RayPosition; // Position rays are cast from
	public Vector3 RayOrigin;
	public MeshInstance3D CollisionLines;
	public StandardMaterial3D CollisionLinesMaterial = new();
	public MeshInstance3D RayLines;
	public StandardMaterial3D RayLinesMaterial = new();
	public MeshInstance3D DirectionLines;
	public StandardMaterial3D DirectionLinesMaterial = new();
	public MeshInstance3D MovingLine;
	public StandardMaterial3D MovingLineMaterial = new();

	public string Type = "ContextTester";


	Entity Entity;

	[Export] public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export] public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
	float PreviousYRotation;
	float CurrentYRotation;

	Vector2 BlendDirection = Vector2.Zero;
	private AnimationTree Tree;

	// ***************************** change this to state machine controlled **********************************
	public bool EntityInDetection;
	public bool SwitchToState2;
	public Node3D Collider;

	public CustomSignals _customSignals; // Custom signal instance
	// *********************************************************************************************************
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RayPosition = GetNode<Node3D>("RayPosition");
		Tree = GetNode<AnimationTree>("AnimationTree");
		CollisionLines = GetNode<MeshInstance3D>("CollisionLines");
		RayLines = GetNode<MeshInstance3D>("RayLines");
		DirectionLines = GetNode<MeshInstance3D>("DirectionLines");
		MovingLine = GetNode<MeshInstance3D>("DirectionMovingLine");

		NavigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		ContextStateMachine = GetNode<StateMachine>("StateMachine");
		// state_machine.GetEntityInfoContextTest(this);

		DetectionArea = GetNode<Area3D>("DetectionArea");
		DetectionArea.BodyEntered += OnDetectionBodyEntered;
		DetectionArea.AreaEntered += OnDetectionAreaEntered;
		DetectionArea.BodyExited += OnDetectionAreaExited;

		HerdDetection = GetNode<Area3D>("HerdDetection");
		HerdDetection.AreaEntered += OnHerdDetectionEntered;
		HerdDetection.AreaExited += OnHerdDetectionExited;
		HerdDetection.BodyEntered += OnHerdBodyEntered;

		// Resize arrays to given number of arrays
		Array.Resize(ref Interest, NumRays);
		
		Array.Resize(ref Danger, NumRays);
		Array.Resize(ref RayDirections, NumRays);
		
		// Get the angles that the ray casts will be emitted, in this case a circle, and populate the directions array
		for( int i = 0; i < NumRays; i++)
		{
			float angle = i * 2 * MathF.PI / NumRays; // <-- circle divided into number of rays
			RayDirections[i] = Vector3.Forward.Rotated(GlobalTransform.Basis.Y.Normalized(), angle); // <-- set the ray directions
			// GD.Print(ray_directions[i]);
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
			HerdMate = herd_entity;
		}
    }

    private void OnHerdDetectionExited(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Away from herd mate");
			NearHerdMate = false;
			
		}
    }

    private void OnHerdDetectionEntered(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Near herd mate");
			NearHerdMate = true;
			HerdMatePosition = area.GlobalPosition;
		}
    }

    private void OnDetectionAreaEntered(Area3D area)
    {
        if(area.IsInGroup("RotateBox"))
		{
			GD.Print("In contact with rotate box");
			EntityInDetection = true;
			BoxPosition = area.GlobalPosition;
		}
		if(area.IsInGroup("Center"))
		{
			GD.Print("within range of center");
			CanSeeCenter = true;
			CenterPosition = area.GlobalPosition with {Y = 0};
			GD.Print(CenterPosition);
		}
		if(area.IsInGroup("InterestPoint"))
		{
			GD.Print("within range of interest position");
			InterestPosition = area.GlobalPosition with {Y = 0};
			GD.Print(InterestPosition);
		}
    }

    private void HandleFinishedCircling()
    {
        ContextStateMachine.CurrentState.Exit("State2");
    }



    private void OnDetectionBodyEntered(Node3D area)
    {
		GD.Print(area.Name + " entered detection area");

        
		if(area.IsInGroup("Center"))
		{
			GD.Print("within range of center");
			CanSeeCenter = true;
			CenterPosition = area.GlobalPosition;
			GD.Print(CenterPosition);
		}
		if(area is Chaser chaser_entered )
		{
			GD.Print("Chaser in detection");
			Chaser = chaser_entered;
		}
    }
    private void OnDetectionAreaExited(Node3D area)
    {
         if(area.IsInGroup("RotateBox"))
		{
			GD.Print("out of contact with rotate box");
			BoxPosition = Vector3.Zero;
			EntityInDetection = false;
			
		}
		

		
		
    }

    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		
		RayOrigin = RayPosition.GlobalPosition;
		var direction = Vector3.Zero;
		if(CollisionLines.Mesh is ImmediateMesh collisionLinesMesh)
		{
			collisionLinesMesh.ClearSurfaces();
		}
		if(RayLines.Mesh is ImmediateMesh rayLinesMesh)
		{
			rayLinesMesh.ClearSurfaces();
		}
		if(DirectionLines.Mesh is ImmediateMesh directionLinesMesh)
		{
			directionLinesMesh.ClearSurfaces();
		}
		if(MovingLine.Mesh is ImmediateMesh movingLineMesh)
		{
			movingLineMesh.ClearSurfaces();
		}

		if (Input.IsActionJustPressed("A"))
		{
			ContextStateMachine.CurrentState.Exit("State4");
		}
		if (Input.IsActionJustPressed("B"))
		{
			ContextStateMachine.CurrentState.Exit("State5");
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

		if(HerdMatePosition != Vector3.Zero)
		{
			GD.Print("herd mate position " + HerdMatePosition + " from " + Name);
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
		if(Chaser != null) 
		{
			if(GlobalPosition.DistanceTo(Chaser.GlobalPosition) < 5)
			{
				RunningAwayFromChaser = true;
				MaxSpeed = 8;
				
			}
			else if (GlobalPosition.DistanceTo(Chaser.GlobalPosition) > 15)
			{
				RunningAwayFromChaser = false;
				MaxSpeed = 4;
			
			}
			GD.Print("Distance from chaser " + GlobalPosition.DistanceTo(Chaser.GlobalPosition));
		}
		if(HerdMate != null) 
		{
			HerdMatePosition = HerdMate.GlobalPosition;
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

		if(CanSeeCenter && GlobalPosition.DistanceTo(CenterPosition) < 2 && !RunningAwayFromChaser) // If the entity is under the center stop moving
		{
			GD.Print("Stop moving");
			_targetVelocity = Vector3.Zero;
			Velocity = _targetVelocity;
		}
		else
		{
			_targetVelocity = ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * MaxSpeed;
			Velocity = Velocity.Lerp(_targetVelocity, SteerForce);
		}
		
		// _targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		// Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		if(Velocity > Vector3.Zero )
		{
			BlendDirection.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			BlendDirection.Y = 0;
		}
		
		
	
		Tree.Set("parameters/IW/blend_position", BlendDirection);
		MoveAndSlide();


		
		// var desired_velocity = chosen_dir.Rotated(entity.direction, ) * max_speed;

	}

	public void LookAtOver() // Look at enemy and switch
	{
		if(!CanSeeCenter)
		{
			if(EntityInDetection)
			{
				
				// target_ability.Execute(this);
				LookAtPosition = BoxPosition;
				LookAt(LookAtPosition with {Y = GlobalPosition.Y});
				
			}
		}
		
		else
		{

			EntityInDetection = false;
			// Sets the animation to walk forward when not targeting
			if(ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) != Vector3.Zero)
			{
				BlendDirection.X = 0;
				BlendDirection.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				BlendDirection.X = Mathf.Lerp(BlendDirection.X, 0, 0.1f);
				BlendDirection.Y = Mathf.Lerp(BlendDirection.Y, 0, 0.1f);
			}
		}
	}

	public void SetInterest()
	{
		// *************** comment/uncomment for test behavior ***************

		// // ******** make state machine controlled **********
		if(ContextStateMachine.CurrentState.StateName == "State3")
		{
			
			SetObjectInterest();
		}
		else if (ContextStateMachine.CurrentState.StateName == "State2")
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
			NavigationAgent.TargetPosition = BoxPosition; 
			TargetPosition = NavigationAgent.GetNextPathPosition();

			for(int i = 0; i < NumRays; i++)
			{
				// GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

				// Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
				// (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
				var d = RayDirections[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(Transform.Basis.X + GlobalPosition.DirectionTo(TargetPosition)); 
				// If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
				Interest[i] = MathF.Max(0, d);

				// GD.Print(interest[i]);
			}
	}

	public void SetDefaultInterest()
	{
		// navigation_agent.TargetPosition = Vector3.Forward; 
		// target_position = navigation_agent.GetNextPathPosition();

		for(int i = 0; i < NumRays; i++)
		{
			// Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move (in this base case forward which is Transform.Basis.Z)
			var d = RayDirections[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(TargetPosition.Normalized());
			// If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
			// GD.Print("d " + d);
			Interest[i] = MathF.Max(0, d);
			// GD.Print(interest[i]);
		}
	}

	public void SetDanger()
	{
		// Create a space state to cast rays
		var space_state = GetWorld3D().DirectSpaceState;
		for(int i = 0; i < NumRays; i++)
		{
			// Cast a ray from the ray origin, in the ray direction(rotated with player .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) with a magnitude of our look_ahead variable
			var ray_query = PhysicsRayQueryParameters3D.Create(RayOrigin, RayOrigin + RayDirections[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * LookAhead);
			var ray_target = RayOrigin + RayDirections[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * LookAhead; // Used in SetRayCastLines
			var result = space_state.IntersectRay(ray_query); // Result dictionary from the ray cast
		
			// Uncomment to show ray casts before collision
			// SetRayCastLines(ray_lines,ray_target);

			// *************** Comment/ Uncomment for test behavior ***************
			if(result.Count > 0)
			{
				Collider = (Node3D)result["collider"];
				SetCollisionLines(CollisionLines, result);
				Danger[i] = 1.0f;
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
				Danger[i] = 0;
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
		for(int i = 0; i < NumRays; i++)
		{
			// If there is danger where the ray was cast, set the interest to zero
			// Need to change this to make the changing direction more versatile
			if(Danger[i] > 0.5f)
			{
				Interest[i] = 0.0f;
			}
		}

		ChosenDir = Vector3.Zero;
		
		for(int i = 0; i < NumRays; i++)
		{

			// Sum up all of the directions where there is interest (if the interest is zero at a given direction that direction will not factor into the chosen direction)
			ChosenDir += RayDirections[i] * Interest[i];
			// GD.Print("directions: " + ray_directions[i] * interest[i]);
			// GD.Print("Interest[i] " + interest[i]);

			// Uncomment to show lines the represent the weight of the directions the entity can move in
			SetDirectionLines(DirectionLines, RayDirections[i] * Interest[i] * DirectionLinesMag);
		}

		// Normalize the chosen direction
		ChosenDir = ChosenDir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		SetDirectionMovingLine(MovingLine, ChosenDir * MovingLineMag);

		// GD.Print("chosen dir " + chosen_dir);
	}

	// Lines representing where the ray cast is emitted from and to
	public void SetRayCastLines(MeshInstance3D meshInstance3D, Vector3 rayTarget)
	{
		if(meshInstance3D.Mesh is ImmediateMesh rayLinesMesh)
		{
			rayLinesMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, RayLinesMaterial);
			RayLinesMaterial.EmissionEnabled = true;
			rayLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			rayLinesMesh.SurfaceAddVertex(ToLocal(rayTarget));
			rayLinesMesh.SurfaceEnd();
		}
			
	}

	// Lines representing when the ray cast makes contact with an object
	public void SetCollisionLines(MeshInstance3D meshInstance3D, Godot.Collections.Dictionary result)
	{
		if(meshInstance3D.Mesh is ImmediateMesh collisionLinesMesh)
		{
			
			collisionLinesMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, CollisionLinesMaterial);
			CollisionLinesMaterial.EmissionEnabled = true;
			CollisionLinesMaterial.Emission = Colors.Red;
			CollisionLinesMaterial.AlbedoColor = Colors.Red;
			collisionLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			collisionLinesMesh.SurfaceAddVertex(ToLocal(result["position"].AsVector3()));
			collisionLinesMesh.SurfaceEnd();
		}
	}

	// Lines representing the weight of which direction the entity will move
	public void SetDirectionLines(MeshInstance3D meshInstance3D, Vector3 directions)
	{
		if(meshInstance3D.Mesh is ImmediateMesh directionLinesMesh)
		{
			directionLinesMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, DirectionLinesMaterial);
			DirectionLinesMaterial.EmissionEnabled = true;
			DirectionLinesMaterial.Emission = Colors.Yellow;
			DirectionLinesMaterial.AlbedoColor = Colors.Yellow;
			directionLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			directionLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin + directions.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			directionLinesMesh.SurfaceEnd();
		}
	}

	// Line representing the direction the entity is moving
	public void SetDirectionMovingLine(MeshInstance3D meshInstance3D, Vector3 direction)
	{
		if(meshInstance3D.Mesh is ImmediateMesh movingLineMesh)
		{
			movingLineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, MovingLineMaterial);
			MovingLineMaterial.EmissionEnabled = true;
			MovingLineMaterial.Emission = Colors.Green;
			MovingLineMaterial.AlbedoColor = Colors.Green;
			movingLineMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			movingLineMesh.SurfaceAddVertex(ToLocal(RayOrigin + direction.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			movingLineMesh.SurfaceEnd();
		}
	}

	// Rotate the entity smoothly in the direction it is looking
	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
		if(!EntityInDetection)
		{
			PreviousYRotation = GlobalRotation.Y;
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y))) // looks at direction the player is moving
			{
				LookAt(GlobalPosition + ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y));
				
			}
			CurrentYRotation = GlobalRotation.Y;
			if(PreviousYRotation != CurrentYRotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(PreviousYRotation, CurrentYRotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}
	


}
