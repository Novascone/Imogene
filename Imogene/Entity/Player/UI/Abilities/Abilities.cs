using Godot;
using System;
using System.Linq;

public partial class Abilities : Control
{
	[Export] public Binds binds { get; set; }
	[Export] public Categories categories { get; set; }
	[Export] public Passives assigned_passives { get; set; }
	public PassiveBindButton passive_button_pressed { get; set; }

	[Signal] public delegate void AbilitiesClosedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Subscribe to the ButtonDown signal for each cross bind button in each cross
		foreach(Control _control in binds.l_cross_primary_assignment.GetChildren().Cast<Control>())
		{
			if(_control is CrossBindButton cross_bind)
			{
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control _control in binds.l_cross_secondary_assignment.GetChildren().Cast<Control>())
		{
			if(_control is CrossBindButton cross_bind)
			{
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control _control in binds.r_cross_primary_assignment.GetChildren().Cast<Control>())
		{
			if(_control is CrossBindButton cross_bind)
			{
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);			}
		}

		foreach(Control _control in binds.r_cross_secondary_assignment.GetChildren().Cast<Control>())
		{
			if(_control is CrossBindButton cross_bind)
			{
				cross_bind.ButtonDown += () => OnCrossBindDown(cross_bind);
			}
		}

		foreach(Control _control in binds.passives.GetChildren().Cast<Control>())
		{
			if(_control is PassiveBindButton passive_bind_button)
			{
				passive_bind_button.PassiveBindButtonPressed += HandlePassiveBindButtonPressed;
			}
		}

		categories.new_assignment.accept.ButtonDown += OnNewAssignmentAccept;
		categories.new_assignment.cancel.ButtonDown += OnNewAssignmentCancel;

		categories.AbilityReassigned += OnAbilityReassigned;
	}

    private void OnAbilityReassigned(Ability.Cross cross_, Ability.Tier tier_, string bind_, string ability_name_, Texture2D icon_)
    {
        
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

    private void HandlePassiveBindButtonPressed(PassiveBindButton passive_bind_button_) // If a passive bind button is pressed set the categories to passive, hide binds, show categories
    {
		categories.passive = true;
		categories.active = false;
        passive_button_pressed = passive_bind_button_;
		binds.Hide();
		categories.Show();
		
    }

    private void OnCrossBindDown(CrossBindButton cross_button_) // When a cross bind button is pressed set categories to active give the new assignment information about the ability on the cross bind button
    {
		categories.active = true;
		categories.passive = false;
		categories.new_assignment.old_ability_name = cross_button_.ability_name;
		categories.new_assignment.old_cross = cross_button_.cross;
		categories.new_assignment.old_tier = cross_button_.tier;
		categories.new_assignment.old_button_bind = cross_button_.button_bind;
		categories.cross_bind_selected = cross_button_;
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

}
