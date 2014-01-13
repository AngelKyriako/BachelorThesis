using UnityEngine;
using System.Collections.Generic;

public class BaseSkill {

    #region attributes
    private int id;
    private string title, description;
    private float coolDownTimer, coolDown;
    private float range;
    private uint manaCost;

    private List<BaseEffect> offensiveEffects,
                             supportEffects,
                             passiveEffects;
    private Requirements requirements;
    private BaseCharacterModel ownerModel;
    private CharacterSkillSlot slot;

    private string castEffect, mainObject, triggerEffect;
    #endregion

    public BaseSkill( int _id, string _title, string _desc, float _cd, float _range, string _castEff, string _mainObject, string _triggerEff) {
        id = _id;
        title = _title;
        description = _desc;
        coolDownTimer = coolDown = _cd;
        range = _range;
        manaCost = 0;
        requirements = new Requirements();
        offensiveEffects = new List<BaseEffect>();
        supportEffects = new List<BaseEffect>();
        passiveEffects = new List<BaseEffect>();
        castEffect = _castEff;
        mainObject = _mainObject;
        triggerEffect = _triggerEff;
    }

    public void SetUpSkill(BaseCharacterModel _owner, CharacterSkillSlot _slot){
        ownerModel = _owner;
        slot = _slot;
    }

    public virtual void OnFrameUpdate() {
        CoolDownTimer -= Time.deltaTime;
    }

    public void UpdateManaCost(BaseCharacterModel _characterModel) {
        manaCost = 0;
        for (int i = 0; i < OffensiveEffectCount; ++i)
            if (GetOffensiveEffect(i).RequirementsFulfilled(_characterModel))
                manaCost += GetOffensiveEffect(i).ManaCost;

        for (int i = 0; i < SupportEffectCount; ++i)
            if (GetSupportEffect(i).RequirementsFulfilled(_characterModel))
                manaCost += GetSupportEffect(i).ManaCost;

        for (int i = 0; i < PassiveEffectCount; ++i)
            if (GetPassiveEffect(i).RequirementsFulfilled(_characterModel))
                manaCost += GetPassiveEffect(i).ManaCost;
    }

    public virtual void Pressed() {
        if (IsUsable) {
            Cast(ownerModel.transform.forward);
            DFSkillModel.Instance.CastSkill(slot);
        }
        else if (!SufficientMana)
            GUIMessageDisplay.Instance.AddMessage("No juice");
        else if (coolDownTimer != 0f)
            GUIMessageDisplay.Instance.AddMessage("Chill for a sec dude");
    }

    public virtual void Unpressed() { }

    public virtual void Select() { }
    public virtual void Unselect() { }

    public virtual void Cast(Vector3 _direction) {
        ownerModel.GetVital((int)VitalType.Mana).CurrentValue -= manaCost;
        RefreshCooldown();

        if (castEffect != null && !castEffect.Equals(string.Empty))
            CombatManager.Instance.InstantiateNetworkObject(castEffect, ownerModel.transform.position, Quaternion.identity);

        if (mainObject != null && !mainObject.Equals(string.Empty))
            CombatManager.Instance.InstantiateNetworkSkill(mainObject, ownerModel.ProjectileOriginPosition, Quaternion.identity, id, ownerModel.name, _direction);
        ActivatePassiveEffects(ownerModel, ownerModel);

        CombatManager.Instance.RaiseSkillUsageCount(Id);
    }

    public virtual void Trigger(Vector3 _position, Quaternion _rotation) {
        if (triggerEffect != null && !triggerEffect.Equals(string.Empty))
            CombatManager.Instance.InstantiateNetworkObject(triggerEffect, _position, _rotation);
    }

