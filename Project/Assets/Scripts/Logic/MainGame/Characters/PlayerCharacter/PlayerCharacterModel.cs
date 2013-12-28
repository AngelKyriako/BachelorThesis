using UnityEngine;
using System;

public class PlayerCharacterModel: BaseCharacterModel {

    #region constants
    public const uint STARTING_EXP_TO_LEVEL = 50;
    public const float STARTING_EXP_MODIFIER = 1.1f, EXP_LOSS_PERCENTAGE = 0.2f;

    private static readonly int[] TRAINING_POINTS_PER_LEVEL = new int[MAX_LEVEL] {  5, 4, 3, 2, 1,
                                                                                    1, 1, 2, 2, 2,
                                                                                    2, 2, 3, 3, 3,
                                                                                    5, 4, 3, 2, 1,
                                                                                    5, 1, 1, 1, 1
                                                                                 };
    #endregion

    #region attributes
    private uint currentExp, expToLevel;
    private float expModifier;
    private int trainingPoints;
    private uint killsCount, deathCount;
    private float respawnTimer;
    private GameObject projectileSpawner;
    #endregion

    public override void Awake() {
        base.Awake();
        expModifier = STARTING_EXP_MODIFIER;
        expToLevel = STARTING_EXP_TO_LEVEL;
        currentExp = 0;
        killsCount = deathCount = 0;
        respawnTimer = 0;
        enabled = false;
    }

    public override void Start() {
        base.Start();        
        trainingPoints = TRAINING_POINTS_PER_LEVEL[Level-1];
        projectileSpawner = GameObject.Find(SceneHierarchyManager.Instance.PlayerCharacterProjectileSpawnerPath(name));
    }

    public override void AddListeners() {
        PlayerInputManager.Instance.SkillQWERorLeftClick += delegate(CharacterSkillSlot _slotPressed) {
            if (!IsStunned && !IsSilenced && SkillExists(_slotPressed))
                GetSkill(_slotPressed).Pressed();
        };

        PlayerInputManager.Instance.SkillQWERorLeftClick += delegate(CharacterSkillSlot _slotPressed) {
            if (!IsStunned && !IsSilenced && SkillExists(PlayerInputManager.Instance.CurrentTargetedSlot))
                GetSkill(PlayerInputManager.Instance.CurrentTargetedSlot).Unpressed();


            PlayerInputManager.Instance.CurrentTargetedSlot = (SkillExists(_slotPressed) &&
                                                               !GetSkill(_slotPressed).IsSelected) ? CharacterSkillSlot.None : _slotPressed;
        };

        PlayerInputManager.Instance.SkillRightClick += delegate(CharacterSkillSlot _slotPressed) {
            if (SkillExists(PlayerInputManager.Instance.CurrentTargetedSlot))
                GetSkill(PlayerInputManager.Instance.CurrentTargetedSlot).Unpressed();


            PlayerInputManager.Instance.CurrentTargetedSlot = (SkillExists(_slotPressed) &&
                                                               !GetSkill(_slotPressed).IsSelected) ? CharacterSkillSlot.None : _slotPressed;
        };
    }

    public override void Update() {
        base.Update();
        if (Input.GetKeyUp(KeyCode.L))
            GainExp(ExpToLevel);
    }

    public override void LevelUp() {
        base.LevelUp();
        currentExp -= expToLevel;
        expToLevel = (uint)(expToLevel * expModifier);
        trainingPoints += TRAINING_POINTS_PER_LEVEL[Level-1];
        VitalsToFull();
    }

    public override void KilledEnemy(BaseCharacterModel _enemy) {
        ++killsCount;
        GainExp((uint)_enemy.ExpWorth / 2);
    }

    public override void Died() {
        ++deathCount;
        RespawnTimer = Level * ((killsCount / 2) /
                                (deathCount * 2)) + 2;

        uint ExpLoss = (uint)(expToLevel * EXP_LOSS_PERCENTAGE);
        LoseExp(ExpLoss > CurrentExp ? CurrentExp : ExpLoss);
        VitalsToZero();
        IsSilenced = true;
    }

    public void GainExp(uint _exp) {
        if (Level < MAX_LEVEL) {
            currentExp += _exp;
            while (currentExp >= expToLevel)
                LevelUp();
        }
    } 

    private void LoseExp(uint _exp) {
        currentExp -= _exp;
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
    public override int ExpWorth {
        get { return (int)(expToLevel * 0.33); }
    }

    public int TrainingPoints {
        get { return trainingPoints; }
        set { trainingPoints = value; }
    }
    public uint Kills {
        get { return killsCount; }
        set { killsCount = value; }
    }
    public uint Deaths {
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