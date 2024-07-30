using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

public partial class UI : Control
{

	
	
	[Export] public int inventory_size { get; set; } = 70;
	public Vector2 last_cursor_clicked_pos;

	

	public List<ItemResource> items = new List<ItemResource>();

	public bool UI_element_open;
	public bool player_in_interact_area;
	public bool clicked_on;
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

	public CustomSignals _customSignals; // Instance of CustomSignals
	

	public Player this_player;

	
	
	public Inventory inventory;
	public InventoryInfo inventory_info;
	public PanelContainer interact_inventory;
	public AbilitiesInterface abilities;
	// public 
	public VBoxContainer character_Sheet_depth;
	public Panel armor;
	public VBoxContainer mats;
	public HUD hud;
	public Journal journal;

	


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

	public InventoryButton cursor_button;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// Cursor
		cursor = GetNode<Sprite2D>("Cursor/CursorSprite");

		mouse_pos = cursor.Position;

		// UI sections
		

		inventory = GetNode<Inventory>("Inventory");
		inventory_info = GetNode<InventoryInfo>("Inventory/CharacterInventoryContainer");
		interact_inventory = GetNode<PanelContainer>("InteractInventory");
		character_Sheet_depth = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/CharacterSheetDepth");
		armor = GetNode<Panel>("Inventory/CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor");
		mats = GetNode<VBoxContainer>("Inventory/CharacterInventoryContainer/FullInventory/Mats");
		cursor_button = GetNode<Area2D>("Cursor/CursorSprite/CursorArea2D").GetNode<InventoryButton>("CursorButton");
		abilities = GetNode<AbilitiesInterface>("Abilities");
		hud = GetNode<HUD>("HUD");
		journal = GetNode<Journal>("Journal");

		foreach (GearInfo gear_button in armor.GetChildren())
		{
			gear_button.GetUIInfo(this);
		}
		

		abilities.GetUIInfo(this);		

		// health_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelHealthContainer/HealthContainer/HealthIcon");
		// resource_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelResourceContainer/ResourceContainer/ResourceIcon");
		interact_bar = GetNode<PanelContainer>("HUD/InteractBar");


		// Signals subscribed to
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		// _customSignals.UIResourceUpdate += HandleUIResourceUpdate;
		// _customSignals.UIHealth += HandleUIHealth;
		// _customSignals.UIResource += HandleUIResource;
		// _customSignals.Interact += HandleInteract;
		// _customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.OverSlot += HandleOverSlot;
		
		// _customSignals.AbilityUISecondaryOpen += HandleAbilityUISecondaryOpen;
		
		// _customSignals.HideCursor += HandleHideCursor;

		// inventory.ConveyPlayerToButtons();
		

		// Items section
		
		

		inventory.GetUIInfo(this);
		inventory_info.GetUIInfo(this);
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
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(inventory_open && eventJoypadButton.ButtonIndex == JoyButton.DpadUp)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(inventory_open && eventJoypadButton.ButtonIndex == JoyButton.DpadDown)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(inventory_open && eventJoypadButton.ButtonIndex == JoyButton.DpadRight)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(inventory_open && eventJoypadButton.ButtonIndex == JoyButton.DpadLeft)
			{
				GD.Print("event accepted ");
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

		// GD.Print(over_trash);
		

		// GD.Print("Hover over button ", hover_over_button.Name);
		
		if(inventory.Visible || abilities_open)
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
			if(cursor != null && !cursor.Visible && inventory_open)
			{
				cursor.Show();
			}
			
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
		inventory.Hide();
		abilities.Show();
	}

	public void _on_journal_label_button_down()
	{
		GD.Print("journal button down");
		inventory_open = true;
		
		journal.Show();

	}

	public void _on_add_button_button_down()
	{
		// AddItem(ResourceLoader.Load<ItemResource>("res://resources/armor.tres"));
		inventory.AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/helmet/helmet.tres"));
		GD.Print("Button 1 down");
	}

	public void _on_remove_button_button_down()
	{
		inventory.Remove(ResourceLoader.Load<ItemResource>("res://resources/armor.tres"));
	}

