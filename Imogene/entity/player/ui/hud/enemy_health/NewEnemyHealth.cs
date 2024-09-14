using Godot;
using System;

public partial class NewEnemyHealth : Control
{
	[Export] public VBoxContainer vBoxContainer;
	[Export] public Label name;
	[Export] public ProgressBar health_bar;
	[Export] public TextureProgressBar posture_bar;
	public Enemy targeted_enemy;

	public void EnemyTargeted(Enemy enemy)
    {
		targeted_enemy = enemy;
        health_bar.MaxValue = enemy.health.max_value;
		health_bar.Value = enemy.health.current_value;
		name.Text = enemy.identifier;
		posture_bar.MaxValue = enemy.posture.max_value;
		posture_bar.Value = enemy.posture.current_value;
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

	public void ShowSoftTargetIcon(Enemy enemy)
	{
		enemy.ui.soft_target_icon.Show();
		
	}

	public void HideSoftTargetIcon(Enemy enemy)
	{
		enemy.ui.soft_target_icon.Hide();
	}



    private void HandleEnemyHealthChangedUI(Enemy enemy, float health)
    {
		if(enemy == targeted_enemy)
		{
			// GD.Print("enemy is enemy");
			health_bar.Value = health;
		}
        
    }
	private void HandleEnemyPostureChangedUI(Enemy enemy, float posture)
    {
		if(enemy == targeted_enemy)
		{
			// GD.Print("enemy is enemy");
			posture_bar.Value = posture;
		}
        
		// GD.Print("enemy max posture from UI " + enemy_posture.MaxValue);
    }
}
