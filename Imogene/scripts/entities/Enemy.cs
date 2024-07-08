using Godot;
using System;
using System.Collections.Generic;

// 																********************************************** MARKED FOR REWORK ************************************************************

public partial class Enemy : Entity
{
	

	

	[Export] public int max_speed = 4; // How fast the entity will move 
	[Export] public float steer_force = 0.02f; // How fast the entity turns
	[Export] public int look_ahead = 5; // How far the rays will project
	[Export] public int direction_lines_mag = 5;
	[Export] public int direction_line_mag = 7;
	[Export] public int num_rays = 16;

	public Vector3[] ray_directions; // Directions the rays will be cast in
	public float[] interest; // Interest weight, how interested the entity is in moving toward a location
	public float[] danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it

	// Movement variables
	private Vector3 _targetVelocity = Vector3.Zero;
	[Export] public int FallAcceleration { get; set; } = 75;
	public Vector3 chosen_dir = Vector3.Zero; // Direction the entity has chosen



	// Mob variables
	private bool playerInAlert = false; 
	private float attack_dist = 2.5f;
	private Label3D damage_label; 
	private AnimationPlayer damage_numbers;
	private Area3D alert_area;
	private Area3D herd_area;

	public Node3D collider;

	

	// Enemy animation
	private AnimationTree tree;
	Vector2 blend_direction = Vector2.Zero;

	// Navigation variables
	public NavigationAgent3D navigation_agent; 
	private List<Marker3D> waypoints = new List<Marker3D>(); 
	private int waypointIndex; 

	//Player variables
	private Vector3 player_position; // Position of player
	private float incoming_damage;
	
	// Signal variables
	private CustomSignals _customSignals;
	private Vector3 camera_position; // Position of camera

	public StatController statController;

	// UI
	public ProgressBar health_bar;
	public TextureProgressBar posture_bar;
	public Sprite3D status_bar;
	public Sprite3D target_icon;

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

	// State machine
	public StateMachine state_machine;

	public bool entity_in_alert_area;

	// Place for entity to look
	Vector3 look_at_position;

	// Switch variable
	public bool switch_to_state2;

	// Context steering test variable
	public Vector3 box_position;
	
	// Herd variables
	public bool near_herd_mate;
	public Vector3 herd_mate_position;
	public Vector3 interest_position;
	public Node3D herd_mate;
	public bool can_see_center;
	
	

	// Multiple interests variables
	public bool running_away_from_chaser;
	public Vector3 center_position;
	public Node3D chaser;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		statController = GetNode<StatController>("StatController");
		health_bar = GetNode<ProgressBar>("HealthBarViewport/VBoxContainer/ProgressBar");
		posture_bar = GetNode<TextureProgressBar>("HealthBarViewport/VBoxContainer/TextureProgressBar");
		status_bar = GetNode<Sprite3D>("StatusBar");
		target_icon = GetNode<Sprite3D>("TargetIcon");
		maximum_health = health;
		health_bar.MaxValue = health;
		health_bar.Value = health;
		posture_bar.MaxValue = maximum_posture;
		posture_bar.Value = 0;
		speed = 2;
		attacking = false;
		level = 1;
		armor = 5;
		stamina = 2000;
		physical_resistance = 10;
		slash_resistance = 3;
		dr_lvl_scale = 50 * (float)level;
		rec_lvl_scale = 100 * (float)level;
		statController.GetEntityInfo(this);
		statController.UpdateStats();
		GD.Print("Posture Regen " + posture_regen);

		ray_position = GetNode<Node3D>("RayPosition");
		tree = GetNode<AnimationTree>("AnimationTree");
		collision_lines = GetNode<MeshInstance3D>("CollisionLines");
		ray_lines = GetNode<MeshInstance3D>("RayLines");
		direction_lines = GetNode<MeshInstance3D>("DirectionLines");
		direction_moving_line = GetNode<MeshInstance3D>("DirectionMovingLine");

		navigation_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		state_machine = GetNode<StateMachine>("StateMachine");
		state_machine.GetEntityInfo(this);

