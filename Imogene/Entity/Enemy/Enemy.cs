using Godot;
using System;
using System.Collections.Generic;

// 																********************************************** MARKED FOR REWORK ************************************************************

public partial class Enemy : Entity
{
	

	

	[Export] public int MaxSpeed = 4; // How fast the entity will move 
	[Export] public float SteerForce = 0.02f; // How fast the entity turns
	
	// Movement variables
	public Vector3 TargetVelocity = Vector3.Zero;
	[Export] public int FallAcceleration { get; set; } = 75;
	
	[Export] EnemyAreas Areas;
	[Export] public BoneAttachment3D Head;
	
	// Ray cast variables
	[Export] public int LookAhead = 5; // How far the rays will project
	[Export] public int DirectionLinesMag = 5;
	[Export] public int DirectionLineMag = 7;
	[Export] public int NumRays = 16;

	[Export] public EnemyDebug Debug;
	[Export] public AnimationTree Tree;
	[Export] public NavigationAgent3D NavigationAgent; 
	[Export] public EnemyControllers EnemyControllers;
	// UI
	[Export] public EnemyUI UI;
	

	public Vector3 ChosenDir = Vector3.Zero; // Direction the entity has chosen

	// Mob variables
	public bool PlayerSeen = false; 
	public Player PlayerInAlert;
	private float AttackDist = 2.5f;
	private Label3D DamageLabel; 
	private AnimationPlayer DamageNumbers;

	public Vector3 RayOrigin;
	public StandardMaterial3D CollisionLinesMaterial = new();
	public StandardMaterial3D RayLinesMaterial = new();
	public StandardMaterial3D DirectionLineMaterial = new();
	public StandardMaterial3D DirectionMovingLineMaterial = new();
	public Vector3[] RayDirections; // Directions the rays will be cast in
	public float[] Interest; // Interest weight, how interested the entity is in moving toward a location
	public float[] Danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it
	public Node3D Collider;


	// Enemy animation
	
	public Vector2 BlendDirection = Vector2.Zero;

	// Navigation variables

	private readonly List<Marker3D> Waypoints = new(); 
	private int waypointIndex; 

	//Player variables
	private Vector3 _playerPosition; // Position of player
	private float _incomingDamage;
	
	// Signal variables
	private CustomSignals _customSignals;
	private Vector3 CameraPosition; // Position of camera


	
	public bool EntityInAlertArea;

	// Place for entity to look
	public Vector3 LookAtPosition;

	// Switch variable
	public bool SwitchToState2;

	public bool InSoftTargetSmall;
	public bool InSoftTargetLarge;
	public bool SoftTarget;
	public bool InPlayerVision;

	public bool Attacking { get; set; } = false; // Boolean to keep track of if the entity is attacking
	public bool Targeted { get; set; } = false;

	public float JumpHeight = 10;
	public float JumpTimeToPeak = 2f;
	public float JumpTimeToDecent = 1.9f;

	public float JumpVelocity ;
	public float JumpGravity;
	public float FallGravity;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		
		base._Ready();

		JumpVelocity = (float)(2.0 * JumpHeight / JumpTimeToPeak);
		JumpGravity = (float)(-2.0 * JumpHeight / JumpTimeToPeak * JumpTimeToPeak);
		FallGravity = (float)(-2.0 * JumpHeight / JumpTimeToDecent * JumpTimeToDecent);

		UI.HealthBar.MaxValue = Health.MaxValue;
		UI.HealthBar.Value = Health.CurrentValue;
		UI.PostureBar.MaxValue = 0;
		UI.PostureBar.Value = 0;
		Attacking = false;
		Level.BaseValue = 1;
		Armor.CurrentValue = 0;
		Stamina.CurrentValue = 2000;
		



		Areas.Alert.BodyEntered += OnAlertAreaBodyEntered;
		Areas.Alert.AreaEntered += OnAlertAreaEntered;
		Areas.Alert.BodyExited += OnAlertAreaBodyExited;

		
		Array.Resize(ref Interest, NumRays);
		Array.Resize(ref Danger, NumRays);
		Array.Resize(ref RayDirections, NumRays);

