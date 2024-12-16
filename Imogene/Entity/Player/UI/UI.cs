using Godot;
using GodotPlugins.Game;
using System;

public partial class UI : Control
{
	[Export] public HUD hud { get; set; }
	[Export] public Inventory inventory { get; set; }
	[Export] public Abilities abilities { get; set; }
	[Export] public Journal journal { get; set; }
	[Export] public UICursor cursor { get; set; }

	public bool preventing_movement { get; set; } = false;
	public bool capturing_input { get; set; } = false;
	public Button hovered_button  { get; set; } = null;

	[Signal] public delegate void InventoryToggleEventHandler();
	[Signal] public delegate void CapturingInputEventHandler(bool capturing_input_);

	public override void _GuiInput(InputEvent @event_)
	{
		
		if(@event_ is InputEventJoypadButton eventJoypadButton)
		{
			if(CheckUIComponentOpen() && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.A)
			{
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadUp)
			{
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadDown)
			{
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadRight)
			{
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadLeft)
			{
				AcceptEvent();
			}
		}
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		inventory.main.bottom_buttons.abilities.ButtonDown += OnAbilitiesButtonDown;
		abilities.AbilitiesClosed += OnAbilitiesClosed;
		abilities.categories.AbilityReassigned += OnAbilityReassigned;
		abilities.categories.ClearAbilityIcon += OnClearAbilityIcon;
		hud.HUDPreventingInput += OnHudPreventingInput;
		cursor.ItemDroppedIntoSlot += inventory.main.HandleItemDroppedInto;
		cursor.ItemDroppedOutSide += inventory.main.HandleItemDroppedOutside;
		foreach(ItemSlot item_slot in inventory.main.items.GetChildren())
		{
			item_slot.CursorHovering += cursor.OnCursorHovering;
			item_slot.CursorLeft += cursor.OnCursorLeft;
			item_slot.ItemEquipped += inventory.main.OnItemEquipped;
			item_slot.inventory_slot_id -= 1;
			inventory.main.inventory_slots.Add(item_slot);
		}
		
	}

	 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta_)
	{
		if(preventing_movement)
		{
			cursor.ControllerCursor();
		}
		
		if(Input.IsActionJustPressed("Inventory"))
		{
			preventing_movement = !preventing_movement;
			EmitSignal(nameof(CapturingInput), preventing_movement);
			if(!CheckUIComponentOpen() && !cursor.Visible || CheckUIComponentOpen()  && cursor.Visible )
			{
				cursor.Visible = !cursor.Visible;
			}
			if(!abilities.Visible)
			{
				inventory.Visible = !inventory.Visible;
				if(inventory.depth_sheet.Visible)
				{
					inventory.depth_sheet.Visible = !inventory.depth_sheet.Visible;
				}
			}
			if(abilities.Visible)
			{
				abilities.ResetPage();
				abilities.Visible = !abilities.Visible;
			}
			
			
			EmitSignal(nameof(InventoryToggle));
		}

		if(preventing_movement)
		{
			if(Input.IsActionJustPressed("CursorUp") || Input.IsActionJustPressed("CursorDown") || Input.IsActionJustPressed("CursorLeft") || Input.IsActionJustPressed("CursorRight"))
			{
				cursor.Show();
			}
		}
		if(Input.IsActionJustPressed("D-PadUp") || Input.IsActionJustPressed("D-PadDown") || Input.IsActionJustPressed("D-PadLeft") || Input.IsActionJustPressed("D-PadRight"))
		{
			cursor.Hide();
		}
		
	}

    private void OnHudPreventingInput(bool preventing_input_)
    {
        if(preventing_input_)
		{
			capturing_input = true;
		}
		else if (!preventing_input_)
		{
			capturing_input = false;
		}
    }

    private void OnClearAbilityIcon(string ability_name_old_, string ability_name_new_)
    {
        abilities.binds.ClearAbility(ability_name_old_, ability_name_new_);
		hud.main.ClearAbility(ability_name_old_, ability_name_new_);
    }


    private void OnAbilityReassigned(Ability.Cross cross_, Ability.Tier tier_, string bind_, string ability_name_, Texture2D icon_)
    {
        abilities.binds.AssignAbility(cross_, tier_, bind_, ability_name_, icon_);
		hud.main.AssignAbility(cross_, tier_, bind_,ability_name_, icon_);
    }

    private void OnAbilitiesClosed()
    {
        EmitSignal(nameof(InventoryToggle));
		preventing_movement = false;
		if(cursor.Visible)
		
		cursor.Visible = !cursor.Visible;
    }

    private void OnAbilitiesButtonDown()
    {
        abilities.Show();
		inventory.Hide();
		preventing_movement = true;
    }



	public void AssignAbility(Ability.Cross cross_, Ability.Tier tier_, string bind, string ability_name, Texture2D icon)
	{
		hud.main.AssignAbility(cross_, tier_, bind, ability_name, icon);
		abilities.binds.AssignAbility(cross_, tier_, bind, ability_name, icon);
	}

	public void SwitchCrosses(string cross)
	{
		hud.main.SwitchCrosses(cross);
	}

	public void UIElementOpen()
	{
		
		foreach(Control control in GetChildren())
		{
			if(control.IsInGroup("prevents_movement"))
			{
				GD.Print("checking ui prevents movement");
				if(control.Visible)
				{
					GD.Print("UI preventing movement ");
					preventing_movement = true;
				}
				else if(!preventing_movement)
				{
					GD.Print("UI not preventing movement ");
					preventing_movement = false;
				}
			}
		}
	}

    internal void HandleUpdatedStats(Player player_)
    {
        hud.main.UpdateHUDStats(player_);
    }

	public bool CheckUIComponentOpen()
	{
		foreach(Control control in GetChildren())
		{
			if(control.Name != "Cursor" && control != hud)
			{
				if(control.Visible)
				{
					return true;
				}
			}
			else if(control == hud)
			{
				if(hud.interact_bar.Visible)
				{
					return true;
				}
			}
		}
		return false;
	}

	
}
