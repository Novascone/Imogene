using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Interactive : Area3D
{
	private MeshInstance3D shader;
	private ShaderMaterial selected_shader;
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void change_cursor(Area3D game_object)
	{
		game_object = (Area3D)GetNode(GetPath());
		if(game_object.IsInGroup("interactive"))
		{
			
			// shader = (MeshInstance3D)game_object.GetNode("Mesh");
			// selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
			// selected_shader.SetShaderParameter("Strength", 1.0f);
			// GD.Print(selected_shader.ResourceName);
			// GD.Print(selected_shader.GetShaderParameter("Strength"));
		
		}

		else if(game_object.IsInGroup("attack_area"))
		{

			// game_object = (Area3D)game_object.GetParent().GetParent();
			// shader = (MeshInstance3D)game_object.GetNode("Mesh");
			// selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
			// selected_shader.SetShaderParameter("Strength", 1.0f);
			// GD.Print(selected_shader.ResourceName);
			// GD.Print(selected_shader.GetShaderParameter("Strength"));
		
		}
		
	}


	public void highlight(Area3D game_object)
	{

		game_object = (Area3D)game_object.GetParent().GetParent();
		shader = (MeshInstance3D)game_object.GetNode("Mesh");
		selected_shader = (ShaderMaterial)shader.GetActiveMaterial(0).NextPass;
		selected_shader.SetShaderParameter("strength", 0.4f);
		GD.Print(selected_shader.ResourceName);
		GD.Print(selected_shader.GetShaderParameter("strength"));
		
	}

	public void unhighlight(Area3D game_object)
	{
		if(selected_shader != null)
			{

				selected_shader.SetShaderParameter("strength", 0);
				GD.Print(selected_shader.ResourceName);
				GD.Print(selected_shader.GetShaderParameter("strength"));
				
			}
	}

}
