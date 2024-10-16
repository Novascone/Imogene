using Godot;
using System;

public partial class BottomButtons : Control
{

	[Export] public BottomButton abilities { get; set; }
	[Export] public BottomButton journal { get; set; }
	[Export] public BottomButton achievements { get; set; }
	[Export] public BottomButton social { get; set; }
	[Export] public BottomButton options { get; set; }
}
