using UnityEngine;
using System.Collections.Generic;

public class BaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private float coolDownTimer, coolDownTime;
    private uint manaCost;
    private Dictionary<string, BaseEffect> offensiveEffects,
                                           supportEffects,
                                           passiveEffects;
    private Requirements requirements;
    private BaseCharacterModel owner;
    private CharacterSkillSlot slot;

    private string castEffect, projectile, triggerEffect;
    #endregion

    public BaseSkill(string _title, string _desc, Texture2D _icon, float _cd, string _castEff, string _projectile, string _triggerEff) {
        title = _title;
        description = _desc;
        icon = _icon;
        coolDownTimer = coolDownTime = _cd;
        manaCost = 0;
        requirements = new Requirements();
        offensiveEffects = new Dictionary<string, BaseEffect>();
        supportEffects = new Dictionary<string, BaseEffect>();
        passiveEffects = new Dictionary<string, BaseEffect>();
        castEffect = _castEff;
        projectile = _projectile;
        triggerEffect = _triggerEff;
    }

    public void SetUpSkill(BaseCharacterModel _owner, CharacterSkillSlot _slot){
        owner = _owner;
        slot = _slot;
    }

    public virtual void OnFrameUpdate() {
        CoolDownTimer -= Time.deltaTime;
    }

    public void UpdateManaCost(BaseCharacterModel _characterModel) {
        manaCost = 0;
        foreach (string title in offensiveEffects.Keys)
            if (offensiveEffects[title].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[title].ManaCost;
        foreach (string title in supportEffects.Keys)
            if (offensiveEffects[title].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[title].ManaCost;
    }

    public virtual void Pressed() {
        if (IsUsable)
            Cast(owner.transform.forward);
    }

    public virtual void Unpressed() { }

    public virtual void Select() { }
    public virtual void Unselect() { }

    public virtual void Cast(Vector3 _direction) {
        owner.GetVital((int)VitalType.Mana).CurrentValue -= manaCost;
        RefreshCooldown();

        if (castEffect != null && !castEffect.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneObject(castEffect, owner.transform.position, Quaternion.identity);

        if (projectile != null && !projectile.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneProjectile(projectile, owner.ProjectileOriginPosition, Quaternion.identity, title, owner.name, _direction);

        ActivatePassiveEffects(owner, owner);
    }

    public virtual void Trigger(Vector3 _position, Quaternion _rotation) {
        if (triggerEffect != null && !triggerEffect.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneObject(triggerEffect, _position, _rotation);
    }

    public virtual void ActivateOffensiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        if (_receiver)
            foreach (string effectTitle in OffensiveEffectKeys)
                if (GetOffensiveEffect(effectTitle).RequirementsFulfilled(_caster))
                    GameManager.Instance.MasterClientNetworkController.AttachEffectToPlayer(_caster.NetworkController,
                                                                                            _receiver.NetworkController,
                                                                                            effectTitle);
    }

    public virtual void ActivateSupportEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        foreach (string effectTitle in SupportEffectKeys)
            if (GetSupportEffect(effectTitle).RequirementsFulfilled(_caster))
                GameManager.Instance.MasterClientNetworkController.AttachEffectToPlayer(_caster.NetworkController,
                                                                                        _receiver.NetworkController,
                                                                                        effectTitle);
    }

    public virtual void ActivatePassiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        foreach (string effectTitle in PassiveEffectKeys)
            if (GetPassiveEffect(effectTitle).RequirementsFulfilled(_caster))
                _caster.NetworkController.AttachEffect(_caster.name, _receiver.name, effectTitle);
    }

    private void RefreshCooldown() {
        CoolDownTimer = coolDownTime - (coolDownTime * owner.GetAttribute((int)AttributeType.AttackSpeed).FinalValue);
    }

    #region Accessors
    public string Title {
        get { return title; }
        set { title = value; }
    }
    public string Description {
        get { return description; }
        set { description = value; }
    }
    public Texture2D Icon {
        get { return icon; }
        set { icon = value; }
    }
    public float CoolDownTimer {
        get { return coolDownTimer; }
        set { coolDownTimer = value > 0 ? value : 0; }
    }
    public virtual bool IsUsable {
        get { return (coolDownTimer == 0f) && (owner.GetVital((int)VitalType.Mana).CurrentValue >= manaCost) && RequirementsFulfilled(); }
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
            if (owner.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                return false;
        for (int i = 0; i < requirements.Maximum.Count; ++i)
            if (owner.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
                return false;
        return true;
    }
    #endregion

    #region Effects
    //offensive
    public void AddOffensiveEffect(BaseEffect _effect) {
        offensiveEffects.Add(_effect.Title, _effect);
    }
    public void RemoveOffensiveEffect(BaseEffect _effect) {
        offensiveEffects.Remove(_effect.Title);
    }
    public BaseEffect GetOffensiveEffect(string key) {
        return offensiveEffects[key];
    }
    public ICollection<string> OffensiveEffectKeys {
        get { return offensiveEffects.Keys; }
    }
    //support
    public void AddSupportEffect(BaseEffect _effect) {
        supportEffects.Add(_effect.Title, _effect);
    }
    public void RemoveSupportEffect(BaseEffect _effect) {
        supportEffects.Remove(_effect.Title);
    }
    public BaseEffect GetSupportEffect(string key) {
        return supportEffects[key];
    }
    public ICollection<string> SupportEffectKeys {
        get { return supportEffects.Keys; }
    }
    //passive
    public void AddPassiveEffect(BaseEffect _effect) {
        passiveEffects.Add(_effect.Title, _effect);
    }
    public void RemovePassiveEffect(BaseEffect _effect) {
        passiveEffects.Remove(_effect.Title);
    }
    public BaseEffect GetPassiveEffect(string key) {
        return passiveEffects[key];
    }
    public ICollection<string> PassiveEffectKeys {
        get { return passiveEffects.Keys; }
    }
    #endregion

    public BaseCharacterModel Owner {
        get { return owner; }
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