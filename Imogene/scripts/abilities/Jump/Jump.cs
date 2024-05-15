using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	int timer = 0;
    // Called when the node enters the scene tree for the first time.
    public override void Execute(player s)
    {	
		
		if(s.IsOnFloor() && off_floor == false && !s.jumping)
		{
			GD.Print("start jumping");
			s.velocity.Y = s.jump_speed;
			s.jumping = true;
			off_floor = true;
			timer += 1;
		}
        else if(!s.IsOnFloor())
		{
			GD.Print("still jumping");
			s.jumping = true;
			timer += 1;
		}
		if(timer > 1 && s.IsOnFloor() && off_floor == true)
		{
			GD.Print("stop jumping");
			off_floor = false;
			s.jumping = false;
			in_use = false;
			timer = 0;
		}
    }
	
}
