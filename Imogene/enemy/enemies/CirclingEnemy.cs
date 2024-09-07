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
			blend_direction.Y = 1; // Sets animation to walk
		}
		if(Velocity.IsEqualApprox(Vector3.Zero))
		{
			blend_direction.Y = 0;
		}
		if(entity_in_alert_area)
		{
			
			// target_ability.Execute(this);
			look_at_position = box_position;
			LookAt(look_at_position with {Y = GlobalPosition.Y});
			
		}
		else
		{
			entity_in_alert_area = false;
			// Sets the animation to walk forward when not targeting
			if(chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) != Vector3.Zero)
			{
				blend_direction.X = 0;
				blend_direction.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
			}
		}
		_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();
	}

	public override void OnAlertAreaEntered(Area3D area) 
    {
		
		if(area.IsInGroup("RotateBox"))
		{
			GD.Print("In contact with rotate box");
			entity_in_alert_area = true;
			box_position = area.GlobalPosition;
		}
    }
}
