using Godot;
using System;

public partial class enemy : Interactive
{
	private Area3D game_object;
	public bool mouse_over = false;
	
	
	public override void _Ready()
	{
		this.game_object = (Area3D)GetNode(GetPath());
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	

   

}

