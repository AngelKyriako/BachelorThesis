using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    private List<BaseSkill> availableSkills;
    private GameObject effectHolder;

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {
        effectHolder = GameObject.Find("Effects");

        BaseSkill tempSkill = null;
        BaseEffect tempEffect = null;

        availableSkills = new List<BaseSkill>();
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f, null, null, null, null);//(GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));
        //@TODO NOT ACTUALLY INSTANTIATING AN OBJECT !!!!
            tempEffect = (StatModifierEffect)GameObject.Instantiate(effectHolder.GetComponent<StatModifierEffect>());
            tempEffect.SetUpEffect("damage effect", "damage effect description", null, true, 0.5f);
            ((StatModifierEffect)tempEffect).AddModifiedVital((int)VitalType.Health, new EffectModifier(-10f, 0f), new EffectModifier(0f, 0f));
            tempSkill.AddEffect(tempEffect);

            tempEffect = (StatModifierEffect)GameObject.Instantiate(effectHolder.GetComponent<StatModifierEffect>());
            tempEffect.SetUpEffect("damage debuff effect", "damage debuff effect description", null, true, 5f);
            ((StatModifierEffect)tempEffect).AddModifiedAttribute((int)AttributeType.Damage, new EffectModifier(0f, -0.5f));
            tempSkill.AddEffect(tempEffect);

        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 2", "skill 2 description", null, 2f, null, null, null, (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 3", "skill 3 description", null, 2f, null, null, null, (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));   
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 4", "skill 4 description", null, 2f, null, null, null, (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 5", "skill 5 description", null, 2f, null, null, null, null);
        availableSkills.Add(tempSkill);
    }

    public List<BaseSkill> AvailableSkills {
        get { return availableSkills; }
    }

}
