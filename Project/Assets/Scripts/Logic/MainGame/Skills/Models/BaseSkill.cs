using UnityEngine;
using System.Collections.Generic;

public class BaseSkill {

    #region attributes
    private int id;
    private string title, description;
    private float coolDownTimer, coolDown;
    private uint manaCost;

    private Dictionary<int, BaseEffect> offensiveEffects,
                                        supportEffects,
                                        passiveEffects;
    private Requirements requirements;
    private BaseCharacterModel ownerModel;
    private CharacterSkillSlot slot;

    private string castEffect, mainObject, triggerEffect;
    #endregion

    public BaseSkill( int _id, string _title, string _desc, float _cd, string _castEff, string _mainObject, string _triggerEff) {
        id = _id;
        title = _title;
        description = _desc;
        coolDownTimer = coolDown = _cd;
        manaCost = 0;
        requirements = new Requirements();
        offensiveEffects = new Dictionary<int, BaseEffect>();
        supportEffects = new Dictionary<int, BaseEffect>();
        passiveEffects = new Dictionary<int, BaseEffect>();
        castEffect = _castEff;
        mainObject = _mainObject;
        triggerEffect = _triggerEff;
    }

    public void SetUpSkill(BaseCharacterModel _owner, CharacterSkillSlot _slot){
        ownerModel = _owner;
        slot = _slot;
    }

    public virtual void OnFrameUpdate() {
        CoolDownTimer -= Time.deltaTime;
    }

    public void UpdateManaCost(BaseCharacterModel _characterModel) {
        manaCost = 0;
        foreach (int _id in offensiveEffects.Keys)
            if (offensiveEffects[_id].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[_id].ManaCost;
        foreach (int _id in supportEffects.Keys)
            if (offensiveEffects[_id].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[_id].ManaCost;
    }

    public virtual void Pressed() {
        if (IsUsable) {
            Cast(ownerModel.transform.forward);
            DFSkillModel.Instance.CastSkill(slot);
        }
        else if (!SufficientMana)
            ;//Insufficient mana //@TODO: message
        else if (coolDownTimer != 0f)
            ;//in cooldown //@TODO: message
    }

    public virtual void Unpressed() { }

    public virtual void Select() { }
    public virtual void Unselect() { }

    public virtual void Cast(Vector3 _direction) {
        ownerModel.GetVital((int)VitalType.Mana).CurrentValue -= manaCost;
        RefreshCooldown();

        if (castEffect != null && !castEffect.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneObject(castEffect, ownerModel.transform.position, Quaternion.identity);

        if (mainObject != null && !mainObject.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneSkill(mainObject, ownerModel.ProjectileOriginPosition,
                                                                     Quaternion.identity, id, ownerModel.name, _direction);
        ActivatePassiveEffects(ownerModel, ownerModel);
    }

    public virtual void Trigger(Vector3 _position, Quaternion _rotation) {
        if (triggerEffect != null && !triggerEffect.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneObject(triggerEffect, _position, _rotation);
    }

    public virtual void ActivateOffensiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        if (_receiver)
            foreach (int _effectId in OffensiveEffectKeys)
                if (GetOffensiveEffect(_effectId).RequirementsFulfilled(_caster))
                    GameManager.Instance.MasterClientNetworkController.AttachEffectToPlayer(_caster.NetworkController,
                                                                                            _receiver.NetworkController,
                                                                                            _effectId);
    }

    public virtual void ActivateSupportEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        foreach (int _effectId in SupportEffectKeys)
            if (GetSupportEffect(_effectId).RequirementsFulfilled(_caster))
                GameManager.Instance.MasterClientNetworkController.AttachEffectToPlayer(_caster.NetworkController,
                                                                                        _receiver.NetworkController,
                                                                                        _effectId);
    }

    public virtual void ActivatePassiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        foreach (int _effectId in PassiveEffectKeys)
            if (GetPassiveEffect(_effectId).RequirementsFulfilled(_caster))
                _caster.NetworkController.AttachEffect(_caster.name, _receiver.name, _effectId);
    }

    private void RefreshCooldown() {
        CoolDownTimer = CasterCoolDown;
    }

    #region Accessors
    public int Id {
        get { return id; }
        set { id = value; }
    }
    public string Title {
        get { return title; }
        set { title = value; }
    }
    public string Description {
        get { return description; }
        set { description = value; }
    }
    public float CoolDownTimer {
        get { return coolDownTimer; }
        set { coolDownTimer = value > 0 ? value : 0; }
    }
    public float CoolDown {
        get { return coolDown; }
    }
    public float CasterCoolDown {
        get { return coolDown - coolDown * ownerModel.GetAttribute((int)AttributeType.AttackSpeed).FinalValue; ; }
    }
    public uint ManaCost {
        get { return manaCost; }
    }
    public virtual bool IsUsable {
        get { return (coolDownTimer == 0f) &&
                     !ownerModel.IsStunned &&
                     !slot.Equals(CharacterSkillSlot.None) &&
                     SufficientMana &&
                     RequirementsFulfilled();
        }
    }
    public bool SufficientMana {
        get { return ownerModel.GetVital((int)VitalType.Mana).CurrentValue >= manaCost; }
    }
    public virtual bool IsSelected {
        get { return true; }
        set { }
    }

    #region requirements
    public void AddMinimumRequirement(StatType _stat, int _value) {
        requirements.Minimum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public void AddMaximumRequirement(StatType _stat, int _value) {
        requirements.Maximum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public bool RequirementsFulfilled() {
        for (int i = 0; i < requirements.Minimum.Count; ++i)
            if (ownerModel.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                return false;
        for (int i = 0; i < requirements.Maximum.Count; ++i)
            if (ownerModel.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
                return false;
        return true;
    }
    #endregion

    #region Effects
    //offensive
    public void AddOffensiveEffect(BaseEffect _effect) {
        offensiveEffects.Add(_effect.Id, _effect);
    }
    public void RemoveOffensiveEffect(BaseEffect _effect) {
        offensiveEffects.Remove(_effect.Id);
    }
    public BaseEffect GetOffensiveEffect(int key) {
        return offensiveEffects[key];
    }
    public ICollection<int> OffensiveEffectKeys {
        get { return offensiveEffects.Keys; }
    }
    //support
    public void AddSupportEffect(BaseEffect _effect) {
        supportEffects.Add(_effect.Id, _effect);
    }
    public void RemoveSupportEffect(BaseEffect _effect) {
        supportEffects.Remove(_effect.Id);
    }
    public BaseEffect GetSupportEffect(int key) {
        return supportEffects[key];
    }
    public ICollection<int> SupportEffectKeys {
        get { return supportEffects.Keys; }
    }
    //passive
    public void AddPassiveEffect(BaseEffect _effect) {
        passiveEffects.Add(_effect.Id, _effect);
    }
    public void RemovePassiveEffect(BaseEffect _effect) {
        passiveEffects.Remove(_effect.Id);
    }
    public BaseEffect GetPassiveEffect(int key) {
        return passiveEffects[key];
    }
    public ICollection<int> PassiveEffectKeys {
        get { return passiveEffects.Keys; }
    }
    #endregion

    public BaseCharacterModel OwnerModel {
        get { return ownerModel; }
    }
    public CharacterSkillSlot Slot {
        get { return slot; }
        set { slot = value; }
    }

    #endregion

    public class Requirements {
        public List<Pair<int, int>> Minimum, Maximum;
        public Requirements() {
            Minimum = new List<Pair<int, int>>();
            Maximum = new List<Pair<int, int>>();
        }
    }
}