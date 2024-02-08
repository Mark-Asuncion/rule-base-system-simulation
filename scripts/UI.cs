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
	}
	public void SetTimeLabel(String time)
	{
		time_label.Text = time;
	}
}