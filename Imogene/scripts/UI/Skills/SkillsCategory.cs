using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkillsCategory : PanelContainer
{
	private GridContainer skills_container;
	private HBoxContainer skills_modifier_container;
	private List<SkillsButton> skills = new List<SkillsButton>();
	private List<Button> modifiers = new List<Button>();
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		skills_container = GetNode<GridContainer>("VBoxContainer/PanelContainer/GridContainer");
		skills_modifier_container = GetNode<HBoxContainer>("VBoxContainer/PanelContainer/ModifierTreeContainer/Modifier/VBoxContainer/SkillModifiers");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

		_customSignals.AddToSkillsSelection += HandleAddToSkillsSelection;

		foreach(SkillsButton skill in skills_container.GetChildren().Cast<SkillsButton>())
		{
			skills.Add(skill);
		}
		// GD.Print(skills.Count);

		foreach(SkillsButton modifier in skills_modifier_container.GetChildren().Cast<SkillsButton>())
		{
			modifiers.Add(modifier);
		}
		
	}

    private void HandleAddToSkillsSelection(string ability, string type, Texture2D icon)
    {
		if(this.IsInGroup(type))
		{
			AddSkill(ability, icon);
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void AddSkill(string ability, Texture2D icon)
	{
		foreach(SkillsButton skill in skills)
		{
			if(!skill.assigned)
			{
				// button.Text = ability;
				skill.Icon = icon;
				skill.assigned = true;
				break;
			}
		}
	}

}
