using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : UI
{

	public UI ui;
	public Player inventory_player;
	public InventoryInfo character_inventory;
	public PackedScene  inventory_button;
	[Export] private string item_button_path = "res://scenes/UI/inventory_button.tscn";
	public GridContainer item_grid_container;
	public InventoryButton grabbed_object { get; set; }
	public InventoryButton hover_over_button { get; set; }
	public Button hover_over_button_non_inventory { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ui = (UI)GetParent();
		character_inventory = GetNode<InventoryInfo>("CharacterInventoryContainer");
		inventory_button = ResourceLoader.Load<PackedScene>(item_button_path);
		item_grid_container = GetNode<GridContainer>("CharacterInventoryContainer/FullInventory/CharacterInventory/Items/ItemsGrid");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		PopulateButtons();

		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(Visible && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ui.this_player != null)
		{
			if(!buttons_informed)
			{
				ConveyInfoToButtons();
			}
			
		}

		if(Input.IsActionJustPressed("B"))
		{
			
			ui.GrabFocus();
			GD.Print(HasFocus());
			if(ui.inventory_open || ui.abilities_open)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), false);
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
				// this_player.can_move = true;
				ui.inventory_open = false;
				Hide();
				ui.mats.Hide();
				ui.character_Sheet_depth.Hide();
				ui.cursor.Hide();
			}
			if(abilities_open && !abilities_secondary_ui_open)
			{
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
				// this_player.can_move = true;
				abilities_open = false;
				// abilities.Hide();
				cursor.Hide();
			}

		}

		if(Input.IsActionJustPressed("Inventory"))
		{
			
			ui.GrabFocus();
			if(!ui.inventory_open)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), true);
				ui.inventory_open = true;
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),true);
				// this_player.can_move = false;
				Show();
				ui.cursor.Show();
				ui.abilities_open = false;
				ui.abilities.Hide();
			}
			else
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), false);
				ui.inventory_open = false;
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
				// this_player.can_move = true;
				Hide();
				ui.mats.Hide();
				ui.character_Sheet_depth.Hide();
				if(!abilities_open)
				{
					ui.cursor.Hide();
				}
				
			}
		}
		if(character_inventory.Visible)
		{
			if(hover_over_button != null && !hover_over_button.IsInGroup("cursorbutton"))
			{
				if(GetViewport().GuiGetFocusOwner() is InventoryButton)
				{
					hover_over_button = (InventoryButton)GetViewport().GuiGetFocusOwner(); // Set current button hovered over
				}
				if(!clicked_on)
				{
					if(Input.IsActionJustPressed("Interact") && hover_over_button is InventoryButton) // If the player presses the interact button and that button is an inventory button send a signal to the player
					{
						if(hover_over_button.inventory_item != null) 
						{
							GD.Print("pressed");
							GD.Print("Hover over button inventory", hover_over_button.inventory_item);
							hover_over_button.InventoryButtonPressed();
							// hover_over_button = null;
						}
						if(hover_over_button.inventory_item.type == "generic")
						{
							_customSignals.EmitSignal(nameof(CustomSignals.ItemInfo), hover_over_button.inventory_item);
						}
						if(hover_over_button.inventory_item.type == "consumable")
						{
							_customSignals.EmitSignal(nameof(CustomSignals.ConsumableInfo), (ConsumableResource)hover_over_button.inventory_item);
							
						}
						if(hover_over_button.inventory_item.type == "equipable")
						{
							// GD.Print("Arm item that UI is sending " + hover_over_button.arm);
							// GD.Print("inventory item: " + hover_over_button.inventory_item);
							// _customSignals.EmitSignal(nameof(CustomSignals.EquipableInfo), hover_over_button.inventory_item);
							ui.this_player.equipmentController.GetEquipableInfo((ArmsResource)hover_over_button.inventory_item);
							
						}
					}
					if(Input.IsActionJustPressed("InteractMenu") && !hover_over_button.is_empty ) // Handle grab object set icon of clicked object to a button attached to the cursor
					{
						
						clicked_on = true;
						grabbed_object = hover_over_button;
						GD.Print("Object grabbed");
						GD.Print(grabbed_object.is_empty);
						last_cursor_clicked_pos = GetTree().Root.GetMousePosition();
						if(hover_over_button is InventoryButton)
						{
							InventoryButton button = ui.cursor_button;
							button.Visible = true;
							button.UpdateItem(grabbed_object.inventory_item, 0);
						}
						
						
					}
				}
				else
				{
					if(Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse") || Input.IsActionJustPressed("ui_accept") && hover_over_button is InventoryButton)
					{
						if(over_trash) // Delete item if over trash
						{
							GD.Print("delete");
							clicked_on = false;
							DeleteItem(grabbed_object);
							InventoryButton button = ui.cursor_button;
							button.Visible = false;
						}
						else
						{
							clicked_on = false;
							InventoryButton button = ui.cursor_button;
							button.Visible = false;
							if(hover_over_button is InventoryButton)
							{
								if(grabbed_object != null && hover_over_button != null && !over_slot)
								{
									SwapButtons(grabbed_object, hover_over_button);
									GD.Print("buttons swapped");
								}
							}
							
						}
					
					}
					
				}
			}
			if((Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse")) && over_trash)
			{
				clicked_on = false;
				grabbed_object.is_empty = true;
				DeleteItem(grabbed_object);
			}
			if(Input.IsActionJustPressed("InteractMenu")   && over_slot)
			{
				
				if(grabbed_object.inventory_item.type == "equipable")
				{
					clicked_on = false;
					InventoryButton button = GetNode<Area2D>("Cursor/CursorSprite/CursorArea2D").GetNode<InventoryButton>("CursorButton");
					button.Visible = false;
					// _customSignals.EmitSignal(nameof(CustomSignals.EquipableInfo), (EquipableResource)grabbed_object.inventory_item);
					this_player.equipmentController.GetEquipableInfo((ArmsResource)grabbed_object.inventory_item);
				}
				
			}
		}
	}

	public void GetPlayerForInventory(Player s)
	{
		inventory_player = s;
	}

	private void SwapButtons(InventoryButton button1, InventoryButton button2)
	{
		int button1_index = button1.GetIndex();
		int button2_index = button2.GetIndex();
		item_grid_container.MoveChild(button1, button2_index);
		item_grid_container.MoveChild(hover_over_button, button1_index);
	}

	public void DeleteItem(InventoryButton inventory_button)
	{
		items.Remove(inventory_button.inventory_item);
		ReflowButtons();
		InventoryButton button = GetNode<Area2D>("Cursor/CursorArea2D").GetNode<InventoryButton>("CursorButton");
		button.Visible = false;
	}

	public void PopulateButtons()
	{
		for (int i = 0; i < inventory_size; i++)
		{
			InventoryButton current_inventory_button = inventory_button.Instantiate<InventoryButton>();
			
			item_grid_container.AddChild(current_inventory_button);
		}
	}
	public void ConveyInfoToButtons()
	{
		foreach(InventoryButton inventoryButton in item_grid_container.GetChildren().Cast<InventoryButton>())
		{
			inventoryButton.this_player = ui.this_player;
			inventoryButton.hud = ui.hud;
			
		}
		buttons_informed = true;
		// GD.Print("buttons informed");
	}

	public void AddItem(ItemResource item)
	{
		ItemResource current_item = item.Copy();
		GD.Print("Add item");
		GD.Print("Item name " + item.name);
		for (int i = 0; i < items.Count; i++)
		{
			if(items[i].id == current_item.id && items[i].quantity != items[i].stack_size)
			{
				if(items[i].quantity + current_item.quantity > items[i].stack_size)
				{
					items[i].quantity = current_item.stack_size;
					current_item.quantity = items[i].stack_size - current_item.quantity;
					UpdateButton(i);
				}
				else
				{
					items[i].quantity += current_item.quantity;
					current_item.quantity = 0;
					UpdateButton(i);
				}
			}
			
		}
		if(current_item.quantity > 0)
		{
			if(current_item.quantity < current_item.stack_size || !current_item.is_stackable)
			{
				items.Add(current_item);
				UpdateButton(items.Count - 1);
			}
			else
			{
				ItemResource temp_item = current_item.Copy();
				temp_item.quantity = current_item.stack_size;
				items.Add(temp_item);
				UpdateButton(items.Count - 1);
				current_item.quantity -= current_item.stack_size;
				AddItem(current_item);
			}
			
		}
	}

	public bool Remove(ItemResource item)
	{
		if(CanAfford(item))
		{
			ItemResource current_item = item.Copy();

			for(int i = 0; i < items.Count; i++)
			{
				if(items[i].id == current_item.id)
				{
					if(items[i].quantity - current_item.quantity < 0)
					{
						current_item.quantity -= items[i].quantity;
						items[i].quantity = 0;
						UpdateButton(i);
					}
					else
					{
						items[i].quantity -= current_item.quantity;
						current_item.quantity = 0;
						UpdateButton(i);
					}
				}
				if(current_item.quantity <= 0)
				{
					break;
				}
			}
			items.RemoveAll(x => x.quantity <= 0);
			if(current_item.quantity > 0)
			{
				Remove(current_item);
			}
			ReflowButtons();
			return true;
		}
		return false;
	}

	private bool CanAfford(ItemResource item)
	{
		List<ItemResource> current_items = items.Where(x => x.id == item.id).ToList();

		int i = 0;
		foreach (var item1 in current_items)
		{
			i += item1.quantity;
		}
		if (item.quantity < i)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void ReflowButtons()
	{
		for (int i = 0; i < inventory_size; i++)
		{
			UpdateButton(i);
		}
	}

	public void UpdateButton(int index)
	{
		if(items.ElementAtOrDefault(index) != null)
		{
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(items[index], index);
			
		}
		else
		{
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(null, index);
		}
		
	}
}
