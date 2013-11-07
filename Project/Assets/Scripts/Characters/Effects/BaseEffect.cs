using UnityEngine;

public enum EffectType {
    UnknownEffect_T,
    StatModifierEffect_T
}

public abstract class BaseEffect {

    private string title, description;
    private Texture2D icon;
    
    public BaseEffect() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
    }

    public BaseEffect(string _title, string _descr, Texture2D _icon) {
        title = _title;
        description = _descr;
        icon = _icon;
    }

    public abstract void Activate(PlayerCharacterModel caster, PlayerCharacterModel receiver);
    public abstract void Deactivate(PlayerCharacterModel caster, PlayerCharacterModel receiver);

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
#endregion

    public virtual EffectType GetEffectType() { return EffectType.UnknownEffect_T; }
}
