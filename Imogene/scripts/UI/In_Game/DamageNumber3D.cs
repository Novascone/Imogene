using Godot;
using System;

public partial class DamageNumber3D : Node3D
{

	private Label label;
	private Sprite3D sprite3D;
	private AnimationPlayer animation_player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("SubViewport/Label");
		sprite3D = GetNode<Sprite3D>("Sprite3D");
		animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetValuesAndAnimate(float value, Vector3 start_pos, float height, float spread)
	{
		label.Text = value.ToString();
		animation_player.Play("RiseAndFade");

		Tween tween = GetTree().CreateTween();
		var random = new RandomNumberGenerator();
		random.Randomize();
		Vector3 end_pos = new Vector3(random.RandfRange(-spread,spread) / 100, height / 100, random.RandfRange(-spread,spread)/ 100) + start_pos;
		float tween_length = animation_player.GetAnimation("RiseAndFade").Length;

		tween.TweenProperty(this, "position", end_pos, tween_length).From(start_pos);

	}

	public void Remove()
	{
		animation_player.Stop();
		if(IsInsideTree())
		{
			GetParent().RemoveChild(this);
		}
	}
}
