using Godot;
using System;
using System.Collections.Generic;

public partial class enemy : Entity
{
	public enum States // States that the enemy can be in
	{
		Chasing,
		Attacking,
		Waiting,
		Dead
	}

	public States currentState; // Current State
	public NavigationAgent3D NavigationAgent; // Navigation agent for the enemy
	private List<Marker3D> waypoints = new List<Marker3D>(); // Waypoints for a possible patrol
	private int waypointIndex; 
	private Area3D hurtbox; // Enemy hurtbox 
	private Area3D hitbox;
	private Vector3 player_position; // Position of player
	private Vector3 camera_position; // Position of camera
	private AnimationPlayer damage_numbers;
	private int incoming_damage;
	private Label3D damage_label; // Damage numbers displayed above enemy when hit
	private AnimationTree tree;
	private CustomSignals _customSignals;
	private Area3D alert; // Area where the enemy will be alerted if the player walks into it
	private bool playerInAlert = false; // Boolean to keep track of if player has entered the alert area
	private float attack_dist = 2.5f;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Setting Node locations, and subscribing to events
		speed = 2;
		attacking = false;

		hurtbox = (Area3D)GetNode("Hurtbox");
		hurtbox.AreaEntered += OnHurtboxEntered;
		hitbox = (Area3D)GetNode("Skeleton3D/BoneAttachment3D/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;

		NavigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		alert = (Area3D)GetNode("Alert");
		alert.AreaEntered += OnAlertEntered;
		alert.AreaExited += OnAlertExited;

		currentState = States.Waiting;

		tree = GetNode<AnimationTree>("AnimationTree");

		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandleDamageEnemy;
		// _customSignals.EnemyTargeted += HandleEnemyTargeted;
		// _customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPosition;
		_customSignals.CameraPosition += HandleCameraPosition;
		_customSignals.EnemyDamage += HandleEnemyDamage;
	}

    private void OnHitboxEntered(Area3D area)
    {
        if(hitbox.IsInGroup("player"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.EnemyDamage), damage); // Sends how much damage the player does to the enemy
			hitbox.RemoveFromGroup("enemy_hitbox"); // Removes weapon from attacking group
			// GD.Print("enemy hit");
		}
    }

    private void HandleEnemyDamage(int DamageAmount)
    {
        DamageAmount += damage;
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
		float distance_to_player = GlobalPosition.DistanceTo(player_position);
		Vector2 blend_direction = Vector2.Zero; // Blend direction for animations
		_customSignals.EmitSignal(nameof(CustomSignals.EnemyPosition), GlobalPosition); // Emits enemy position
		// GD.Print(currentState);
		damage_label.LookAt(camera_position,Vector3.Up,true); // Gets damage numbers to face camera, use model front is true\
		if(dead)
		{
			currentState = States.Dead;
		}
		switch (currentState)
		{
		
			case States.Chasing: 

				attacking = false;
				hitbox.Monitoring = false;
				NavigationAgent.TargetPosition = player_position; // Sets navigation to player position
				var targetPos = NavigationAgent.GetNextPathPosition();
				var direction = GlobalPosition.DirectionTo(targetPos);
				blend_direction.Y = 1; // Sets animation to walk
				Velocity = direction * speed;
				if (!GlobalTransform.Origin.IsEqualApprox(player_position))
				{
					LookAt(player_position with {Y = GlobalPosition.Y}); // Looks at player
				}
				tree.Set("parameters/IW/blend_position", blend_direction);
				tree.Set("parameters/conditions/attacking", attacking);
				MoveAndSlide();
				if(distance_to_player < attack_dist && playerInAlert) // 	Change states
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
				hitbox.AddToGroup("enemy_hitbox");
				attacking = true;
				hitbox.Monitoring = true;
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
				break;
			default:
				break;
		}
		
	}

	private void OnHurtboxEntered(Area3D hitbox) // Displays damage numbers when hurt
	{
		if(hitbox.IsInGroup("player_hitbox"))
		{
			
			GD.Print("hit");
			damage_numbers.Play("Rise_and_Fade");
			TakeDamage(incoming_damage);
			GD.Print("Enemy: ", health);
			damage_label.Text = Convert.ToString(incoming_damage);

		}
		
	}

	private void HandleDamageEnemy(int damage_amount) // Reduced enemy health when hit
	{
		incoming_damage = damage_amount;
	}

	 private void OnAlertEntered(Area3D area) 
    {
		if(area.IsInGroup("player")) // If player enters alert Change states
		{
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

	private void HandleEnemyPosition(Vector3 position){}

	private void HandlePlayerPosition(Vector3 position) // Get player position
	{
       player_position = position;
    }

	private void HandleCameraPosition(Vector3 position) // Get Camera position
    {
		camera_position = position;
    }
}