	public void _on_add_button_2_button_down()
	{
		inventory.AddItem(ResourceLoader.Load<ItemResource>("res://resources/HealthPotion.tres"));
		GD.Print("Button 2 down");
	}
	public void _on_add_button_3_button_down()
	{
		inventory.AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/chest/chest.tres"));
		GD.Print("Button 3 down");
	}
	public void _on_add_button_4_button_down()
	{
		inventory.AddItem(ResourceLoader.Load<ItemResource>("res://scenes/armor/shoulders_split.tres"));
		GD.Print("Button 4 down");
	}

	public void _on_remove_button_2_button_down()
	{
		inventory.Remove(ResourceLoader.Load<ItemResource>("res://resources/HealthPotion.tres"));
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
			inventory.hover_over_button = (InventoryButton)button;
			inventory.hover_over_button.GrabFocus();
		}
		else if (button is Button && button.Visible)
		{
			inventory.hover_over_button_non_inventory = (Button)button;
			inventory.hover_over_button_non_inventory.GrabFocus();
		}
		
	}

	public void _on_cursor_area_2d_area_exited(Area2D area)
	{
		if(area.GetParent() is InventoryButton)
		{
			if(inventory.hover_over_button != null)
			{
				inventory.hover_over_button.ReleaseFocus();
				GD.Print("Release focus");
				inventory.hover_over_button = null;
			}
			
		}
		else if (area.GetParent() is Button)
		{
			if(inventory.hover_over_button_non_inventory != null)
			{
				inventory.hover_over_button_non_inventory.ReleaseFocus();
				inventory.hover_over_button_non_inventory = null;
			}
			
		}
		
	}

	public void GetPlayerInfo(Player player)
	{
		this_player = player;
	}

	// private void HandleHideCursor()
    // {
    //     abilities_open = false;
	// 	inventory_open = false;
    // }

	public void CloseInterface()
	{
		abilities_open = false;
		inventory_open = false;
	}


    // private void HandleAbilityUISecondaryOpen(bool secondary_open)
    // {
    //     abilities_secondary_ui_open = secondary_open;
    // }

	public void TestFunction()
	{
		GD.Print("This is a test function");
	}

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

	
	public void GetInteract(Area3D area, bool in_interact_area, bool interacting)
	{
		// GD.Print("Getting interaction");
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

	// private void HandleOverSlot(string slot)
    // {
	// 	// GD.Print("Over Head signal received");
	// 	over_slot = true;
    //     if(slot == "Head")
	// 	{
	// 		over_head = true;
	// 	}
	// 	else
	// 	{
	// 		over_head = false;
	// 	}
    // }

	// private void HandleInteract(Area3D area, bool in_interact_area, bool interacting)
    // {
	
	// 	if(in_interact_area)
	// 	{
			
	// 		HBoxContainer TextContainer = (HBoxContainer)interact_bar.GetChild(0);
	// 		Label press = (Label)TextContainer.GetChild(0);
	// 		Label object_to_interact = (Label)TextContainer.GetChild(1);
	// 		press.Text = "A : ";
	// 		object_to_interact.Text = "Interact with " + area.GetParent().Name;
	// 		interact_bar.Show();
	// 		player_in_interact_area = true;
	// 		if(interacting)
	// 		{
	// 			interact_inventory.Show();
	// 		}
	// 		if(!interacting)
	// 		{
	// 			interact_inventory.Hide();
	// 		}

	// 	}
	// 	else
	// 	{
	// 		interact_bar.Visible = false;
	// 		player_in_interact_area = false;
	// 		HBoxContainer TextContainer = (HBoxContainer)interact_bar.GetChild(0);
	// 		Label press = (Label)TextContainer.GetChild(0);
	// 		Label object_to_interact = (Label)TextContainer.GetChild(1);
	// 		press.Text = null;
	// 		object_to_interact.Text = null;
	// 		interact_inventory.Hide();
			
	// 	}
		
    // }

	 // private void UpdateHealth() // Updates UI health
	// {
	// 	// GD.Print("Health: ", health);
	// 	health_icon.Value = health;
	// 	// GD.Print("Health Icon Value: ", health_icon.MaxValue);
	// }

	// private void HandlePlayerInfo(Player player)
    // {
    //     this_player = player;
    // }

}
