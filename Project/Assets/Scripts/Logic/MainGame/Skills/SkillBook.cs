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
        tempSkill = new TargetedSkill("Fire ball", "skill 1 description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("14")),
                                      2f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Damage"));
        AddSkill(tempSkill);
        //2
        tempSkill = new TargetedSkill("Water gun", "skill 2 description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("15")),
                                      4f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Slow"));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Mana burn"));
        AddSkill(tempSkill);
        //3
        tempSkill = new TargetedSkill("Mud shot", "skill 3 description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("52")),
                                      5f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Damage"));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("Immobilize"));
        AddSkill(tempSkill);
        //4
        tempSkill = new BaseSkill("Heal", "Heal description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("6")),
                                  5f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("Health Heal"));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("Mana Heal"));
        AddSkill(tempSkill);

        #region Testing skill helpers
        tempSkill = new TargetedSkill("Fuck up Ball", "skill 3 description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("45")),
                                      1f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("OneHitKO"));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect("20Immobilize"));
        AddSkill(tempSkill);

        tempSkill = new BaseSkill("KillMe", "KillMe description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("107")),
                                   1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("OneHitKO"));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("20Immobilize"));
        AddSkill(tempSkill);

        tempSkill = new BaseSkill("DamageMe", "DamageMe description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("87")),
                                   1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("Damage"));
        AddSkill(tempSkill);

        tempSkill = new BaseSkill("StunMe", "DamageMe description", (Texture2D)Resources.Load(ResourcesPathManager.Instance.SkillIcon48x48("80")),
                           1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect("Stun"));
        AddSkill(tempSkill);
        #endregion
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
