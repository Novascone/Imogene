using Godot;
using System;

public partial class CrossAssignment : Button
{
	public AbilitiesInterface this_interface;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_down() // Set the selected button and the input of that selected button
	{
		this_interface.selected_button = this;
		if(Name == "LCrossPrimaryUpAssign"){ this_interface.button_to_be_assigned_text = "Primary RB";}
		if(Name == "LCrossPrimaryLeftAssign"){ this_interface.button_to_be_assigned_text = "Primary LB";}
		if(Name == "LCrossPrimaryRightAssign"){ this_interface.button_to_be_assigned_text = "Primary RT";}
		if(Name == "LCrossPrimaryDownAssignment"){ this_interface.button_to_be_assigned_text = "Primary LT";}

		if(Name == "RCrossPrimaryUpAssign"){ this_interface.button_to_be_assigned_text = "Primary Y";}
		if(Name == "RCrossPrimaryLeftAssign"){ this_interface.button_to_be_assigned_text = "Primary X";}
		if(Name == "RCrossPrimaryRightAssign"){ this_interface.button_to_be_assigned_text = "Primary B";}
		if(Name == "RCrossPrimaryDownAssign"){ this_interface.button_to_be_assigned_text = "Primary A";}

		if(Name == "LCrossSecondaryUpAssign"){ this_interface.button_to_be_assigned_text = "Secondary RB";}
		if(Name == "LCrossSecondaryLeftAssign"){ this_interface.button_to_be_assigned_text = "Secondary LB";}
		if(Name == "LCrossSecondaryRightAssign"){ this_interface.button_to_be_assigned_text = "Secondary RT";}
		if(Name == "LCrossSecondaryDownAssign"){ this_interface.button_to_be_assigned_text = "Secondary LT";}

		if(Name == "RCrossSecondaryUpAssign"){ this_interface.button_to_be_assigned_text = "Secondary Y";}
		if(Name == "RCrossSecondaryLeftAssign"){ this_interface.button_to_be_assigned_text = "Secondary X";}
		if(Name == "RCrossSecondaryRightAssign"){ this_interface.button_to_be_assigned_text = "Secondary B";}
		if(Name == "RCrossSecondaryDownAssign"){ this_interface.button_to_be_assigned_text = "Secondary A";}
		GD.Print("button text from cross assign: " + Name + " " + this_interface.button_to_be_assigned_text );
		this_interface.AssignmentButtonNavigation();
		// _customSignals.EmitSignal(nameof(CustomSignals.ButtonName), selected_button, "Primary RB");
	
	
		// _customSignals.EmitSignal(nameof(CustomSignals.CurrentAbilityBoundOnCrossButton), selected_button.Icon);
		// assign_ability.Show();
		
	}

	public void GetAbilityInterfaceInfo(AbilitiesInterface i)
	{
		this_interface = i;
	}
}
