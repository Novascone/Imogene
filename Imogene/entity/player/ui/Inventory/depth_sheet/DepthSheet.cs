using Godot;
using System;
using System.Linq;

public partial class DepthSheet : Control
{

	[Export] public Control title { get; set; }
	[Export] public DepthScroller scroll_container { get; set; }

    internal void HandleUpdateStats(Player player_)
    {
		int i = 0;
        foreach(Control _control in scroll_container.vbox.GetChildren().Cast<Control>())
		{
			if(_control is UIStat ui_stat)
			{
				ui_stat.GetStatInfo(player_.entity_controllers.stats_controller.depth_stats[i].base_value);
				i += 1;
			}
		}
		i = 0;
    }
}
