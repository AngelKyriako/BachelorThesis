using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private float coolDownTimer, coolDownTime;
    private uint manaCost;
    private Dictionary<string, BaseEffect> offensiveEffects;
    private Dictionary<string, BaseEffect> supportEffects;
    private Requirements requirements;
    private bool isSelected;
    #endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        coolDownTimer = coolDownTime = 0f;
        manaCost = 0;
        requirements = null;
        offensiveEffects = null;
        supportEffects = null;        
        isSelected = false;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon, float _cd) {
        title = _title;
        description = _desc;
        icon = _icon;
        coolDownTimer = coolDownTime = _cd;
        manaCost = 0;
        requirements = new Requirements();
        offensiveEffects = new Dictionary<string, BaseEffect>();
        supportEffects = new Dictionary<string, BaseEffect>();
        isSelected = false;
    }
    #endregion

    public virtual void OnFrameUpdate() {
        CoolDownTimer -= Time.deltaTime;
    }

    public void UpdateManaCost(BaseCharacterModel _characterModel) {
        manaCost = 10;
        foreach (string title in offensiveEffects.Keys)
            if (offensiveEffects[title].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[title].ManaCost;
        foreach (string title in supportEffects.Keys)
            if (offensiveEffects[title].RequirementsFulfilled(_characterModel))
                manaCost += offensiveEffects[title].ManaCost;
    }

    public abstract void Select(BaseCharacterModel _caster, CharacterSkillSlot _slot);
    public virtual void Cast(BaseCharacterModel _caster, Vector3 _destination) {
        _caster.GetVital((int)VitalType.Mana).CurrentValue -= manaCost;
        coolDownTimer = coolDownTime - (coolDownTime * _caster.GetAttribute((int)AttributeType.AttackSpeed).FinalValue);
    }
    public abstract void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateOffensiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateSupportEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);

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
    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public bool IsCastableBy(BaseCharacterModel _characterModel) {
        return (_characterModel.GetVital((int)VitalType.Mana).CurrentValue >= manaCost);
    }
    public virtual bool IsUsable(BaseCharacterModel _characterModel) {
        return (!isSelected && (coolDownTimer == 0f) && RequirementsFulfilled(_characterModel));
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