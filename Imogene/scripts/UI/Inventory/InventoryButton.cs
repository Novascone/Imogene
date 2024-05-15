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
	public bool focused;
	private CustomSignals _customSignals; // Custom signal instance
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon = GetNode<TextureRect>("TextureRect");
		quantity_label = GetNode<Label>("Label");
		info = GetNode<Control>("Info");
		consumable_slot_selection = GetNode<Control>("ConsumableSlotSelection");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		// if(focused && inventory_item != null && Input.IsActionJustPressed("Interact"))
		// {
		// 	InventoryButtonPressed();
		// }
	}

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
	}
	public void _on_consumable_2_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 2);
		consumable_slot_selection.Hide();
	}
	public void _on_consumable_3_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 3);
		consumable_slot_selection.Hide();
	}
	public void _on_consumable_4_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.EquipConsumable),inventory_item, 4);
		consumable_slot_selection.Hide();
	}
	public void _on_cancel_button_down()
	{
		consumable_slot_selection.Hide();
	}

}
