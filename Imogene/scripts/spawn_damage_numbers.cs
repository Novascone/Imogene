using Godot;
using System;
using System.Collections.Generic;

public partial class spawn_damage_numbers : Control
{
	// Called when the node enters the scene tree for the first time.
	private Control spawn_point;
	private Label3D spread_value;
	private Label3D height_value;
	private bool rmb_held = false;
	
	
	Queue<damage_numbers> damage_number_3d_pool ;
	

	public override void _Ready()
	{
		spawn_point = GetNode<Control>("SpawnPoint");
		spread_value = GetNode<Label3D>("VBoxContainer/HBoxContainer/SpreadValue");
		height_value = GetNode<Label3D>("VBoxContainer/HBoxContainer2/HeightValue");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (rmb_held)
		{
			Random rand = new Random();
			spawn_damage_number(rand.Next(0,10));
			GD.Print("here");
		}
	}

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("RightMouse"))
		{
			
		}
    }

	public void spawn_damage_number(float value)
	{
		var damage_number = GetDamageNumber();
		var val = Convert.ToString(Mathf.Round(value));
		var height = height_value.Text.ToInt();
		Vector3 pos;
		pos.X = spawn_point.Position.X;
		pos.Y = height;
		pos.Z = spawn_point.Position.Y;
		var spread = spread_value.Text.ToInt();
		AddChild(damage_number, true);
		damage_number.SetValueAnimate(val, pos, height, spread);

	}

	public damage_numbers GetDamageNumber()
	{
		var damage_number_3D_template = GD.Load<PackedScene>("res://scenes/damage_number_3d.tscn");
		if( damage_number_3d_pool.Count > 0)
		{
			return damage_number_3d_pool.Dequeue();
		}

		else
		{
			var new_damage_number = damage_number_3D_template.Instantiate<damage_numbers>();
			new_damage_number.TreeExiting += () => damage_number_3d_pool.Enqueue(new_damage_number);
			return new_damage_number;
		}

	}
	
	public void _on_spread_value_changed(float value)
	{
		spread_value.Text = Convert.ToString(value);
	}

	public void _on_height_value_changed(float value)
	{
		height_value.Text = Convert.ToString(value);
	}

}
