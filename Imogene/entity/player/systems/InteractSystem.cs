using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[GlobalClass]
public partial class InteractSystem : Node3D
{
	[Export] public Array<ItemData> item_types {get; set;} = new Array<ItemData>();
	public bool in_interact_area { get; set; } = false;
	private bool can_interact { get; set; } = true;
	private bool inventory_full  { get; set; } = false;
	Area3D area_interacting  { get; set; } = null;
	InteractableItem pickup_item { get; set; } = null;
	InteractableItem next_nearest { get; set; } = null;
	public List<InteractableItem> near_by_items { get; set; } = new List<InteractableItem>();
	public List<InteractableItem> items_in_pickup { get; set; } = new List<InteractableItem>();

	[Signal] public delegate void NearInteractableEventHandler(bool near_interactable_);
	[Signal] public delegate void ItemPickedUpEventHandler(ItemData item_);
	[Signal] public delegate void InputPickupEventHandler(InteractableItem item_);
	[Signal] public delegate void SwitchToNextNearestItemEventHandler(InteractableItem item_, bool last_item_);
	

    public void PickupItem(Node3D item_, Player player_)
	{

		InteractableItem _nearest_item = near_by_items.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();
		if(_nearest_item != null)
		{
			if(player_.PlayerUI.inventory.main.slots_filled < player_.PlayerUI.inventory.main.item_slots_count)
			{
				
				item_.QueueFree();
				items_in_pickup.Remove((InteractableItem)item_);
				near_by_items.Remove((InteractableItem)item_);
				if(items_in_pickup.Count > 0)
				{
					next_nearest = items_in_pickup.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();

					pickup_item = next_nearest;
					EmitSignal(nameof(SwitchToNextNearestItem), pickup_item, false);
				}
				else
				{
					next_nearest = null;
					EmitSignal(nameof(SwitchToNextNearestItem), pickup_item, true);
				}
				ItemData _template = item_types.FirstOrDefault(X => X.item_model_prefab.ResourcePath == _nearest_item.SceneFilePath);
				if(_template != null)
				{
					EmitSignal(nameof(ItemPickedUp), _template);
				}
			}
		}
	}


	private void OnInteractAreaEntered(Area3D area_, Player player_)
	{
		
		if(area_ is InteractableObject)
		{
			in_interact_area = true;
			area_interacting = area_;
			EmitSignal(nameof(NearInteractable),true);
		}
	}

	private void OnInteractAreaExited(Area3D area_, Player player_)
	{

		if(area_ is InteractableObject)
		{
			in_interact_area = false;
			area_interacting = null;
			EmitSignal(nameof(NearInteractable),false);
		}
		
	}

	public void OnObjectEnteredArea(Node3D body_, Player player_)
	{
		
		if(body_ is InteractableItem _item)
		{
			_item.GainFocus(inventory_full);
			near_by_items.Add(_item);
		}
	}

	public void OnObjectExitedArea(Node3D body_, Player player_)
	{
		
		if(body_ is InteractableItem _item && near_by_items.Contains(_item))
		{
			_item.LoseFocus();
			near_by_items.Remove(_item);
		}
	}


    private void OnObjectEnteredPickup(Node3D body_, Player player_)
    {
		if(body_ is InteractableItem _item)
		{
			items_in_pickup.Add(_item);
			if(!_item.interact_to_pick_up)
			{
				PickupItem(_item, player_);
			}
			else
			{
				pickup_item = _item;
			}
		}
    }

	private void OnObjectExitedPickup(Node3D body_, Player player_)
    {
		if(body_ is InteractableItem item)
		{
			items_in_pickup.Remove(item);
			if(item.interact_to_pick_up)
			{
				pickup_item = next_nearest;
			}
		}

    }

    internal void HandleUICapturingInput(bool capturing_input_)
    {
        can_interact = !capturing_input_;
    }

    private void OnAcceptHUDInput(Node3D interacting_object_)
    {
        if(interacting_object_ is InteractableItem && can_interact && items_in_pickup.Count > 0)
		{
			EmitSignal(nameof(InputPickup), pickup_item);
		}
    }


    private void HandleInventoryCapacity(bool full_)
    {
		
        inventory_full = full_;
		foreach(InteractableItem _item in near_by_items)
		{
			_item.GainFocus(full_);
		}
	
    }

	public void Subscribe(Player player_)
	{
		player_.PlayerAreas.interact.AreaEntered += (area_) => OnInteractAreaEntered(area_, player_);
		player_.PlayerAreas.interact.AreaExited += (area_) => OnInteractAreaExited(area_, player_);
		player_.PlayerAreas.interact.BodyEntered += (body_) => OnObjectEnteredArea(body_, player_);
		player_.PlayerAreas.interact.BodyExited += (body_) => OnObjectExitedArea(body_, player_);
		player_.PlayerAreas.pickup_items.BodyEntered += (body_) => OnObjectEnteredPickup(body_, player_);
		player_.PlayerAreas.pickup_items.BodyExited += (body_) => OnObjectExitedPickup(body_, player_);
		player_.PlayerUI.CapturingInput += HandleUICapturingInput;
		player_.PlayerUI.hud.AcceptHUDInput += OnAcceptHUDInput;
		player_.PlayerUI.inventory.main.InventoryCapacity += HandleInventoryCapacity;
	
	}

	public void Unsubscribe(Player player_)
	{
		player_.PlayerAreas.interact.AreaEntered -= (area_) => OnInteractAreaEntered(area_, player_);
		player_.PlayerAreas.interact.AreaExited -= (area_) => OnInteractAreaExited(area_, player_);
		player_.PlayerAreas.interact.BodyEntered -= (body_) => OnObjectEnteredArea(body_, player_);
		player_.PlayerAreas.interact.BodyExited -= (body_) => OnObjectExitedArea(body_, player_);
		player_.PlayerAreas.pickup_items.BodyEntered -= (body_) => OnObjectEnteredPickup(body_, player_);
		player_.PlayerAreas.pickup_items.BodyExited -= (body_) => OnObjectExitedPickup(body_, player_);
		player_.PlayerUI.inventory.main.InventoryCapacity -= HandleInventoryCapacity;
		player_.PlayerUI.hud.AcceptHUDInput -= OnAcceptHUDInput;
	}
}
