﻿using UnityEngine;
using System.Collections;

public class DamageEffect: VitalEffect {

    public override void Activate() {
        //Caster.GetAttribute((int)AttributeType.Critical); //@TODO: critical hit functionality
        //Receiver.GetAttribute((int)AttributeType.Evasion); //@TODO: evasion functionality
        if ((Receiver.GetVital((int)VitalType.Health).CurrentValue -= (uint)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                                                  Receiver.GetAttribute((int)AttributeType.Defence).FinalValue))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Health).FinalValue))) <= 0) {
            
            GameManager.Instance.MyDeathController.Enable(Caster.name, Receiver.name, Receiver.transform.position);
        }
    }
}