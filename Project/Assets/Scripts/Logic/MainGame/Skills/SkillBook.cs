using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    public enum Skill {
        None,
        FireBall,
        WaterGun,
        MudShot,
        Test1,
        Test2,
        Test3,
        Test4,
        Test5
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

    private static SkillBook instance = new SkillBook();
    public static SkillBook Instance {
        get { return SkillBook.instance; }
    }

    private SkillBook() {        
        BaseSkill tempSkill = null;

        allSkills = new Dictionary<int, SkillBookSkill>();
        //0
        AddSkill(new TargetedSkill((int)Skill.None, "Unknown", "Skill does not exist",
                                   0f, string.Empty, string.Empty, string.Empty, null), "");
        //1
        tempSkill = new TargetedSkill((int)Skill.FireBall, "Fire ball", "skill 1 description",
                                      2f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireFlame"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(151));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(152));
        AddSkill(tempSkill, "137");
        //2
        tempSkill = new TargetedSkill((int)Skill.WaterGun, "Water gun", "skill 2 description",
                                      3f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(202));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(053));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(054));
        AddSkill(tempSkill, "140");
        //3
        tempSkill = new TargetedSkill((int)Skill.MudShot, "Mud shot", "skill 3 description",
                                      4f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudShot"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(201));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(051));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(052));
        AddSkill(tempSkill, "136");

        #region Testing skill helpers
        tempSkill = new BaseSkill((int)Skill.Test3, "DamageMe", "DamageMe description",
                                   1f, string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill((int)Skill.Test4, "StunMe", "StunMe description",
                           1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(201));
        AddSkill(tempSkill, "136");

        tempSkill = new BaseSkill((int)Skill.Test5, "Heal", "Heal description",
                          5f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(053));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(004));
        AddSkill(tempSkill, "153");
        #endregion
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
        return allSkills[_id].Skill;
    }
    public string GetIcon(int _id) {
        return allSkills[_id].Icon;
    }
    public bool IsSkillAvailable(int _id) {
        return allSkills[_id].IsAvailable;
    }
    #endregion
}