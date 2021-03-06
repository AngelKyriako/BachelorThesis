﻿using UnityEngine;
using System;
using System.Collections.Generic;

public enum CharacterSkillSlot {
    None,
    Q,
    W,
    E,
    R    
}

public struct AttachedEffect {
    public BaseEffect Self;
    public BaseCharacterModel Caster;

    public AttachedEffect(BaseEffect _effect, BaseCharacterModel _caster) {
        Self = _effect;
        Caster = _caster;
    }
}

public abstract class BaseCharacterModel: MonoBehaviour  {

    #region constants
    public const int STARTING_LEVEL = 1;
    public const int MAX_LEVEL = 25;
    private const uint REGEN_FREQUENCY = 1;

    private static readonly int[] STAT_BASE_VALUES = new int[5] { 0, 0, 0, 0, 0 };
    private static readonly string[] STAT_DESCRIPTIONS = new string[5]{ "The power of your attack and defence",
                                                                        "Your survivability",
                                                                        "How quick you can move and operate",
                                                                        "Your ability of finding ways to use skills more often",
                                                                        "Capability of playing on a leading role" };

    private static readonly float[] ATTRIBUTE_BASE_VALUES = new float[12] { 10, 10, 10, 10, 0.5f, 0.5f, 1, 0, 0, 0, 1, 1 };

    private static readonly string[] ATTRIBUTE_NAMES = new string[12]{  "Physical Power",
                                                                        "Special Power",
                                                                        "Physical Defence",
                                                                        "Special Defence",
                                                                        "Health Regeneration",
                                                                        "Mana Regeneration",
                                                                        "Movement Bonus",
                                                                        "Attack Speed Bonus",
                                                                        "Critical Chance",
                                                                        "Evasion Chance",
                                                                        "Vision Bonus",
                                                                        "Leadership"};

    private static readonly string[] ATTRIBUTE_DESCRIPTIONS = new string[12]{ "Boosts HP loss on attack attack effect",
                                                                              "Boosts HP gain on heal effect",
                                                                              "Reduces HP loss on attack effect",
                                                                              "Reduces mana loss on attack effect",
                                                                              "Hit points gained per second",
                                                                              "Mana points gained per second",
                                                                              "Movement speed percent added to base speed",
                                                                              "Skill cooldowns percent subtracted for cooldown time",
                                                                              "Percent chance of dealing double damage",
                                                                              "Percent chance evading an attack",
                                                                              "Percent of radius added to base radius",
                                                                              "Boosts supportive spells"};

    private static readonly int[] VITAL_BASE_VALUES = new int[2] { 200, 100 };
    private static readonly string[] VITAL_DESCRIPTIONS = new string[2]{ "Your life",
                                                                         "Spent when casting skills" };

                                                                        //  str     sta     agi     int     cha                               
    private static readonly float[,] ATTRIBUTE_RATIOS = new float[12, 5]{ { 10.0f,  0.0f,   0.0f,   0.0f,   0.0f  },//PhysicalDamage (boosts hp loss on atk)
                                                                          { 0.0f,   0.0f,   0.0f,   6.0f,   0.0f  },//SpecialDamage (hp boost on heal)
                                                                          { 4.0f,   8.0f,   0.0f,   0.0f,   0.0f  },//PhysicalDefence (reduces hp loss on atk)
                                                                          { 0.0f,   6.0f,   0.0f,   3.0f,   0.0f  },//SpecialDefence (reduces mana loss on atk)
                                                                          { 0.03f,  0.08f,  0.0f,   0.0f,   0.0f  },//HeathRegen (hp gained per second)
                                                                          { 0.0f,   0.12f,  0.0f,   0.05f,  0.04f },//ManaRegen (mana gained per second)                                                                            
                                                                          { 0.0f,   0.0f,   0.021f, 0.0f,   0.0f  },//moveSpeed (movement speed percent added to basic movement)
                                                                          { 0.0f,   0.0f,   0.016f, 0.0f,   0.0f  },//attackSpeed (skill cooldowns percent subtracted for cooldown time)
                                                                          { 0.0f,   0.0f,   0.005f, 0.01f,  0.0f  },//critical (percent chance of dealing double damage)
                                                                          { 0.0f,   0.0f,   0.013f, 0.0f,   0.0f  },//evasion (percent chance evading an attack)
                                                                          { 0.0f,   0.0f,   0.0f,   0.015f, 0.06f },//radius (percent of radius added to base radius)
                                                                          { 0.0f,   0.0f,   0.0f,   0.0f,   0.7f  } //leadership (boosts supportive spells)
                                                                        },
                                     VITAL_RATIOS = new float[2, 5]     { { 8.0f,   30.0f,  0.0f,   0.0f,   5.0f  },//health (hit points)
                                                                          { 0.0f,   35.0f,  0.0f,   12.0f,  8.0f  } //mana (mana points)
                                                                        };

#endregion

    #region attributes                                                      
    private PlayerCharacterNetworkController networkController;//@TODO: this should be the super class of itself (CharacterNetworkController)
                                                               //       and ovveridable from player character model  -----> DONE just bored to fix it
    private int level;
    private Stat[] stats;
    private Attribute[] attributes;
    private Vital[] vitals;
    private Dictionary<CharacterSkillSlot, BaseSkill> skills;
    private float lastRegenTime;
    private bool isStunned, isSilenced;
#endregion

