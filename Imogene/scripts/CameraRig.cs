using Godot;
using System;

public partial class CameraRig : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private Camera3D camera; // Gets camera node
	private int default_camera_size = 20; // Controls camera zoom
	private int zoom_camera_size = 10; // Controls camera zoom
	private CustomSignals _customSignals;
	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.CameraPosition += HandleCameraPosition;
		_customSignals.ZoomCamera += HandleZoomCamera;
		
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_customSignals.EmitSignal(nameof(CustomSignals.CameraPosition), camera.GlobalTransform.Origin); // Sends camera position
	}

	private void HandleCameraPosition(Vector3 position){}


	//  ************************************************* Need to add tween for camera zoom **********************************************************
	private void HandleZoomCamera(bool zoom)
	{
		if(zoom)
		{
			camera.Size = zoom_camera_size;
		}
		else
		{
			camera.Size = default_camera_size;
		}
	}
}
