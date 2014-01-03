using UnityEngine;
using System.Collections.Generic;

public class EffectBook {

    public struct EffectBookEffect {
        public BaseEffect Effect;
        public string Icon;
        public EffectBookEffect(BaseEffect _effect, string _icon) {
            Effect = _effect;
            Icon = _icon;
        }
    }

    private Dictionary<int, EffectBookEffect> allEffects;

    private static EffectBook instance = new EffectBook();
    public static EffectBook Instance {
        get { return EffectBook.instance; }
    }

    private EffectBook() {
        allEffects = new Dictionary<int, EffectBookEffect>();

        GameObject effectsHolder = new GameObject("EffectHolder");
        BaseEffect tempEffect = null;

        //Effect Templates
        #region Vital modifiers
        #region Direct  (id - title - description - mana - req - modifier)
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, "Damage75", "", 0, 1, new EffectMod(75f, 0f));
        AddEffect(tempEffect, "001");

        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(002, "ManaBurn30", "", 10, 1, new EffectMod(30f, 0.2f));
        AddEffect(tempEffect, "002");

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(003, "HealthHeal20_30%", "", 15, 1, new EffectMod(20f, 0.3f), VitalType.Health);
        AddEffect(tempEffect, "003");

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(004, "ManaHeal20_30%", "", 15, 1, new EffectMod(20f, 0.3f), VitalType.Mana);
        AddEffect(tempEffect, "004");
        #endregion

        #region Over time  (id - title - description - mana - req - dur - overtime dur - freq - modifier)
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(051, "DamageoT15", "", 0, 1, 10f, 10f, 2f, new EffectMod(15f, 0f));
        AddEffect(tempEffect, "051");

        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(052, "ManaBurnoT15", "", 15, 1, 10f, 10f, 2f, new EffectMod(15f, 0f));
        AddEffect(tempEffect, "052");

        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(053, "HealthHealoT15", "", 15, 1, 10f, 10f, 2f, new EffectMod(15f, 0f), VitalType.Health);
        AddEffect(tempEffect, "053");

        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(054, "ManaHealoT15", "", 15, 1, 10f, 10f, 2f, new EffectMod(15f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "054");
        #endregion
        #endregion

        #region Buffs & Debuffs
        #region Direct  (id - title - description - mana - req - dur - modifier - Type)
        tempEffect = NewVitalBuff(effectsHolder);
        ((VitalBuff)tempEffect).SetUpEffect(101, "HealthBuff50%", "", 15, 1, 3f, new EffectMod(0f, 0.5f), VitalType.Health);
        AddEffect(tempEffect, "101");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(102, "MovementDebuff100%", "", 15, 1, 3f, new EffectMod(0f, -1.0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "102");
        #endregion

        #region Over time  (id - title - description - mana - req - dur - overtime dur - freq - modifier - Type)
        tempEffect = NewVitalOverTimeBuff(effectsHolder);
        ((VitalOverTimeBuff)tempEffect).SetUpEffect(151, "HealthBuffoT10%", "", 15, 1, 10f, 10f, 2f, new EffectMod(0f, 0.1f), VitalType.Health);
        AddEffect(tempEffect, "151");

        tempEffect = NewAttributeOverTimeBuff(effectsHolder);
        ((AttributeOverTimeBuff)tempEffect).SetUpEffect(152, "MovementDebuffoT20%", "", 15, 1, 10f, 10f, 2f, new EffectMod(0f, -0.2f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "152");
        #endregion
        #endregion

        #region Special
        //(id - title - description - mana - req - duration)
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(201, "Stun4", "", 15, 1, 4f);
        AddEffect(tempEffect, "201");

        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(202, "Silence4", "", 15, 1, 4f);
        AddEffect(tempEffect, "202");

        //(id - title - description - mana - req - type)
        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(203, "Cleanse", "", 15, 1, EffectType.Negative);
        AddEffect(tempEffect, "203");
        #endregion

        GameObject.DestroyImmediate(effectsHolder);
    }

    #region Utility constructors
    //direct vital effects
    private DamageEffect NewDamageEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<DamageEffect>();
    }
    private ManaBurnEffect NewManaBurnEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<ManaBurnEffect>();
    }
    private HealingEffect NewHealingEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<HealingEffect>();
    }
    //direct buff & debuffs
    private VitalBuff NewVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }
    private AttributeBuff NewAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
    }
    //over time vital effects
    private DamageOverTimeEffect NewDamageOverTimeEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<DamageOverTimeEffect>();
    }
    private ManaBurnOverTimeEffect NewManaBurnOverTimeEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<ManaBurnOverTimeEffect>();
    }
    private HealingOverTimeEffect NewHealingOverTimeEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<HealingOverTimeEffect>();
    }
    //over time buff & debuffs
    private VitalOverTimeBuff NewVitalOverTimeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalOverTimeBuff>();
    }
    private AttributeOverTimeBuff NewAttributeOverTimeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeOverTimeBuff>();
    }
    //special
    private StunEffect NewStunEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<StunEffect>();
    }
    private SilenceEffect NewSilenceEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<SilenceEffect>();
    }
    private CleanseEffect NewCleanseEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<CleanseEffect>();
    }
    #endregion

    private void AddEffect(BaseEffect _effect, string _icon) {
        allEffects.Add(_effect.Id, new EffectBookEffect(_effect, _icon));
    }

    #region Accesors
    public ICollection<int> AllEffectsKeys {
        get { return allEffects.Keys; }
    }
    public BaseEffect GetEffect(int _id) {
        return allEffects[_id].Effect;
    }
    public string GetIcon(int _id) {
        return allEffects[_id].Icon;
    }
#endregion
}
