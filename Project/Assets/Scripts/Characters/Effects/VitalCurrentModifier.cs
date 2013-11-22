using UnityEngine;
using System;
using System.Collections.Generic;

public class VitalCurrentModifier: BaseEffect {

    private VitalType vitalType;
    private EffectMod modifier;

    private DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int> modifierDispatcher;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, VitalType _vital, EffectMod _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive);
        vitalType = _vital;
        modifier = _modifier;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        vitalType = ((VitalCurrentModifier)_effect).vitalType;
        modifier = ((VitalCurrentModifier)_effect).modifier;
        SetUpModifierDispatcheTable();
        enabled = true;
    }

    public override void Activate() {
        Receiver.GetVital((int)vitalType).CurrentValue += modifierDispatcher.Dispatch(vitalType, (int)modifier.RawValue, Caster, Receiver) +
                                                          (int)(modifier.PercentageValue * Receiver.GetVital((int)vitalType).FinalValue);
        base.Activate();
    }

    private void SetUpModifierDispatcheTable(){
        modifierDispatcher = new DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int>();

        modifierDispatcher.AddAction(VitalType.Health, delegate(int rawValue, BaseCharacterModel Caster, BaseCharacterModel Receiver) {
            return (int)((rawValue<0)?
                          //Physical Damage
                          rawValue*(Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
                                    Receiver.GetAttribute((int)AttributeType.Defence).FinalValue)
                          ://Health Healing
                          rawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
                        );
        });
        modifierDispatcher.AddAction(VitalType.Mana, delegate(int rawValue, BaseCharacterModel Caster, BaseCharacterModel Receiver) {
            return (int)((rawValue < 0) ?
                          //Magic Damage
                          rawValue * (Caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
                                      Receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue)
                          ://Mana Healing
                          rawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
                                      (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
                        );
        });
    }

    public VitalType VitalType {
        get { return vitalType; }
        set { vitalType = value; }
    }
    public EffectMod Modifier {
        get { return modifier; }
        set { modifier = value; }
    }

    public DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int> ModifierDispatcher {
        get { return modifierDispatcher; }
    }
}