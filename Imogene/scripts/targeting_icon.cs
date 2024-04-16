using Godot;
using System;

public partial class targeting_icon : Node3D
{

	private MeshInstance3D icon;
	private CustomSignals _customSignals;
	private bool visible = true;
	private Vector3 icon_position;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon = GetNode<MeshInstance3D>("TargetingIcon");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.Targeting += HandleTargeting;
	}

   
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(visible)
		{
			icon.Visible = true;
			icon.GlobalPosition = icon_position with {Y = 4};
		}
		else
		{
			icon.Visible = false;
			icon.GlobalPosition = Vector3.Zero;
		}
	}

	 

    private void HandleTargeting(bool targeting, Vector3 position)
    {
		visible = targeting;
		icon_position = position;
    }
}
