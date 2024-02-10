using Godot;
using System;

public partial class SkyBox : WorldEnvironment
{
	public mTime time;
	private float NormalizeTime(int min)
	{
		return (min * 360f / 1440f) / 360f;
	}
	public override void _Ready()
	{
		var gm = GetParent<GameManager>();
	}
	public void _timeout()
	{
		var sky_mat = Environment.Sky.SkyMaterial as ShaderMaterial;
		float norm_time = NormalizeTime(time.T);
		sky_mat.SetShaderParameter("ratio", norm_time);
		var gradient = ((GradientTexture1D) sky_mat.GetShaderParameter("gradient")).Gradient;
		var light = GetChild<DirectionalLight3D>(0);
		Color gradient_color = gradient.Sample(norm_time);
		float energy = ( gradient_color.R + gradient_color.G + gradient_color.B ) / 3;
		light.LightEnergy = energy;
		// GD.PrintS("energy", energy);
	}
}
