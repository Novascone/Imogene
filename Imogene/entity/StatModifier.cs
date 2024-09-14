using Godot;
using System;

public partial class StatModifier : Resource
{
	public StatModifier(ModificationType modification)
	{
		modification_type = modification;
	}
	public enum ModificationType{ Add, Multiply }
	public ModificationType modification_type;
	public float value_to_add;
	public float mod;
	public void Apply(Stat stat)
	{
		if(modification_type == ModificationType.Add)
		{
			if(stat.current_value + value_to_add <= stat.max_value || stat.max_value == 0)
			{
				stat.current_value += value_to_add;
			}
			
		}
		if(modification_type == ModificationType.Multiply)
		{
			stat.current_value *= 1 + mod;
		}
	}
	public void Release(Stat stat)
	{
		if(modification_type == ModificationType.Add)
		{
			if(stat.current_value + value_to_add <= stat.max_value || stat.max_value == 0)
			{
				stat.current_value -= value_to_add;
			}
			
		}
		if(modification_type == ModificationType.Multiply)
		{
			stat.current_value /= 1 + mod;
		}
	}
}
