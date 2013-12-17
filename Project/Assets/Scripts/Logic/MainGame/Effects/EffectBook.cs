using UnityEngine;
using System.Collections.Generic;

public enum Effect {
    None,
    Damage,
    Slow,
    Immobilize,
    Stun
}

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

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect((int)Effect.Immobilize, "Immobilize", "Immobilize description", 20, 1, 3f, new EffectMod(0f, -1.0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "140");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect((int)Effect.Slow, "Slow", "Slow description", 15, 1, 10f, new EffectMod(0f, -0.5f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "141");

        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect((int)Effect.Damage, "Damage", "damage effect description", 13, 1, new EffectMod(100f, 0f));
        AddEffect(tempEffect, "142");

        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(7, "Mana burn", "mana burn effect description", 10, 1, new EffectMod(0f, 0.2f));
        AddEffect(tempEffect, "143");

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(5, "Health Heal", "Health Heal effect description", 15, 1, new EffectMod(20f, 0.3f), VitalType.Health);
        AddEffect(tempEffect, "144");

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(6, "Mana Heal", "Mana Heal effect description", 10, 1, new EffectMod(20f, 0.4f), VitalType.Mana);
        AddEffect(tempEffect, "145");

        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect((int)Effect.Stun, "Stun", "Stun description", 15, 1, 4f);
        AddEffect(tempEffect, "146");

        #region Testing effect helpers
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(99, "OneHitKO", "damage effect description", 13, 1, new EffectMod(0f, 1f));
        AddEffect(tempEffect, "147");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(98, "20Immobilize", "20Immobilize description", 20, 1, 20f, new EffectMod(0f, -1.0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "148");
        #endregion

        GameObject.Destroy(effectsHolder);
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
    //buff & debuffs
    private VitalBuff NewVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }
    private AttributeBuff NewAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
    }
    private StunEffect NewStunEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<StunEffect>();
    }
    #endregion

    #region Accesors
    public ICollection<int> AllEffectsKeys {
        get { return allEffects.Keys; }
    }
    public void AddEffect(BaseEffect _effect, string _icon) {
        allEffects.Add(_effect.Id, new EffectBookEffect(_effect, _icon));
    }
    public void RemoveEffect(BaseEffect _effect) {
        allEffects.Remove(_effect.Id);
    }
    public BaseEffect GetEffect(int _id) {
        return allEffects[_id].Effect;
    }
    public string GetIcon(int _id) {
        return allEffects[_id].Icon;
    }
#endregion
}
