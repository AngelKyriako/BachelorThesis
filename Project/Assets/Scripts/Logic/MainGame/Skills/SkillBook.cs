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
        LavaBall,
        SnowBall,
        RockBall,
        PoisonBall,                
        LeafBall,
        BlueBall,
        LoveBall,
        RevengeBall,
        SunBall,
        DarkBall,
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

        #region None ( 0 )
        AddSkill(new BaseSkill((int)Skill.None, "Unknown", "Skill does not exist, what the fuck did you do ?", 0f, 0f,
                               string.Empty, null, string.Empty), "");
        #endregion
        #region projectiles ( 1 - 14 )
        tempSkill = new TargetedSkill((int)Skill.FireBall, "Flame", "", 5f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        AddSkill(tempSkill, "137");

        tempSkill = new TargetedSkill((int)Skill.WaterBall, "Water Shot", "", 5f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("WaterBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(054));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(057));
        AddSkill(tempSkill, "140");

        tempSkill = new TargetedSkill((int)Skill.MudBall, "Mud Shot", "", 10f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("MudBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(253));
        AddSkill(tempSkill, "136");

        tempSkill = new TargetedSkill((int)Skill.IceBall, "Ice ball", "", 12f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("IceBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(402));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LavaBall, "Fire Meteorite", "", 17f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LavaBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(001));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.SnowBall, "Snow Ball", "", 15f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("SnowBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(054));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(057));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(060));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(251));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.RockBall, "Rock Blast", "", 16f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("RockBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.PoisonBall, "Poison Shot", "", 14f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("PoisonBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(005));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(006));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(008));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(009));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(011));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(012));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LeafBall, "Leaf Gun", "", 9f, 20f,
                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("LeafBall"), string.Empty,
                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(053));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(254));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(437));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(201));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.BlueBall, "Blue Flame", "", 39f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("BlueBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(053));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(058));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(059));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(060));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(061));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(062));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(255));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.LoveBall, "Love Shot", "", 8f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("LoveBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(411));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(412));
        AddSkill(tempSkill, "");

        //tempSkill = new TargetedSkill((int)Skill.RevengeBall, "Revenge Ball", "", 22f, 20f,
        //                              string.Empty, ResourcesPathManager.Instance.ProjectilePath("RevengeBall"), string.Empty,
        //                              (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(0)); //@TODO: Need to add new Effect
        //AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.SunBall, "Sun Burst", "", 48f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("SunBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(101));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(102));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(204));
        AddSkill(tempSkill, "");

        tempSkill = new TargetedSkill((int)Skill.DarkBall, "Darth Bolt", "", 44f, 20f,
                                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("DarkBall"), string.Empty,
                                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));        
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(412));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(413));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(252));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(257));
        AddSkill(tempSkill, "");
        #endregion
        #region traps ( 15 - 18 )
        tempSkill = new BaseSkill((int)Skill.FireTrap, "Fire Trap", "", 25f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(005));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.IceTrap, "Icy Terrain", "", 38f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(051));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(052));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(401));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(402));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(403));
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.LavaTrap, "Lava Trap", "", 35f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddSupportEffect(EffectBook.Instance.GetEffect(004));
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.PoisonTrap, "Poison Sludge", "", 41f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(004));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(005));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(006));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(007));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(008));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(009));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(010));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(011));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(012));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(012));
        AddSkill(tempSkill, "");
        #endregion
        #region directly casted ( 19 - 26)
        tempSkill = new BaseSkill((int)Skill.BlowUp, "Big Bang", "", 80f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(001));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(002));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddOffensiveEffect(EffectBook.Instance.GetEffect(003));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(013));
        AddSkill(tempSkill, "");

        //tempSkill = new BaseSkill((int)Skill.LastStand, "Last Stand", "", 0f, 0f,
        //                          string.Empty, null, string.Empty);
        //AddSkill(tempSkill, "");

        //tempSkill = new BaseSkill((int)Skill.Resurrection, "Resurrection", "", 0f, 0f,
        //                          string.Empty, null, string.Empty);
        //AddSkill(tempSkill, "");

        //tempSkill = new BaseSkill((int)Skill.HawkEyes, "Hawk Eyes", "", 0f, 0f,
        //                          string.Empty, null, string.Empty);
        //AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.HermesSandals, "Hermes Sandals", "", 25f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(201));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(205));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(206));
        AddSkill(tempSkill, "");

        tempSkill = new BaseSkill((int)Skill.DragonSkin, "Dragon's Skin", "", 40f, 0f,
                                  string.Empty, null, string.Empty);
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(207));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(208));
        tempSkill.AddPassiveEffect(EffectBook.Instance.GetEffect(209));
        AddSkill(tempSkill, "");

        //tempSkill = new TargetedSkill((int)Skill.HolyGround, "Holy Ground", "", 2f, 20f,
        //                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
        //                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //AddSkill(tempSkill, "");

        //tempSkill = new TargetedSkill((int)Skill.HealingPrayer, "Healing Prayer", "", 2f, 20f,
        //                      string.Empty, ResourcesPathManager.Instance.ProjectilePath("FireBall"), string.Empty,
        //                      (GameObject)Resources.Load(ResourcesPathManager.Instance.TargetCursorPath(DIRECTION_CURSOR)));
        //AddSkill(tempSkill, "");
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