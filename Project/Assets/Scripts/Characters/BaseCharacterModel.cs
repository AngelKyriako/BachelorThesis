﻿using UnityEngine;
using System;
using System.Collections.Generic;

public struct AttachedEffect {
    public BaseEffect Self;
    public BaseCharacterModel Caster;

    public AttachedEffect(BaseEffect _effect, BaseCharacterModel _caster) {
        Self = _effect;
        Caster = _caster;
    }
}

public class BaseCharacterModel: MonoBehaviour  {

    #region constants
    public const uint MAX_LEVEL = 25;
    public const uint STARTING_LEVEL = 1;

    private static readonly int[] STAT_BASE_VALUES = new int[5] { 0, 0, 0, 0, 0 };
    private static readonly string[] STAT_DESCRIPTIONS = new string[5]{ "The power of your attack and defence",
                                                                        "How quick you can move and operate",
                                                                        "Your survivability",
                                                                        "Your ability of finding ways to use skills more often",
                                                                        "Capability of playing on a leading role" };

    private static readonly float[] ATTRIBUTE_BASE_VALUES = new float[10] { 10, 8, 7, 1, 1, 1, 0, 0, 0, 1 };
    private static readonly string[] ATTRIBUTE_DESCRIPTIONS = new string[10]{ "Boosts HP loss on attack attack effect",
                                                                              "Boosts HP gain on heal effect",
                                                                              "Reduces HP loss on attack effect",
                                                                              "Hit points gained per second",
                                                                              "Mana points gained per second",
                                                                              "Movement speed percent added to base speed",
                                                                              "Skill cooldowns percent subtracted for cooldown time",
                                                                              "Percent chance of dealing double damage",
                                                                              "Percent chance evading an attack",
                                                                              "Percent of radius added to base radius"};

    private static readonly int[] VITAL_BASE_VALUES =     new int[2]  { 200, 100 };
    private static readonly string[] VITAL_DESCRIPTIONS = new string[2]{ "Your life",
                                                                         "Spent when casting skills" };
    
                                                                        //  str     agi     sta     int     cha                               
    private static readonly float[,] ATTRIBUTE_RATIOS = new float[10, 5]{ { 11.0f,  0.0f,   0.0f,   0.0f,   0.0f  },//damage (boosts hp loss on atk)
                                                                          { 0.0f,   0.0f,   0.0f,   0.0f,   7.0f  },//healing (hp boost on heal)
                                                                          { 3.0f,   0.0f,   8.0f,   0.0f,   0.0f  },//defence (reduces hp loss on atk)
                                                                          { 0.01f,  0.0f,   0.04f,  0.0f,   0.005f},//HeathRegen (hp gained per second)
                                                                          { 0.0f,   0.0f,   0.08f,  0.05f,  0.02f },//ManaRegen (mana gained per second)                                                                            
                                                                          { 0.0f,   0.025f, 0.0f,   0.0f,   0.0f  },//moveSpeed (movement speed percent added to basic movement)
                                                                          { 0.0f,   0.02f,  0.0f,   0.0f,   0.0f  },//attackSpeed (skill cooldowns percent subtracted for cooldown time)
                                                                          { 0.0f,   0.0f,   0.0f,   0.012f, 0.0f  },//critical (percent chance of dealing double damage)
                                                                          { 0.0f,   0.012f, 0.0f,   0.0f,   0.003f},//evasion (percent chance evading an attack)
                                                                          { 0.0f,   0.0f,   0.0f,   0.01f,  0.06f } //radius (percent of radius added to base radius)
                                                                        },
                                     VITAL_RATIOS = new float[2, 5]     { { 5.0f,   0.0f,   20.0f,  0.0f,   0.0f  },//health (hit points)
                                                                          { 0.0f,   0.0f,   5.0f,   15.0f,  0.0f  } //mana (mana points)
                                                                        };

#endregion

    #region attributes
    private NetworkController networkController;
    private new string name;
    private uint level;
    private List<AttachedEffect> effectsAttached;
    private Stat[] stats;
    private Attribute[] attributes;
    private Vital[] vitals;
#endregion

    public virtual void Awake() {
        networkController = gameObject.GetComponent<NetworkController>();
    }

    public virtual void Start() {
        name = networkController.photonView.owner.name;
        Level = STARTING_LEVEL;
        effectsAttached = new List<AttachedEffect>();

        stats = new Stat[Enum.GetValues(typeof(StatType)).Length];
        SetupStats();
        attributes = new Attribute[Enum.GetValues(typeof(AttributeType)).Length];
        SetupAttributes();
        vitals = new Vital[Enum.GetValues(typeof(VitalType)).Length];
        SetupVitals();
        UpdateAttributes();
    }

