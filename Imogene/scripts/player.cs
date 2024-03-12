using Godot;
using System;
using System.Diagnostics;
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

	
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
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


	private void OnAreaEntered(Area3D area_hit) // handler for area entered signal
	{
		if(area_hit.IsInGroup("enemy"))
		{
			GD.Print("Enemy Seen");
		}
		
	}



}
