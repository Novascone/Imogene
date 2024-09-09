using Godot;
using System;

public partial class Stat : Control
{

    [Export] public Button label;
	[Export] public Label value;
	[Export] public Control info;
	[Export] public RichTextLabel info_text;
	public string set_info_text;
	public string stat_value = "1";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
       
	}

    public virtual void GetStatInfo(string stat_value_ui)
	{
		// stat_number = stat_value;
		// value.Text = stat_value;
		// GD.Print("stat number " + stat_number);
		// info_text.Text = string.Format(set_info_text, stat_number, 0, stat_number, stat_number);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

   	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}
	public void _on_area_2d_area_entered(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			label.GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			label.ReleaseFocus();
		}
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
