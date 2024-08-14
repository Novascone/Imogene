using Godot;
using System;

public partial class EnemyHealthUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private ProgressBar enemy_health;
	private TextureProgressBar enemy_posture;
	private Label enemy_name;
	public Enemy targeted_enemy;
	private CustomSignals _customSignals; // Instance of CustomSignals
	public override void _Ready()
	{

		enemy_health = GetNode<ProgressBar>("VBoxContainer/HealthBar");
		enemy_posture = GetNode<TextureProgressBar>("VBoxContainer/PostureBar");
		enemy_name = GetNode<Label>("VBoxContainer/Label");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyHealthChangedUI += HandleEnemyHealthChangedUI;
		_customSignals.EnemyPostureChangedUI += HandleEnemyPostureChangedUI;
		// _customSignals.EnemyTargetedUI += HandleEnemyTargeted;
		// _customSignals.EnemyUntargetedUI += HandleEnemyUntargeted;
	}

    // private void HandleEnemyUntargeted()
    // {
    //     Hide();
    // }

    // private void HandleEnemyTargeted(Enemy enemy)
    // {
    //     enemy_health.MaxValue = enemy.maximum_health;
	// 	enemy_health.Value = enemy.health;
	// 	enemy_name.Text = enemy.identifier;
	// 	enemy_posture.MaxValue = enemy.maximum_posture;
	// 	enemy_posture.Value = enemy.posture;
	// 	Show();
    // }
	public void EnemyTargeted(Enemy enemy)
    {
		targeted_enemy = enemy;
        enemy_health.MaxValue = enemy.maximum_health;
		enemy_health.Value = enemy.health;
		enemy_name.Text = enemy.identifier;
		enemy_posture.MaxValue = enemy.maximum_posture;
		enemy_posture.Value = enemy.posture;
		targeted_enemy.hard_target_icon.Show();
		targeted_enemy.status_bar.Show();
		targeted_enemy.soft_target_icon.Show();
		
		Show();
    }
	public void EnemyUntargeted()
    {
        Hide();
		if(targeted_enemy != null)
		{
			targeted_enemy.hard_target_icon.Hide();
			targeted_enemy.status_bar.Hide();
			targeted_enemy.soft_target_icon.Hide();
			targeted_enemy.targeted = false;
			targeted_enemy = null;
		}
		
    }

	public void SetSoftTargetIcon(Enemy enemy)
	{
		if(enemy.soft_target || enemy.targeted)
		{
			enemy.soft_target_icon.Show();
		}
		else
		{
			enemy.soft_target_icon.Hide();
		}
		
	}



    private void HandleEnemyHealthChangedUI(Enemy enemy, float health)
    {
		if(enemy == targeted_enemy)
		{
			GD.Print("enemy is enemy");
			enemy_health.Value = health;
		}
        
    }
	private void HandleEnemyPostureChangedUI(Enemy enemy, float posture)
    {
		if(enemy == targeted_enemy)
		{
			GD.Print("enemy is enemy");
			enemy_posture.Value = posture;
		}
        
		GD.Print("enemy max posture from UI " + enemy_posture.MaxValue);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(double delta)
	// {
	// }
}
