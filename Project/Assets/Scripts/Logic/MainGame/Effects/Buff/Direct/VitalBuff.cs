using UnityEngine;
using System.Collections;

public class VitalBuff: BuffEffect {

    private VitalType vitalType;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(int _id, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                                 //lasting
                            EffectMod _modifier,                                                             //buff
                            VitalType _vital) {
        base.SetUpEffect(_id, _title, _descr, _manaCost, _minLevelReq, _duration, _modifier);
        vitalType = _vital;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        vitalType = ((VitalBuff)_effect).vitalType;
        enabled = true;
    }

    public override void Activate() {
        int modifyingValue = (int)(Modifier.RawValue + (Modifier.PercentageValue * Receiver.GetVital((int)vitalType).FinalValue));
        BuffValue += modifyingValue;
        Receiver.GetVital((int)vitalType).BuffValue += modifyingValue;
        Receiver.GetVital((int)vitalType).CurrentValue += modifyingValue;
        base.Activate();
    }

    public override void Deactivate() {
        Receiver.GetVital((int)vitalType).BuffValue -= BuffValue;
        Receiver.GetVital((int)vitalType).CurrentValue -= BuffValue;
        base.Deactivate();
    }
}