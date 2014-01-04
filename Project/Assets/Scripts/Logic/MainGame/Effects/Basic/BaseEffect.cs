using UnityEngine;
using System;
using System.Collections.Generic;

public enum EffectType {
    None,
    Damage,
    HealthHeal,
    ManaBurn,
    ManaHeal,
    Buff,
    DeBuff,
    Stun,
    Silence,
    Invulnerability
}

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
    private int id;
    private EffectType type;
    private string title, description;
    private uint manaCost, levelRequirement;
    #endregion

    public virtual void Awake() {
        caster = null;
        title = string.Empty;
        description = string.Empty;
        id = 0;
        type = EffectType.None;
        enabled = false;
        manaCost = 0;
        levelRequirement = 0;
    }

    public void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq) {
        id = _id;
        type = _type;
        title = _title;
        description = _descr;
        manaCost = _manaCost;
        levelRequirement = _minLevelReq;
    }

    public virtual void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        caster = _caster;
        title = _effect.Title;
        description = _effect.Description;
        id = _effect.Id;
        manaCost = _effect.ManaCost;
        levelRequirement = _effect.levelRequirement;
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
    public GameObject ReceiverGameObject {
        get { return gameObject; }
    }

    public bool CasterCriticalHit {
        get { return Utilities.Instance.GotLucky(Caster.GetAttribute((int)AttributeType.Critical).FinalValue); }
    }

    public bool ReceiverEvaded {
        get { return Utilities.Instance.GotLucky(Receiver.GetAttribute((int)AttributeType.Evasion).FinalValue); }
    }

    public bool IsEffectingSelf {
        get { return Caster.name.Equals(Receiver.name); }
    }

    public int Id {
        get { return id; }
    }
    public EffectType Type {
        get { return type; }
        set { type = value; }
    }
    public string Title {
        get { return title; }
    }
    public string Description {
        get { return description; }
    }
    public uint ManaCost {
        get { return manaCost; }
    }
    public bool RequirementsFulfilled(BaseCharacterModel _characterModel) {
        return (_characterModel.Level >= levelRequirement);
    }
    #endregion
}