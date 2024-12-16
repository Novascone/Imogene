using Godot;
using System;
using System.Reflection.Metadata;

public partial class DissolveCollisionItem : StaticBody3D
{

	[Export] RayCastDissolve ray_cast_dissolve;
	MeshInstance3D mesh;
	bool is_colliding;
	float opacity = 1.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ray_cast_dissolve.colliding += HandleCollision;
		ray_cast_dissolve.notcolliding += HandleNotColliding;
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
	}

    private void HandleNotColliding()
    {
        is_colliding = false;
    }

    private void HandleCollision(int id)
    {
		
        if(id ==(int)GetInstanceId())
		{
			is_colliding = true;
		}
		else
		{
			is_colliding = false;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
		if(is_colliding)
		{
			opacity = (float)Mathf.Lerp(opacity, 0.2, delta * 5);
			// GD.Print("Changing opacity");
        }
		else
		{
			opacity = (float)Mathf.Lerp(opacity, 1.0, delta * 5);
			
		}
		SetOpacity();
	}

	public void SetOpacity()
	{
		mesh.SetInstanceShaderParameter("opacity", opacity);
	}
}
