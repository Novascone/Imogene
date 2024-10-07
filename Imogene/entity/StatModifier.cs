using Godot;
using System;

public partial class StatModifier : Resource
{
	public StatModifier(ModificationType modification_)
	{
		modification = modification_;
	}
	public enum ModificationType{ None, AddCurrent, AddBase, MultiplyCurrent, MultiplyBase, Nullify}
	public ModificationType modification { get; set; } = ModificationType.None;
	public float value_to_add { get; set; } = 0.0f;
	public float mod { get; set; } = 0.0f;

	public void Apply(Stat stat_)
	{
		
		if(modification == ModificationType.AddCurrent)
		{
			if(stat_.current_value + value_to_add <= stat_.max_value || stat_.max_value == 0) // Checks if adding will but the stat over max value
			{
				stat_.current_value += value_to_add;
				stat_.RemoveModifier(this); // Removes the modifier from the stat because additions don't need top be kept track of (this might not make sense)
			}
		}
		if(modification == ModificationType.AddBase)
		{
			if(stat_.base_value + value_to_add <= stat_.max_value || stat_.max_value == 0)
			{
				stat_.current_value += value_to_add;
			}
		}
		if(modification == ModificationType.MultiplyCurrent)
		{
			stat_.current_value *= 1 + mod;
		}
		if(modification == ModificationType.MultiplyBase)
		{
			
			stat_.base_value *= 1 + mod;
			stat_.current_value *= 1 + mod;
		}
		if(modification == ModificationType.Nullify)
		{
			stat_.current_value = 0;
		}
	}
	public void Release(Stat stat_)
	{
		if(modification == ModificationType.AddBase)
		{
			if(stat_.base_value + value_to_add <= stat_.max_value || stat_.max_value == 0)
			{
				stat_.base_value -= value_to_add;
			}
			
		}
		if(modification == ModificationType.MultiplyCurrent)
		{
			stat_.current_value /= 1 + mod;
		}
		if(modification == ModificationType.Nullify)
		{
			stat_.current_value = stat_.base_value;
		}
	}
}
