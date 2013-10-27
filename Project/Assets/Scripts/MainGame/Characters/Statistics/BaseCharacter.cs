using UnityEngine;
using System;
using System.Collections;

public class BaseCharacter: MonoBehaviour {

    public const uint MAX_LEVEL = 25;
    public const uint STARTING_LEVEL = 1;
    public const uint STARTING_EXP_TO_LEVEL = 50;
    public const float STARTING_EXP_MODIFIER = 1.1f;

    public const float STRENGTH_ATTACK_RATIO = 0.2f;
    //...

    private new string name;
    private uint level;
    private uint currentExp, expToLevel;
    private float expModifier;
    private Stat[] stats;
    private Attribute[] attributes;
    private Vital[] vitals;

    public void Awake() {
        name = base.name;
        Level = STARTING_LEVEL;
        expModifier = STARTING_EXP_MODIFIER;
        expToLevel = STARTING_EXP_TO_LEVEL;
        currentExp = 0;
        stats = new Stat[Enum.GetValues(typeof(StatType)).Length];
        attributes = new Attribute[Enum.GetValues(typeof(AttributeType)).Length];
        vitals = new Vital[Enum.GetValues(typeof(VitalType)).Length];
        SetupStats();
        SetupAttributes();
        SetupVitals();
    }
    #region accessors
    public string Name {
        get { return name; }
        set { name = value; }
    }

    public uint Level {
        get { return level; }
        set { level = value; }
    }

    public uint CurrentExp {
        get { return currentExp; }
        set { currentExp = value; }
    }

    public uint ExpToLevel {
        get { return expToLevel; }
        set { expToLevel = value; }
    }

    public Stat GetStat(StatType stat) {
        return stats[(int)stat];
    }

    public Attribute GetAttribute(AttributeType attribute) {
        return attributes[(int)attribute];
    }

    public Vital GetVital(VitalType vital) {
        return vitals[(int)vital];
    }
    #endregion


    public void UpdateAttributes() {
        for (int i = 0; i < attributes.Length; ++i)
            attributes[i].UpdateAttribute();
        for (int i = 0; i < vitals.Length; ++i)
            vitals[i].UpdateAttribute();
    }


    public void GainExp(uint exp) {
        if (level != MAX_LEVEL) {
            currentExp += exp;
            LevelUp();
        }
    }

    private void LevelUp() {
        if (currentExp >= expToLevel) {
            ++level;
            expToLevel = (uint)(expToLevel * expModifier);
            currentExp = 0;
        }
    }

    private void SetupStats() {
        for (int i = 0; i < stats.Length; ++i)
            stats[i] = new Stat();
    }

    private void SetupAttributes() {
        for (int i = 0; i < attributes.Length; ++i)
            attributes[i] = new Attribute();

        GetAttribute(AttributeType.Attack).AddModifier(new ModifyingStat(GetStat(StatType.Strength), 0.2f));
        GetAttribute(AttributeType.Defence).AddModifier(new ModifyingStat(GetStat(StatType.Strength), 0.2f));
        GetAttribute(AttributeType.MovementSpeed).AddModifier(new ModifyingStat(GetStat(StatType.Agility), 0.2f));
        GetAttribute(AttributeType.AttackSpeed).AddModifier(new ModifyingStat(GetStat(StatType.Agility), 0.2f));
        GetAttribute(AttributeType.CriticalChance).AddModifier(new ModifyingStat(GetStat(StatType.Charisma), 0.2f));
        GetAttribute(AttributeType.Radius).AddModifier(new ModifyingStat(GetStat(StatType.Charisma), 0.2f));
        GetAttribute(AttributeType.Regeneration).AddModifier(new ModifyingStat(GetStat(StatType.Stamina), 0.2f));
    }

    private void SetupVitals() {
        for (int i = 0; i < vitals.Length; ++i)
            vitals[i] = new Vital();

        GetVital(VitalType.Health).AddModifier(new ModifyingStat(GetStat(StatType.Stamina), 0.2f));
        GetVital(VitalType.Mana).AddModifier(new ModifyingStat(GetStat(StatType.Intelligence), 0.2f));
    }
}