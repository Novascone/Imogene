using Godot;
using System;

public partial class HerdEnemy : Enemy
{
	public bool near_herd_mate;
	public Vector3 herd_mate_position;
	public Vector3 interest_position;
	private Area3D herd_area;
	public Node3D herd_mate;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		herd_area = GetNode<Area3D>("Areas/HerdDetection");
		herd_area.AreaEntered += OnHerdDetectionEntered;
		herd_area.AreaExited += OnHerdDetectionExited;
		herd_area.BodyEntered += OnHerdBodyEntered;
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
		if(herd_mate_position != Vector3.Zero)
		{
			GD.Print("herd mate position " + herd_mate_position + " from " + GetParent().Name);
			GD.Print(herd_mate);
		}
		if(herd_mate != null) 
		{
			herd_mate_position = herd_mate.GlobalPosition;
			// if(GlobalPosition.DistanceTo(herd_mate.GlobalPosition) < 5)
			// {
			// 	running_away_from_chaser = true;
			// 	max_speed = 8;
				
			// }
			// else if (GlobalPosition.DistanceTo(chaser.GlobalPosition) > 15)
			// {
			// 	running_away_from_chaser = false;
			// 	max_speed = 4;
			
			// }
			
		}
		_targetVelocity = chosen_dir.Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) * max_speed;
		Velocity = Velocity.Lerp(_targetVelocity, steer_force);
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();
	}

	public override void OnAlertAreaEntered(Area3D area) 
    {
		
		if(area.IsInGroup("InterestPoint"))
		{
			GD.Print("within range of interest position");
			interest_position = area.GlobalPosition with {Y = 0};
			GD.Print(interest_position);
		}
		
    }

	virtual public void OnHerdDetectionExited(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Away from herd mate");
			near_herd_mate = false;
			
		}
    }

    virtual public void OnHerdDetectionEntered(Area3D area)
    {
        if(area.IsInGroup("Herd"))
		{
			GD.Print("Near herd mate");
			near_herd_mate = true;
			herd_mate_position = area.GlobalPosition;
		}
    }

	virtual public void OnHerdBodyEntered(Node3D body)
    {
		
        if(body is Enemy herd_entity && body.Name != Name)
		{
			GD.Print(body.Name + " entered detection area of " + Name);
			GD.Print("Herd mate in detection");
			herd_mate = herd_entity;
		}
    }
}
