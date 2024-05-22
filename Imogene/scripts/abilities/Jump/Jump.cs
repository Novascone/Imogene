using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	int timer = 0;
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.KeyBind += HandleKeyBind;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;

		
    }


	private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    {
        if(this.Name == ability)
		{
			CheckAssignment(button_name);
		}
    }

    private void HandlePlayerInfo(player s)
    {
        player = s;
    }
	  public override void _PhysicsProcess(double delta)
    {
		
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}
		if(player.can_use_abilities && Input.IsActionPressed(assigned_button) && CheckCross() || player.jumping)
		{
			Execute();
		}
		
    }

    // Called when the node enters the scene tree for the first time.
    public void Execute()
    {	
		
		if(player.IsOnFloor() && !player.jumping)
		{
			GD.Print("start jumping");
			player.tree.Set("parameters/Master/Main/conditions/jumping", true);
			player.velocity.Y = player.jump_speed;			
			player.jumping = true;
			timer += 1;
		}
		else if(player.IsOnFloor())
		{
			GD.Print("stop jumping");
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true);
			off_floor = false;
			player.jumping = false;
			in_use = false;
			timer = 0;
		}
        if(!player.IsOnFloor())
		{
			player.tree.Set("parameters/Master/Main/conditions/jumping", false);
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", false);
			GD.Print("still jumping");
			player.jumping = true;
			timer += 1;
		}
		
    }
	
}
