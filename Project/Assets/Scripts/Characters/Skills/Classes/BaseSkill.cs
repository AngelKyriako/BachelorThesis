using UnityEngine;
using System.Collections.Generic;

public enum SkillType {
    UnknownSkill_T,
    BaseSpell_T
}

public abstract class BaseSkill: IBaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private List<BaseEffect> effects;
#endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        effects = null;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon) {
        title = _title;
        description = _desc;
        icon = _icon;
        effects = new List<BaseEffect>();
    }

    //public BaseSkill(string _title, string _desc, Texture2D _icon, float _cd, GameObject _cast, GameObject _trigger) {
    //    title = _title;
    //    description = _desc;
    //    icon = _icon;
    //}
#endregion

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

    public virtual SkillType GetSkillType() { return SkillType.UnknownSkill_T; }
}