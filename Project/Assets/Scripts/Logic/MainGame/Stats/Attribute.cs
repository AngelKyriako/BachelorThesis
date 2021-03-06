﻿using System.Collections.Generic;

public enum AttributeType {
    Damage,
    MagicDamage,
    Defence,
    MagicDefence,
    HealthRegen,
    ManaRegen,
    MovementSpeed,
    AttackSpeed,
    Critical,
    Evasion,    
    VisionRadius,
    Leadership
}

public struct ModifyingStat {
    public Stat stat;
    public float ratio;

    public ModifyingStat(Stat st, float rt) {
        stat = st;
        ratio = rt;
    }
}

public class Attribute: BaseStat {

    private List<ModifyingStat> modifiers;
    private float statBonusValue;

    public Attribute(string _name, string _desc, float _val)
        : base(_name, _desc, _val) {
        modifiers = new List<ModifyingStat>();
        statBonusValue = 0;
    }

    public void AddModifier(ModifyingStat mod) {
        modifiers.Add(mod);
    }

    public virtual void UpdateAttribute() {
        statBonusValue = 0;
        foreach (ModifyingStat mod in modifiers)
            statBonusValue += mod.stat.FinalValue * mod.ratio;
    }

    public override float FinalValue {
        get { return base.FinalValue + statBonusValue; }
    }

    public override string DisplayFinalValue {
        get { return System.Math.Round(FinalValue, 1).ToString(); }
    }
}