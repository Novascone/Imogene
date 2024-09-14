using Godot;
using System;
using System.Collections.Generic;

public partial class Stat : Resource
{
	public Stat (string stat_name, float stat_value)
	{
		name = stat_name;
		base_value = stat_value;
		current_value = base_value;
	}
	public string name { get; set; }
	public float base_value { get; set; } 
	public float current_value { get; set; }
	public float max_value { get; set; }
	
	public List<StatModifier> modifiers = new List<StatModifier>();

	public void AddModifier(StatModifier stat_modifier)
	{
	
		if(!modifiers.Contains(stat_modifier))
		{
			modifiers.Add(stat_modifier);
			stat_modifier.Apply(this);
			GD.Print("added modifier");
		}
	}


    public void RemoveModifier(StatModifier stat_modifier)
	{
		if(modifiers.Contains(stat_modifier))
		{
			modifiers.Remove(stat_modifier);
			stat_modifier.Release(this);
			GD.Print("removed modifier");
		}
		
	}

	
}
