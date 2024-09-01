using Godot;
using System;

public partial class NewUI : Control
{
	[Export] public NewHUD hud;
	[Export] public NewInventory inventory;
	[Export] public NewAbilities abilities;
	[Export] public NewJournal journal;
	[Export] public NewCursor cursor;

	public Button hovered_button;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(ItemButton item_button in inventory.main.items.GetChildren())
		{
			item_button.CursorHovering += HandleCursorOverItemButton;
			item_button.CursorLeft += HandleCursorLeftItemButton;
		}
	}

    

    private void HandleCursorOverItemButton(ItemButton item_button)
    {
        GD.Print("cursor over item button");
		hovered_button = item_button;
		hovered_button.GrabFocus();
		GD.Print(hovered_button.Name);
    }

	private void HandleCursorLeftItemButton(ItemButton item_button)
    {
        GD.Print("cursor left item button");
		if(hovered_button == item_button)
		{
			hovered_button.ReleaseFocus();
			hovered_button = null;
		}
		
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		cursor.ControllerCursor();

		if(Input.IsActionJustPressed("Inventory"))
		{
			inventory.Visible = !inventory.Visible;
		}
	}

	
}
