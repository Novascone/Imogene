using Godot;
using System;

public partial class InteractBar : Control
{
	[Export] public Label button { get; set; }
	[Export] public Label interact_object { get; set; }
	[Export] public Control interact_inventory { get; set; }

	public void SetInteractText(string object_name_)
	{
		interact_object.Text = object_name_;
	}
}
