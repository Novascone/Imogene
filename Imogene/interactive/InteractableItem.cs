using Godot;
using System;

public partial class InteractableItem : Node3D
{
	[Export] public MeshInstance3D item_highlight_mesh_white {get; set;}
	[Export] public MeshInstance3D item_highlight_mesh_red {get; set;}

	public void GainFocus(bool inventory_full)
	{
		if(!inventory_full)
		{
			item_highlight_mesh_red.Visible = false;
			item_highlight_mesh_white.Visible = true;
			
		}
		else
		{
			item_highlight_mesh_white.Visible = false;
			item_highlight_mesh_red.Visible = true;
		}
		
	}

	public void LoseFocus()
	{
		item_highlight_mesh_white.Visible = false;
		item_highlight_mesh_red.Visible = false;
	}
	
}
