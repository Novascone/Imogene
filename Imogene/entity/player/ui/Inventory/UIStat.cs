using Godot;
using System;
using System.Text;

public partial class UIStat : Control
{

	[Export] public string stat_name;
	[Export] public StatInfo info;
    [Export] public Button label;
	[Export] public Area2D area;
	[Export] public Label value;
	
	public string stat_value = "1";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = SeparateByCapitals(Name);
		GD.Print(Name + "Subscribed to area entered");
       	area.AreaEntered += _on_area_2d_area_entered;
	   	area.AreaExited += _on_area_2d_area_exited;
	}

    public virtual void GetStatInfo(float stat_value_ui)
	{
		stat_value = stat_value_ui.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value);
		//physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power_UI);
	}
	public virtual void GetStatInfo(float stat_value_ui, float p_m_dps, float s_m_dps, float p_r_dps, float s_r_dps)
	{
		
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

	public string SeparateByCapitals(string name)
	{
		StringBuilder new_name = new StringBuilder(name);
		
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
