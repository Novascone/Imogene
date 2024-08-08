using Godot;
using System;

public partial class Dissolve : Node3D
{
	public void _on_area_3d_body_entered(Node3D body)
	{
		if(body is Player player)
		{
			GD.Print("player entered");
			var tween = CreateTween().SetEase(Tween.EaseType.In);
			Callable call_dissolve = new Callable(this, MethodName.SetDissolve);
			tween.TweenMethod(call_dissolve, 1.0, 0.0,0.7);
		}
		
	}

	public void _on_area_3d_body_exited(Node3D body)
	{
		if(body is Player player)
		{
			var tween = CreateTween().SetEase(Tween.EaseType.Out);
			Callable call_dissolve = new Callable(this, MethodName.SetDissolve);
			tween.TweenMethod(call_dissolve, 0.0, 1.0 ,0.7);
		}
		
	}

	public void  SetDissolve(float dissolve)
	{
		foreach(MeshInstance3D roof_tile in GetChildren())
		{
			((ShaderMaterial)roof_tile.GetSurfaceOverrideMaterial(0)).SetShaderParameter("opacity", dissolve);
		}
	}
}
