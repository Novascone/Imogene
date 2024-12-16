using Godot;
using System;

public partial class GenericInventoryButton : Button
{
	[Export] public StatInfo info { get; set; }
	[Export] public RichTextLabel info_text { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = Name;
	}

	public void _on_area_2d_area_entered(Area2D area_)
	{
		if (area_.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area_)
	{
		if (area_.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}

	public void _on_button_down()
	{
		AcceptEvent();
	}

	public void _on_focus_entered()
	{
		info.tool_tip_container.Show();
	}

	public void _on_focus_exited()
	{
		info.tool_tip_container.Hide();
	}
}
