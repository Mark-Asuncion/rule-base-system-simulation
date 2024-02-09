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
    public bool Status {
        get { return status; }
    }
    public String name = "";
    public String notif_prefix = "";
    public String notif_on_message = "Turned On";
    public String notif_off_message = "Turned Off";
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
            notif.CreateNotif(prefix + ":: " + name + " " + notif_on_message);
            status = true;
        }
        else {
            EmitSignal(SignalName.Off);
            notif.CreateNotif(prefix + ":: " + name + " " + notif_off_message);
            status = false;
        }
    }
    private void _listener(GameManager context)
    {
        var notif = context.notification;
        String prefix = (notif_prefix == "") ? context.time.ToString() : notif_prefix;
        if (IsOn(context)) {
            if (type == Type.CustomOff)
                EmitSignal(SignalName.On);
            if (!status) {
                EmitSignal(SignalName.On);
                notif.CreateNotif(prefix + ":: " + name + " " + notif_on_message);
            }
            status = true;
        }
        else {
            if (type == Type.CustomOff)
                return;
            if (status) {
                EmitSignal(SignalName.Off);
                status = false;
                notif.CreateNotif(prefix + ":: " + name + " " + notif_off_message);
            }
        }
    }
}