using Godot;
using System;

public partial class Thermostat : Node3D
{
	[Export] private float min = 15f;
	[Export] private float target = 35f;
	[Export] private float max = 37f;
	[Export] private float max_step = 3f;
	Rule rule;
	Label3D label;
	bool is_on = false;
	GameManager gm;
	public override void _Ready()
	{
		label = GetNode<Label3D>("Label");
		rule = GetNode<Rule>("Rule");
		gm = GetTree().Root.GetChild<GameManager>(0);
		gm.TempChanged += (GameManager ctx) => {
			label.Text = ctx.temp.ToString();
		};
		rule.notif_prefix = "Thermostat";
		rule.notif_on_message = "Trying to Maintain " + new mTemp(target).ToString();
		rule.notif_off_message = rule.notif_on_message;
		rule.IsOn = (GameManager ctx) => {
			float temp = gm.temp.T;
			return temp < min || temp > max;
		};
		rule.On += () => is_on = true;
		rule.Off += () => is_on = false;
	}
    public override void _Process(double delta)
    {
        if (is_on) {
			float temp = gm.temp.T;
			gm.temp.T = Mathf.Min(max_step, temp - target);
		}
    }
}
