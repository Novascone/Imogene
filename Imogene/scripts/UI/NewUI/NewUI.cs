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
		inventory.main.bottom_buttons.abilities.ButtonDown += OnAbilitiesButtonDown;
	}

    private void OnAbilitiesButtonDown()
    {
        abilities.Show();
		inventory.Hide();
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
