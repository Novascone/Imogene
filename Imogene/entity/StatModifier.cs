using Godot;
using System;

public partial class StatModifier : Resource
{
	public StatModifier(ModificationType modification)
	{
		modification_type = modification;
	}
	public enum ModificationType{ add_current, add_base, multiply_current, multiply_base }
	public ModificationType modification_type;
	public Stat stat;
	public float value_to_add;
	public float mod;
	public void Apply(Stat stat)
	{
		
		if(modification_type == ModificationType.add_current)
		{
			if(stat.current_value + value_to_add <= stat.max_value || stat.max_value == 0)
			{
				stat.current_value += value_to_add;
				stat.RemoveModifier(this);
			}
		}
		if(modification_type == ModificationType.add_base)
		{
			if(stat.base_value + value_to_add <= stat.max_value || stat.max_value == 0)
			{
				stat.current_value += value_to_add;
			}
		}
		if(modification_type == ModificationType.multiply_current)
		{
			
			stat.current_value *= 1 + mod;

		}
		if(modification_type == ModificationType.multiply_base)
		{
			
			stat.base_value *= 1 + mod;
			stat.current_value *= 1 + mod;
		}
	}
	public void Release(Stat stat)
	{
		if(modification_type == ModificationType.add_base)
		{
			if(stat.base_value + value_to_add <= stat.max_value || stat.max_value == 0)
			{
				stat.base_value -= value_to_add;
			}
			
		}
		if(modification_type == ModificationType.multiply_current)
		{
			
			stat.current_value /= 1 + mod;
		}
	}
}
