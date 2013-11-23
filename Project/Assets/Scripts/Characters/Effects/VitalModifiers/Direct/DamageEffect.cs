using UnityEngine;
using System.Collections;

public class DamageEffect: VitalEffect {

    public override void Activate() {
        Receiver.GetVital((int)VitalType.Health).CurrentValue -=
                                    (int)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                                                  Receiver.GetAttribute((int)AttributeType.Defence).FinalValue))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Health).FinalValue)
                                         );
    }
}
