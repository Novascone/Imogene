using Godot;
using System;

public partial class StatModifier : Resource
{
	public StatModifier(ModificationType modification)
	{
		Modification = modification;
	}
	public enum ModificationType{ None, AddCurrent, AddBase, MultiplyCurrent, MultiplyBase, Nullify}
	public ModificationType Modification { get; set; } = ModificationType.None;
	public float ValueToAdd { get; set; } = 0.0f;
	public float Mod { get; set; } = 0.0f;

	public void Apply(Stat stat_)
	{
		
		if(Modification == ModificationType.AddCurrent)
		{
			if(stat_.CurrentValue + ValueToAdd <= stat_.MaxValue || stat_.MaxValue == 0) // Checks if adding will but the stat over max value
			{
				stat_.CurrentValue += ValueToAdd;
				stat_.RemoveModifier(this); // Removes the modifier from the stat because additions don't need top be kept track of (this might not make sense)
			}
		}
		if(Modification == ModificationType.AddBase)
		{
			if(stat_.BaseValue + ValueToAdd <= stat_.MaxValue || stat_.MaxValue == 0)
			{
				stat_.CurrentValue += ValueToAdd;
			}
		}
		if(Modification == ModificationType.MultiplyCurrent)
		{
			stat_.CurrentValue *= 1 + Mod;
		}
		if(Modification == ModificationType.MultiplyBase)
		{
			
			stat_.BaseValue *= 1 + Mod;
			stat_.CurrentValue *= 1 + Mod;
		}
		if(Modification == ModificationType.Nullify)
		{
			stat_.CurrentValue = 0;
		}
	}
	public void Release(Stat stat_)
	{
		if(Modification == ModificationType.AddBase)
		{
			if(stat_.BaseValue + ValueToAdd <= stat_.MaxValue || stat_.MaxValue == 0)
			{
				stat_.BaseValue -= ValueToAdd;
			}
			
		}
		if(Modification == ModificationType.MultiplyCurrent)
		{
			stat_.CurrentValue /= 1 + Mod;
		}
		if(Modification == ModificationType.Nullify)
		{
			stat_.CurrentValue = stat_.BaseValue;
		}
	}
}
