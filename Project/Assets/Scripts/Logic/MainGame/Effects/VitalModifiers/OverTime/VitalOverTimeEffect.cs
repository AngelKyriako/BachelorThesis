using UnityEngine;
using System.Collections;

public class VitalOverTimeEffect: OverTimeEffect {

    private EffectMod modifier;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                         //lasting
                            float _overTimeDuration, float _freq,                                    //overtime
                            EffectMod _modifier) {
        base.SetUpEffect(_id, _type, _title, _descr, _manaCost, _minLevelReq, _duration, _overTimeDuration, _freq);
        modifier = _modifier;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifier = ((VitalOverTimeEffect)_effect).modifier;
        enabled = true;
    }

    public EffectMod Modifier {
        get { return modifier; }
        set { modifier = value; }
    }
}