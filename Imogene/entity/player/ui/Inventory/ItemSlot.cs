using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ItemSlot : Button
{
	
	[Signal] public delegate void CursorHoveringEventHandler(ItemSlot item_button);
	[Signal] public delegate void CursorLeftEventHandler(ItemSlot item_button);
	[Signal] public delegate void ItemDroppedEventHandler(int from_slot_id, int to_slot_id);

	[Export] public TextureRect slot_icon;
	[Export] public int inventory_slot_id { get; set; } = -1;
	public bool slot_filled { get; set; } = false;
	public ItemData slot_data { get; set; }

	public void FillSlot(ItemData data)
	{
		GD.Print("Filling slot of id " + inventory_slot_id);
		slot_data = data;
		if(slot_data != null)
		{
			slot_filled = true;
			slot_icon.Texture = data.icon;
		}
		else
		{
			GD.Print("data is empty in " + inventory_slot_id);
			slot_filled = false;
			slot_icon.Texture = null;
			
		}
	}

    public override Variant _GetDragData(Vector2 atPosition)
    {
		GD.Print("getting drag data");
		if(slot_filled)
		{
			TextureRect preview = new TextureRect();
			preview.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			preview.Size = slot_icon.Size;
			preview.PivotOffset = slot_icon.Size / 2.0f;
			// preview.Rotation = 2.0f;
			preview.Texture = slot_icon.Texture;
			SetDragPreview(preview);

			return new Dictionary {{ "Type", "Item" },{ "ID", inventory_slot_id } };
		}
		else
		{
			return false;
		}
        
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		GD.Print("Checking can");
        return data.VariantType == Variant.Type.Dictionary && (string)data.AsGodotDictionary()["Type"] == "Item";
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        EmitSignal(nameof(ItemDropped), (int)data.AsGodotDictionary()["ID"], inventory_slot_id);
    }

    public void _on_area_2d_area_entered(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}

	// public void _on_button_down()
	// {
	// 	AcceptEvent();
	// }

	

}
