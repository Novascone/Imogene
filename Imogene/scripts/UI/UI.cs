using Godot;
using System;

public partial class UI : CanvasLayer
{

	private TextureProgressBar health_icon; // Health icon in the UI that displays how much health the player has
	private TextureProgressBar resource_icon; // Resource icon in the UI that displays how much resource (mana, fury, etc) the player has
	// Called when the node enters the scene tree for the first time.
	private int health;
	private int resource;
	private CustomSignals _customSignals; // Instance of CustomSignals
	public override void _Ready()
	{
		health_icon = GetNode<TextureProgressBar>("HBoxContainer/PanelHealthContainer/HealthContainer/HealthIcon");
		resource_icon = GetNode<TextureProgressBar>("HBoxContainer/PanelResourceContainer/ResourceContainer/ResourceIcon");

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
		UpdateHealth();
		UpdateResource();
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
}
