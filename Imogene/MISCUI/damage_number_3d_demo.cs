using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class damage_number_3d_demo : Node3D
{

	[Export] private Node3D spawn_point;
	[Export] private Label spread_value;
	[Export] private Label height_value;
	[Export] PackedScene damage_number_3d_template;
	private Queue<DamageNumber3D> damage_number_3d_pool = new Queue<DamageNumber3D>();
	private bool rmb_held;

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(rmb_held)
		{
			var random = new RandomNumberGenerator();
			random.Randomize();
			SpawnDamageNumber(Mathf.Round(random.RandfRange(0,10)));
		}
		
	}

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("one"))
		{
			var random = new RandomNumberGenerator();
			random.Randomize();
			SpawnDamageNumber(Mathf.Round(random.RandfRange(0,100)));
		}
		if(@event.IsActionPressed("two"))
		{
			rmb_held = true;
		}
		if(@event.IsActionReleased("two"))
		{
			rmb_held = false;
		}
    }

	public void SpawnDamageNumber(float value)
	{
		DamageNumber3D damage_number = GetDamageNumber();
		Vector3 position = spawn_point.GlobalTransform.Origin;
		float height = height_value.Text.ToFloat();
		float spread = spread_value.Text.ToFloat();
		bool is_crit;
		float random_number = GD.Randf();
		if(random_number > 0.5f)
		{
			is_crit = true;
		}
		else
		{
			is_crit = false;
		}
		AddChild(damage_number, true);
		damage_number.SetValuesAndAnimate(value, is_crit, position, height, spread);
	}

	public DamageNumber3D GetDamageNumber()
	{
		if(damage_number_3d_pool.Count > 0)
		{
			return damage_number_3d_pool.Dequeue();
		}

		else
		{
			DamageNumber3D new_damage_number = (DamageNumber3D)damage_number_3d_template.Instantiate();
			new_damage_number.TreeExiting += () => damage_number_3d_pool.Enqueue(new_damage_number);
			return new_damage_number;
		}
	}

	public void _on_spread_value_changed(float value)
	{
		spread_value.Text = value.ToString();
	}

	public void _on_height_value_changed(float value)
	{
		height_value.Text = value.ToString();
	}


}
