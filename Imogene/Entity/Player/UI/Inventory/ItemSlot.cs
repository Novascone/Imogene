using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ItemSlot : Button
{

	[Export] public TextureRect slot_icon { get; set; }
	[Export] public Area2D interact_area { get; set; }
	[Export] public Panel equipped_highlight { get; set; }
	[Export] public int inventory_slot_id { get; set; } = -1;
	public bool slot_filled { get; set; } = false;
	public ItemData slot_data { get; set; } = null;

	[Signal] public delegate void CursorHoveringEventHandler(ItemSlot item_button_);
	[Signal] public delegate void CursorLeftEventHandler(ItemSlot item_button_);
	[Signal] public delegate void ItemEquippedEventHandler(int id_);
	[Signal] public delegate void ItemDroppedEventHandler(int from_slot_id_, int to_slot_id_);
	

    public override void _GuiInput(InputEvent @event_)
    {
        if(@event_.IsActionPressed("InteractMenu"))
		{
			EmitSignal(nameof(ItemEquipped), inventory_slot_id);
		}
    }

    public void FillSlot(ItemData data_, bool equipped_)
	{
		slot_data = data_;
		equipped_highlight.Visible = equipped_;
		if(slot_data != null)
		{
			slot_filled = true;
			slot_icon.Texture = data_.Icon;
		}
		else
		{
			slot_filled = false;
			slot_icon.Texture = null;
		}
	}


    public void _on_area_2d_area_entered(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			GrabFocus();
			EmitSignal(nameof(CursorHovering), this);
		}
	}

	public void _on_area_2d_area_exited(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			ReleaseFocus();
			EmitSignal(nameof(CursorLeft), this);
		}
	}

}
