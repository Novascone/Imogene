using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[GlobalClass]
public partial class InteractSystem : Node3D
{
	[Export] public Array<ItemData> ItemTypes {get; set;} = new Array<ItemData>();
	public bool InInteractArea { get; set; } = false;
	private bool CanInteract { get; set; } = true;
	private bool InventoryFull  { get; set; } = false;
	Area3D AreaInteracting  { get; set; } = null;
	InteractableItem PickUpItem { get; set; } = null;
	InteractableItem NextNearest { get; set; } = null;
	public List<InteractableItem> NearByItems { get; set; } = new List<InteractableItem>();
	public List<InteractableItem> ItemsInPickup { get; set; } = new List<InteractableItem>();

	[Signal] public delegate void NearInteractableEventHandler(bool nearInteractable);
	[Signal] public delegate void ItemPickedUpEventHandler(ItemData item);
	[Signal] public delegate void InputPickupEventHandler(InteractableItem item);
	[Signal] public delegate void SwitchToNextNearestItemEventHandler(InteractableItem item, bool lastItem);
	

    public void PickupItem(Node3D item, Player player)
	{
		GD.Print("Picking up item");
		InteractableItem nearestItem = NearByItems.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();
		if(nearestItem != null)
		{
			if(player.PlayerUI.inventory.main.slots_filled < player.PlayerUI.inventory.main.item_slots_count)
			{
				
				item.QueueFree();
				ItemsInPickup.Remove((InteractableItem)item);
				NearByItems.Remove((InteractableItem)item);
				if(ItemsInPickup.Count > 0)
				{
					NextNearest = ItemsInPickup.OrderBy(X => X.GlobalPosition.DistanceTo(GlobalPosition)).FirstOrDefault();

					PickUpItem = NextNearest;
					EmitSignal(nameof(SwitchToNextNearestItem), PickUpItem, false);
				}
				else
				{
					NextNearest = null;
					EmitSignal(nameof(SwitchToNextNearestItem), PickUpItem, true);
				}
				ItemData template = ItemTypes.FirstOrDefault(X => X.item_model_prefab.ResourcePath == nearestItem.SceneFilePath);
				if(template != null)
				{
					EmitSignal(nameof(ItemPickedUp), template);
					GD.Print("Emit picking up item signal");
				}
			}
		}
	}


	private void OnInteractAreaEntered(Area3D area, Player player)
	{
		
		if(area is InteractableObject)
		{
			InInteractArea = true;
			AreaInteracting = area;
			EmitSignal(nameof(NearInteractable),true);
		}
	}

	private void OnInteractAreaExited(Area3D area, Player player)
	{

		if(area is InteractableObject)
		{
			InInteractArea = false;
			AreaInteracting = null;
			EmitSignal(nameof(NearInteractable), false);
		}
		
	}

	public void OnObjectEnteredArea(Node3D body, Player player)
	{
		
		if(body is InteractableItem item)
		{
			item.GainFocus(InventoryFull);
			NearByItems.Add(item);
		}
	}

	public void OnObjectExitedArea(Node3D body, Player player)
	{
		
		if(body is InteractableItem item && NearByItems.Contains(item))
		{
			item.LoseFocus();
			NearByItems.Remove(item);
		}
	}


    private void OnObjectEnteredPickup(Node3D body, Player player)
    {
		if(body is InteractableItem item)
		{
			ItemsInPickup.Add(item);
			if(!item.interact_to_pick_up)
			{
				PickupItem(item, player);
			}
			else
			{
				PickUpItem = item;
			}
		}
    }

	private void OnObjectExitedPickup(Node3D body, Player player)
    {
		if(body is InteractableItem item)
		{
			ItemsInPickup.Remove(item);
			if(item.interact_to_pick_up)
			{
				PickUpItem = NextNearest;
			}
		}

    }

    internal void HandleUICapturingInput(bool capturingInput)
    {
        CanInteract = !capturingInput;
    }

    private void OnAcceptHUDInput(Node3D interactingObject)
    {
        if(interactingObject is InteractableItem && CanInteract && ItemsInPickup.Count > 0)
		{
			EmitSignal(nameof(InputPickup), PickUpItem);
		}
    }


    private void HandleInventoryCapacity(bool full)
    {
		
        InventoryFull = full;
		foreach(InteractableItem item in NearByItems)
		{
			item.GainFocus(full);
		}
	
    }

	public void Subscribe(Player player)
	{
		player.PlayerAreas.Interact.AreaEntered += (area) => OnInteractAreaEntered(area, player);
		player.PlayerAreas.Interact.AreaExited += (area) => OnInteractAreaExited(area, player);
		player.PlayerAreas.Interact.BodyEntered += (body) => OnObjectEnteredArea(body, player);
		player.PlayerAreas.Interact.BodyExited += (body) => OnObjectExitedArea(body, player);
		player.PlayerAreas.PickUpItems.BodyEntered += (body) => OnObjectEnteredPickup(body, player);
		player.PlayerAreas.PickUpItems.BodyExited += (body) => OnObjectExitedPickup(body, player);
		player.PlayerUI.CapturingInput += HandleUICapturingInput;
		player.PlayerUI.hud.AcceptHUDInput += OnAcceptHUDInput;
		player.PlayerUI.inventory.main.InventoryCapacity += HandleInventoryCapacity;
	
	}

	public void Unsubscribe(Player player)
	{
		player.PlayerAreas.Interact.AreaEntered -= (area) => OnInteractAreaEntered(area, player);
		player.PlayerAreas.Interact.AreaExited -= (area) => OnInteractAreaExited(area, player);
		player.PlayerAreas.Interact.BodyEntered -= (body) => OnObjectEnteredArea(body, player);
		player.PlayerAreas.Interact.BodyExited -= (body) => OnObjectExitedArea(body, player);
		player.PlayerAreas.PickUpItems.BodyEntered -= (body) => OnObjectEnteredPickup(body, player);
		player.PlayerAreas.PickUpItems.BodyExited -= (body) => OnObjectExitedPickup(body, player);
		player.PlayerUI.inventory.main.InventoryCapacity -= HandleInventoryCapacity;
		player.PlayerUI.hud.AcceptHUDInput -= OnAcceptHUDInput;
	}
}