    public void ActivateOffensiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        GameManager.Instance.MyNetworkController.AttachOffensiveEffectsToPlayer(_caster.NetworkController,
                                                                                _receiver.NetworkController,
                                                                                Id);
    }

    public void ActivateSupportEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        GameManager.Instance.MyNetworkController.AttachSupportEffectsToPlayer(_caster.NetworkController,
                                                                              _receiver.NetworkController,
                                                                              Id);
    }

    public void ActivatePassiveEffects(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        for (int i = 0; i < PassiveEffectCount; ++i)
            if (GetPassiveEffect(i).RequirementsFulfilled(_caster))
                CombatManager.Instance.AttachEffectToSelf(_caster.name, GetPassiveEffect(i));
    }

    private void RefreshCooldown() {
        CoolDownTimer = CasterBasedCoolDown;
    }

    #region Accessors
    public int Id {
        get { return id; }
        set { id = value; }
    }
    public string Title {
        get { return title; }
        set { title = value; }
    }
    public string Description {
        get { return description + "\n" + MinRequirementsToString + "\n" + MaxRequirementsToString; }
        set { description = value; }
    }
    public float CoolDownTimer {
        get { return coolDownTimer; }
        set { coolDownTimer = value > 0 ? value : 0; }
    }
    public float CoolDown {
        get { return coolDown; }
    }
    public float CasterBasedCoolDown {
        get { return coolDown - coolDown * ownerModel.GetAttribute((int)AttributeType.AttackSpeed).FinalValue; ; }
    }
    public float Range {
        get { return range; }
        set { range = value; }
    }
    public uint ManaCost {
        get { return manaCost; }
    }

    public virtual bool IsUsable {
        get { return (coolDownTimer == 0f) &&
                     !ownerModel.IsStunned &&
                     !slot.Equals(CharacterSkillSlot.None) &&
                     SufficientMana &&
                     RequirementsFulfilled();
        }
    }
    public bool SufficientMana {
        get { return ownerModel.GetVital((int)VitalType.Mana).CurrentValue >= manaCost; }
    }
    public virtual bool IsSelected {
        get { return true; }
        set { }
    }

    #region requirements
    public void AddMinimumRequirement(StatType _stat, int _value) {
        requirements.Minimum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public void AddMaximumRequirement(StatType _stat, int _value) {
        requirements.Maximum.Add(new Pair<int, int>((int)_stat, _value));
    }
    public bool RequirementsFulfilled() {
        for (int i = 0; i < requirements.Minimum.Count; ++i)
            if (GameManager.Instance.MyCharacterModel.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                return false;
        for (int i = 0; i < requirements.Maximum.Count; ++i)
            if (GameManager.Instance.MyCharacterModel.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
                return false;
        return true;
    }

    private string MinRequirementsToString {
        get {
            string temp = "Min: ";
            for (int i = 0; i < requirements.Minimum.Count; ++i)
                if (GameManager.Instance.MyCharacterModel.GetStat(requirements.Minimum[i].First).FinalValue < requirements.Minimum[i].Second)
                    temp += ((StatType)requirements.Minimum[i].First).ToString() + ": " + requirements.Minimum[i].Second + " ";
            if (!temp.Equals("Min: "))
                return temp;
            else
                return string.Empty;
            
        }
    }
    private string MaxRequirementsToString {
        get {
            string temp = "Max: ";
            for (int i = 0; i < requirements.Maximum.Count; ++i)
                if (GameManager.Instance.MyCharacterModel.GetStat(requirements.Maximum[i].First).FinalValue > requirements.Maximum[i].Second)
                    temp += ((StatType)requirements.Maximum[i].First).ToString() + ": " + requirements.Maximum[i].Second + " ";
            if (!temp.Equals("Max: "))
                return temp;
            else
                return string.Empty;
        }
    }
    #endregion

    #region Effects
    //offensive
    public void AddOffensiveEffect(BaseEffect _effect) {
        Utilities.Instance.PreCondition(_effect != null, "BaseSkill", "AddOffensiveEffect", "effect argument is null");
        offensiveEffects.Add(_effect);
    }
    public BaseEffect GetOffensiveEffect(int _index) {
        return offensiveEffects[_index];
    }
    public int OffensiveEffectCount {
        get { return offensiveEffects.Count; }
    }
    //support
    public void AddSupportEffect(BaseEffect _effect) {
        Utilities.Instance.PreCondition(_effect != null, "BaseSkill", "AddSupportEffect", "effect argument is null");
        supportEffects.Add(_effect);
    }
    public BaseEffect GetSupportEffect(int _index) {
        return supportEffects[_index];
    }
    public int SupportEffectCount {
        get { return supportEffects.Count; }
    }
    //passive
    public void AddPassiveEffect(BaseEffect _effect) {
        Utilities.Instance.PreCondition(_effect != null, "BaseSkill", "AddPassiveEffect", "effect argument is null");
        passiveEffects.Add(_effect);
    }
    public BaseEffect GetPassiveEffect(int _index) {
        return passiveEffects[_index];
    }
    public int PassiveEffectCount {
        get { return passiveEffects.Count; }
    }
    #endregion

    public BaseCharacterModel OwnerModel {
        get { return ownerModel; }
    }
    public CharacterSkillSlot Slot {
        get { return slot; }
        set { slot = value; }
    }

    #endregion

    public class Requirements {
        public List<Pair<int, int>> Minimum, Maximum;
        public Requirements() {
            Minimum = new List<Pair<int, int>>();
            Maximum = new List<Pair<int, int>>();
        }
    }
}