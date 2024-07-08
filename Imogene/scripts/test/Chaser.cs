using Godot;
using System;

public partial class Chaser : CharacterBody3D
{
	public const float Speed = 1.0f;
	public const float JumpVelocity = 4.5f;
	public Area3D chase_area;
	public Vector3 direction_to_chase;
	public NavigationAgent3D navigation_agent;
	public Node3D body_to_chase;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
		GD.Print("chaser ready");
        chase_area = GetNode<Area3D>("ChaseArea");
		chase_area.BodyEntered += OnChaseAreaEntered;
		navigation_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		// SetPhysicsProcess(false);
		// await GetTree().PhysicsFrame;
		// SetPhysicsProcess(true)
    }

    private void OnChaseAreaEntered(Node3D body)
    {
        if(body.IsInGroup("enemy"))
		GD.Print("Entity in chase area");
		{
			body_to_chase = body;
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		// if (!IsOnFloor())
		// 	velocity.Y -= gravity * (float)delta;

		// // Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// 	velocity.Y = JumpVelocity;

		// // Get the input direction and handle the movement/deceleration.
		// // As good practice, you should replace UI actions with custom gameplay actions.
		// Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		// Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		// if (direction != Vector3.Zero)
		// {
		// 	velocity.X = direction.X * Speed;
		// 	velocity.Z = direction.Z * Speed;
		// }
		// else
		// {
		// 	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		// 	velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		// }
		direction_to_chase = body_to_chase.GlobalPosition;
		navigation_agent.TargetPosition = direction_to_chase; 
		var targetPos = navigation_agent.GetNextPathPosition();
		var direction = GlobalPosition.DirectionTo(targetPos);
		
		Velocity = direction * Speed;

		// Velocity = velocity;
		MoveAndSlide();
	}
}
