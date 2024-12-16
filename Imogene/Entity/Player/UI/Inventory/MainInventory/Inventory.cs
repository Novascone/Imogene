using Godot;
using System;

public partial class Inventory : Control
{

	[Export] public MainInventory main { get; set; }
	[Export] public DepthSheet depth_sheet { get; set; }
	[Export] public Control mats { get; set; }
	[Export] public Control temp_buttons { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main.character_outline.sheet.ButtonDown += OnSheetButtonDown;
	}

    private void OnSheetButtonDown()
    {
        depth_sheet.Visible = !depth_sheet.Visible;
    }
	
}
