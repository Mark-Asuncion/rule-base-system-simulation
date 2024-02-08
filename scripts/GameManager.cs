using Godot;
using System;

public partial class GameManager : Node3D
{
    [Signal]
    public delegate void DayTimeChangedEventHandler(GameManager context);
	[Signal]
	public delegate void PauseEventHandler(bool pause);
	UI ui;
	public mNotification notification;
	[Export] float time_scale = 1f;
	[Export] float time_scale_step = 1f;
	Timer timer;
	public mTime time;
	public float TimeScale {
		get { return time_scale; }
		set {
			time_scale = value;
			timer.WaitTime = 1/time_scale;
		}
	}
	private WorldEnvironment env;
	// t * 360 / 1440 (Map time(min) to 360 degrees rotation)
	public Vector4 Lerp(Vector4 origin, Vector4 target, float amount)
	{
		return origin + (target - origin) * amount;
	}

	public override void _Ready()
	{
		time = new mTime(0,0);
		timer = GetNode<Timer>("Timer");
		env = GetNode<WorldEnvironment>("WorldEnvironment");
		notification = GetNode<mNotification>("Notification");
		ui = GetNode<UI>("ui");

		timer.WaitTime = 1/time_scale;
		ui.SetTimeLabel(time.ToString());

		timer.Timeout += _TimeChanged;
		_TimeChanged();
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public void _on_animation_end(String anim_name)
	{
		timer.Start();
		EmitSignal(SignalName.Pause, false);
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
	private float NormalizeTime(int min)
	{
		return (min * 360f / 1440f) / 360f;
	}
	public void _TimeChanged()
	{
		time.Increment(1);
		EmitSignal(SignalName.DayTimeChanged, this);
		ui.SetTimeLabel(time.ToString());
		var sky_mat = env.Environment.Sky.SkyMaterial as ShaderMaterial;
		float norm_time = NormalizeTime(time.T);
		sky_mat.SetShaderParameter("ratio", norm_time);
		var light = env.GetChild<DirectionalLight3D>(0);
		if (norm_time >= 0.3 && norm_time < 0.7) {
			norm_time = 1f;
		}
		else if (norm_time >= 0f || norm_time >= 0.7) {
			norm_time = 0.1f;
		}
		light.LightEnergy = norm_time;
	}
}
