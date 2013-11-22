using UnityEngine;
using System.Collections;

public class VitalBuff: BuffEffect {

    private VitalType vitalType;

    public override void Awake() {
        base.Awake();
    }

    public void SetUpEffect(string _title, string _descr, Texture2D _icon, bool _isPassive, float _duration, VitalType _vital, EffectMod _modifier) {
        base.SetUpEffect(_title, _descr, _icon, _isPassive, _modifier, _duration);
        vitalType = _vital;
    }

    public override void SetUpEffect(BaseCharacterModel _caster, BaseEffect _effect) {
        base.SetUpEffect(_caster, _effect);
        vitalType = ((VitalBuff)_effect).vitalType;
        BuffValue = 0;
        enabled = true;
    }

    public override void Activate() {
        int tempValue = (int)(Modifier.RawValue + (Modifier.PercentageValue * Receiver.GetVital((int)vitalType).FinalValue));
        BuffValue += tempValue;
        Receiver.GetVital((int)vitalType).BuffValue += tempValue;
        base.Activate();
    }

    public override void Deactivate() {
        Receiver.GetVital((int)vitalType).BuffValue -= BuffValue;
        base.Deactivate();
    }
}