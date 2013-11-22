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
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f, null, null, null, null);

            tempEffect = NextVitalCurrentModifier(effectsHolder);
            ((VitalCurrentModifier)tempEffect).SetUpEffect("damage effect", "damage effect description", null, true,
                                                           VitalType.Health, new EffectMod(-10f, 0f));
            tempSkill.AddEffect(tempEffect);

            tempEffect = NextVitalBuff(effectsHolder);
            ((VitalBuff)tempEffect).SetUpEffect("mana burn effect", "mana burn effect description", null, true, 10f,
                                                VitalType.Mana, new EffectMod(0f, 1f));
            tempSkill.AddEffect(tempEffect);

        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 2", "skill 2 description", null, 2f, null, null, null, null);

        tempEffect = NextOverTimeAttributeBuff(effectsHolder);
        ((OverTimeAttributeBuff)tempEffect).SetUpEffect("damage effect", "damage effect description", null, true, AttributeType.Defence, new EffectMod(5f, 0f), 8f, 5f, 1f);
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

    private BaseEffect NextBaseEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<BaseEffect>();
    }

    private BuffEffect NextBuffEffect(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<BuffEffect>();
    }

    private VitalCurrentModifier NextVitalCurrentModifier(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalCurrentModifier>();
    }

    private VitalBuff NextVitalBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<VitalBuff>();
    }

    private AttributeBuff NextAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<AttributeBuff>();
    }

    private OverTimeAttributeBuff NextOverTimeAttributeBuff(GameObject _effectsHolder) {
        return _effectsHolder.AddComponent<OverTimeAttributeBuff>();
    }
    

    public List<BaseSkill> AvailableSkills {
        get { return availableSkills; }
    }

}
