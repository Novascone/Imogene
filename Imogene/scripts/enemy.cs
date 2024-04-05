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
	private AnimationPlayer damage_numbers;
	private Label3D damage_label;
	private int health = 20;
	private MeshInstance3D targeting_icon;
	private CustomSignals _customSignals;
	private Area3D alert;
	private bool playerInAlert = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy_hitbox = (Area3D)GetNode("EnemyHitbox");
		enemy_hitbox.AreaEntered += OnHitboxEntered;
		NavigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		alert = (Area3D)GetNode("Alert");
		alert.AreaEntered += OnAlertEntered;
		currentState = States.Waiting;
		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");
		targeting_icon = GetNode<MeshInstance3D>("TargetingIcon");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandleDamageEnemy;
		_customSignals.EnemyTargeted += HandleEnemyTargeted;
		_customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
		_customSignals.EnemyPosition += HandleEnemyPosition;
	}

    









    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition);
		// GD.Print(currentState);
		switch (currentState)
		{
		
			case States.Chasing:
			
				NavigationAgent.TargetPosition = player_position;
				var targetPos = NavigationAgent.GetNextPathPosition();
				var direction = GlobalPosition.DirectionTo(targetPos);
				Velocity = direction * speed;
				if (!GlobalTransform.Origin.IsEqualApprox(player_position))
				{
					LookAt(player_position with {Y = GlobalPosition.Y});
				}
				
				MoveAndSlide();
				if(NavigationAgent.IsNavigationFinished() && playerInAlert)
				{
					currentState = States.Attacking;
					return;
				}
				else if(NavigationAgent.IsNavigationFinished())
				{
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

	 private void HandleEnemyTargeted()
    {
		targeting_icon.Visible = true;
    }
	private void HandleEnemyUnTargeted()
    {
        targeting_icon.Visible = false;
    }

	private void HandleEnemyPosition(Vector3 position)
    {
        
    }
}
