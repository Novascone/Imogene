using Godot;
using GodotPlugins.Game;
using System;

public partial class HUD : Control

{

	[Export] public EnemyHealth EnemyHealth  { get; set; }
	[Export] public MainHUD Main { get; set; }
	[Export] public InteractBar InteractBar { get; set; }
	[Export] public TopRightHUD TopRight { get; set; }
	public JoyButton InteractButton { get; set; } = JoyButton.A;
	public bool InInteractArea  { get; set; } = false;
	private Node3D ObjectInteractingWith  { get; set; } = null;

	[Signal] public delegate void HUDPreventingInputEventHandler(bool preventingInput);
	[Signal] public delegate void AcceptHUDInputEventHandler(Node3D interactingObject);

	public override void _Input(InputEvent @event) {
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(InInteractArea && eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == InteractButton)
			{
				if(ObjectInteractingWith is InteractableObject)
				{
					InteractBar.InteractInventory.Visible = !InteractBar.InteractInventory.Visible;
				}
				EmitSignal(nameof(AcceptHUDInput), ObjectInteractingWith);
				AcceptEvent();
				
			}
		}
	}
	
	public void Subscribe(Player player)
	{
		player.PlayerAreas.Interact.AreaEntered += OnInteractAreaEntered;
		player.PlayerAreas.Interact.AreaExited += OnInteractAreaExited;
		player.PlayerAreas.PickUpItems.BodyEntered += OnPickUpAreaEntered;
		player.PlayerAreas.PickUpItems.BodyExited += (body) => OnPickUpAreaExited(body, player);
		player.PlayerSystems.InteractSystem.SwitchToNextNearestItem += HandleSwitchToNextItem;
		player.PlayerSystems.TargetingSystem.ShowSoftTargetIcon += HandleShowSoftTargetIcon;
		player.PlayerSystems.TargetingSystem.HideSoftTargetIcon += HandleHideSoftTargetIcon;
		player.PlayerSystems.TargetingSystem.EnemyTargeted += HandleEnemyTargeted;
		player.PlayerSystems.TargetingSystem.EnemyUntargeted += HandleEnemyUntargeted;
		player.PlayerSystems.TargetingSystem.BrightenSoftTargetHUD += HandleBrightenSoftTargetHUD;
		player.PlayerSystems.TargetingSystem.DimSoftTargetHUD += HandleDimSoftTargetHUD;
		EnemyHealth.Subscribe(player);
	}

    private void HandleSwitchToNextItem(InteractableItem item, bool lastItem)
    {
		
        ObjectInteractingWith = item;
		if(!lastItem)
		{
			if(item.InteractToPickUp)
			{
				InInteractArea = true;
				InteractBar.Button.Text = InteractButton.ToString() + ":" + " Pick Up";
				InteractBar.InteractObject.Text = item.Name;
				InteractBar.Show();
				EmitSignal(nameof(HUDPreventingInput),true);
			}
		}
		else
		{
			InInteractArea = false;
			InteractBar.Button.Text = InteractButton.ToString() + ";";
			InteractBar.InteractInventory.Hide();
			InteractBar.Hide(); 
			EmitSignal(nameof(HUDPreventingInput),false);
		}
    }

    private void OnPickUpAreaExited(Node3D body, Player player)
    {
		
		if(body is InteractableItem item)
		{

			if(item == ObjectInteractingWith || item == null)
			{
				InInteractArea = false;
				InteractBar.Button.Text = InteractButton.ToString() + ";";
				InteractBar.InteractInventory.Hide();
				InteractBar.Hide(); 
				EmitSignal(nameof(HUDPreventingInput),false);
				ObjectInteractingWith = null;
			}
			
		}
    }

    private void OnPickUpAreaEntered(Node3D body)
    {
		if(body is InteractableItem item)
		{
			
			if(item.InteractToPickUp)
			{
				ObjectInteractingWith = item;
				InInteractArea = true;
				InteractBar.Button.Text = InteractButton.ToString() + ":" + " Pick Up";
				InteractBar.InteractObject.Text = body.Name;
				InteractBar.Show();
				EmitSignal(nameof(HUDPreventingInput),true);
			}
		}
    }

    private void OnInteractAreaExited(Area3D area)
    {
		if(area is InteractableObject interactableObject)
		{	
			
			InInteractArea = false;
			InteractBar.Button.Text = InteractButton.ToString() + ";";
			InteractBar.InteractInventory.Hide();
			InteractBar.Hide(); 
			EmitSignal(nameof(HUDPreventingInput),false);
			if(interactableObject == ObjectInteractingWith)
			{
				ObjectInteractingWith = null;
			}
			
		}
		
    }

    private void OnInteractAreaEntered(Area3D area)
    {
		if(area is InteractableObject interactableObject)
		{
			InInteractArea = true;
			InteractBar.Button.Text = InteractButton.ToString() + ":" + " Interact";
			InteractBar.InteractObject.Text = interactableObject.Name;
			InteractBar.Show();
			EmitSignal(nameof(HUDPreventingInput),true);
			ObjectInteractingWith = interactableObject;
		}
		
    }

    private void HandleDimSoftTargetHUD()
    {
        TopRight.DimSoftTargetIndicator();
    }

    private void HandleBrightenSoftTargetHUD()
    {
        TopRight.BrightenSoftTargetIndicator();
    }

    private void HandleEnemyUntargeted()
    {
        EnemyHealth.EnemyUntargeted();
    }

    private void HandleEnemyTargeted(Enemy enemy)
    {
		GD.Print("Enemy targeted");
        EnemyHealth.EnemyTargeted(enemy);
    }

    private void HandleHideSoftTargetIcon(Enemy enemy)
    {
        EnemyHealth.HideSoftTargetIcon(enemy);
    }

    private void HandleShowSoftTargetIcon(Enemy enemy)
    {
        EnemyHealth.ShowSoftTargetIcon(enemy);
    }
}
