using Godot;
using Microsoft.VisualBasic;
using System;

public partial class StatSheetsStat : HBoxContainer
{

	private Button label;
	private Control info;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Button>("Label");
		info = GetNode<Control>("Label/Info");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_label_focus_entered()
	{
		info.Show();
	}
	public void _on_label_focus_exited()
	{
		info.Hide();
	}
}
