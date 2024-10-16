using Godot;
using System;

public partial class HUDButton : Button
{
	[Export] public string button_bind { get; set; }
	[Export] public string side { get; set; }
	[Export] public string level { get; set; }
	[Export] public Label label  { get; set; }
	public string ability_name  { get; set; } = "";
 	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = button_bind;
	}

}
