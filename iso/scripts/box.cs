using Godot;
using System;


public partial class box : interactive
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
		if(Is_over())
		{
			if(item.IsInGroup("interactive"))
			GD.Print("interactive");
		}

	}

	public override void _MouseEnter()
    {
		mouse_over = true;
        base._MouseEnter();
    }

    public override void _MouseExit()
    {
		mouse_over = false;
        base._MouseExit();
    }

	public bool Is_over()
	{
		if(mouse_over)
		{
			return true;
		}
		return false;
	}


	
}
