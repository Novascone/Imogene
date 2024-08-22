using Godot;
using System;
using System.Collections.Generic;

// 																********************************************** MARKED FOR REWORK ************************************************************

public partial class Enemy : Entity
{
	

	

	[Export] public int max_speed = 4; // How fast the entity will move 
	[Export] public float steer_force = 0.02f; // How fast the entity turns
	
	// Movement variables
	public Vector3 _targetVelocity = Vector3.Zero;
	[Export] public int FallAcceleration { get; set; } = 75;
	public Vector3 chosen_dir = Vector3.Zero; // Direction the entity has chosen

	// Mob variables
	public bool player_seen = false; 
	public Player player_in_alert;
	private float attack_dist = 2.5f;
	private Label3D damage_label; 
	private AnimationPlayer damage_numbers;
	public Area3D alert_area;
	public BoneAttachment3D head;
	
	// Ray cast variables
	[Export] public int look_ahead = 5; // How far the rays will project
	[Export] public int direction_lines_mag = 5;
	[Export] public int direction_line_mag = 7;
	[Export] public int num_rays = 16;
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
	public Vector3[] ray_directions; // Directions the rays will be cast in
	public float[] interest; // Interest weight, how interested the entity is in moving toward a location
	public float[] danger; // Is the object a given array collided with "dangerous" meaning that the entity wants to avoid it
	public Node3D collider;


	// Enemy animation
	public AnimationTree tree;
	public Vector2 blend_direction = Vector2.Zero;

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

	public StatController stat_controller;

	// UI
	public ProgressBar health_bar;
	public TextureProgressBar posture_bar;
	public Sprite3D status_bar;
	public Sprite3D hard_target_icon;
	public Sprite3D soft_target_icon;

	

	// State machine
	public StateMachine state_machine;

	public bool entity_in_alert_area;

	// Place for entity to look
	public Vector3 look_at_position;

	// Switch variable
	public bool switch_to_state2;

	public bool in_soft_target_small;
	public bool in_soft_target_large;
	public bool soft_target;
	public bool in_player_vision;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		stat_controller = GetNode<StatController>("Controllers/StatController");
		health_bar = GetNode<ProgressBar>("UI/HealthBarSubViewport/VBoxContainer/ProgressBar");
		posture_bar = GetNode<TextureProgressBar>("UI/HealthBarSubViewport/VBoxContainer/TextureProgressBar");
		status_bar = GetNode<Sprite3D>("UI/StatusBar");
		hard_target_icon = GetNode<Sprite3D>("UI/HardTargetIcon");
		soft_target_icon = GetNode<Sprite3D>("UI/SoftTargetIcon");
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
		stat_controller.GetEntityInfo(this);
		stat_controller.UpdateStats();
		GD.Print("Posture Regen " + posture_regen);

		ray_position = GetNode<Node3D>("Controllers/RayPosition");
		tree = GetNode<AnimationTree>("Animation/AnimationTree");
		collision_lines = GetNode<MeshInstance3D>("Debug/CollisionLines");
		ray_lines = GetNode<MeshInstance3D>("Debug/RayLines");
		direction_lines = GetNode<MeshInstance3D>("Debug/DirectionLines");
		direction_moving_line = GetNode<MeshInstance3D>("Debug/DirectionMovingLine");

		main_hand_hitbox = GetNode<MeleeHitbox>("Armature/Skeleton3D/MainHand/MainHandSlot/Weapon/Hitbox");

		navigation_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		state_machine = GetNode<StateMachine>("Controllers/StateMachine");
		state_machine.GetEntityInfo(this);

		hurtbox = GetNode<Hurtbox>("Armature/Skeleton3D/Chest/ChestSlot/Hurtbox");
		hurtbox.BodyEntered += OnHurtboxBodyEntered;
		hurtbox.AreaEntered += OnHurtboxAreaEntered;

		head = GetNode<BoneAttachment3D>("Armature/Skeleton3D/Head");

		alert_area = GetNode<Area3D>("Areas/Alert");
		alert_area.BodyEntered += OnAlertAreaBodyEntered;
		alert_area.AreaEntered += OnAlertAreaEntered;
		alert_area.BodyExited += OnAlertAreaBodyExited;

		
		Array.Resize(ref interest, num_rays);
		Array.Resize(ref danger, num_rays);
		Array.Resize(ref ray_directions, num_rays);

		for( int i = 0; i < num_rays; i++)
		{
			float angle = i * 2 * MathF.PI / num_rays; // <-- circle divided into number of rays
			ray_directions[i] = Vector3.Forward.Rotated(GlobalTransform.Basis.Y.Normalized(), angle); // <-- set the ray directions
			GD.Print(ray_directions[i]);
		}

		// damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		// damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.EnemyTargetedUI += HandleEnemyTargetedUI;
		// _customSignals.EnemyUntargetedUI += HandleEnemyUntargetedUI;
		_customSignals.FinishedCircling += HandleFinishedCircling;		
	}

    private void OnHurtboxAreaEntered(Area3D area)
    {
        if(area is MeleeHitbox melee_box)
		{
			GD.Print(Name + " hurtbox entered by " + melee_box.Name);
			if(area is MeleeHitbox)
			{
				damage_system.TakeDamage(melee_box.damage_type, melee_box.damage, melee_box.is_critical);
				resource_system.Posture(melee_box.posture_damage);
				if(melee_box.effect_1 != "")
				{
					GD.Print(Name + " has " + melee_box.effect_1 + " applied");
				}
			}
		}
		
    }

    private void OnHurtboxBodyEntered(Node3D body)
    {
		
		if(body is RangedHitbox ranged_box)
		{
			if(body is RangedHitbox)
			{
				damage_system.TakeDamage(ranged_box.damage_type, ranged_box.damage, ranged_box.is_critical);
				resource_system.Posture(ranged_box.posture_damage);
			}
		}
    }

    private void HandleFinishedCircling()
    {
        state_machine.current_state.Exit("ForwardState");
    }

    // private void HandleEnemyUntargetedUI()
    // {
    //     status_bar.Hide();
	// 	target_icon.Hide();
    // }

    // private void HandleEnemyTargetedUI(Enemy enemy)
    // {
    //     status_bar.Show();
	// 	target_icon.Show();
    // }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		// GD.Print("Max Health " + maximum_health);
		// GD.Print("Health " + health);
		
		// GD.Print(Name + " In soft small " + in_soft_target_small);

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

		// if (Input.IsActionJustPressed("one"))
		// {
		// 	state_machine.current_state.Exit("AvoidanceInterestsState");
		// }
		// if (Input.IsActionJustPressed("two"))
		// {
		// 	state_machine.current_state.Exit("HerdState");
		// }
	
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

		SmoothRotation();
		LookAtOver();
		
		
		
		
		// GD.Print(currentState);
	}
	public void LookAtOver() // Look at enemy and switch
	{
		
		
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

	


	virtual public void OnAlertAreaEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) 
		{
			// currentState = States.Chasing;
			player_seen = true;
			// GD.Print("Player Entered Alert");
		}
    }

	virtual public void OnAlertAreaBodyEntered(Node3D body) 
    {
		if(body is Player player) 
		{
			// currentState = States.Chasing;
			player_seen = true;
			player_in_alert = player;
			GD.Print("Player Entered Alert");
		}
		
    }
	
	virtual public void OnAlertAreaBodyExited(Node3D body)
    {
		if(body is Player player)
		{
			player_seen = false;
			player_in_alert = null;
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
