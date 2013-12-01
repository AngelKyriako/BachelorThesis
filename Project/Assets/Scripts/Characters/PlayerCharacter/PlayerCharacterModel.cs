using UnityEngine;
using System;

public enum CharacterSkillSlot{
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
    private GameObject projectileSpawner;
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
        killsCount = 0;
        projectileSpawner = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterProjectileSpawnerPath(name));
    }

    public override void AddListeners() {
        PlayerInputManager.Instance.OnSkillSelectInput += OnSkillUse;
    }

    private void ModifyExp(uint exp) {
        if (Level != MAX_LEVEL) {
            currentExp += exp;
            while (currentExp >= expToLevel)
                LevelUp();
        }
    }

    public override void LevelUp() {
        base.LevelUp();
        currentExp -= expToLevel;
        expToLevel = (uint)(expToLevel * expModifier);
    }

    private void OnSkillUse(CharacterSkillSlot _slot) {
        //@TODO check if not static skill
        if (GetSkill((int)_slot) != null)
            GetSkill((int)_slot).Select(this, _slot);
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
    public override Vector3 ProjectileOriginPosition {
        get { return projectileSpawner.transform.position; }
    }
#endregion
}