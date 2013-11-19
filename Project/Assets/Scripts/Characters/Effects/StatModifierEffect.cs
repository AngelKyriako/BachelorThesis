using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectModifier {
    public int RawValue;
    public float PercentageValue;

    public EffectModifier(int _RawValue, float _percent) {
        RawValue = _RawValue;
        PercentageValue = _percent;
    }
}

public struct VitalEffectModifier {
    public EffectModifier Max;
    public EffectModifier Current;

    public VitalEffectModifier(EffectModifier _Current, EffectModifier _Max) {
        Current = _Current;
        Max = _Max;
    }
}

public class StatModifierEffect: BaseEffect {

    // modifiers of the value of the stat
    private Dictionary<int, EffectModifier> modifiedStats, modifiedAttributes;
    private Dictionary<int, VitalEffectModifier> modifiedVitals;
    // to be removed during deactivation 
    private Dictionary<int, int> statActivationBuffValues,
                                 attributeActivationBuffValues,
                                 vitalActivationBuffValues;

    public override void Awake() {
        base.Awake();
        modifiedStats = new Dictionary<int, EffectModifier>();
        modifiedAttributes = new Dictionary<int, EffectModifier>();
        modifiedVitals = new Dictionary<int, VitalEffectModifier>();
        statActivationBuffValues = new Dictionary<int, int>();
        attributeActivationBuffValues = new Dictionary<int, int>();
        vitalActivationBuffValues = new Dictionary<int, int>();
        RefreshActivationBuffValues();
    }

    public override void Activate(BaseCharacterModel caster, BaseCharacterModel receiver) {
        int tempValue = 0;
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats) {
            tempValue = entry.Value.RawValue + (int)(entry.Value.PercentageValue * receiver.GetStat(entry.Key).FinalValue);
            statActivationBuffValues[entry.Key] += tempValue;
            receiver.GetStat(entry.Key).BuffValue += tempValue;
        }
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes) {
            tempValue = entry.Value.RawValue + (int)(entry.Value.PercentageValue * receiver.GetAttribute(entry.Key).FinalValue);
            attributeActivationBuffValues[entry.Key] += tempValue;
            receiver.GetAttribute(entry.Key).BuffValue += tempValue;
        }
        foreach (KeyValuePair<int, VitalEffectModifier> entry in modifiedVitals) {
            //@TODO dispatch depending on the stat to include caster and receiver stats
            tempValue = entry.Value.Max.RawValue + (int)(entry.Value.Max.PercentageValue * receiver.GetVital(entry.Key).FinalValue);
            vitalActivationBuffValues[entry.Key] += tempValue;
            receiver.GetVital(entry.Key).BuffValue += tempValue;

            receiver.GetVital(entry.Key).CurrentValue += entry.Value.Current.RawValue +
                                                         (int)(entry.Value.Current.PercentageValue * receiver.GetVital(entry.Key).CurrentValue);
        }
    }

    public override void Deactivate(BaseCharacterModel _receiver) {
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats)
            _receiver.GetStat(entry.Key).BuffValue -= statActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes)
            _receiver.GetAttribute(entry.Key).BuffValue -= attributeActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, VitalEffectModifier> entry in modifiedVitals)
            _receiver.GetVital(entry.Key).BuffValue -= vitalActivationBuffValues[entry.Key];
        RefreshActivationBuffValues();
    }

    private void RefreshActivationBuffValues() {
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; ++i)
            statActivationBuffValues[i] = 0;
        for (int i = 0; i < Enum.GetValues(typeof(AttributeType)).Length; ++i)
            attributeActivationBuffValues[i] = 0;
        for (int i = 0; i < Enum.GetValues(typeof(VitalType)).Length; ++i)
            vitalActivationBuffValues[i] = 0;
    }

    #region accessors
    public void AddModifiedStat(int index, EffectModifier _mod) {
        modifiedStats.Add(index, _mod);
    }
    public void RemoveModifiedStat(int index) {
        modifiedStats.Remove(index);
    }

    public void AddModifiedAttribute(int index, EffectModifier _mod) {
        modifiedAttributes.Add(index, _mod);
    }
    public void RemoveModifiedAttribute(int index) {
        modifiedAttributes.Remove(index);
    }

    public void AddModifiedVital(int index, EffectModifier _modCurrent, EffectModifier _modMax) {
        modifiedVitals.Add(index, new VitalEffectModifier(_modCurrent, _modMax));
    }
    public void RemoveModifiedVital(int index) {
        modifiedVitals.Remove(index);
    }
#endregion
}
