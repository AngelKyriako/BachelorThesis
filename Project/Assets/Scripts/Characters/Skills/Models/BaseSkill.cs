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

    public virtual void Select(BaseCharacterModel _caster, CharacterSkillSlot _slot) {
        if (IsUsable(_caster))
            Cast(_caster, _caster.transform.forward);
    }

    public virtual void Cast(BaseCharacterModel _caster, Vector3 _direction) {        
        _caster.GetVital((int)VitalType.Mana).CurrentValue -= manaCost;
        coolDownTimer = coolDownTime - (coolDownTime * _caster.GetAttribute((int)AttributeType.AttackSpeed).FinalValue);
        if (!castEffect.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneObject(castEffect, _caster.transform.position, Quaternion.identity);

        if (!projectile.Equals(string.Empty))
            CombatManager.Instance.MasterClientInstantiateSceneProjectile(projectile, _caster.transform.position, Quaternion.identity, title, _caster.name, _direction);
    }

    public virtual void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver, Vector3 _position, Quaternion _rotation) {
        if (!triggerEffect.Equals(string.Empty))
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
                GameManager.Instance.MasterClientNetworkController.AttachEffectToPlayer(_caster.NetworkController,
                                                                                        _receiver.NetworkController,
                                                                                        effectTitle);
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

    public bool IsCastableBy(BaseCharacterModel _characterModel) {
        return (_characterModel.GetVital((int)VitalType.Mana).CurrentValue >= manaCost);
    }
    public virtual bool IsUsable(BaseCharacterModel _characterModel) {
        return ((coolDownTimer == 0f) && RequirementsFulfilled(_characterModel));
    }

    #region requirements
    public void AddMinimumRequirement(StatType _stat, int _value) {
        requirements.Minimum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public void AddMaximumRequirement(StatType _stat, int _value) {
        requirements.Maximum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public bool RequirementsFulfilled(BaseCharacterModel _characterModel) {
        for (int i = 0; i < requirements.Minimum.Count; ++i)
            if (_characterModel.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                return false;
        for (int i = 0; i < requirements.Maximum.Count; ++i)
            if (_characterModel.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
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

    #endregion

    public class Requirements {
        public List<Pair<int, int>> Minimum, Maximum;
        public Requirements() {
            Minimum = new List<Pair<int, int>>();
            Maximum = new List<Pair<int, int>>();
        }
    }
}