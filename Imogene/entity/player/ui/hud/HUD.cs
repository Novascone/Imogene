using Godot;
using GodotPlugins.Game;
using System;

public partial class HUD : Control

{

	[Export] public EnemyHealth enemy_health;
	[Export] public MainHUD main;
	[Export] public InteractBar interact_bar;
	[Export] public TopRightHUD top_right;

	[Signal] public delegate void HUDPreventingInputEventHandler(bool preventing_input);
	[Signal] public delegate void AcceptHUDInputEventHandler(Node3D interacting_object);

	public JoyButton interact_button = JoyButton.A;

	public bool in_interact_area;
	private Node3D object_interacting_with;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event) {
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(in_interact_area && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == interact_button)
			{
				if(object_interacting_with is InteractableObject)
				{
					interact_bar.interact_inventory.Visible = !interact_bar.interact_inventory.Visible;
				}
				EmitSignal(nameof(AcceptHUDInput), object_interacting_with);
				AcceptEvent();
				
			}
			
		}
	}
	

	public void SubscribeToInteractSignals(Player player)
	{
		player.areas.interact.AreaEntered += OnInteractAreaEntered;
		player.areas.interact.AreaExited += OnInteractAreaExited;
		player.areas.pickup_items.BodyEntered += OnPickUpAreaEntered;
		player.areas.pickup_items.BodyExited += (body) => OnPickUpAreaExited(body, player);
		player.systems.interact_system.SwitchToNextNearestItem += HandleSwitchToNextItem;
	}

    private void HandleSwitchToNextItem(InteractableItem item, bool last_item)
    {
		
        object_interacting_with = item;
		if(!last_item)
		{
			if(item.interact_to_pick_up)
			{
				GD.Print("received switch item signal switching to " + item.Name );
				in_interact_area = true;
				interact_bar.button.Text = interact_button.ToString() + ":" + " Pick Up";
				interact_bar.interact_object.Text = item.Name;
				interact_bar.Show();
				EmitSignal(nameof(HUDPreventingInput),true);
			}
		}
		else
		{
			in_interact_area = false;
			interact_bar.button.Text = interact_button.ToString() + ";";
			interact_bar.interact_inventory.Hide();
			interact_bar.Hide(); 
			EmitSignal(nameof(HUDPreventingInput),false);
		}
    }

    private void OnPickUpAreaExited(Node3D body, Player player)
    {
		
		if(body is InteractableItem item)
		{
			// GD.Print("item exited " + item.Name);
			// GD.Print("item interacting with " + object_interacting_with.Name);
			
			if(item == object_interacting_with || item == null)
			{
				in_interact_area = false;
				interact_bar.button.Text = interact_button.ToString() + ";";
				interact_bar.interact_inventory.Hide();
				interact_bar.Hide(); 
				EmitSignal(nameof(HUDPreventingInput),false);
				object_interacting_with = null;
			}
			
		}
    }

    private void OnPickUpAreaEntered(Node3D body)
    {
		if(body is InteractableItem item)
		{
			
			if(item.interact_to_pick_up)
			{
				object_interacting_with = item;
				in_interact_area = true;
				interact_bar.button.Text = interact_button.ToString() + ":" + " Pick Up";
				interact_bar.interact_object.Text = body.Name;
				interact_bar.Show();
				EmitSignal(nameof(HUDPreventingInput),true);
			}
		}
    }

    public void SubscribeToTargetingSignals(Player player)
	{
		player.systems.targeting_system.ShowSoftTargetIcon += HandleShowSoftTargetIcon;
		player.systems.targeting_system.HideSoftTargetIcon += HandleHideSoftTargetIcon;
		player.systems.targeting_system.EnemyTargeted += HandleEnemyTargeted;
		player.systems.targeting_system.EnemyUntargeted += HandleEnemyUntargeted;
		player.systems.targeting_system.BrightenSoftTargetHUD += HandleBrightenSoftTargetHUD;
		player.systems.targeting_system.DimSoftTargetHUD += HandleDimSoftTargetHUD;
	}

    private void OnInteractAreaExited(Area3D area)
    {
		if(area is InteractableObject interactable_object)
		{	
			
			in_interact_area = false;
			interact_bar.button.Text = interact_button.ToString() + ";";
			interact_bar.interact_inventory.Hide();
			interact_bar.Hide(); 
			EmitSignal(nameof(HUDPreventingInput),false);
			if(interactable_object == object_interacting_with)
			{
				object_interacting_with = null;
			}
			
		}
		
    }

    private void OnInteractAreaEntered(Area3D area)
    {
		if(area is InteractableObject interactable_object)
		{
			in_interact_area = true;
			interact_bar.button.Text = interact_button.ToString() + ":" + " Interact";
			interact_bar.interact_object.Text = interactable_object.Name;
			interact_bar.Show();
			EmitSignal(nameof(HUDPreventingInput),true);
			object_interacting_with = interactable_object;
		}
		
    }

    private void HandleDimSoftTargetHUD()
    {
        top_right.DimSoftTargetIndicator();
    }

    private void HandleBrightenSoftTargetHUD()
    {
        top_right.BrightenSoftTargetIndicator();
    }

    private void HandleEnemyUntargeted()
    {
        enemy_health.EnemyUntargeted();
    }

    private void HandleEnemyTargeted(Enemy enemy)
    {
        enemy_health.EnemyTargeted(enemy);
    }

    private void HandleHideSoftTargetIcon(Enemy enemy)
    {
        enemy_health.HideSoftTargetIcon(enemy);
    }

    private void HandleShowSoftTargetIcon(Enemy enemy)
    {
        enemy_health.ShowSoftTargetIcon(enemy);
    }
}
