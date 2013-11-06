using UnityEngine;

public abstract class OverTimeEffect: StatModifierEffect {

    private float countdownTimer, time;

    public OverTimeEffect()
        : base() {
        countdownTimer = time = 0f;
    }

    public OverTimeEffect(string _title, string _descr, Texture2D _icon)
        : base(_title, _descr, _icon) {
            countdownTimer = time = 0f;
    }

    #region Accessors
    public float CountdownTimer {
        get { return countdownTimer; }
    }
    public float Time {
        get { return time; }
        set { time = value; }
    }
#endregion
}