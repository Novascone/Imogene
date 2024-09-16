using Godot;
using System;

public partial class DepthSheet : Control
{

	[Export] public Control title;
	[Export] public DepthScroller scroll_container;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void HandleUpdateStats(Player player)
    {
		GD.Print("Got update stats signal");
		int i = 0;
        foreach(Control control in scroll_container.vbox.GetChildren())
		{
			GD.Print(control.Name);
			if(control is UIStat ui_stat)
			{
				GD.Print(ui_stat.Name);
				ui_stat.GetStatInfo(player.entity_controllers.stats_controller.depth_stats[i].base_value);
				i += 1;
			}
		}
		i = 0;
    }
}
