using Godot;
using System;

public partial class InteractableItem : Node3D
{
	[Export] public MeshInstance3D ItemHighlightMeshWhite {get; set;}
	[Export] public MeshInstance3D ItemHighlightMeshRed {get; set;}
	public bool InteractToPickUp;

	public void GainFocus(bool inventoryFull)
	{
		if(!inventoryFull)
		{
			ItemHighlightMeshRed.Visible = false;
			ItemHighlightMeshWhite.Visible = true;
			
		}
		else
		{
			ItemHighlightMeshWhite.Visible = false;
			ItemHighlightMeshRed.Visible = true;
		}
		
	}

	public void LoseFocus()
	{
		ItemHighlightMeshWhite.Visible = false;
		ItemHighlightMeshRed.Visible = false;
	}
	
}
