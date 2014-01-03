using UnityEngine;
using System.Collections;

public class DamageOverTimeEffect: VitalOverTimeEffect {

    private const float CRITICAL_HIT_SEVERITY = 1.4f;

    public override void Activate() {
        uint damage;
        if (!ReceiverEvaded || IsEffectingSelf) {
            damage = (uint)((Modifier.RawValue * (Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                                    Receiver.GetAttribute((int)AttributeType.Defence).FinalValue))
                                                                +
                            (Modifier.PercentageValue * Receiver.GetVital((int)VitalType.Health).FinalValue));

            if (CasterCriticalHit && !IsEffectingSelf) {
                damage = (uint)(damage * CRITICAL_HIT_SEVERITY);
                GameManager.Instance.BroadcastChatMessage(SystemMessages.Instance.CriticalHit(GameManager.Instance.GetPlayerName(Caster.name),
                                                                                              GameManager.Instance.GetPlayerName(Receiver.name)));
            }

            if ((Receiver.GetVital((int)VitalType.Health).CurrentValue -= damage) <= 0)
                GameManager.Instance.MyDeathController.Enable(Caster.name, Receiver.name, Receiver.transform.position);
        }
        else
            GameManager.Instance.BroadcastChatMessage(SystemMessages.Instance.Evasion(GameManager.Instance.GetPlayerName(Caster.name),
                                                                                      GameManager.Instance.GetPlayerName(Receiver.name)));
        base.Activate();
    }
}