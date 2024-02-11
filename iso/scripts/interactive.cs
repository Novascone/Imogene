using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Interactive : Area3D
{
	private Area3D item;
	public Resource cursor_sword = ResourceLoader.Load("res://Images/custom_prelim_sword_cursor.png");
	public Resource cursor_interact = ResourceLoader.Load("res://Images/custom_prelim_interact_cursor.png");
	public Resource cursor = ResourceLoader.Load("res://Images/custom_prelim_cursor.png");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		item = (Area3D)GetNode(GetPath());
		reset_cursor();
		change_cursor(item);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void change_cursor(Area3D item)
	{
		if(item.IsInGroup("interactive"))
		{
				if(item.IsInGroup("attack_area"))
				{
					Input.SetCustomMouseCursor(cursor_sword);
				}
				else
				{
					Input.SetCustomMouseCursor(cursor_interact);
				}
				
		}
		
		
	}

	public void change_cursor_attack(Area3D item)
	{
		Input.SetCustomMouseCursor(cursor_sword);
	}

	public void reset_cursor()
	{
			Input.SetCustomMouseCursor(cursor);
	}

}
