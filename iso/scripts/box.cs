using Godot;
using System;


public partial class box : Interactive
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
		if(item.IsInGroup("interactive"))
		{
			GD.Print("over interactive");
		}

        base._MouseEnter();
    }


    public override void _MouseExit()
    {
		reset_cursor();
        base._MouseExit();
    }


}
