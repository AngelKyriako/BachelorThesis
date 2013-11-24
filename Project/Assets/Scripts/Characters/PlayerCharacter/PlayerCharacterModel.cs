using UnityEngine;
using System;

public enum CharacterSkillSlots{
    Q, W, E, R
}

public class PlayerCharacterModel: BaseCharacterModel {

    #region constants
    public const uint STARTING_EXP_TO_LEVEL = 50;
    public const float STARTING_EXP_MODIFIER = 1.1f;
    public readonly uint MAX_TRAINING_POINTS = 20;
#endregion

    #region attributes
    private uint currentExp, expToLevel;
    private float expModifier;
    private uint trainingPoints;
    private uint killsCount;
    private BaseSkill[] skills;    
#endregion

    public override void Awake() {
        base.Awake();
    }

    public override void Start() {
        base.Start();
        expModifier = STARTING_EXP_MODIFIER;
        expToLevel = STARTING_EXP_TO_LEVEL;
        currentExp = 0;
        trainingPoints = MAX_TRAINING_POINTS;
        skills = new BaseSpell[Enum.GetValues(typeof(CharacterSkillSlots)).Length];        

        PlayerInputManager.Instance.OnSkillSelectInput += OnSkillUse;
    }

    public override void Update() {
        base.Update();
        for (int i = 0; i < skills.Length; ++i ) {
            if (skills[i] != null)
                skills[i].Update();
        }
    }

    private void ModifyExp(uint exp) {
        if (Level != MAX_LEVEL) {
            currentExp += exp;
            while (currentExp >= expToLevel)
                LevelUp();
        }
    }

    private void LevelUp() {
        ++Level;
        currentExp -= expToLevel;
        expToLevel = (uint)(expToLevel * expModifier);
    }

    private void OnSkillUse(CharacterSkillSlots _slot) {
        //@TODO check if not static skill & cooldown is ready
        if (skills[(int)_slot] != null && skills[(int)_slot].IsReady(this))
            skills[(int)_slot].Target(this, _slot);
    }

    #region Accessors
    public uint CurrentExp {
        get { return currentExp; }
        set { currentExp = value; }
    }
    public uint ExpToLevel {
        get { return expToLevel; }
        set { expToLevel = value; }
    }
    public override uint ExpWorth {
        get { return (uint)(expToLevel * 0.33); }
    }

    public uint TrainingPoints {
        get { return trainingPoints; }
        set { trainingPoints = (value < 0) ? 0 : ((value > MAX_TRAINING_POINTS)?MAX_TRAINING_POINTS:value); }
    }
    public uint Kills {
        get { return killsCount; }
        set { killsCount = value; }
    }

    public void SetSkill(int index, BaseSkill skill) {
        skills[index] = skill;
    }
    public BaseSkill GetSkill(int index) {
        return skills[index];
    }    
    public int SkillCount {
        get { return skills.Length; }
    }
#endregion

    public void LogAttributes() {
        for (int i = 0; i < VitalsLength; ++i)
            Utilities.Instance.LogMessage(GetVital(i).Name + ": (" + GetVital(i).CurrentValue + "/" + GetVital(i).FinalValue + ")");
        for (int i = 0; i < AttributesLength; ++i)
            Utilities.Instance.LogMessage(GetAttribute(i).Name + ": " + GetAttribute(i).FinalValue);
    }
}