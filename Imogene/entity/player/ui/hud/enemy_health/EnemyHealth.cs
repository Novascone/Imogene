using Godot;
using System;

public partial class EnemyHealth : Control
{
	[Export] public VBoxContainer vBoxContainer { get; set; }
	[Export] public Label name { get; set; }
	[Export] public ProgressBar health_bar { get; set; }
	[Export] public TextureProgressBar posture_bar { get; set; }
	public Enemy targeted_enemy { get; set; } = null;

	public void EnemyTargeted(Enemy enemy_)
    {
		targeted_enemy = enemy_;
        health_bar.MaxValue = enemy_.health.max_value;
		health_bar.Value = enemy_.health.current_value;
		name.Text = enemy_.identifier;
		posture_bar.MaxValue = enemy_.posture.max_value;
		posture_bar.Value = enemy_.posture.current_value;
		targeted_enemy.ui.hard_target_icon.Show();
		targeted_enemy.ui.status_bar.Show();
		targeted_enemy.ui.soft_target_icon.Show();
		
		Show();
    }
	public void EnemyUntargeted()
    {
        Hide();
		if(targeted_enemy != null)
		{
			targeted_enemy.ui.hard_target_icon.Hide();
			targeted_enemy.ui.status_bar.Hide();
			targeted_enemy.ui.soft_target_icon.Hide();
			targeted_enemy.targeted = false;
			targeted_enemy = null;
		}
		
    }

	public void ShowSoftTargetIcon(Enemy enemy_)
	{
		enemy_.ui.soft_target_icon.Show();
		
	}

	public void HideSoftTargetIcon(Enemy enemy_)
	{
		enemy_.ui.soft_target_icon.Hide();
	}



    private void HandleEnemyHealthChangedUI(Enemy enemy_, float health_)
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
}
