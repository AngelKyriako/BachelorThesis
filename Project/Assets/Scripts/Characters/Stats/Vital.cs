public enum VitalType {
    HitPoints,
    Mana
}

public class Vital: Attribute {

    private float currentValue;

    public Vital(string _name, string _desc, float _val): base(_name, _desc, _val) {
        currentValue = base.FinalValue;
    }

    public override void UpdateAttribute() {
        float previousFinalValue = FinalValue;
        base.UpdateAttribute();
        currentValue += FinalValue - previousFinalValue;
    }

    public int CurrentValue {
        get { return (int)currentValue; }
        set { currentValue = (int)((value < 0)?0:((value > base.FinalValue)?base.FinalValue:value)); }
    }

    public override float FinalValue {
        get { return (int)base.FinalValue; }
    }
}