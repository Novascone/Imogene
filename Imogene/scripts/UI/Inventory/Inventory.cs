using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : CanvasLayer
{

	private GridContainer grid_container;
	private PackedScene  inventory_button;
	[Export]
	private string item_button_path = "res://scenes/UI/inventory_button.tscn";
	[Export]
	public int inventory_size { get; set; } = 24;
	private Vector2 last_cursor_clicked_pos;

	public InventoryButton grabbed_object { get; set; }
	public InventoryButton hover_over_button { get; set; }
	public Button hover_over_button_non_inventory { get; set; }

	private List<Item> items = new List<Item>();

	private bool clicked_on;
	private bool over_trash;
	
	private CustomSignals _customSignals; // Instance of CustomSignals

	private player this_player;

	public Panel character_inventory;
	public Panel interact_inventory;

	public Sprite2D cursor;
	public Area2D cursor_area;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		_customSignals.InteractPressed += HandleInteractPressed;
		
		grid_container = GetNode<GridContainer>("CharacterInventory/ScrollContainer/GridContainer");
		inventory_button = ResourceLoader.Load<PackedScene>(item_button_path);
		PopulateButtons();

		character_inventory = GetNode<Panel>("CharacterInventory");
		interact_inventory = GetNode<Panel>("InteractInventory");
		cursor_area = GetNode<Area2D>("CursorArea2D");
		cursor = GetNode<Sprite2D>("Cursor");
	}

    


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(character_inventory.Visible)
		{
			GetNode<Area2D>("CursorArea2D").Position = GetTree().Root.GetMousePosition();
			if(hover_over_button != null)
			{

				if(!clicked_on)
				{
					if(Input.IsActionJustPressed("Interact") && hover_over_button is InventoryButton)
					{
						hover_over_button.InventoryButtonPressed();
						if(hover_over_button.inventory_item.type_of_item == "generic")
						{
							_customSignals.EmitSignal(nameof(CustomSignals.ItemInfo), hover_over_button.inventory_item);
						}
						if(hover_over_button.inventory_item.type_of_item == "consumable")
						{
							_customSignals.EmitSignal(nameof(CustomSignals.ConsumableInfo), (Consumable)hover_over_button.inventory_item);
						}
						if(hover_over_button.inventory_item.type_of_item == "equipable")
						{
							_customSignals.EmitSignal(nameof(CustomSignals.EquipableInfo), (Equipable)hover_over_button.inventory_item);
						}
					}
					if(Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse"))
					{
						clicked_on = true;
						grabbed_object = hover_over_button;
						last_cursor_clicked_pos = GetTree().Root.GetMousePosition();
						InventoryButton button = GetNode<Area2D>("CursorArea2D").GetNode<InventoryButton>("InventoryButton");
						button.Visible = true;
						button.UpdateItem(grabbed_object.inventory_item, 0);
					}
				}
				else
				{
					if(Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse"))
					{
						if(over_trash)
						{
							clicked_on = false;
							DeleteItem(grabbed_object);
							InventoryButton button = GetNode<Area2D>("CursorArea2D").GetNode<InventoryButton>("InventoryButton");
							button.Visible = false;
							GD.Print("trashed");
							
						}
						else
						{
							clicked_on = false;
							// SwapButtons(grabbed_object, hover_over_button);
							InventoryButton button = GetNode<Area2D>("CursorArea2D").GetNode<InventoryButton>("InventoryButton");
							button.Visible = false;
						}
					
					}
					
				}
			}
			if((Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse")) && over_trash)
			{
				clicked_on = false;
				DeleteItem(grabbed_object);
			}
		}
	}

	private void SwapButtons(InventoryButton button1, InventoryButton button2)
	{
		int button1_index = button1.GetIndex();
		int button2_index = button2.GetIndex();
		grid_container.MoveChild(button1, button2_index);
		grid_container.MoveChild(hover_over_button, button1_index);
	}

	public void DeleteItem(InventoryButton inventory_button)
	{
		items.Remove(inventory_button.inventory_item);
		ReflowButtons();
		InventoryButton button = GetNode<Area2D>("CursorArea2D").GetNode<InventoryButton>("InventoryButton");
		button.Visible = false;
	}

	private void PopulateButtons()
	{
		for (int i = 0; i < inventory_size; i++)
		{
			InventoryButton current_inventory_button = inventory_button.Instantiate<InventoryButton>();
			grid_container.AddChild(current_inventory_button);
		}
	}

	public void Add(Item item)
	{
		Item current_item = item.Copy();
		
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
				Item temp_item = current_item.Copy();
				temp_item.quantity = current_item.stack_size;
				items.Add(temp_item);
				UpdateButton(items.Count - 1);
				current_item.quantity -= current_item.stack_size;
				Add(current_item);
			}
			
		}
	}

	public bool Remove(Item item)
	{
		if(CanAfford(item))
		{
			Item current_item = item.Copy();

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

	private bool CanAfford(Item item)
	{
		List<Item> current_items = items.Where(x => x.id == item.id).ToList();

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
			grid_container.GetChild<InventoryButton>(index).UpdateItem(items[index], index);
		}
		else
		{
			grid_container.GetChild<InventoryButton>(index).UpdateItem(null, index);
		}
		
	}

	public void _on_add_button_button_down()
	{
		Add(ResourceLoader.Load<Item>("res://resources/armor.tres"));
	}

	public void _on_remove_button_button_down()
	{
		Remove(ResourceLoader.Load<Item>("res://resources/armor.tres"));
	}

	public void _on_cursor_area_2d_area_entered(Area2D area)
	{
		Control button = area.GetParent<Control>();
		if(button is InventoryButton) 
		{
			hover_over_button = (InventoryButton)button;
			hover_over_button.GrabFocus();
		}
		else if (button is Button)
		{
			hover_over_button_non_inventory = (Button)button;
			hover_over_button_non_inventory.GrabFocus();
		}
		
	}

	public void _on_cursor_area_2d_area_exited(Area2D area)
	{
		if(area.GetParent() is InventoryButton)
		{
			if(hover_over_button != null)
			{
				hover_over_button.ReleaseFocus();
				hover_over_button = null;
			}
			
		}
		else if (area.GetParent() is Button)
		{
			if(hover_over_button_non_inventory != null)
			{
				hover_over_button_non_inventory.ReleaseFocus();
				hover_over_button_non_inventory = null;
			}
			
		}
		
	}

	public void _on_trash_area_2d_area_entered(Area2D area)
	{	
		over_trash = true;
	}

	public void _on_trash_area_2d_area_exited(Area2D area)
	{
		over_trash = false;
	}
	public void _on_add_button_2_button_down()
	{
		Add(ResourceLoader.Load<Item>("res://resources/HealthPotion.tres"));
	}

	public void _on_remove_button_2_button_down()
	{
		Remove(ResourceLoader.Load<Item>("res://resources/HealthPotion.tres"));
	}

	public void _on_remove_equiped_button_down()
	{
		GD.Print("button down");
		_customSignals.EmitSignal(nameof(CustomSignals.RemoveEquipped));
	}

	private void HandlePlayerInfo(player player)
    {
        this_player = player;
    }

	private void HandleInteractPressed(bool in_area)
    {
        if(!interact_inventory.Visible)
		{
			interact_inventory.Visible = true;
		}
		else
		{
			interact_inventory.Visible = false;
		}
		if(!in_area)
		{
			interact_inventory.Visible = false;
		}
    }

}
