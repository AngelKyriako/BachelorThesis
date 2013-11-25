using UnityEngine;
using System.Collections.Generic;

public struct SkillBookSkill {
    public BaseSkill Skill;
    public bool IsAvailable;
    public SkillBookSkill(BaseSkill _skill, bool _isAvailable) {
        Skill = _skill;
        IsAvailable = _isAvailable;
    }
}

public class SkillBook{

    private Dictionary<string, SkillBookSkill> allSkills;

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {        
        BaseSkill tempSkill = null;

        allSkills = new Dictionary<string, SkillBookSkill>();
        //1
        tempSkill = new BaseSpell("skill 1", "skill 1 description", null, 2f,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")), null, null,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath("FireBall")));
        tempSkill.AddEffect(EffectBook.Instance.GetEffect("Damage"));
            AddSkill(tempSkill);
        //2
        tempSkill = new BaseSpell("skill 2", "skill 2 description", null, 5f,
                          (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")), null, null,
                          (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath("WaterBall")));
        tempSkill.AddEffect(EffectBook.Instance.GetEffect("Mana burn"));
        AddSkill(tempSkill);
        //3
        tempSkill = new BaseSpell("skill 3", "skill 3 description", null, 8f,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")), null, null,
                                  (GameObject)Resources.Load(ResourcesPathManager.Instance.ProjectilePath("MudBall")));
        tempSkill.AddEffect(EffectBook.Instance.GetEffect("Slow"));
        AddSkill(tempSkill);
        //4
        tempSkill = new BaseSpell("skill 4", "skill 4 description", null, 2f, null, null, null, null);
        tempSkill.AddEffect(EffectBook.Instance.GetEffect("Health Heal"));
        AddSkill(tempSkill);
    }

    #region Accesors
    public ICollection<string> AllSkillsKeys {
        get { return allSkills.Keys; }
    }
    public void AddSkill(BaseSkill _skill) {
        allSkills.Add(_skill.Title, new SkillBookSkill(_skill, true));
    }
    public void RemoveSkill(BaseSkill _skill) {
        allSkills.Remove(_skill.Title);
    }
    public void SetSkillAvailable(BaseSkill _skill, bool _isAvailable) {
        allSkills[_skill.Title] = new SkillBookSkill(allSkills[_skill.Title].Skill, _isAvailable);
    }
    public BaseSkill GetSkill(string _title) {
        return allSkills[_title].Skill;
    }
    public bool IsSkillAvailable(string _title) {
        return allSkills[_title].IsAvailable;
    }
#endregion
}
