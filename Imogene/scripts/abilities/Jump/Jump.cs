using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	int timer = 0;
    // Called when the node enters the scene tree for the first time.
    public override void Execute(player s)
    {	
		GD.Print("here");
		if(s.IsOnFloor() && off_floor == false && !s.jumping)
		{
			GD.Print("start jumping");
			s.tree.Set("parameters/Master/Main/conditions/jumping", true);
			s.velocity.Y = Mathf.Lerp(s.velocity.Y, s.jump_speed, 0.7f);			s.jumping = true;
			off_floor = true;
			timer += 1;
		}
        else if(!s.IsOnFloor())
		{
			s.tree.Set("parameters/Master/Main/conditions/jumping", false);
			s.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", false);
			GD.Print("still jumping");
			s.jumping = true;
			timer += 1;
		}
		if(timer > 1 && s.IsOnFloor() && off_floor == true)
		{
			GD.Print("stop jumping");
			s.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true);
			off_floor = false;
			s.jumping = false;
			in_use = false;
			timer = 0;
		}
    }
	
}
