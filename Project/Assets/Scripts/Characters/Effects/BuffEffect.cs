using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectMod {
    public float RawValue;
    public float PercentageValue;

    public EffectMod(float _RawValue, float _percent) {
        RawValue = _RawValue;
        PercentageValue = _percent;
    }
}

public class BuffEffect: BaseEffect {

    private EffectMod modifier;
    private float buffValue;
    private float countdownTimer;

    public override void Awake() {
        base.Awake();
        modifier = default(EffectMod);
        countdownTimer = 0;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, EffectMod _modifier, float _duration) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive);
        modifier = _modifier;
        countdownTimer = _duration;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifier = ((BuffEffect)_effect).modifier;
        countdownTimer = ((BuffEffect)_effect).countdownTimer;
    }

    public override void Update() {
        if (!IsActivated)
            Activate();
        else if (!InProgress)
            Deactivate();
        countdownTimer -= Time.deltaTime;
    }

    #region Accessors
    public EffectMod Modifier {
        get { return modifier; }
        set { modifier = value; }
    }
    public float BuffValue {
        get { return buffValue; }
        set { buffValue = value; }
    }

    public float CountdownTimer {
        set { countdownTimer = value; }
        get { return countdownTimer; }
    }
    public bool InProgress {
        get { return countdownTimer > 0; }
    }
#endregion
}