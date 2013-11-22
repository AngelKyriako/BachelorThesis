using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectModifier {
    public float RawValue;
    public float PercentageValue;

    public EffectModifier(float _RawValue, float _percent) {
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
    private Dictionary<int, float> statActivationBuffValues,
                                   attributeActivationBuffValues,
                                   vitalActivationBuffValues;

    private DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int> currentValueDispatcher;

    public override void Awake() {
        base.Awake();
        modifiedStats = new Dictionary<int, EffectModifier>();
        modifiedAttributes = new Dictionary<int, EffectModifier>();
        modifiedVitals = new Dictionary<int, VitalEffectModifier>();
        attributeActivationBuffValues = new Dictionary<int, float>();
        statActivationBuffValues = new Dictionary<int, float>();
        vitalActivationBuffValues = new Dictionary<int, float>();
        enabled = false;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifiedStats = ((StatModifierEffect)_effect).modifiedStats;
        modifiedAttributes = ((StatModifierEffect)_effect).modifiedAttributes;
        modifiedVitals = ((StatModifierEffect)_effect).modifiedVitals;
        RefreshActivationBuffValues();
        SetUpVitalDispatcheTable();
        enabled = true;
    }

    public override void Activate() {
        float tempBuffValue = 0;
        //stats
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats) {
            tempBuffValue = entry.Value.RawValue + (entry.Value.PercentageValue * Receiver.GetStat(entry.Key).FinalValue);
            statActivationBuffValues[entry.Key] += (int)tempBuffValue;
            Receiver.GetStat(entry.Key).BuffValue += (int)tempBuffValue;
            Utilities.Instance.LogMessage("Stat buff value: " + tempBuffValue);
        }
        Receiver.UpdateAttributes();
        //attributes
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes) {
            tempBuffValue = entry.Value.RawValue + (entry.Value.PercentageValue * Receiver.GetAttribute(entry.Key).FinalValue);
            attributeActivationBuffValues[entry.Key] += tempBuffValue;
            Receiver.GetAttribute(entry.Key).BuffValue += tempBuffValue;
            Utilities.Instance.LogMessage("Attribute buff value: " + tempBuffValue);
        }
        //vitals
        foreach (KeyValuePair<int, VitalEffectModifier> entry in modifiedVitals) {
            tempBuffValue = entry.Value.Max.RawValue + (entry.Value.Max.PercentageValue * Receiver.GetVital(entry.Key).FinalValue);
            Utilities.Instance.LogMessage("Vital buff value: " + tempBuffValue);
            vitalActivationBuffValues[entry.Key] += (int)tempBuffValue;
            //Max Value
            Receiver.GetVital(entry.Key).BuffValue += (int)tempBuffValue;
            //Current Value
            Receiver.GetVital(entry.Key).CurrentValue += currentValueDispatcher.Dispatch((VitalType)entry.Key, (int)entry.Value.Current.RawValue, Caster, Receiver) +
                                                         (int)(entry.Value.Current.PercentageValue * Receiver.GetVital(entry.Key).FinalValue);
            Utilities.Instance.LogMessage("Dispatch ret value: " + currentValueDispatcher.Dispatch((VitalType)entry.Key, (int)entry.Value.Current.RawValue, Caster, Receiver));
        }
        base.Activate();
    }

    public override void Deactivate() {
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats)
            Receiver.GetStat(entry.Key).BuffValue -= statActivationBuffValues[entry.Key];
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes)
            Receiver.GetAttribute(entry.Key).BuffValue -= attributeActivationBuffValues[entry.Key];
        foreach (KeyValuePair<int, VitalEffectModifier> entry in modifiedVitals)
            Receiver.GetVital(entry.Key).BuffValue -= vitalActivationBuffValues[entry.Key];
        base.Deactivate();
    }

    private void RefreshActivationBuffValues() {
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; ++i)
            statActivationBuffValues[i] = 0;
        for (int i = 0; i < Enum.GetValues(typeof(AttributeType)).Length; ++i)
            attributeActivationBuffValues[i] = 0;
        for (int i = 0; i < Enum.GetValues(typeof(VitalType)).Length; ++i)
            vitalActivationBuffValues[i] = 0;
    }

    private void SetUpVitalDispatcheTable() {
        currentValueDispatcher = new DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int>();

        currentValueDispatcher.AddAction(VitalType.Health, delegate(int rawValue, BaseCharacterModel _caster, BaseCharacterModel _receiver) {
            return (int)((rawValue < 0) ?
                           //Physical Damage
                          rawValue * (_caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                      _receiver.GetAttribute((int)AttributeType.Defence).FinalValue)
                          ://Health Healing
                          rawValue * (_caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      _receiver.GetAttribute((int)AttributeType.Leadership).FinalValue)
                        );
        });
        currentValueDispatcher.AddAction(VitalType.Mana, delegate(int rawValue, BaseCharacterModel _caster, BaseCharacterModel _receiver) {
            return (int)((rawValue < 0) ?
                           //Magic Damage
                          rawValue * (_caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
                                      _receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue)
                          ://Mana Healing
                          rawValue * (_caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      _receiver.GetAttribute((int)AttributeType.Leadership).FinalValue)
                        );
        });
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
