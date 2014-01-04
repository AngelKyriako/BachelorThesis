using UnityEngine;

public class OverTimeBuffEffect: OverTimeEffect {

    private EffectMod modifier;
    private float buffValue;

    public override void Awake() {
        base.Awake();
        modifier = default(EffectMod);
    }

    public void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                                           //lasting
                            float _overTimeDuration, float _freq,                                                      //overtime
                            EffectMod _modifier) {
        base.SetUpEffect(_id, _type, _title, _descr, _manaCost, _minLevelReq, _duration, _overTimeDuration, _freq);
        modifier = _modifier;
        buffValue = 0;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifier = ((OverTimeBuffEffect)_effect).modifier;
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
#endregion
}