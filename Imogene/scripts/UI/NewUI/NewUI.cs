using Godot;
using System;

public partial class NewUI : Control
{
	[Export] public NewHUD hud;
	[Export] public NewInventory inventory;
	[Export] public NewAbilities abilities;
	[Export] public NewJournal journal;
	[Export] public NewCursor cursor;

	public bool ui_preventing_movement;

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
		UIElementOpen();
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		cursor.ControllerCursor();
		
		if(Input.IsActionJustPressed("Inventory"))
		{
			inventory.Visible = !inventory.Visible;
			cursor.Visible = !cursor.Visible;
		}

		if(ui_preventing_movement)
		{
			if(Input.IsActionJustPressed("CursorUp") || Input.IsActionJustPressed("CursorDown") || Input.IsActionJustPressed("CursorLeft") || Input.IsActionJustPressed("CursorRight"))
			{
				cursor.Show();
			}
		}
		if(Input.IsActionJustPressed("D-PadUp") || Input.IsActionJustPressed("D-PadDown") || Input.IsActionJustPressed("D-PadLeft") || Input.IsActionJustPressed("D-PadRight"))
		{
			cursor.Hide();
		}
		
	}

	public void AssignAbility(Ability ability)
	{
		hud.main.AssignAbility(ability);
	}

	public void UIElementOpen()
	{
		foreach(Control control in GetChildren())
		{
			if(control.IsInGroup("prevents_movement"))
			{
				if(control.Visible)
				{
					ui_preventing_movement = true;
				}
				else
				{
					ui_preventing_movement = false;
				}
			}
		}
	}
	
}
