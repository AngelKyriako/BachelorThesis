using UnityEngine;

public enum EffectType {
    DirectDamage,
    DamageOverTime,
    Heal,
    Buff,
    DeBuff,
    Stun
}

public abstract class BaseEffect {

    private string title, description;
    private Texture2D icon;
    private EffectType type;
    
    public BaseEffect() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
    }

    public BaseEffect(string ttl, string dsc, Texture2D icn) {
        title = ttl;
        description = dsc;
        icon = icn;
    }

    public abstract void Activate();

#region Setters and Getters
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

    public EffectType Type {
        get { return type; }
        set { type = value; }
    }
#endregion
}
