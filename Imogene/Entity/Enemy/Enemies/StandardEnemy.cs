using Godot;
using System;

public partial class StandardEnemy : Enemy
{
	public override void _Ready()
	{
		base._Ready();
		
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
		TargetVelocity = ChosenDir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * MaxSpeed;
		Velocity = Velocity.Lerp(TargetVelocity, SteerForce);
		Tree.Set("parameters/IW/blend_position", BlendDirection);
		MoveAndSlide();
	}
}
