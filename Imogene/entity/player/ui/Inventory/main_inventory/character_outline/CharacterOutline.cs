using Godot;
using System;

public partial class CharacterOutline : Controller
{
	// Base stats
	[Export] public Control level;
	[Export] public Control strength;
	[Export] public Control dexterity;
	[Export] public Control intellect;
	[Export] public Control vitality;
	[Export] public Control stamina;
	[Export] public Control wisdom;
	[Export] public Control charisma;
	[Export] public Control damage;
	[Export] public Control resistance;
	[Export] public Control recovery;

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
}
