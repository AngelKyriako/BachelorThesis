using UnityEngine;
using System.Collections.Generic;

public class CleanseEffect : BaseEffect {

    private Dictionary<EffectType, bool> cleansedTypes;

    public override void SetUpEffect(int _id, EffectType _type, string _title, string _descr, uint _manaCost, uint _minLevelReq) {
        base.SetUpEffect(_id, _type, _title, _descr, _manaCost, _minLevelReq);
        cleansedTypes = new Dictionary<EffectType, bool>();
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        CleanseEffect _cleanseEff = (CleanseEffect)_effect;
        foreach (EffectType _type in _cleanseEff.cleansedTypes.Keys)
            AddEffectTypeToBeCleansed(_type);
    }

    public void AddEffectTypeToBeCleansed(EffectType _cleansedType){
        Utilities.Instance.PreCondition(!cleansedTypes.ContainsKey(_cleansedType), "Cleanse Effect", "AddEffectTypeToBeCleansed", "each effect type of the effects to be cleansed, must be unique in a clease effect");
        cleansedTypes.Add(_cleansedType, true);
    }

    public override void Activate() {
        BaseEffect[] effects = GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            if (ToBeCleansed(effects[i].Type))
                effects[i].Deactivate();        
    }

    private bool ToBeCleansed(EffectType _type){
        return cleansedTypes.ContainsKey(_type) && cleansedTypes[_type];
    }
}
