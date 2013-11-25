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
        ((AttributeBuff)tempEffect).SetUpEffect("Slow", "Slow description", null, false, 20f, AttributeType.MovementSpeed, new EffectMod(0f, -0.5f));
        AddEffect(tempEffect);

        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect("Damage", "damage effect description", null, false, new EffectMod(20f, 0f));
        AddEffect(tempEffect);

        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect("Mana burn", "mana burn effect description", null, false, new EffectMod(0f, 0.2f));
        AddEffect(tempEffect);

        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect("Health Heal", "Health Heal effect description", null, true, new EffectMod(0f, 0.3f), VitalType.Health);
        AddEffect(tempEffect);

        GameObject.Destroy(effectsHolder);
    }

    private DamageEffect NewDamageEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<DamageEffect>();
    }

    private ManaBurnEffect NewManaBurnEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<ManaBurnEffect>();
    }

    private HealingEffect NewHealingEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<HealingEffect>();
    }

    private VitalBuff NewVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }

    private AttributeBuff NewAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
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
