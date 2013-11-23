using UnityEngine;
using System.Collections;

public class ManaBurnEffect: VitalEffect {

    public override void Activate() {
        Receiver.GetVital((int)VitalType.Mana).CurrentValue -=
                                    (int)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
                                                                  Receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Mana).FinalValue)
                                         );
    }
}
