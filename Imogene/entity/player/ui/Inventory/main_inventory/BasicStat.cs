using Godot;
using Microsoft.VisualBasic;
using System;

public partial class BasicStat : HBoxContainer
{
	private Button label { get; set; } = null;
	private Label value { get; set; } = null;
	private Control info { get; set; } = null;
	private RichTextLabel info_text  { get; set; } = null;
	public string set_info_label_text  { get; set;} = "";
	private string stat_number = "1";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Button>("Label");
		value = GetNode<Label>("Value");
		value.Text = "20";
		info = GetNode<Control>("Label/Info");
		info_text = GetNode<RichTextLabel>("Label/Info/MarginContainer/PanelContainer/Label");
		set_info_label_text = info_text.Text;
	}

	
	public void GetStatInfo(string stat_value_)
	{
		stat_number = stat_value_;
		value.Text = stat_value_;
		info_text.Text = string.Format(set_info_label_text, stat_number, 0, stat_number, stat_number);
	}

	public void _on_label_focus_entered()
	{
		info.Show();
	}
	public void _on_label_focus_exited()
	{
		info.Hide();
	}

	public override void _GuiInput(InputEvent @event_)
	{
		if(@event_ is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
		}
		
	}
}
