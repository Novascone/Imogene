using Godot;
using System;

public partial class CameraRig : Node3D
{
	[Export] public Camera3D camera { get; set; }
	private int default_camera_size { get; set; } = 21;
	private int zoom_camera_size { get; set; } = 10;
	private bool zoomed { get; set; } = false;

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
