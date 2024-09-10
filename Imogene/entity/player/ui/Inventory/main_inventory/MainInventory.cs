using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

[GlobalClass]
public partial class MainInventory : Control
{
	[Export] public GridContainer items;
	[Export] public GenericInventoryButton mats;
	[Export] public BottomButtons bottom_buttons;
	[Export] public int item_slots_count { get; set; } = 72;

	[Signal] public delegate void DroppingItemEventHandler();

	private Vector3 drop_position;
	public List<ItemSlot> inventory_slots = new List<ItemSlot>();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (ItemSlot slot in items.GetChildren())
		{
			slot.inventory_slot_id -= 1;
			inventory_slots.Add(slot);
			slot.ItemDropped += HandleItemDropped;
		}
		GD.Print("inventory slots: " + inventory_slots.Count);
	}

    private void HandleItemDropped(int from_slot_id, int to_slot_id)
    {
		GD.Print("from slot id " + from_slot_id + " to slot id " + to_slot_id);
        var to_slot_item = inventory_slots[to_slot_id].slot_data;
		var from_slot_item = inventory_slots[from_slot_id].slot_data;

		inventory_slots[to_slot_id].FillSlot(from_slot_item);
		inventory_slots[from_slot_id].FillSlot(to_slot_item);
	}

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		GD.Print("Checking can");
        return data.VariantType == Variant.Type.Dictionary && (string)data.AsGodotDictionary()["Type"] == "Item";
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
		GD.Print("Drop item");
		EmitSignal(nameof(DroppingItem));
        int id = (int)data.AsGodotDictionary()["ID"];
		var new_item = inventory_slots[id].slot_data.item_model_prefab.Instantiate() as Node3D;

		inventory_slots[id].FillSlot(null);
		GetTree().CurrentScene.AddChild(new_item);
		new_item.GlobalPosition = drop_position;
    }

	public void GetDropPosition(Player player)
	{
		drop_position = player.GlobalPosition;
		drop_position.Y = drop_position.Y * 1.25f;
		drop_position = drop_position + new Vector3(2f, 2f, 2f);
	}


    public void PickUpItem(ItemData item)
	{
		foreach(ItemSlot slot in inventory_slots)
		{
			if(!slot.slot_filled)
			{
				slot.FillSlot(item);
				break;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_mats_button_down()
	{
		GD.Print("Mats button down");
		mats.Show();
	}

    internal void OnItemPickedUp(ItemData item)
    {
        PickUpItem(item);
    }
}
