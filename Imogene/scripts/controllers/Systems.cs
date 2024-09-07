using Godot;
using System;

public partial class Systems : Node
{	
	[Export] public VisionSystem vision_system;
	[Export] public InteractSystem interact_system;
	[Export] public TargetingSystem targeting_system;
	[Export] public XPSystem xp_system;
	// [Export] public DamageSystem damage_system;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
