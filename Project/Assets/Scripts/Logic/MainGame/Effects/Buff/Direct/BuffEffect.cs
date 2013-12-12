using UnityEngine;
using System;
using System.Collections.Generic;

public class BuffEffect: LastingEffect {

    private EffectMod modifier;
    private float buffValue;

    public override void Awake() {
        base.Awake();
        modifier = default(EffectMod);
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, uint _manaCost, uint _minLevelReq, //base
                            float _duration,                                                                  //lasting
                            EffectMod _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _manaCost, _minLevelReq, _duration);
        modifier = _modifier;
        buffValue = 0;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifier = ((BuffEffect)_effect).modifier;
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