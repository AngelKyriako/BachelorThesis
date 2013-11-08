using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    private List<BaseSkill> availableSkills;

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {
        BaseSkill tempSkill = null;
        BaseEffect tempEffect = null;

        availableSkills = new List<BaseSkill>();
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f, null, null, null, (GameObject)Resources.Load("MyPrefab"));
            tempEffect = new BaseEffect("effect1", "effect1 description", null, 2f);
            tempEffect.AddModifiedAttribute((int)AttributeType.Attack, new StatModifier(10, 0.5f));           
        tempSkill.AddEffect(tempEffect);
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 2", "skill 2 description", null, 2f, null, null, null, (GameObject)Resources.Load("MyPrefab"));
            tempEffect = new BaseEffect("effect1", "effect1 description", null, 2f);
            tempEffect.AddModifiedAttribute((int)AttributeType.Defence, new StatModifier(20, 0.5f));
        tempSkill.AddEffect(tempEffect);    
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 3", "skill 3 description", null, 2f, null, null, null, (GameObject)Resources.Load("MyPrefab"));
            tempEffect = new BaseEffect("effect1", "effect1 description", null, 2f);
            tempEffect.AddModifiedAttribute((int)AttributeType.AttackSpeed, new StatModifier(30, 1f));
        tempSkill.AddEffect(tempEffect);    
        availableSkills.Add(tempSkill);

        tempSkill = new BaseSpell("skill 4", "skill 4 description", null, 2f, null, null, null, (GameObject)Resources.Load("MyPrefab"));
            tempEffect = new BaseEffect("effect1", "effect1 description", null, 2f);
            tempEffect.AddModifiedAttribute((int)AttributeType.MovementSpeed, new StatModifier(40, 1f));
        tempSkill.AddEffect(tempEffect);
        availableSkills.Add(tempSkill);

        //availableSkills.Add(new BaseSkill("skill 5", "skill 5 description", null));
        //availableSkills.Add(new BaseSkill("skill 6", "skill 6 description", null));
        //availableSkills.Add(new BaseSkill("skill 7", "skill 7 description", null));
        //availableSkills.Add(new BaseSkill("skill 8", "skill 8 description", null));
        //availableSkills.Add(new BaseSkill("skill 9", "skill 9 description", null));
        //availableSkills.Add(new BaseSkill("skill 10", "skill 10 description", null));
        //availableSkills.Add(new BaseSkill("skill 11", "skill 11 description", null));
        //availableSkills.Add(new BaseSkill("skill 12", "skill 12 description", null));
        //availableSkills.Add(new BaseSkill("skill 13", "skill 13 description", null));
        //availableSkills.Add(new BaseSkill("skill 14", "skill 14 description", null));
        //availableSkills.Add(new BaseSkill("skill 15", "skill 15 description", null));
    }

    public List<BaseSkill> AvailableSkills {
        get { return availableSkills; }
    }

}
