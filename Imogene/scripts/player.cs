using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{
	
	private float speed = 5.0f; // speed of character
	private bool can_move = true;
	private AnimationPlayer animation_player; // animation player
	private Area3D weapon_hitbox; // weapon hitbox
	private Area3D player_hitbox;
	private Area3D vision;
	private Area3D target;

	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		animation_player = (AnimationPlayer)GetNode("AnimationPlayer"); // get reference to AnimationPlater
		weapon_hitbox = (Area3D)GetNode("WeaponPivot/WeaponMesh/WeaponHitbox"); // get reference to Hitbox
		weapon_hitbox.AreaEntered += OnAreaEntered; // subscribe hitbox to OnHitboxAreaEntered signal
		player_hitbox = (Area3D)GetNode("PlayerHitbox");
		player_hitbox.AreaEntered += OnAreaEntered;
		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnAreaEntered;
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
	{
		move();
		
	}



	public void move()
	{
		Vector3 velocity = Velocity;
		Vector2 inputDir = Input.GetVector("Right", "Left", "Backward", "Forward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X,0,inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, speed);
		}
		// if(!GlobalTransform.Origin.IsEqualApprox(direction))
		// {
		// 	LookAt(direction);
		// }
		
		Velocity = velocity;
		MoveAndSlide();
	}


    public void rotate()

	{
		if (Input.IsActionPressed("RotateCharacterLeft"))
		{
			GD.Print("RotateCharacterLeft");
		RotationDegrees = RotationDegrees with { X = 90 };
		}
	}



    public bool attack_check() // changes weapon hitbox monitoring based on animation
	{
		if(Input.IsActionPressed("Attack"))
		{
			animation_player.Play("attack");
			weapon_hitbox.Monitoring = true;
			return true;
		}
		
		else if(Input.IsActionJustReleased("Attack"))
		{
			weapon_hitbox.Monitoring = false;
			can_move = false;
			return false;
		}

		return false;
	}


	private void OnAreaEntered(Area3D area_hit) // handler for area entered signal
	{
		if(area_hit.IsInGroup("enemy"))
		{
			GD.Print("Enemy Seen");
		}
		
	}



}
