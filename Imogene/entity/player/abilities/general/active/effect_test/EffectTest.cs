using Godot;
using System;

public partial class EffectTest : Ability
{

	[Export] private Timer duration_timer;
	[Export] public PackedScene hitbox_to_load;
	[Export] public PackedScene mesh_to_load;
	EffectTestHitbox effect_test_hitbox;
	MeshInstance3D effect_test_mesh;
	// [Export] public PackedScene effect_scene;
	public StatusEffect effect;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		// {
			
		// 	// GD.Print("Queuing whirlwind");
		// 	QueueAbility();	
		// }
		// else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		// {
		// 	if(duration_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 	}		
		// }
		// if(duration_timer.TimeLeft == 0) // If not held check if ability can be used
		// {
		// 	CheckCanUseAbility();
		// }		
		// if(Input.IsActionJustReleased(assigned_button) || player.resource - resource_cost <= 0 && player.ability_in_use == this)
		// {
		// 	// GD.Print("Remove hit box");
		// 	RemoveHitbox();
		// 	RemoveMesh();
		// }	
	}

	public override void Execute(Player player)
	{
		
		// GD.Print("execute");
		state = States.not_queued;
		duration_timer.Start();
		
		if(effect_test_hitbox == null)
		{
			AddHitbox(player);
		}
	}

    public override void FrameCheck(Player player)
    {

		// else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		// {
		// 	if(duration_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 	}		
		// }
		// if(duration_timer.TimeLeft == 0) // If not held check if ability can be used
		// {
		// 	CheckCanUseAbility();
		// }		
		// if(Input.IsActionJustReleased(assigned_button) || player.resource - resource_cost <= 0 && player.ability_in_use == this)
		// {
		// 	// GD.Print("Remove hit box");
		// 	RemoveHitbox();
		// 	RemoveMesh();
		// }	

		if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(duration_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue),this);
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(duration_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}		
		if(Input.IsActionJustReleased(assigned_button) || player.resource.current_value + resource_change <= 0 && player.ability_in_use == this)
		{
			// GD.Print("Remove hit box");
			RemoveHitbox();
			RemoveMesh(player);
			EmitSignal(nameof(AbilityFinished),this);
		}	
        
    }

    public void AddHitbox(Player player)
	{
		effect_test_hitbox = (EffectTestHitbox)hitbox_to_load.Instantiate(); // Instantiate the projectile
		player.surrounding_hitbox.AddChild(effect_test_hitbox);
		effect = new Knockback(player);
		effect_test_hitbox.effects.Add(effect);
		if(effect_test_mesh == null)
		{
			// GD.Print("Add mesh");
			effect_test_mesh = (MeshInstance3D)mesh_to_load.Instantiate();
			player.surrounding_hitbox.AddChild(effect_test_mesh);
		}
	}

	public void RemoveHitbox()
	{
		if(effect_test_hitbox != null)
		{
			effect_test_hitbox.QueueFree();
			effect_test_hitbox = null;
		}
		
		EmitSignal(nameof(AbilityFinished),this);
		duration_timer.Stop();
	}

	public void RemoveMesh(Player player)
	{
		// GD.Print("remove mesh");
		if(effect_test_mesh != null)
		{
			player.surrounding_hitbox.RemoveChild(effect_test_mesh);
			effect_test_mesh = null;
		}
		
	}
}
