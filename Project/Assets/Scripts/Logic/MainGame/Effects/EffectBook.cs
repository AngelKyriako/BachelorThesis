using UnityEngine;
using System.Collections.Generic;

public class EffectBook {

    private const string DIRECT_DAMAGE = "DD", DAMAGE_OVER_TIME = "DoT",
                         DIRECT_MANA_BURN = "DM", MANA_BURN_OVER_TIME = "MoT",
                         DIRECT_HEALTH_HEAL = "DHH", HEALTH_HEAL_OVER_TIME = "HHoT",
                         DIRECT_MANA_HEAL = "DMH", MANA_HEAL_OVER_TIME = "MHoT";

    private const string MINOR = "Minor", MODERATE = "Moderate", MAJOR = "Major", // level requirements Power: 1 - 10 - 20
                         SWIFT = "Swift", EXTENDED = "Extended", LONG = "Long";   // level requirements Duration: 1 - 10 - 20
                                                                                  // if both then: (lvlReqPower + lvlReqDur) / 2: 1 - 5 - 10 -15 -20

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
        int effectsCount = 0;

        #region Damage Effects
        #region Direct (id - title - description - mana - req - modifier)
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 10, 1, new EffectMod(20f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier)
        //Minor
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(5f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(5f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(5f, 0f));
        //Major
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region ManaBurn Effects
        #region Direct (id - title - description - mana - req - modifier)
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(001, EffectType.ManaBurn, MINOR + DIRECT_DAMAGE, "", 10, 1, new EffectMod(15f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(002, EffectType.ManaBurn, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(35f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(003, EffectType.ManaBurn, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(70f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier)
        //Minor
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(4f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(4f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(4f, 0f));
        //Major
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Heal Effects
        #region Health
        #region Direct (id - title - description - mana - req - modifier - VitalType)
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(001, EffectType.ManaBurn, MINOR + DIRECT_DAMAGE, "", 10, 1, new EffectMod(10f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(002, EffectType.ManaBurn, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(25f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(003, EffectType.ManaBurn, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(60f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - VitalType)
        //Minor
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        //Major
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Mana
        #region Direct (id - title - description - mana - req - modifier - VitalType)
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(001, EffectType.ManaBurn, MINOR + DIRECT_DAMAGE, "", 10, 1, new EffectMod(12f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(002, EffectType.ManaBurn, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(30f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(003, EffectType.ManaBurn, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(65f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - VitalType)
        //Minor
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        //Major
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 10f, 10f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.ManaBurn, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 18f, 18f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #endregion
        #region Buff & DeBuff Effects
        #region Direct (id - title - description - mana - req - dur - modifier - Type)
        #region Buffs
        tempEffect = NewVitalBuff(effectsHolder);
        ((VitalBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.Buff, "HealthBuff50%", "", 30, 10, 15f, new EffectMod(0f, 0.5f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.Buff, "MovementBuff30%", "", 15, 1, 3f, new EffectMod(0f, 0.3f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #region DeBuffs
        tempEffect = NewVitalBuff(effectsHolder);
        ((VitalBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.DeBuff, "HealthDeBuff20%", "", 30, 10, 15f, new EffectMod(0f, -0.2f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.DeBuff, "MovementDeBuff30%", "", 15, 1, 3f, new EffectMod(0f, -0.3f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - Type)
        #region Buffs
        tempEffect = NewVitalOverTimeBuff(effectsHolder);
        ((VitalOverTimeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.Buff, "HealthBuffoT3%", "", 30, 1, 10f, 10f, 2f, new EffectMod(0f, 0.03f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeOverTimeBuff(effectsHolder);
        ((AttributeOverTimeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.Buff, "MovementBuffoT4%", "", 25, 1, 10f, 10f, 2f, new EffectMod(0f, 0.04f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #region DeBuffs
        tempEffect = NewVitalOverTimeBuff(effectsHolder);
        ((VitalOverTimeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.DeBuff, "HealthDeBuffoT3%", "", 40, 1, 10f, 10f, 2f, new EffectMod(0f, -0.03f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeOverTimeBuff(effectsHolder);
        ((AttributeOverTimeBuff)tempEffect).SetUpEffect(++effectsCount, EffectType.DeBuff, "MovementDeBuffoT4%", "", 35, 1, 10f, 10f, 2f, new EffectMod(0f, -0.04f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #endregion
        #region Special Effects
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Stun, "Stun", "", 25, 1, 4f);
        AddEffect(tempEffect, "");

        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.Silence, "Silence", "", 15, 1, 4f);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(++effectsCount, EffectType.None, "Cleanse", "", 20, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Damage);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.DeBuff);
        AddEffect(tempEffect, "");
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
