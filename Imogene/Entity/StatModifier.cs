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

	public void Apply(Stat stat)
	{
		
		if(Modification == ModificationType.AddCurrent)
		{
			if(stat.CurrentValue + ValueToAdd <= stat.MaxValue || stat.MaxValue == 0) // Checks if adding will but the stat over max value
			{
				stat.CurrentValue += ValueToAdd;
				stat.RemoveModifier(this); // Removes the modifier from the stat because additions don't need top be kept track of (this might not make sense)
			}
		}
		if(Modification == ModificationType.AddBase)
		{
			if(stat.BaseValue + ValueToAdd <= stat.MaxValue || stat.MaxValue == 0)
			{
				stat.CurrentValue += ValueToAdd;
			}
		}
		if(Modification == ModificationType.MultiplyCurrent)
		{
			stat.CurrentValue *= 1 + Mod;
		}
		if(Modification == ModificationType.MultiplyBase)
		{
			
			stat.BaseValue *= 1 + Mod;
			stat.CurrentValue *= 1 + Mod;
		}
		if(Modification == ModificationType.Nullify)
		{
			stat.CurrentValue = 0;
		}
	}
	public void Release(Stat stat)
	{
		if(Modification == ModificationType.AddBase)
		{
			if(stat.BaseValue + ValueToAdd <= stat.MaxValue || stat.MaxValue == 0)
			{
				stat.BaseValue -= ValueToAdd;
			}
			
		}
		if(Modification == ModificationType.MultiplyCurrent)
		{
			stat.CurrentValue /= 1 + Mod;
		}
		if(Modification == ModificationType.Nullify)
		{
			stat.CurrentValue = stat.BaseValue;
		}
	}
}
