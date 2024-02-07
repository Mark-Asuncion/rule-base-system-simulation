using Godot;
using System;
public partial class Rule: Node3D {
    [Signal]
    public delegate void OnEventHandler();
    [Signal]
    public delegate void OffEventHandler();
    public mTime time_on;
    public mTime time_off;
    private bool status = false;
    public String rule_name;
    bool use_temp = false;
    public double temp_on = 0.0;
    public override void _Ready()
    {
        base._Ready();
        var gm = (GameManager)GetTree().Root.GetChild(0);
        gm.DayTimeChanged += _listener;
    }

    private void _listener(int time, double temp)
    {
        bool is_on = false;
        if (time >= time_on.T)
            is_on = true;
        if (time >= time_off.T)
            is_on = false;

        if (use_temp)
            // change this
            if (is_on && temp_on != temp)
                is_on = false;

        var notif = GetTree().Root.GetChild<GameManager>(0).notification;
        if (is_on) {
            EmitSignal(SignalName.On);
            if (!status)
                notif.CreateNotif(time_on + ":: " + rule_name + " Turned on");

            status = true;
        }
        else {
            EmitSignal(SignalName.Off);
            if (status) {
                status = false;
                notif.CreateNotif(time_off + ":: " + rule_name + " Turned off");
            }
        }
    }

}