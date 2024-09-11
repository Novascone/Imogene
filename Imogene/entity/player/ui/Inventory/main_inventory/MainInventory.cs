using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;

[GlobalClass]
public partial class MainInventory : Control
{
	[Export] public GridContainer items;
	[Export] public GenericInventoryButton mats;
	[Export] public BottomButtons bottom_buttons;
	[Export] public int item_slots_count { get; set; } = 72;
	public int slots_filled { get; set; } = 0;

	[Signal] public delegate void DroppingItemEventHandler();
	[Signal] public delegate void InventoryCapacityEventHandler(bool full);
	

	private Vector3 drop_position;
	public List<ItemSlot> inventory_slots = new List<ItemSlot>();
	int equipped_slot = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("inventory slots: " + inventory_slots.Count);
	}

    public void HandleItemDroppedInto(int from_slot_id, int to_slot_id)
    {
		if(equipped_slot != -1)
		{
			if(equipped_slot == from_slot_id)
			{
				equipped_slot = to_slot_id;
			}
			else if (equipped_slot == to_slot_id)
			{
				equipped_slot = from_slot_id;
			}
		}
        var to_slot_item = inventory_slots[to_slot_id].slot_data;
		var from_slot_item = inventory_slots[from_slot_id].slot_data;

		inventory_slots[to_slot_id].FillSlot(from_slot_item, equipped_slot == to_slot_id);
		inventory_slots[from_slot_id].FillSlot(to_slot_item, equipped_slot == from_slot_id);
	}


    // public override bool _CanDropData(Vector2 atPosition, Variant data)
    // {
	// 	GD.Print("Checking can");
    //     return data.VariantType == Variant.Type.Dictionary && (string)data.AsGodotDictionary()["Type"] == "Item";
    // }

    public void HandleItemDroppedOutside(int id, ItemData data)
    {
		GD.Print("Drop item");
		if(equipped_slot == id)
		{
			equipped_slot = -1;
		}
		EmitSignal(nameof(DroppingItem));
		var new_item = inventory_slots[id].slot_data.item_model_prefab.Instantiate() as InteractableItem;
		new_item.dropped_by_player = true;
		inventory_slots[id].FillSlot(null, false);
		slots_filled -= 1;
		EmitSignal(nameof(InventoryCapacity), false);
		GetTree().CurrentScene.AddChild(new_item);
		new_item.GlobalPosition = drop_position;
    }

	public void GetDropPosition(Player player)
	{
		var random = new RandomNumberGenerator();
		random.Randomize();
		var x = random.RandiRange(-2, 2);
		var y = random.RandiRange(0, 2) ;
		var z = random.RandiRange(-2, 2);
		if(x == 0)
		{
			x += 1;
		}
		if(y == 0)
		{
			y += 1;
		}
		drop_position = player.GlobalPosition;
		drop_position.Y = drop_position.Y * 1.25f;

		drop_position = drop_position + new Vector3(x, y, z);
	}


    public void PickUpItem(ItemData item)
	{
		bool found_slot = false;
		foreach(ItemSlot slot in inventory_slots)
		{
			if(!slot.slot_filled)
			{
				slot.FillSlot(item, false);
				found_slot = true;
				slots_filled += 1;
				if(slots_filled == item_slots_count)
				{
					EmitSignal(nameof(InventoryCapacity), true);
				}
				break;
			}
		}

		if(!found_slot)
		{
			GD.Print("can't pick up item");
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

    internal void OnItemEquipped(int id)
    {
		GD.Print("Received item equipped");
        if(equipped_slot != -1)
		{
			inventory_slots[equipped_slot].FillSlot(inventory_slots[equipped_slot].slot_data, false);
		}

		if(id != equipped_slot && inventory_slots[id].slot_data != null)
		{
			inventory_slots[id].FillSlot(inventory_slots[id].slot_data, true);
			equipped_slot = id;
		}
    }
}
