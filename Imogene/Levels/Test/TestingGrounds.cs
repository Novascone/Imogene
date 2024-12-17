using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class TestingGrounds : Node3D
{
	[Export] public StaticBody3D static_body;
	public override void _Input(InputEvent @event)
    {
		
		
        if(@event.IsActionPressed("six"))
		{
			GD.Print("changing static body layer");
			static_body.CollisionLayer = 4;
		}
		
		
		
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
