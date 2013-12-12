using UnityEngine;
using System.Collections;

public class OverTimeEffect : LastingEffect {

    private float overTimeCountdownTimer;
    private float frequency, lastActivationTime;

    public override void Awake() {
        base.Awake();
        overTimeCountdownTimer = 0f;
        frequency = 0f;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                                 //lasting
                            float _overTimeDuration, float _freq) {
        base.SetUpEffect(_title, _descr, _icon, _manaCost, _minLevelReq, _duration);
        overTimeCountdownTimer = _overTimeDuration;
        frequency = _freq;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        overTimeCountdownTimer = ((OverTimeEffect)_effect).OverTimeCountdownTimer;
        frequency = ((OverTimeEffect)_effect).Frequency;
    }

    public override void Update() {
        if (!IsActivated || IsReadyForNextActivation(Time.time)) {
            lastActivationTime = Time.time;
            Activate();
        }
        else if (!InProgress)
            Deactivate();

        DurationTimer -= Time.deltaTime;
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
