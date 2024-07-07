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


	// Mob variables
	private bool playerInAlert = false; 
	private float attack_dist = 2.5f;
	private Label3D damage_label; 
	private AnimationPlayer damage_numbers;
	private Area3D alert_area;

	public Node3D collider;

	

	// Enemy animation
	private AnimationTree tree;

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

	public StateMachine state_machine;
	



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
	


		tree = GetNode<AnimationTree>("AnimationTree");

		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyTargetedUI += HandleEnemyTargetedUI;
		_customSignals.EnemyUntargetedUI += HandleEnemyUntargetedUI;		
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
	
		float distance_to_player = GlobalPosition.DistanceTo(player_position);
		Vector2 blend_direction = Vector2.Zero;
		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition);
		// GD.Print(currentState);
		

	}

	


	private void OnAlertAreaEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) 
		{
			// currentState = States.Chasing;
			playerInAlert = true;
			// GD.Print("Player Entered Alert");
		}

		// if(area.IsInGroup("RotateBox"))
		// {
		// 	GD.Print("In contact with rotate box");
		// 	entity_in_detection = true;
		// 	box_position = area.GlobalPosition;
		// }
		// if(area.IsInGroup("Center"))
		// {
		// 	GD.Print("within range of center");
		// 	can_see_center = true;
		// 	center_position = area.GlobalPosition with {Y = 0};
		// 	GD.Print(center_position);
		// }
		// if(area.IsInGroup("InterestPoint"))
		// {
		// 	GD.Print("within range of interest position");
		// 	interest_position = area.GlobalPosition with {Y = 0};
		// 	GD.Print(interest_position);
		// }
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
        
    }

	

	
}
