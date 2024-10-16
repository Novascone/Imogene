using Godot;
using System;

public partial class Assignment : Control
{
	[Export] public Button assigned_label { get; set; }
	[Export] public Button assigned { get; set; }
	[Export] public Button accept { get; set; }
	[Export] public Button cancel { get; set; }
	public string old_ability_name { get; set; } = "";
	public string new_ability_name { get; set; } = "";
	public string old_button_bind { get; set; } = "";
	public string new_button_bind { get; set; } = "";
	public Ability.Cross old_cross { get; set; } = Ability.Cross.None;
	public Ability.Cross new_cross { get; set; } = Ability.Cross.None;
	public Ability.Tier old_tier { get; set; } = Ability.Tier.None;
	public Ability.Tier new_tier { get; set; } = Ability.Tier.None;
	
}
