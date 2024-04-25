using Godot;
using System;

public partial class UI : CanvasLayer
{
	private Sprite2D cursor;
	private Vector2 mouse_pos = Vector2.Zero;
	private float mouse_max_speed = 10.0f;
	private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	// Called when the node enters the scene tree for the first time.
	private int health;
	private int resource;
	private CustomSignals _customSignals; // Instance of CustomSignals
	private Inventory inventory;
	private bool inventory_open;
	public override void _Ready()
	{
		cursor = GetNode<Sprite2D>("Inventory/Cursor");
		mouse_pos = GetWindow().Size / 4;
		
		health_icon = GetNode<TextureProgressBar>("main_UI/HBoxContainer/PanelHealthContainer/HealthContainer/HealthIcon");
		resource_icon = GetNode<TextureProgressBar>("main_UI/HBoxContainer/PanelResourceContainer/ResourceContainer/ResourceIcon");

		inventory = GetNode<Inventory>("Inventory");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		_customSignals.UIResourceUpdate += HandleUIResourceUpdate;
		_customSignals.UIHealth += HandleUIHealth;
		_customSignals.UIResource += HandleUIResource;
		_customSignals.Interact += HandleInteract;
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
		if(inventory_open)
		{
			ControllerCursor();
		}
		

		UpdateHealth();
		UpdateResource();

		if(Input.IsActionJustPressed("Inventory"))
		{
			if(!inventory_open)
			{
				inventory_open = true;
			}
			else
			{
				inventory_open = false;
			}
			inventory.Visible = !inventory.Visible;
			// if(inventory.Visible)
			// {
			// 	Input.MouseMode = Input.MouseModeEnum.Visible;
			// }
			// else
			// {
			// 	Input.MouseMode = Input.MouseModeEnum.Captured;
			// }
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

	private void HandleInteract(Area3D area, bool in_interact_area)
    {
		if(in_interact_area)
		{
			GD.Print("Interact with ", area.GetParent().Name);
		}
		else
		{
			GD.Print("left ", area.GetParent().Name , " interact area");
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

	public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			mouse_pos = mouseMotion.Position;
		}
    }
}
