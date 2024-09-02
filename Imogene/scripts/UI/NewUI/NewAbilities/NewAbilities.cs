using Godot;
using System;

public partial class NewAbilities : Control
{
	[Export] public Binds binds;
	[Export] public Categories categories;
	[Export] public Passives passives;
	public CrossBindButton button_selected;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Control control in binds.l_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
				
			}
		}

		foreach(Control control in binds.l_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}

		foreach(Control control in binds.r_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}

		foreach(Control control in binds.r_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind)
			{
				cross_bind.CrossButtonPressed += OnCrossBindDown;
			}
		}
		// foreach(Button cross_bind in binds.l_cross_secondary_assignment.GetChildren())
		// {
		// 	cross_bind.ButtonDown += OnCrossBindDown;
		// }
	}

    private void OnCrossBindDown(CrossBindButton cross_button)
    {
		button_selected = cross_button;
		binds.Hide();
		categories.Show();
		GD.Print("Button selected " + button_selected.button_bind, " on side " + button_selected.side + " at level " + button_selected.level);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
