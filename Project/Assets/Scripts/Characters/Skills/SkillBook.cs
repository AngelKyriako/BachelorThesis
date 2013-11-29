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
        tempSkill = new BaseSkill("skill 1", "skill 1 description", null, 2f, string.Empty,
                                  ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Damage"));
        AddSkill(tempSkill);
        //2
        tempSkill = new TargetedSkill("skill 2", "skill 2 description", null, 5f, string.Empty,
                                      ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Mana burn"));
        AddSkill(tempSkill);
        //3
        tempSkill = new BaseSkill("skill 3", "skill 3 description", null, 8f, string.Empty,
                                  ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty);        
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Slow"));
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
