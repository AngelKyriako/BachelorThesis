using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseEffect: MonoBehaviour{

    #region attributes
    private string title, description;
    private Texture2D icon;
    private bool isActivated, isPassive;
    private float countdownTimer;
#endregion

    public virtual void Awake() {
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isActivated = false;
        isPassive = false;
        countdownTimer = 0;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration) {
        title = _title;
        description = _descr;
        icon = _icon;
        isPassive = _isPassive;
        countdownTimer = _duration;
    }

    public virtual void Activate(BaseCharacterModel caster, BaseCharacterModel receiver) {
        isActivated = true;
    }

    public virtual void Deactivate(BaseCharacterModel _receiver) {
        isActivated = false;
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

    public bool IsActivated {
        get { return isActivated; }
    }
    public bool IsPassive {
        get { return isPassive; }
        set { isPassive = value; }
    }

    public float CountdownTimer {
        get { return countdownTimer; }
        set { countdownTimer = value; }
    }

    public bool InProgress {
        get { return countdownTimer > 0; }
    }

    public virtual bool IsOverTimeEffect {
        get { return false; }
    }

#endregion
}