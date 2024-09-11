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
	private bool can_interact;
	// private InteractableItem item_in_pick_up;
	private bool inventory_full;
	Area3D area_interacting;

	[Signal] public delegate void NearInteractableEventHandler(bool near_interactable);
	[Signal] public delegate void ItemPickedUpEventHandler(ItemData item);
	[Signal] public delegate void InputPickUpEventHandler(InteractableItem item);
	[Signal] public delegate void SwitchToNextNearestItemEventHandler(InteractableItem item, bool last_item);
	[Export] public Array<ItemData> item_types {get; set;} = new Array<ItemData>();
	public List<InteractableItem> items_in_pick_up = new List<InteractableItem>();
	InteractableItem next_nearest;
	InteractableItem pick_up_item;
	public List<InteractableItem> near_by_items = new List<InteractableItem>();

    // public override void _Input(InputEvent @event)
    // {
    //     if(@event.IsActionPressed("Interact") && can_interact && items_in_pick_up.Count > 0)
	// 	{
	// 		EmitSignal(nameof(InputPickUp), pick_up_item);
			
	// 	}
    // }



    public void SubscribeToInteractSignals(Player player)
	{
		player.areas.interact.AreaEntered += (area) => OnInteractAreaEntered(area, player);
		player.areas.interact.AreaExited += (area) => OnInteractAreaExited(area, player);
		player.areas.interact.BodyEntered += (body) => OnObjectEnteredArea(body, player);
		player.areas.interact.BodyExited += (body) => OnObjectExitedArea(body, player);
		player.areas.pick_up_items.BodyEntered += (body) => OnObjectEnteredPickUp(body, player);
		player.areas.pick_up_items.BodyExited += (body) => OnObjectExitedPickUp(body, player);
		player.ui.inventory.main.InventoryCapacity += HandleInventoryCapacity;
		player.ui.hud.AcceptHUDInput += OnAcceptHUDInput;
	
		GD.Print("item types count " + item_types.Count);
		GD.Print("subscribed top interact signals");
	}

    private void OnAcceptHUDInput(Node3D interacting_object)
    {
        if(interacting_object is InteractableItem && can_interact && items_in_pick_up.Count > 0)
		{
			EmitSignal(nameof(InputPickUp), pick_up_item);
		}
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
		if(body is InteractableItem item)
		{
			items_in_pick_up.Add(item);
			if(!item.dropped_by_player)
			{
				PickUpItem(item, player);
			}
			else
			{
				pick_up_item = item;
			}
		}
    }

	private void OnObjectExitedPickUp(Node3D body, Player player)
    {
		if(body is InteractableItem item)
		{
			items_in_pick_up.Remove(item);
			if(item.dropped_by_player)
			{
				pick_up_item = next_nearest;
			}
		}

    }
    // public override void _UnhandledInput(InputEvent @event)
    // {
    // 	if(@event.IsActionPressed("Interact"))
    // 	{
    // 		PickUpNearestItem();
    // 	}
    // }

    public void PickUpItem(Node3D item, Player player)
	{
		GD.Print("pick up item");
		InteractableItem nearest_item = near_by_items.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();
		if(nearest_item != null)
		{
			if(player.ui.inventory.main.slots_filled < player.ui.inventory.main.item_slots_count)
			{
				
				item.QueueFree();
				items_in_pick_up.Remove((InteractableItem)item);
				near_by_items.Remove((InteractableItem)item);
				if(items_in_pick_up.Count > 0)
				{
					next_nearest = items_in_pick_up.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();

					pick_up_item = next_nearest;
					GD.Print("next nearest " + next_nearest.Name);
					EmitSignal(nameof(SwitchToNextNearestItem), pick_up_item, false);
				}
				else
				{
					next_nearest = null;
					EmitSignal(nameof(SwitchToNextNearestItem), pick_up_item, true);
				}
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

    internal void HandleCapturingInput(bool capturing_input)
    {
        can_interact = capturing_input;
    }
}
