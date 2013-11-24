using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isSelected;
    private Requirements requirements;
    private List<BaseEffect> effects;

#endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isSelected = false;
        requirements = new Requirements();
        effects = null;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon) {
        title = _title;
        description = _desc;
        icon = _icon;
        isSelected = false;
        requirements = new Requirements();
        effects = new List<BaseEffect>();
    }
#endregion

    public abstract void Target(BaseCharacterModel _caster, CharacterSkillSlots _slot);
    public abstract void Cast(BaseCharacterModel _caster, Vector3 _destination);
    public abstract void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);

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
    public bool RequirementsFulfilled(BaseCharacterModel _char) {
        for (int i = 0; i < requirements.Minimum.Count; ++i)
            if (_char.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                return false;
        for (int i = 0; i < requirements.Maximum.Count; ++i)
            if (_char.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
                return false;
            return true;
    }

    public void AddEffect(BaseEffect _effect) {
        effects.Add(_effect);
    }
    public void RemoveEffect(BaseEffect _effect){
        effects.Remove(_effect);
    }
    public BaseEffect GetEffect(int index) {
        return effects[index];
    }
    public int EffectsCount {
        get { return effects.Count; }
    }

    public virtual bool IsReady(BaseCharacterModel _char) {
        return RequirementsFulfilled(_char) && !isSelected;
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