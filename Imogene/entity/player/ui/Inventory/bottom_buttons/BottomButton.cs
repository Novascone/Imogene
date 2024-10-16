using Godot;
using System;

public partial class BottomButton : Button
{
	[Export] public Control info { get; set; }
	[Export] public RichTextLabel info_text { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = Name;
		info_text.Text = Name;
	}

	public void _on_focus_entered()
	{
		info.Show();
	}
	public void _on_focus_exited()
	{
		info.Hide();
	}

	public void _on_area_2d_area_entered(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}
}
