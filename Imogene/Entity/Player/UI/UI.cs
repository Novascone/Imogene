using Godot;
using GodotPlugins.Game;
using System;
using System.Linq;

public partial class UI : Control
{
	[Export] public HUD HUD { get; set; }
	[Export] public Inventory Inventory { get; set; }
	[Export] public Abilities Abilities { get; set; }
	[Export] public Journal Journal { get; set; }
	[Export] public UICursor Cursor { get; set; }

	public bool UIPreventingMovement { get; set; } = false;
	public bool UICapturingInput { get; set; } = false;
	public Button HoveredButton  { get; set; } = null;

	[Signal] public delegate void InventoryToggleEventHandler();
	[Signal] public delegate void CapturingInputEventHandler(bool capturingInput);

	public override void _GuiInput(InputEvent @event)
	{
		
		if(@event is InputEventJoypadButton eventJoypadButton)
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
		Inventory.main.bottom_buttons.abilities.ButtonDown += OnAbilitiesButtonDown;
		Abilities.AbilitiesClosed += OnAbilitiesClosed;
		Abilities.Categories.AbilityReassigned += OnAbilityReassigned;
		Abilities.Categories.ClearAbilityIcon += OnClearAbilityIcon;
		HUD.HUDPreventingInput += OnHudPreventingInput;
		Cursor.ItemDroppedIntoSlot += Inventory.main.HandleItemDroppedInto;
		Cursor.ItemDroppedOutSide += Inventory.main.HandleItemDroppedOutside;
		foreach(ItemSlot itemSlot in Inventory.main.items.GetChildren().Cast<ItemSlot>())
		{
			itemSlot.CursorHovering += Cursor.OnCursorHovering;
			itemSlot.CursorLeft += Cursor.OnCursorLeft;
			itemSlot.ItemEquipped += Inventory.main.OnItemEquipped;
			itemSlot.inventory_slot_id -= 1;
			Inventory.main.inventory_slots.Add(itemSlot);
		}
		
	}

	 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(UIPreventingMovement)
		{
			Cursor.ControllerCursor();
		}
		
		if(Input.IsActionJustPressed("Inventory"))
		{
			UIPreventingMovement = !UIPreventingMovement;
			EmitSignal(nameof(CapturingInput), UIPreventingMovement);
			if(!CheckUIComponentOpen() && !Cursor.Visible || CheckUIComponentOpen()  && Cursor.Visible )
			{
				Cursor.Visible = !Cursor.Visible;
			}
			if(!Abilities.Visible)
			{
				Inventory.Visible = !Inventory.Visible;
				if(Inventory.depth_sheet.Visible)
				{
					Inventory.depth_sheet.Visible = !Inventory.depth_sheet.Visible;
				}
			}
			if(Abilities.Visible)
			{
				Abilities.ResetPage();
				Abilities.Visible = !Abilities.Visible;
			}
			
			
			EmitSignal(nameof(InventoryToggle));
		}

		if(UIPreventingMovement)
		{
			if(Input.IsActionJustPressed("CursorUp") || Input.IsActionJustPressed("CursorDown") || Input.IsActionJustPressed("CursorLeft") || Input.IsActionJustPressed("CursorRight"))
			{
				Cursor.Show();
			}
		}
		if(Input.IsActionJustPressed("D-PadUp") || Input.IsActionJustPressed("D-PadDown") || Input.IsActionJustPressed("D-PadLeft") || Input.IsActionJustPressed("D-PadRight"))
		{
			Cursor.Hide();
		}
		
	}

    private void OnHudPreventingInput(bool preventingInput)
    {
        if(preventingInput)
		{
			UICapturingInput = true;
		}
		else if (!preventingInput)
		{
			UICapturingInput = false;
		}
    }

    private void OnClearAbilityIcon(string abilityNameOld, string abilityNameNew)
    {
        Abilities.Binds.ClearAbility(abilityNameOld, abilityNameNew);
		HUD.Main.ClearAbility(abilityNameOld, abilityNameNew);
    }


    private void OnAbilityReassigned(Ability.Cross cross, Ability.Tier tier, string bind, string abilityName, Texture2D icon)
    {
        Abilities.Binds.AssignAbility(cross, tier, bind, abilityName, icon);
		HUD.Main.AssignAbility(cross, tier, bind,abilityName, icon);
    }

    private void OnAbilitiesClosed()
    {
        EmitSignal(nameof(InventoryToggle));
		UIPreventingMovement = false;
		if(Cursor.Visible)
		
		Cursor.Visible = !Cursor.Visible;
    }

    private void OnAbilitiesButtonDown()
    {
        Abilities.Show();
		Inventory.Hide();
		UIPreventingMovement = true;
    }



	public void AssignAbility(Ability.Cross cross, Ability.Tier tier, string bind, string abilityName, Texture2D icon)
	{
		HUD.Main.AssignAbility(cross, tier, bind, abilityName, icon);
		Abilities.Binds.AssignAbility(cross, tier, bind, abilityName, icon);
	}

	public void SwitchCrosses(string cross)
	{
		HUD.Main.SwitchCrosses(cross);
	}

	public void UIElementOpen()
	{
		
		foreach(Control control in GetChildren().Cast<Control>())
		{
			if(control.IsInGroup("prevents_movement"))
			{
				GD.Print("checking ui prevents movement");
				if(control.Visible)
				{
					GD.Print("UI preventing movement ");
					UIPreventingMovement = true;
				}
				else if(!UIPreventingMovement)
				{
					GD.Print("UI not preventing movement ");
					UIPreventingMovement = false;
				}
			}
		}
	}

    internal void HandleUpdatedStats(Player player)
    {
        HUD.Main.UpdateHUDStats(player);
    }

	public bool CheckUIComponentOpen()
	{
		foreach(Control control in GetChildren().Cast<Control>())
		{
			if(control.Name != "Cursor" && control != HUD)
			{
				if(control.Visible)
				{
					return true;
				}
			}
			else if(control == HUD)
			{
				if(HUD.InteractBar.Visible)
				{
					return true;
				}
			}
		}
		return false;
	}

	
}
