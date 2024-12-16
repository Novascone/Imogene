using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;

[GlobalClass]
public partial class MainInventory : Control
{
	[Export] public CharacterOutline character_outline { get; set; }
	[Export] public GridContainer items { get; set; }
	[Export] public GenericInventoryButton mats { get; set; }
	[Export] public BottomButtons bottom_buttons { get; set; }
	[Export] public int item_slots_count { get; set; } = 72;
	public int slots_filled { get; set; } = 0;
	public List<ItemSlot> inventory_slots { get; set; } = new();
	int equipped_slot { get; set; } = -1;

	private Vector3 _drop_position;

	[Signal] public delegate void DroppingItemEventHandler();
	[Signal] public delegate void InventoryCapacityEventHandler(bool full_);
	
    public void HandleItemDroppedInto(int from_slot_id_, int to_slot_id)
    {
		if(equipped_slot != -1)
		{
			if(equipped_slot == from_slot_id_)
			{
				equipped_slot = to_slot_id;
			}
			else if (equipped_slot == to_slot_id)
			{
				equipped_slot = from_slot_id_;
			}
		}
        var to_slot_item = inventory_slots[to_slot_id].slot_data;
		var from_slot_item = inventory_slots[from_slot_id_].slot_data;

		inventory_slots[to_slot_id].FillSlot(from_slot_item, equipped_slot == to_slot_id);
		inventory_slots[from_slot_id_].FillSlot(to_slot_item, equipped_slot == from_slot_id_);
	}

    public void HandleItemDroppedOutside(int id_, ItemData data_)
    {
		if(equipped_slot == id_)
		{
			equipped_slot = -1;
		}
		EmitSignal(nameof(DroppingItem));
		var new_item = inventory_slots[id_].slot_data.item_model_prefab.Instantiate() as InteractableItem;
		new_item.interact_to_pick_up = true;
		inventory_slots[id_].FillSlot(null, false);
		slots_filled -= 1;
		EmitSignal(nameof(InventoryCapacity), false);
		GetTree().CurrentScene.AddChild(new_item);
		new_item.GlobalPosition = _drop_position;
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
		_drop_position = player.GlobalPosition;
		_drop_position.Y *= 1.25f;

		_drop_position += new Vector3(x, y, z);
	}


    public void PickUpItem(ItemData item_)
	{
		bool _found_slot = false;
		foreach(ItemSlot _slot in inventory_slots)
		{
			if(!_slot.slot_filled)
			{
				_slot.FillSlot(item_, false);
				_found_slot = true;
				slots_filled += 1;
				if(slots_filled == item_slots_count)
				{
					EmitSignal(nameof(InventoryCapacity), true);
				}
				break;
			}
		}

		if(!_found_slot)
		{

		}
	}

	public void _on_mats_button_down()
	{
		mats.Show();
	}

    internal void OnItemPickedUp(ItemData item_)
    {
        PickUpItem(item_);
    }

    internal void OnItemEquipped(int id_)
    {
        if(equipped_slot != -1)
		{
			inventory_slots[equipped_slot].FillSlot(inventory_slots[equipped_slot].slot_data, false);
		}

		if(id_ != equipped_slot && inventory_slots[id_].slot_data != null)
		{
			inventory_slots[id_].FillSlot(inventory_slots[id_].slot_data, true);
			equipped_slot = id_;
		}
    }

   
}
