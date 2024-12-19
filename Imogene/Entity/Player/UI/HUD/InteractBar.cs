using Godot;
using System;

public partial class InteractBar : Control
{
	[Export] public Label Button { get; set; }
	[Export] public Label InteractObject { get; set; }
	[Export] public Control InteractInventory { get; set; }

	public void SetInteractText(string objectName)
	{
		InteractObject.Text = objectName;
	}
}
