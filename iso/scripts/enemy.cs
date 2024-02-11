using Godot;
using System;

public partial class enemy : Interactive
{
	private Area3D item;
	public bool mouse_over = false;
	
	
	public override void _Ready()
	{
		item = (Area3D)GetNode(GetPath());
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public override void _MouseEnter()
    {
		change_cursor(item);
			if(item.IsInGroup("attack_area"))
			{
				GD.Print("over enemy");
			}
        base._MouseEnter();
    }

    public override void _MouseExit()
    {
		reset_cursor();
        base._MouseExit();
    }


}
