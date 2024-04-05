using Godot;
using System;
using System.Collections.Generic;

public partial class enemy : CharacterBody3D
{
	public enum States
	{
		Chasing,
		Attacking,
		Waiting
	}

	public States currentState;
	public NavigationAgent3D NavigationAgent;
	
	private int speed = 2;
	private List<Marker3D> waypoints = new List<Marker3D>();
	private int waypointIndex;
	private Area3D enemy_hitbox;
	private Vector3 player_position;
	private Vector3 camera_position;
	private AnimationPlayer damage_numbers;
	private Label3D damage_label;
	private int health = 20;
	private MeshInstance3D targeting_icon;
	private AnimationTree tree;
	private CustomSignals _customSignals;
	private Area3D alert;
	private bool playerInAlert = false;
	private Vector3 number_target;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy_hitbox = (Area3D)GetNode("EnemyHitbox");
		enemy_hitbox.AreaEntered += OnHitboxEntered;
		NavigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		alert = (Area3D)GetNode("Alert");
		alert.AreaEntered += OnAlertEntered;
		alert.AreaExited += OnAlertExited;
		currentState = States.Waiting;
		tree = GetNode<AnimationTree>("AnimationTree");
		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");
		targeting_icon = GetNode<MeshInstance3D>("TargetingIcon");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandleDamageEnemy;
		_customSignals.EnemyTargeted += HandleEnemyTargeted;
		_customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPosition;
		_customSignals.CameraPosition += HandleCameraPosition;
	}



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Vector2 blend_direction = Vector2.Zero;
		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition);
		// GD.Print(currentState);
		damage_label.LookAt(camera_position,Vector3.Up,true);
		switch (currentState)
		{
		
			case States.Chasing:
			
				NavigationAgent.TargetPosition = player_position;
				var targetPos = NavigationAgent.GetNextPathPosition();
				var direction = GlobalPosition.DirectionTo(targetPos);
				blend_direction.Y = 1;
				Velocity = direction * speed;
				if (!GlobalTransform.Origin.IsEqualApprox(player_position))
				{
					LookAt(player_position with {Y = GlobalPosition.Y});
				}
				tree.Set("parameters/IW/blend_position", blend_direction);
				MoveAndSlide();
				if(NavigationAgent.IsNavigationFinished() && playerInAlert)
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
				break;
			case States.Attacking:
				break;
			case States.Waiting:
				break;
			default:
				break;
		}
		
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("attacking"))
		{
			
			GD.Print("hit");
			damage_numbers.Play("Rise_and_Fade");

		}
		
	}

	private void HandleDamageEnemy(int damage_amount)
	{
		health -= damage_amount;
		damage_label.Text = Convert.ToString(damage_amount);
	}

	 private void OnAlertEntered(Area3D area)
    {
		if(area.IsInGroup("player"))
		{
			player_position = area.GlobalPosition;
			currentState = States.Chasing;
			playerInAlert = true;
			GD.Print("Player Entered Alert");
		}
    }

	private void OnAlertExited(Area3D area)
    {
		if(area.IsInGroup("player"))
		{
			playerInAlert = false;
		}
        
    }

	 private void HandleEnemyTargeted()
    {
		targeting_icon.Visible = true;
    }
	private void HandleEnemyUnTargeted()
    {
        targeting_icon.Visible = false;
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
