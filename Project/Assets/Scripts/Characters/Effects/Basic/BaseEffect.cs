using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectMod {
    public float RawValue;
    public float PercentageValue;

    public EffectMod(float _RawValue, float _percent) {
        RawValue = _RawValue;
        PercentageValue = _percent;
    }
}

public abstract class BaseEffect: MonoBehaviour {

    #region attributes
    private BaseCharacterModel caster;
    private string title, description;
    private Texture2D icon;
    private bool isPassive;
    private uint manaCost, levelRequirement;
#endregion

    public virtual void Awake() {
        caster = null;
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isPassive = false;
        enabled = false;
        manaCost = 50;
        levelRequirement = 0;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive) {
        title = _title;
        description = _descr;
        icon = _icon;
        isPassive = _isPassive;
        manaCost = 50;
        levelRequirement = 0;
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
        Activate();
        Deactivate();
    }

    public abstract void Activate();

    public virtual void Deactivate() { Destroy(this); }

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
    public bool IsPassive {
        get { return isPassive; }
    }
    public uint ManaCost {
        get { return manaCost; }
    }
    public bool RequirementsFulfilled(BaseCharacterModel _characterModel) {
        return (_characterModel.Level >= levelRequirement);
    }
    #endregion
}