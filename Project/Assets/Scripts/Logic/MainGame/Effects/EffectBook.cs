using UnityEngine;
using System.Collections.Generic;

public class EffectBook {

    private Dictionary<string, BaseEffect> allEffects;

    private static EffectBook instance = new EffectBook();
    public static EffectBook Instance {
        get { return EffectBook.instance; }
    }

    private EffectBook() {
        allEffects = new Dictionary<string, BaseEffect>();

        GameObject effectsHolder = new GameObject("EffectHolder");
        BaseEffect tempEffect = null;

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect("Immobilize", "Immobilize description", null, 20, 1, 3f, new EffectMod(0f, -1.0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect);

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect("Slow", "Slow description", null, 15, 1, 10f, new EffectMod(0f, -0.5f), AttributeType.MovementSpeed);
        AddEffect(tempEffect);

        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect("Damage", "damage effect description", null, 13, 1, new EffectMod(20f, 0f));
        AddEffect(tempEffect);

        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect("Mana burn", "mana burn effect description", null, 10, 1, new EffectMod(0f, 0.2f));
        AddEffect(tempEffect);

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect("Health Heal", "Health Heal effect description", null, 15, 1, new EffectMod(20f, 0.3f), VitalType.Health);
        AddEffect(tempEffect);

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect("Mana Heal", "Mana Heal effect description", null, 10, 1, new EffectMod(20f, 0.4f), VitalType.Mana);
        AddEffect(tempEffect);

        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect("Stun", "Stun description", null, 15, 1, 4f);
        AddEffect(tempEffect);

        #region Testing effect helpers
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect("OneHitKO", "damage effect description", null, 13, 1, new EffectMod(0f, 1f));
        AddEffect(tempEffect);


        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect("20Immobilize", "20Immobilize description", null, 20, 1, 20f, new EffectMod(0f, -1.0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect);
        #endregion

        GameObject.Destroy(effectsHolder);
    }
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
    #region Accesors
    public ICollection<string> AllEffectsKeys {
        get { return allEffects.Keys; }
    }
    public void AddEffect(BaseEffect _effect) {
        allEffects.Add(_effect.Title, _effect);
    }
    public void RemoveEffect(BaseEffect _effect) {
        allEffects.Remove(_effect.Title);
    }
    public BaseEffect GetEffect(string _title) {
        return allEffects[_title];
    }
#endregion
}
