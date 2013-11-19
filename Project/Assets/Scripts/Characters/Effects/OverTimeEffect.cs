using UnityEngine;

public class OverTimeEffect: BaseEffect {

    private float overTimeCountdownTimer;
    private float frequency, lastActivationTime;

    public override void Awake(){
        base.Awake();
        overTimeCountdownTimer = 0f;
        frequency = 0f;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration, float _overTimeDuration, float _freq) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive, _duration);        
        overTimeCountdownTimer = _overTimeDuration;
        frequency = _freq;
    }

    #region Accessors
    public float OverTimeCountdownTimer {
        get { return overTimeCountdownTimer; }
        set { overTimeCountdownTimer = value; }
    }
    public bool IsReadyForNextActivation(float nowTime) {
        return (overTimeCountdownTimer > 0 && (nowTime - lastActivationTime) >= frequency);
    }
    public float LastActivationTime {
        get { return lastActivationTime; }
        set { lastActivationTime = value; }
    }
    public float Frequency {
        get { return frequency; }
    }

    public override bool IsOverTimeEffect {
        get { return true; }
    }
#endregion
}