using UnityEngine;
using System.Collections.Generic;

public class SkillBook{


    private const string DIRECT_DAMAGE = "DD", DAMAGE_OVER_TIME = "DoT",
                         DIRECT_MANA_BURN = "DM", MANA_BURN_OVER_TIME = "MoT",
                         DIRECT_HEALTH_HEAL = "DHH", HEALTH_HEAL_OVER_TIME = "HHoT",
                         DIRECT_MANA_HEAL = "DMH", MANA_HEAL_OVER_TIME = "MHoT";

    private const string MINOR = "Minor", MODERATE = "Moderate", MAJOR = "Major",
                         SWIFT = "Swift", EXTENDED = "Extended", LONG = "Long";

    private const string DESTINATION_CURSOR = "DestinationTargetCursor",
                         DIRECTION_CURSOR = "DirectionTargetCursor";

    public enum Skill {
        None,
        #region projectiles
        FireBall,        
        WaterBall,
        MudBall,
        IceBall,                
        LavaBall,
        SnowBall,
        RockBall,
        PoisonBall,                
        LeafBall,
        BlueBall,
        LoveBall,
        RevengeBall,
        SunBall,
        DarkBall,
        #endregion
        #region traps
        FireTrap,
        IceTrap,
        LavaTrap,
        PoisonTrap,
        #endregion
        #region directly casted
        BlowUp,
        LastStand,
        Resurrection,
        HawkEyes,
        HermesSandals,
        DragonSkin,
        HolyGround,
        HealingPrayer
        #endregion
    }

    public struct SkillBookSkill {
        public BaseSkill Skill;
        public bool IsAvailable;
        public string Icon;
        public SkillBookSkill(BaseSkill _skill, string _icon, bool _isAvailable) {
            Skill = _skill;
            Icon = _icon;
            IsAvailable = _isAvailable;
        }
    }


    private Dictionary<int, SkillBookSkill> allSkills;

    private static SkillBook instance = null;
    public static SkillBook Instance {
        get {
            if (instance != null)
                return instance;
            else {
                instance = new SkillBook();
                return instance;
            }
        }
    }

