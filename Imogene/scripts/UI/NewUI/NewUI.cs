using Godot;
using System;

public partial class NewUI : Control
{
	[Export] public NewHUD hud;
	[Export] public NewInventory inventory;
	[Export] public NewAbilities abilities;
	[Export] public NewJournal journal;
	[Export] public NewCursor cursor;

	[Signal] public delegate void InventoryToggleEventHandler();

	public bool preventing_movement;

	public Button hovered_button;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		inventory.main.bottom_buttons.abilities.ButtonDown += OnAbilitiesButtonDown;
		abilities.AbilitiesClosed += OnAbilitiesClosed;
	}

    private void OnAbilitiesClosed()
    {
        EmitSignal(nameof(InventoryToggle));
		preventing_movement = false;
		cursor.Visible = !cursor.Visible;
    }

    private void OnAbilitiesButtonDown()
    {
        abilities.Show();
		inventory.Hide();
		preventing_movement = true;
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(preventing_movement)
		{
			cursor.ControllerCursor();
		}
		
		if(Input.IsActionJustPressed("Inventory"))
		{
			preventing_movement = !preventing_movement;
			if(!abilities.Visible)
			{
				inventory.Visible = !inventory.Visible;
			}
			if(abilities.Visible)
			{
				abilities.ResetPage();
				abilities.Visible = !abilities.Visible;
			}
			cursor.Visible = !cursor.Visible;
			EmitSignal(nameof(InventoryToggle));
		}

		if(preventing_movement)
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

	public void SwitchCrosses(string cross)
	{
		hud.main.SwitchCrosses(cross);
	}

	public void UIElementOpen()
	{
		
		foreach(Control control in GetChildren())
		{
			if(control.IsInGroup("prevents_movement"))
			{
				GD.Print("checking ui prevents movement");
				if(control.Visible)
				{
					GD.Print("UI preventing movement ");
					preventing_movement = true;
				}
				else if(!preventing_movement)
				{
					GD.Print("UI not preventing movement ");
					preventing_movement = false;
				}
			}
		}
	}

    internal void HandleUpdatedStats(Player player)
    {
        hud.main.UpdateHUDStats(player);
    }
}
