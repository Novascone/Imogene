using Godot;
using System;

public partial class Journal : Control
{
	public JournalTab quest_tab;
	public JournalTab bestiary_tab;
	public JournalTab lore_tab;
	public JournalTab tutorial_tab;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		quest_tab = GetNode<JournalTab>("PanelContainer/QuestsTab");
		bestiary_tab = GetNode<JournalTab>("PanelContainer/BestiaryTab");
		lore_tab = GetNode<JournalTab>("PanelContainer/LoreTab");
		tutorial_tab = GetNode<JournalTab>("PanelContainer/TutorialsTab");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_quest_button_down()
	{
		quest_tab.text.Show();
		bestiary_tab.text.Hide();
		lore_tab.text.Hide();
		tutorial_tab.text.Hide();
	}

	public void _on_bestiary_button_down()
	{
		quest_tab.text.Hide();
		bestiary_tab.text.Show();
		lore_tab.text.Hide();
		tutorial_tab.text.Hide();

	}

	public void _on_lore_button_down()
	{
		quest_tab.text.Hide();
		bestiary_tab.text.Hide();
		lore_tab.text.Show();
		tutorial_tab.text.Hide();
		
	}

	public void _on_tutorials_button_down()
	{
		quest_tab.text.Hide();
		bestiary_tab.text.Hide();
		lore_tab.text.Hide();
		tutorial_tab.text.Show();
	}
}
