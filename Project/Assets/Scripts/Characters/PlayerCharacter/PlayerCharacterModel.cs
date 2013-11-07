using System;
using System.Collections.Generic;

public enum CharacterSkillSlots{
    Q, W, E, R
}

public class PlayerCharacterModel: BaseCharacterModel {

    #region constants
    public const uint STARTING_EXP_TO_LEVEL = 50;
    public const float STARTING_EXP_MODIFIER = 1.1f;
    public readonly uint MAX_TRAINING_POINTS = 10;
#endregion

    #region attributes
    private uint currentExp, expToLevel;
    private float expModifier;
    private uint trainingPoints;
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
        PlayerInputManager.Instance.OnSkillSelectInput += OnSkillClick;
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

    private void OnSkillClick(CharacterSkillSlots _slot) {
        Utilities.Instance.LogMessage("Casting skill " + skills[(int)_slot] + "using key: " + _slot.ToString());
        if (skills[(int)_slot]!=null)
            ((BaseSpell)skills[(int)_slot]).Trigger(this, this);
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
    public uint TrainingPoints {
        get { return trainingPoints; }
        set { trainingPoints = (value < 0) ? 0 : ((value > MAX_TRAINING_POINTS)?MAX_TRAINING_POINTS:value); }
    }
    public BaseSkill GetSkill(int index) {
        return skills[index];
    }
    public BaseSkill SetSkill(int index, BaseSkill skill) {
        return skills[index] = skill;
    }
    public int SkillCount {
        get { return skills.Length; }
    }
#endregion
}