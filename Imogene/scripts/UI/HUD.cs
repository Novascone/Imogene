using Godot;
using System;

public partial class HUD : UI
{
	public GridContainer l_cross_primary;
	public Button l_cross_primary_up_action_button;
	public Button l_cross_primary_right_action_button;
	public Button l_cross_primary_left_action_button;
	public Button l_cross_primary_down_action_button;
	public Label l_cross_primary_up_action_label;
	public Label l_cross_primary_down_action_label;
	public Label l_cross_primary_left_action_label;
	public Label l_cross_primary_right_action_label;


	public GridContainer r_cross_primary;
	public Button r_cross_primary_up_action_button;
	public Button r_cross_primary_right_action_button;
	public Button r_cross_primary_left_action_button;
	public Button r_cross_primary_down_action_button;
	public Label r_cross_primary_up_action_label;
	public Label r_cross_primary_down_action_label;
	public Label r_cross_primary_left_action_label;
	public Label r_cross_primary_right_action_label;

	public GridContainer l_cross_secondary;
	public Button l_cross_secondary_up_action_button;
	public Button l_cross_secondary_right_action_button;
	public Button l_cross_secondary_left_action_button;
	public Button l_cross_secondary_down_action_button;
	public Label l_cross_secondary_up_action_label;
	public Label l_cross_secondary_down_action_label;
	public Label l_cross_secondary_left_action_label;
	public Label l_cross_secondary_right_action_label;

	public GridContainer r_cross_secondary;
	public Button r_cross_secondary_up_action_button;
	public Button r_cross_secondary_right_action_button;
	public Button r_cross_secondary_left_action_button;
	public Button r_cross_secondary_down_action_button;
	public Label r_cross_secondary_up_action_label;
	public Label r_cross_secondary_down_action_label;
	public Label r_cross_secondary_left_action_label;
	public Label r_cross_secondary_right_action_label;

	public Button consumable_1;
	public Button consumable_2;
	public Button consumable_3;
	public Button consumable_4;

	private bool l_cross_primary_selected = true;
	private bool r_cross_primary_selected = true;
	private bool l_cross_secondary_selected = false;
	private bool r_cross_secondary_selected = false;
	// Called when the node enters the scene tree for the first time.
	// private CustomSignals _customSignals; // Instance of CustomSignals
	public override void _Ready()
	{
		l_cross_primary = GetNode<GridContainer>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary");
		l_cross_primary_up_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryUpAction");
		l_cross_primary_right_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryRightAction");
		l_cross_primary_left_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryLeftAction");
		l_cross_primary_down_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryDownAction");
		l_cross_primary_up_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryUpAction/Label");
		l_cross_primary_down_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryDownAction/Label");
		l_cross_primary_left_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryLeftAction/Label");
		l_cross_primary_right_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryRightAction/Label");


		r_cross_primary = GetNode<GridContainer>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary");
		r_cross_primary_up_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryUpAction");
		r_cross_primary_right_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryRightAction");
		r_cross_primary_left_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryLeftAction");
		r_cross_primary_down_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryDownAction");
		r_cross_primary_up_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryUpAction/Label");
		r_cross_primary_down_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryDownAction/Label");
		r_cross_primary_left_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryLeftAction/Label");
		r_cross_primary_right_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary/RCrossPrimaryRightAction/Label");

		l_cross_secondary = GetNode<GridContainer>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary");
		l_cross_secondary_up_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryUpAction");
		l_cross_secondary_right_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryRightAction");
		l_cross_secondary_left_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryLeftAction");
		l_cross_secondary_down_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryDownAction");
		l_cross_secondary_up_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryUpAction/Label");
		l_cross_secondary_down_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryDownAction/Label");
		l_cross_secondary_left_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryLeftAction/Label");
		l_cross_secondary_right_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary/LCrossSecondaryRightAction/Label");

		r_cross_secondary = GetNode<GridContainer>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary");
		r_cross_secondary_up_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryUpAction");
		r_cross_secondary_right_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryRightAction");
		r_cross_secondary_left_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryLeftAction");
		r_cross_secondary_down_action_button = GetNode<Button>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryDownAction");
		r_cross_secondary_up_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryUpAction/Label");
		r_cross_secondary_down_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryDownAction/Label");
		r_cross_secondary_left_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryLeftAction/Label");
		r_cross_secondary_right_action_label = GetNode<Label>("BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary/RCrossSecondaryRightAction/Label");

		consumable_1 = GetNode<Button>("BottomHUD/BottomHUDVBox/HBoxContainer/Consumables/Consumable1");
		consumable_2 = GetNode<Button>("BottomHUD/BottomHUDVBox/HBoxContainer/Consumables/Consumable2");
		consumable_3 = GetNode<Button>("BottomHUD/BottomHUDVBox/HBoxContainer/Consumables/Consumable3");
		consumable_4 = GetNode<Button>("BottomHUD/BottomHUDVBox/HBoxContainer/Consumables/Consumable4");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.AbilityAssigned += HandleAbilityAssigned;
		// _customSignals.AbilityRemoved += HandleAbilityRemoved;
		_customSignals.LCrossPrimaryOrSecondary += HandleLCrossPrimaryOrSecondary;
		_customSignals.RCrossPrimaryOrSecondary += HandleRCrossPrimaryOrSecondary;
		_customSignals.WhichConsumable += HandleWhichConsumable;
		_customSignals.EquipConsumable += HandleEquipConsumable;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	public void AbilityAssigned(AbilityResource ability_resource, string button_name)
    {
		GD.Print("got assignment in HUD");
		if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_action_button.Icon = ability_resource.icon;}

		if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_action_button.Icon = ability_resource.icon;}

