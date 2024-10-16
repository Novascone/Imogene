using Godot;
using System;

public partial class StatInfo : Control
{
	[Export] public RichTextLabel tool_tip { get; set; }
	[Export] public Control tool_tip_container { get; set; }

}
