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
			blend_direction.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			blend_direction.Y = 0;
		}
		_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();
	}
}
