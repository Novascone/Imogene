using Godot;
using System;

public partial class EnemyHealth : Control
{
	[Export] public VBoxContainer vBoxContainer { get; set; }
	[Export] public Label name { get; set; }
	[Export] public ProgressBar health_bar { get; set; }
	[Export] public TextureProgressBar posture_bar { get; set; }
	public Enemy targeted_enemy { get; set; } = null;

	public void EnemyTargeted(Enemy enemy)
    {
		targeted_enemy = enemy;
        health_bar.MaxValue = enemy.Health.MaxValue;
		health_bar.Value = enemy.Health.MaxValue;
		name.Text = enemy.Identifier;
	
		targeted_enemy.UI.HardTargetIcon.Show();
		targeted_enemy.UI.StatusBar.Show();
		targeted_enemy.UI.SoftTargetIcon.Show();
		
		Show();
    }
	public void EnemyUntargeted()
    {
        Hide();
		if(targeted_enemy != null)
		{
			targeted_enemy.UI.HardTargetIcon.Hide();
			targeted_enemy.UI.StatusBar.Hide();
			targeted_enemy.UI.SoftTargetIcon.Hide();
			targeted_enemy.Targeted = false;
			targeted_enemy = null;
		}
		
    }

	public static void ShowSoftTargetIcon(Enemy enemy_)
	{
		enemy_.UI.SoftTargetIcon.Show();
		
	}

	public static void HideSoftTargetIcon(Enemy enemy_)
	{
		enemy_.UI.SoftTargetIcon.Hide();
	}



    public void HandleEnemyHealthChangedUI(Entity enemy_, float health_)
    {
		if(enemy_ == targeted_enemy)
		{
			health_bar.Value = health_;
		}
        
    }
	private void HandleEnemyPostureChangedUI(Enemy enemy_, float posture_)
    {
		if(enemy_ == targeted_enemy)
		{
			posture_bar.Value = posture_;
		}
    
    }

	public void Subscribe(Player player_)
	{
		player_.PlayerSystems.TargetingSystem.TargetHealthChanged += HandleEnemyHealthChangedUI;
	}
	
	public void UnSubscribe(Player player)
	{
		player.EntitySystems.damage_system.HealthChanged -= HandleEnemyHealthChangedUI;
	}
}
