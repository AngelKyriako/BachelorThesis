using UnityEngine;
using System.Collections.Generic;

public struct StatModifier {
    public int raw;
    public float basePercent;

    public StatModifier(int _raw, float _percent) {
        raw = _raw;
        basePercent = _percent;
    }
}

public struct VitalModifier {
    public StatModifier max;
    public StatModifier current;

    public VitalModifier(StatModifier _max, StatModifier _current) {
        max = _max;
        current = _current;
    }
}

public class StatModifierEffect: BaseEffect {

    private Dictionary<int, StatModifier> modifiedStats, modifiedAttributes;
    private Dictionary<int, VitalModifier> modifiedVitals;

    public StatModifierEffect()
        : base() {
        modifiedStats = new Dictionary<int, StatModifier>();
        modifiedAttributes = new Dictionary<int, StatModifier>();
        modifiedVitals = new Dictionary<int, VitalModifier>();
    }

    public StatModifierEffect(string _title, string _descr, Texture2D _icon)
        : base(_title, _descr, _icon) {
        modifiedStats = new Dictionary<int, StatModifier>();
        modifiedAttributes = new Dictionary<int, StatModifier>();
        modifiedVitals = new Dictionary<int, VitalModifier>();
    }

    //@TODO figure out how to consider the caster's stats in this
    public override void Activate(PlayerCharacterModel caster, PlayerCharacterModel receiver) {
        foreach (KeyValuePair<int, StatModifier> entry in modifiedStats)
            receiver.GetStat(entry.Key).BuffValue += entry.Value.raw +
                                                     (int)(entry.Value.basePercent * receiver.GetStat(entry.Key).FinalValue);

        foreach (KeyValuePair<int, StatModifier> entry in modifiedAttributes)
            receiver.GetAttribute(entry.Key).BuffValue += entry.Value.raw +
                                                          (int)(entry.Value.basePercent * receiver.GetAttribute(entry.Key).FinalValue);

        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals) {
            receiver.GetVital(entry.Key).BuffValue += entry.Value.max.raw +
                                                      (int)(entry.Value.max.basePercent * receiver.GetVital(entry.Key).FinalValue);
            receiver.GetVital(entry.Key).CurrentValue += entry.Value.current.raw +
                                                         (int)(entry.Value.current.basePercent * receiver.GetVital(entry.Key).CurrentValue);
        }
    }

    public override void Deactivate(PlayerCharacterModel caster, PlayerCharacterModel receiver) {
        foreach (KeyValuePair<int, StatModifier> entry in modifiedStats)
            receiver.GetStat(entry.Key).BuffValue -= entry.Value.raw +
                                                     (int)(entry.Value.basePercent * receiver.GetStat(entry.Key).FinalValue);

        foreach (KeyValuePair<int, StatModifier> entry in modifiedAttributes)
            receiver.GetAttribute(entry.Key).BuffValue -= entry.Value.raw +
                                                          (int)(entry.Value.basePercent * receiver.GetAttribute(entry.Key).FinalValue);

        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals)
            receiver.GetVital(entry.Key).BuffValue -= entry.Value.max.raw +
                                                      (int)(entry.Value.max.basePercent * receiver.GetVital(entry.Key).FinalValue);
    }

    #region Accessors
    public void AddModifiedStat(int index, StatModifier _mod) {
        modifiedStats.Add(index, _mod);
    }
    public void RemoveModifiedStat(int index) {
        modifiedStats.Remove(index);
    }

    public void AddModifiedAttribute(int index, StatModifier _mod) {
        modifiedAttributes.Add(index, _mod);
    }
    public void RemoveModifiedAttribute(int index) {
        modifiedAttributes.Remove(index);
    }

    public void AddModifiedVital(int index, VitalModifier _mod) {
        modifiedVitals.Add(index, _mod);
    }
    public void RemoveModifiedVital(int index) {
        modifiedVitals.Remove(index);
    }
#endregion

    public override EffectType GetEffectType() { return EffectType.StatModifierEffect_T; }
}