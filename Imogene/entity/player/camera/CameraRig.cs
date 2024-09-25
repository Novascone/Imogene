using Godot;
using System;

public partial class CameraRig : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public Camera3D camera; // Gets camera node
	private int default_camera_size = 21; // Controls camera zoom
	private int zoom_camera_size = 10; // Controls camera zoom
	private bool zoomed;
	private Player player;
	private CustomSignals _customSignals;
	
	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera");	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// _customSignals.EmitSignal(nameof(CustomSignals.CameraPosition), camera.GlobalTransform.Origin); // Sends camera position
		// Check if zoomed, zoom if needed
		// if(!zoomed)
		// {
		// 	if (player.ui.inventory_open || player.ui.abilities_open)
		// 	{
		// 		camera.Size = zoom_camera_size;
		// 		zoomed = true;
		// 	}
		// }
		// else if (zoomed)
		// {
		// 	if(!player.ui.inventory_open && !player.ui.abilities_open)
		// 	{
		// 		camera.Size = default_camera_size;
		// 		zoomed = false;
		// 	}
		// }
	}

	public void Zoom()
	{
		if(!zoomed)
		{
			camera.Size = zoom_camera_size;
			zoomed = true;
		}
		else if (zoomed)
		{

			camera.Size = default_camera_size;
			zoomed = false;

		}
	}


}
