using Godot;
using System;

public partial class InventoryButton : Button
{
	public Item inventory_item;
	public TextureRect icon;
	public Label quantity_label;
	private int index;
	public Control info;
	public Control consumable_slot_selection;
	public Button consumable_1;
	public Button consumable_2;
	public Button consumable_3;
	public Button consumable_4;
	public bool focused;
	private CustomSignals _customSignals; // Custom signal instance
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		icon = GetNode<TextureRect>("TextureRect");
		quantity_label = GetNode<Label>("Label");
		info = GetNode<Control>("Info");
		
		consumable_slot_selection = GetNode<Control>("ConsumableSlotSelection");
		consumable_1 = GetNode<Button>("ConsumableSlotSelection/PanelContainer/VBoxContainer/Consumable1/Consumable1");
		consumable_2 = GetNode<Button>("ConsumableSlotSelection/PanelContainer/VBoxContainer/Consumable2/Consumable2");
		consumable_3 = GetNode<Button>("ConsumableSlotSelection/PanelContainer/VBoxContainer/Consumable3/Consumable3");
		consumable_4 = GetNode<Button>("ConsumableSlotSelection/PanelContainer/VBoxContainer/Consumable4/Consumable4");
		consumable_slot_selection.Hide();
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		// if(focused && inventory_item != null && Input.IsActionJustPressed("Interact"))
		// {
		// 	InventoryButtonPressed();
		// }
		// GD.Print(info.Visible);
		if(consumable_slot_selection.Visible)
		{
			consumable_slot_selection.FocusMode = FocusModeEnum.All;

		}
		else if(!consumable_slot_selection.Visible)
		{
			consumable_slot_selection.FocusMode = FocusModeEnum.None;
		}
	}

	// public override void _GuiInput(InputEvent @event)
	// {
	// 	if(@event is InputEventJoypadButton eventJoypadButton)
	// 	{
	// 		if(eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
	// 		{
	// 			GD.Print("event accepted ");
	// 			AcceptEvent();
	// 		}
	// 	}
		
	// }

	public void UpdateItem(Item item, int index)
	{
		this.index = index;
		inventory_item = item;
		if(item == null)
		{
			icon.Texture = null;
			quantity_label.Text = string.Empty;
		}
		else
		{
			icon.Texture = item.icon;
			quantity_label.Text = item.quantity.ToString();
		}
		
	}


	 public void InventoryButtonPressed()
    {
		
		if(inventory_item != null && inventory_item.type != "consumable")
		{
			GD.Print("not a consumable");
			inventory_item.UseItem();
			// GD.Print("button pressed");
		}
		if(inventory_item != null && inventory_item.type == "consumable")
		{
			// GD.Print("consumable");
			// inventory_item.EquipItem();
			consumable_slot_selection.Show();
			consumable_1.Show();
			consumable_2.Show();
			consumable_3.Show();
			consumable_4.Show();
			
		}
			
    }

	public void _on_focus_entered()
	{
		info.Show();
		focused = true;
	}

	public void _on_focus_exited()
	{
		info.Hide();
		focused = false;
	}

	public void _on_consumable_1_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 1);
		consumable_slot_selection.Hide();
		consumable_1.Hide();
		consumable_2.Hide();
		consumable_3.Hide();
		consumable_4.Hide();
	}
	public void _on_consumable_2_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 2);
		consumable_slot_selection.Hide();
		consumable_1.Hide();
		consumable_2.Hide();
		consumable_3.Hide();
		consumable_4.Hide();
	}
	public void _on_consumable_3_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 3);
		consumable_slot_selection.Hide();
		consumable_1.Hide();
		consumable_2.Hide();
		consumable_3.Hide();
		consumable_4.Hide();
	}
	public void _on_consumable_4_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 4);
		consumable_slot_selection.Hide();
		consumable_1.Hide();
		consumable_2.Hide();
		consumable_3.Hide();
		consumable_4.Hide();
	}
	public void _on_cancel_button_down()
	{
		consumable_slot_selection.Hide();
		consumable_1.Hide();
		consumable_2.Hide();
		consumable_3.Hide();
		consumable_4.Hide();
	}

}
