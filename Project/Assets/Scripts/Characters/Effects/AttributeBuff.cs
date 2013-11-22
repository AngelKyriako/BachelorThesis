using UnityEngine;
using System;
using System.Collections.Generic;

public class AttributeBuff: BuffEffect {

    private AttributeType attributeType;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration, AttributeType _attribute, EffectMod _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive, _modifier, _duration);
        attributeType = _attribute;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        attributeType = ((AttributeBuff)_effect).attributeType;
        BuffValue = 0;
        enabled = true;
    }

    public override void Activate() {
        float tempValue = Modifier.RawValue + (Modifier.PercentageValue * Receiver.GetAttribute((int)attributeType).FinalValue);
        BuffValue += tempValue;
        Receiver.GetAttribute((int)attributeType).BuffValue += tempValue;
        base.Activate();
    }

    public override void Deactivate() {
        Receiver.GetAttribute((int)attributeType).BuffValue -= BuffValue;
        base.Deactivate();
    }
}