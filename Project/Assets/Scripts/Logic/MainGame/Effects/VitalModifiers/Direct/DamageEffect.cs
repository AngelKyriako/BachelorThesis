using UnityEngine;
using System.Collections;

public class DamageEffect: VitalEffect {

    public override void Activate() {
        if ((Receiver.GetVital((int)VitalType.Health).CurrentValue -= (uint)(
                                            (Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                                                  Receiver.GetAttribute((int)AttributeType.Defence).FinalValue))
                                                                                +
                                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Health).FinalValue))) <= 0) {
            
            GameManager.Instance.MyDeathController.Enable();
            if(!Caster.name.Equals(Receiver.name))
                CombatManager.Instance.KillHappened(Caster.name, Receiver.name, Receiver.transform.position);
            TeleportManager.Instance.StandardTeleportation(false);
        }
    }
}