using UnityEngine;

public class BaseSkill: IBaseSkill {

    #region attributes
    private string title, description;
    private Texture2D icon;
    private BaseEffect[] effects;
#endregion

    #region constructors
    public BaseSkill() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        effects = null;
    }

    public BaseSkill(string _title, string _desc, Texture2D _icon, int _effectsCount) {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        effects = new BaseEffect[_effectsCount];
    }

    //public BaseSkill(string _title, string _desc, Texture2D _icon, float _cd, GameObject _cast, GameObject _trigger) {
    //    title = string.Empty;
    //    description = string.Empty;
    //    icon = null;
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
    public void SetEffect(int index, BaseEffect effect) {
        effects[index] = effect;
    }
    public BaseEffect GetEffect(int index) {
        return effects[index];
    }
    public int EffectsCount {
        get { return effects.Length; }
    }
#endregion
}