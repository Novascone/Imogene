using Godot;
using GodotPlugins.Game;
using System;

public partial class NewHUD : Control

{

	[Export] public NewEnemyHealth enemy_health;
	[Export] public MainHUD main;
	[Export] public InteractBar interact_bar;
	[Export] public TopRightHUD top_right;

	[Signal] public delegate void HUDPreventingInputEventHandler(bool preventing_input);

	public JoyButton interact_button = JoyButton.A;

	public bool in_interact_area;
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
				interact_bar.interact_inventory.Visible = !interact_bar.interact_inventory.Visible;
				AcceptEvent();
			}
			
		}
	}

	public void SubscribeToInteractSignals(Player player)
	{
		player.areas.interact.AreaEntered += OnInteractAreaEntered;
		player.areas.interact.AreaExited += OnInteractAreaExited;
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
		if(!area.IsInGroup("PlayerPickUp"))
		in_interact_area = false;
		interact_bar.button.Text = interact_button.ToString() + ";";
		interact_bar.interact_inventory.Hide();
        interact_bar.Hide(); 
		EmitSignal(nameof(HUDPreventingInput),false);
    }

    private void OnInteractAreaEntered(Area3D area)
    {
		if(!area.IsInGroup("PlayerPickUp"))
		{
			in_interact_area = true;
			interact_bar.button.Text = interact_button.ToString() + ":" + " interact";
			interact_bar.interact_object.Text = area.Name;
			interact_bar.Show();
			EmitSignal(nameof(HUDPreventingInput),true);
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