		if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_action_button.Icon = ability_resource.icon;}
		if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_action_button.Icon = ability_resource.icon;}

		if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_action_button.Icon = ability_resource.icon;}
		if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_action_button.Icon = ability_resource.icon;}
        
    }
	public void AbilityRemoved(string button_name)
	{
		GD.Print(button_name);
		
		if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_action_button.Icon  = null;}
		if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_action_button.Icon  = null;}
		if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_action_button.Icon  = null;}
		if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_action_button.Icon  = null;}
		
		if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_action_button.Icon  = null;}
		if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_action_button.Icon  = null;}
		if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_action_button.Icon  = null;}
		if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_action_button.Icon  = null;}

		if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_action_button.Icon  = null;}
		if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_action_button.Icon  = null;}
		if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_action_button.Icon  = null;}
		if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_action_button.Icon  = null;}

		if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_action_button.Icon  = null;}
		if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_action_button.Icon  = null;}
		if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_action_button.Icon  = null;}
		if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_action_button.Icon  = null;}
	}

	private void HandleRCrossPrimaryOrSecondary(bool r_cross_primary_selected_signal) // Hides/ shows the selected crosses
    {
	
        if(r_cross_primary_selected_signal)
			{
				r_cross_primary_selected = true;
				r_cross_secondary_selected = false;
				r_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				r_cross_primary.Modulate = new Color(Colors.White, 1f);
				r_cross_primary_up_action_label.Show();
				r_cross_primary_down_action_label.Show();
				r_cross_primary_left_action_label.Show();
				r_cross_primary_right_action_label.Show();
				
				r_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				r_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_secondary_up_action_label.Hide();
				r_cross_secondary_down_action_label.Hide();
				r_cross_secondary_left_action_label.Hide();
				r_cross_secondary_right_action_label.Hide();

				// GD.Print("primary r cross selected");
				
			}
			else
			{
				r_cross_primary_selected = false;
				r_cross_secondary_selected = true;
				r_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				r_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_primary_up_action_label.Hide();
				r_cross_primary_down_action_label.Hide();
				r_cross_primary_left_action_label.Hide();
				r_cross_primary_right_action_label.Hide();

				r_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				r_cross_secondary.Modulate = new Color(Colors.White, 1f);
				r_cross_secondary_up_action_label.Show();
				r_cross_secondary_down_action_label.Show();
				r_cross_secondary_left_action_label.Show();
				r_cross_secondary_right_action_label.Show();
				// GD.Print("secondary r cross selected");
			}
    }

	private void HandleLCrossPrimaryOrSecondary(bool l_cross_primary_selected_signal)
    {
		
        if(l_cross_primary_selected_signal)
			{
				// GD.Print("primary l cross selected");
				
				l_cross_primary_selected = true;
				l_cross_secondary_selected = false;
				l_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				l_cross_primary.Modulate = new Color(Colors.White, 1f);
				l_cross_primary_up_action_label.Show();
				l_cross_primary_down_action_label.Show();
				l_cross_primary_left_action_label.Show();
				l_cross_primary_right_action_label.Show();

				l_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;
				l_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_secondary_up_action_label.Hide();
				l_cross_secondary_down_action_label.Hide();
				l_cross_secondary_left_action_label.Hide();
				l_cross_secondary_right_action_label.Hide();
			}
			else
			{
				// GD.Print("secondary  cross selected");
				l_cross_primary_selected = false;
				l_cross_secondary_selected = true;
				l_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;
				l_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_primary_up_action_label.Hide();
				l_cross_primary_down_action_label.Hide();
				l_cross_primary_left_action_label.Hide();
				l_cross_primary_right_action_label.Hide();

				l_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				l_cross_secondary.Modulate = new Color(Colors.White, 1f);
				l_cross_secondary_up_action_label.Show();
				l_cross_secondary_down_action_label.Show();
				l_cross_secondary_left_action_label.Show();
				l_cross_secondary_right_action_label.Show();

				
			}
    }

	private void HandleWhichConsumable(int consumable) // Hides/ shows the current selected consumable
    {
        if(consumable == 1){consumable_1.Show(); consumable_2.Hide(); consumable_3.Hide(); consumable_4.Hide();}
		if(consumable == 2){consumable_1.Hide(); consumable_2.Show(); consumable_3.Hide(); consumable_4.Hide();}
		if(consumable == 3){consumable_1.Hide(); consumable_2.Hide(); consumable_3.Show(); consumable_4.Hide();}
		if(consumable == 4){consumable_1.Hide(); consumable_2.Hide(); consumable_3.Hide(); consumable_4.Show();}
    }
	private void HandleEquipConsumable(ConsumableResource item, int consumable_slot)
    {
        if(consumable_slot == 1){consumable_1.Icon = item.icon;}
		if(consumable_slot == 2){consumable_2.Icon = item.icon;}
		if(consumable_slot == 3){consumable_3.Icon = item.icon;}
		if(consumable_slot == 4){consumable_4.Icon = item.icon;}
    }
	public void EquipConsumableHUD(ConsumableResource item, int consumable_slot)
    {
        if(consumable_slot == 1){consumable_1.Icon = item.icon;}
		if(consumable_slot == 2){consumable_2.Icon = item.icon;}
		if(consumable_slot == 3){consumable_3.Icon = item.icon;}
		if(consumable_slot == 4){consumable_4.Icon = item.icon;}
    }

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(inventory_open && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(UI_element_open && eventJoypadButton.ButtonIndex == JoyButton.A)
			{

				AcceptEvent();
			}
		}
		
	}

	// private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    // {
	// 	GD.Print("got assignment in HUD");
	// 	if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_action_button.Icon = icon;}
	// 	if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_action_button.Icon = icon;}
	// 	if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_action_button.Icon = icon;}
	// 	if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_action_button.Icon = icon;}

	// 	if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_action_button.Icon = icon;}
	// 	if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_action_button.Icon = icon;}
	// 	if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_action_button.Icon = icon;}
	// 	if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_action_button.Icon = icon;}

	// 	if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_action_button.Icon = icon;}
	// 	if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_action_button.Icon = icon;}
	// 	if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_action_button.Icon = icon;}
	// 	if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_action_button.Icon = icon;}

	// 	if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_action_button.Icon = icon;}
	// 	if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_action_button.Icon = icon;}
	// 	if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_action_button.Icon = icon;}
	// 	if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_action_button.Icon = icon;}
        
    // }
	// private void HandleAbilityRemoved(string ability, string button_name)
	// {
	// 	GD.Print(button_name);
		
	// 	if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_action_button.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_action_button.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_action_button.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_action_button.Icon  = null;}
		
	// 	if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_action_button.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_action_button.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_action_button.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_action_button.Icon  = null;}

	// 	if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_action_button.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_action_button.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_action_button.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_action_button.Icon  = null;}

	// 	if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_action_button.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_action_button.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_action_button.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_action_button.Icon  = null;}
	// }

}
