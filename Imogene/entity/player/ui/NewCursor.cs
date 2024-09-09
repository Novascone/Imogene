using Godot;
using System;

public partial class NewCursor : Sprite2D
{
	[Export] public ItemButton item_button;
	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 30.0f;
	public ItemButton hover_over_item_button;
	public Button hover_over_button;

	public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			mouse_pos = mouseMotion.Position;
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
		if(mouse_direction != Vector2.Zero)
		{
			GetViewport().WarpMouse(mouse_pos + mouse_direction * Mathf.Lerp(0, mouse_max_speed, 0.1f));
		}
		Position = GetViewport().GetMousePosition();
	}
	
}
