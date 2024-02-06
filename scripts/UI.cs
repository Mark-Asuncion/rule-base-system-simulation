using Godot;
using System;

public partial class UI : CanvasLayer
{
	Label time_label;
	Label temp_label;
	Button override_button;
	Panel override_popup;
	public override void _Ready()
	{
		time_label = GetNode<Label>("Control/TimeFrame/TimeLabel");
		temp_label = GetNode<Label>("Control/TimeFrame/TempLabel");
		override_button = GetNode<Button>("Control/MarginContainer/OverrideButton");
		override_popup = override_button.GetChild<Panel>(0);
		override_popup.Visible = false;
		override_button.Pressed += PopupToggle;
	}
	public void PopupToggle()
	{
		override_popup.Visible = !override_popup.Visible;
	}
	public void SetTimeLabel(String time)
	{
		time_label.Text = time;
	}
}