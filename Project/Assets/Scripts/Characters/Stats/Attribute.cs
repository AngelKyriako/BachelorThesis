using System.Collections.Generic;

public enum AttributeType {
    Attack,
    Defence,
    MovementSpeed,
    AttackSpeed,
    Luck,
    Regeneration
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
    private int statBonusValue;

    public Attribute(): base() {
        modifiers = new List<ModifyingStat>();
        statBonusValue = 0;
    }
    public Attribute(string _name, string _desc, int _val): base(_name, _desc, _val) {
        modifiers = new List<ModifyingStat>();
        statBonusValue = 0;
    }

    public void AddModifier(ModifyingStat mod) {
        modifiers.Add(mod);
    }

    public virtual void UpdateAttribute() {
        statBonusValue = 0;
        foreach (ModifyingStat mod in modifiers)
            statBonusValue += (int)(mod.stat.FinalValue * mod.ratio);
    }

    public override int FinalValue {
        get { return base.FinalValue + statBonusValue; }
    }
}