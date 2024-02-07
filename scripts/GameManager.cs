using Godot;
using System;

public partial class GameManager : Node3D
{
    [Signal]
    public delegate void DayTimeChangedEventHandler(int time, double temp);
	UI ui;
	public mNotification notification;
	[Export] float time_scale = 1f;
	[Export] float time_scale_step = 0.25f;
	Timer timer;
	public mTime time;
	public float TimeScale {
		get { return time_scale; }
		set {
			time_scale = value;
			timer.WaitTime = 1/time_scale;
		}
	}
	// t * 360 / 1440 (Map time(min) to 360 degrees rotation)
	public Vector4 Lerp(Vector4 origin, Vector4 target, float amount)
	{
		return origin + (target - origin) * amount;
	}

	public override void _Ready()
	{
		time = new mTime(12,0);
		timer = GetNode<Timer>("Timer");
		notification = GetNode<mNotification>("Notification");
		ui = GetNode<UI>("ui");

		timer.WaitTime = 1/time_scale;
		ui.SetTimeLabel(time.ToString());

		timer.Timeout += _TimeChanged;
	}
    public override void _UnhandledKeyInput(InputEvent @event)
    {
		if (Input.MouseMode == Input.MouseModeEnum.Captured) {
			@event = @event as InputEventKey;
			if (@event == null)
				return;
			if (@event.IsActionPressed("ui_left")) {
				TimeScale = TimeScale - time_scale_step;
				notification.CreateNotif("Decreased Time Speed to " + TimeScale);
			}
			if (@event.IsActionPressed("ui_right")) {
				TimeScale = TimeScale + time_scale_step;
				notification.CreateNotif("Increased Time Speed to " + TimeScale);
			}
		}
	}
	public void _TimeChanged()
	{
		time.Increment(1);
		EmitSignal(SignalName.DayTimeChanged, time.T, 0.0);
		ui.SetTimeLabel(time.ToString());
	}
}
