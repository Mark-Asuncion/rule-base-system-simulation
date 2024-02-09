using System;
public class mTemp {
    private float t = 0.0f;
    public float min_threshold = 20f;
    public float max_threshold = 42f;
    public float T {
        get { return t; }
        set {
            float ntemp = t + value;
            while (ntemp < min_threshold || ntemp > max_threshold) {
                if (ntemp < min_threshold) {
                    ntemp -= value;
                }
                else if (ntemp > max_threshold) {
                    ntemp -= value;
                }
            }
            t = ntemp;
        }
    }
    public mTemp(float t)
    {
        T = t;
    }
    
    public override String ToString()
    {
        return String.Format("{0:0.0}", t) + "°C";
    }
}