    private SkillBook() {        
        BaseSkill tempSkill = null;
        BaseEffect tempEffect = null;

        allSkills = new Dictionary<int, SkillBookSkill>();

        GameObject effectsHolder = new GameObject("EffectHolder");

        #region None ( 0 )
        AddSkill(new BaseSkill((int)Skill.None, "Unknown", "Skill does not exist, what the fuck did you do ?", 0f, 0f,
                               string.Empty, null, string.Empty), "");
        #endregion
        #region projectiles ( 1 - 14 )
        tempSkill = new TargetedSkill((int)Skill.FireBall, "Flame", "Physical Damage and DoT", 5f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 5, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "137");

        tempSkill = new TargetedSkill((int)Skill.WaterBall, "Water Shot", "Special Damage and DoT", 5f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(051, EffectType.ManaBurn, MINOR + DIRECT_MANA_BURN, "", 5, 1, new EffectMod(10f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(054, EffectType.ManaBurn, SWIFT + MINOR + MANA_BURN_OVER_TIME, "", 5, 1, 6f, 6f, 1f, new EffectMod(2f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(057, EffectType.ManaBurn, SWIFT + MODERATE + MANA_BURN_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(4f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(052, EffectType.ManaBurn, MODERATE + DIRECT_MANA_BURN, "", 20, 10, new EffectMod(25f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "140");

        tempSkill = new TargetedSkill((int)Skill.MudBall, "Mud Shot", "Physical Damage and Immobilize", 10f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(253, EffectType.DeBuff, "Immobilize", "", 45, 10, 5f, new EffectMod(0f, -1f), AttributeType.MovementSpeed);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "136");

        tempSkill = new TargetedSkill((int)Skill.IceBall, "Ice ball", "Physical Damage and Stun (AoE)", 12f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("IceBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(402, EffectType.Stun, MODERATE + "Stun", "", 20, 3, 5f);
        tempSkill.AddOffensiveEffect(tempEffect);            
        AddSkill(tempSkill, "134");

        tempSkill = new TargetedSkill((int)Skill.LavaBall, "Fire Meteorite", "Physical Damage and DoT (AoE)", 17f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LavaBall"), ResourcesPathManager.Instance.TriggerEffectPath("FireExplosion"),
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddPassiveEffect(tempEffect);
        AddSkill(tempSkill, "148");

        tempSkill = new TargetedSkill((int)Skill.SnowBall, "Snow Ball", "Physical & Special Damage and Slow", 15f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("SnowBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(054, EffectType.ManaBurn, SWIFT + MINOR + MANA_BURN_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(2f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(057, EffectType.ManaBurn, SWIFT + MODERATE + MANA_BURN_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(4f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(060, EffectType.ManaBurn, SWIFT + MAJOR + MANA_BURN_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(7f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(251, EffectType.DeBuff, "MovementDeBuff20%", "", 20, 1, 10f, new EffectMod(-0.2f, 0f), AttributeType.MovementSpeed);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new TargetedSkill((int)Skill.RockBall, "Rock Blast", "Physical Damage", 16f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("RockBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "136");

        tempSkill = new TargetedSkill((int)Skill.PoisonBall, "Poison Shot", "Physical DoT", 14f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("PurpleBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(005, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(006, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(008, EffectType.Damage, EXTENDED + MODERATE + DAMAGE_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(009, EffectType.Damage, LONG + MODERATE + DAMAGE_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(011, EffectType.Damage, EXTENDED + MAJOR + DAMAGE_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(012, EffectType.Damage, LONG + MAJOR + DAMAGE_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "146");

        tempSkill = new TargetedSkill((int)Skill.LeafBall, "Leaf Gun", "Special Damage, Healing and Buff dispel and speed buff to self", 9f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("LeafBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(051, EffectType.ManaBurn, MINOR + DIRECT_MANA_BURN, "", 10, 1, new EffectMod(15f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(052, EffectType.ManaBurn, MODERATE + DIRECT_MANA_BURN, "", 20, 10, new EffectMod(35f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(053, EffectType.ManaBurn, MAJOR + DIRECT_MANA_BURN, "", 30, 20, new EffectMod(70f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(254, EffectType.DeBuff, "DamageDebuffPer25%", "", 25, 10, 12f, new EffectMod(0f, -0.25f), AttributeType.Damage);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewCleanseEffect(effectsHolder);
        ((CleanseEffect)tempEffect).SetUpEffect(437, EffectType.None, "Absolute Dispel", "", 40, 20);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.HealthHeal);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.ManaHeal);
        ((CleanseEffect)tempEffect).AddEffectTypeToBeCleansed(EffectType.Buff);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(201, EffectType.Buff, "MovementBuff20%", "", 15, 1, 15f, new EffectMod(0.2f, 0f), AttributeType.MovementSpeed);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(155, EffectType.ManaHeal, EXTENDED + MINOR + MANA_HEAL_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(2f, 0f), VitalType.Mana);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(158, EffectType.ManaHeal, EXTENDED + MODERATE + MANA_HEAL_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(4f, 0f), VitalType.Mana);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewHealingOverTimeEffect(effectsHolder);
        ((HealingOverTimeEffect)tempEffect).SetUpEffect(161, EffectType.ManaHeal, EXTENDED + MAJOR + MANA_HEAL_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(7f, 0f), VitalType.Mana);
        tempSkill.AddPassiveEffect(tempEffect);
        AddSkill(tempSkill, "145");

        tempSkill = new TargetedSkill((int)Skill.BlueBall, "Blue Flame", "Special Damage & DoT and Defence debuff (AoE)", 39f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("BlueBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(052, EffectType.ManaBurn, MODERATE + DIRECT_MANA_BURN, "", 20, 10, new EffectMod(35f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(053, EffectType.ManaBurn, MAJOR + DIRECT_MANA_BURN, "", 30, 20, new EffectMod(70f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(058, EffectType.ManaBurn, EXTENDED + MODERATE + MANA_BURN_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(4f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(059, EffectType.ManaBurn, LONG + MODERATE + MANA_BURN_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(4f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(060, EffectType.ManaBurn, SWIFT + MAJOR + MANA_BURN_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(7f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(061, EffectType.ManaBurn, EXTENDED + MAJOR + MANA_BURN_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(7f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnOverTimeEffect(effectsHolder);
        ((ManaBurnOverTimeEffect)tempEffect).SetUpEffect(062, EffectType.ManaBurn, LONG + MAJOR + MANA_BURN_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(7f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(255, EffectType.DeBuff, "DefenceDebuffPer30%", "", 40, 10, 15f, new EffectMod(0f, -0.3f), AttributeType.Defence);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "141");

        tempSkill = new TargetedSkill((int)Skill.LoveBall, "Love Shot", "Physical Damage and Silence", 8f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("PinkBall"), ResourcesPathManager.Instance.TriggerEffectPath("BlueFlameExplosion"),
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(411, EffectType.Silence, MINOR + "Silence", "", 15, 1, 8f);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(412, EffectType.Silence, MODERATE + "Silence", "", 30, 10, 12f);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new TargetedSkill((int)Skill.RevengeBall, "Revenge Ball", "Physical Damage based on lost health", 22f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("DarkBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(0)); //@TODO: Need to add new Effect
        AddSkill(tempSkill, "135");

        tempSkill = new TargetedSkill((int)Skill.SunBall, "Sun Burst", "Physical Damage & Healing, vision buff to Allies(AoE)", 48f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("YellowBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(101, EffectType.HealthHeal, MINOR + DIRECT_HEALTH_HEAL, "", 10, 1, new EffectMod(10f, 0f), VitalType.Health);
        tempSkill.AddSupportEffect(tempEffect);
        tempEffect = NewHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect(102, EffectType.HealthHeal, MODERATE + DIRECT_HEALTH_HEAL, "", 20, 10, new EffectMod(25f, 0f), VitalType.Health);
        tempSkill.AddSupportEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(204, EffectType.Buff, "VisionBuffPer60%", "", 20, 5, 15f, new EffectMod(0f, 0.6f), AttributeType.VisionRadius);
        tempSkill.AddSupportEffect(tempEffect);
        AddSkill(tempSkill, "150");

        tempSkill = new TargetedSkill((int)Skill.DarkBall, "Darth Bolt", "Physical Damage, Silence, Slow and vision debuff (AoE)", 44f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("DarkBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(412, EffectType.Silence, MODERATE + "Silence", "", 30, 10, 12f);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewSilenceEffect(effectsHolder);
        ((SilenceEffect)tempEffect).SetUpEffect(413, EffectType.Silence, MAJOR + "Silence", "", 45, 20, 18f);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(252, EffectType.DeBuff, "MovementDeBuffPer50%", "", 30, 7, 8f, new EffectMod(0f, -0.5f), AttributeType.MovementSpeed);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(257, EffectType.DeBuff, "VisionDebuffPer70%", "", 40, 10, 15f, new EffectMod(0f, -0.7f), AttributeType.VisionRadius);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "132");
        #endregion
        #region traps ( 15 - 18 )
        tempSkill = new BaseSkill((int)Skill.FireTrap, "Fire Trap", "Physical Damage and DoT", 25f, 0f,
                                  string.Empty, ResourcesPathManager.Instance.TrapPath("FireTrap"), ResourcesPathManager.Instance.TriggerEffectPath("FireExplosion"));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(005, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.IceTrap, "Icy Terrain", "Special Damage and Stun (AoE)", 38f, 0f,
                                  string.Empty, ResourcesPathManager.Instance.TrapPath("IceTrap"), string.Empty);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(051, EffectType.ManaBurn, MINOR + DIRECT_MANA_BURN, "", 10, 1, new EffectMod(15f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewManaBurnEffect(effectsHolder);
        ((ManaBurnEffect)tempEffect).SetUpEffect(052, EffectType.ManaBurn, MODERATE + DIRECT_MANA_BURN, "", 20, 10, new EffectMod(35f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(401, EffectType.Stun, MINOR + "Stun", "", 20, 1, 3f);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(402, EffectType.Stun, MODERATE + "Stun", "", 35, 10, 5f);
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewStunEffect(effectsHolder);
        ((StunEffect)tempEffect).SetUpEffect(403, EffectType.Stun, MAJOR + "Stun", "", 60, 20, 8f);
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.LavaTrap, "Lava Trap", "Physical Damage and DoT (AoE)", 35f, 0f,
                                  string.Empty, ResourcesPathManager.Instance.TrapPath("LavaTrap"), ResourcesPathManager.Instance.TriggerEffectPath("FireExplosion"));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddSupportEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.PoisonTrap, "Poison Sludge", "Physical DoT (AoE)", 41f, 0f,
                                  string.Empty, ResourcesPathManager.Instance.TrapPath("PurpleTrap"), string.Empty);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(004, EffectType.Damage, SWIFT + MINOR + DAMAGE_OVER_TIME, "", 10, 1, 6f, 6f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(005, EffectType.Damage, EXTENDED + MINOR + DAMAGE_OVER_TIME, "", 10, 5, 10f, 10f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(006, EffectType.Damage, LONG + MINOR + DAMAGE_OVER_TIME, "", 10, 10, 18f, 18f, 1f, new EffectMod(3f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(007, EffectType.Damage, SWIFT + MODERATE + DAMAGE_OVER_TIME, "", 10, 5, 6f, 6f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(008, EffectType.Damage, EXTENDED + MODERATE + DAMAGE_OVER_TIME, "", 10, 10, 10f, 10f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(009, EffectType.Damage, LONG + MODERATE + DAMAGE_OVER_TIME, "", 10, 15, 18f, 18f, 1f, new EffectMod(5f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(010, EffectType.Damage, SWIFT + MAJOR + DAMAGE_OVER_TIME, "", 10, 10, 6f, 6f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(011, EffectType.Damage, EXTENDED + MAJOR + DAMAGE_OVER_TIME, "", 10, 15, 10f, 10f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(012, EffectType.Damage, LONG + MAJOR + DAMAGE_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageOverTimeEffect(effectsHolder);
        ((DamageOverTimeEffect)tempEffect).SetUpEffect(012, EffectType.Damage, LONG + MAJOR + DAMAGE_OVER_TIME, "", 10, 20, 18f, 18f, 1f, new EffectMod(8f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        AddSkill(tempSkill, "135");
        #endregion
        #region directly casted ( 19 - 26)
        tempSkill = new BaseSkill((int)Skill.BlowUp, "Big Bang", "Physical Damage and suicide (AoE)", 80f, 0f,
                                  string.Empty, null, ResourcesPathManager.Instance.TriggerEffectPath("FireExplosion"));
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(001, EffectType.Damage, MINOR + DIRECT_DAMAGE, "", 5, 1, new EffectMod(20f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(002, EffectType.Damage, MODERATE + DIRECT_DAMAGE, "", 20, 10, new EffectMod(50f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(003, EffectType.Damage, MAJOR + DIRECT_DAMAGE, "", 30, 20, new EffectMod(100f, 0f));
        tempSkill.AddOffensiveEffect(tempEffect);
        tempEffect = NewDamageEffect(effectsHolder);
        ((DamageEffect)tempEffect).SetUpEffect(013, EffectType.Damage, "KO" + DIRECT_DAMAGE, "", 20, 1, new EffectMod(0f, 100f));
        tempSkill.AddPassiveEffect(tempEffect);
        AddSkill(tempSkill, "143");

        tempSkill = new BaseSkill((int)Skill.LastStand, "Last Stand", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.Resurrection, "Resurrection", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.HawkEyes, "Hawk Eyes", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.HermesSandals, "Hermes Sandals", "Movement Buff to self", 25f, 0f,
                                  string.Empty, null, string.Empty);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(201, EffectType.Buff, "MovementBuff20%", "", 15, 1, 15f, new EffectMod(0.2f, 0f), AttributeType.MovementSpeed);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(205, EffectType.Buff, "MovementBuff20%", "", 15, 10, 15f, new EffectMod(0.1f, 0f), AttributeType.MovementSpeed);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(206, EffectType.Buff, "MovementBuffPer20%", "", 15, 18, 15f, new EffectMod(0f, 0.2f), AttributeType.MovementSpeed);
        tempSkill.AddPassiveEffect(tempEffect);
        AddSkill(tempSkill, "147");

        tempSkill = new BaseSkill((int)Skill.DragonSkin, "Dragon's Skin", "Defence buff to self", 40f, 0f,
                                  string.Empty, null, string.Empty);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(207, EffectType.Buff, "DefenceBuff20%", "", 15, 5, 15f, new EffectMod(0f, 0.2f), AttributeType.Defence);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(208, EffectType.Buff, "DefenceBuffPer20%", "", 15, 12, 15f, new EffectMod(0f, 0.2f), AttributeType.Defence);
        tempSkill.AddPassiveEffect(tempEffect);
        tempEffect = NewAttributeBuff(effectsHolder);
        ((AttributeBuff)tempEffect).SetUpEffect(209, EffectType.Buff, "DefenceBuffPer20%", "", 15, 18, 15f, new EffectMod(0f, 0.1f), AttributeType.Defence);
        tempSkill.AddPassiveEffect(tempEffect);
        AddSkill(tempSkill, "135");

        tempSkill = new TargetedSkill((int)Skill.HolyGround, "Holy Ground", "", 2f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "149");

        tempSkill = new TargetedSkill((int)Skill.HealingPrayer, "Healing Prayer", "", 2f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "149");
        #endregion

        GameObject.DestroyImmediate(effectsHolder);
    }
    
    #region Accesors
    public ICollection<int> AllSkillsKeys {
        get { return allSkills.Keys; }
    }
    public void AddSkill(BaseSkill _skill, string _icon) {
        allSkills.Add(_skill.Id, new SkillBookSkill(_skill, _icon, true));
    }
    public void RemoveSkill(BaseSkill _skill) {
        allSkills.Remove(_skill.Id);
    }
    public void SetSkillAvailable(int _id, bool _isAvailable) {
        allSkills[_id] = new SkillBookSkill(GetSkill(_id), GetIcon(_id), _isAvailable);
    }
    public bool SkillExists(int _id) {
        return allSkills.ContainsKey(_id);
    }
    public BaseSkill GetSkill(int _id) {
        Utilities.Instance.PreCondition(allSkills.ContainsKey(_id), "SkillBook", "GetSkill", "Skillbook does not contain skill with Id: "+_id);
        return allSkills[_id].Skill;
    }
    public string GetIcon(int _id) {
        return allSkills[_id].Icon;
    }
    public bool IsSkillAvailable(int _id) {
        return allSkills[_id].IsAvailable;
    }
    #endregion


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
}