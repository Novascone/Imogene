using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Interactive : Area3D
{
	private Area3D item;
	private MeshInstance3D shader;
	private ShaderMaterial selected_shader;
	public Resource cursor_sword = ResourceLoader.Load("res://images/custom_prelim_sword_cursor.png");
	public Resource cursor_interact = ResourceLoader.Load("res://images/custom_prelim_interact_cursor.png");
	public Resource cursor = ResourceLoader.Load("res://images/custom_prelim_cursor.png");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		item = (Area3D)GetNode(GetPath());
		shader = (MeshInstance3D)item.GetChild(0);
		selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
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
				// selected_shader.SetShaderParameter("Strength", 0.2);
				if(item.IsInGroup("attack_area"))
				{
					
				}
				else
				{
					Input.SetCustomMouseCursor(cursor_interact);
				}
		}
		
		else
		{
			// selected_shader.SetShaderParameter("Strength", 0.0);
		}
		
		
	}
	public void reset_cursor()
	{
			Input.SetCustomMouseCursor(cursor);
	}

}
