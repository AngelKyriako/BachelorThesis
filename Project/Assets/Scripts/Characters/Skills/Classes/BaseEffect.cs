﻿using UnityEngine;

public enum EffectType {
    VitalModifierDirect,
    VitalModifierOverTime,
    StatModifierDirect,
    StatModifierOverTime,
    Stun,
    Invulnerable
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

    public BaseEffect(string _title, string _descr, Texture2D _icon) {
        title = _title;
        description = _descr;
        icon = _icon;
    }

    public abstract void Activate();

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

    public EffectType Type {
        get { return type; }
        set { type = value; }
    }
#endregion
}
