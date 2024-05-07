using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class UI : CanvasLayer
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

	private List<Item> items = new List<Item>();

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
	private CustomSignals _customSignals; // Instance of CustomSignals
	

	public player this_player;

	public GridContainer l_cross_primary;
	public GridContainer r_cross_primary;
	public GridContainer l_cross_secondary;
	public GridContainer r_cross_secondary;
	public Button l_cross_primary_up_action;
	public CanvasLayer character_inventory;
	public PanelContainer interact_inventory;
	public CanvasLayer skills;
	private VBoxContainer character_Sheet_depth;
	private VBoxContainer mats;

	private bool l_cross_primary_selected = true;
	private bool r_cross_primary_selected = true;
	private bool l_cross_secondary_selected = false;
	private bool r_cross_secondary_selected = false;


	public Sprite2D cursor;


	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 20.0f;
	// private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	// Called when the node enters the scene tree for the first time.
	private PanelContainer interact_bar;
	private int health;
	private int resource;
	private bool inventory_open;
	private bool skills_open;
	public Node3D player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// Cursor
		cursor = GetNode<Sprite2D>("Cursor");
		mouse_pos = cursor.Position;

		// UI sections
		l_cross_primary = GetNode<GridContainer>("HUD/BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary");
		l_cross_primary_up_action = GetNode<Button>("HUD/BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossPrimary/LCrossPrimaryUpAction");

		r_cross_primary = GetNode<GridContainer>("HUD/BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossPrimary");
		l_cross_secondary = GetNode<GridContainer>("HUD/BottomHUD/BottomHUDVBox/BottomHUDHBox/LCross/LCrossSecondary");
		r_cross_secondary = GetNode<GridContainer>("HUD/BottomHUD/BottomHUDVBox/BottomHUDHBox/RCross/RCrossSecondary");

		character_inventory = GetNode<CanvasLayer>("Inventory");
		interact_inventory = GetNode<PanelContainer>("InteractInventory");
		character_Sheet_depth = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/CharacterSheetDepth");
		mats = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/Mats");
		skills = GetNode<CanvasLayer>("Skills");
		

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
		_customSignals.PlayerInfo += HandlePlayerInfo;
		_customSignals.OverSlot += HandleOverSlot;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;

		// Items section
		item_grid_container = GetNode<GridContainer>("Inventory/CharacterInventoryContainer/FullInventory/CharacterInventory/Items/ItemsGrid");
		inventory_button = ResourceLoader.Load<PackedScene>(item_button_path);
		PopulateButtons();


	}

    private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    {
        l_cross_primary_up_action.Icon = icon;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(this_player != null)
		{
			
		}
		if(inventory_open || skills_open)
		{
			ControllerCursor();
			HideCursor();
			ShowCursor();
		}
		

		// UpdateHealth();
		// UpdateResource();

		ChangeActionBars();

		if(Input.IsActionJustPressed("Inventory"))
		{
			
			if(!inventory_open)
			{
				inventory_open = true;
				character_inventory.Show();
				cursor.Show();
				skills_open = false;
				skills.Hide();
			}
			else
			{
				inventory_open = false;
				character_inventory.Hide();
				if(!skills_open)
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
					hover_over_button = (InventoryButton)GetViewport().GuiGetFocusOwner();
				}
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
						GD.Print("hover over button name" + hover_over_button.Name);
						clicked_on = true;
						grabbed_object = hover_over_button;
						last_cursor_clicked_pos = GetTree().Root.GetMousePosition();
						if(hover_over_button is InventoryButton)
						{
							InventoryButton button = GetNode<Area2D>("Cursor/CursorArea2D").GetNode<InventoryButton>("CursorButton");
							button.Visible = true;
							button.UpdateItem(grabbed_object.inventory_item, 0);
						}
						
						
					}
				}
				else
				{
					if(Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse") && hover_over_button is InventoryButton)
					{
						if(over_trash)
						{
							clicked_on = false;
							DeleteItem(grabbed_object);
							InventoryButton button = GetNode<Area2D>("Cursor/CursorArea2D").GetNode<InventoryButton>("CursorButton");
							button.Visible = false;
						}
						else
						{
							clicked_on = false;
							InventoryButton button = GetNode<Area2D>("Cursor/CursorArea2D").GetNode<InventoryButton>("CursorButton");
							button.Visible = false;
							if(hover_over_button is InventoryButton)
							{
								if(grabbed_object != null && hover_over_button != null)
								{
								SwapButtons(grabbed_object, hover_over_button);
								}
							}
							
						}
					
					}
					
				}
			}
			if((Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse")) && over_trash)
			{
				clicked_on = false;
				DeleteItem(grabbed_object);
			}
			if((Input.IsActionJustPressed("InteractMenu") || Input.IsActionJustPressed("RightMouse")) && over_head)
			{
				GD.Print("overhead clicking");
				if(grabbed_object.inventory_item.type_of_item == "equipable")
				{
					clicked_on = false;
					InventoryButton button = GetNode<Area2D>("Cursor/CursorArea2D").GetNode<InventoryButton>("CursorButton");
					button.Visible = false;
					GD.Print("clicked over head");
					_customSignals.EmitSignal(nameof(CustomSignals.EquipableInfo), (Equipable)grabbed_object.inventory_item);
			
				}
				
			}
		}
		
	}

	public void ControllerCursor()
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
			Input.WarpMouse(mouse_pos + mouse_direction * Mathf.Lerp(0, mouse_max_speed, 0.1f));
		}
		cursor.Position = GetViewport().GetMousePosition();
	}

	private void HideCursor()
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
			press.Text = "Y : ";
			object_to_interact.Text = "Interact with " + area.GetParent().Name;
			interact_bar.Visible = true;
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
			HBoxContainer TextContainer = (HBoxContainer)interact_bar.GetChild(0);
			Label press = (Label)TextContainer.GetChild(0);
			Label object_to_interact = (Label)TextContainer.GetChild(1);
			press.Text = null;
			object_to_interact.Text = null;
			interact_inventory.Hide();
			
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
			item_grid_container.AddChild(current_inventory_button);
		}
	}

	public void AddItem(Item item)
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
				AddItem(current_item);
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
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(items[index], index);
		}
		else
		{
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(null, index);
		}
		
	}

	public void ChangeActionBars()
	{
		if(!inventory_open && !skills_open && Input.IsActionJustPressed("D-PadLeft"))
		{
			if(l_cross_primary_selected)
			{
				l_cross_primary_selected = false;
				l_cross_secondary_selected = true;
				_customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
				l_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;
				l_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				l_cross_secondary.Modulate = new Color(Colors.White, 1f);
			}
			else
			{
				l_cross_primary_selected = true;
				l_cross_secondary_selected = false;
				_customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
				l_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				l_cross_primary.Modulate = new Color(Colors.White, 1f);
				l_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;
				l_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
			}
			
		}
		if(!inventory_open && !skills_open && Input.IsActionJustPressed("D-PadRight"))
		{
			if(r_cross_primary_selected)
			{
				r_cross_primary_selected = false;
				r_cross_secondary_selected = true;
				_customSignals.EmitSignal(nameof(CustomSignals.RCrossPrimaryOrSecondary), r_cross_primary_selected);
				r_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				r_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				r_cross_secondary.Modulate = new Color(Colors.White, 1f);
			}
			else
			{
				r_cross_primary_selected = true;
				r_cross_secondary_selected = false;
				_customSignals.EmitSignal(nameof(CustomSignals.RCrossPrimaryOrSecondary), r_cross_primary_selected);
				r_cross_primary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
				r_cross_primary.Modulate = new Color(Colors.White, 1f);
				r_cross_secondary.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
				r_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
			}
			
		}
	}

	public void _on_sheet_button_down()
	{
		character_Sheet_depth.Visible = !character_Sheet_depth.Visible;
		mats.Hide();
	}

	public void _on_mats_button_down()
	{
		mats.Visible = !mats.Visible;
		character_Sheet_depth.Hide();
	}

	public void _on_skills_label_button_down()
	{
		skills_open = true;
		inventory_open = false;
		character_inventory.Hide();
		skills.Show();
	}

	public void _on_add_button_button_down()
	{
		AddItem(ResourceLoader.Load<Item>("res://resources/armor.tres"));
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

	

	public void _on_add_button_2_button_down()
	{
		AddItem(ResourceLoader.Load<Item>("res://resources/HealthPotion.tres"));
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

	private void HandleOverSlot(string slot)
    {
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
