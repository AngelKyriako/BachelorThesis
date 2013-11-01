using UnityEngine;
using System;
using System.Collections;

public class BaseCharacterModel: MonoBehaviour {

#region const values
    public const uint MAX_LEVEL = 25;
    public const uint STARTING_LEVEL = 1;
    public const uint STARTING_EXP_TO_LEVEL = 50;
    public const float STARTING_EXP_MODIFIER = 1.1f;

                                                                        //  str   agi   sta   int   cha                               
    private static readonly float[,] ATTRIBUTE_RATIOS = new float[6, 5] { { 0.5f, 0.0f, 0.0f, 0.0f, 0.0f },//atk
                                                                          { 0.4f, 0.0f, 0.1f, 0.0f, 0.0f },//def
                                                                          { 0.0f, 0.5f, 0.0f, 0.0f, 0.1f },//mvs 
                                                                          { 0.0f, 0.4f, 0.0f, 0.0f, 0.0f },//ats 
                                                                          { 0.0f, 0.1f, 0.0f, 0.1f, 0.6f },//crt 
                                                                          { 0.0f, 0.0f, 0.4f, 0.2f, 0.2f } //rgn 
                                                                        },
                                   VITAL_RATIOS = new float[2, 5]       { { 0.1f, 0.0f, 0.7f, 0.0f, 0.2f },//hp 
                                                                          { 0.0f, 0.0f, 0.3f, 0.7f, 0.2f } //mana 
                                                                        };
#endregion

    private new string name;
    private uint level;
    private uint currentExp, expToLevel;
    private float expModifier;
    private Stat[] stats;
    private Attribute[] attributes;
    private Vital[] vitals;

    public virtual void Awake() {
        name = PlayerPrefs.GetString("name");
        Level = STARTING_LEVEL;
        expModifier = STARTING_EXP_MODIFIER;
        expToLevel = STARTING_EXP_TO_LEVEL;
        currentExp = 0;

        stats = new Stat[Enum.GetValues(typeof(StatType)).Length];
        SetupStats();
        attributes = new Attribute[Enum.GetValues(typeof(AttributeType)).Length];
        SetupAttributes();
        vitals = new Vital[Enum.GetValues(typeof(VitalType)).Length];
        SetupVitals();
    }

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

#region Setup
    private void SetupStats() {
        for (int i = 0; i < stats.Length; ++i)
            stats[i] = new Stat("", "", 5);
    }

    private void SetupAttributes() {
        for (int i = 0; i < attributes.Length; ++i) {
            attributes[i] = new Attribute();
            for (int j = 0; j < stats.Length; ++j)
                if (ATTRIBUTE_RATIOS[i,j] > 0)
                    attributes[i].AddModifier(new ModifyingStat(stats[j], ATTRIBUTE_RATIOS[i,j]));
        }
    }

    private void SetupVitals() {
        for (int i = 0; i < vitals.Length; ++i) {
            vitals[i] = new Vital();
            for (int j = 0; j < stats.Length; ++j)
                if (VITAL_RATIOS[i,j] > 0)
                    vitals[i].AddModifier(new ModifyingStat(stats[j], VITAL_RATIOS[i, j]));
        }
    }
#endregion

#region Setters and Getters
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
}