    void Update() {
        ManageEffectsAttached();
    }

    //@TODO Find a more elegant way to do this
    private void ManageEffectsAttached() {
        if (effectsAttached.Count > 0) {
            for (int i = 0; i < effectsAttached.Count; ++i) {
                if (!effectsAttached[i].Self.IsActivated) {
                    effectsAttached[i].Self.Activate(effectsAttached[i].Caster, this);
                    //LogAttributes();
                    if (effectsAttached[i].Self.IsOverTimeEffect) {
                        ((OverTimeEffect)effectsAttached[i].Self).LastActivationTime = Time.time;
                        Utilities.Instance.LogMessage("LastActivationTime: "+((OverTimeEffect)effectsAttached[i].Self).LastActivationTime);
                    }
                }
                if (effectsAttached[i].Self.IsOverTimeEffect) {
                    if (((OverTimeEffect)effectsAttached[i].Self).IsReadyForNextActivation(Time.time)) {
                        effectsAttached[i].Self.Activate(effectsAttached[i].Caster, this);
                        ((OverTimeEffect)effectsAttached[i].Self).LastActivationTime = Time.time;
                        Utilities.Instance.LogMessage("LastActivationTime: " + ((OverTimeEffect)effectsAttached[i].Self).LastActivationTime);
                        //LogAttributes();
                    }
                    ((OverTimeEffect)effectsAttached[i].Self).OverTimeCountdownTimer -= Time.deltaTime;
                }
                effectsAttached[i].Self.CountdownTimer -= Time.deltaTime;

                if (!effectsAttached[i].Self.InProgress) {
                    effectsAttached[i].Self.Deactivate(this);
                    effectsAttached.Remove(effectsAttached[i]);
                    //LogAttributes();
                }
            }
            Utilities.Instance.LogMessage("effect count:" + effectsAttached.Count);
        }
    }

    public void UpdateAttributes() {
        for (int i = 0; i < attributes.Length; ++i)
            attributes[i].UpdateAttribute();
        for (int i = 0; i < vitals.Length; ++i)
            vitals[i].UpdateAttribute();
    }

#region Setup
    private void SetupStats() {
        for (int i = 0; i < stats.Length; ++i)
            stats[i] = new Stat(Enum.GetName(typeof(StatType), i), STAT_DESCRIPTIONS[i], STAT_BASE_VALUES[i]);
    }

    private void SetupAttributes() {
        for (int i = 0; i < attributes.Length; ++i) {
            attributes[i] = new Attribute(Enum.GetName(typeof(AttributeType), i), ATTRIBUTE_DESCRIPTIONS[i], ATTRIBUTE_BASE_VALUES[i]);
            for (int j = 0; j < stats.Length; ++j)
                if (ATTRIBUTE_RATIOS[i,j] > 0)
                    attributes[i].AddModifier(new ModifyingStat(stats[j], ATTRIBUTE_RATIOS[i,j]));
        }
    }

    private void SetupVitals() {
        for (int i = 0; i < vitals.Length; ++i) {
            vitals[i] = new Vital(Enum.GetName(typeof(VitalType), i), VITAL_DESCRIPTIONS[i], VITAL_BASE_VALUES[i]);
            for (int j = 0; j < stats.Length; ++j)
                if (VITAL_RATIOS[i,j] > 0)
                    vitals[i].AddModifier(new ModifyingStat(stats[j], VITAL_RATIOS[i, j]));
        }
    }
#endregion

#region Accessors
    public string Name {
        get { return name; }
        set { name = value; }
    }
    public uint Level {
        get { return Level; }
        set { level = value; }
    }
    public virtual uint ExpWorth {
        get { return level * 10; }
    }

    public Stat GetStat(int _index) {
        return stats[_index];
    }
    public int StatsLength{
        get { return Enum.GetValues(typeof(StatType)).Length; }
    }

    public Attribute GetAttribute(int _index) {
        return attributes[_index];
    }
    public int AttributesLength {
        get { return Enum.GetValues(typeof(AttributeType)).Length; }
    }

    public Vital GetVital(int _index) {
        return vitals[_index];
    }
    public int VitalsLength {
        get { return Enum.GetValues(typeof(VitalType)).Length; }
    }

    public void AddEffectAttached(AttachedEffect _effect) {
        effectsAttached.Add(_effect);
    }
    public void RemoveEffectAttached(AttachedEffect _effect) {
        effectsAttached.Remove(_effect);
    }
    public AttachedEffect GetEffectAttached(int _index) {
        return effectsAttached[_index];
    }
    public int EffectAttachedCount {
        get { return effectsAttached.Count; }
    }
#endregion
}