		for( int i = 0; i < NumRays; i++)
		{
			float angle = i * 2 * MathF.PI / NumRays; // <-- circle divided into number of rays
			RayDirections[i] = Vector3.Forward.Rotated(GlobalTransform.Basis.Y.Normalized(), angle); // <-- set the ray directions
			// GD.Print(ray_directions[i]);
		}
	}

    private void HandleFinishedCircling()
    {
        EnemyControllers.StateMachine.CurrentState.Exit("ForwardState");
    }

 
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
	
		RayOrigin = EnemyControllers.RayPosition.GlobalPosition;
		if(Debug.CollisionLines.Mesh is ImmediateMesh collisionLinesMesh)
		{
			collisionLinesMesh.ClearSurfaces();
		}
		if(Debug.RayLines.Mesh is ImmediateMesh rayLinesMesh)
		{
			rayLinesMesh.ClearSurfaces();
		}
		if(Debug.DirectionLines.Mesh is ImmediateMesh directionLinesMesh)
		{
			directionLinesMesh.ClearSurfaces();
		}
		if(Debug.MovingLine.Mesh is ImmediateMesh directionMovingLineMesh)
		{
			directionMovingLineMesh.ClearSurfaces();
		}

		// if (Input.IsActionJustPressed("one"))
		// {
		// 	state_machine.current_state.Exit("AvoidanceInterestsState");
		// }
		// if (Input.IsActionJustPressed("two"))
		// {
		// 	state_machine.current_state.Exit("HerdState");
		// }
	
		float distance_to_player = GlobalPosition.DistanceTo(_playerPosition);
		Vector2 blend_direction = Vector2.Zero;

		if (DirectionVector != Vector3.Zero)
		{
			DirectionVector = DirectionVector.Normalized();
			// Setting the basis property will affect the rotation of the node.
			// GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(look_at_position);
		}

		EnemyControllers.MovementController.StatusEffectsAffectingSpeed(this);
		EnemyControllers.MovementController.StatusEffectsPreventingMovement(this);
		EnemyControllers.AbilityController.CheckCanUseAbility(this);

		EnemyControllers.MovementController.MoveEnemy(this, delta);
		SmoothRotation();
		LookAtOver();
		MoveAndSlide();
		
		
		
		
		// GD.Print(currentState);
	}
	public void LookAtOver() // Look at enemy and switch
	{
		
		
	}

	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
		if(!EntityInAlertArea)
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

	


	virtual public void OnAlertAreaEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) 
		{
			// currentState = States.Chasing;
			PlayerSeen = true;
			// GD.Print("Player Entered Alert");
		}
    }

	virtual public void OnAlertAreaBodyEntered(Node3D body) 
    {
		if(body is Player player) 
		{
			// currentState = States.Chasing;
			PlayerSeen = true;
			PlayerInAlert = player;
			GD.Print("Player Entered Alert");
		}
		
    }
	
	virtual public void OnAlertAreaBodyExited(Node3D body)
    {
		if(body is Player player)
		{
			PlayerSeen = false;
			PlayerInAlert = null;
		}
    }

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
			directionLinesMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, DirectionLineMaterial);
			DirectionLineMaterial.EmissionEnabled = true;
			DirectionLineMaterial.Emission = Colors.Yellow;
			DirectionLineMaterial.AlbedoColor = Colors.Yellow;
			directionLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			directionLinesMesh.SurfaceAddVertex(ToLocal(RayOrigin + directions.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			directionLinesMesh.SurfaceEnd();
		}
	}

	// Line representing the direction the entity is moving
	public void SetDirectionMovingLine(MeshInstance3D meshInstance3D, Vector3 direction)
	{
		if(meshInstance3D.Mesh is ImmediateMesh directionMovingLineMesh)
		{
			directionMovingLineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, DirectionLineMaterial);
			DirectionLineMaterial.EmissionEnabled = true;
			DirectionLineMaterial.Emission = Colors.Green;
			DirectionLineMaterial.AlbedoColor = Colors.Green;
			directionMovingLineMesh.SurfaceAddVertex(ToLocal(RayOrigin));
			directionMovingLineMesh.SurfaceAddVertex(ToLocal(RayOrigin + direction.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)));
			directionMovingLineMesh.SurfaceEnd();
		}
	}
}
