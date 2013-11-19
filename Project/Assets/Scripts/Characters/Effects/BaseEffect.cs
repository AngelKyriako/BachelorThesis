using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectModifier {
    public int Raw;
    public float Percent;

    public EffectModifier(int _raw, float _percent) {
        Raw = _raw;
        Percent = _percent;
    }
}

public class BaseEffect{

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isActivated, isPassive;
    private float countdownTimer;
    // modifiers of the value of the stat
    private Dictionary<int, EffectModifier> modifiedStats, modifiedAttributes;
    private Dictionary<int, VitalModifier> modifiedVitals;
    // to be removed during deactivation 
    private Dictionary<int, int> statActivationBuffValues,
                                 attributeActivationBuffValues,
                                 vitalActivationBuffValues;
#endregion

    #region constructors
    public BaseEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration) {
        title = _title;
        description = _descr;
        icon = _icon;
        isActivated = false;
        isPassive = _isPassive;
        countdownTimer = _duration;
        modifiedStats = new Dictionary<int, EffectModifier>();
        modifiedAttributes = new Dictionary<int, EffectModifier>();
        modifiedVitals = new Dictionary<int, VitalModifier>();
        statActivationBuffValues = new Dictionary<int, int>();
        attributeActivationBuffValues = new Dictionary<int, int>();
        vitalActivationBuffValues = new Dictionary<int, int>();
        RefreshActivationBuffValues();
    }

    public BaseEffect(BaseEffect _effect) {
        title = _effect.title;
        description = _effect.description;
        icon = _effect.icon;
        isActivated = false;
        isPassive = _effect.IsPassive;
        countdownTimer = _effect.CountdownTimer;
        modifiedStats = _effect.modifiedStats;
        modifiedAttributes = _effect.modifiedAttributes;
        modifiedVitals = _effect.modifiedVitals;
        statActivationBuffValues = new Dictionary<int, int>();
        attributeActivationBuffValues = new Dictionary<int, int>();
        vitalActivationBuffValues = new Dictionary<int, int>();
        RefreshActivationBuffValues();
    }
#endregion

    public void Activate(BaseCharacterModel caster, BaseCharacterModel receiver) {
        int tempValue = 0;
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats) {
            tempValue = entry.Value.Raw + (int)(entry.Value.Percent * receiver.GetStat(entry.Key).FinalValue);
            statActivationBuffValues[entry.Key] += tempValue;
            receiver.GetStat(entry.Key).BuffValue += tempValue;
        }
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes) {
            tempValue = entry.Value.Raw + (int)(entry.Value.Percent * receiver.GetAttribute(entry.Key).FinalValue);
            attributeActivationBuffValues[entry.Key] += tempValue;
            receiver.GetAttribute(entry.Key).BuffValue += tempValue;
        }
        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals) {
            tempValue = entry.Value.Max.Raw + (int)(entry.Value.Max.Percent * receiver.GetVital(entry.Key).FinalValue);
            vitalActivationBuffValues[entry.Key] += tempValue;
            receiver.GetVital(entry.Key).BuffValue += tempValue;

            receiver.GetVital(entry.Key).CurrentValue += entry.Value.Current.Raw +
                                                         (int)(entry.Value.Current.Percent * receiver.GetVital(entry.Key).CurrentValue);
        }
        isActivated = true;
    }

    public void Deactivate(BaseCharacterModel _receiver) {
        foreach (KeyValuePair<int, EffectModifier> entry in modifiedStats)
            _receiver.GetStat(entry.Key).BuffValue -= statActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, EffectModifier> entry in modifiedAttributes)
            _receiver.GetAttribute(entry.Key).BuffValue -= attributeActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals)
            _receiver.GetVital(entry.Key).BuffValue -= vitalActivationBuffValues[entry.Key];
        isActivated = false;
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

    #region Accessors
    public string Title {
        get { return title; }
        set { title = value; }
    }
    public string Description {
        get { return description; }
        set { description = value; }
    }
    public Texture2D Icon {
        get { return icon; }
        set { icon = value; }
    }

    public bool IsActivated {
        get { return isActivated; }
        set { isActivated = value; }
    }
    public bool IsPassive {
        get { return isPassive; }
        set { isPassive = value; }
    }

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

    public void AddModifiedVital(int index, VitalModifier _mod) {
        modifiedVitals.Add(index, _mod);
    }
    public void RemoveModifiedVital(int index) {
        modifiedVitals.Remove(index);
    }

    public float CountdownTimer {
        get { return countdownTimer; }
        set { countdownTimer = value; }
    }

    public bool InProgress {
        get { return countdownTimer > 0; }
    }

    public virtual bool IsOverTimeEffect {
        get { return false; }
    }

#endregion

}