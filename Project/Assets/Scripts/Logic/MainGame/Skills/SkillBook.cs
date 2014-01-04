using UnityEngine;
using System.Collections.Generic;

public class SkillBook{

    private const string DESTINATION_CURSOR = "DestinationTargetCursor",
                         DIRECTION_CURSOR = "DirectionTargetCursor";

    public enum Skill {
        None,
        #region projectiles
        FireBall,        
        WaterBall,
        MudBall,
        IceBall,        
        LeafBall,
        LoveBall,
        RevengeBall,
        SunBall,
        DarkBall,
        LavaBall,
        SnowBall,
        RockBall,
        PoisonBall,        
        BlueBall,
        #endregion
        #region traps
        FireTrap,
        IceTrap,
        LavaTrap,
        PoisonTrap,
        #endregion
        #region directly casted
        BlowUp,
        LastStand,
        Resurrection,
        HawkEyes,
        HermesSandals,
        DragonSkin,
        HolyGround,
        HealingPrayer
        #endregion
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

        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(0));
        //tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(0));
        //tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(0));

        #region None ( 0 )
        AddSkill(new BaseSkill((int)Skill.None, "Unknown", "Skill does not exist, what the fuck did you do ?", 0f, 0f,
                               string.Empty, null, string.Empty), "");
        #endregion
        #region projectiles ( 1 - 14 )
        tempSkill = new TargetedSkill((int)Skill.FireBall, "Flame", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(0));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(0));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(0));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.WaterBall, "Water Shot", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(0));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(0));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(0));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.MudBall, "Mud Shot", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.IceBall, "Ice ball", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("IceBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LeafBall, "Leaf Gun", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LeafBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LoveBall, "Love Shot", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LoveBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.RevengeBall, "Revenge Ball", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("RevengeBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.SunBall, "Sun Burst", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("SunBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.DarkBall, "Dark Passenger", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("DarkBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LavaBall, "Fire Meteorite", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LavaBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.SnowBall, "Snow Ball", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("SnowBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.RockBall, "Rock Blast", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("RockBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.PoisonBall, "Poison Shot", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("PoisonBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.BlueBall, "Blue Flame", "", 2f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("BlueBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");
        #endregion
        #region traps ( 15 - 18 )
        tempSkill = new BaseSkill((int)Skill.FireTrap, "Fire Trap", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.IceTrap, "Icy Terrain", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.LavaTrap, "Lava Trap", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.PoisonTrap, "Poison Puddle", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");
        #endregion
        #region directly casted ( 19 - 26)
        tempSkill = new BaseSkill((int)Skill.BlowUp, "Art is a Bang", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.LastStand, "Last Stand", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.Resurrection, "Resurrection", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.HawkEyes, "Hawk Eyes", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.HermesSandals, "Hermes Sandals", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.DragonSkin, "Dragon Skin", "", 0f, 0f,
                                  string.Empty, null, string.Empty);
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.HolyGround, "Holy Ground", "", 2f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.HealingPrayer, "Healing Prayer", "", 2f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        AddSkill(tempSkill, "");
        #endregion        

        //tempSkill = new TargetedSkill((int)Skill.FireBall, "Fire ball", "skill 1 description",
        //                              2f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireFlame"), string.Empty,
        //                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));

        ////1
        //tempSkill = new TargetedSkill((int)Skill.FireBall, "Fire ball", "skill 1 description",
        //                              2f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireFlame"), string.Empty,
        //                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(151));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(152));
        //AddSkill(tempSkill, "137");
        ////2
        //tempSkill = new TargetedSkill((int)Skill.WaterBall, "Water gun", "skill 2 description",
        //                              3f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
        //                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(202));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(053));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(054));
        //AddSkill(tempSkill, "140");
        ////3
        //tempSkill = new TargetedSkill((int)Skill.MudBall, "Mud shot", "skill 3 description",
        //                              4f, string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudShot"), string.Empty,
        //                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(201));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(051));
        ////tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(052));
        //AddSkill(tempSkill, "136");
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