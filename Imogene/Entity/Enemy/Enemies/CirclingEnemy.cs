using Godot;
using System;

public partial class CirclingEnemy : Enemy
{
	public Vector3 box_position;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Velocity > Vector3.Zero )
		{
			BlendDirection.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			BlendDirection.Y = 0;
		}
		if(EntityInAlertArea)
		{
			
			// target_ability.Execute(this);
			LookAtPosition = box_position;
			LookAt(LookAtPosition with {Y = GlobalPosition.Y});
			
		}
		else
		{
			EntityInAlertArea = false;
			// Sets the animation to walk forward when not targeting
			if(ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) != Vector3.Zero)
			{
				BlendDirection.X = 0;
				BlendDirection.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				BlendDirection.X = Mathf.Lerp(BlendDirection.X, 0, 0.1f);
				BlendDirection.Y = Mathf.Lerp(BlendDirection.Y, 0, 0.1f);
			}
		}
		TargetVelocity = ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * MaxSpeed;
		Velocity = Velocity.Lerp(TargetVelocity, SteerForce);
		Tree.Set("parameters/IW/blend_position", BlendDirection);
		MoveAndSlide();
	}

	public override void OnAlertAreaEntered(Area3D area) 
    {
		
		if(area.IsInGroup("RotateBox"))
		{
			GD.Print("In contact with rotate box");
			EntityInAlertArea = true;
			box_position = area.GlobalPosition;
		}
    }
}
