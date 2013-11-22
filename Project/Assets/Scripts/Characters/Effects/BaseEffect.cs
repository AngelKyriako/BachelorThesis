using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseEffect: MonoBehaviour {

    #region attributes
    private BaseCharacterModel caster;
    private string title, description;
    private Texture2D icon;
    private bool isActivated, isPassive;
    #endregion

    public virtual void Awake() {
        caster = null;
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isActivated = false;
        isPassive = false;
        enabled = false;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive) {
        title = _title;
        description = _descr;
        icon = _icon;
        isPassive = _isPassive;
    }

    public virtual void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        caster = _caster;
        title = _effect.Title;
        description = _effect.Description;
        icon = _effect.Icon;
        isPassive = _effect.IsPassive;
        enabled = true;
    }

    public virtual void Update() {
        if (!isActivated)
            Activate();
        else
            Deactivate();
    }

    public virtual void Activate() {
        isActivated = true;
    }

    public virtual void Deactivate() {
        Destroy(this);
    }

    #region Accessors
    public BaseCharacterModel Caster {
        get { return caster; }
    }
    public BaseCharacterModel Receiver {
        get { return gameObject.GetComponent<BaseCharacterModel>(); }
    }

    public string Title {
        get { return title; }
    }
    public string Description {
        get { return description; }
    }
    public Texture2D Icon {
        get { return icon; }
    }

    public bool IsActivated {
        get { return isActivated; }
        set { isActivated = value; }
    }
    public bool IsPassive {
        get { return isPassive; }
    }
    #endregion
}