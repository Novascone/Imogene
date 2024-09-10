using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[GlobalClass]
public partial class InteractSystem : Node3D
{
	public bool in_interact_area;
	public bool left_interact;
	public bool entered_interact;
	private bool inventory_full;
	Area3D area_interacting;

	[Signal] public delegate void NearInteractableEventHandler(bool near_interactable);
	[Signal] public delegate void ItemPickedUpEventHandler(ItemData item);
	[Export] public Array<ItemData> item_types {get; set;} = new Array<ItemData>();
	private List<InteractableItem> near_by_items = new List<InteractableItem>();
	
	

	public void SubscribeToInteractSignals(Player player)
	{
		player.areas.interact.AreaEntered += (area) => OnInteractAreaEntered(area, player);
		player.areas.interact.AreaExited += (area) => OnInteractAreaExited(area, player);
		player.areas.interact.BodyEntered += (body) => OnObjectEnteredArea(body, player);
		player.areas.interact.BodyExited += (body) => OnObjectExitedArea(body, player);
		player.areas.pick_up_items.BodyEntered += (body) => OnObjectEnteredPickUp(body, player);
		player.ui.inventory.main.InventoryCapacity += HandleInventoryCapacity;
	
		GD.Print("item types count " + item_types.Count);
		GD.Print("subscribed top interact signals");
	}

    private void HandleInventoryCapacity(bool full)
    {
		GD.Print("receiving inventory full signal " + full);
        inventory_full = full;
		foreach(InteractableItem item in near_by_items)
		{
			item.GainFocus(full);
		}
	
    }

    private void OnObjectEnteredPickUp(Node3D body, Player player)
    {
       PickUpItem(body, player);
    }

    // public override void _UnhandledInput(InputEvent @event)
    // {
    // 	if(@event.IsActionPressed("Interact"))
    // 	{
    // 		PickUpNearestItem();
    // 	}
    // }

    private void PickUpItem(Node3D item, Player player)
	{
		InteractableItem nearest_item = near_by_items.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();

		if(nearest_item != null)
		{
			if(player.ui.inventory.main.slots_filled < player.ui.inventory.main.item_slots_count)
			{
				item.QueueFree();
				near_by_items.Remove((InteractableItem)item);
				GD.Print("picking up nearest item");
				ItemData template = item_types.FirstOrDefault(X => X.item_model_prefab.ResourcePath == nearest_item.SceneFilePath);
				if(template != null)
				{
					GD.Print("Item id: " + item_types.IndexOf(template) +" Item name: " + template.item_name);
					EmitSignal(nameof(ItemPickedUp), template);
				}
				else
				{
					GD.PrintErr("Item not found");
				}
			}
			else
			{
				GD.Print("Inventory full");
			}
			// nearest_item.QueueFree();
			
			
			
			// if(near_by_items.Count == 0)
			// {
			// 	EmitSignal(nameof(NearInteractable),false);
			// }
		}
		
	}

	private void OnInteractAreaExited(Area3D area, Player player)
	{

		GD.Print("Interact area exited");
		if(area is InteractableObject)
		{
			left_interact = true;
			in_interact_area = false;
			area_interacting = null;
			EmitSignal(nameof(NearInteractable),false);
		}
		
	}

	private void OnInteractAreaEntered(Area3D area, Player player)
	{
		GD.Print("Interact area entered");
		if(area is InteractableObject)
		{
			entered_interact = true;
			in_interact_area = true;
			area_interacting = area;
			EmitSignal(nameof(NearInteractable),true);
		}
	}

	public void OnObjectExitedArea(Node3D body, Player player)
	{
		GD.Print("Item exited interact area");
		if(body is InteractableItem item && near_by_items.Contains(item))
		{
			// EmitSignal(nameof(NearInteractable),false);
			item.LoseFocus();

			near_by_items.Remove(item);
		}
	}

	public void OnObjectEnteredArea(Node3D body, Player player)
	{
		GD.Print("Item entered interact area");
		if(body is InteractableItem item)
		{
			// EmitSignal(nameof(NearInteractable),true);
			item.GainFocus(inventory_full);
			near_by_items.Add(item);
		}
	}

	
}
