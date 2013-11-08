using UnityEngine;
using System;
using System.Collections.Generic;

public enum CharacterSkillSlots{
    Q, W, E, R
}

public struct AttachedEffect {
    public BaseEffect Self;
    public PlayerCharacterModel Caster;

    public AttachedEffect(BaseEffect _effect, PlayerCharacterModel _caster) {
        Self = _effect;
        Caster = _caster;
    }
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
    private List<AttachedEffect> effectsAttached; 
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
        effectsAttached = new List<AttachedEffect>();

        PlayerInputManager.Instance.OnSkillSelectInput += OnSkillClick;
    }

    public void Update() {
        ManageEffectsAttached();
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
        if (skills[(int)_slot]!=null)
            ((BaseSpell)skills[(int)_slot]).Trigger(this, this);
    }

    //@TODO Find a more elegant way to do this
    private void ManageEffectsAttached() {
        if (effectsAttached.Count > 0) {
            for (int i = 0; i < effectsAttached.Count; ++i) {
                if (!effectsAttached[i].Self.IsActivated) {
                    effectsAttached[i].Self.Activate(effectsAttached[i].Caster, this);

                    if (effectsAttached[i].Self.Equals(typeof(OverTimeEffect)))
                        ((OverTimeEffect)effectsAttached[i].Self).LastActivationTime = Time.time;
                }
                if (effectsAttached[i].Self.Equals(typeof(OverTimeEffect))) {
                    if (((OverTimeEffect)effectsAttached[i].Self).IsReadyForNextActivation(Time.time)) {
                        effectsAttached[i].Self.Activate(effectsAttached[i].Caster, this);
                        ((OverTimeEffect)effectsAttached[i].Self).LastActivationTime = Time.time;
                    }
                    ((OverTimeEffect)effectsAttached[i].Self).OverTimeCountdownTimer -= Time.deltaTime;
                }
                effectsAttached[i].Self.CountdownTimer -= Time.deltaTime;

                if (!effectsAttached[i].Self.InProgress) {
                    effectsAttached[i].Self.Deactivate(this);
                    effectsAttached.Remove(effectsAttached[i]);
                }
            }
            Utilities.Instance.LogMessage("effect count:" + effectsAttached.Count);
        }
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

    public void SetSkill(int index, BaseSkill skill) {
        skills[index] = skill;
    }
    public BaseSkill GetSkill(int index) {
        return skills[index];
    }    
    public int SkillCount {
        get { return skills.Length; }
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