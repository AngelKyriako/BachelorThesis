using UnityEngine;
using System.Collections;

public class HealingOverTimeEffect: VitalOverTimeEffect {
    private VitalType vitalType;

    public void SetUpEffect(int _id, string _title, string _descr, uint _manaCost, uint _minLevelReq,//base
                            float _duration,                                                         //lasting
                            float _overTimeDuration, float _freq,                                    //overtime
                            EffectMod _modifier,                                                     //vital
                            VitalType _vitalType) {
        base.SetUpEffect(_id, _title, _descr, _manaCost, _minLevelReq, _duration, _overTimeDuration, _freq, _modifier);
        vitalType = _vitalType;
    }

    public override void Activate() {
        Receiver.GetVital((int)vitalType).CurrentValue += 
                                    (uint)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                                                    (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2)))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)vitalType).FinalValue)
                                          );
        base.Activate();
    }

    public VitalType VitalType {
        get { return vitalType; }
        set { vitalType = value; }
    }
}
