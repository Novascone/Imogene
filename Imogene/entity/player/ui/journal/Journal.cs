using Godot;
using System;

public partial class Journal : Control
{
	[Export] public Control quest_tab { get; set; }
	[Export] public Control quest_page { get; set; }
	[Export] public Control bestiary_tab { get; set; }
	[Export] public Control bestiary_page { get; set; }
	[Export] public Control lore_tab { get; set; }
	[Export] public Control lore_page { get; set; }
	[Export] public Control tutorial_tab { get; set; }
	[Export] public Control tutorial_page { get; set; }

}
