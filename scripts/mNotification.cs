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

	public void CreateNotif(String text)
	{
		Label l = label.Instantiate<Label>();
		l.Text = text;
		container.AddChild(l);

		GetTree().CreateTimer(3f).Timeout += () => {
			container.RemoveChild(l);
			l.QueueFree();
		};
	}
}
