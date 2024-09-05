using Godot;
using System;

public partial class InteractSystem : Node
{
	public bool in_interact_area;
	public bool left_interact;
	public bool entered_interact;
	Area3D area_interacting;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SubscribeToInteractSignals(Player player)
	{
		player.areas.interact.AreaEntered += (area) => OnInteractAreaEntered(area, player);
		player.areas.interact.AreaExited += (area) => OnInteractAreaExited(area, player);
	}


    private void OnInteractAreaExited(Area3D area, Player player)
    {

		GD.Print("Interact area exited");
        left_interact = true;
    	in_interact_area = false;
    	area_interacting = null;
    }

    private void OnInteractAreaEntered(Area3D area, Player player)
    {
		GD.Print("Interact area entered");
        entered_interact = true;
    	in_interact_area = true;
    	area_interacting = area;
    }
}
