using UnityEngine;
using System;
using System.Collections.Generic;

public struct VitalModifier {
    public EffectModifier Current;
    public EffectModifier Max;

    public VitalModifier(EffectModifier _current, EffectModifier _max) {
        Current = _current;
        Max = _max;
    }
}

public class VitalModifierEffect: BaseEffect {

    private DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int> currentValueDispatcher;

    private VitalType vital;
    private VitalModifier modifier;
    private int buffValue;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration, VitalType _vital, VitalModifier _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive, _duration);
        vital = _vital;
        modifier = _modifier;
        buffValue = 0;
        SetUpVitalDispatcheTable();
    }

    public virtual void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        vital = ((VitalModifierEffect)_effect).Vital;
        modifier = ((VitalModifierEffect)_effect).Modifier;
        enabled = true;
    }

    public void Activate(BaseCharacterModel _caster, BaseCharacterModel _receiver) {
        //max
        buffValue = (int)modifier.Max.RawValue + (int)(modifier.Max.PercentageValue * _receiver.GetVital((int)vital).FinalValue);
        _receiver.GetVital((int)vital).BuffValue += buffValue;
        //current
        _receiver.GetVital((int)vital).CurrentValue += currentValueDispatcher.Dispatch(vital, (int)modifier.Current.RawValue, _caster, _receiver) +
                                                       (int)(modifier.Current.PercentageValue * _receiver.GetVital((int)vital).CurrentValue);        
    }

    public void Deactivate(BaseCharacterModel _receiver) {
        _receiver.GetVital((int)vital).BuffValue -= buffValue;
    }

    private void SetUpVitalDispatcheTable(){
        currentValueDispatcher = new DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int>();

        currentValueDispatcher.AddAction(VitalType.Health, delegate(int rawValue, BaseCharacterModel _caster, BaseCharacterModel _receiver){
            return (int)((rawValue<0)?
                          //Physical Damage
                          rawValue*(_caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                    _receiver.GetAttribute((int)AttributeType.Defence).FinalValue)
                          ://Health Healing
                          rawValue * (_caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      (_receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
                        );
        });
        currentValueDispatcher.AddAction(VitalType.Mana, delegate(int rawValue, BaseCharacterModel _caster, BaseCharacterModel _receiver) {
            return (int)((rawValue < 0) ?
                          //Magic Damage
                          rawValue * (_caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
                                      _receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue)
                          ://Mana Healing
                          rawValue * (_caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      (_receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
                        );
        });
    }

    public VitalType Vital {
        get { return vital; }
        set { vital = value; }
    }
    public VitalModifier Modifier {
        get { return modifier; }
        set { modifier = value; }
    }
}