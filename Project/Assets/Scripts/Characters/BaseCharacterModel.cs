using UnityEngine;
using System;
using System.Collections;

public class BaseCharacterModel: MonoBehaviour  {

    #region constants
    public const uint MAX_LEVEL = 25;
    public const uint STARTING_LEVEL = 1;
                                                                        //  str   agi   sta   int   cha                               
    private static readonly float[,] ATTRIBUTE_RATIOS = new float[6, 5] { { 4.5f, 0.0f, 0.0f, 0.0f, 0.0f },//atk
                                                                          { 3.5f, 0.0f, 1.0f, 0.0f, 0.0f },//def
                                                                          { 0.0f, 4.5f, 0.0f, 0.0f, 0.5f },//mvs 
                                                                          { 0.0f, 4.0f, 0.0f, 0.0f, 2.5f },//ats 
                                                                          { 0.0f, 0.5f, 0.0f, 0.5f, 5.5f },//crt 
                                                                          { 0.0f, 0.0f, 4.0f, 2.0f, 1.0f } //rgn 
                                                                        },
                                     VITAL_RATIOS = new float[2, 5]     { { 1.0f, 0.0f, 7.0f, 0.0f, 1.5f },//hp 
                                                                          { 0.0f, 0.0f, 2.0f, 7.5f, 1.0f } //mana 
                                                                        };
#endregion

    #region attributes
    private NetworkController networkController;
    private new string name;
    private uint level;
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

        stats = new Stat[Enum.GetValues(typeof(StatType)).Length];
        SetupStats();
        attributes = new Attribute[Enum.GetValues(typeof(AttributeType)).Length];
        SetupAttributes();
        vitals = new Vital[Enum.GetValues(typeof(VitalType)).Length];
        SetupVitals();
        UpdateAttributes();
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
            stats[i] = new Stat(Enum.GetName(typeof(StatType), i), "", 5);//@TODO: Define an array for stat descriptions & starting baseValues
    }

    private void SetupAttributes() {
        for (int i = 0; i < attributes.Length; ++i) {
            attributes[i] = new Attribute(Enum.GetName(typeof(AttributeType), i), "", 0);//@TODO: Define an array for attribute descriptions (& starting baseValues)
            for (int j = 0; j < stats.Length; ++j)
                if (ATTRIBUTE_RATIOS[i,j] > 0)
                    attributes[i].AddModifier(new ModifyingStat(stats[j], ATTRIBUTE_RATIOS[i,j]));
        }
    }

    private void SetupVitals() {
        for (int i = 0; i < vitals.Length; ++i) {
            vitals[i] = new Vital(Enum.GetName(typeof(VitalType), i), "", 0);//@TODO: Define an array for vital descriptions (& starting baseValues)
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
#endregion
}