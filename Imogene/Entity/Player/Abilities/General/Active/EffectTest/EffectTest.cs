using Godot;
using System;

public partial class EffectTest : Ability
{

	[Export] private Timer DurationTimer;
	[Export] public PackedScene HitboxToLoad;
	[Export] public PackedScene MeshToLoad;
	EffectTestHitbox EffectTestHitbox;
	MeshInstance3D EffectTestMesh;
	// [Export] public PackedScene effect_scene;
	public StatusEffect Effect;
	
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
		base.Execute(player);
		DurationTimer.Start();
		
		if(EffectTestHitbox == null)
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
			if(DurationTimer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue), this);
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(DurationTimer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}		
		if(Input.IsActionJustReleased(AssignedButton) || player.Resource.CurrentValue + ResourceChange <= 0 && player.AbilityInUse == this)
		{
			// GD.Print("Remove hit box");
			RemoveHitbox(player);
			RemoveMesh(player);
			EmitSignal(nameof(AbilityFinished), this);
		}	
        
    }

    public void AddHitbox(Player player)
	{
		EffectTestHitbox = (EffectTestHitbox)HitboxToLoad.Instantiate(); // Instantiate the projectile
		player.SurroundingHitbox.AddChild(EffectTestHitbox);
		Effect = new Virulent();
		EffectTestHitbox.Effects.Add(Effect);
		if(EffectTestMesh == null)
		{
			// GD.Print("Add mesh");
			EffectTestMesh = (MeshInstance3D)MeshToLoad.Instantiate();
			player.SurroundingHitbox.AddChild(EffectTestMesh);
		}
	}

	public void RemoveHitbox(Player player)
	{
		if(EffectTestHitbox != null)
		{
			EffectTestHitbox.QueueFree();
			EffectTestHitbox = null;
		}
		
		EmitSignal(nameof(AbilityFinished), this);
		DurationTimer.Stop();
	}

	public void RemoveMesh(Player player)
	{
		// GD.Print("remove mesh");
		if(EffectTestMesh != null)
		{
			player.SurroundingHitbox.RemoveChild(EffectTestMesh);
			EffectTestMesh = null;
		}
		
	}
}
