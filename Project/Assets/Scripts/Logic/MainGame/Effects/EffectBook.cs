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

        #region Damage Effects (001 - 050)
        #region Direct (id - title - description - mana - req - modifier)
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 10, 1, new EffectMod(20f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier)
        //Minor
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(005, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(006, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(3f, 0f));
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(008, EffectType.Damage, EXTENDED + MODERATE + DAMAGE_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(5f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(009, EffectType.Damage, LONG + MODERATE + DAMAGE_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(5f, 0f));
        //Major
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(011, EffectType.Damage, EXTENDED + MAJOR + DAMAGE_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(012, EffectType.Damage, LONG + MAJOR + DAMAGE_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(8f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region ManaBurn Effects (051 - 100)
        #region Direct (id - title - description - mana - req - modifier)
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(051, EffectType.ManaBurn, MINOR + DIRECT_MANA_BURN, "", 10, 1, new EffectMod(15f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(052, EffectType.ManaBurn, MODERATE + DIRECT_MANA_BURN, "", 20, 10, new EffectMod(35f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(053, EffectType.ManaBurn, MAJOR + DIRECT_MANA_BURN, "", 30, 20, new EffectMod(70f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier)
        //Minor
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(054, EffectType.ManaBurn, SWIFT + MINOR + MANA_BURN_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(055, EffectType.ManaBurn, EXTENDED + MINOR + MANA_BURN_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(056, EffectType.ManaBurn, LONG + MINOR + MANA_BURN_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(2f, 0f));
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(057, EffectType.ManaBurn, SWIFT + MODERATE + MANA_BURN_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(4f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(058, EffectType.ManaBurn, EXTENDED + MODERATE + MANA_BURN_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(4f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(059, EffectType.ManaBurn, LONG + MODERATE + MANA_BURN_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(4f, 0f));
        //Major
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(060, EffectType.ManaBurn, SWIFT + MAJOR + MANA_BURN_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(061, EffectType.ManaBurn, EXTENDED + MAJOR + MANA_BURN_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(062, EffectType.ManaBurn, LONG + MAJOR + MANA_BURN_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(7f, 0f));
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Heal Effects (101 - 200)
        #region Health (101 - 150)
        #region Direct (id - title - description - mana - req - modifier - VitalType)
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(101, EffectType.HealthHeal, MINOR + DIRECT_HEALTH_HEAL, "", 10, 1, new EffectMod(10f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(102, EffectType.HealthHeal, MODERATE + DIRECT_HEALTH_HEAL, "", 20, 10, new EffectMod(25f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(103, EffectType.HealthHeal, MAJOR + DIRECT_HEALTH_HEAL, "", 30, 20, new EffectMod(60f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - VitalType)
        //Minor
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(104, EffectType.HealthHeal, SWIFT + MINOR + HEALTH_HEAL_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(105, EffectType.HealthHeal, EXTENDED + MINOR + HEALTH_HEAL_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(106, EffectType.HealthHeal, LONG + MINOR + HEALTH_HEAL_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(2f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(107, EffectType.HealthHeal, SWIFT + MODERATE + HEALTH_HEAL_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(108, EffectType.HealthHeal, EXTENDED + MODERATE + HEALTH_HEAL_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(109, EffectType.HealthHeal, LONG + MODERATE + HEALTH_HEAL_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(4f, 0f), VitalType.Health);
        //Major
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(110, EffectType.HealthHeal, SWIFT + MAJOR + HEALTH_HEAL_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(111, EffectType.HealthHeal, EXTENDED + MAJOR + HEALTH_HEAL_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(112, EffectType.HealthHeal, LONG + MAJOR + HEALTH_HEAL_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(7f, 0f), VitalType.Health);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Mana (151 - 200)
        #region Direct (id - title - description - mana - req - modifier - VitalType)
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(151, EffectType.ManaHeal, MINOR + DIRECT_MANA_HEAL, "", 10, 1, new EffectMod(12f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(152, EffectType.ManaHeal, MODERATE + DIRECT_MANA_HEAL, "", 20, 10, new EffectMod(30f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(153, EffectType.ManaHeal, MAJOR + DIRECT_MANA_HEAL, "", 30, 20, new EffectMod(65f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - VitalType)
        //Minor
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(154, EffectType.ManaHeal, SWIFT + MINOR + MANA_HEAL_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(155, EffectType.ManaHeal, EXTENDED + MINOR + MANA_HEAL_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(156, EffectType.ManaHeal, LONG + MINOR + MANA_HEAL_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        //Moderate
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(157, EffectType.ManaHeal, SWIFT + MODERATE + MANA_HEAL_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(158, EffectType.ManaHeal, EXTENDED + MODERATE + MANA_HEAL_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(159, EffectType.ManaHeal, LONG + MODERATE + MANA_HEAL_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        //Major
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(160, EffectType.ManaHeal, SWIFT + MAJOR + MANA_HEAL_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(161, EffectType.ManaHeal, EXTENDED + MAJOR + MANA_HEAL_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(162, EffectType.ManaHeal, LONG + MAJOR + MANA_HEAL_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #endregion
        #region Buff & DeBuff Effects (201 - 400)
        #region Direct (id - title - description - mana - req - dur - modifier - Type)
        #region Buffs (201 - 250)
        //Movement
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(201, EffectType.Buff, "MovementBuff20%", "", 15, 1, 15f, new EffectMod(0.2f, 0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(202, EffectType.Buff, "MovementBuffPer50%", "", 20, 5, 18f, new EffectMod(0f, 0.5f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        //Vision
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(203, EffectType.Buff, "VisionBuffPer30%", "", 20, 5, 10f, new EffectMod(0f, 0.3f), AttributeType.VisionRadius);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(204, EffectType.Buff, "VisionBuffPer60%", "", 20, 5, 15f, new EffectMod(0f, 0.6f), AttributeType.VisionRadius);
        AddEffect(tempEffect, "");
        //Health
        tempEffect = NewVitalBuff(effectsHolder);
        ((VitalBuff)tempEffect).SetUpEffect(999, EffectType.Buff, "HealthBuff50%", "", 30, 10, 15f, new EffectMod(0f, 0.5f), VitalType.Health);
        AddEffect(tempEffect, "");

        #endregion
        #region DeBuffs (251 - 300)
        //Movement
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(251, EffectType.DeBuff, "MovementDeBuff20%", "", 20, 1, 10f, new EffectMod(-0.2f, 0f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(252, EffectType.DeBuff, "MovementDeBuffPer50%", "", 30, 7, 8f, new EffectMod(0f, -0.5f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(253, EffectType.DeBuff, "Immobilize", "", 45, 10, 5f, new EffectMod(0f, -1f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        //Damage
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(254, EffectType.DeBuff, "DamageDebuffPer25%", "", 25, 10, 12f, new EffectMod(0f, -0.25f), AttributeType.Damage);
        AddEffect(tempEffect, "");
        //Defence
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(255, EffectType.DeBuff, "DefenceDebuffPer30%", "", 40, 10, 15f, new EffectMod(0f, -0.3f), AttributeType.Defence);
        AddEffect(tempEffect, "");
        //Vision
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(256, EffectType.DeBuff, "VisionDebuffPer30%", "", 40, 10, 15f, new EffectMod(0f, -0.3f), AttributeType.VisionRadius);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(257, EffectType.DeBuff, "VisionDebuffPer70%", "", 40, 10, 15f, new EffectMod(0f, -0.7f), AttributeType.VisionRadius);
        AddEffect(tempEffect, "");
        //Health
        tempEffect = NewVitalBuff(effectsHolder);
        ((VitalBuff)tempEffect).SetUpEffect(999, EffectType.DeBuff, "HealthDeBuff20%", "", 30, 10, 15f, new EffectMod(0f, -0.2f), VitalType.Health);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #region Over Time (id - title - description - mana - req - dur - overtime dur - freq - modifier - Type)
        #region Buffs (301 - 350)
        tempEffect = NewVitalOverTimeBuff(effectsHolder);
        ((VitalOverTimeBuff)tempEffect).SetUpEffect(301, EffectType.Buff, "HealthBuffoT3%", "", 30, 1, 10f, 10f, 2f, new EffectMod(0f, 0.03f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeOverTimeBuff(effectsHolder);
        ((AttributeOverTimeBuff)tempEffect).SetUpEffect(302, EffectType.Buff, "MovementBuffoT4%", "", 25, 1, 10f, 10f, 2f, new EffectMod(0f, 0.04f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #region DeBuffs (351 - 400)
        tempEffect = NewVitalOverTimeBuff(effectsHolder);
        ((VitalOverTimeBuff)tempEffect).SetUpEffect(351, EffectType.DeBuff, "HealthDeBuffoT3%", "", 40, 1, 10f, 10f, 2f, new EffectMod(0f, -0.03f), VitalType.Health);
        AddEffect(tempEffect, "");

        tempEffect = NewAttributeOverTimeBuff(effectsHolder);
        ((AttributeOverTimeBuff)tempEffect).SetUpEffect(352, EffectType.DeBuff, "MovementDeBuffoT4%", "", 35, 1, 10f, 10f, 2f, new EffectMod(0f, -0.04f), AttributeType.MovementSpeed);
        AddEffect(tempEffect, "");
        #endregion
        #endregion
        #endregion
        #region Special Effects (401 - 500)
        //Stun Effects
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(401, EffectType.Stun, MINOR + "Stun", "", 20, 1, 3f);
        AddEffect(tempEffect, "");
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(402, EffectType.Stun, MODERATE + "Stun", "", 35, 10, 5f);
        AddEffect(tempEffect, "");
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(403, EffectType.Stun, MAJOR + "Stun", "", 60, 20, 8f);
        AddEffect(tempEffect, "");
        //Silence Effects
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(411, EffectType.Silence, MINOR + "Silence", "", 15, 1, 8f);
        AddEffect(tempEffect, "");
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(412, EffectType.Silence, MODERATE + "Silence", "", 30, 10, 12f);
        AddEffect(tempEffect, "");
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(413, EffectType.Silence, MAJOR + "Silence", "", 45, 20, 18f);
        AddEffect(tempEffect, "");
        //Clease effects
        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(420, EffectType.None, "Damage Cleanse", "", 25, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Damage);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(421, EffectType.None, "Mana Burn Cleanse", "", 20, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.ManaBurn);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(422, EffectType.None, "DeBuff Cleanse", "", 15, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.DeBuff);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(423, EffectType.None, "Absolute Cleanse", "", 45, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Damage);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.ManaBurn);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.DeBuff);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Stun);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Silence);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(435, EffectType.None, "Heal Dispel", "", 25, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.HealthHeal);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.ManaHeal);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(436, EffectType.None, "Buff Dispel", "", 20, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Buff);
        AddEffect(tempEffect, "");

        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(437, EffectType.None, "Absolute Dispel", "", 40, 1);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.HealthHeal);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.ManaHeal);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Buff);
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
    //direct buff & debuffs
    private VitalBuff NewVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }
    private AttributeBuff NewAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
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
