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
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		reset_cursor();
		change_cursor(item);
	}

	public void change_cursor(Area3D item)
	{
		item = (Area3D)GetNode(GetPath());
		if(item.IsInGroup("interactive"))
		{
			
			Input.SetCustomMouseCursor(cursor_interact);
			shader = (MeshInstance3D)item.GetNode("Mesh");
			selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
			selected_shader.SetShaderParameter("Strength", 0.2);
			GD.Print(selected_shader.GetShaderParameter("Strength"));
		
		}

		else if(item.IsInGroup("attack_area"))
		{
			item = (Area3D)item.GetParent().GetParent();
			shader = (MeshInstance3D)item.GetNode("Mesh");
			selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
			selected_shader.SetShaderParameter("Strength", 0.2);
			GD.Print(selected_shader.GetShaderParameter("Strength"));
			
			// shader = (MeshInstance3D)item.GetParent();
			// selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
			// selected_shader.SetShaderParameter("Strength", 0.2);
			// GD.Print(selected_shader.GetShaderParameter("Strength"));
		}
		
	}
	public void reset_cursor()
	{
			Input.SetCustomMouseCursor(cursor);
			if(selected_shader != null)
			{
				selected_shader.SetShaderParameter("Strength", 0);
				GD.Print(selected_shader.GetShaderParameter("Strength"));
			}

	}

}