		alert_area = GetNode<Area3D>("Alert");
		alert_area.BodyEntered += OnAlertAreaBodyEntered;
		alert_area.AreaEntered += OnAlertAreaEntered;
		alert_area.BodyExited += OnAlertAreaBodyExited;

		herd_area = GetNode<Area3D>("HerdDetection");
		herd_area.AreaEntered += OnHerdDetectionEntered;
		herd_area.AreaExited += OnHerdDetectionExited;
		herd_area.BodyEntered += OnHerdBodyEntered;

		Array.Resize(ref interest, num_rays);
		Array.Resize(ref danger, num_rays);
		Array.Resize(ref ray_directions, num_rays);

		for( int i = 0; i < num_rays; i++)
		{
			float angle = i * 2 * MathF.PI / num_rays; // <-- circle divided into number of rays
			ray_directions[i] = Vector3.Forward.Rotated(GlobalTransform.Basis.Y.Normalized(), angle); // <-- set the ray directions
			GD.Print(ray_directions[i]);
		}

		
	


		tree = GetNode<AnimationTree>("AnimationTree");

		// damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		// damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyTargetedUI += HandleEnemyTargetedUI;
		_customSignals.EnemyUntargetedUI += HandleEnemyUntargetedUI;
		_customSignals.FinishedCircling += HandleFinishedCircling;		
	}

    private void HandleFinishedCircling()
    {
        state_machine.current_state.Exit("State2");
    }

    private void HandleEnemyUntargetedUI()
    {
        status_bar.Hide();
		target_icon.Hide();
    }

    private void HandleEnemyTargetedUI(Enemy enemy)
    {
        status_bar.Show();
		target_icon.Show();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		// GD.Print("Max Health " + maximum_health);
		// GD.Print("Health " + health);

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

		if (Input.IsActionJustPressed("one"))
		{
			state_machine.current_state.Exit("State4");
		}
		if (Input.IsActionJustPressed("two"))
		{
			state_machine.current_state.Exit("State5");
		}
	
		float distance_to_player = GlobalPosition.DistanceTo(player_position);
		Vector2 blend_direction = Vector2.Zero;
		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition);

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			// Setting the basis property will affect the rotation of the node.
			// GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(look_at_position);
		}

		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		if(herd_mate_position != Vector3.Zero)
		{
			GD.Print("herd mate position " + herd_mate_position + " from " + GetParent().Name);
			GD.Print(herd_mate);
		}

		SmoothRotation();
		LookAtOver();

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
		// GD.Print(currentState);
		

	}
	public void LookAtOver() // Look at enemy and switch
	{
		if(!can_see_center)
		{
			if(entity_in_alert_area)
			{
				
				// target_ability.Execute(this);
				look_at_position = box_position;
				LookAt(look_at_position with {Y = GlobalPosition.Y});
				
			}
		}
		
		else
		{

			entity_in_alert_area = false;
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

	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
		if(!entity_in_alert_area)
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

	


	private void OnAlertAreaEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) 
		{
			// currentState = States.Chasing;
			playerInAlert = true;
			// GD.Print("Player Entered Alert");
		}

		if(area.IsInGroup("RotateBox"))
		{
			GD.Print("In contact with rotate box");
			entity_in_alert_area = true;
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

	private void OnAlertAreaBodyEntered(Node3D body) 
    {
		if(body.IsInGroup("player")) 
		{
			// currentState = States.Chasing;
			playerInAlert = true;
			// GD.Print("Player Entered Alert");
		}
		
    }

	private void OnAlertAreaBodyExited(Node3D body)
    {
		if(body.IsInGroup("player"))
		{
			playerInAlert = false;
		}
		if(body is Chaser chaser_entered )
		{
			GD.Print("Chaser in detection");
			chaser = chaser_entered;
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

	private void OnHerdBodyEntered(Node3D body)
    {
		
        if(body is Enemy herd_entity && body.GetParent().Name != GetParent().Name)
		{
			GD.Print(body.GetParent().Name + " entered detection area of " + GetParent().Name);
			GD.Print("Herd mate in detection");
			herd_mate = herd_entity;
		}
    }

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

	

	
}
