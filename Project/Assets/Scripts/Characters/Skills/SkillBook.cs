using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    private List<BaseSkill> availableSkills;

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {
        GameObject effectsHolder = new GameObject("EffectHolder");

        BaseSkill tempSkill = null;
        BaseEffect tempEffect = null;

        availableSkills = new List<BaseSkill>();
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath), null, null,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));

            tempEffect = NextDamageEffect(effectsHolder);
            ((DamageEffect)tempEffect).SetUpEffect("damage effect", "damage effect description", null, false, new EffectMod(10f, 0f));
            tempSkill.AddEffect(tempEffect);

            tempEffect = NextManaBurnEffect(effectsHolder);
            ((HealingEffect)tempEffect).SetUpEffect("mana burn effect", "mana burn effect description", null, false, new EffectMod(0f, 0.2f));
            tempSkill.AddEffect(tempEffect);

        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 2", "skill 2 description", null, 2f, null, null, null, null);

        tempEffect = NextHealingEffect(effectsHolder);
        ((HealingEffect)tempEffect).SetUpEffect("mana burn effect", "mana burn effect description", null, true, new EffectMod(0f, 1f), VitalType.Mana);
        tempSkill.AddEffect(tempEffect);

        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 3", "skill 3 description", null, 2f, null, null, null, (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));   
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 4", "skill 4 description", null, 2f, null, null, null, (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 5", "skill 5 description", null, 2f, null, null, null, null);
        availableSkills.Add(tempSkill);

        GameObject.Destroy(effectsHolder);
    }

    private DamageEffect NextDamageEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<DamageEffect>();
    }

    private ManaBurnEffect NextManaBurnEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<ManaBurnEffect>();
    }

    private HealingEffect NextHealingEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<HealingEffect>();
    }

    private VitalBuff NextVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }

    private AttributeBuff NextAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
    }

    public List<BaseSkill> AvailableSkills {
        get { return availableSkills; }
    }

}
