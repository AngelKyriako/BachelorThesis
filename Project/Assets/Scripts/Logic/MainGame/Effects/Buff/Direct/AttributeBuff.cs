using UnityEngine;
using System;
using System.Collections.Generic;

public class AttributeBuff: BuffEffect {

    private AttributeType attributeType;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(int _id, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                         //lasting
                            EffectMod _modifier,                                                     //buff
                            AttributeType _attribute) {
        base.SetUpEffect(_id, _title, _descr, _manaCost, _minLevelReq, _duration, _modifier);
        attributeType = _attribute;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        attributeType = ((AttributeBuff)_effect).attributeType;
        enabled = true;
    }

    public override void Activate() {
        float modyfyingValue = Modifier.RawValue + (Modifier.PercentageValue * Receiver.GetAttribute((int)attributeType).FinalValue);
        BuffValue += modyfyingValue;
        Receiver.GetAttribute((int)attributeType).BuffValue += modyfyingValue;
        base.Activate();
    }

    public override void Deactivate() {
        Receiver.GetAttribute((int)attributeType).BuffValue -= BuffValue;
        base.Deactivate();
    }
}