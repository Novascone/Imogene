using Godot;
using System;

public partial class CameraRig : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private Camera3D camera;
	private CustomSignals _customSignals;
	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.CameraPosition += HandleCameraPosition;
		
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_customSignals.EmitSignal(nameof(CustomSignals.CameraPosition), camera.GlobalTransform.Origin);
	}

	private void HandleCameraPosition(Vector3 position){}
}
