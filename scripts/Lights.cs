using Godot;
using System;

public partial class Lights : Node3D
{
	Rule rule;
	private OmniLight3D spotlight;
	[Export] private float energy = 1;
	public override void _Ready()
	{
		rule = GetNode<Rule>("Rule");
		spotlight = GetNode<OmniLight3D>("LightNode");
		rule.rule_name = "Lights";
		// rule.time_on = new mTime(18,00);
		// rule.time_off = new mTime(22,00);

		rule.time_on = new mTime(12,30);
		rule.time_off = new mTime(22,00);

		rule.On += _on;
		rule.Off += _off;
	}

	public void _on()
	{
		spotlight.LightEnergy = energy;
	}

	public void _off()
	{
		spotlight.LightEnergy = 0;
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
