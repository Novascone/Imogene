using Godot;
using System;

public partial class ChestInventory : Inventory
{

	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.InteractPressed += HandleInteractPressed;
	}

    private void HandleInteractPressed(bool in_area)
    {
        if(!Visible)
		{
			Visible = true;
		}
		else
		{
			Visible = false;
		}
		if(!in_area)
		{
			Visible = false;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
