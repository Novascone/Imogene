using Godot;
using System;

public partial class HUDCross : Control
{
	[Export] public HUDButton up { get; set; }
	[Export] public Label up_label { get; set; }
	[Export] public HUDButton left { get; set; }
	[Export] public Label left_label { get; set; }
	[Export] public HUDButton right { get; set; }
	[Export]public Label right_label { get; set; }
	[Export] public HUDButton down { get; set; }
	[Export] public Label down_label { get; set; }
	
}
