using Godot;
using System;

public partial class Stat : Control
{

    public Button label;
	public Label value;
	public Control info;
	public RichTextLabel info_text;
	public string set_info_text;
	public string stat_value = "1";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        label = GetNode<Button>("Label");
		value = GetNode<Label>("Value");
		value.Text = "20";
		info = GetNode<Control>("Label/Info");
		info_text = GetNode<RichTextLabel>("Label/Info/MarginContainer/PanelContainer/Label");
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
			if(eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
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
