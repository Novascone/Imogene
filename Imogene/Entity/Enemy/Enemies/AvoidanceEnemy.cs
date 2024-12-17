using Godot;
using System;

public partial class AvoidanceEnemy : Enemy
{

	public bool running_away_from_chaser;
	public Vector3 center_position;
	public Node3D chaser;
	public bool can_see_center;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		running_away_from_chaser = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(Velocity > Vector3.Zero )
		{
			BlendDirection.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			BlendDirection.Y = 0;
		}
		if(chaser != null) 
		{
			if(GlobalPosition.DistanceTo(chaser.GlobalPosition) < 5)
			{
				running_away_from_chaser = true;
				MaxSpeed = 8;
				
			}
			else if (GlobalPosition.DistanceTo(chaser.GlobalPosition) > 15)
			{
				running_away_from_chaser = false;
				MaxSpeed = 4;
			
			}
			GD.Print("Distance from chaser " + GlobalPosition.DistanceTo(chaser.GlobalPosition));
		}
		if(can_see_center && GlobalPosition.DistanceTo(center_position) < 2 && !running_away_from_chaser) // If the entity is under the center stop moving
		{
			GD.Print("Stop moving");
			TargetVelocity = Vector3.Zero;
			Velocity = TargetVelocity;
			BlendDirection = Vector2.Zero;
			GD.Print(BlendDirection);
		}
		else
		{
			TargetVelocity = ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * MaxSpeed;
			Velocity = Velocity.Lerp(TargetVelocity, SteerForce);
		}
		Tree.Set("parameters/IW/blend_position", BlendDirection);
		MoveAndSlide();
	}

	public override void OnAlertAreaEntered(Area3D area) 
    {
		
		if(area.IsInGroup("Center"))
		{
			GD.Print("within range of center");
			can_see_center = true;
			center_position = area.GlobalPosition with {Y = 0};
			GD.Print(center_position);
		}
		
		
    }

	public override void OnAlertAreaBodyEntered(Node3D body) 
	{
		if(body is Chaser chaser_entered )
		{
			GD.Print("Chaser in detection");
			chaser = chaser_entered;
		}
	}

	// public override void OnAlertAreaBodyExited(Node3D body)
    // {
		
        
    // }
}
