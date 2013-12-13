public enum VitalType {
    Health,
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

    public float CurrentValue {
        get { return currentValue; }
        set { currentValue = (value < 0) ? 0 : ((value > base.FinalValue) ? base.FinalValue : value); }
    }

    public override string DisplayFinalValue {
        get { return Utilities.Instance.VitalDisplay(FinalValue); }
    }

    public string DisplayCurrentValue {
        get { return Utilities.Instance.VitalDisplay(currentValue); }
    }

    public override string ToString() {
        return "(" + DisplayCurrentValue + "/" + DisplayFinalValue + ")";
    }
}