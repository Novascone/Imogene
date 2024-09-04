using Godot;
using System;

public partial class NewAbilities : Control
{
	[Export] public Binds binds;
	[Export] public Categories categories;
	[Export] public Passives assigned_passives;
	public CrossBindButton button_selected;
	public PassiveBindButton passive_button_pressed;

	[Signal] public delegate void AbilitiesClosedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Control control in binds.l_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
				
			}
		}

		foreach(Control control in binds.l_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}

		foreach(Control control in binds.r_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}

		foreach(Control control in binds.r_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}

		foreach(Control control in binds.passives.GetChildren())
		{
			if(control is PassiveBindButton passive_bind_button)
			{
				passive_bind_button.PassiveBindButtonPressed += HandlePassiveBindButtonPressed;
			}
		}
	}

    private void HandlePassiveBindButtonPressed(PassiveBindButton passive_bind_button)
    {
		categories.passive = true;
		categories.active = false;
        passive_button_pressed = passive_bind_button;
		binds.Hide();
		categories.Show();
		
    }

    private void OnCrossBindDown(CrossBindButton cross_button)
    {
		categories.active = true;
		categories.passive = false;
		button_selected = cross_button;
		binds.Hide();
		categories.Show();
		GD.Print("Button selected " + button_selected.button_bind, " on side " + button_selected.side + " at level " + button_selected.level);
    }

	public void _on_close_button_down()
	{
		ResetPage();
		categories.ResetPage();
		Hide();
		EmitSignal(nameof(AbilitiesClosed));
	}

	public void ResetPage()
	{
		binds.Show();
		categories.Hide();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
