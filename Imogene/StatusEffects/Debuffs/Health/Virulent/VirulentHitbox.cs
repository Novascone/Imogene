using Godot;
using System;
using System.Collections.Generic;

public partial class VirulentHitbox : Area3D
{
	public Entity RootInfected { get; set; } = null;
	public List<Enemy> EnemiesToBeInfected = new();
	

	public void _on_body_entered(Node3D body)
	{
		GD.Print("name of body entering virulent area " + body.Name);
		if(body is Enemy enemy)
		{
			if(enemy != RootInfected)
			{
				// EmitSignal(nameof(AddAdditionalStatusEffect), enemy, infect_poison);
				EnemiesToBeInfected.Add(enemy);
			}
		}
		
	}
}
