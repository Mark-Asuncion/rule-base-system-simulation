using Godot;
using System;

public partial class mNotification : CanvasLayer
{
	private VBoxContainer container;
	private PackedScene label;
	public override void _Ready()
	{
		container = GetNode<VBoxContainer>("Control/MarginContainer/VBoxContainer");
		label = ResourceLoader.Load<PackedScene>("res://scene/notif_label.tscn");
	}

	public void CreateNotif(String text, float time_to_destroy = 3f)
	{
		Label l = label.Instantiate<Label>();
		l.Text = text;
		container.AddChild(l);

		GetTree().CreateTimer(time_to_destroy).Timeout += () => {
			container.RemoveChild(l);
			l.QueueFree();
		};
	}
}
