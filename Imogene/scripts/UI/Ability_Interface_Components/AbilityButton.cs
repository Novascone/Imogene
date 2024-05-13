using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class AbilityButton : Button
{
	[Export]
	public bool assigned;
	public Button assign_button;
	public Button icon_button;
	public string ability_name;

	public string ability_type;

	public Ability current_ability;

	// public List<Ability> abilities;

	private CustomSignals _customSignals;

	public PackedScene[] modifiers = new PackedScene[5];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.ButtonToBeAssigned += HandleButtonToBeAssigned;
		_customSignals.AbilityCancel += HandleAbilityCancel;
		// _customSignals.AvailableAbilities += HandleAvailableAbilities;
	}

    // private void HandleAvailableAbilities(Ability ability)
    // {
    //     abilities.Add(ability);
    // }

    private void HandleButtonToBeAssigned(Button cross_button, Button representative_button)
    {
		GD.Print("Cross Button name " + cross_button.Name);
		GD.Print("Representative Button name " + representative_button.Name);
        assign_button = cross_button;
		icon_button = representative_button;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}

	public void _on_button_down()
	{
		assign_button.Icon = this.Icon;
		icon_button.Icon = this.Icon;
		GD.Print(icon_button.Name);
		_customSignals.EmitSignal(nameof(CustomSignals.AbilitySelected), this, assign_button);
	}

	private void HandleAbilityCancel()
	{
		assign_button.Icon = null;
		icon_button = null;
	}
}
