using Godot;
using System;

public partial class Assignment : Control
{
	[Export] public Button assigned_label;
	[Export] public Button assigned;
	[Export] public Button accept;
	[Export] public Button cancel;
	// Stores information about the ability to be assigned and the ability that it will replace
	public string old_ability_name;
	public string new_ability_name;
	public string old_button_bind;
	public string new_button_bind;
	public Ability.Cross old_cross;
	public Ability.Cross new_cross;
	public Ability.Tier old_tier;
	public Ability.Tier new_tier;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
