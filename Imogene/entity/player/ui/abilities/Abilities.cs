using Godot;
using System;

public partial class Abilities : Control
{
	[Export] public Binds binds;
	[Export] public Categories categories;
	[Export] public Passives assigned_passives;
	
	public PassiveBindButton passive_button_pressed;

	[Signal] public delegate void AbilitiesClosedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Subscribe to the ButtonDown signal for each cross bind button in each cross
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

		// subscribe to the accept and cancel buttons in new assignment
		categories.new_assignment.accept.ButtonDown += OnNewAssignmentAccept;
		categories.new_assignment.cancel.ButtonDown += OnNewAssignmentCancel;

		// subscribe to the ability reassigned signal 
		categories.AbilityReassigned += OnAbilityReassigned;
	}

    private void OnAbilityReassigned(string cross, string level, string bind, string ability_name, Texture2D icon)
    {
        GD.Print("Abilities received ability reassignment");
    }

    private void OnNewAssignmentCancel()
    {
        throw new NotImplementedException();
    }

    private void OnNewAssignmentAccept()
    {
		categories.Hide();
		binds.Show();
    }

    private void HandlePassiveBindButtonPressed(PassiveBindButton passive_bind_button) // If a passive bind button is pressed set the categories to passive, hide binds, show categories
    {
		categories.passive = true;
		categories.active = false;
        passive_button_pressed = passive_bind_button;
		binds.Hide();
		categories.Show();
		
    }

    private void OnCrossBindDown(CrossBindButton cross_button) // When a cross bind button is pressed set categories to active give the new assignment information about the ability on the cross bind button
    {
		categories.active = true;
		categories.passive = false;
		categories.new_assignment.old_ability_name = cross_button.ability_name;
		categories.new_assignment.old_cross = cross_button.cross;
		categories.new_assignment.old_level = cross_button.level;
		categories.new_assignment.old_button_bind = cross_button.button_bind;
		categories.cross_bind_selected = cross_button;
		binds.Hide();
		categories.Show();
		
    }

	public void _on_close_button_down() // Reset page and child pages, hide this page
	{
		ResetPage();
		categories.ResetPage();
		Hide();
		EmitSignal(nameof(AbilitiesClosed));
	}

	public void ResetPage() // Reset this page
	{
		binds.Show();
		categories.Hide();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
