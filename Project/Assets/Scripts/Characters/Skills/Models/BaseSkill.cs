using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isSelected;    
    private Dictionary<string, BaseEffect> offensiveEffects;
    private Dictionary<string, BaseEffect> supportEffects;
    private Requirements requirements;

#endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isSelected = false;
        requirements = null;
        offensiveEffects = null;
        supportEffects = null;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon) {
        title = _title;
        description = _desc;
        icon = _icon;
        isSelected = false;
        requirements = new Requirements();
        offensiveEffects = new Dictionary<string, BaseEffect>();
        supportEffects = new Dictionary<string, BaseEffect>();
    }
#endregion

    public abstract void Target(BaseCharacterModel _caster, CharacterSkillSlot _slot);
    public abstract void Cast(BaseCharacterModel _caster, Vector3 _destination);
    public abstract void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateOffensiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateSupportEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);

    public virtual void Update() { }

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
    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }

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

    public virtual bool IsReady(BaseCharacterModel _characterModel) {
        return RequirementsFulfilled(_characterModel) && !isSelected;
    }

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