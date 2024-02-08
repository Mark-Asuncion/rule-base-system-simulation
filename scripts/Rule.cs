using Godot;
using System;
public partial class Rule: Node3D {
    public enum Type {
        Default,
        CustomOff
    }
    [Signal]
    public delegate void OnEventHandler();
    [Signal]
    public delegate void OffEventHandler();
    public Type type = Type.Default;
    public Func<GameManager, bool> IsOn = ctx => false;
    private bool status = false;
    public String rule_name = "";
    public String notif_prefix = "";
    public override void _Ready()
    {
        base._Ready();
        var gm = (GameManager)GetTree().Root.GetChild(0);
        gm.DayTimeChanged += _listener;
    }
    public void OEmitSignal(bool on)
    {
        var gm = (GameManager)GetTree().Root.GetChild(0);
        var notif = gm.notification;
        String prefix = (notif_prefix == "") ? gm.time.ToString() : notif_prefix;
        if (on) {
            EmitSignal(SignalName.On);
            notif.CreateNotif(prefix + ":: " + rule_name + " Turned on");
        }
        else {
            EmitSignal(SignalName.Off);
            notif.CreateNotif(prefix + ":: " + rule_name + " Turned off");
        }
    }
    private void _listener(GameManager context)
    {
        var notif = context.notification;
        String prefix = (notif_prefix == "") ? context.time.ToString() : notif_prefix;
        if (IsOn(context)) {
            EmitSignal(SignalName.On);
            if (!status)
                notif.CreateNotif(prefix + ":: " + rule_name + " Turned on");
            status = true;
        }
        else {
            if (type == Type.CustomOff)
                return;
            EmitSignal(SignalName.Off);
            if (status) {
                status = false;
                notif.CreateNotif(prefix + ":: " + rule_name + " Turned off");
            }
        }
    }
}