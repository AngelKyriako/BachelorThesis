using System.Collections.Generic;

public class SkillBook{

    private List<BaseSkill> availableSkills;

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {
        availableSkills = new List<BaseSkill>();
        availableSkills.Add(new BaseSkill("skill 1", "skill 1 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 2", "skill 2 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 3", "skill 3 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 4", "skill 4 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 5", "skill 5 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 6", "skill 6 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 7", "skill 7 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 8", "skill 8 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 9", "skill 9 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 10", "skill 10 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 11", "skill 11 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 12", "skill 12 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 13", "skill 13 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 14", "skill 14 description", null, 0));
        availableSkills.Add(new BaseSkill("skill 15", "skill 15 description", null, 0));
    }

    public List<BaseSkill> AvailableSkills {
        get { return availableSkills; }
    }

}
