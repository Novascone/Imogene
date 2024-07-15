using Godot;
using System;

public partial class CrossAssignment : Button
{
	public AbilitiesInterface this_interface;
	public string button_text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(Name == "LCrossPrimaryUpAssignment"){ button_text = "Primary RB";}
		if(Name == "LCrossPrimaryLeftAssignment"){ button_text = "Primary LB";}
		if(Name == "LCrossPrimaryRightAssignment"){ button_text = "Primary RT";}
		if(Name == "LCrossPrimaryDownAssignment"){ button_text = "Primary LT";}

		if(Name == "RCrossPrimaryUpAssignment"){ button_text = "Primary Y";}
		if(Name == "RCrossPrimaryLeftAssignment"){ button_text = "Primary X";}
		if(Name == "RCrossPrimaryRightAssignment"){ button_text = "Primary B";}
		if(Name == "RCrossPrimaryDownAssignment"){ button_text = "Primary A";}

		if(Name == "LCrossSecondaryUpAssignment"){ button_text = "Secondary RB";}
		if(Name == "LCrossSecondaryLeftAssignment"){ button_text = "Secondary LB";}
		if(Name == "LCrossSecondaryRightAssignment"){ button_text = "Secondary RT";}
		if(Name == "LCrossSecondaryDownAssignment"){ button_text = "Secondary LT";}

		if(Name == "RCrossSecondaryUpAssignment"){ button_text = "Secondary Y";}
		if(Name == "RCrossSecondaryLeftAssignment"){ button_text = "Secondary X";}
		if(Name == "RCrossSecondaryRightAssignment"){ button_text = "Secondary B";}
		if(Name == "RCrossSecondaryDownAssignment"){ button_text = "Secondary A";}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_down()
	{
		this_interface.selected_button = this;
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
