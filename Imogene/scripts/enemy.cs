using Godot;
using System;

public partial class enemy : CharacterBody3D
{
	private Area3D enemy_hitbox;
	private AnimationPlayer damage_numbers;
	private Label3D damage_label;
	private int health = 20;
	private MeshInstance3D targeting_icon;
	private CustomSignals _customSignals;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy_hitbox = (Area3D)GetNode("EnemyHitbox");
		enemy_hitbox.AreaEntered += OnHitboxEntered;
		damage_numbers = GetNode<AnimationPlayer>("Damage_Number_3D/AnimationPlayer");
		damage_label = GetNode<Label3D>("Damage_Number_3D/Label3D");
		targeting_icon = GetNode<MeshInstance3D>("TargetingIcon");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandleDamageEnemy;
		_customSignals.EnemyTargeted += HandleEnemyTargeted;
		_customSignals.EnemyUnTargeted += HandleEnemyUnTargeted;
	}

    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("attacking"))
		{
			
			GD.Print("hit");

			damage_numbers.Play("Rise_and_Fade");

		}
		
	}

	private void HandleDamageEnemy(int damage_amount)
	{
		health -= damage_amount;
		damage_label.Text = Convert.ToString(damage_amount);
	}

	 private void HandleEnemyTargeted()
    {
		targeting_icon.Visible = true;
    }
	private void HandleEnemyUnTargeted()
    {
        targeting_icon.Visible = false;
    }
}
