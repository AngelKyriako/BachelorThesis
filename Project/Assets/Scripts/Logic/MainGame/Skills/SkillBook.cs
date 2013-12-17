using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    public enum Skill {
        None,
        FireBall,
        WaterGun,
        MudShot
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
        //1
        tempSkill = new TargetedSkill((int)Skill.FireBall, "Fire ball", "skill 1 description",
                                      2f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect((int)Effect.Damage));
        AddSkill(tempSkill, "130");
        //2
        tempSkill = new TargetedSkill((int)Skill.WaterGun, "Water gun", "skill 2 description",
                                      4f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect((int)Effect.Slow));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(7));
        AddSkill(tempSkill, "131");
        //3
        tempSkill = new TargetedSkill((int)Skill.MudShot, "Mud shot", "skill 3 description",
                                      5f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect((int)Effect.Damage));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect((int)Effect.Immobilize));
        AddSkill(tempSkill, "132");

        #region Testing skill helpers
        tempSkill = new TargetedSkill(99, "Fuck up Ball", "skill 3 description",
                                      1f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath("TestTargetCursor")));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(99));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(98));
        AddSkill(tempSkill, "133");

        tempSkill = new BaseSkill(98, "KillMe", "KillMe description",
                                   1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(99));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(98));
        AddSkill(tempSkill, "134");

        tempSkill = new BaseSkill(97, "DamageMe", "DamageMe description",
                                   1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect((int)Effect.Damage));
        AddSkill(tempSkill, "135");

        tempSkill = new BaseSkill(96, "StunMe", "DamageMe description",
                           1f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect((int)Effect.Stun));
        AddSkill(tempSkill, "136");

        tempSkill = new BaseSkill(95, "Heal", "Heal description",
                          5f, string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(5));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(6));
        AddSkill(tempSkill, "137");
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
    public void SetSkillAvailable(BaseSkill _skill, bool _isAvailable) {
        allSkills[_skill.Id] = new SkillBookSkill(GetSkill(_skill.Id), GetIcon(_skill.Id), _isAvailable);
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
