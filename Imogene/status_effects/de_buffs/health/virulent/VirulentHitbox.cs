using Godot;
using System;
using System.Collections.Generic;

public partial class VirulentHitbox : Area3D
{
	public Entity root_infected { get; set; } = null;
	public List<Enemy> enemies_to_be_infected = new();
	

	public void _on_body_entered(Node3D body_)
	{
		GD.Print("name of body entering virulent area " + body_.Name);
		if(body_ is Enemy enemy)
		{
			if(enemy != root_infected)
			{
				// EmitSignal(nameof(AddAdditionalStatusEffect), enemy, infect_poison);
				enemies_to_be_infected.Add(enemy);
			}
		}
		
	}
}
