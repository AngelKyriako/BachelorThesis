using System.Collections.Generic;

public enum AttributeType {
    Attack,
    Defence,
    MovementSpeed,
    AttackSpeed,
    CriticalChance,
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

public class Attribute: BaseStatistic {

    private List<ModifyingStat> modifiers;
    private int statBonus;

    public Attribute(): base() {
        modifiers = new List<ModifyingStat>();
        statBonus = 0;
    }
    public Attribute(string _name, string _desc, int _val): base(_name, _desc, _val) {
        modifiers = new List<ModifyingStat>();
        statBonus = 0;
    }

    public void AddModifier(ModifyingStat mod) {
        modifiers.Add(mod);
    }

    public void UpdateAttribute() {
        UpdateStatBonus();
    }

    public override int FinalValue {
        get { return base.FinalValue + statBonus; }
    }

    private void UpdateStatBonus() {
        statBonus = 0;
        foreach (ModifyingStat mod in modifiers)
            statBonus += (int)(mod.stat.FinalValue * mod.ratio);
    }
}