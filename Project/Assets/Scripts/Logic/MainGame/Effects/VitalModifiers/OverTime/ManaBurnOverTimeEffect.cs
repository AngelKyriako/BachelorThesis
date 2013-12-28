using UnityEngine;
using System.Collections;

public class ManaBurnOverTimeEffect: VitalOverTimeEffect {

    public override void Activate() {
        Receiver.GetVital((int)VitalType.Mana).CurrentValue -=
                                    (uint)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
                                                                  Receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Mana).FinalValue)
                                          );
    }
}
