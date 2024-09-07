using Godot;
using System;

// 																********************************************** MARKED FOR REWORK ************************************************************

public partial class DamageNumbers : Node3D
{	
	private Label3D label;
	private Node3D label_container;
	private AnimationPlayer animationPlayer;
	private Vector3 position;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label3D>("Label3D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetValueAnimate(String value, Vector3 start_pos, int height, int spread)
	{
		label.Text = value;
		GlobalPosition = position;
		animationPlayer.Play("Rise_and_Fade");
		
		Tween tween = GetTree().CreateTween();
		Random randx = new Random();
		Random randz = new Random();
		int x = randx.Next(-spread,spread);
		int z = randz.Next(-spread,spread);
		Vector3 end_pos;
		end_pos.X = x;
		end_pos.Y = height;
		end_pos.Z = z;

		float tween_length = animationPlayer.GetAnimation("Rise_and_Fade").Length;

		tween.TweenProperty(label, "GlobalPosition", end_pos,tween_length).From(start_pos);

	}

	public void Remove()
	{
		animationPlayer.Stop();
		if (IsInsideTree())
		{
			GetParent().RemoveChild(label_container);
		}
	}
}
