using UnityEngine;
using System;
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

public class BaseEffect{

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isActivated;    
    private float countdownTimer;
    // modifiers of the value of the stat
    private Dictionary<int, StatModifier> modifiedStats, modifiedAttributes;
    private Dictionary<int, VitalModifier> modifiedVitals;
    // to be removed during deactivation 
    private Dictionary<int, int> statActivationBuffValues,
                                 attributeActivationBuffValues,
                                 vitalActivationBuffValues;
#endregion

    #region constructors
    public BaseEffect(){
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isActivated = false;
        countdownTimer = 0f;
        modifiedStats = new Dictionary<int, StatModifier>();
        modifiedAttributes = new Dictionary<int, StatModifier>();
        modifiedVitals = new Dictionary<int, VitalModifier>();
        statActivationBuffValues = new Dictionary<int, int>();
        attributeActivationBuffValues = new Dictionary<int, int>();
        vitalActivationBuffValues = new Dictionary<int, int>();
        RefreshActivationBuffValues();
    }

    public BaseEffect(string _title, string _descr, Texture2D _icon, float _duration) {
        title = _title;
        description = _descr;
        icon = _icon;
        isActivated = false;
        countdownTimer = _duration;
        modifiedStats = new Dictionary<int, StatModifier>();
        modifiedAttributes = new Dictionary<int, StatModifier>();
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

    public void Activate(PlayerCharacterModel caster, PlayerCharacterModel receiver) {
        int tempValue = 0;
        foreach (KeyValuePair<int, StatModifier> entry in modifiedStats) {
            //@TODO dispatch depending on the stat to include caster and receiver stats
            tempValue = entry.Value.raw + (int)(entry.Value.basePercent * receiver.GetStat(entry.Key).FinalValue);
            statActivationBuffValues[entry.Key] += tempValue;
            receiver.GetStat(entry.Key).BuffValue += tempValue;
        }

        foreach (KeyValuePair<int, StatModifier> entry in modifiedAttributes) {
            //@TODO dispatch depending on the stat to include caster and receiver stats
            tempValue = entry.Value.raw + (int)(entry.Value.basePercent * receiver.GetStat(entry.Key).FinalValue);
            attributeActivationBuffValues[entry.Key] += tempValue;
            receiver.GetAttribute(entry.Key).BuffValue += tempValue;
        }
        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals) {
            //@TODO dispatch depending on the stat to include caster and receiver stats
            tempValue = entry.Value.max.raw + (int)(entry.Value.max.basePercent * receiver.GetVital(entry.Key).FinalValue);
            vitalActivationBuffValues[entry.Key] += tempValue;
            receiver.GetVital(entry.Key).BuffValue += tempValue;

            receiver.GetVital(entry.Key).CurrentValue += entry.Value.current.raw +
                                                         (int)(entry.Value.current.basePercent * receiver.GetVital(entry.Key).CurrentValue);
        }
        isActivated = true;
    }

    public void Deactivate(PlayerCharacterModel receiver) {
        foreach (KeyValuePair<int, StatModifier> entry in modifiedStats)
            receiver.GetStat(entry.Key).BuffValue -= statActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, StatModifier> entry in modifiedAttributes)
            receiver.GetAttribute(entry.Key).BuffValue -= attributeActivationBuffValues[entry.Key];

        foreach (KeyValuePair<int, VitalModifier> entry in modifiedVitals)
            receiver.GetVital(entry.Key).BuffValue -= vitalActivationBuffValues[entry.Key];
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
    }

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

    public float CountdownTimer {
        get { return countdownTimer; }
        set { countdownTimer = value; }
    }
    public bool InProgress {
        get { return countdownTimer > 0; }
    }
#endregion

}