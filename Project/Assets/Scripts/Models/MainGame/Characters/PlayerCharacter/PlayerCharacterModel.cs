using UnityEngine;
using System;

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
    private uint killsCount, deathCount;
    private float respawnTimer;
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
        deathCount = 0;
        respawnTimer = 0;
        projectileSpawner = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterProjectileSpawnerPath(name));
    }

    public override void AddListeners() {
        PlayerInputManager.Instance.SkillQWERorLeftClick += SkillSelect;
        PlayerInputManager.Instance.SkillQWERorLeftClick += SkillUnselect;

        PlayerInputManager.Instance.SkillRightClick += SkillUnselect;
    }

    public override void Update() {
        base.Update();
    }

    public override void LevelUp() {
        base.LevelUp();
        currentExp -= expToLevel;
        expToLevel = (uint)(expToLevel * expModifier);
    }

    public void GainExp(uint _exp) {
        if (Level != MAX_LEVEL) {
            currentExp += _exp;
            while (currentExp >= expToLevel)
                LevelUp();
        }
    }

    private void LoseExp(uint _exp) {
        currentExp -= _exp;
    }

    public override void Died() {
        RespawnTimer = Level * ((killsCount / 2)/
                                (++deathCount * 2));
        LoseExp(expToLevel/10);
        
    }

    public override void KilledEnemy(BaseCharacterModel _enemy) {
        ++killsCount;
    }

    private void SkillSelect(CharacterSkillSlot _slotPressed) {
        if (SkillExists(_slotPressed))
            GetSkill(_slotPressed).Pressed();
    }
    private void SkillUnselect(CharacterSkillSlot _slotPressed) {
        if (SkillExists(PlayerInputManager.Instance.CurrentTargetedSlot))
            GetSkill(PlayerInputManager.Instance.CurrentTargetedSlot).Unpressed();


        PlayerInputManager.Instance.CurrentTargetedSlot = (SkillExists(_slotPressed) &&
                                                           !GetSkill(_slotPressed).IsSelected) ? CharacterSkillSlot.None : _slotPressed;
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
    public uint DeathCount {
        get { return deathCount; }
        set { deathCount = value; }
    }
    public float RespawnTimer {
        get { return respawnTimer; }
        set { respawnTimer = (value < 0) ? 0 : value; }
    }
    public bool IsDead {
        get { return RespawnTimer != 0; }
    }

    public override Vector3 ProjectileOriginPosition {
        get { return projectileSpawner.transform.position; }
    }
#endregion
}