    public virtual void Awake() {
        networkController = gameObject.GetComponent<PlayerCharacterNetworkController>();
        
        skills = new Dictionary<CharacterSkillSlot, BaseSkill>();

        stats = new Stat[StatsLength];
        attributes = new Attribute[AttributesLength];
        vitals = new Vital[VitalsLength];
    }

    public virtual void Start() {
        Level = STARTING_LEVEL;
        SetupStats();
        SetupAttributes();
        SetupVitals();
        UpdateAttributesBasedOnStats();
        UpdateVitalsBasedOnStats();
        isStunned = false;

        lastRegenTime = Time.time;
    }

    public virtual void AddListeners() { }

    public virtual void Update() {
        //vital regeneration
        if (Time.time - lastRegenTime >= REGEN_FREQUENCY){
            GetVital((int)VitalType.Health).CurrentValue += GetAttribute((int)AttributeType.HealthRegen).FinalValue;
            GetVital((int)VitalType.Mana).CurrentValue += GetAttribute((int)AttributeType.ManaRegen).FinalValue;
            lastRegenTime = Time.time;
        }
        //skill cooldowns
        foreach (CharacterSkillSlot _key in skills.Keys)
            skills[_key].OnFrameUpdate();
    }

    public void UpdateAttributesBasedOnStats() {
        for (int i = 0; i < attributes.Length; ++i)
            attributes[i].UpdateAttribute();
    }
    public void UpdateVitalsBasedOnStats() {
        for (int i = 0; i < vitals.Length; ++i)
            vitals[i].UpdateAttribute();
    }

    public virtual void LevelUp() {
        ++Level;
        SetupSkillsManaCost();
    }

    public abstract void Died();
    public abstract void KilledEnemy(BaseCharacterModel _enemy);

    public void VitalsToFull() {
        for (int i = 0; i < VitalsLength; ++i)
            GetVital(i).CurrentValue = GetVital(i).FinalValue;
    }

    public void VitalsToZero() {
        GetVital((int)VitalType.Health).CurrentValue = 0;
        GetVital((int)VitalType.Mana).CurrentValue = 0;
    }   

#region Setup
    private void SetupStats() {
        for (int i = 0; i < stats.Length; ++i)
            stats[i] = new Stat(((StatType)i).ToString(), STAT_DESCRIPTIONS[i], STAT_BASE_VALUES[i]);
    }

    private void SetupAttributes() {
        for (int i = 0; i < attributes.Length; ++i) {
            attributes[i] = new Attribute(ATTRIBUTE_NAMES[i], ATTRIBUTE_DESCRIPTIONS[i], ATTRIBUTE_BASE_VALUES[i]);
            for (int j = 0; j < stats.Length; ++j)
                if (ATTRIBUTE_RATIOS[i,j] > 0)
                    attributes[i].AddModifier(new ModifyingStat(stats[j], ATTRIBUTE_RATIOS[i,j]));
        }
    }

    private void SetupVitals() {
        for (int i = 0; i < vitals.Length; ++i) {
            vitals[i] = new Vital(((VitalType)i).ToString(), VITAL_DESCRIPTIONS[i], VITAL_BASE_VALUES[i]);
            for (int j = 0; j < stats.Length; ++j)
                if (VITAL_RATIOS[i,j] > 0)
                    vitals[i].AddModifier(new ModifyingStat(stats[j], VITAL_RATIOS[i, j]));
        }
    }

    private void SetupSkillsManaCost() {
        foreach (CharacterSkillSlot _key in skills.Keys)
            skills[_key].UpdateManaCost(this);
    }
#endregion

#region Accessors
    public PlayerCharacterNetworkController NetworkController {
        get { return networkController; }
    }

    public int Level {
        get { return level; }
        set { level = value; }
    }
    public int StartingLevel {
        get { return STARTING_LEVEL; }
    }
    public int MaxLevel {
        get { return MAX_LEVEL; }
    }
    public virtual int ExpWorth {
        get { return (int)level * 10; }
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

    #region skills
    public void AddSkill(CharacterSkillSlot _key, BaseSkill _skill) {
        RemoveSkill(_key);
        skills.Add(_key, _skill);
        skills[_key].SetUpSkill(this, _key);
        skills[_key].UpdateManaCost(this);
    }
    public void RemoveSkill(CharacterSkillSlot _key) {
        if (SkillExists(_key))
            skills.Remove(_key);
    }
    public bool SkillExists(CharacterSkillSlot _key) {
        return skills.ContainsKey(_key) && GetSkill(_key).Id != 0;
    }
    public BaseSkill GetSkill(CharacterSkillSlot _key) {
        return skills[_key];
    }
    public float GetSkillCooldown(CharacterSkillSlot _key) {
        return skills[_key].CoolDownTimer;
    }
    public int SkillSlotsLength {
        get { return Enum.GetValues(typeof(CharacterSkillSlot)).Length; }
    }
    #endregion

    public bool IsStunned {
        get { return isStunned || gameObject.GetComponent<StunEffect>() != null; }
        set { isStunned = value; }
    }
    public bool IsSilenced {
        get { return isSilenced || gameObject.GetComponent<SilenceEffect>() != null; }
        set { isSilenced = value; }
    }

    public virtual bool IsAbleToCast() {
        return !IsSilenced && !IsStunned;
    }

    public virtual Vector3 ProjectileOriginPosition {
        get { return transform.position; }
    }

    #endregion
}