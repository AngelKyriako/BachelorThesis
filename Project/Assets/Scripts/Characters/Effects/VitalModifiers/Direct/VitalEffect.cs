using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class VitalEffect: BaseEffect {

    private EffectMod modifier;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, //base
                            EffectMod _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive);
        modifier = _modifier;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        modifier = ((VitalEffect)_effect).modifier;
    }

    public EffectMod Modifier {
        get { return modifier; }
        set { modifier = value; }
    }

    #region DispatchTable Example
    //private DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int> modifierDispatcher;

    //private void SetUpModifierDispatcheTable(){
    //    modifierDispatcher = new DispatchTable<VitalType, int, BaseCharacterModel, BaseCharacterModel, int>();

    //    modifierDispatcher.AddAction(VitalType.Health, delegate(int rawValue, BaseCharacterModel Caster, BaseCharacterModel Receiver) {
    //        return (int)((rawValue<0)?
    //                      //Physical Damage
    //                      rawValue*(Caster.GetAttribute((int)AttributeType.Damage).FinalValue /
    //                                Receiver.GetAttribute((int)AttributeType.Defence).FinalValue)
    //                      ://Health Healing
    //                      rawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
    //                                  (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
    //                    );
    //    });
    //    modifierDispatcher.AddAction(VitalType.Mana, delegate(int rawValue, BaseCharacterModel Caster, BaseCharacterModel Receiver) {
    //        return (int)((rawValue < 0) ?
    //                      //Magic Damage
    //                      rawValue * (Caster.GetAttribute((int)AttributeType.MagicDamage).FinalValue /
    //                                  Receiver.GetAttribute((int)AttributeType.MagicDefence).FinalValue)
    //                      ://Mana Healing
    //                      rawValue * (Caster.GetAttribute((int)AttributeType.Leadership).FinalValue /
    //                                  (Receiver.GetAttribute((int)AttributeType.Leadership).FinalValue/2))
    //                    );
    //    });
    //}
#endregion
}