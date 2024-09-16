using Godot;
using System;

public partial class CharacterOutline : Controller
{
	// Base stats
	[Export] public UIStat level;
	[Export] public Control base_stats;
	[Export] public UIStat strength;
	[Export] public UIStat dexterity;
	[Export] public UIStat intellect;
	[Export] public UIStat vitality;
	[Export] public UIStat stamina;
	[Export] public UIStat wisdom;
	[Export] public UIStat charisma;
	[Export] public Control summary_stats;
	[Export] public UIStat damage;
	[Export] public UIStat resistance;
	[Export] public UIStat recovery;

	[Export] public Control reputation;
	[Export] public Button sheet;

	// Armor
	[Export] public Control shoulders;
	[Export] public Control gloves;
	[Export] public Control ring_1;
	[Export] public Control main_hand;
	[Export] public Control main_hand_secondary;
	[Export] public Control head;
	[Export] public Control chest;
	[Export] public Control belt;
	[Export] public Control pants;
	[Export] public Control boots;
	[Export] public Control neck;
	[Export] public Control bracers;
	[Export] public Control ring_2;
	[Export] public Control off_hand;
	[Export] public Control off_hand_secondary;

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_sheet_button_down()
	{

	}

    internal void HandleUpdateStats(Player player)
    {
		
		int i = 0;
        level.value.Text = player.entity_controllers.stats_controller.base_stats[i].base_value.ToString();
		level.GetStatInfo(player.entity_controllers.stats_controller.base_stats[i].base_value);
		i += 1;
		foreach(UIStat ui_stat in base_stats.GetChildren())
		{
			ui_stat.GetStatInfo(player.entity_controllers.stats_controller.base_stats[i].base_value);
			i += 1;
		}
		i = 0;
		foreach(Control control in summary_stats.GetChildren())
		{
			if(control is UIStat ui_stat)
			{
				ui_stat.GetStatInfo(player.entity_controllers.stats_controller.summary_stats[i]);
				i += 1;
			}
		}

    }
}
