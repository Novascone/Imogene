using Godot;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{
	
	private float speed = 5.0f; // speed of character

	private Vector3 _targetVelocity = Vector3.Zero;
	private bool can_move = true;
	private Area3D weapon_hitbox; // weapon hitbox
	private Area3D player_hitbox;
	private Area3D vision;
	private Area3D target;
	private bool enemy_in_vision = false;
	private Vector3 enemy_position;
	private bool targeting = false;

	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnAreaEntered;
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
	{
		if(enemy_in_vision)
			{
				GD.Print(enemy_position);
				lock_on(enemy_position);
			}

		move();
	}



	public void move()
	{
		var direction = Vector3.Zero;

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

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

		_targetVelocity.X = direction.X * speed;
        _targetVelocity.Z = direction.Z * speed;

		Velocity = _targetVelocity;
		if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + direction))
		{
			LookAt(GlobalPosition + direction);
		}
		
        MoveAndSlide();
	}



    public bool attack_check() // changes weapon hitbox monitoring based on animation
	{
		if(Input.IsActionPressed("Attack"))
		{
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


	private void OnAreaEntered(Area3D interactable) // handler for area entered signal
	{
		if(interactable.IsInGroup("enemy"))
		{
			enemy_in_vision = true;
			enemy_position = interactable.GlobalPosition ;
			GD.Print("Enemy Seen");
			GD.Print(enemy_position);
			get_enemy_position(interactable);
	
		}
		
	}

	private Vector3 get_enemy_position(Area3D interactable)
	{
		return interactable.GlobalPosition;
	}

	private void lock_on(Vector3 enemy_position)
	{
		
		if(Input.IsActionJustPressed("Target"))
		{
			targeting = true;
			GD.Print("Targeting");
			LookAt(enemy_position with { Y = 0.0f });
		}

	}



}
