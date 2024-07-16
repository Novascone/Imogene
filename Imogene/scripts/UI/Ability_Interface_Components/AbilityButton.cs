using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

// Button inside of ability assignment UI
public partial class AbilityButton : Button
{
	[Export]
	public bool assigned;
	public AbilityCategory category;
	public string button_assigned; // The cross slot the current ability/ ability button is assigned to
	public Button assign_button; // The cross slot the ability is going to be assigned to
	public Button icon_button; // The icon of the current button assigned/ the one thats going to be assigned
	public string ability_name;
	public AbilityResource ability_resource;
	public Control info; // Info about the skill
	public RichTextLabel info_text; // Info text
	public string ability_type; // Type of ability e.g. melee, movement etc
	private CustomSignals _customSignals;

	public PackedScene[] modifiers = new PackedScene[5]; // Modifiers for abilities, not yet implemented
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		info = GetNode<Control>("Info");
		info_text = GetNode<RichTextLabel>("Info/MarginContainer/Panel/RichTextLabel");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.ButtonToBeAssigned += HandleButtonToBeAssigned;
		// _customSignals.AbilityCancel += HandleAbilityCancel;
		// _customSignals.AvailableAbilities += HandleAvailableAbilities;
	}


    // private void HandleButtonToBeAssigned(Button cross_button, Button representative_button)
    // {
	// 	// GD.Print("Cross Button name " + cross_button.Name);
	// 	// GD.Print("Representative Button name " + representative_button.Name);
    //     assign_button = cross_button;
	// 	icon_button = representative_button;
    // }

	public void ButtonToBeAssigned(Button cross_button, Button representative_button)
	{
		GD.Print("cross button " + cross_button.Name);
		assign_button = cross_button;
		icon_button = representative_button;
		// GD.Print("Cross Button name " + cross_button.Name);
		// GD.Print("Representative Button name " + representative_button.Name);
	}

	public void AbilityAssigned(string button_name)
	{
		button_assigned = button_name;
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}

	public void _on_button_down()
	{
		GD.Print("assign_button name " + assign_button.Name);
		assign_button.Icon = this.Icon;
		icon_button.Icon = this.Icon;
		// GD.Print(icon_button.Name);
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilitySelected), this, assign_button);
		GD.Print("assign button " + assign_button.Name);
		category.AbilitySelected(this, assign_button);
		
	}
	public void _on_focus_entered()
	{
		info.Show();
	}

	public void _on_focus_exited()
	{
		info.Hide();
	}

	public void AbilityCancel()
	{
		assign_button.Icon = null;
		icon_button = null;
	}

	// private void HandleAbilityCancel()
	// {
	// 	assign_button.Icon = null;
	// 	icon_button = null;
	// }
}
