using Godot;
using System;

public partial class UICursor : Sprite2D
{
	[Export] public ItemSlot item_preview;
	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 30.0f;
	public ItemSlot hover_over_item_button;
	public ItemSlot hover_over_button;

	[Signal] public delegate void ItemDroppedIntoSlotEventHandler(int from_slot_id, int to_slot_id);
	[Signal] public delegate void ItemDroppedOutSideEventHandler(int slot_id, ItemData data);

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			mouse_pos = mouseMotion.Position;
		}
		if (@event.IsActionPressed("Interact"))
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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

    internal void OnCursorHovering(ItemSlot item_slot)
    {
        hover_over_button = item_slot;
    }

    internal void OnCursorLeft(ItemSlot item_slot)
    {
        hover_over_button = null;
    }
}
