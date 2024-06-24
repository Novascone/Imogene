using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class ContextSteering : CharacterBody3D
{
	[Export] int max_speed = 4;
	[Export] float steer_force = 0.1f;
	[Export] int look_ahead = 10;
	[Export] int num_rays = 8;

	Vector3[] ray_directions;
	float[] interest;
	float[] danger;

	Vector3 chosen_dir = Vector3.Zero;
	Vector3 velocity = Vector3.Zero;
	Vector3 acceleration = Vector3.Zero;


	Vector3 ray_origin;

	Entity entity;

	 [Export]
    public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

	Vector2 blend_direction = Vector2.Zero;
	private AnimationTree tree;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tree = GetNode<AnimationTree>("AnimationTree");
		Array.Resize(ref interest, num_rays);
		Array.Resize(ref danger, num_rays);
		Array.Resize(ref ray_directions, num_rays);

		for( int i = 0; i < num_rays; i++)
		{
			float angle = i * 2 * MathF.PI / num_rays;
			ray_directions[i] = Vector3.Right.Rotated(GlobalTransform.Basis.Y.Normalized(), angle);
			GD.Print(ray_directions[i]);
		}
	
		// ray_two.TargetPosition = ray_directions[1]* look_ahead;
		// ray_three.TargetPosition = ray_directions[2]* look_ahead;
		// ray_four.TargetPosition = ray_directions[3]* look_ahead;
		// ray_five.TargetPosition = ray_directions[4]* look_ahead;
		// ray_six.TargetPosition = ray_directions[5]* look_ahead;
		// ray_seven.TargetPosition = ray_directions[6]* look_ahead;
		// ray_eight.TargetPosition = ray_directions[7]* look_ahead;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		ray_origin = GlobalTransform.Origin;
		var direction = Vector3.Zero;

	// We check for each move input and update the direction accordingly.
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
	// direction.Z -= 1;
	if (direction != Vector3.Zero)
    {
        direction = direction.Normalized();
        // Setting the basis property will affect the rotation of the node.
        GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
    }

	if (direction != Vector3.Zero)
    {
        // ...
    }

    // Ground velocity
    // _targetVelocity.X = direction.X * Speed;
    // _targetVelocity.Z = direction.Z * Speed;

    // Vertical velocity
    if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
    {
        _targetVelocity.Y -= FallAcceleration * (float)delta;
    }

    // Moving the character
    

	SetInterest();
	foreach(float i in interest)
	{
		GD.Print("interest " + i);
	}
	
	SetDanger();
	foreach(float i in danger)
	{
		GD.Print("danger " + i);
	}
	
	ChooseDirection();
	GD.Print("direction  " + chosen_dir);
	// GD.Print("chosen dir " + chosen_dir);
	_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
	Velocity = Velocity.Lerp(_targetVelocity, steer_force);
	if(Velocity > Vector3.Zero)
	{
		blend_direction.Y = 1; // Sets animation to walk
	}	
	var prev_y_rotation = GlobalRotation.Y;
	LookAt(GlobalPosition + chosen_dir);
	var current_y_rotation = GlobalRotation.Y;
			if(prev_y_rotation != current_y_rotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.05f)}; // smoothly rotates between the previous angle and the new angle!
			}
	tree.Set("parameters/IW/blend_position", blend_direction);
    MoveAndSlide();


	
		// var desired_velocity = chosen_dir.Rotated(entity.direction, ) * max_speed;

	}

	public void SetInterest()
	{
		for(int i = 0; i < num_rays; i++)
		{
			var d = ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y).Dot(Transform.Basis.Z);
			
			GD.Print("d " + d);
			interest[i] = MathF.Max(0, d);
			// GD.Print(interest[i]);
		}
	}

	public void SetDanger()
	{
		var space_state = GetWorld3D().DirectSpaceState;
		for(int i = 0; i < num_rays; i++)
		{
			var ray_query = PhysicsRayQueryParameters3D.Create(ray_origin, ray_origin + ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * look_ahead);
			var result = space_state.IntersectRay(ray_query);
		
			// GD.Print("Result " + result);
			if(result.Count > 0)
			{
				danger[i] = 1.0f;
			}
			else
			{
				danger[i] = 0.0f;
			}
			// GD.Print("Danger i: " + i + " " + danger[i]);
		}
		
	}

	public void ChooseDirection()
	{
		for(int i = 0; i < num_rays; i++)
		{
			if(danger[i] > 0.0)
			{
				interest[i] = 0.0f;
			}
		}

		chosen_dir = Vector3.Zero;
		
		for(int i = 0; i < num_rays; i++)
		{
			chosen_dir += ray_directions[i] * interest[i];
		}

		chosen_dir = chosen_dir.Normalized();
		// GD.Print("chosen dir " + chosen_dir);
	}

}
