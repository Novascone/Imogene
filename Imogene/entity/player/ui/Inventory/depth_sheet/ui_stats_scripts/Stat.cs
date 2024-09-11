using Godot;
using System;

public partial class Stat : Control
{

    [Export] public Button label;
	[Export] public Label value;
	[Export] public StatInfo info;
	public string stat_value = "1";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
       
	}

    public void GetStatInfo(string stat_value_ui)
	{
		stat_value = stat_value_ui;
		value.Text = stat_value;
		// info.info.Text = string.Format(info.info.Text, stat_value);
		//physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power_UI);
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
		info.tool_tip_container.Show();
	}
	public void _on_label_focus_exited()
	{
		info.tool_tip_container.Hide();
	}

	
}
