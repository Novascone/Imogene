using Godot;
using System;

public partial class InventoryButton : Button
{
	public Item inventory_item;
	public TextureRect icon;
	public Label quantity_label;
	private int index;
	public Control info;
	public bool focused;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon = GetNode<TextureRect>("TextureRect");
		quantity_label = GetNode<Label>("Label");
		info = GetNode<Control>("Info");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(focused && inventory_item != null && Input.IsActionJustPressed("Interact"))
		{
			InventoryButtonPressed();
		}
	}

	public void UpdateItem(Item item, int index)
	{
		this.index = index;
		inventory_item = item;
		if(item == null)
		{
			icon.Texture = null;
			quantity_label.Text = string.Empty;
		}
		else
		{
			icon.Texture = item.icon;
			quantity_label.Text = item.quantity.ToString();
		}
		
	}


	 public void InventoryButtonPressed()
    {
		
		if(inventory_item != null)
		{
			inventory_item.UseItem();
			GD.Print("button pressed");
		}
			
    }

	public void _on_focus_entered()
	{
		info.Show();
		focused = true;
	}

	public void _on_focus_exited()
	{
		info.Hide();
		focused = false;
	}

}
