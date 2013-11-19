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
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f, null, null, null, null);//(GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath));
        //tempSkill.AddEffect(tempEffect);
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
