using Godot;
using System;

public partial class EnemyHealthUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private ProgressBar enemy_health;
	private TextureProgressBar enemy_posture;
	private Label enemy_name;
	private CustomSignals _customSignals; // Instance of CustomSignals
	public override void _Ready()
	{

		enemy_health = GetNode<ProgressBar>("VBoxContainer/HealthBar");
		enemy_posture = GetNode<TextureProgressBar>("VBoxContainer/PostureBar");
		enemy_name = GetNode<Label>("VBoxContainer/Label");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyHealthChangedUI += HandleEnemyHealthChangedUI;
		_customSignals.EnemyPostureChangedUI += HandleEnemyPostureChangedUI;
		_customSignals.EnemyTargetedUI += HandleEnemyTargeted;
		_customSignals.EnemyUntargetedUI += HandleEnemyUntargeted;
	}

    private void HandleEnemyUntargeted()
    {
        Hide();
    }

    private void HandleEnemyTargeted(Enemy enemy)
    {
        enemy_health.MaxValue = enemy.maximum_health;
		enemy_health.Value = enemy.health;
		enemy_name.Text = enemy.identifier;
		enemy_posture.MaxValue = enemy.maximum_posture;
		enemy_posture.Value = enemy.posture;
		Show();
    }

    private void HandleEnemyHealthChangedUI(float health)
    {
        enemy_health.Value = health;
    }
	private void HandleEnemyPostureChangedUI(float posture)
    {
        enemy_posture.Value = posture;
		GD.Print("enemy max posture from UI " + enemy_posture.MaxValue);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(double delta)
	// {
	// }
}
