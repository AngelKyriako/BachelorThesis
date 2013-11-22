using UnityEngine;

public class OverTimeAttributeBuff: AttributeBuff {

    private float overTimeCountdownTimer;
    private float frequency, lastActivationTime;

    public override void Awake(){
        base.Awake();
        overTimeCountdownTimer = 0f;
        frequency = 0f;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, AttributeType _attribute, EffectMod _modifier,
                                                                           float _duration, float _overTimeDuration, float _freq) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive, _duration, _attribute, _modifier);
        overTimeCountdownTimer = _overTimeDuration;
        frequency = _freq;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        overTimeCountdownTimer = ((OverTimeAttributeBuff)_effect).OverTimeCountdownTimer;
        frequency = ((OverTimeAttributeBuff)_effect).Frequency;
    }

    public override void Update() {
        if (!IsActivated || IsReadyForNextActivation(Time.time)) {
            lastActivationTime = Time.time;
            Activate();
        }
        else if (!InProgress)
            Deactivate();
        Utilities.Instance.LogMessage("overTimeCountdownTimer: " + overTimeCountdownTimer);
        Utilities.Instance.LogMessage("left bool" + (overTimeCountdownTimer > 0 )+ "right bool: " + ((Time.time - lastActivationTime) >= frequency));
        CountdownTimer -= Time.deltaTime;
        overTimeCountdownTimer -= Time.deltaTime;
    }

    #region Accessors
    public float OverTimeCountdownTimer {
        get { return overTimeCountdownTimer; }
    }
    public bool IsReadyForNextActivation(float nowTime) {
        return (overTimeCountdownTimer > 0 && ((nowTime - lastActivationTime) >= frequency));
    }
    public float LastActivationTime {
        get { return lastActivationTime; }
    }
    public float Frequency {
        get { return frequency; }
    }
#endregion
}