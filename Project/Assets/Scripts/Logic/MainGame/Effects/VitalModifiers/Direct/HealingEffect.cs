using UnityEngine;
using System.Collections;

public class HealingEffect: VitalEffect {
    private VitalType vitalType;

    public void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            EffectMod _modifier,                                                                       //vital
                            VitalType _vital) {
        base.SetUpEffect(_id, _type, _title, _descr, _manaCost, _minLevelReq, _modifier);
        vitalType = _vital;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        vitalType = ((HealingEffect)_effect).vitalType;
    }

    public override void Activate() {
        Receiver.GetVital((int)vitalType).CurrentValue += 
                                    (uint)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue/3 /
                                                                    (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2)))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)vitalType).FinalValue)
                                          );
    }

    public VitalType VitalType {
        get { return vitalType; }
        set { vitalType = value; }
    }
}
