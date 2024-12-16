using Godot;
using System;
using System.Text;

public partial class UIStat : Control
{

	[Export] public string stat_name { get; set; }
	[Export] public StatInfo info { get; set; }
    [Export] public Button label { get; set; }
	[Export] public Area2D area { get; set; }
	[Export] public Label value { get; set; }
	
	public string stat_value  { get; set; } = "1";
	// Called when the node enters the scene tree for the first time.

	public override void _GuiInput(InputEvent @event_)
	{
		if(@event_ is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}
	public override void _Ready()
	{
		label.Text = SeparateByCapitals(Name);
       	area.AreaEntered += _on_area_2d_area_entered;
	   	area.AreaExited += _on_area_2d_area_exited;
	}

    public virtual void GetStatInfo(float stat_value_ui_)
	{
		stat_value = stat_value_ui_.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value);
		//physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power_UI);
	}
	public virtual void GetStatInfo(float stat_value_ui, float p_m_dps, float s_m_dps, float p_r_dps, float s_r_dps)
	{
		
	}

	public string SeparateByCapitals(string name_)
	{
		StringBuilder new_name = new StringBuilder(name_);
		
		for(int i = 1; i < new_name.Length; i++)
		{
			if(char.IsUpper(new_name[i]))
			{
				new_name.Insert(i, " ");
				i++;
			}
		}

		return new_name.ToString();
	}
	public void _on_area_2d_area_entered(Area2D area_)
	{
		
		if(area_.IsInGroup("cursor"))
		{
			label.GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area_)
	{
		
		if(area_.IsInGroup("cursor"))
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
