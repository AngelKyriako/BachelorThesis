using UnityEngine;
using System;
using System.Collections.Generic;

public struct EffectModifier {
    public int Raw;
    public float Percent;

    public EffectModifier(int _raw, float _percent) {
        Raw = _raw;
        Percent = _percent;
    }
}

public class BaseEffect: MonoBehaviour{

    #region attributes
    private BaseCharacterModel caster, receiver;
    private string title, description;
    private Texture2D icon;
    private bool isActivated, isPassive;
    private float countdownTimer;
#endregion

    public virtual void Awake() {
        caster = receiver = null;
        title = string.Empty;
        description = string.Empty;
        icon = null;
        isActivated = false;
        isPassive = false;
        countdownTimer = 0;
        enabled = false;
    }

    public virtual void Update() {
        if (!isActivated)
            Activate();
        else if (!InProgress)
            Deactivate();
        countdownTimer -= Time.deltaTime;
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration) {
        title = _title;
        description = _descr;
        icon = _icon;
        isPassive = _isPassive;
        countdownTimer = _duration;
    }

    public virtual void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        caster = _caster;
        receiver = gameObject.GetComponent<BaseCharacterModel>();
        title = _effect.Title;
        description = _effect.Description;
        icon = _effect.Icon;
        isPassive = _effect.IsPassive;
        countdownTimer = _effect.CountdownTimer;
        enabled = true;
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
        get { return receiver; }
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

    public float CountdownTimer {
        get { return countdownTimer; }
    }

    public bool InProgress {
        get { return countdownTimer > 0; }
    }

    public virtual bool IsOverTimeEffect {
        get { return false; }
    }

#endregion
}