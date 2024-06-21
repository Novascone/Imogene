using Godot;
using System;
using System.Collections.Generic;

// 																********************************************** MARKED FOR REWORK ************************************************************

public partial class Enemy : Entity
{
	public enum States 
	{
		Chasing,
		Attacking,
		Waiting,
		Dead
	}

	public States currentState;

	// Mob variables
	private bool playerInAlert = false; 
	private float attack_dist = 2.5f;
	private Label3D damage_label; 
	private AnimationPlayer damage_numbers;
	private Area3D alert;

	// Enemy animation
	private AnimationTree tree;

	// Navigation variables
	public NavigationAgent3D NavigationAgent; 
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
	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		statController = GetNode<StatController>("StatController");
		health_bar = GetNode<ProgressBar>("SubViewport/VBoxContainer/ProgressBar");
		posture_bar = GetNode<TextureProgressBar>("SubViewport/VBoxContainer/TextureProgressBar");
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
		// hurtbox = (Area3D)GetNode("Hurtbox");
		// hurtbox.AreaEntered += OnHurtboxEntered;
		// hitbox = (Area3D)GetNode("Skeleton3D/BoneAttachment3D/Hitbox");
		// hitbox.AreaEntered += OnHitboxEntered;

		NavigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		alert = (Area3D)GetNode("Alert");
		alert.AreaEntered += OnAlertEntered;
		alert.AreaExited += OnAlertExited;

		currentState = States.Waiting;

		tree = GetNode<AnimationTree>("AnimationTree");

		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPosition;
		_customSignals.CameraPosition += HandleCameraPosition;
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		// GD.Print("Max Health " + maximum_health);
		// GD.Print("Health " + health);
		float distance_to_player = GlobalPosition.DistanceTo(player_position);
		Vector2 blend_direction = Vector2.Zero;
		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition);
		// GD.Print(currentState);
		damage_label.LookAt(camera_position,Vector3.Up,true); 
		if(dead)
		{
			currentState = States.Dead;
		}
		switch (currentState)
		{
		
			case States.Chasing: 

				// comment/ uncomment to get enemy to chase

				attacking = false;
				if(can_move)
				{
					NavigationAgent.TargetPosition = player_position; 
					var targetPos = NavigationAgent.GetNextPathPosition();
					var direction = GlobalPosition.DirectionTo(targetPos);
					blend_direction.Y = 1; // Sets animation to walk
					Velocity = direction * speed;
					if (!GlobalTransform.Origin.IsEqualApprox(player_position))
					{
						LookAt(player_position with {Y = GlobalPosition.Y}); 
					}
					tree.Set("parameters/IW/blend_position", blend_direction);
					tree.Set("parameters/conditions/attacking", attacking);
					MoveAndSlide();
					if(distance_to_player < attack_dist && playerInAlert)
					{
						currentState = States.Attacking;
						return;
					}
					else if(!playerInAlert)
					{
						blend_direction.Y = 0;
						tree.Set("parameters/IW/blend_position", blend_direction);
						currentState = States.Waiting;
					}
				}
				else
				{
					tree.Set("parameters/IW/blend_position", Vector2.Zero);
					tree.Set("parameters/conditions/attacking", false);
				}
				
				break;
			case States.Attacking:
				// hitbox.AddToGroup("enemy_hitbox");
				// attacking = true;
				// hitbox.Monitoring = true;
				tree.Set("parameters/conditions/attacking", attacking);
				if(distance_to_player > attack_dist && playerInAlert)
				{
					currentState = States.Chasing;
				}
				break;
			case States.Waiting:
				break;
			case States.Dead:
				can_move = false;
				tree.Set("parameters/conditions/dead", dead);
				health = 0;
				break;
			default:
				break;
		}
		
	}

	


	 private void OnAlertEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) 
		{
			currentState = States.Chasing;
			playerInAlert = true;
			// GD.Print("Player Entered Alert");
		}
    }

	private void OnAlertExited(Area3D area)
    {
		if(area.IsInGroup("player"))
		{
			playerInAlert = false;
		}
        
    }

	private void HandleEnemyPosition(Vector3 position){}

	private void HandlePlayerPosition(Vector3 position) 
	{
       player_position = position;
    }

	private void HandleCameraPosition(Vector3 position)
    {
		camera_position = position;
    }
}
