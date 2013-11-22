using UnityEngine;
using System.Collections.Generic;

public enum SkillType {
    UnknownSkill_T,
    BaseSpell_T
}

public abstract class BaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isSelected;
    private List<BaseEffect> effects;
#endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isSelected = false;
        effects = null;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon) {
        title = _title;
        description = _desc;
        icon = _icon;
        isSelected = false;
        effects = new List<BaseEffect>();
    }
#endregion

    public abstract void Target(BaseCharacterModel _caster, CharacterSkillSlots _slot);
    public abstract void Cast(BaseCharacterModel _caster, Vector3 _destination);
    public abstract void Trigger(BaseCharacterModel _caster, BaseCharacterModel _receiver);
    public abstract void ActivateEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver);

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
#endregion

    public virtual SkillType Type{
        get { return SkillType.UnknownSkill_T; }
    }
}