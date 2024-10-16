using Godot;
using System;

public partial class Passives : Control
{
	[Export] public Control general_passives { get; set; }
	[Export] public Control class_passives { get; set; }
	[Export] public Control class_title { get; set; }

	public void ResetPage()
	{
		
	}
}
