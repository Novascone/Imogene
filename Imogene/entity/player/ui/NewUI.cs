using Godot;
using GodotPlugins.Game;
using System;

public partial class NewUI : Control
{
	[Export] public NewHUD hud;
	[Export] public NewInventory inventory;
	[Export] public NewAbilities abilities;
	[Export] public NewJournal journal;
	[Export] public NewCursor cursor;

	[Signal] public delegate void InventoryToggleEventHandler();
	[Signal] public delegate void CapturingInputEventHandler(bool capturing_input);

	public bool preventing_movement;
	public bool capturing_input;

	public Button hovered_button;

	
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


    private void OnHudPreventingInput(bool preventing_input)
    {
        if(preventing_input)
		{
			capturing_input = true;
		}
		else if (!preventing_input)
		{
			capturing_input = false;
		}
    }

    private void OnClearAbilityIcon(string ability_name_old, string ability_name_new)
    {
        abilities.binds.ClearAbility(ability_name_old, ability_name_new);
		hud.main.ClearAbility(ability_name_old, ability_name_new);
    }


    private void OnAbilityReassigned(string cross, string level, string bind, string ability_name, Texture2D icon)
    {
        abilities.binds.AssignAbility(cross, level, bind, ability_name, icon);
		hud.main.AssignAbility(cross, level, bind,ability_name, icon);
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

	public override void _GuiInput(InputEvent @event)
	{
		
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			GD.Print("event " + @event);
			if(CheckUIComponentOpen() && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.A)
			{
				GD.Print("event accepted from ui");
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadUp)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadDown)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadRight)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
			if(CheckUIComponentOpen()  && eventJoypadButton.ButtonIndex == JoyButton.DpadLeft)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
		}
		
	}



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(preventing_movement)
		{
			cursor.ControllerCursor();
		}
		
		if(Input.IsActionJustPressed("Inventory"))
		{
			preventing_movement = !preventing_movement;
			EmitSignal(nameof(CapturingInput), !preventing_movement);
			if(!CheckUIComponentOpen() && !cursor.Visible || CheckUIComponentOpen()  && cursor.Visible )
			{
				cursor.Visible = !cursor.Visible;
			}
			if(!abilities.Visible)
			{
				inventory.Visible = !inventory.Visible;
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

	public void AssignAbility(string cross, string level, string bind, string ability_name, Texture2D icon)
	{
		hud.main.AssignAbility(cross, level, bind, ability_name, icon);
		abilities.binds.AssignAbility(cross, level, bind, ability_name, icon);
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

    internal void HandleUpdatedStats(Player player)
    {
        hud.main.UpdateHUDStats(player);
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
