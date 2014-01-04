using UnityEngine;
using System.Collections;

public class LastingEffect : BaseEffect {

    private bool isActivated;
    private float durationTimer;

    public override void Awake() {
        base.Awake();
        isActivated = false;
        durationTimer = 0;
    }

    public void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration) {
        base.SetUpEffect(_id, _type, _title, _descr, _manaCost, _minLevelReq);
        durationTimer = _duration;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        durationTimer = ((LastingEffect)_effect).DurationTimer;
    }

    public override void Update() {
        if (!isActivated)
            Activate();
        else if (!InProgress)
            Deactivate();
        durationTimer -= Time.deltaTime;
    }

    public override void Activate() {
        isActivated = true;
    }

    #region Accessors
    public bool IsActivated {
        get { return isActivated; }
    }
    public float DurationTimer {
        set { durationTimer = value; }
        get { return durationTimer; }
    }
    public bool InProgress {
        get { return durationTimer > 0; }
    }
#endregion
}
