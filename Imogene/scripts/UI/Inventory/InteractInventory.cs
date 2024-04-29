using Godot;
using System;

public partial class InteractInventory : Inventory
{

	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
