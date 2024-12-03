using Godot;
using System;

public partial class CharacterOutline : Control
{
	// Base stats
	[Export] public UIStat level { get; set;}
	[Export] public Control base_stats { get; set;}
	[Export] public UIStat strength { get; set;}
	[Export] public UIStat dexterity { get; set;}
	[Export] public UIStat intellect { get; set;}
	[Export] public UIStat vitality { get; set;}
	[Export] public UIStat stamina { get; set;}
	[Export] public UIStat wisdom { get; set;}
	[Export] public UIStat charisma { get; set;}
	[Export] public Control summary_stats { get; set;}
	[Export] public UIStat damage { get; set;}
	[Export] public UIStat resistance { get; set;}
	[Export] public UIStat recovery { get; set;}

	[Export] public Control reputation { get; set;}
	[Export] public Button sheet { get; set;}

	// Armor
	[Export] public Control shoulders { get; set;}
	[Export] public Control gloves { get; set;}
	[Export] public Control ring_1 { get; set;}
	[Export] public Control main_hand { get; set;}
	[Export] public Control main_hand_secondary { get; set;}
	[Export] public Control head { get; set;}
	[Export] public Control chest { get; set;}
	[Export] public Control belt { get; set;}
	[Export] public Control pants { get; set;}
	[Export] public Control boots { get; set;}
	[Export] public Control neck { get; set;}
	[Export] public Control bracers { get; set;}
	[Export] public Control ring_2 { get; set;}
	[Export] public Control off_hand { get; set;}
	[Export] public Control off_hand_secondary { get; set;}

	public void _on_sheet_button_down()
	{

	}

    internal void HandleUpdateStats(Player player_)
    {
		// Needs updating
		// int i = 0;
        // level.value.Text = player_.entity_controllers.stats_controller.base_stats[i].base_value.ToString();
		// level.GetStatInfo(player_.entity_controllers.stats_controller.base_stats[i].base_value);
		// i += 1;
		// foreach(UIStat ui_stat in base_stats.GetChildren())
		// {
		// 	ui_stat.GetStatInfo(player_.entity_controllers.stats_controller.base_stats[i].base_value);
		// 	i += 1;
		// }
		// i = 0;
		// foreach(Control control in summary_stats.GetChildren())
		// {
		// 	if(control is UIStat ui_stat)
		// 	{
		// 		ui_stat.GetStatInfo(player_.entity_controllers.stats_controller.summary_stats[i]);
		// 		i += 1;
		// 	}
		// }

    }
}
