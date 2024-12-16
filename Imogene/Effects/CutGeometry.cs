using Godot;
using System;

[Tool]
public partial class CutGeometry : MeshInstance3D
{

	public MeshInstance3D cut_plane;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cut_plane = GetNode<MeshInstance3D>("%CutPlane");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		((ShaderMaterial)MaterialOverride).SetShaderParameter("cutplane", cut_plane.Transform);
	}
}
