using Godot;
using System;
using System.Linq;

public partial class Abilities : Control
{
	[Export] public Binds Binds { get; set; }
	[Export] public Categories Categories { get; set; }
	[Export] public Passives AssignedPassives { get; set; }
	public PassiveBindButton PassiveButtonPressed { get; set; }

	[Signal] public delegate void AbilitiesClosedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Subscribe to the ButtonDown signal for each cross bind button in each cross
		foreach(Control control in Binds.l_cross_primary_assignment.GetChildren().Cast<Control>())
		{
			if(control is CrossBindButton crossBind)
			{
				crossBind.ButtonDown += () => OnCrossBindDown(crossBind);
			}
		}

		foreach(Control control in Binds.l_cross_secondary_assignment.GetChildren().Cast<Control>())
		{
			if(control is CrossBindButton crossBind)
			{
				crossBind.ButtonDown += () => OnCrossBindDown(crossBind);
			}
		}

		foreach(Control control in Binds.r_cross_primary_assignment.GetChildren().Cast<Control>())
		{
			if(control is CrossBindButton crossBind)
			{
				crossBind.ButtonDown += () => OnCrossBindDown(crossBind);		
			}
		}

		foreach(Control control in Binds.r_cross_secondary_assignment.GetChildren().Cast<Control>())
		{
			if(control is CrossBindButton crossBind)
			{
				crossBind.ButtonDown += () => OnCrossBindDown(crossBind);
			}
		}

		foreach(Control control in Binds.passives.GetChildren().Cast<Control>())
		{
			if(control is PassiveBindButton passiveBindButton)
			{
				passiveBindButton.PassiveBindButtonPressed += HandlePassiveBindButtonPressed;
			}
		}

		Categories.new_assignment.accept.ButtonDown += OnNewAssignmentAccept;
		Categories.new_assignment.cancel.ButtonDown += OnNewAssignmentCancel;

		Categories.AbilityReassigned += OnAbilityReassigned;
	}

    private void OnAbilityReassigned(Ability.Cross cross, Ability.Tier tier, string bind, string abilityName, Texture2D icon)
    {
        
    }

    private void OnNewAssignmentCancel()
    {
        throw new NotImplementedException();
    }

    private void OnNewAssignmentAccept()
    {
		Categories.Hide();
		Binds.Show();
    }

    private void HandlePassiveBindButtonPressed(PassiveBindButton passiveBindButton) // If a passive bind button is pressed set the categories to passive, hide binds, show categories
    {
		Categories.passive = true;
		Categories.active = false;
        PassiveButtonPressed = passiveBindButton;
		Binds.Hide();
		Categories.Show();
		
    }

    private void OnCrossBindDown(CrossBindButton crossButton) // When a cross bind button is pressed set categories to active give the new assignment information about the ability on the cross bind button
    {
		Categories.active = true;
		Categories.passive = false;
		Categories.new_assignment.old_ability_name = crossButton.ability_name;
		Categories.new_assignment.old_cross = crossButton.cross;
		Categories.new_assignment.old_tier = crossButton.tier;
		Categories.new_assignment.old_button_bind = crossButton.button_bind;
		Categories.cross_bind_selected = crossButton;
		Binds.Hide();
		Categories.Show();
		
    }

	public void OnCloseButtonDown() // Reset page and child pages, hide this page
	{
		ResetPage();
		Categories.ResetPage();
		Hide();
		EmitSignal(nameof(AbilitiesClosed));
	}

	public void ResetPage() // Reset this page
	{
		Binds.Show();
		Categories.Hide();
	}

}
