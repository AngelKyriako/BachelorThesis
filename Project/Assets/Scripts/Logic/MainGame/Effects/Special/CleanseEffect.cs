using UnityEngine;
using System.Collections.Generic;

public class CleanseEffect : BaseEffect {

    private EffectType cleansedType;

    public void SetUpEffect(int _id, string _title, string _descr, uint _manaCost, uint _minLevelReq, EffectType _cleansedType) {
        base.SetUpEffect(_id, _title, _descr, _manaCost, _minLevelReq);
        cleansedType = _cleansedType;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        
    }

    public override void Activate() {
        BaseEffect[] effects = GetComponents<BaseEffect>();
        for (int i = 0; i < effects.Length; ++i)
            //if (effects[i].Type.Equals(cleansedType)) for now deactivate all
                effects[i].Deactivate();        
    }
}
