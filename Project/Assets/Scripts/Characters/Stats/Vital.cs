public enum VitalType {
    Health,
    Mana
}

public class Vital: Attribute {

    private int currentValue;

    public Vital(): base() {
        currentValue = base.FinalValue;
    }
    public Vital(string _name, string _desc, int _val): base(_name, _desc, _val) {
        currentValue = base.FinalValue;
    }

    public override void UpdateAttribute() {
        int previousFinalValue = FinalValue;
        base.UpdateAttribute();
        currentValue += FinalValue - previousFinalValue;
    }

    public int CurrentValue {
        get { return currentValue; }
        set { currentValue = (value < 0)?0:((value > base.FinalValue)?base.FinalValue:value); }
    }
}