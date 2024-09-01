using Godot;
using System;

public partial class Categories : Control
{

	[Export] public Control class_category;
	[Export] public Control general_category;
	[Export] public Label ability_type_title;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
 