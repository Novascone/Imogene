using Godot;
using GodotPlugins.Game;
using System;

public partial class HUD : Control

{

	[Export] public EnemyHealth enemy_health  { get; set; }
	[Export] public MainHUD main { get; set; }
	[Export] public InteractBar interact_bar { get; set; }
	[Export] public TopRightHUD top_right { get; set; }
	public JoyButton interact_button { get; set; } = JoyButton.A;
	public bool in_interact_area  { get; set; } = false;
	private Node3D object_interacting_with  { get; set; } = null;

	[Signal] public delegate void HUDPreventingInputEventHandler(bool preventing_input_);
	[Signal] public delegate void AcceptHUDInputEventHandler(Node3D interacting_object_);

	public override void _Input(InputEvent @event_) {
		if(@event_ is InputEventJoypadButton eventJoypadButton)
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
	
	public void Subscribe(Player player_)
	{
		player_.PlayerAreas.interact.AreaEntered += OnInteractAreaEntered;
		player_.PlayerAreas.interact.AreaExited += OnInteractAreaExited;
		player_.PlayerAreas.pickup_items.BodyEntered += OnPickUpAreaEntered;
		player_.PlayerAreas.pickup_items.BodyExited += (body) => OnPickUpAreaExited(body, player_);
		player_.PlayerSystems.interact_system.SwitchToNextNearestItem += HandleSwitchToNextItem;
		player_.PlayerSystems.targeting_system.ShowSoftTargetIcon += HandleShowSoftTargetIcon;
		player_.PlayerSystems.targeting_system.HideSoftTargetIcon += HandleHideSoftTargetIcon;
		player_.PlayerSystems.targeting_system.EnemyTargeted += HandleEnemyTargeted;
		player_.PlayerSystems.targeting_system.EnemyUntargeted += HandleEnemyUntargeted;
		player_.PlayerSystems.targeting_system.BrightenSoftTargetHUD += HandleBrightenSoftTargetHUD;
		player_.PlayerSystems.targeting_system.DimSoftTargetHUD += HandleDimSoftTargetHUD;
		enemy_health.Subscribe(player_);
	}

    private void HandleSwitchToNextItem(InteractableItem item_, bool last_item_)
    {
		
        object_interacting_with = item_;
		if(!last_item_)
		{
			if(item_.interact_to_pick_up)
			{
				in_interact_area = true;
				interact_bar.button.Text = interact_button.ToString() + ":" + " Pick Up";
				interact_bar.interact_object.Text = item_.Name;
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

    private void OnPickUpAreaExited(Node3D body_, Player player_)
    {
		
		if(body_ is InteractableItem _item)
		{

			if(_item == object_interacting_with || _item == null)
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

    private void OnPickUpAreaEntered(Node3D body_)
    {
		if(body_ is InteractableItem _item)
		{
			
			if(_item.interact_to_pick_up)
			{
				object_interacting_with = _item;
				in_interact_area = true;
				interact_bar.button.Text = interact_button.ToString() + ":" + " Pick Up";
				interact_bar.interact_object.Text = body_.Name;
				interact_bar.Show();
				EmitSignal(nameof(HUDPreventingInput),true);
			}
		}
    }

    private void OnInteractAreaExited(Area3D area_)
    {
		if(area_ is InteractableObject _interactable_object)
		{	
			
			in_interact_area = false;
			interact_bar.button.Text = interact_button.ToString() + ";";
			interact_bar.interact_inventory.Hide();
			interact_bar.Hide(); 
			EmitSignal(nameof(HUDPreventingInput),false);
			if(_interactable_object == object_interacting_with)
			{
				object_interacting_with = null;
			}
			
		}
		
    }

    private void OnInteractAreaEntered(Area3D area_)
    {
		if(area_ is InteractableObject _interactable_object)
		{
			in_interact_area = true;
			interact_bar.button.Text = interact_button.ToString() + ":" + " Interact";
			interact_bar.interact_object.Text = _interactable_object.Name;
			interact_bar.Show();
			EmitSignal(nameof(HUDPreventingInput),true);
			object_interacting_with = _interactable_object;
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

    private void HandleEnemyTargeted(Enemy enemy_)
    {
		GD.Print("Enemy targeted");
        enemy_health.EnemyTargeted(enemy_);
    }

    private void HandleHideSoftTargetIcon(Enemy enemy_)
    {
        EnemyHealth.HideSoftTargetIcon(enemy_);
    }

    private void HandleShowSoftTargetIcon(Enemy enemy_)
    {
        EnemyHealth.ShowSoftTargetIcon(enemy_);
    }
}
