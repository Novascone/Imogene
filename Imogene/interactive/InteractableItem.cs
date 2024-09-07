using Godot;
using System;

public partial class InteractableItem : Node3D
{
	[Export] public MeshInstance3D item_highlight_mesh {get; set;}

	public void GainFocus()
	{
		item_highlight_mesh.Visible = true;
	}

	public void LoseFocus()
	{
		item_highlight_mesh.Visible = false;
	}
	
}
