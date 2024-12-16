using Godot;
using System;

public partial class JournalPage : Node
{
	[Export] public Label label { get; set; }
	[Export] public string page_text { get; set; }
}
