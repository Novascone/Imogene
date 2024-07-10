using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class UI : Control
{

	private GridContainer item_grid_container;
	private PackedScene  inventory_button;
	[Export]
	private string item_button_path = "res://scenes/UI/inventory_button.tscn";
	[Export]
	public int inventory_size { get; set; } = 70;
	private Vector2 last_cursor_clicked_pos;

	public InventoryButton grabbed_object { get; set; }
	public InventoryButton hover_over_button { get; set; }
	public Button hover_over_button_non_inventory { get; set; }

	private List<ItemResource> items = new List<ItemResource>();

	public bool UI_element_open;
	public bool player_in_interact_area;
	private bool clicked_on;
	public bool over_trash;
	public bool over_head;
	public bool over_shoulders;
	public bool over_neck;
	public bool over_chest;
	public bool over_gloves;
	public bool over_bracers;
	public bool over_belt;
	public bool over_ring1;
	public bool over_ring2;
	public bool over_main;
	public bool over_off;
	public bool over_pants;
	public bool over_boots;
	public bool over_slot;
	public bool buttons_informed;

	private CustomSignals _customSignals; // Instance of CustomSignals
	

	public Player this_player;

	
	
	public Control character_inventory;
	public PanelContainer interact_inventory;
	public Control abilities;
	private VBoxContainer character_Sheet_depth;
	private VBoxContainer mats;
	public HUD hud;

	


	public Sprite2D cursor;


	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 25.0f;
	// private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	// Called when the node enters the scene tree for the first time.
	private PanelContainer interact_bar;
	private int health;
	private int resource;
	public bool inventory_open;
	public bool abilities_open;
	public bool abilities_secondary_ui_open;
	public Node3D player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// Cursor
		cursor = GetNode<Sprite2D>("Cursor/CursorSprite");
		mouse_pos = cursor.Position;

		// UI sections
		

		character_inventory = GetNode<Control>("Inventory");
		interact_inventory = GetNode<PanelContainer>("InteractInventory");
		character_Sheet_depth = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/CharacterSheetDepth");
		mats = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/Mats");
		abilities = GetNode<Control>("Abilities");
		hud = GetNode<HUD>("HUD");
		

		// HUD icons
		// health_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelHealthContainer/HealthContainer/HealthIcon");
		// resource_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelResourceContainer/ResourceContainer/ResourceIcon");
		interact_bar = GetNode<PanelContainer>("HUD/InteractBar");


		// Signals subscribed to
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		_customSignals.UIResourceUpdate += HandleUIResourceUpdate;
		// _customSignals.UIHealth += HandleUIHealth;
		// _customSignals.UIResource += HandleUIResource;
		_customSignals.Interact += HandleInteract;
		// _customSignals.PlayerInfo += HandlePlayerInfo;
		_customSignals.OverSlot += HandleOverSlot;
		
		_customSignals.AbilityUISecondaryOpen += HandleAbilityUISecondaryOpen;
		
		_customSignals.HideCursor += HandleHideCursor;
		

		// Items section
		item_grid_container = GetNode<GridContainer>("Inventory/CharacterInventoryContainer/FullInventory/CharacterInventory/Items/ItemsGrid");
		inventory_button = ResourceLoader.Load<PackedScene>(item_button_path);
		PopulateButtons();


		FocusMode = FocusModeEnum.All;
	}

   
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(inventory_open && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
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
		// var node_focused = GetViewport().GuiGetFocusOwner();
		// if(node_focused != null)
		// {
		// 	GD.Print(node_focused.Name);
		// }
		
		// GD.Print(HasFocus());

		// GD.Print("Hover over button ", hover_over_button.Name);
		if(this_player != null)
		{
			if(!buttons_informed)
			{
				ConveyPlayerToButtons();
			}
			
		}
		if(inventory_open || abilities_open)
		{
			ControllerCursor();
			HideCursor();
			ShowCursor();
		}
		

		// UpdateHealth();
		// UpdateResource();

		if(inventory_open || abilities_open || player_in_interact_area)
		{
			UI_element_open = true;
		}
		else
		{
			UI_element_open = false;
		}

		if(Input.IsActionJustPressed("B"))
		{
			
			GrabFocus();
			GD.Print(HasFocus());
			if(inventory_open)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), false);
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
				// this_player.can_move = true;
				inventory_open = false;
				character_inventory.Hide();
				mats.Hide();
				character_Sheet_depth.Hide();
				cursor.Hide();
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
			
			GrabFocus();
			if(!inventory_open)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), true);
				inventory_open = true;
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),true);
				// this_player.can_move = false;
				character_inventory.Show();
				cursor.Show();
				abilities_open = false;
				abilities.Hide();
			}
			else
			{
				_customSignals.EmitSignal(nameof(CustomSignals.ZoomCamera), false);
				inventory_open = false;
				// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
				// this_player.can_move = true;
				character_inventory.Hide();
				mats.Hide();
				character_Sheet_depth.Hide();
				if(!abilities_open)
				{
					cursor.Hide();
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
							this_player.equipmentController.GetEquipableInfo((ArmsResource)hover_over_button.inventory_item);
							
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
							InventoryButton button = GetNode<Area2D>("Cursor/CursorSprite/CursorArea2D").GetNode<InventoryButton>("CursorButton");
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
							clicked_on = false;
							DeleteItem(grabbed_object);
							InventoryButton button = GetNode<Area2D>("Cursor/CursorSprite/CursorArea2D").GetNode<InventoryButton>("CursorButton");
							button.Visible = false;
						}
						else
						{
							clicked_on = false;
							InventoryButton button = GetNode<Area2D>("Cursor/CursorSprite/CursorArea2D").GetNode<InventoryButton>("CursorButton");
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

	public void ControllerCursor() // Control the cursor with the joysticks
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		Vector2 mouse_direction = Vector2.Zero;
	
		if (Input.IsActionPressed("CursorLeft"))
		{
			mouse_direction.X -= 1.0f;		
		}
		if (Input.IsActionPressed("CursorRight"))
		{
			mouse_direction.X += 1.0f;
		}
		if (Input.IsActionPressed("CursorUp"))
		{
			mouse_direction.Y -= 1.0f;
		}
		if (Input.IsActionPressed("CursorDown"))
		{
			mouse_direction.Y += 1.0f;
		}
		if(mouse_direction != Vector2.Zero)
		{
			GetViewport().WarpMouse(mouse_pos + mouse_direction * Mathf.Lerp(0, mouse_max_speed, 0.1f));
		}
		cursor.Position = GetViewport().GetMousePosition();
	}

	public void NavigateMenus()
	{
	
	}

	public void HideCursor() // Hide cursor is the player wants to navigate with the D-Pad
	{
		if(Input.IsActionJustPressed("D-PadUp") || Input.IsActionJustPressed("D-PadDown") || Input.IsActionJustPressed("D-PadLeft") || Input.IsActionJustPressed("D-PadRight"))
		{
			cursor.Hide();
		}
	}
	private void ShowCursor()
	{
		if(Input.IsActionJustPressed("CursorUp") || Input.IsActionJustPressed("CursorDown") || Input.IsActionJustPressed("CursorLeft") || Input.IsActionJustPressed("CursorRight"))
		{
			cursor.Show();
		}
	}
	
    // private void HandleUIHealth(int amount)
    // {
	// 	health = amount;
    //     health_icon.MaxValue = amount;
    // }
	//    private void HandleUIResource(int amount)
    // {
    //     resource_icon.MaxValue = amount;
    // }


	public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			mouse_pos = mouseMotion.Position;
		}
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

	private void PopulateButtons()
	{
		for (int i = 0; i < inventory_size; i++)
		{
			InventoryButton current_inventory_button = inventory_button.Instantiate<InventoryButton>();
			current_inventory_button.hud = hud;
			item_grid_container.AddChild(current_inventory_button);
		}
	}
	private void ConveyPlayerToButtons()
	{
		foreach(InventoryButton inventoryButton in item_grid_container.GetChildren().Cast<InventoryButton>())
		{
			inventoryButton.this_player = this_player;
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



	public void _on_sheet_label_button_down()
	{
		GD.Print("sheet button down");
		character_Sheet_depth.Visible = !character_Sheet_depth.Visible;
		mats.Hide();
	}

	public void _on_mats_button_down()
	{
		mats.Visible = !mats.Visible;
		character_Sheet_depth.Hide();
	}

	public void _on_abilities_label_button_down()
	{
		abilities_open = true;
		// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),true);
		// this_player.can_move = false;
		inventory_open = false;
		character_inventory.Hide();
		abilities.Show();
	}

	public void _on_add_button_button_down()
	{
		// AddItem(ResourceLoader.Load<ItemResource>("res://resources/armor.tres"));
		AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/helmet/helmet.tres"));
		GD.Print("Button 1 down");
	}

	public void _on_remove_button_button_down()
	{
		Remove(ResourceLoader.Load<ItemResource>("res://resources/armor.tres"));
	}

	public void _on_add_button_2_button_down()
	{
		AddItem(ResourceLoader.Load<ItemResource>("res://resources/HealthPotion.tres"));
		GD.Print("Button 2 down");
	}
	public void _on_add_button_3_button_down()
	{
		AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/chest/chest.tres"));
		GD.Print("Button 3 down");
	}
	public void _on_add_button_4_button_down()
	{
		AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/shoulders_split.tres"));
		GD.Print("Button 4 down");
	}

	public void _on_remove_button_2_button_down()
	{
		Remove(ResourceLoader.Load<ItemResource>("res://resources/HealthPotion.tres"));
	}

	public void _on_remove_equiped_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.RemoveEquipped));
	}

	public void _on_cursor_area_2d_area_entered(Area2D area)
	{
		Control button = area.GetParent<Control>();
		if(button is InventoryButton) 
		{
			hover_over_button = (InventoryButton)button;
			hover_over_button.GrabFocus();
		}
		else if (button is Button && button.Visible)
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
				GD.Print("Release focus");
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

	// private void HandlePlayerInfo(Player player)
    // {
    //     this_player = player;
    // }

	public void GetPlayerInfo(Player player)
	{
		this_player = player;
	}

	 private void HandleHideCursor()
    {
        abilities_open = false;
		inventory_open = false;
    }


    private void HandleAbilityUISecondaryOpen(bool secondary_open)
    {
        abilities_secondary_ui_open = secondary_open;
    }

	public void TestFunction()
	{
		GD.Print("This is a test function");
	}

    

    // private void UpdateHealth() // Updates UI health
	// {
	// 	// GD.Print("Health: ", health);
	// 	health_icon.Value = health;
	// 	// GD.Print("Health Icon Value: ", health_icon.MaxValue);
	// }

	private void UpdateResource() // Updates UI resource
	{
		resource_icon.Value = resource;
	}

	private void HandleUIHealthUpdate(int health_update)
    {
		
        health -= health_update;
		
    }

	public void UIHealthUpdate(int health_update)
	{
		health -= health_update;
	}
    private void HandleUIResourceUpdate(int resource_amount)
    {
        resource -= resource_amount;
    }

	private void HandleInteract(Area3D area, bool in_interact_area, bool interacting)
    {
	
		if(in_interact_area)
		{
			
			HBoxContainer TextContainer = (HBoxContainer)interact_bar.GetChild(0);
			Label press = (Label)TextContainer.GetChild(0);
			Label object_to_interact = (Label)TextContainer.GetChild(1);
			press.Text = "A : ";
			object_to_interact.Text = "Interact with " + area.GetParent().Name;
			interact_bar.Show();
			player_in_interact_area = true;
			if(interacting)
			{
				interact_inventory.Show();
			}
			if(!interacting)
			{
				interact_inventory.Hide();
			}

		}
		else
		{
			interact_bar.Visible = false;
			player_in_interact_area = false;
			HBoxContainer TextContainer = (HBoxContainer)interact_bar.GetChild(0);
			Label press = (Label)TextContainer.GetChild(0);
			Label object_to_interact = (Label)TextContainer.GetChild(1);
			press.Text = null;
			object_to_interact.Text = null;
			interact_inventory.Hide();
			
		}
		
    }

	private void HandleOverSlot(string slot)
    {
		// GD.Print("Over Head signal received");
		over_slot = true;
        if(slot == "Head")
		{
			over_head = true;
		}
		else
		{
			over_head = false;
		}
    }

	

}
