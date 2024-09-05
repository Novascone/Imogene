using Godot;
using System;

public partial class NewAbilities : Control
{
	[Export] public Binds binds;
	[Export] public Categories categories;
	[Export] public Passives assigned_passives;
	
	public PassiveBindButton passive_button_pressed;

	[Signal] public delegate void AbilitiesClosedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Control control in binds.l_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				// cross_bind.CrossButtonPressed += OnCrossBindDown;
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control control in binds.l_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				// cross_bind.CrossButtonPressed += OnCrossBindDown;
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control control in binds.r_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				// cross_bind.CrossButtonPressed += OnCrossBindDown;
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);			}
		}

		foreach(Control control in binds.r_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				// cross_bind.CrossButtonPressed += OnCrossBindDown;
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control control in binds.passives.GetChildren())
		{
			if(control is PassiveBindButton passive_bind_button)
			{
				passive_bind_button.PassiveBindButtonPressed += HandlePassiveBindButtonPressed;
			}
		}

		categories.assigned_accepted.accept.ButtonDown += OnAssignedAcceptAccept;
		categories.assigned_accepted.cancel.ButtonDown += OnAssignedAcceptCancel;

		categories.AbilityReassigned += OnAbilityReassigned;
	}

    private void OnAbilityReassigned(string cross, string level, string bind, string ability_name, Texture2D icon)
    {
        GD.Print("Abilities received ability reassignment");
    }

    private void OnAssignedAcceptCancel()
    {
        throw new NotImplementedException();
    }

    private void OnAssignedAcceptAccept()
    {
		categories.Hide();
		binds.Show();
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
		categories.assigned_accepted.old_ability_name = cross_button.ability_name;
		categories.assigned_accepted.old_cross = cross_button.cross;
		categories.assigned_accepted.old_level = cross_button.level;
		categories.assigned_accepted.old_button_bind = cross_button.button_bind;
		categories.button_selected = cross_button;
		binds.Hide();
		categories.Show();
		
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
