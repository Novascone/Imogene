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
	private bool over_trash;
	private bool over_head;
	private bool over_shoulders;
	private bool over_neck;
	private bool over_chest;
	private bool over_gloves;
	private bool over_bracers;
	private bool over_belt;
	private bool over_ring1;
	private bool over_ring2;
	private bool over_main;
	private bool over_off;
	private bool over_pants;
	private bool over_boots;
	private CustomSignals _customSignals; // Instance of CustomSignals
	private bool stats_need_updated;

	private player this_player;

	public PanelContainer character_inventory;
	public PanelContainer interact_inventory;

	// Character equipment
	private Button head_slot;
	private Button shoulder_slot;
	private Button neck_slot;
	private Button chest_slot;
	private Button gloves_slot;
	private Button bracers_slot;
	private Button belt_slot;
	private Button ring1_slot;
	private Button ring2_slot;
	private Button mainhand_slot;
	private Button offhand_slot;
	private Button pants_slot;
	private Button boots_slot;

	// Character stats
	private Button strength_label;
	private Label strength_value;
	private RichTextLabel strength_info_label;
	private string strength_info_text = " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} ";

	private Button dexterity_label;
	private Label dexterity_value;
	private RichTextLabel dexterity_info_label;
	private string dexterity_info_text = " Dexterity {0} \n * Primary stat for melee \n * Calculated by physical ranged  and critical damage \n * Increases damage by {1} \n * Increases critical chance by {2} * Increases critical damage by {3}";

	private Button intellect_label;
	private Label intellect_value;
	private RichTextLabel intellect_info_label;
	private string intellect_info_text = " Intellect {0} \n * main stat for spell damage \n * Increases spell damage by {1} \n * Increases spell hit chance by {2}";

	private Button vitality_label;
	private Label vitality_value;
	private RichTextLabel vitality_info_label;
	private string vitality_info_text = " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";

	private Button stamina_label;
	private Label stamina_value;
	private RichTextLabel stamina_info_label;
	private string stamina_info_text = " Stamina {0} \n * Primary stat for resource and regeneration \n * Increases total resource by {1} \n * Increases health and resource regeneration by {2} \n * Increases health by {3}";

	private Button wisdom_label;
	private Label wisdom_value;
	private RichTextLabel wisdom_info_label;
	private string wisdom_info_text = " Wisdom {0} \n * Primary stat for hit and interaction \n * Increases hit chance by {1}";

	private Button charisma_label;
	private Label charisma_value;
	private RichTextLabel charisma_info_label;
	private string charisma_info_text = "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";

	private Button trash_label;

	// Character details
	private Button damage_label;
	private Button resistance_label;
	private Button recovery_label;
	private Button rep_label;
	private Button level_label;
	private Button sheet_label;
	private Button mats_label;
	private Button gold_label;
	
	private VBoxContainer character_Sheet_depth;
	private VBoxContainer mats;

	public Sprite2D cursor;


	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 10.0f;
	private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	// Called when the node enters the scene tree for the first time.
	private PanelContainer interact_bar;
	private int health;
	private int resource;
	private bool inventory_open;
	public Node3D player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		stats_need_updated = true;
		health_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelHealthContainer/HealthContainer/HealthIcon");
		resource_icon = GetNode<TextureProgressBar>("HUD/BottomHUD/PanelResourceContainer/ResourceContainer/ResourceIcon");
		interact_bar = GetNode<PanelContainer>("HUD/InteractBar");



		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		_customSignals.UIResourceUpdate += HandleUIResourceUpdate;
		_customSignals.UIHealth += HandleUIHealth;
		_customSignals.UIResource += HandleUIResource;
		_customSignals.Interact += HandleInteract;
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;

		
		item_grid_container = GetNode<GridContainer>("CharacterInventoryContainer/RightUI/CharacterInventory/Items/ItemsGrid");
		inventory_button = ResourceLoader.Load<PackedScene>(item_button_path);
		PopulateButtons();

		character_inventory = GetNode<PanelContainer>("CharacterInventoryContainer");
		interact_inventory = GetNode<PanelContainer>("InteractInventory");
		character_Sheet_depth = GetNode<VBoxContainer>("CharacterInventoryContainer/RightUI/CharacterSheetDepth");
		mats = GetNode<VBoxContainer>("CharacterInventoryContainer/RightUI/Mats");
		cursor = GetNode<Sprite2D>("Cursor");
		mouse_pos = cursor.Position;

		// Armor 
		head_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Head");
		shoulder_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Shoulder");
		neck_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Neck");
		chest_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Chest");
		gloves_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Gloves");
		bracers_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Bracers");
		belt_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Belt");
		ring1_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring1");
		ring2_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring2");
		mainhand_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/MainHand");
		offhand_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/OffHand");
		pants_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Pants");
		boots_slot = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Boots");

		// Stats
		strength_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/StrengthLabel");
		strength_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/Value");
		strength_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/StrengthLabel/Info/MarginContainer/PanelContainer/Label");

		dexterity_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel");
		dexterity_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Value");
		dexterity_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel/Info/MarginContainer/PanelContainer/Label");

		intellect_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel");
		intellect_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Value");
		intellect_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel/Info/MarginContainer/PanelContainer/Label");

		vitality_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel");
		vitality_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Value");
		vitality_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel/Info/MarginContainer/PanelContainer/Label");

		stamina_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel");
		stamina_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Value");
		stamina_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel/Info/MarginContainer/PanelContainer/Label");

		wisdom_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel");
		wisdom_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Value");
		wisdom_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel/Info/MarginContainer/PanelContainer/Label");

		charisma_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel");
		charisma_value = GetNode<Label>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Value");
		charisma_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel/Info/MarginContainer/PanelContainer/Label");

		// Stats details
		damage_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel");
		damage_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel");
		resistance_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/ResistanceLabel");
		recovery_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/RecoveryLabel");
		rep_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/RepLabel");
		level_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel");
		sheet_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/SheetLabel");

		trash_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/BagSlots/TrashLabel");
		mats_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/MatsGold/MatsLabel");
		gold_label = GetNode<Button>("CharacterInventoryContainer/RightUI/CharacterInventory/MatsGold/GoldLabel");
	}

    


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(stats_need_updated)
		{
			stats();
			stats_need_updated = false;
		}
		

		if(inventory_open)
		{
			ControllerCursor();
			HideCursor();
			ShowCursor();
		}
		

		UpdateHealth();
		UpdateResource();

		if(Input.IsActionJustPressed("Inventory"))
		{
			
			if(!inventory_open)
			{
				inventory_open = true;
				character_inventory.Show();
				cursor.Show();
			}
			else
			{
				inventory_open = false;
				character_inventory.Hide();
				cursor.Hide();
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
			Input.WarpMouse(mouse_pos + mouse_direction * Mathf.Lerp(0, mouse_max_speed, 0.2f));
		}
		cursor.Position = GetViewport().GetMousePosition();
	}

	private void HideCursor()
	{
		if(Input.IsActionJustPressed("D-Pad_up") || Input.IsActionJustPressed("D-Pad_down") || Input.IsActionJustPressed("D-Pad_left") || Input.IsActionJustPressed("D-Pad_right"))
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

    private void UpdateHealth() // Updates UI health
	{
		// GD.Print("Health: ", health);
		health_icon.Value = health;
		// GD.Print("Health Icon Value: ", health_icon.MaxValue);
	}

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

	
    private void HandleUIHealth(int amount)
    {
		health = amount;
        health_icon.MaxValue = amount;
    }
	   private void HandleUIResource(int amount)
    {
        resource_icon.MaxValue = amount;
    }

	private void stats()
	{
		string strength = this_player.strength.ToString();
		string dexterity = this_player.dexterity.ToString();
		string intellect = this_player.intellect.ToString();
		string vitality = this_player.vitality.ToString();
		string stamina = this_player.stamina.ToString();
		string wisdom = this_player.wisdom.ToString();
		string charisma = this_player.charisma.ToString();
		
		strength_value.Text = strength;
		strength_info_label.Text = string.Format(strength_info_text, strength, 0, 0); // 3 variable(s)

		dexterity_value.Text = dexterity;
		dexterity_info_label.Text = string.Format(dexterity_info_text, dexterity, 0, 0, 0); // 4 variable(s)

		intellect_value.Text = intellect;
		intellect_info_label.Text = string.Format(intellect_info_text, intellect, 0, 0); // 3 variable(s)

		vitality_value.Text = vitality;
		vitality_info_label.Text = string.Format(vitality_info_text, vitality, 0, 0); // 2 variable(s)

		stamina_value.Text = stamina;
		stamina_info_label.Text = string.Format(stamina_info_text, stamina, 0, 0, 0); // 4 variable(s)

		wisdom_value.Text = wisdom;
		wisdom_info_label.Text = string.Format(wisdom_info_text, wisdom, 0); // 2 variable(s)

		charisma_value.Text = charisma;
		charisma_info_label.Text = string.Format(charisma_info_text, charisma); // 1 variable(s)
		GD.Print("updated");
		
	}

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
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(items[index], index);
		}
		else
		{
			item_grid_container.GetChild<InventoryButton>(index).UpdateItem(null, index);
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

	public void _on_head_focus_entered()
	{
		over_head = true;
		Control info = (Control)head_slot.GetChild(1);
		info.Show();
	}

	public void _on_head_focus_exited()
	{
		over_head = false;
		Control info = (Control)head_slot.GetChild(1);
		info.Hide();

	}

	public void _on_shoulder_focus_entered()
	{
		over_shoulders = true;
		Control info = (Control)shoulder_slot.GetChild(1);
		info.Show();
	}

	public void _on_shoulder_focus_exited()
	{
		over_shoulders = false;
		
		Control info = (Control)shoulder_slot.GetChild(1);
		info.Hide();
	}

	public void _on_neck_focus_entered()
	{
		over_neck = true;
		Control info = (Control)neck_slot.GetChild(1);
		info.Show();
	}

	public void _on_neck_focus_exited()
	{
		over_neck = false;
		Control info = (Control)neck_slot.GetChild(1);
		info.Hide();
	}

	public void _on_chest_focus_entered()
	{
		over_chest = true;
		Control info = (Control)chest_slot.GetChild(1);
		info.Show();
	}

	public void _on_chest_focus_exited()
	{
		over_chest = false;
		Control info = (Control)chest_slot.GetChild(1);
		info.Hide();
	}

	public void _on_gloves_focus_entered()
	{
		over_gloves = true;
		Control info = (Control)gloves_slot.GetChild(1);
		info.Show();
	}

	public void _on_gloves_focus_exited()
	{
		over_gloves = false;
		Control info = (Control)gloves_slot.GetChild(1);
		info.Hide();
	}

	public void _on_bracers_focus_entered()
	{
		over_bracers = true;
		Control info = (Control)bracers_slot.GetChild(1);
		info.Show();
	}

	public void _on_bracers_focus_exited()
	{
		over_bracers = false;
		Control info = (Control)bracers_slot.GetChild(1);
		info.Hide();
	}

	public void _on_belt_focus_entered()
	{
		over_belt = true;
		Control info = (Control)belt_slot.GetChild(1);
		info.Show();
	}

	public void _on_belt_focus_exited()
	{
		over_belt = false;
		Control info = (Control)belt_slot.GetChild(1);
		info.Hide();
	}

	public void _on_ring_1_focus_entered()
	{
		over_ring1 = true;
		Control info = (Control)ring1_slot.GetChild(1);
		info.Show();
	}

	public void _on_ring_1_focus_exited()
	{
		over_ring1 = false;
		Control info = (Control)ring1_slot.GetChild(1);
		info.Hide();
	}

	public void _on_ring_2_focus_entered()
	{
		over_ring2 = true;
		Control info = (Control)ring2_slot.GetChild(1);
		info.Show();
	}

	public void _on_ring_2_focus_exited()
	{
		over_ring2 = false;
		Control info = (Control)ring2_slot.GetChild(1);
		info.Hide();
	}

	public void _on_main_hand_focus_entered()
	{
		over_main = true;
		Control info = (Control)mainhand_slot.GetChild(1);
		info.Show();
	}

	public void _on_main_hand_focus_exited()
	{
		over_main = false;
		Control info = (Control)mainhand_slot.GetChild(1);
		info.Hide();
		
	}

	public void _on_off_hand_focus_entered()
	{
		over_off = true;
		Control info = (Control)offhand_slot.GetChild(1);
		info.Show();
	}

	public void _on_off_hand_focus_exited()
	{
		over_off = false;
		Control info = (Control)offhand_slot.GetChild(1);
		info.Hide();
	}

	public void _on_pants_focus_entered()
	{
		over_pants = true;
		Control info = (Control)pants_slot.GetChild(1);
		info.Show();
	}

	public void _on_pants_focus_exited()
	{
		over_pants = false;
		Control info = (Control)pants_slot.GetChild(1);
		info.Hide();
	}

	public void _on_boots_focus_entered()
	{
		over_boots = true;
		Control info = (Control)boots_slot.GetChild(1);
		info.Show();
	}

	public void _on_boots_focus_exited()
	{
		over_boots = false;
		Control info = (Control)boots_slot.GetChild(1);
		info.Hide();
	}


	public void _on_level_label_focus_entered()
	{
		Control info = (Control)level_label.GetChild(0);
		info.Show();
	}

	public void _on_level_label_focus_exited()
	{
		Control info = (Control)level_label.GetChild(0);
		info.Hide();
	}

	public void _on_rep_focus_entered()
	{
		Control info = (Control)rep_label.GetChild(0);
		info.Show();
	}

	public void _on_rep_focus_exited()
	{
		Control info = (Control)rep_label.GetChild(0);
		info.Hide();
	}

	public void _on_strength_label_focus_entered()
	{
		Control info = (Control)strength_label.GetChild(0);
		info.Show();
	}

	public void _on_strength_label_focus_exited()
	{
		Control info = (Control)strength_label.GetChild(0);
		info.Hide();
	}
	public void _on_dexterity_label_focus_entered()
	{
		Control info = (Control)dexterity_label.GetChild(0);
		info.Show();
	}

	public void _on_dexterity_label_focus_exited()
	{
		Control info = (Control)dexterity_label.GetChild(0);
		info.Hide();
	}

	public void _on_intellect_label_focus_entered()
	{
		Control info = (Control)intellect_label.GetChild(0);
		info.Show();
	}

	public void _on_intellect_label_focus_exited()
	{
		Control info = (Control)intellect_label.GetChild(0);
		info.Hide();
	}

	public void _on_vitality_label_focus_entered()
	{
		Control info = (Control)vitality_label.GetChild(0);
		info.Show();
	}

	public void _on_vitality_label_focus_exited()
	{
		Control info = (Control)vitality_label.GetChild(0);
		info.Hide();
	}

	public void _on_stamina_label_focus_entered()
	{
		Control info = (Control)stamina_label.GetChild(0);
		info.Show();
	}

	public void _on_stamina_label_focus_exited()
	{
		Control info = (Control)stamina_label.GetChild(0);
		info.Hide();
	}

	public void _on_wisdom_label_focus_entered()
	{
		Control info = (Control)wisdom_label.GetChild(0);
		info.Show();
	}

	public void _on_wisdom_label_focus_exited()
	{
		Control info = (Control)wisdom_label.GetChild(0);
		info.Hide();
	}

	public void _on_charisma_label_focus_entered()
	{
		Control info = (Control)charisma_label.GetChild(0);
		info.Show();
	}

	public void _on_charisma_label_focus_exited()
	{
		Control info = (Control)charisma_label.GetChild(0);
		info.Hide();
	}

	public void _on_damage_label_focus_entered()
	{
		Control info = (Control)damage_label.GetChild(0);
		info.Show();
	}

	public void _on_damage_label_focus_exited()
	{
		Control info = (Control)damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_resistance_label_focus_entered()
	{
		Control info = (Control)resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_resistance_label_focus_exited()
	{
		Control info = (Control)resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_recovery_label_focus_entered()
	{
		Control info = (Control)recovery_label.GetChild(0);
		info.Show();
	}

	public void _on_recovery_label_focus_exited()
	{
		Control info = (Control)recovery_label.GetChild(0);
		info.Hide();
	}


	public void _on_sheet_focus_entered()
	{
		Control info = (Control)sheet_label.GetChild(0);
		info.Show();
	}

	public void _on_sheet_focus_exited()
	{
		Control info = (Control)sheet_label.GetChild(0);
		info.Hide();
	}

	public void _on_mats_label_focus_entered()
	{
		Control info = (Control)mats_label.GetChild(0);
		info.Show();
	}

	public void _on_mats_label_focus_exited()
	{
		Control info = (Control)mats_label.GetChild(0);
		info.Hide();
	}

	public void _on_gold_focus_entered()
	{
		Control info = (Control)gold_label.GetChild(0);
		info.Show();
	}

	public void _on_gold_focus_exited()
	{
		Control info = (Control)gold_label.GetChild(0);
		info.Hide();
	}

	public void _on_trash_focus_entered()
	{	
		Control info = (Control)trash_label.GetChild(0);
		info.Show();
	}

	public void _on_trash_focus_exited()
	{
		Control info = (Control)trash_label.GetChild(0);
		info.Hide();
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

	

}
