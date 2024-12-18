using Godot;
using System;

public partial class CameraRig : Node3D
{
	[Export] public Camera3D Camera { get; set; }
	private int DefaultCameraSize { get; set; } = 21;
	private int ZoomCameraSize { get; set; } = 10;
	private bool Zoomed { get; set; } = false;

	public void Zoom()
	{
		if(!Zoomed)
		{
			Camera.Size = ZoomCameraSize;
			Zoomed = true;
		}
		else if (Zoomed)
		{

			Camera.Size = DefaultCameraSize;
			Zoomed = false;

		}
	}


}
