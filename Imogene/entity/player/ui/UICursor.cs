using Godot;
using System;

public partial class UICursor : Sprite2D
{
	[Export] public ItemSlot item_preview { get; set; }
	private Vector2 mouse_pos { get; set; } = Vector2.Zero;
	private float mouse_max_speed { get; set; } = 30.0f;
	public ItemSlot hover_over_item_button { get; set; } = null;
	public ItemSlot hover_over_button { get; set; } = null;

	[Signal] public delegate void ItemDroppedIntoSlotEventHandler(int from_slot_id_, int to_slot_id_);
	[Signal] public delegate void ItemDroppedOutSideEventHandler(int slot_id_, ItemData data_);

	public override void _Input(InputEvent @event_)
	{
		if (@event_ is InputEventMouseMotion mouseMotion)
		{
			mouse_pos = mouseMotion.Position;
		}
		if (@event_.IsActionPressed("Interact"))
		{

			if (hover_over_button != null)
			{
				if (item_preview.slot_data == null)
				{
					if(hover_over_button.slot_data != null)
					{
						item_preview.Icon = hover_over_button.slot_icon.Texture;
						item_preview.slot_data = hover_over_button.slot_data;
						item_preview.inventory_slot_id = hover_over_button.inventory_slot_id;
						item_preview.Show();
					}
				}
				else
				{
	
					EmitSignal(nameof(ItemDroppedIntoSlot), item_preview.inventory_slot_id, hover_over_button.inventory_slot_id);
					item_preview.Icon = null;
					item_preview.slot_data = null;
					item_preview.inventory_slot_id = -1;
					item_preview.Hide();
				}
			}
			else
			{
				if (item_preview.slot_data != null)
				{
					EmitSignal(nameof(ItemDroppedOutSide), item_preview.inventory_slot_id, item_preview.slot_data);
					item_preview.Icon = null;
					item_preview.slot_data = null;
					item_preview.inventory_slot_id = -1;
					item_preview.Hide();
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
		if (mouse_direction != Vector2.Zero)
		{
			GetViewport().WarpMouse(mouse_pos + mouse_direction * Mathf.Lerp(0, mouse_max_speed, 0.1f));
		}
		Position = GetViewport().GetMousePosition();
	}

    internal void OnCursorHovering(ItemSlot item_slot_)
    {
        hover_over_button = item_slot_;
    }

    internal void OnCursorLeft(ItemSlot item_slot_)
    {
        hover_over_button = null;
    }
}
