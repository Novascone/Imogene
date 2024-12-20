using Godot;
using System;

public partial class DamageNumber3D : Node3D
{

	private Label Label;
	private Sprite3D Sprite3D;
	private AnimationPlayer AnimationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label = GetNode<Label>("SubViewport/Label");
		Sprite3D = GetNode<Sprite3D>("Sprite3D");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetValuesAndAnimate(float value, bool critical, Vector3 startPos, float height, float spread)
	{
		Label.Text = value.ToString();
		if(critical)
		{
			Label.AddThemeColorOverride("font_color", new Color(Colors.Yellow, 1.0f));
			// GD.Print("Is critical from DamageNumber3D");
		}
		else
		{
			Label.AddThemeColorOverride("font_color", new Color(Colors.White, 1.0f));
			// GD.Print("Is not critical from DamageNumber3D");
		}
		AnimationPlayer.Play("RiseAndFade");

		Tween tween = GetTree().CreateTween();
		var random = new RandomNumberGenerator();
		random.Randomize();
		Vector3 end_pos = new Vector3(random.RandfRange(-spread,spread) / 100, height / 100, random.RandfRange(-spread,spread)/ 100) + startPos;
		float tween_length = AnimationPlayer.GetAnimation("RiseAndFade").Length;

		tween.TweenProperty(this, "position", end_pos, tween_length).From(startPos);

	}

	public void Remove()
	{
		AnimationPlayer.Stop();
		if(IsInsideTree())
		{
			GetParent().RemoveChild(this);
		}
	}
}
