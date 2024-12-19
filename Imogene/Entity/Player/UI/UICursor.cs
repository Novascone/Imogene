using Godot;
using System;

public partial class UICursor : Sprite2D
{
	[Export] public ItemSlot ItemPreview { get; set; }
	private Vector2 MousePOS { get; set; } = Vector2.Zero;
	private float MouseMaxSpeed { get; set; } = 30.0f;
	public ItemSlot HoverOverItemButton { get; set; } = null;
	public ItemSlot HoverOverButton { get; set; } = null;

	[Signal] public delegate void ItemDroppedIntoSlotEventHandler(int fromSlotID, int toSlotID);
	[Signal] public delegate void ItemDroppedOutSideEventHandler(int slotID, ItemData data);

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			MousePOS = mouseMotion.Position;
		}
		if (@event.IsActionPressed("Interact"))
		{

			if (HoverOverButton != null)
			{
				if (ItemPreview.slot_data == null)
				{
					if(HoverOverButton.slot_data != null)
					{
						ItemPreview.Icon = HoverOverButton.slot_icon.Texture;
						ItemPreview.slot_data = HoverOverButton.slot_data;
						ItemPreview.inventory_slot_id = HoverOverButton.inventory_slot_id;
						ItemPreview.Show();
					}
				}
				else
				{
	
					EmitSignal(nameof(ItemDroppedIntoSlot), ItemPreview.inventory_slot_id, HoverOverButton.inventory_slot_id);
					ItemPreview.Icon = null;
					ItemPreview.slot_data = null;
					ItemPreview.inventory_slot_id = -1;
					ItemPreview.Hide();
				}
			}
			else
			{
				if (ItemPreview.slot_data != null)
				{
					EmitSignal(nameof(ItemDroppedOutSide), ItemPreview.inventory_slot_id, ItemPreview.slot_data);
					ItemPreview.Icon = null;
					ItemPreview.slot_data = null;
					ItemPreview.inventory_slot_id = -1;
					ItemPreview.Hide();
				}
			}
		}
	}

	public void ControllerCursor() // Control the cursor with the joysticks
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		Vector2 mouseDirection = Vector2.Zero;

		if (Input.IsActionPressed("CursorLeft"))
		{
			mouseDirection.X -= 1.0f;
		}
		if (Input.IsActionPressed("CursorRight"))
		{
			mouseDirection.X += 1.0f;
		}
		if (Input.IsActionPressed("CursorUp"))
		{
			mouseDirection.Y -= 1.0f;
		}
		if (Input.IsActionPressed("CursorDown"))
		{
			mouseDirection.Y += 1.0f;
		}
		if (mouseDirection != Vector2.Zero)
		{
			GetViewport().WarpMouse(MousePOS + mouseDirection * Mathf.Lerp(0, MouseMaxSpeed, 0.1f));
		}
		Position = GetViewport().GetMousePosition();
	}

    internal void OnCursorHovering(ItemSlot itemSlot)
    {
        HoverOverButton = itemSlot;
    }

    internal void OnCursorLeft(ItemSlot itemSlot)
    {
        HoverOverButton = null;
    }
}
