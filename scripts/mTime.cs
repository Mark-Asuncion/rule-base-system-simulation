using System;
using System.ComponentModel.DataAnnotations;
using Godot;
using Microsoft.VisualBasic;
public class mTime
{
    private int t;
    public int T {
        get { return t; }
    }

    public mTime(int hour, int min)
    {
        t = (hour*60) + min;
        while (t >= 1440) {
            int r = t - 1440;
            t = r;
        }
    }
    public override String ToString()
    {
        double hour = (t / 60.0) % 12;
        int hr = ((int)hour == 0)? 12:(int) hour;
        int min = t % 60;
        String suffix = "am";
        if (t >= 12)
            suffix = "pm";
        String h = (hr < 10)? "0"+hr:""+hr;
        String m = (min < 10)? "0"+min:""+min;
        return h + ":" + m + suffix;
    }
    public void Increment(int amount)
    {
        t += amount;
        while (t >= 1440) {
            int r = t - 1440;
            t = r;
        }
    }
}