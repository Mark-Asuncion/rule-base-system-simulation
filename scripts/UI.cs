using Godot;
using System;

public partial class UI : CanvasLayer
{
	Label time_label;
	Label temp_label;
	public override void _Ready()
	{
		time_label = GetNode<Label>("Control/TimeFrame/TimeLabel");
		temp_label = GetNode<Label>("Control/TimeFrame/TempLabel");
		var gm = GetTree().Root.GetChild<GameManager>(0);
		gm.DayTimeChanged += (GameManager ctx) => {
			time_label.Text = ctx.time.ToString();
		};
		gm.TempChanged += (GameManager ctx) => {
			temp_label.Text = ctx.temp.ToString();
		};
	}
}